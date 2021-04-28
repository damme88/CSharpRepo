using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TApp.Base;

namespace BTea
{
    class BillItem
    {
        public BillItem()
        {
            BillDetailCmd = new RelayCommand(new Action<object>(BillDetailInfo));
            _orderItemList = new List<string>();
        }
        #region PROPERTY
        public string BillId {set; get;}
        public string BillName { set; get; }
        public string BillPrice { set; get; }
        public string BillCreator { set; get; }
        public string BillDate { set; get; }
        public string BillTableNumber { set; get; }
        public string BillPhone { set; get; }
        public string BillAddress { set; get; }
        public string BillNote { set; get; }

        public string BillKM { set; get; }
        public string BillKMType { set; get; }

        public List<string> OrderItemList
        {
            get { return _orderItemList; }
            set { _orderItemList = value; }
        }
        #endregion

        #region MEMBERS
        public RelayCommand BillDetailCmd { set; get; }
        private List<string> _orderItemList;
        #endregion

        #region METHOD
        public void AddItemOrder(string idItem)
        {
            if (_orderItemList != null)
            {
                _orderItemList.Add(idItem);
            }
        }
        public void BillDetailInfo(object sender)
        {
            frmOrderItemBill _frmItemBill = new frmOrderItemBill();
            frmOrderItemBillVM _frmItemVM = new frmOrderItemBillVM(_orderItemList, BillName);
            _frmItemBill.DataContext = _frmItemVM;
            _frmItemBill.ShowDialog();
        }
        #endregion
    }
}
