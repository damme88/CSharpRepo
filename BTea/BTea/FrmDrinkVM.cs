using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TApp.Base;

namespace BTea
{
    class FrmDrinkVM : TBaseVM
    {
        public FrmDrinkVM()
        {
            CmdDrinkAdd = new RelayCommand(new Action<object>(DrinkAdd));
            _drinkName = "";
            _drinkPrice = "";
            _drinkNote = "";
            _drId = "";
        }

        public FrmDrinkVM(Action parentAction)
        {
            CmdDrinkAdd = new RelayCommand(new Action<object>(DrinkAdd));
            CmdDrinkEdit = new RelayCommand(new Action<object>(DrinkEdit));
            _pDrinkMethod = parentAction;
            _drinkName = "";
            _drinkPrice = "";
            _drinkNote = "";
            _drId = "";
        }
        #region MEMBER
        public RelayCommand CmdDrinkAdd { set; get; }
        public RelayCommand CmdDrinkEdit { set; get; }
        private readonly Action _pDrinkMethod;
        private string _drId;
        private string _drinkName;
        private string _drinkPrice;
        private string _drinkNote;
        private Visibility _drinkStateAdd;
        private Visibility _drinkStateEdit;
        #endregion

        #region PROPERTY

        public Visibility DrinkStateAdd
        {
            get { return _drinkStateAdd; }
            set
            {
                _drinkStateAdd = value;
            }
        }

        public Visibility DrinkStateEdit
        {
            get { return _drinkStateEdit; }
            set
            {
                _drinkStateEdit = value;
            }
        }

        public string DrinkId
        {
            get { return _drId; }
        }

        public string DrinkName
        {
            get { return _drinkName; }
            set
            {
                _drinkName = value;
                OnPropertyChange("DrinkName");
            }
        }

        public string DrinkPrice
        {
            get { return _drinkPrice; }
            set
            {
                _drinkPrice = value;
                OnPropertyChange("DrinkPrice");
            }
        }

        public string DrinkNote
        {
            get { return _drinkNote; }
            set
            {
                _drinkNote = value;
                OnPropertyChange("DrinkNote");
            }
        }

        #endregion


        #region METHOD
        public void SetData(string Id, string Name, string Price, string Note)
        {
            _drId = Id;
            _drinkName = Name;
            _drinkPrice = Price;
            _drinkNote = Note; 
        }

        public void DrinkEdit(object sender)
        {
            if (_drinkName != string.Empty &&
                _drinkPrice != string.Empty)
            {
                DrinkObject drinkItem = new DrinkObject();

                string drId = _drId.Replace("DR", "");
                drinkItem.BId = drId;
                drinkItem.BName = _drinkName;
                drinkItem.BPrice = Convert.ToDouble(_drinkPrice);
                drinkItem.BNote = _drinkNote;
                bool bRet = DBConnection.GetInstance().EditDrinkItem(drinkItem);
                _pDrinkMethod.Invoke();
            }
        }
        public void DrinkAdd(object sender)
        {
            if (_drinkName != string.Empty &&
                _drinkPrice != string.Empty)
            {
                DrinkObject drinkItem = new DrinkObject();
                drinkItem.BName = _drinkName;
                drinkItem.BPrice = Convert.ToDouble(_drinkPrice);
                drinkItem.BNote = _drinkNote;
                bool bRet = DBConnection.GetInstance().AddDrinkItem(drinkItem);
                _pDrinkMethod.Invoke();
            }
            else
            {
                MessageBox.Show("Thông tin chưa đủ. \nTối thiểu cần tên sản phẩm và giá", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion

    }
}
