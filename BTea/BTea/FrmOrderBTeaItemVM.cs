using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TApp.Base;

namespace BTea
{
    class BTTypeItem
    {
        public BTTypeItem(int id, string content)
        {
            Id = id;
            Content = content;
        }

        public int Id { set; get; }
        public string Content { set; get; }
    }

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

        public string OrderItemKMType { set; get; }

    }

    class FrmOrderBTeaItemVM : TBaseVM
    {
        public FrmOrderBTeaItemVM()
        {

            _isCheckTKTotal = true;
            _isCheckTKType = false;
            _tKTypeItems = new List<BTTypeItem>();

            _tKTypeItems.Add(new BTTypeItem((int)BTBaseObject.BTeaType.DRINK_TYPE, "Đồ Uống"));
            _tKTypeItems.Add(new BTTypeItem((int)BTBaseObject.BTeaType.FOOD_TYPE, "Đồ Nhâm Nhi"));
            _tKTypeItems.Add(new BTTypeItem((int)BTBaseObject.BTeaType.OTHER_TYPE, "Đồ Lặt Vặt"));
            _tKTypeItems.Add(new BTTypeItem((int)BTBaseObject.BTeaType.TOPPING_TYPE, "Topping"));

            _seletedTKTypeItem = _tKTypeItems[0];
            _isCheckRankItem = false;
            TKApplyCommand = new RelayCommand(new Action<object>(DoTKApply));

            _orderItems = new ObservableCollection<OrderBTeaItem>();

            DoTKTotal();
        }

        #region MEMBER
        #endregion
        private ObservableCollection<OrderBTeaItem> _orderItems;
        private OrderBTeaItem _selectedOrderItem;
        private int _orderItemsCount = 0;
        private int _totalItemOrder = 0;
        private string _sumPriceItems;
        private bool _isCheckTKTotal;
        private bool _isCheckTKType;
        private bool _isCheckRankItem;
        private int _tkType;

        private List<BTTypeItem> _tKTypeItems;
        private BTTypeItem _seletedTKTypeItem;
        public RelayCommand TKApplyCommand { set; get; }
        #region PROPERTY

        public bool IsCheckTKTotal
        {
            get { return _isCheckTKTotal; }
            set
            {
                _isCheckTKTotal = value;
                if (value == true)
                {
                    _tkType = 0;
                }
                OnPropertyChange("IsCheckTKTotal");
            }
        }

        public bool IsCheckTKType
        {
            get { return _isCheckTKType; }
            set
            {
                _isCheckTKType = value;
                if (value == true)
                {
                    _tkType = 1;
                }
                OnPropertyChange("IsCheckTKType");
            }
        }
        public bool IsCheckRankItem
        {
            get { return _isCheckRankItem; }
            set
            {
                _isCheckRankItem = value;
                if (value == true)
                {
                    _tkType = 2;
                }
                OnPropertyChange("IsCheckRankItem");
            }
        }

        public List<BTTypeItem> TKTypeItems
        {
            get { return _tKTypeItems; }
            set
            {
                _tKTypeItems = value;
                OnPropertyChange("TKTypeItems");
            }
        }

        public BTTypeItem SeletedTKTypeItem
        {
            get { return _seletedTKTypeItem; }
            set
            {
                _seletedTKTypeItem = value;
                OnPropertyChange("SeletedTKTypeItem");
            }
        }

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

        public int TotalOrderItemsCount
        {
            set
            {
                _totalItemOrder = value;
                OnPropertyChange("TotalOrderItemsCount");
            }
            get
            {
                return _totalItemOrder;
            }
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
        public void DoTKTotal()
        {
            int sumPrice = 0;
            if (_orderItems != null)
                _orderItems.Clear();

            List<BTeaOrderObject> listOrder = DBConnection.GetInstance().GetDataOrderObject();
            int totalItem = 0;
            for (int i = 0; i < listOrder.Count; ++i)
            {
                BTeaOrderObject obj = listOrder[i];
                OrderBTeaItem objItem = new OrderBTeaItem();

                objItem.OrderItemId = obj.BOrderId;
                objItem.OrderItemName = obj.BOrderName;
                objItem.OrderItemPrice = obj.BOrderPrice.ToString(TConst.K_MONEY_FORMAT);
                objItem.OrderItemNum = obj.BOrderNum.ToString();
                totalItem += obj.BOrderNum;
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
                if (obj.BOrderKmType == TConst.K_KM_VND)
                {
                    objItem.OrderItemKM = obj.BOrderKm.ToString(TConst.K_MONEY_FORMAT);
                    objItem.OrderItemKMType = "VND";
                }
                else
                {
                    objItem.OrderItemKM = obj.BOrderKm.ToString();
                    objItem.OrderItemKMType = "%";
                }

                _orderItems.Add(objItem);

                sumPrice += obj.BOrderPrice;
            }

            _totalItemOrder = totalItem;

            _sumPriceItems = sumPrice.ToString(TConst.K_MONEY_FORMAT);
            if (_orderItems.Count > 0)
            {
                _selectedOrderItem = _orderItems[0];
                _orderItemsCount = _orderItems.Count;
            }
        }

        public void DoTKByType()
        {
            if (_orderItems != null)
                _orderItems.Clear();

            int sumPrice = 0;
            int totalItem = 0;
            BTTypeItem btType = SeletedTKTypeItem;

            List<BTeaOrderObject> listOrder = DBConnection.GetInstance().GetDataOrderObject();
            for (int i = 0; i < listOrder.Count; ++i)
            {
                BTeaOrderObject obj = listOrder[i];

                if (btType.Id == (int)obj.Type)
                {
                    OrderBTeaItem objItem = new OrderBTeaItem();
                    objItem.OrderItemId = obj.BOrderId;
                    objItem.OrderItemName = obj.BOrderName;
                    objItem.OrderItemPrice = obj.BOrderPrice.ToString(TConst.K_MONEY_FORMAT);
                    objItem.OrderItemNum = obj.BOrderNum.ToString();
                    totalItem += obj.BOrderNum;
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
                    if (obj.BOrderKmType == TConst.K_KM_VND)
                    {
                        objItem.OrderItemKM = obj.BOrderKm.ToString(TConst.K_MONEY_FORMAT);
                        objItem.OrderItemKMType = "VND";
                    }
                    else
                    {
                        objItem.OrderItemKM = obj.BOrderKm.ToString();
                        objItem.OrderItemKMType = "%";
                    }

                    _orderItems.Add(objItem);

                    sumPrice += obj.BOrderPrice;
                }
            }

            _totalItemOrder = totalItem;
            _sumPriceItems = sumPrice.ToString(TConst.K_MONEY_FORMAT);
            if (_orderItems.Count > 0)
            {
                _selectedOrderItem = _orderItems[0];
                _orderItemsCount = _orderItems.Count;
            }
        }

        public void DoTKByRank()
        {
            if (_orderItems != null)
                _orderItems.Clear();
        }
        public void DoTKApply(object obj)
        {
            if (_tkType == 1) // by type
            {
                DoTKByType();
            }
            else if (_tkType == 2) // by rank
            {

            }
            else // Total
            {
                DoTKTotal();
            }

            OnPropertyChange("OrderItems");
            OnPropertyChange("SelectedOrderItem");
            OnPropertyChange("SumPriceItems");
            OnPropertyChange("OrderItemsCount");
            OnPropertyChange("TotalOrderItemsCount");
        }
        #endregion
    }
}
