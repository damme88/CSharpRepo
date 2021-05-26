using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TApp.Base;

namespace BTea
{
    class frmOrderItemBillVM : TBaseVM
    {
        public frmOrderItemBillVM(List<int> idItems)
        {
            _idItemList = idItems;

            _billOrderList = new ObservableCollection<BTeaOrderItems>();
            List<BTeaOrderObject> billItems = new List<BTeaOrderObject>();
            List<BTeaOrderObject> listOrder = DBConnection.GetInstance().GetDataOrderObject();

            for (int i = 0; i < _idItemList.Count; i++)
            {
                for (int j = 0; j < listOrder.Count; ++j)
                {
                    BTeaOrderObject orderObj = listOrder[j];
                    int idItem = _idItemList[i];
                    if (idItem == orderObj.BOrderId)
                    {
                        billItems.Add(orderObj);
                    }
                }
            }

            int numberItem = 0;
            int priceItem = 0;

            for (int k = 0; k < billItems.Count; k++)
            {
                BTeaOrderObject obj = billItems[k];
                BTeaOrderItems objItem = new BTeaOrderItems();
                objItem.OrderName = obj.BOrderName;
                objItem.OrderNum = obj.BOrderNum.ToString();
                objItem.OrderPrice = obj.BOrderPrice.ToString(TConst.K_MONEY_FORMAT);
                objItem.OrderKm = obj.BOrderKm.ToString();
                numberItem += obj.BOrderNum;
                priceItem += obj.BOrderPrice;

                if (obj.BOrderKmType == TConst.K_KM_VND)
                {
                    objItem.OrderKmType = " "; //VND
                }
                else
                {
                    objItem.OrderKmType = "%";
                }

                if (obj.Type == BTBaseObject.BTeaType.DRINK_TYPE)
                {
                    string strSizeSI = "";
                    string strTp = "";

                    int size = obj.BOrderSize;
                    if (size == 0) strSizeSI = "Size: M";
                    else strSizeSI = "Size: L";

                    string sSugar = obj.SugarToString();
                    string sIce = obj.IceToString();

                    if (sSugar != string.Empty)
                    {
                        strSizeSI += ";Đường: " + sSugar;
                    }

                    if (sIce != string.Empty)
                    {
                        strSizeSI += ";Đá: " + sIce;
                    }

                    strSizeSI += "\n";
                    strTp = obj.ToppingToString();

                    if (strTp != string.Empty)
                    {
                        objItem.OrderNote = strSizeSI + "Topping: " + strTp;
                    }
                    else
                    {
                        objItem.OrderNote = strSizeSI;
                    }
                }
                else
                {
                    objItem.OrderNote = "";
                }
                _billOrderList.Add(objItem);

                OrderItemNumber = numberItem.ToString();
                OrderItemPrice = priceItem.ToString(TConst.K_MONEY_FORMAT);
            }
        }
            
        #region MEMBER
        private ObservableCollection<BTeaOrderItems> _billOrderList;
        private BTeaOrderItems _billOrderSelectedItem;
        private List<int> _idItemList;
        #endregion

        #region PROPERTY
        public string OrderItemNumber { set; get; }
        public string OrderItemPrice { set; get; }

        public List<int> IdItemList
        {
            get { return _idItemList; }
            set { _idItemList = value;  OnPropertyChange("IdItemList"); }
        }

        public ObservableCollection<BTeaOrderItems> BillOrderList
        {
            get { return _billOrderList; }
            set { _billOrderList = value; OnPropertyChange("BillOrderList"); }
        }

        public BTeaOrderItems BillOrderSelectedItem
        {
            get { return _billOrderSelectedItem; }
            set { _billOrderSelectedItem = value; OnPropertyChange("BillOrderList"); }
        }
        #endregion
    }
}
