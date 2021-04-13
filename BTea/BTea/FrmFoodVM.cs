using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TApp.Base;

namespace BTea
{
    class FrmFoodVM : TBaseVM
    {
        public FrmFoodVM()
        {
            CmdFoodAdd = new RelayCommand(new Action<object>(FoodAdd));
            _foodName = "";
            _foodPrice = "";
            _foodNote = "";
            _foodId = "";
        }

        public FrmFoodVM(Action parentAction)
        {
            CmdFoodAdd = new RelayCommand(new Action<object>(FoodAdd));
            CmdFoodEdit = new RelayCommand(new Action<object>(FoodEdit));
            _pFoodMethod = parentAction;
            _foodName = "";
            _foodPrice = "";
            _foodNote = "";
            _foodId = "";
        }
        #region MEMBER
        public RelayCommand CmdFoodAdd { set; get; }
        public RelayCommand CmdFoodEdit { set; get; }
        private readonly Action _pFoodMethod;
        private string _foodId;
        private string _foodName;
        private string _foodPrice;
        private string _foodNote;
        private Visibility _foodStateAdd;
        private Visibility _foodStateEdit;
        #endregion

        #region PROPERTY

        public Visibility FoodStateAdd
        {
            get { return _foodStateAdd; }
            set
            {
                _foodStateAdd = value;
            }
        }

        public Visibility FoodStateEdit
        {
            get { return _foodStateEdit; }
            set
            {
                _foodStateEdit = value;
            }
        }

        public string FoodId
        {
            get { return _foodId; }
        }

        public string FoodName
        {
            get { return _foodName; }
            set
            {
                _foodName = value;
                OnPropertyChange("FoodName");
            }
        }

        public string FoodPrice
        {
            get { return _foodPrice; }
            set
            {
                _foodPrice = value;
                OnPropertyChange("FoodPrice");
            }
        }

        public string FoodNote
        {
            get { return _foodNote; }
            set
            {
                _foodNote = value;
                OnPropertyChange("FoodNote");
            }
        }

        #endregion


        #region METHOD
        public void SetData(string Id, string Name, string Price, string Note)
        {
            _foodId = Id;
            _foodName = Name;
            _foodPrice = Price;
            _foodNote = Note; 
        }

        public void FoodEdit(object sender)
        {
            if (_foodName != string.Empty &&
                _foodPrice != string.Empty)
            {
                FoodObject tpItem = new FoodObject();

                string tpId = _foodId.Replace("TP", "");
                tpItem.BId = tpId;
                tpItem.BName = _foodName;
                tpItem.BPrice = Convert.ToDouble(_foodPrice);
                tpItem.BNote = _foodNote;
                bool bRet = DBConnection.GetInstance().EditFoodItem(tpItem);
                _pFoodMethod.Invoke();
            }
        }
        public void FoodAdd(object sender)
        {
            if (_foodName != string.Empty &&
                _foodPrice != string.Empty)
            {
                FoodObject fItem = new FoodObject();
                fItem.BName = _foodName;
                fItem.BPrice = Convert.ToDouble(_foodPrice);
                fItem.BNote = _foodNote;
                bool bRet = DBConnection.GetInstance().AddFoodItem(fItem);
                _pFoodMethod.Invoke();
            }
            else
            {
                MessageBox.Show("Thông tin chưa đủ. \nTối thiểu cần tên sản phẩm và giá", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion

    }
}
