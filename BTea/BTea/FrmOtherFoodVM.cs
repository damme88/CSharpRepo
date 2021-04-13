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
            _tpName = "";
            _tpPrice = "";
            _tpNote = "";
            _tpId = "";
        }

        public FrmOtherFoodVM(Action parentAction)
        {
            CmdOtherFoodAdd = new RelayCommand(new Action<object>(OtherFoodAdd));
            CmdOtherFoodEdit = new RelayCommand(new Action<object>(OtherFoodEdit));
            _pOtherFoodMethod = parentAction;
            _tpName = "";
            _tpPrice = "";
            _tpNote = "";
            _tpId = "";
        }
        #region MEMBER
        public RelayCommand CmdOtherFoodAdd { set; get; }
        public RelayCommand CmdOtherFoodEdit { set; get; }
        private readonly Action _pOtherFoodMethod;
        private string _tpId;
        private string _tpName;
        private string _tpPrice;
        private string _tpNote;
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
            get { return _tpId; }
        }

        public string OtherFoodName
        {
            get { return _tpName; }
            set
            {
                _tpName = value;
                OnPropertyChange("OtherFoodName");
            }
        }

        public string OtherFoodPrice
        {
            get { return _tpPrice; }
            set
            {
                _tpPrice = value;
                OnPropertyChange("OtherFoodPrice");
            }
        }

        public string OtherFoodNote
        {
            get { return _tpNote; }
            set
            {
                _tpNote = value;
                OnPropertyChange("OtherFoodNote");
            }
        }

        #endregion


        #region METHOD
        public void SetData(string Id, string Name, string Price, string Note)
        {
            _tpId = Id;
            _tpName = Name;
            _tpPrice = Price;
            _tpNote = Note; 
        }

        public void OtherFoodEdit(object sender)
        {
            if (_tpName != string.Empty &&
                _tpPrice != string.Empty)
            {
                OtherFoodObject tpItem = new OtherFoodObject();

                string tpId = _tpId.Replace("TP", "");
                tpItem.BId = tpId;
                tpItem.BName = _tpName;
                tpItem.BPrice = Convert.ToDouble(_tpPrice);
                tpItem.BNote = _tpNote;
                bool bRet = false;// DBConnection.GetInstance().EditOtherFoodItem(tpItem);
                _pOtherFoodMethod.Invoke();
            }
        }
        public void OtherFoodAdd(object sender)
        {
            if (_tpName != string.Empty &&
                _tpPrice != string.Empty)
            {
                OtherFoodObject tpItem = new OtherFoodObject();
                tpItem.BName = _tpName;
                tpItem.BPrice = Convert.ToDouble(_tpPrice);
                tpItem.BNote = _tpNote;
                bool bRet = false;//= DBConnection.GetInstance().AddOtherFoodItem(tpItem);
                _pOtherFoodMethod.Invoke();
            }
            else
            {
                MessageBox.Show("Thông tin chưa đủ. \nTối thiểu cần tên sản phẩm và giá", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion

    }
}
