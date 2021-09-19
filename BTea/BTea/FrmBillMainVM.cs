using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using TApp.Base;

namespace BTea
{
    class FrmBillMainVM : TBaseVM
    {
        
        public FrmBillMainVM()
        {
            CmdDeleteBill = new RelayCommand(new Action<object>(DoDeleteBill));
            CmdEditBill = new RelayCommand(new Action<object>(DoEditBill));
            CmdPrintBill = new RelayCommand(new Action<object>(DoPrintBill));
            _billItem = new ObservableCollection<BillItem>();
            GetDataBillFromDB();
            if (_billItem.Count >  0)
            {
                _selectedBillItem = _billItem[0];
            }

            _tkCheckTotal = true;
            _tkCheckDay = _tkCheckMonth = _tkCheckYear = false;
            _tkDate = DateTime.Now;

            _tkMonth = new List<string>();
            _tkMonth.Add("1");
            _tkMonth.Add("2");
            _tkMonth.Add("3");
            _tkMonth.Add("4");
            _tkMonth.Add("5");
            _tkMonth.Add("6");
            _tkMonth.Add("7");
            _tkMonth.Add("8");
            _tkMonth.Add("9");
            _tkMonth.Add("10");
            _tkMonth.Add("11");
            _tkMonth.Add("12");
            SelectedTKMonth = _tkMonth[0];

            _tkYear = new List<string>();
            _tkYear.Add("2020");
            _tkYear.Add("2021");
            _tkYear.Add("2022");
            _tkYear.Add("2023");
            _tkYear.Add("2024");
            _tkYear.Add("2025");
            _tkYear.Add("2027");
            _tkYear.Add("2028");
            _tkYear.Add("2029");
            _tkYear.Add("2030");
            _tkYear.Add("2031");
            _tkYear.Add("2032");
            _tkYear.Add("2033");
            _tkYear.Add("2034");
            _tkYear.Add("2035");

            SelectedTKYear = _tkYear[0];

            CmdTK = new RelayCommand(new Action<object>(DoTKBill));
        }

        public RelayCommand CmdDeleteBill { set; get; }
        public RelayCommand CmdEditBill { set; get; }
        public RelayCommand CmdPrintBill { set; get; }

        FrmEditBill _frmEditBill;
        private ObservableCollection<BillItem> _billItem;
        private BillItem _selectedBillItem;

        private int _billCount;
        private int _billSumValue;
        private int _tkType;

        public int BillCount
        {
            get { return _billCount; }
            set
            {
                _billCount = value;
                OnPropertyChange("BillCount");
            }
        }

        public string BillSumValue
        {
            get { return _billSumValue.ToString(TConst.K_MONEY_FORMAT); }
            set
            {
                int iValue = TConst.ConvertMoney(value);
                _billSumValue = iValue;
                OnPropertyChange("BillSumValue");
            }
        }

        public ObservableCollection<BillItem> BillItems
        {
            get { return _billItem; }
            set
            {
                _billItem = value;
                OnPropertyChange("BillItems");
            }
        }

        public BillItem SelectedBillItem
        {
            get { return _selectedBillItem; }
            set
            {
                _selectedBillItem = value;
                OnPropertyChange("SelectedBillItem");
            }
        }

        public void GetDataBillFromDB()
        {
            List<BillObject> data_list = DBConnection.GetInstance().GetDataBillObject();
            if (_billItem != null)
            {
                _billItem.Clear();
                _billCount = 0;
                _billSumValue = 0;
            }

            for (int i = 0; i < data_list.Count; ++i)
            {
                BillItem bItem = new BillItem();
                BillObject billObject = data_list[i];
                bItem.BillId = "BL" + billObject.BillId;
                bItem.BillName = billObject.BillName;
                bItem.BillPrice = billObject.BillPrice.ToString(TConst.K_MONEY_FORMAT);
                bItem.BillCreator = billObject.BillCreator;
                bItem.BillDate = billObject.BillDate.ToString("dd-MMM-yyyy");
                bItem.BillTableNumber = billObject.BillTableNumber;
                bItem.BillPhone = billObject.BillPhone;
                bItem.BillAddress = billObject.BillAddress;
                bItem.BillNote = billObject.BillNote;

                _billSumValue += billObject.BillPrice;

                int kmType = billObject.KMType;
                if (kmType == TConst.K_KM_PERCENT)
                {
                    bItem.BillKM = billObject.KMValue.ToString();
                    bItem.BillKMType = "%";
                }
                else
                {
                    bItem.BillKM = billObject.KMValue.ToString(TConst.K_MONEY_FORMAT);
                    bItem.BillKMType = " ";
                }

                string strItemOrder = billObject.BillOrderItem;
                string[] itemsArr = strItemOrder.Split(',');
                for (int ii = 0; ii < itemsArr.Length; ii++)
                {
                    string strValue = itemsArr[ii];
                    bItem.AddItemOrder(TConst.ConvertInt(strValue));
                }
                _billItem.Add(bItem);
            }

            _billCount = _billItem.Count;
        }

        public void UpdateBillCmd()
        {
            if (_frmEditBill != null)
            {
                _frmEditBill.Close();
            }

            DoTKBill(null);
        }
        public void DoEditBill(object obj)
        {
            if (_selectedBillItem != null)
            {
                _frmEditBill = new FrmEditBill();
                FrmEditBillVM _frmEditBillVM = new FrmEditBillVM(UpdateBillCmd);

                string strBillId = _selectedBillItem.BillId;
                strBillId = strBillId.Replace("BL", "");
                int nId = TConst.ConvertInt(strBillId);

                _frmEditBillVM.SetInfo1(nId, _selectedBillItem.BillName, _selectedBillItem.BillCreator,
                                        _selectedBillItem.BillTableNumber, _selectedBillItem.BillDate,
                                        _selectedBillItem.BillPhone, _selectedBillItem.BillAddress, _selectedBillItem.BillNote);

                _frmEditBillVM.SetInfo2(_selectedBillItem.BillKM, _selectedBillItem.BillKMType);

                _frmEditBillVM.SetDataList(_selectedBillItem.OrderItemList);

                _frmEditBill.DataContext = _frmEditBillVM;
                _frmEditBill.ShowDialog();
            }
            else
            {
                MessageBox.Show("Hay chon hoa don de sua", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public ObservableCollection<BTeaOrderItems> GetListOrder()
        {
            ObservableCollection<BTeaOrderItems> _dataList = new ObservableCollection<BTeaOrderItems>();
            List<BTeaOrderObject> billItems = new List<BTeaOrderObject>();
            List<BTeaOrderObject> listOrderDB = DBConnection.GetInstance().GetDataOrderObject();

            List<int> itemOrderList = _selectedBillItem.OrderItemList;
            for (int i = 0; i < itemOrderList.Count; i++)
            {
                for (int j = 0; j < listOrderDB.Count; ++j)
                {
                    BTeaOrderObject orderObj = listOrderDB[j];
                    int idItem = itemOrderList[i];
                    if (idItem == orderObj.BOrderId)
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
                objItem.OrderId = obj.BOrderId;

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
                    if (size == 0) strSizeSI = "Size: M";
                    else           strSizeSI = "Size: L";

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
                _dataList.Add(objItem);
            }

            return _dataList;
        }

        public void DoPrintBill(object obj)
        {
            if (_selectedBillItem == null)
            {
                return;
            }
            
            FrmPrintBill printDlg = new FrmPrintBill();
            FrmPrintBillVM _printVM = new FrmPrintBillVM();
            int totalNumber = 0;
            int totalNumDrink = 0;
            double totalPriceDrink = 0;

            ObservableCollection<BTeaOrderItems> _dataOrderList = GetListOrder();
            for (int i = 0; i < _dataOrderList.Count; ++i)
            {
                BTeaOrderItems orderItem = _dataOrderList[i];
                PrintBillIData dataItem = new PrintBillIData();
                dataItem.NumberProduct = orderItem.OrderNum;
                dataItem.SumPrice = orderItem.OrderPrice;
                dataItem.NameProduct = orderItem.MakePrintDescription();
                dataItem.BasePriceProduct = orderItem.OrderObject.BPrice.ToString();

                if (orderItem.OrderObject.Type == BTBaseObject.BTeaType.DRINK_TYPE)
                {
                    PrintBillIData dataItemDrink = new PrintBillIData();
                    dataItemDrink.NumberProduct = orderItem.OrderNum;
                    dataItemDrink.SumPrice = orderItem.OrderPrice;
                    dataItemDrink.NameProduct = orderItem.MakePrintDescription();
                    try
                    {
                        totalNumDrink += Convert.ToInt32(orderItem.OrderNum);
                    }
                    catch
                    {
                        totalNumDrink = 0;
                    }

                    try
                    {
                        totalPriceDrink += Convert.ToDouble(orderItem.OrderPrice);
                    }
                    catch
                    {
                        totalPriceDrink = 0.0;
                    }

                    _printVM.AddDataDrink(dataItemDrink);
                }

                try
                {
                    totalNumber += Convert.ToInt32(orderItem.OrderNum);
                }
                catch
                {
                    totalNumber = 0;
                }

                _printVM.AddData(dataItem);

                // Print topping list
                int orderNum = Convert.ToInt32(orderItem.OrderNum);

                BTBaseObject bObj = orderItem.OrderObject;
                DrinkObject dObj = bObj as DrinkObject;
                if (dObj != null)
                {
                    if (dObj.TPListObj.Count > 0)
                    {
                        for (int ii = 0; ii < dObj.TPListObj.Count; ii++)
                        {
                            ToppingObject tpObj = dObj.TPListObj[ii];
                            if (tpObj != null)
                            {
                                PrintBillIData dItem = new PrintBillIData();
                                //dItem.NumberProduct = orderItem.OrderNum;
                                dItem.SumPrice = (tpObj.BPrice * orderNum).ToString(TConst.K_MONEY_FORMAT);
                                dItem.BasePriceProduct = tpObj.BPrice.ToString(TConst.K_MONEY_FORMAT);
                                dItem.NameProduct = " +" + tpObj.BName;
                                _printVM.AddData(dItem);
                            }
                        }
                    }
                }
            }

            if (_printVM.PrintBillItemsDrink.Count == 0)
            {
                _printVM.SetHideDrinkBill();
            }

            string billName = _selectedBillItem.BillName;
            string billNumber = _selectedBillItem.BillTableNumber;
            string billCreator = _selectedBillItem.BillCreator;
            string billDate = _selectedBillItem.BillDate;
            string billPrice = _selectedBillItem.BillPrice;
            string billKm = _selectedBillItem.BillKM;
            if (billKm == "0")
            {
                billKm = "";
            }
            else
            {
                if (_selectedBillItem.BillKMType == "%")
                {
                    billKm += "%";
                }
                else
                {
                    billKm += "đ";
                }
            }
            

            _printVM.SetInfo(billName, billNumber, billCreator,
                            billDate, billKm, billPrice,
                            totalNumber.ToString());

            _printVM.SetInfoDrink(totalNumDrink, totalPriceDrink);
            printDlg.DataContext = _printVM;
            printDlg.ShowDialog();
        }

        public void DoDeleteBill(object obj)
        {
            string sCap = "Dữ liệu sẽ được xóa trong Database(xóa vĩnh viễn).";
            sCap += "\nBạn có chắc chắn muốn xóa hóa đơn này?";

            MessageBoxResult msg = TConst.MsgYNQ(sCap);
            if (msg == MessageBoxResult.No)
            {
                return;
            }

            BillItem bItem = _selectedBillItem;
            if (bItem != null)
            {
                string billName = bItem.BillName;
                List<int> orderList = bItem.OrderItemList;
                for (int i = 0; i < orderList.Count; ++i)
                {
                    int id = orderList[i];
                    bool bRet = DBConnection.GetInstance().DeleteOrderItem(id);
                }

                string strBillId = bItem.BillId;
                strBillId = strBillId.Replace("BL", "");
                int nId = TConst.ConvertInt(strBillId);
                if (nId > 0)
                {
                    bool bRet = DBConnection.GetInstance().DeleteBillItem(nId);
                }
            }

            DoTKBill(null);
        }
        public void DoFilterByDay()
        {
            if (_billItem != null)
            {
                _billItem.Clear();
                _billCount = 0;
                _billSumValue = 0;
            }

            List<BillObject> data_list = DBConnection.GetInstance().GetDataBillObject();
            _billItem = new ObservableCollection<BillItem>();

            for (int i = 0; i < data_list.Count; ++i)
            {
                BillObject billObject = data_list[i];
                DateTime itemDate = billObject.BillDate;
                if (_tkDate.Day == itemDate.Day &&
                    _tkDate.Month == itemDate.Month &&
                    _tkDate.Year == itemDate.Year)
                {
                    BillItem bItem = new BillItem();
                    bItem.BillId = "BL" + billObject.BillId;
                    bItem.BillName = billObject.BillName;
                    bItem.BillPrice = billObject.BillPrice.ToString(TConst.K_MONEY_FORMAT);
                    bItem.BillCreator = billObject.BillCreator;
                    bItem.BillDate = billObject.BillDate.ToString("dd-MMM-yyyy");
                    bItem.BillTableNumber = billObject.BillTableNumber;
                    bItem.BillPhone = billObject.BillPhone;
                    bItem.BillAddress = billObject.BillAddress;
                    bItem.BillNote = billObject.BillNote;
                    _billSumValue += billObject.BillPrice;
                    int kmType = billObject.KMType;
                    if (kmType == TConst.K_KM_PERCENT)
                    {
                        bItem.BillKM = billObject.KMValue.ToString();
                        bItem.BillKMType = "%";
                    }
                    else
                    {
                        bItem.BillKM = billObject.KMValue.ToString(TConst.K_MONEY_FORMAT);
                        bItem.BillKMType = " ";
                    }

                    string strItemOrder = billObject.BillOrderItem;
                    string[] itemsArr = strItemOrder.Split(',');
                    for (int ii = 0; ii < itemsArr.Length; ii++)
                    {
                        string strValue = itemsArr[ii];
                        bItem.AddItemOrder(TConst.ConvertInt(strValue));
                    }
                    _billItem.Add(bItem);
                }
                _billCount = _billItem.Count;
            }
        }

        public void DoFilterByMonthYear()
        {
            if (_billItem != null)
            {
                _billItem.Clear();
                _billCount = 0;
                _billSumValue = 0;
            }

            List<BillObject> data_list = DBConnection.GetInstance().GetDataBillObject();
            _billItem = new ObservableCollection<BillItem>();

            for (int i = 0; i < data_list.Count; ++i)
            {
                BillObject billObject = data_list[i];
                string strMonth = SelectedTKMonth;
                string strYear = SelectedTKYear;
                string strItemMonth = billObject.BillDate.Month.ToString();
                string strItemYear = billObject.BillDate.Year.ToString();

                if (strMonth == strItemMonth &&
                    strYear == strItemYear)
                {
                    BillItem bItem = new BillItem();
                    bItem.BillId = "BL" + billObject.BillId;
                    bItem.BillName = billObject.BillName;
                    bItem.BillPrice = billObject.BillPrice.ToString(TConst.K_MONEY_FORMAT);
                    bItem.BillCreator = billObject.BillCreator;
                    bItem.BillDate = billObject.BillDate.ToString("dd-MMM-yyyy");
                    bItem.BillTableNumber = billObject.BillTableNumber;
                    bItem.BillPhone = billObject.BillPhone;
                    bItem.BillAddress = billObject.BillAddress;
                    bItem.BillNote = billObject.BillNote;
                    _billSumValue += billObject.BillPrice;
                    int kmType = billObject.KMType;
                    if (kmType == TConst.K_KM_PERCENT)
                    {
                        bItem.BillKM = billObject.KMValue.ToString();
                        bItem.BillKMType = "%";
                    }
                    else
                    {
                        bItem.BillKM = billObject.KMValue.ToString(TConst.K_MONEY_FORMAT);
                        bItem.BillKMType = " ";
                    }

                    string strItemOrder = billObject.BillOrderItem;
                    string[] itemsArr = strItemOrder.Split(',');
                    for (int ii = 0; ii < itemsArr.Length; ii++)
                    {
                        string strValue = itemsArr[ii];
                        bItem.AddItemOrder(TConst.ConvertInt(strValue));
                    }
                    _billItem.Add(bItem);
                }
                _billCount = _billItem.Count;
            }
        }

        public void DoFilterByYear()
        {
            if (_billItem != null)
            {
                _billItem.Clear();
                _billCount = 0;
                _billSumValue = 0;
            }

            List<BillObject> data_list = DBConnection.GetInstance().GetDataBillObject();
            _billItem = new ObservableCollection<BillItem>();

            for (int i = 0; i < data_list.Count; ++i)
            {

                BillObject billObject = data_list[i];

                string strYear = SelectedTKYear;
                string strItemYear = billObject.BillDate.Year.ToString();
                if (strYear == strItemYear)
                {
                    BillItem bItem = new BillItem();
                    bItem.BillId = "BL" + billObject.BillId;
                    bItem.BillName = billObject.BillName;
                    bItem.BillPrice = billObject.BillPrice.ToString(TConst.K_MONEY_FORMAT);
                    bItem.BillCreator = billObject.BillCreator;
                    bItem.BillDate = billObject.BillDate.ToString("dd-MMM-yyyy");
                    bItem.BillTableNumber = billObject.BillTableNumber;
                    bItem.BillPhone = billObject.BillPhone;
                    bItem.BillAddress = billObject.BillAddress;
                    bItem.BillNote = billObject.BillNote;
                    _billSumValue += billObject.BillPrice;
                    int kmType = billObject.KMType;
                    if (kmType == TConst.K_KM_PERCENT)
                    {
                        bItem.BillKM = billObject.KMValue.ToString();
                        bItem.BillKMType = "%";
                    }
                    else
                    {
                        bItem.BillKM = billObject.KMValue.ToString(TConst.K_MONEY_FORMAT);
                        bItem.BillKMType = " ";
                    }

                    string strItemOrder = billObject.BillOrderItem;
                    string[] itemsArr = strItemOrder.Split(',');
                    for (int ii = 0; ii < itemsArr.Length; ii++)
                    {
                        string strValue = itemsArr[ii];
                        bItem.AddItemOrder(TConst.ConvertInt(strValue));
                    }
                    _billItem.Add(bItem);
                }
                _billCount = _billItem.Count;
            }
        }

        private bool _tkCheckTotal;
        public bool TKCheckTotal
        {
            get { return _tkCheckTotal; }
            set
            {
                _tkCheckTotal = value;
                if (value == true) _tkType = TConst.K_TK_TOTAL;
                OnPropertyChange("TKCheckTotal");
            }
        }

        private bool _tkCheckDay;
        public bool TKCheckDay
        {
            get { return _tkCheckDay; }
            set
            {
                _tkCheckDay = value;
                if (value == true) _tkType = TConst.K_TK_DAY;
                OnPropertyChange("TKCheckDay");
            }
        }

        private bool _tkCheckMonth;
        public bool TKCheckMonth
        {
            get { return _tkCheckMonth; }
            set
            {
                _tkCheckMonth = value;
                if (value == true) _tkType = TConst.K_TK_MONTH;
                OnPropertyChange("TKCheckMonth");
            }
        }

        private bool _tkCheckYear;
        public bool TKCheckYear
        {
            get { return _tkCheckYear; }
            set
            {
                _tkCheckYear = value;
                if (value == true) _tkType = TConst.K_TK_YEAR;
                OnPropertyChange("TKCheckYear");
            }
        }

        private DateTime _tkDate;
        public DateTime TKDate
        {
            get { return _tkDate; }
            set
            {
                _tkDate = value;
                OnPropertyChange("TKDate");
            }
        }

        private List<string> _tkMonth;
        public List<string> TKMonth
        {
            get { return _tkMonth; }
            set
            {
                _tkMonth = value;
                OnPropertyChange("TKMonth");
            }
        }

        public string SelectedTKMonth { set; get; }

        private List<string> _tkYear;
        public List<string> TKYear
        {
            get { return _tkYear; }
            set
            {
                _tkYear = value;
                OnPropertyChange("TKYear");
            }
        }

        public string SelectedTKYear { set; get; }

        public RelayCommand CmdTK { set; get; }

        public void DoTKBill(object obj)
        {
            if (_tkType == TConst.K_TK_TOTAL)
            {
                GetDataBillFromDB();
            }
            else if (_tkType == TConst.K_TK_DAY)
            {
                DoFilterByDay();
            }
            else if (_tkType == TConst.K_TK_MONTH)
            {
                DoFilterByMonthYear();
            }
            else if (_tkType == TConst.K_TK_YEAR)
            {
                DoFilterByYear();
            }

            if (_billItem.Count > 0)
            {
                SelectedBillItem = _billItem[0];
            }

            OnPropertyChange("BillItems");
            OnPropertyChange("BillSumValue");
            OnPropertyChange("BillCount");
        }
    }
}
