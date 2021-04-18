using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TApp.Base;

namespace BTea
{
    class OrderBTeaItem
    {
        public OrderBTeaItem() {; }
        public string OrderItemId { get; set; }
        public string OrderItemName { get; set; }

        public string OrderItemNum { get; set; }
        public string OrderItemPrice { get; set; }

        public string OrderItemSize { get; set; }

        public string OrderItemSRate { set; get; }

        public string OrderItemIRate { set; get; }

        public string OrderItemTopping { set; get; }

        public string OrderItemBillId { set; get; }

        public string OrderItemOrderDate { set; get; }

        public string OrderItemKM { set; get; }

    }

    class FrmOrderBTeaItemVM : TBaseVM
    {
        public FrmOrderBTeaItemVM()
        {
            double sumPrice = 0.0;
            _orderItems = new ObservableCollection<OrderBTeaItem>();

            List<BTeaOrderObject> listOrder =  DBConnection.GetInstance().GetDataOrderObject();
            for (int i = 0; i < listOrder.Count; ++i)
            {
                BTeaOrderObject obj = listOrder[i];
                OrderBTeaItem objItem = new OrderBTeaItem();

                objItem.OrderItemId = obj.BOrderId;
                objItem.OrderItemName = obj.BOrderName;
                objItem.OrderItemPrice = obj.BOrderPrice.ToString(TConst.K_MONEY_FORMAT);
                objItem.OrderItemNum = obj.BOrderNum.ToString();
                if (obj.Type == BTBaseObject.BTeaType.DRINK_TYPE)
                {
                    objItem.OrderItemSize = obj.BOrderSize.ToString();
                    objItem.OrderItemSRate = obj.BOrderSugarRate.ToString();
                    objItem.OrderItemIRate = obj.BOrderIceRate.ToString();
                    objItem.OrderItemTopping = obj.BOrderTopping.ToString();
                }
                else
                {
                    objItem.OrderItemSize = "";
                    objItem.OrderItemSRate = "";
                    objItem.OrderItemIRate = "";
                    objItem.OrderItemTopping = "X";
                }
                objItem.OrderItemBillId = obj.BOrderBillId.ToString();
                objItem.OrderItemOrderDate = obj.BOrderDate.ToString("dd-MMM-yyyy");
                objItem.OrderItemKM = obj.BOrderKm.ToString();

                _orderItems.Add(objItem);

                sumPrice += obj.BOrderPrice;
            }

            _sumPriceItems = sumPrice.ToString(TConst.K_MONEY_FORMAT);
            if (_orderItems.Count > 0)
            {
                _selectedOrderItem = _orderItems[0];
                _orderItemsCount = _orderItems.Count;
            }
        }

        #region MEMBER
        #endregion
        private ObservableCollection<OrderBTeaItem> _orderItems;
        private OrderBTeaItem _selectedOrderItem;
        private int _orderItemsCount = 0;
        private string _sumPriceItems;
        #region PROPERTY

        public string SumPriceItems
        {
            get { return _sumPriceItems; }
            set { _sumPriceItems = value; OnPropertyChange("SumPriceItems"); }
        }

        public int OrderItemsCount
        {
            get { return _orderItemsCount; }
            set { _orderItemsCount = value;  OnPropertyChange("OrderItemsCount"); }
        }
        public ObservableCollection<OrderBTeaItem> OrderItems
        {
            get { return _orderItems; }
            set
            {
                _orderItems = value;
                OnPropertyChange("OrderItems");
            }
        }


        public OrderBTeaItem SelectedOrderItem
        {
            get { return _selectedOrderItem; }
            set
            {
                _selectedOrderItem = value;
                OnPropertyChange("SelectedOrderItem");
            }
        }
        #endregion

        #region METHOD
        #endregion
    }
}
