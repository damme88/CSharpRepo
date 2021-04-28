using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TApp.Base;

namespace BTea
{
    class FrmToppingVM : TBaseVM
    {
        public FrmToppingVM()
        {
            CmdToppingAdd = new RelayCommand(new Action<object>(ToppingAdd));
            _tpName = "";
            _tpPrice = 0;
            _tpNote = "";
            _tpId = "";
        }

        public FrmToppingVM(Action parentAction)
        {
            CmdToppingAdd = new RelayCommand(new Action<object>(ToppingAdd));
            CmdToppingEdit = new RelayCommand(new Action<object>(ToppingEdit));
            _pToppingMethod = parentAction;
            _tpName = "";
            _tpPrice = 0;
            _tpNote = "";
            _tpId = "";
        }
        #region MEMBER
        public RelayCommand CmdToppingAdd { set; get; }
        public RelayCommand CmdToppingEdit { set; get; }
        private readonly Action _pToppingMethod;
        private string _tpId;
        private string _tpName;
        private int _tpPrice;
        private string _tpNote;
        private Visibility _toppingStateAdd;
        private Visibility _toppingStateEdit;
        #endregion

        #region PROPERTY

        public Visibility ToppingStateAdd
        {
            get { return _toppingStateAdd; }
            set
            {
                _toppingStateAdd = value;
            }
        }

        public Visibility ToppingStateEdit
        {
            get { return _toppingStateEdit; }
            set
            {
                _toppingStateEdit = value;
            }
        }

        public string ToppingId
        {
            get { return _tpId; }
        }

        public string ToppingName
        {
            get { return _tpName; }
            set
            {
                _tpName = value;
                OnPropertyChange("ToppingName");
            }
        }

        public string ToppingPrice
        {
            get
            {
                return _tpPrice.ToString(TConst.K_MONEY_FORMAT);
            }
            set
            {
                int iVal = TConst.ConvertMoney(value);
                _tpPrice = iVal;
                OnPropertyChange("ToppingPrice");
            }
        }

        public string ToppingNote
        {
            get { return _tpNote; }
            set
            {
                _tpNote = value;
                OnPropertyChange("ToppingNote");
            }
        }

        #endregion


        #region METHOD
        public void SetData(string Id, string Name, string Price, string Note)
        {
            _tpId = Id;
            _tpName = Name;
            _tpPrice = TConst.ConvertMoney(Price);
            _tpNote = Note; 
        }

        public void ToppingEdit(object sender)
        {
            if (_tpName != string.Empty)
            {
                ToppingObject tpItem = new ToppingObject();

                string tpId = _tpId.Replace("TP", "");
                tpItem.BId = tpId;
                tpItem.BName = _tpName;
                tpItem.BPrice = _tpPrice;
                tpItem.BNote = _tpNote;
                bool bRet = DBConnection.GetInstance().EditToppingItem(tpItem);
                _pToppingMethod.Invoke();
            }
        }
        public void ToppingAdd(object sender)
        {
            if (_tpName != string.Empty)
            {
                ToppingObject tpItem = new ToppingObject();
                tpItem.BName = _tpName;
                tpItem.BPrice = _tpPrice;
                tpItem.BNote = _tpNote;
                bool bRet = DBConnection.GetInstance().AddToppingItem(tpItem);
                _pToppingMethod.Invoke();
            }
            else
            {
                string strInfo = "Thông báo";
                string strContent = "Tối thiểu cần tên sản phẩm.";
                MessageBox.Show(strContent, strInfo, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion

    }
}
