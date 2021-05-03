using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TApp.Base;

namespace BTea
{
    class FrmEditBillVM : TBaseVM
    {
        public FrmEditBillVM(Action parentAction)
        {
            RemoveOrderItemCmd = new RelayCommand(new Action<object>(RemoveItemBill));
            EditOrderItemCmd = new RelayCommand(new Action<object>(EditItemBill));
            EditBillCmd = new RelayCommand(new Action<object>(EDitBill));
            _pItemMethod = parentAction;
            _ekmTotalVNDType = false;
            _ekmTotalPercentType = true;

            _dataOrderList = new ObservableCollection<BTeaOrderItems>();
        }

        #region MEMBERS
        public RelayCommand RemoveOrderItemCmd { set; get; }
        public RelayCommand EditOrderItemCmd { set; get; }
        public RelayCommand EditBillCmd { set; get; }

        private readonly Action _pItemMethod;
        private int _billId;
        private int _kmTotalType;
        FrmOrderItemSingle _frmItemSingle;
        FrmOrderItemSingleVM _frmItemSingleVM;
        #endregion


        #region PROPERTY
        private string _ebillName;
        public string EBillName
        {
            get { return _ebillName; }
            set { _ebillName = value; OnPropertyChange("EBillName"); }
        }

        private string _ebillCreator;
        public string EBillCreator
        {
            get { return _ebillCreator; }
            set { _ebillCreator = value; OnPropertyChange("EBillCreator"); }
        }

        private string _ebillTable;
        public string EBillTable
        {
            get { return _ebillTable; }
            set { _ebillTable = value;  OnPropertyChange("EBillTable"); }
        }

        private DateTime _ebillDate;
        public DateTime EBillStartDate
        {
            get { return _ebillDate; }
            set { _ebillDate = value;  OnPropertyChange("EBillStartDate"); }
        }

        private string _ebillPhone;
        public string EBillPhone
        {
            get { return _ebillPhone; }
            set { _ebillPhone = value;  OnPropertyChange("EBillPhone"); }
        }

        private string _ebillAddress;
        public string EBillAddress
        {
            get { return _ebillAddress; }
            set
            {
                _ebillAddress = value; OnPropertyChange("EBillAddress");
            }
        }

        private string _ebillNote;
        public string EBillNote
        {
            get { return  _ebillNote; }
            set { _ebillNote = value;  OnPropertyChange("EBillNote"); }
        }

        private string _ebillSumprice;
        public string EBillSumPrice
        {
            get { return _ebillSumprice; }
            set { _ebillSumprice = value;  OnPropertyChange("EBillSumPrice"); }
        }

        private string _eKMSumBill;
        public string EKMSumBill
        {
            get
            {
                return _eKMSumBill.ToString();
            }
            set
            {
                int iVal = TConst.ConvertMoney(value);
                if (_kmTotalType == TConst.K_KM_PERCENT)
                {
                    if (iVal < 0 || iVal > 100)
                    {
                        _eKMSumBill = "0";
                    }
                    else
                    {
                        _eKMSumBill = iVal.ToString(TConst.K_MONEY_FORMAT);
                    }
                }
                else
                {
                    _eKMSumBill = iVal.ToString();
                }

                OnPropertyChange("EKMSumBill");
                UpdateTotalPriceByKM();
            }
        }

        private ObservableCollection<BTeaOrderItems> _dataOrderList;
        public ObservableCollection<BTeaOrderItems> EDataOrderList
        {
            get { return _dataOrderList; }
            set
            {
                _dataOrderList = value;
                OnPropertyChange("DataOrderList");
            }
        }

        private BTeaOrderItems _bteaOderItem;
        public BTeaOrderItems EBTeaOrderSelectedItem
        {
            get { return _bteaOderItem; }
            set { _bteaOderItem = value; OnPropertyChange("BTeaOrderSelectedItem"); }
        }

        private bool _ekmTotalVNDType;
        public bool EKMTotalVNDType
        {
            get { return _ekmTotalVNDType; }
            set
            {
                _ekmTotalVNDType = value;
                if (value == true)
                {
                    _kmTotalType = TConst.K_KM_VND;
                }
                OnPropertyChange("EKMTotalVNDType");
                EKMSumBill = "0";
                UpdateTotalPriceByKM();
            }
        }

        private bool _ekmTotalPercentType;
        public bool EKMTotalPercentType
        {
            get { return _ekmTotalPercentType; }
            set
            {
                _ekmTotalPercentType = value;
                if (value == true)
                {
                    _kmTotalType = TConst.K_KM_PERCENT;
                }
                OnPropertyChange("EKMTotalPercentType");
                EKMSumBill = "0";
                UpdateTotalPriceByKM();
            }
        }
        #endregion


        #region METHOD
        void UpdateTotalPriceByKM()
        {
            int sumPrice = 0;
            for (int i = 0; i < _dataOrderList.Count; i++)
            {
                BTeaOrderItems orderItem = _dataOrderList[i];
                string strPrice = orderItem.OrderPrice;
                int oPrice = TConst.ConvertMoney(strPrice);
                sumPrice += oPrice;
            }

            if (_kmTotalType == TConst.K_KM_VND)
            {
                if (sumPrice > 0)
                {
                    int kmVal = TConst.ConvertMoney(_eKMSumBill);
                    int billPriceEnd = sumPrice - kmVal;
                    if (billPriceEnd < 0)
                    {
                        billPriceEnd = 0;
                    }
                    EBillSumPrice = billPriceEnd.ToString(TConst.K_MONEY_FORMAT);
                }
            }
            else
            {
                int kmVal = TConst.ConvertInt(_eKMSumBill);
                if (kmVal >= 0 && kmVal <= 100)
                {
                    if (sumPrice > 0)
                    {
                        double value_off = sumPrice * kmVal / 100.0;
                        double billPriceEnd = sumPrice - value_off;
                        if (billPriceEnd < 0)
                        {
                            billPriceEnd = 0;
                        }
                        EBillSumPrice = billPriceEnd.ToString(TConst.K_MONEY_FORMAT);
                    }
                }
            }
        }

        public void RemoveItemBill(object obj)
        {
            for (int i = 0; i < _dataOrderList.Count; i++)
            {
                if (_dataOrderList[i] == _bteaOderItem)
                {
                    _dataOrderList.RemoveAt(i);
                }
            }

            if (_dataOrderList.Count > 0)
            {
                _bteaOderItem = _dataOrderList[0];
            }
            else
            {
                _ebillSumprice = "0.0000";
            }

            OnPropertyChange("EDataOrderList");
            OnPropertyChange("EBTeaOrderSelectedItem");
        }

        public void EditItemBill(object obj)
        {
            BTeaOrderItems sOrderItem = _bteaOderItem;
            if (sOrderItem == null)
            {
                return;
            }

            _frmItemSingle = new FrmOrderItemSingle();
            _frmItemSingleVM = new FrmOrderItemSingleVM(UpdateOrderItemCmd);
            BTBaseObject orderObj = sOrderItem.OrderObject as BTBaseObject;

            int number = Convert.ToInt32(sOrderItem.OrderNum);
            _frmItemSingleVM.SetInfo1(orderObj.BName, orderObj.BPrice, number, sOrderItem.OrderKm, sOrderItem.OrderKmType);

            if (orderObj != null)
            {
                if (orderObj.Type == BTBaseObject.BTeaType.DRINK_TYPE)
                {
                    DrinkObject drObject = orderObj as DrinkObject;
                    if (drObject != null)
                    {
                        int idx1 = drObject.DrinkSize;
                        int idx2 = drObject.SugarRate;
                        int idx3 = drObject.IceRate;
                        _frmItemSingleVM.SetInfo2(idx1, idx2, idx3);
                        _frmItemSingleVM.SetIsDrinkItem(true);
                        for (int i = 0; i < drObject.TPListObj.Count; i++)
                        {
                            ToppingObject tpObj = drObject.TPListObj[i];
                            if (tpObj != null)
                            {
                                for (int j = 0; j < _frmItemSingleVM.SingleToppingItems.Count; j++)
                                {
                                    ToppingItemCheck tpCheck = _frmItemSingleVM.SingleToppingItems[j];
                                    if (tpCheck.Content == tpObj.BName)
                                    {
                                        _frmItemSingleVM.SetTopping(j);
                                    }
                                }
                            }
                        }

                    }
                }
                else if (orderObj.Type == BTBaseObject.BTeaType.FOOD_TYPE)
                {
                    _frmItemSingleVM.SetIsDrinkItem(false);
                }
                else if (orderObj.Type == BTBaseObject.BTeaType.OTHER_TYPE)
                {
                    _frmItemSingleVM.SetIsDrinkItem(false);
                }
                else if (orderObj.Type == BTBaseObject.BTeaType.TOPPING_TYPE)
                {
                    _frmItemSingleVM.SetIsDrinkItem(false);
                }
            }

            _frmItemSingle.DataContext = _frmItemSingleVM;
            _frmItemSingle.ShowDialog();
        }


        public void SetInfo1(int id, string name, string creator, 
                             string table, string date, 
                             string phone, string address, string note)
        {
            _billId = id;
            _ebillName = name;
            _ebillCreator = creator;
            _ebillDate = Convert.ToDateTime(date);
            _ebillTable = table;
            _ebillPhone = phone;
            _ebillAddress = address;
            _ebillNote = note;
        }

        public void SetInfo2(string kmval,  string kmtype)
        {
            if (kmtype == "%")
            {
                _ekmTotalPercentType = true;
                _ekmTotalVNDType = false;
                _eKMSumBill = kmval;
            }
            else
            {
                _ekmTotalPercentType = false;
                _ekmTotalVNDType = true;
                _eKMSumBill = kmval;
            }
        }

        public void SetDataList(List<int> dataList)
        {
            List<BTeaOrderObject> billItems = new List<BTeaOrderObject>();
            List<BTeaOrderObject> listOrder = DBConnection.GetInstance().GetDataOrderObject();

            for (int i = 0; i < dataList.Count; i++)
            {
                for (int j = 0; j < listOrder.Count; ++j)
                {
                    BTeaOrderObject orderObj = listOrder[j];
                    int idItem = dataList[i];
                    if (idItem == orderObj.BOrderId)
                    {
                        billItems.Add(orderObj);
                    }
                }
            }

            int sumPrice = 0;
            int sumNumber = 0;
            for (int k = 0; k < billItems.Count; k++)
            {
                BTeaOrderObject obj = billItems[k];
                BTeaOrderItems objItem = new BTeaOrderItems();
                objItem.OrderName = obj.BOrderName;
                objItem.OrderNum = obj.BOrderNum.ToString();
                objItem.OrderPrice = obj.BOrderPrice.ToString(TConst.K_MONEY_FORMAT);
                objItem.OrderId = obj.BOrderId;
                sumPrice += obj.BOrderPrice;
                sumNumber += obj.BOrderNum;

                objItem.OrderKm = obj.BOrderKm.ToString();
                objItem.OrderObject = obj.MakeObject();

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
                    string sSize = "";
                    if (size == 0)
                    {
                        sSize = "M";
                    }
                    else
                    {
                        sSize = "L";
                    }

                    string sSugar = obj.SugarToString();
                    string sIce = obj.IceToString();

                    strSizeSI = "Size: " + sSize;
                    if (sSugar != string.Empty)
                    {
                        strSizeSI += ";" + "Đường: " + sSugar;
                    }

                    if (sIce != string.Empty)
                    {
                        strSizeSI += ";" + "Đá: " + sIce;
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
                _dataOrderList.Add(objItem);
            }

            if (_dataOrderList.Count > 0)
            {
                _bteaOderItem = _dataOrderList[0];
            }
            _ebillSumprice = sumPrice.ToString(TConst.K_MONEY_FORMAT);
        }

        public void UpdateOrderItemCmd()
        {
            BTBaseObject obj = _bteaOderItem.OrderObject;
            if (obj != null && _frmItemSingleVM != null)
            {
                _bteaOderItem.OrderNum = _frmItemSingleVM.OrderSingleNum.ToString();
                _bteaOderItem.OrderKm = _frmItemSingleVM.OrderSingleItemKM;
                _bteaOderItem.OrderKmType = _frmItemSingleVM.OrderKmType;

                if (obj.Type == BTBaseObject.BTeaType.DRINK_TYPE)
                {
                    DrinkObject drObj = obj as DrinkObject;
                    if (drObj != null)
                    {
                        drObj.DrinkSize = _frmItemSingleVM.SelectedSizeItem.Index;
                        drObj.IceRate = _frmItemSingleVM.SelectedIceItem.Index;
                        drObj.SugarRate = _frmItemSingleVM.SelectedSugarItem.Index;

                        // Update topping data
                        drObj.TPListObj.Clear();

                        List<ToppingObject> dataTopping = DBConnection.GetInstance().GetDataTopping();
                        int nCountTopping = _frmItemSingleVM.SingleToppingItems.Count;
                        for (int i = 0; i < nCountTopping; ++i)
                        {
                            ToppingItemCheck checkTp = _frmItemSingleVM.SingleToppingItems[i];
                            if (checkTp.IsSelected == true)
                            {
                                for (int j = 0; j < dataTopping.Count; j++)
                                {
                                    ToppingObject tpData = dataTopping[j];
                                    if (checkTp.Id.ToString() == tpData.BId)
                                    {
                                        ToppingObject newTpObj = new ToppingObject();
                                        newTpObj.BId = tpData.BId;
                                        newTpObj.BName = tpData.BName;
                                        newTpObj.BPrice = tpData.BPrice;
                                        drObj.TPListObj.Add(newTpObj);
                                    }
                                }
                            }
                        }

                    }
                }

                //Calucate price and note again
                _bteaOderItem.MakeSummaryPrice();
                _bteaOderItem.MakeNoteSumary();
            }

            if (_frmItemSingle != null)
            {
                _frmItemSingle.Close();
            }

            //Update list view 
            CollectionViewSource.GetDefaultView(_dataOrderList).Refresh();
            OnPropertyChange("EDataOrderList");

            double sumPrice = 0.0;
            for (int i = 0; i < _dataOrderList.Count; i++)
            {
                BTeaOrderItems orderItem = _dataOrderList[i];
                string strPrice = orderItem.OrderPrice;
                double oPrice = Convert.ToDouble(strPrice);
                sumPrice += oPrice;
            }
            _ebillSumprice = sumPrice.ToString(TConst.K_MONEY_FORMAT);
            OnPropertyChange("EBillSumPrice");
        }


        public void EDitBill(object obj)
        {
            BillObject billObj = new BillObject();
            billObj.BillId = _billId;
            billObj.BillName = _ebillName;
            billObj.BillCreator = _ebillCreator;
            billObj.BillPrice = TConst.ConvertMoney(_ebillSumprice);
            billObj.BillTableNumber = _ebillTable;
            billObj.BillDate = _ebillDate;
            billObj.BillPhone = _ebillPhone;
            billObj.BillAddress = _ebillAddress;
            billObj.BillNote = _ebillNote;
            
            if (_ekmTotalPercentType == true)
            {
                billObj.KMValue = TConst.ConvertInt(_eKMSumBill);
                billObj.KMType = 0;
            }
            else if (_ekmTotalVNDType == true)
            {
                billObj.KMValue = TConst.ConvertMoney(_eKMSumBill);
                billObj.KMType = 1;
            }

            string strOrderItem = "";
            for (int i = 0; i < _dataOrderList.Count; i++)
            {
                BTBaseObject orderBaseObj = _dataOrderList[i].OrderObject;
                BTeaOrderObject orderObject = new BTeaOrderObject();

                orderObject.BOrderId = _dataOrderList[i].OrderId;
                orderObject.BOrderIdItem = orderBaseObj.BId;
                orderObject.BOrderName = orderBaseObj.BName;
                orderObject.BOrderPrice = _dataOrderList[i].MakeSummaryPrice();
                orderObject.BOrderNum = TConst.ConvertInt(_dataOrderList[i].OrderNum);
                strOrderItem = strOrderItem + _dataOrderList[i].OrderId + ",";
                orderObject.Type = orderBaseObj.Type;
                if (orderBaseObj.Type == BTBaseObject.BTeaType.DRINK_TYPE)
                {
                    DrinkObject drObj = orderBaseObj as DrinkObject;
                    if (drObj != null)
                    {
                        orderObject.BOrderSize = drObj.DrinkSize;
                        orderObject.BOrderSugarRate = drObj.SugarRate;
                        orderObject.BOrderIceRate = drObj.IceRate;

                        string strTp = "";
                        for (int ii = 0; ii < drObj.TPListObj.Count; ii++)
                        {
                            string strId = drObj.TPListObj[ii].BId;
                            strTp = strTp + strId + ",";
                        }
                        orderObject.BOrderTopping = strTp;
                    }
                }

                orderObject.BOrderBillId = _ebillName;
                orderObject.BOrderDate = _ebillDate;
                if (_dataOrderList[i].OrderKmType == " ")
                {
                    orderObject.BOrderKmType = TConst.K_KM_VND;
                    orderObject.BOrderKm = TConst.ConvertMoney(_dataOrderList[i].OrderKm);
                }
                else
                {
                    orderObject.BOrderKmType = TConst.K_KM_PERCENT;
                    orderObject.BOrderKm = TConst.ConvertInt(_dataOrderList[i].OrderKm);
                }

                bool bRet2 = DBConnection.GetInstance().EditOrderItem(orderObject);

                billObj.BillOrderItem = strOrderItem;
                DBConnection.GetInstance().EditBillItem(billObj);
                _pItemMethod.Invoke();
            }
        }

        #endregion
    }
}
