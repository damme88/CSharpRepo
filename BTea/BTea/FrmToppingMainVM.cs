﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TApp.Base;

namespace BTea
{
    public class ToppingItem
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Price { get; set; }

        public string Note { get; set; }
    }
    class FrmToppingMainVM : TBaseVM
    {
        public FrmToppingMainVM()
        {
            CodeTopping = "TP";
            GetDataToppingFromDB();

            CmdAddTopping = new RelayCommand(new Action<object>(DoAddTopping));
            CmdEditTopping = new RelayCommand(new Action<object>(DoEditTopping));
            CmdDeleteTopping = new RelayCommand(new Action<object>(DoDeleteTopping));
            ImgPathCmd = new RelayCommand(new Action<object>(DoImgPathCmd));
            _frmToppingVM = new FrmToppingVM(ToppingAddEditDoneCmd);
        }

        #region MEMBERS
        public RelayCommand CmdAddTopping { set; get; }
        public RelayCommand CmdEditTopping { set; get; }
        public RelayCommand CmdDeleteTopping { set; get; }
        public RelayCommand ImgPathCmd { set; get; }

        private FrmToppingVM _frmToppingVM;
        private string CodeTopping;
        private FrmTopping frmTopping;
        private int _toppingCount;
        #endregion

        #region Properties

        public int ToppingCount
        {
            get { return _toppingCount; }
            set
            {
                _toppingCount = value;
                OnPropertyChange("ToppingCount");
            }
        }
        private ToppingItem _selecteItem;
        public ToppingItem SelectedItemTopping
        {
            get { return _selecteItem; }
            set
            {
                _selecteItem = value;
                OnPropertyChange("SelectedItemTopping");
            }
        }

        private ObservableCollection<ToppingItem> _toppingItem;
        public ObservableCollection<ToppingItem> ToppingItems
        {
            get { return _toppingItem; }
            set
            {
                _toppingItem = value;
                OnPropertyChange("ToppingItems");
            }
        }
        #endregion

        #region Method
        public void DoImgPathCmd(object sender)
        {
            string path = Environment.CurrentDirectory;
            path += "\\img_data\\";

            if (path != "")
            {
                if (Directory.Exists(path) == true)
                {
                    Process.Start("explorer.exe", path);
                    return;
                }
            }

            TConst.MsgError("Folder ảnh không tồn tại.");
        }

        public void DoAddTopping(object sender)
        {
            if (_frmToppingVM != null)
            {
                _frmToppingVM.ToppingStateAdd = System.Windows.Visibility.Visible;
                _frmToppingVM.ToppingStateEdit = System.Windows.Visibility.Collapsed;

                _frmToppingVM.SetData(string.Empty, string.Empty, string.Empty, string.Empty);
                frmTopping = new FrmTopping();
                frmTopping.DataContext = _frmToppingVM;
                if (frmTopping.ShowDialog() == true)
                {
                    frmTopping.Close();
                    GetDataToppingFromDB();
                    OnPropertyChange("ToppingItems");
                    OnPropertyChange("ToppingCount");
                }
            }
        }
        public void DoEditTopping(object sender)
        {
            ToppingItem tpItem = _selecteItem;
            if (tpItem != null && _frmToppingVM != null)
            {
                frmTopping = new FrmTopping();
                frmTopping.DataContext = _frmToppingVM;

                _frmToppingVM.ToppingStateAdd = System.Windows.Visibility.Collapsed;
                _frmToppingVM.ToppingStateEdit = System.Windows.Visibility.Visible;

                _frmToppingVM.SetData(tpItem.Id, tpItem.Name, tpItem.Price, tpItem.Note);

                if (frmTopping.ShowDialog() == true)
                {
                    frmTopping.Close();
                    GetDataToppingFromDB();
                    OnPropertyChange("ToppingItems");
                }
            }
        }

        public void DoDeleteTopping(object sender)
        {
            string strQa = "Dữ liệu sẽ được xóa trong Database.";
            strQa += "\nBạn có chắc chắn muốn xóa sản phẩm này ?";

            MessageBoxResult msg = TConst.MsgYNQ(strQa);
            if (msg == MessageBoxResult.No)
            {
                return;
            }

            ToppingItem tpItem = _selecteItem;
            if (tpItem != null)
            {
                string strId = tpItem.Id;
                string subId = strId.Replace("TP", "");
                int nId = TConst.ConvertInt(subId);

                if (nId > 0)
                {
                    bool bRet = DBConnection.GetInstance().DeleteToppingItem(nId);
                    if (bRet == true)
                    {
                        GetDataToppingFromDB();
                        OnPropertyChange("ToppingItems");
                        OnPropertyChange("ToppingCount");
                        OnPropertyChange("SelectedItemTopping");
                    }
                }
            }
        }
        public void ToppingAddEditDoneCmd()
        {
            if (frmTopping != null)
            {
                frmTopping.DialogResult = true;
            }
        }
        public void GetDataToppingFromDB()
        {
            List<ToppingObject> data_list = DBConnection.GetInstance().GetDataTopping();
            _toppingItem = new ObservableCollection<ToppingItem>();
            for (int i = 0; i < data_list.Count; ++i)
            {
                ToppingItem tpItem = new ToppingItem();
                ToppingObject tpObject = data_list[i];
                tpItem.Id = CodeTopping + tpObject.BId;
                tpItem.Name = tpObject.BName;
                tpItem.Price = tpObject.BPrice.ToString(TConst.K_MONEY_FORMAT);
                tpItem.Note = tpObject.BNote;

                _toppingItem.Add(tpItem);
            }

            _toppingCount = _toppingItem.Count;
            if (_toppingCount > 0)
            {
                _selecteItem = _toppingItem[0];
            }
        }
        #endregion
    }
}
