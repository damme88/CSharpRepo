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
        public frmOrderItemBillVM(List<string> idItems, string billName)
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
                    string idBill = _idItemList[i];
                    if (idBill == orderObj.BOrderId && orderObj.BOrderBillId == billName)
                    {
                        billItems.Add(orderObj);
                    }
                }
            }

            for (int k = 0; k < billItems.Count; k++)
            {
                BTeaOrderObject obj = billItems[k];
                BTeaOrderItems objItem = new BTeaOrderItems();
                objItem.OrderName = obj.BOrderName;
                objItem.OrderNum = obj.BOrderNum.ToString();
                objItem.OrderPrice = obj.BOrderPrice.ToString(TConst.K_MONEY_FORMAT);
                objItem.OrderKm = obj.BOrderKm.ToString();

                if (obj.Type == BTBaseObject.BTeaType.DRINK_TYPE)
                {
                    string Note1 = "";
                    string NoteStr = "";

                    int size = obj.BOrderSize;
                    string sSize = "";
                    if (size == 0)
                    {
                        sSize = "M";
                    }
                    else
                    {
                        sSize = "L";
                    }

                    int nSugar = obj.BOrderSugarRate;
                    string sSugar = "";
                    if (nSugar == 0)
                    {
                        sSugar = "100%";
                    }
                    else if (nSugar == 1)
                    {
                        sSugar = "30%";
                    }
                    else if (nSugar == 2)
                    {
                        sSugar = "50%";
                    }
                    else if (nSugar == 3)
                    {
                        sSugar = "70%";
                    }

                    int nIce = obj.BOrderIceRate;
                    string sIce = nIce.ToString();
                    if (nIce == 0)
                    {
                        sIce = "100%";
                    }
                    else if (nIce == 1)
                    {
                        sIce = "30%";
                    }
                    else if (nIce == 2)
                    {
                        sIce = "50%";
                    }
                    else if (nIce == 3)
                    {
                        sIce = "70%";
                    }

                    Note1 = "Size: " + sSize + ";" + "Duong: " + sSugar + ";" + "Da: " + sIce + "\n";

                    string sTopping = obj.BOrderTopping;
                    string[] itemsTp = sTopping.Split(',');

                    List<ToppingObject> toppingObj = DBConnection.GetInstance().GetDataTopping();
                    for (int i1 = 0; i1 < itemsTp.Length; i1++)
                    {
                        string idTp = itemsTp[i1];
                        for (int i2 = 0; i2 < toppingObj.Count; i2++)
                        {
                            ToppingObject tpObj = toppingObj[i2];
                            string nId = tpObj.BId;
                            if (idTp == nId)
                            {
                                NoteStr += tpObj.BName + ",";
                            }
                        }
                    }
                    NoteStr = Note1 + "Topping: " + NoteStr;
                    objItem.OrderNote = NoteStr;
                }
                else
                {
                    objItem.OrderNote = "Bui's Tea";
                }

                _billOrderList.Add(objItem);
            }
        }
            
        #region MEMBER
        private ObservableCollection<BTeaOrderItems> _billOrderList;
        private BTeaOrderItems _billOrderSelectedItem;
        private List<string> _idItemList;
        #endregion

        #region PROPERTY
        public List<string> IdItemList
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
