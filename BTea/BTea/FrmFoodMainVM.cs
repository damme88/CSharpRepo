using System;
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
            ImgPathCmd = new RelayCommand(new Action<object>(DoImgPathCmd));
        }

        #region MEMBERS
        public RelayCommand CmdAddFood { set; get; }
        public RelayCommand CmdEditFood { set; get; }
        public RelayCommand CmdDeleteFood { set; get; }

        public RelayCommand ImgPathCmd { set; get; }

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
            string strQa = "Dữ liệu sẽ được xóa trong Database.";
            strQa += "\nBạn có chắc chắn muốn xóa sản phẩm này ?";

            MessageBoxResult msg = TConst.MsgYNQ(strQa);
            if (msg == MessageBoxResult.No)
            {
                return;
            }

            FoodItem fItem = _selecteItem;
            if (fItem != null)
            {
                string strId = fItem.Id;
                string subId = strId.Replace(CodeFood, "");
                int nId = TConst.ConvertInt(subId);

                if (nId > 0)
                {
                    bool bRet = DBConnection.GetInstance().DeleteFoodItem(nId);
                    if (bRet == true)
                    {
                        GetDataFoodFromDB();
                        OnPropertyChange("FoodItems");
                        OnPropertyChange("FoodCount");
                        OnPropertyChange("SelectedItemFood");
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
                FoodItem fItem = new FoodItem();
                FoodObject fObject = data_list[i];
                fItem.Id = CodeFood + fObject.BId;
                fItem.Name = fObject.BName;
                fItem.Price = fObject.BPrice.ToString(TConst.K_MONEY_FORMAT);
                fItem.Note = fObject.BNote;

                _foodItem.Add(fItem);
            }

            _foodCount = _foodItem.Count;
            if (_foodCount > 0)
            {
                _selecteItem = _foodItem[0];
            }
        }
        #endregion
    }
}
