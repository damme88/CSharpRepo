﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TApp.Base;

namespace BTea
{
    public class OtherFoodItem
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Price { get; set; }

        public string Note { get; set; }
    }
    class FrmOtherFoodMainVM : TBaseVM
    {
        public FrmOtherFoodMainVM()
        {
            CodeOtherFood = "OF";
            GetDataOtherFoodFromDB();

            CmdAddOtherFood = new RelayCommand(new Action<object>(DoAddOtherFood));
            CmdEditOtherFood = new RelayCommand(new Action<object>(DoEditOtherFood));
            CmdDeleteOtherFood = new RelayCommand(new Action<object>(DoDeleteOtherFood));

            _frmOtherFoodVM = new FrmOtherFoodVM(OtherFoodAddEditDoneCmd);
        }

        #region MEMBERS
        public RelayCommand CmdAddOtherFood { set; get; }
        public RelayCommand CmdEditOtherFood { set; get; }
        public RelayCommand CmdDeleteOtherFood { set; get; }

        private FrmOtherFoodVM _frmOtherFoodVM;
        private string CodeOtherFood;
        private FrmOtherFood frmOtherFood;
        private int _OtherFoodCount;
        #endregion

        #region Properties

        public int OtherFoodCount
        {
            get { return _OtherFoodCount; }
            set
            {
                _OtherFoodCount = value;
                OnPropertyChange("OtherFoodCount");
            }
        }
        private OtherFoodItem _selecteItem;
        public OtherFoodItem SelectedItemOtherFood
        {
            get { return _selecteItem; }
            set
            {
                _selecteItem = value;
                OnPropertyChange("SelectedItemOtherFood");
            }
        }

        private ObservableCollection<OtherFoodItem> _OtherFoodItem;
        public ObservableCollection<OtherFoodItem> OtherFoodItems
        {
            get { return _OtherFoodItem; }
            set
            {
                _OtherFoodItem = value;
                OnPropertyChange("OtherFoodItems");
            }
        }
        #endregion

        #region Method
        public void DoAddOtherFood(object sender)
        {
            if (_frmOtherFoodVM != null)
            {
                _frmOtherFoodVM.OtherFoodStateAdd = System.Windows.Visibility.Visible;
                _frmOtherFoodVM.OtherFoodStateEdit = System.Windows.Visibility.Collapsed;

                _frmOtherFoodVM.SetData(string.Empty, string.Empty, string.Empty, string.Empty);
                frmOtherFood = new FrmOtherFood();
                frmOtherFood.DataContext = _frmOtherFoodVM;
                if (frmOtherFood.ShowDialog() == true)
                {
                    frmOtherFood.Close();
                    GetDataOtherFoodFromDB();
                    OnPropertyChange("OtherFoodItems");
                    OnPropertyChange("OtherFoodCount");
                }
            }
        }
        public void DoEditOtherFood(object sender)
        {
            OtherFoodItem otherItem = _selecteItem;
            if (otherItem != null && _frmOtherFoodVM != null)
            {
                frmOtherFood = new FrmOtherFood();
                frmOtherFood.DataContext = _frmOtherFoodVM;

                _frmOtherFoodVM.OtherFoodStateAdd = System.Windows.Visibility.Collapsed;
                _frmOtherFoodVM.OtherFoodStateEdit = System.Windows.Visibility.Visible;

                _frmOtherFoodVM.SetData(otherItem.Id, otherItem.Name, otherItem.Price, otherItem.Note);

                if (frmOtherFood.ShowDialog() == true)
                {
                    frmOtherFood.Close();
                    GetDataOtherFoodFromDB();
                    OnPropertyChange("OtherFoodItems");
                }
            }
        }

        public void DoDeleteOtherFood(object sender)
        {
            OtherFoodItem tpItem = _selecteItem;
            if (tpItem != null)
            {
                string strId = tpItem.Id;
                string subId = strId.Replace("TP", "");
                int nId = 0;
                try
                {
                    nId = Convert.ToInt32(subId);
                }
                catch (Exception ex)
                {
                    nId = -1;
                }

                if (nId > 0)
                {
                    bool bRet = DBConnection.GetInstance().DeleteOtherFoodItem(nId);
                    if (bRet == true)
                    {
                        GetDataOtherFoodFromDB();
                        OnPropertyChange("OtherFoodItems");
                        OnPropertyChange("OtherFoodCount");
                    }
                }
            }
        }
        public void OtherFoodAddEditDoneCmd()
        {
            if (frmOtherFood != null)
            {
                frmOtherFood.DialogResult = true;
            }
        }
        public void GetDataOtherFoodFromDB()
        {
            List<OtherFoodObject> data_list = DBConnection.GetInstance().GetDataOtherFood();
            _OtherFoodItem = new ObservableCollection<OtherFoodItem>();
            for (int i = 0; i < data_list.Count; ++i)
            {
                OtherFoodItem otherItem = new OtherFoodItem();
                OtherFoodObject otherFoodObject = data_list[i];
                otherItem.Id = CodeOtherFood + otherFoodObject.BId;
                otherItem.Name = otherFoodObject.BName;
                otherItem.Price = otherFoodObject.BPrice.ToString("#,##0.00;(#,##0.00)");
                otherItem.Note = otherFoodObject.BNote;

                _OtherFoodItem.Add(otherItem);
            }

            _OtherFoodCount = data_list.Count;
        }
        #endregion
    }
}
