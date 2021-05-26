using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TApp.Base;

namespace BTea
{
    class FrmOtherFoodVM : TBaseVM
    {
        public FrmOtherFoodVM()
        {
            CmdOtherFoodAdd = new RelayCommand(new Action<object>(OtherFoodAdd));
            _ofName = "";
            _ofPrice = 0;
            _ofNote = "";
            _ofId = "";
        }

        public FrmOtherFoodVM(Action parentAction)
        {
            CmdOtherFoodAdd = new RelayCommand(new Action<object>(OtherFoodAdd));
            CmdOtherFoodEdit = new RelayCommand(new Action<object>(OtherFoodEdit));
            _pOtherFoodMethod = parentAction;
            _ofName = "";
            _ofPrice = 0;
            _ofNote = "";
            _ofId = "";
        }
        #region MEMBER
        public RelayCommand CmdOtherFoodAdd { set; get; }
        public RelayCommand CmdOtherFoodEdit { set; get; }
        private readonly Action _pOtherFoodMethod;
        private string _ofId;
        private string _ofName;
        private int _ofPrice;
        private string _ofNote;
        private Visibility _OtherFoodStateAdd;
        private Visibility _OtherFoodStateEdit;
        #endregion

        #region PROPERTY

        public Visibility OtherFoodStateAdd
        {
            get { return _OtherFoodStateAdd; }
            set
            {
                _OtherFoodStateAdd = value;
            }
        }

        public Visibility OtherFoodStateEdit
        {
            get { return _OtherFoodStateEdit; }
            set
            {
                _OtherFoodStateEdit = value;
            }
        }

        public string OtherFoodId
        {
            get { return _ofId; }
        }

        public string OtherFoodName
        {
            get { return _ofName; }
            set
            {
                _ofName = value;
                OnPropertyChange("OtherFoodName");
            }
        }

        public string OtherFoodPrice
        {
            get {
                return _ofPrice.ToString(TConst.K_MONEY_FORMAT);
            }
            set
            {
                int iVal = TConst.ConvertMoney(value);
                _ofPrice = iVal;
                OnPropertyChange("OtherFoodPrice");
            }
        }

        public string OtherFoodNote
        {
            get { return _ofNote; }
            set
            {
                _ofNote = value;
                OnPropertyChange("OtherFoodNote");
            }
        }

        #endregion


        #region METHOD
        public void SetData(string Id, string Name, string Price, string Note)
        {
            _ofId = Id;
            _ofName = Name;
            _ofPrice = TConst.ConvertMoney(Price);
            _ofNote = Note; 
        }

        public void OtherFoodEdit(object sender)
        {
            if (_ofName != string.Empty)
            {
                OtherFoodObject ofItem = new OtherFoodObject();

                string ofId = _ofId.Replace("OF", "");
                ofItem.BId = ofId;
                ofItem.BName = _ofName;
                ofItem.BPrice = _ofPrice;
                ofItem.BNote = _ofNote;
                bool bRet = DBConnection.GetInstance().EditOtherFoodItem(ofItem);
                _pOtherFoodMethod.Invoke();
            }
        }
        public void OtherFoodAdd(object sender)
        {
            if (_ofName != string.Empty)
            {
                OtherFoodObject tpItem = new OtherFoodObject();
                tpItem.BName = _ofName;
                tpItem.BPrice = _ofPrice;
                tpItem.BNote = _ofNote;
                bool bRet = DBConnection.GetInstance().AddOtherFoodItem(tpItem);
                _pOtherFoodMethod.Invoke();
            }
            else
            {
                string strContent = "Tối thiểu cần tên sản phẩm.";
                TConst.MsgInfo(strContent);
            }
        }
        #endregion

    }
}
