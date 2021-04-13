using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TApp.Base;

namespace BTea
{
    public class FoodItem
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Price { get; set; }

        public string Note { get; set; }
    }
    class FrmFoodMainVM : TBaseVM
    {
        public FrmFoodMainVM()
        {
            CodeFood = "F";
            GetDataFoodFromDB();

            CmdAddFood = new RelayCommand(new Action<object>(DoAddFood));
            CmdEditFood = new RelayCommand(new Action<object>(DoEditFood));
            CmdDeleteFood = new RelayCommand(new Action<object>(DoDeleteFood));

            _frmFoodVM = new FrmFoodVM(FoodAddEditDoneCmd);
        }

        #region MEMBERS
        public RelayCommand CmdAddFood { set; get; }
        public RelayCommand CmdEditFood { set; get; }
        public RelayCommand CmdDeleteFood { set; get; }

        private FrmFoodVM _frmFoodVM;
        private string CodeFood;
        private FrmFood frmFood;
        private int _foodCount;
        #endregion

        #region Properties

        public int FoodCount
        {
            get { return _foodCount; }
            set
            {
                _foodCount = value;
                OnPropertyChange("FoodCount");
            }
        }
        private FoodItem _selecteItem;
        public FoodItem SelectedItemFood
        {
            get { return _selecteItem; }
            set
            {
                _selecteItem = value;
                OnPropertyChange("SelectedItemFood");
            }
        }

        private ObservableCollection<FoodItem> _foodItem;
        public ObservableCollection<FoodItem> FoodItems
        {
            get { return _foodItem; }
            set
            {
                _foodItem = value;
                OnPropertyChange("FoodItems");
            }
        }
        #endregion

        #region Method
        public void DoAddFood(object sender)
        {
            if (_frmFoodVM != null)
            {
                _frmFoodVM.FoodStateAdd = System.Windows.Visibility.Visible;
                _frmFoodVM.FoodStateEdit = System.Windows.Visibility.Collapsed;

                _frmFoodVM.SetData(string.Empty, string.Empty, string.Empty, string.Empty);
                frmFood = new FrmFood();
                frmFood.DataContext = _frmFoodVM;
                if (frmFood.ShowDialog() == true)
                {
                    frmFood.Close();
                    GetDataFoodFromDB();
                    OnPropertyChange("FoodItems");
                    OnPropertyChange("FoodCount");
                }
            }
        }
        public void DoEditFood(object sender)
        {
            FoodItem fItem = _selecteItem;
            if (fItem != null && _frmFoodVM != null)
            {
                frmFood = new FrmFood();
                frmFood.DataContext = _frmFoodVM;

                _frmFoodVM.FoodStateAdd = System.Windows.Visibility.Collapsed;
                _frmFoodVM.FoodStateEdit = System.Windows.Visibility.Visible;

                _frmFoodVM.SetData(fItem.Id, fItem.Name, fItem.Price, fItem.Note);

                if (frmFood.ShowDialog() == true)
                {
                    frmFood.Close();
                    GetDataFoodFromDB();
                    OnPropertyChange("FoodItems");
                }
            }
        }

        public void DoDeleteFood(object sender)
        {
            FoodItem tpItem = _selecteItem;
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
                    bool bRet = DBConnection.GetInstance().DeleteFoodItem(nId);
                    if (bRet == true)
                    {
                        GetDataFoodFromDB();
                        OnPropertyChange("FoodItems");
                        OnPropertyChange("FoodCount");
                    }
                }
            }
        }
        public void FoodAddEditDoneCmd()
        {
            if (frmFood != null)
            {
                frmFood.DialogResult = true;
            }
        }
        public void GetDataFoodFromDB()
        {
            List<FoodObject> data_list = DBConnection.GetInstance().GetDataFood();
            _foodItem = new ObservableCollection<FoodItem>();
            for (int i = 0; i < data_list.Count; ++i)
            {
                FoodItem tpItem = new FoodItem();
                FoodObject tpObject = data_list[i];
                tpItem.Id = CodeFood + tpObject.BId;
                tpItem.Name = tpObject.BName;
                tpItem.Price = tpObject.BPrice.ToString("#,##0.00;(#,##0.00)");
                tpItem.Note = tpObject.BNote;

                _foodItem.Add(tpItem);
            }

            _foodCount = data_list.Count;
        }
        #endregion
    }
}
