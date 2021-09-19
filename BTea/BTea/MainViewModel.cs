using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using TApp.Base;

namespace BTea
{
    class SizeItemData
    {
        public SizeItemData(int idx, string content)
        {
            Index = idx;
            Content = content;
        }
        public int Index { set; get; }
        public string Content { set; get; }
    }

    class SugarItemData
    {
        public SugarItemData(int idx, string content)
        {
            Index = idx;
            Content = content;
        }
        public int Index { set; get; }
        public string Content { set; get; }
    }

    class IceItemData
    {
        public IceItemData(int idx, string content)
        {
            Index = idx;
            Content = content;
        }
        public int Index { set; get; }
        public string Content { set; get; }
    }

    class BTeaOrderItems
    {
        public BTeaOrderItems()
        {
            OrderName = "";
            OrderPrice = "";
            OrderNote = "";
            OrderNum = "1";

            _orderObject = null;

            _sizeItems = new List<SizeItemData>();
            _sizeItems.Add(new SizeItemData(0, "M"));
            _sizeItems.Add(new SizeItemData(0, "L"));
            _selectedSizeItem = _sizeItems[0];

            _sugarItems = new List<SugarItemData>();
            _sugarItems.Add(new SugarItemData(0, "")); // 100%
            _sugarItems.Add(new SugarItemData(1, "70%"));
            _sugarItems.Add(new SugarItemData(2, "50%"));
            _sugarItems.Add(new SugarItemData(3, "30%"));
            _sugarItems.Add(new SugarItemData(4, "0%"));
            _selectedSugarItem = _sugarItems[0];

            _iceItems = new List<IceItemData>();
            _iceItems.Add(new IceItemData(0, "")); //100%
            _iceItems.Add(new IceItemData(1, "70%"));
            _iceItems.Add(new IceItemData(2, "50%"));
            _iceItems.Add(new IceItemData(3, "30%"));
            _iceItems.Add(new IceItemData(4, "0%"));
            _selectedIceItem = _iceItems[0];
        }

        private BTBaseObject _orderObject;
        public BTBaseObject OrderObject
        {
            get { return _orderObject; }
            set { _orderObject = value; }
        }

        public string OrderName { set; get; }
        public string OrderNum { set; get; }

        private List<SizeItemData> _sizeItems;
        public List<SizeItemData> SizeItems
        {
            get { return _sizeItems; }
            set { _sizeItems = value;}
        }

        private SizeItemData _selectedSizeItem;
        public SizeItemData SelectedSizeItem
        {
            get { return _selectedSizeItem; }
            set { _selectedSizeItem = value; }
        }

        private List<SugarItemData> _sugarItems;
        public List<SugarItemData> SugarItems
        {
            get { return _sugarItems; }
            set { _sugarItems = value; }
        }

        private SugarItemData _selectedSugarItem;
        public SugarItemData SelectedSugarItem
        {
            get { return _selectedSugarItem; }
            set { _selectedSugarItem = value; }
        }

        private List<IceItemData> _iceItems;
        public List<IceItemData> IceItems
        {
            get { return _iceItems; }
            set { _iceItems = value; }
        }

        private IceItemData _selectedIceItem;
        public IceItemData SelectedIceItem
        {
            get { return _selectedIceItem; }
            set { _selectedIceItem = value; }
        }

        public int OrderId { set; get; }
        public string OrderBasePrice { set; get; } // with drink, include size L
        public string OrderPrice { set; get; } // full price (include *number)
        public string DrinkPriceNoToping { set; get; }
        public string OrderNote { set; get; }
        public string OrderKm { set; get; }
        public string OrderKmType { set; get; }

        public int GetKmType()
        {
            int type = TConst.K_KM_PERCENT;
            if (OrderKmType == " ")
            {
                type = TConst.K_KM_VND;
            }
            return type;
        }
        public int GetKmValue()
        {
            int val = 0;
            int kmType = GetKmType();
            if (kmType == TConst.K_KM_PERCENT)
            {
                val = TConst.ConvertInt(OrderKm);
            }
            else
            {
                val = TConst.ConvertMoney(OrderKm);
            }

            return val;
        }

        public void MakeNoteSumary()
        {
            BTBaseObject.BTeaType type = _orderObject.Type;
            if (type == BTBaseObject.BTeaType.DRINK_TYPE)
            {
                DrinkObject drObj = (DrinkObject)_orderObject;
                if (drObj != null)
                {
                    string sizeStr  = drObj.SizeToString();
                    string SuStr    = drObj.SugarToString();
                    string IceStr   = drObj.IceToString();
                    string tpContent = drObj.ToppingToString();

                    string strNote = "";
                    strNote += "Size: " + sizeStr + " ";
                    if (SuStr != string.Empty)
                    {
                        strNote += "Đường: " + SuStr + " ";
                    }
                    
                    if (IceStr != string.Empty)
                    {
                        strNote += "Đá: " + IceStr + " ";
                    }
                    
                    if (tpContent != string.Empty)
                    {
                        strNote += "\nTopping: " + tpContent;
                    }
                    this.OrderNote = strNote;
                }
            }
        }
        public int MakeSummaryPrice()
        {
            int sumPrice = 0;
            int number = TConst.ConvertInt(OrderNum);
            DrinkPriceNoToping = "";
            if (_orderObject.Type == BTBaseObject.BTeaType.DRINK_TYPE)
            {
                DrinkObject drObj = _orderObject as DrinkObject;
                if (drObj != null)
                {
                    // Get price of size L
                    string slPrice = ConfigurationManager.AppSettings["lprice"].ToString();
                    int lPrice = TConst.ConvertMoney(slPrice);

                    int dPrice = drObj.BPrice;
                    if (drObj.DrinkSize == TConst.K_SIZE_L)
                    {
                        dPrice += lPrice;
                    }
                    OrderBasePrice = dPrice.ToString(TConst.K_MONEY_FORMAT);

                    // Offset sumPrice with KM value
                    int kmValue = TConst.ConvertMoney(OrderKm);
                    if (OrderKmType == "%")
                    {
                        int offVal = dPrice * kmValue / 100;
                        dPrice = dPrice - offVal;
                    }
                    else
                    {
                        dPrice = dPrice - kmValue;
                    }

                    DrinkPriceNoToping = (dPrice*number).ToString(TConst.K_MONEY_FORMAT);

                    // Get price of Topping Items
                    for (int ii = 0; ii < drObj.TPListObj.Count; ++ii)
                    {
                        int price = drObj.TPListObj[ii].BPrice;
                        dPrice += price;
                    }

                    sumPrice = dPrice * number;
                }
            }
            else
            {
                int sPrice = _orderObject.BPrice;
                // Offset sumPrice with KM value
                int kmValue = TConst.ConvertMoney(OrderKm);
                if (OrderKmType == "%")
                {
                    int offVal = sPrice * kmValue / 100;
                    sPrice = sPrice - offVal;
                }
                else
                {
                    sPrice = sPrice - kmValue;
                }

                sumPrice = sPrice * number;
            }

            OrderPrice = sumPrice.ToString(TConst.K_MONEY_FORMAT);
            return sumPrice;
        }

        // Using for print bill
        public string MakePrintDescription()
        {
            string strDes = "";
            BTBaseObject.BTeaType type = _orderObject.Type;
            if (type == BTBaseObject.BTeaType.DRINK_TYPE)
            {
                DrinkObject dObj = (DrinkObject)_orderObject;
                if (dObj != null)
                {
                    string szS = dObj.SizeToString();
                    string sugaS = dObj.SugarToString();
                    string IceS = dObj.IceToString();
                    string strNote = "";
                    strNote += "\nSize: " + szS + "\n";
                    if (sugaS != string.Empty)
                    {
                        strNote += "Đường: " + sugaS + "\n";
                    }

                    if (IceS != string.Empty)
                    { 
                       strNote += "Đá: " + IceS + "\n";
                    }

                    //string strTopping = dObj.ToppingToStringSingleLine();
                    strDes = _orderObject.BName + strNote;
                }
            }
            else
            {
                strDes = _orderObject.BName;
            }

            return strDes;
        }
        public string MakeKMPrint()
        {
            string str;
            if (GetKmValue() == 0)
            {
                str = string.Empty;
                return str;
            }

            str = GetKmValue().ToString();
            if (GetKmType() == TConst.K_KM_PERCENT)
            {
                str += "%";
            }
            else
            {
                str += "đ";
            }

            return str;
        }

        public bool CheckDuplicate(BTeaOrderItems obj)
        {
            bool bDuplicate = false;

            bool bCheck1 = false;
            if (this.OrderName == obj.OrderName &&
                this.OrderNote == obj.OrderNote &&
                this.OrderKmType == obj.OrderKmType &&
                this.OrderKm == obj.OrderKm)
            {
                bCheck1 = true;
            }

            bool bCheck2 = false;
            if (_orderObject.Type == obj.OrderObject.Type)
            {
                if (_orderObject.Type == BTBaseObject.BTeaType.DRINK_TYPE)
                {
                    DrinkObject pOne = _orderObject as DrinkObject;
                    DrinkObject pTwo = obj.OrderObject as DrinkObject;

                    if (pOne.DrinkSize == pTwo.DrinkSize && 
                        pOne.SugarRate == pTwo.SugarRate &&
                        pOne.IceRate == pTwo.IceRate)
                    {
                        int n1 = pOne.TPListObj.Count;
                        int n2 = pTwo.TPListObj.Count;
                        if (n1 == n2)
                        {
                            bool bCheckTopping = true;
                            for (int j = 0; j < n1; j++)
                            {
                                if (pOne.TPListObj[j].BId != pTwo.TPListObj[j].BId)
                                {
                                    bCheckTopping = false;
                                    break;
                                }
                            }

                            bCheck2 = bCheckTopping;
                        }
                    }
                }
                else
                {
                    if (_orderObject.BId == obj.OrderObject.BId)
                    {
                        bCheck2 = true;
                    }
                }
            }

            if (bCheck1 && bCheck2 )
            {
                bDuplicate = true;
            }
            return bDuplicate;
        }
    }

    class ToppingItemCheck
    {
        public int Id { set; get; }
        public bool IsSelected { set; get; }
        public string Content { set; get; }
    }

    class MainViewModel : TBaseVM
    {
        public MainViewModel()
        {
            _statusBarText = "Ready";
            _frmWaitOrder = null;
            _frmWaitVM = null;

            DrinkCmd = new RelayCommand(new Action<object>(DoDrink));
            ToppingCmd = new RelayCommand(new Action<object>(DoTopping));
            FoodCmd = new RelayCommand(new Action<object>(DoFood));
            OtherFoodCmd = new RelayCommand(new Action<object>(DoOtherFood));
            BillCmd = new RelayCommand(new Action<object>(DoBill));
            WaitBillCmd = new RelayCommand(new Action<object>(DoWaitBill));
            PrintBillCmd = new RelayCommand(new Action<object>(PrintBill));
            RevenueCmd = new RelayCommand(new Action<object>(DoRevenue));
            CmdSelectItem = new RelayCommand(new Action<object>(DoSelectItem));
            MakeBillCmd = new RelayCommand(new Action<object>(DoMakeBill));
            ClearBillCmd = new RelayCommand(new Action<object>(DoClearBill));
            SaveBillCmd = new RelayCommand(new Action<object>(DoSaveBill));
            OrderItemMinusCmd = new RelayCommand(new Action<object>(DoMinusOrderItem));
            OrderItemPlusCmd = new RelayCommand(new Action<object>(DoPlusOrderItem));
            EditOrderItemCmd = new RelayCommand(new Action<object>(DoEditOderItem));
            RemoveOrderItemCmd = new RelayCommand(new Action<object>(DoRemoveOderItem));

            //Command for file menu
            FileAboutCmd  = new RelayCommand(new Action<object>(DoAboutMenu));
            FileSettingCmd = new RelayCommand(new Action<object>(DoSettingMenu));
            FileBackupDBCmd = new RelayCommand(new Action<object>(DoBackupDB));
            FileCloseCmd = new RelayCommand(new Action<object>(DoCloseMenu));
            FileEB1Cmd = new RelayCommand(new Action<object>(DoEbook1Menu));
            FileEB2Cmd = new RelayCommand(new Action<object>(DoEbook2Menu));
            FileEB3Cmd = new RelayCommand(new Action<object>(DoEbook3Menu));

            _kmVNDType = false;
            _kmPercentType = true;
            _kmTotalVNDType = false;
            _kmTotalPercentType = true;

            _orderItemKM = 0;
            _kMSumBill = 0;

            _drinkCheck = true;
            _foodCheck = false;
            _toppingCheck = false;
            _foodOtherCheck = false;

            _isEnableOrderItem = false;

            _dataList = new ObservableCollection<BTeaItem>();
            _originDataList = new ObservableCollection<BTeaItem>();
            _bTeaSelectedItem = null;

            _dataList = GetDataList();
            if (_drinkCheck == true)
            {
                _stateSize = true;
                _stateSugarRate = true;
                _stateTopping = true;
                _stateIceRate = true;
            }
            else
            {
                _stateSize = false;
                _stateSugarRate = false;
                _stateTopping = false;
                _stateIceRate = false;
            }

            _sizeItems = new List<SizeItemData>();
            _sizeItems.Add(new SizeItemData(0, "M"));
            _sizeItems.Add(new SizeItemData(1, "L"));

            _selectedSizeItem = _sizeItems[0];

            _sugarItems = new List<SugarItemData>();
            _sugarItems.Add(new SugarItemData(0, "")); // 100%
            _sugarItems.Add(new SugarItemData(1, "70%"));
            _sugarItems.Add(new SugarItemData(2, "50%"));
            _sugarItems.Add(new SugarItemData(3, "30%"));
            _sugarItems.Add(new SugarItemData(4, "0%"));
            _selectedSugarItem = _sugarItems[0];

            _iceItems = new List<IceItemData>();
            _iceItems.Add(new IceItemData(0, "")); //100%
            _iceItems.Add(new IceItemData(1, "70%"));
            _iceItems.Add(new IceItemData(2, "50%"));
            _iceItems.Add(new IceItemData(3, "30%"));
            _iceItems.Add(new IceItemData(4, "0%"));

            _selectedIceItem = _iceItems[0];

            _billPrice = "0.0000";
            _billPriceNoKM = _billPrice;

            _toppingItemList = new List<ToppingItemCheck>();

            Tlog.GetInstance().WriteLog("Start:Get topping Order Drink");

            List<ToppingObject> data_topping = DBConnection.GetInstance().GetDataTopping();
            for (int i = 0; i < data_topping.Count; ++i)
            {
                ToppingObject toppingObj = data_topping[i];
                if (toppingObj != null)
                {
                    ToppingItemCheck item = new ToppingItemCheck();

                    item.Id = Convert.ToInt32(toppingObj.BId);
                    item.IsSelected = false;
                    item.Content = toppingObj.BName;
                    _toppingItemList.Add(item);
                }
            }

            Tlog.GetInstance().WriteLog("End: Get topping Order Drink");

            _billMoreInfo = false;
            _billCreator = ConfigurationManager.AppSettings["shopname"].ToString();
            _billStartDate = DateTime.Now;
            _statusBarDateInfo = "Ngày: " + _billStartDate.ToString("dd-MM-yyyy");

            _dataOrderList = new ObservableCollection<BTeaOrderItems>();

            _orderItemNum = 1;
            _stateMinus = false;
            CreateBillName();
        }

        public void DoRemoveOderItem(object obj)
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
                UpdateTotalPriceByKM();
            }
            else
            {
                IsEnableOrderItem = false;
                BillSumPrice = "0.0000";
                BillSumPriceNoKM = "0.0000";
                KMSumBill = "0.0";
            }

            OnPropertyChange("DataOrderList");
            OnPropertyChange("BTeaOrderSelectedItem");
        }

        public void DoEditOderItem(object obj)
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

        #region Member
        FrmOrderItemSingleVM _frmItemSingleVM;
        FrmOrderItemSingle _frmItemSingle;
        public RelayCommand DrinkCmd { set; get; }
        public RelayCommand FoodCmd { set; get; }
        public RelayCommand OtherFoodCmd { set; get; }
        public RelayCommand ToppingCmd { set; get; }
        public RelayCommand BillCmd { set; get; }
        public RelayCommand WaitBillCmd { set; get; }
        public RelayCommand PrintBillCmd { set; get; }
        public RelayCommand RevenueCmd { set; get; }
        public RelayCommand CmdSelectItem { set; get; }
        public RelayCommand MakeBillCmd { set; get; }
        public RelayCommand ClearBillCmd { set; get; }
        public RelayCommand SaveBillCmd { set; get; }

        public RelayCommand OrderItemMinusCmd { set; get; }
        public RelayCommand OrderItemPlusCmd { set; get; }

        public RelayCommand EditOrderItemCmd { set; get; }
        public RelayCommand RemoveOrderItemCmd { set; get; }

        public RelayCommand FileAboutCmd { set; get; }
        public RelayCommand FileSettingCmd { set; get; }
        public RelayCommand FileBackupDBCmd { set; get; }
        public RelayCommand FileCloseCmd { set; get; }
        public RelayCommand FileEB1Cmd { set; get; }
        public RelayCommand FileEB2Cmd { set; get; }
        public RelayCommand FileEB3Cmd { set; get; }

        private int _orderItemNum;
        private FrmDrinkMainVM _frmDrinkVm;
        private FrmToppingMainVM _frmToppingVm;
        private FrmFoodMainVM _frmFoodVM;
        private FrmOtherFoodMainVM _frmOtherFoodVM;
        private FrmBillMainVM _frmBillVm;
        private FrmWaitOrderVM _frmWaitVM;
        private FrmWaitOrder _frmWaitOrder;
        private FrmOrderBTeaItemVM _frmOderVM;

        private ObservableCollection<BTeaItem> _dataList;
        private ObservableCollection<BTeaItem> _originDataList;
        private BTeaItem _bTeaSelectedItem;
        private bool _drinkCheck;
        private bool _foodCheck;
        private bool _toppingCheck;
        private bool _foodOtherCheck;
        private string _findItem;

        private bool _stateSize;
        private bool _stateSugarRate;
        private bool _stateIceRate;
        private bool _stateTopping;
        private bool _stateMinus;

        private List<SizeItemData> _sizeItems;
        private List<SugarItemData> _sugarItems;
        private List<IceItemData> _iceItems;
        private List<ToppingItemCheck> _toppingItemList;

        private bool _billMoreInfo;
        private string _billName;
        private string _billPrice;
        private string _billPriceNoKM;
        private string _billCreator;
        private DateTime _billStartDate;
        private string _billTableNumber;
        private string _billPhone;
        private string _billAddress;
        private string _billNote;

        private ObservableCollection<BTeaOrderItems> _dataOrderList;
        private BTeaOrderItems _bteaOderItem;

        private string _statusBarText;
        private string _statusBarDateInfo { set; get; }

        private bool _isEnableOrderItem;

        private bool _kmVNDType;
        private bool _kmPercentType;
        private bool _kmTotalVNDType;
        private bool _kmTotalPercentType;

        private int _orderItemKM;
        private int _kMSumBill;
        private int _kmItemType;
        private int _kmTotalType;
        #endregion

        #region Method
        void UpdateTotalPriceByKM()
        {
            int sumPrice = 0;
            for (int i = 0; i < _dataOrderList.Count; i++)
            {
                int oPrice = TConst.ConvertMoney(_dataOrderList[i].OrderPrice);
                sumPrice += oPrice;
            }

            BillSumPriceNoKM = sumPrice.ToString(TConst.K_MONEY_FORMAT);
            if (_kmTotalType == TConst.K_KM_VND)
            {
                int billPriceEnd = sumPrice - _kMSumBill;
                if (billPriceEnd < 0)
                {
                    billPriceEnd = 0;
                }
                BillSumPrice = billPriceEnd.ToString(TConst.K_MONEY_FORMAT);
            }
            else
            {
                if (_kMSumBill >= 0 && _kMSumBill <= 100)
                {
                    if (sumPrice > 0)
                    {
                        int value_off = sumPrice * _kMSumBill / 100;
                        int billPriceEnd = sumPrice - value_off;
                        if (billPriceEnd < 0)
                        {
                            billPriceEnd = 0;
                        }
                        BillSumPrice = billPriceEnd.ToString(TConst.K_MONEY_FORMAT);
                    }
                }
            }
        }
        public void DoAboutMenu(object obj)
        {

        }
        public void DoSettingMenu(object obj)
        {
            BSetting settingDlg = new BSetting();
            BSettingVM _settingVM = new BSettingVM();
            settingDlg.DataContext = _settingVM;
            settingDlg.ShowDialog();
        }

        public void DoBackupDB(object obj)
        {
            string _mServer = ConfigurationManager.AppSettings["server"].ToString();
            string _mDBName = ConfigurationManager.AppSettings["dbname"].ToString();
            string _mUserName = ConfigurationManager.AppSettings["username"].ToString();
            string _mPassword = ConfigurationManager.AppSettings["password"].ToString();
            string _mPort = ConfigurationManager.AppSettings["port"].ToString();

            string connectionString = "server=" + _mServer +
                               ";user=" + _mUserName +
                               ";pwd=" + _mPassword +
                               ";database=" + _mDBName;

            string path = Environment.CurrentDirectory;
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            string file = path + "\\backup_db.sql";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        try
                        {
                            cmd.Connection = conn;
                            conn.Open();
                            mb.ExportToFile(file);
                            conn.Close();
                            TConst.MsgInfo("Backup Successful!");
                            Process.Start("explorer.exe", path);
                        }
                        catch(Exception ex)
                        {
                            string str = "Backup DB failed. Error = ";
                            str += ex.Message;
                            Tlog.GetInstance().WriteLog(str);
                        }
                    }
                }
            }
        }

        public void DoCloseMenu(object obj)
        {
            Application.Current.Shutdown();
        }

        public void DoEbook1Menu(object obj)
        {
            // Sinh to
            string path = Environment.CurrentDirectory;
            path += "\\ebook\\Ebook_KT_Sinh_To.pdf";
            System.Diagnostics.Process.Start(path);
        }
        public void DoEbook2Menu(object obj)
        {
            // Cocktail
            string path = Environment.CurrentDirectory;
            path += "\\ebook\\EBook_460_Cocktail.pdf";
            System.Diagnostics.Process.Start(path);
        }
        public void DoEbook3Menu(object obj)
        {
            //Tra sua 1
            string path = Environment.CurrentDirectory;
            path += "\\ebook\\Ebook_Tra_Sua_1.pdf";
            System.Diagnostics.Process.Start(path);
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
                    DrinkObject dObj = obj as DrinkObject;
                    if (dObj != null)
                    {
                        dObj.DrinkSize = _frmItemSingleVM.SelectedSizeItem.Index;
                        dObj.IceRate = _frmItemSingleVM.SelectedIceItem.Index;
                        dObj.SugarRate = _frmItemSingleVM.SelectedSugarItem.Index;

                        // Update topping data
                        dObj.TPListObj.Clear();

                        List<ToppingObject> dbTopping = DBConnection.GetInstance().GetDataTopping();
                        int nCountTop = _frmItemSingleVM.SingleToppingItems.Count;
                        for (int i = 0; i < nCountTop; ++i)
                        {
                            ToppingItemCheck topCheck = _frmItemSingleVM.SingleToppingItems[i];
                            if (topCheck.IsSelected == true)
                            {
                                for (int j = 0; j < dbTopping.Count; j++)
                                {
                                    ToppingObject dbObj = dbTopping[j];
                                    if (topCheck.Id.ToString() == dbObj.BId)
                                    {
                                        dObj.TPListObj.Add(dbObj);
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
            OnPropertyChange("DataOrderList");

            UpdateTotalPriceByKM();
        }

        public void DoMinusOrderItem(object obj)
        {
            if (_orderItemNum > 1)
            {
                _orderItemNum--;
                if (_orderItemNum <= 1)
                {
                    _stateMinus = false;
                }
                OnPropertyChange("OrderItemNum");
                OnPropertyChange("StateMinus");
            }
        }

        public void DoPlusOrderItem(object obj)
        {
            _orderItemNum++;
            if (_orderItemNum > 1)
            {
                _stateMinus = true;
            }
            
            OnPropertyChange("OrderItemNum");
             OnPropertyChange("StateMinus");
        }

        public void DoDrink(object obj)
        {
            FrmDrinkMain frmDrinkMain = new FrmDrinkMain();
            _frmDrinkVm = new FrmDrinkMainVM();
            frmDrinkMain.DataContext = _frmDrinkVm;
            frmDrinkMain.ShowDialog();
        }

        public void DoFood(object obj)
        {
            FrmFoodMain frmfoodMain = new FrmFoodMain();
            _frmFoodVM = new FrmFoodMainVM();
            frmfoodMain.DataContext = _frmFoodVM;
            frmfoodMain.ShowDialog();
        }

        public void DoOtherFood(object obj)
        {
            FrmOtherFoodMain frmOtherfoodMain = new FrmOtherFoodMain();
            _frmOtherFoodVM = new FrmOtherFoodMainVM();
            frmOtherfoodMain.DataContext = _frmOtherFoodVM;
            frmOtherfoodMain.ShowDialog();
        }

        public void DoTopping(object obj)
        {
            FrmToppingMain frmToppingMain = new FrmToppingMain();
            _frmToppingVm = new FrmToppingMainVM();
            frmToppingMain.DataContext = _frmToppingVm;
            frmToppingMain.ShowDialog();
        }

        public void DoBill(object obj)
        {
            FrmBillMain frmBillMain = new FrmBillMain();
            _frmBillVm = new FrmBillMainVM();
            frmBillMain.DataContext = _frmBillVm;
            frmBillMain.ShowDialog();
        }

        public void DoWaitBill(object obj)
        {
             _frmWaitOrder = new FrmWaitOrder();
            _frmWaitVM = new FrmWaitOrderVM(ReOrderItemCmd);
            _frmWaitOrder.DataContext = _frmWaitVM;
            _frmWaitOrder.ShowDialog();
        }

        public void ReOrderItemCmd()
        {
            _frmWaitOrder.Close();
            _dataOrderList.Clear();

            string billName = _frmWaitVM.GetBillName();
            string billPrice = _frmWaitVM.GetBillPrice();

            List<BTeaOrderObject> bWaitOrderItemList = _frmWaitVM.GetListOrderItem();

            for (int i = 0; i < bWaitOrderItemList.Count; i++)
            {
                BTeaOrderItems bItem = new BTeaOrderItems();
                BTeaOrderObject bOrderObj = bWaitOrderItemList[i];
                
                bItem.OrderObject = bOrderObj.MakeObject();
                bItem.OrderName = bOrderObj.BOrderName;
                bItem.OrderNum = bOrderObj.BOrderNum.ToString();
                bItem.OrderKm = bOrderObj.BOrderKm.ToString(TConst.K_MONEY_FORMAT);
                if (bOrderObj.BOrderKmType == TConst.K_KM_VND)
                {
                    bItem.OrderKmType = " ";
                }
                else
                {
                    bItem.OrderKmType = "%";
                }
                bItem.MakeSummaryPrice();
                bItem.MakeNoteSumary();

                _dataOrderList.Add(bItem);
            }

            if (_dataOrderList.Count > 0)
            {
                _bteaOderItem = _dataOrderList[0];
            }
            
            int sumPrice = 0;
            for (int i = 0; i < _dataOrderList.Count; i++)
            {
                string sPrice = _dataOrderList[i].OrderPrice;
                sumPrice += TConst.ConvertMoney(sPrice);
            }

            _billPrice = sumPrice.ToString(TConst.K_MONEY_FORMAT);
            _billPriceNoKM = _billPrice;
            IsEnableOrderItem = true;

            //Update GUI
            OnPropertyChange("DataOrderList");
            OnPropertyChange("BTeaOrderSelectedItem");
            OnPropertyChange("BillSumPrice");
            OnPropertyChange("BillSumPriceNoKM");
        }

        public void PrintBill(object obj)
        {
            if (_dataOrderList.Count <= 0)
            {
                TConst.MsgInfo(TConst.K_S_LIST_EMPTY);
                return;
            }

            FrmPrintBill printDlg = new FrmPrintBill();
            FrmPrintBillVM _printVM = new FrmPrintBillVM();
            int totalNumber = 0;
#if PRINT_DRINK
            int totalNumDrink = 0;
            double totalPriceDrink = 0;
#endif
            for (int i = 0; i < _dataOrderList.Count; ++i)
            {
                BTeaOrderItems orderItem = _dataOrderList[i];
                PrintBillIData dataItem = new PrintBillIData();
                dataItem.NumberProduct = orderItem.OrderNum;
                bool bDrink = false;
                if (orderItem.OrderObject.Type == BTBaseObject.BTeaType.DRINK_TYPE)
                {
                    bDrink = true;
                }

                if (bDrink)
                { 
                    dataItem.SumPrice = orderItem.DrinkPriceNoToping;
                }
                else
                {
                    dataItem.SumPrice = orderItem.OrderPrice;
                }
                
                dataItem.NameProduct = orderItem.MakePrintDescription();
                if (orderItem.OrderObject != null)
                {
                    int basePrice = orderItem.OrderObject.BPrice;
                    if (bDrink)
                    {
                        DrinkObject drObj = orderItem.OrderObject as DrinkObject;
                        if (drObj != null)
                        {
                            if (drObj.DrinkSize == 1)
                            {
                                string slPrice = ConfigurationManager.AppSettings["lprice"].ToString();
                                int lPrice = TConst.ConvertMoney(slPrice);
                                basePrice += lPrice;
                                dataItem.BasePriceProduct = basePrice.ToString(TConst.K_MONEY_FORMAT);
                            }
                        }
                    }
                    else
                    {
                        dataItem.BasePriceProduct = orderItem.OrderObject.BPrice.ToString(TConst.K_MONEY_FORMAT);
                    }
                    
                }
                dataItem.KMProduct = orderItem.MakeKMPrint();

                int orderNum = TConst.ConvertInt(orderItem.OrderNum);
                totalNumber += orderNum;
                _printVM.AddData(dataItem);

                //  Add price of topping list
                if (bDrink)
                {
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


#if PRINT_DRINK
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
#endif
            }

#if PRINT_DRINK
            if (_printVM.PrintBillItemsDrink.Count == 0)
            {
                _printVM.SetHideDrinkBill();
            }
            _printVM.SetInfoDrink(totalNumDrink, totalPriceDrink);
#endif
            string billKm = _kMSumBill.ToString();
            if (billKm == "0")
            {
                billKm = "";
            }
            else
            {
                if (_kmTotalType == TConst.K_KM_PERCENT)
                {
                    billKm += "%";
                }
                else
                {
                    billKm += "đ";
                }
            }
            

            _printVM.SetInfo(_billName, _billTableNumber, _billCreator, 
                            _billStartDate.ToString(), billKm, _billPrice,
                            totalNumber.ToString());
            printDlg.DataContext = _printVM;
            printDlg.ShowDialog();

            string sCap = "Hóa đơn chưa lưu vào DB.\nBạn có muốn lưu không ?";
            MessageBoxResult msg = TConst.MsgYNQ(sCap);
            if (msg == MessageBoxResult.Yes)
            {
                DoMakeBill(obj);
            }
        }

        public void DoRevenue(object obj)
        {
            FrmOrderBTeaItem frmOrderItem = new FrmOrderBTeaItem();
            _frmOderVM = new FrmOrderBTeaItemVM();
            frmOrderItem.DataContext = _frmOderVM;
            frmOrderItem.ShowDialog();
        }

        public void DoMakeBill(object obj)
        {
            if (_dataOrderList.Count == 0)
            {
                TConst.MsgInfo(TConst.K_S_LIST_EMPTY);
                return;
            }

            bool bAddItemOrder = false;
            for (int i = 0; i < _dataOrderList.Count; i++)
            {
                // Include information of OrderObject
                BTBaseObject bObj = _dataOrderList[i].OrderObject;

                // write to data base
                BTeaOrderObject bOrderObj = new BTeaOrderObject();

                bOrderObj.BOrderPrice   = _dataOrderList[i].MakeSummaryPrice();
                bOrderObj.BOrderNum     = TConst.ConvertInt(_dataOrderList[i].OrderNum);
                bOrderObj.BOrderName    = bObj.BName;
                bOrderObj.Type          = bObj.Type;
                bOrderObj.BOrderIdItem  = bObj.BId;
                bOrderObj.BOrderBillName  = _billName;
                bOrderObj.BOrderDate    = _billStartDate;
                bOrderObj.BOrderKmType  = _dataOrderList[i].GetKmType();
                bOrderObj.BOrderKm      = _dataOrderList[i].GetKmValue();

                if (bObj.Type == BTBaseObject.BTeaType.DRINK_TYPE)
                {
                    DrinkObject dObj = bObj as DrinkObject;
                    if (dObj != null)
                    {
                        bOrderObj.BOrderSize = dObj.DrinkSize;
                        bOrderObj.BOrderSugarRate = dObj.SugarRate;
                        bOrderObj.BOrderIceRate = dObj.IceRate;
                        bOrderObj.BOrderTopping = dObj.ToppingIDToString();
                    }
                }

                bAddItemOrder = DBConnection.GetInstance().AddOrderItem(bOrderObj);
                if (bAddItemOrder == false)
                {
                    break;
                }
            }

            if (bAddItemOrder)
            {
                string strOrderItem = "";
                List<BTeaOrderObject> dbOrderList = DBConnection.GetInstance().GetDataOrderObject();
                for (int i = 0; i < dbOrderList.Count; i++)
                {
                    if (dbOrderList[i].BOrderBillName == _billName)
                    {
                        strOrderItem = strOrderItem + dbOrderList[i].BOrderId + ",";
                    }
                }

                BillObject billObject = new BillObject();
                billObject.BillName = _billName;
                billObject.BillPrice = TConst.ConvertMoney(_billPrice);
                billObject.BillCreator = _billCreator;
                billObject.BillDate = _billStartDate;
                billObject.BillPhone = _billPhone;
                billObject.BillAddress = _billAddress;
                billObject.BillNote = _billNote;
                billObject.KMValue = _kMSumBill;
                billObject.BillTableNumber = _billTableNumber;
                billObject.KMType = _kmItemType;
                billObject.BillOrderItem = strOrderItem;
                bool bAddBill = DBConnection.GetInstance().AddBillItem(billObject);

                if (bAddBill == true)
                {
                    TConst.MsgInfo("Thành công!");
                    CreateBillName();
                    OnPropertyChange("BillName");
                }
                else
                {
                    TConst.MsgError("Thất bại!");
                    return;
                }
            }

            //Reset data then make bill
            DataOrderList.Clear();
            BillPhone = BillAddress = BillNote = "";
            BillSumPrice = "0.0000";
            BillSumPriceNoKM = "0.0000";
            BillTableNumber = "";
            KMSumBill = "0";
            KMTotalVNDType = false;
            KMTotalPercentType = true;
            BillMoreInfo = false;

            BillStartDate = DateTime.Now;
        }

        public void ResetOrderInfo()
        {
            //Reset topping order table
            List<ToppingItemCheck> tempList = new List<ToppingItemCheck>();
            for (int i = 0; i < _toppingItemList.Count; ++i)
            {
                _toppingItemList[i].IsSelected = false;
                tempList.Add(_toppingItemList[i]);
            }
            _toppingItemList.Clear();
            ToppingItems = tempList;

            SelectedSugarItem = _sugarItems[0];
            SelectedIceItem = _iceItems[0];
            SelectedSizeItem = _sizeItems[0];

            OrderItemNum = 1;
            OrderItemKM = "0";
            KMPercentType = true;
            KMVNDType = false;
        }

        public void DoClearBill(object obj)
        {
            if (_dataOrderList.Count > 0)
            {
                string caption = "Bạn có chắc chắn xóa hóa đơn?";
                MessageBoxResult msg = TConst.MsgYNQ(caption);
                if (msg == MessageBoxResult.Yes)
                {
                    BillAddress = BillPhone = BillNote= "";
                    BillTableNumber = "";
                    IsEnableOrderItem = false;
                    DataOrderList.Clear();
                    KMSumBill = "0";
                    BillSumPrice = "0.0000";
                    BillSumPriceNoKM = "0.0000";
                    BillMoreInfo = false;
                }
            }
            else
            {
                TConst.MsgInfo(TConst.K_S_LIST_EMPTY);
            }
        }

        public void DoSaveBill(object obj)
        {
            if (_dataOrderList.Count == 0)
            {
                TConst.MsgInfo(TConst.K_S_LIST_EMPTY);
                return;
            }

            bool bAddItemOrder = false;
            for (int i = 0; i < _dataOrderList.Count; i++)
            {
                // Include information of OrderObject
                BTBaseObject bObj = _dataOrderList[i].OrderObject;

                // write to data base
                BTeaOrderObject bOrderObj = new BTeaOrderObject();

                bOrderObj.BOrderPrice = _dataOrderList[i].MakeSummaryPrice();
                bOrderObj.BOrderNum = TConst.ConvertInt(_dataOrderList[i].OrderNum);
                bOrderObj.BOrderName = bObj.BName;
                bOrderObj.Type = bObj.Type;
                bOrderObj.BOrderIdItem = bObj.BId;
                bOrderObj.BOrderBillName = _billName;
                bOrderObj.BOrderDate = _billStartDate;
                bOrderObj.BOrderKmType = _dataOrderList[i].GetKmType();
                bOrderObj.BOrderKm = _dataOrderList[i].GetKmValue();

                if (bObj.Type == BTBaseObject.BTeaType.DRINK_TYPE)
                {
                    DrinkObject dObj = bObj as DrinkObject;
                    if (dObj != null)
                    {
                        bOrderObj.BOrderSize = dObj.DrinkSize;
                        bOrderObj.BOrderSugarRate = dObj.SugarRate;
                        bOrderObj.BOrderIceRate = dObj.IceRate;
                        bOrderObj.BOrderTopping = dObj.ToppingIDToString();
                    }
                }

                bAddItemOrder = DBConnection.GetInstance().AddWaitItem(bOrderObj);
                if (bAddItemOrder == false)
                {
                    break;
                }
            }

            if (bAddItemOrder)
            {
                string strOrderItem = "";
                List<BTeaOrderObject> dbOrderList = DBConnection.GetInstance().GetWaitItemObject();
                for (int i = 0; i < dbOrderList.Count; i++)
                {
                    if (dbOrderList[i].BOrderBillName == _billName)
                    {
                        strOrderItem = strOrderItem + dbOrderList[i].BOrderId + ",";
                    }
                }

                BillObject billObject = new BillObject();
                billObject.BillName = _billName;
                billObject.BillPrice = TConst.ConvertMoney(_billPrice);
                billObject.BillOrderItem = strOrderItem;
                bool bAddBill = DBConnection.GetInstance().AddWaitBillItem(billObject);

                if (bAddBill == true)
                {
                    TConst.MsgInfo("Thành công!");
                }
                else
                {
                    TConst.MsgError("Thất bại!");
                }
            }

            //Reset data then make bill
            DataOrderList.Clear();
            BillPhone = BillAddress = BillNote = "";
            BillSumPrice = "0.0000";
            BillSumPriceNoKM = "0.0000";
            BillStartDate = DateTime.Now;
        }

        public BTBaseObject MakeOrderObject()
        {
            BTBaseObject btObject = null;
            if (_drinkCheck == true)
            {
                List<DrinkObject> drinkDb = DBConnection.GetInstance().GetDataDrink();
                DrinkObject drObject = new DrinkObject();

                // Get Name;
                drObject.BId = _bTeaSelectedItem.ImgId;
                drObject.BName = _bTeaSelectedItem.Name;

                // Find object in database
                string id = drObject.BId.Replace("DR", "");
                DrinkObject dbObj = drinkDb.Find(x => x.BId == id);
                if (dbObj != null)
                {
                    // get origin price
                    drObject.BPrice = dbObj.BPrice;
                }

                // Get size, sugar, ice.
                drObject.BNote = _bTeaSelectedItem.Note;
                drObject.DrinkSize = SelectedSizeItem.Index;
                drObject.SugarRate = SelectedSugarItem.Index;
                drObject.IceRate = SelectedIceItem.Index;

                // Get topping info
                List<ToppingObject> dbTopping = DBConnection.GetInstance().GetDataTopping();

                // Find checked item in db
                for (int i = 0; i < ToppingItems.Count; ++i)
                {
                    ToppingItemCheck topItem = ToppingItems[i];
                    if (topItem.IsSelected == true)
                    {
                        for (int j = 0; j < dbTopping.Count; j++)
                        {
                            ToppingObject topObj = dbTopping[j];
                            if (topItem.Id.ToString() == topObj.BId)
                            {
                                // Get topping
                                drObject.TPListObj.Add(topObj);
                            }
                        }
                    }
                }

                btObject = drObject;
            }
            else
            {
                if (_foodCheck == true)
                {
                    btObject = new FoodObject();
                }
                else if (_foodOtherCheck == true)
                {
                    btObject = new OtherFoodObject();
                }
                else if (_toppingCheck == true)
                {
                    btObject = new ToppingObject();
                }

                btObject.BId = _bTeaSelectedItem.ImgId;
                btObject.BName = _bTeaSelectedItem.Name;
                btObject.BPrice = TConst.ConvertMoney(_bTeaSelectedItem.Price);
                btObject.BNote = _bTeaSelectedItem.Note;
            }

            return btObject;
        }
        public void DoSelectItem(object obj)
        {
            if (_bTeaSelectedItem == null)
                return;

            BTBaseObject bObj = MakeOrderObject();

            //Make order item;
            BTeaOrderItems bItem = new BTeaOrderItems();

            bItem.OrderObject   = bObj;
            bItem.OrderName     = bObj.BName;
            bItem.OrderNum      = _orderItemNum.ToString();
            bItem.OrderKm       = _orderItemKM.ToString(TConst.K_MONEY_FORMAT);
            if (_kmItemType == TConst.K_KM_VND)
            {
                bItem.OrderKmType = " ";
            }
            else
            {
                bItem.OrderKmType = "%";
            }

            bItem.MakeSummaryPrice();
            bItem.MakeNoteSumary();

            bool bExist = false;
            int idxExist = -1;
            for (int ik = 0; ik < _dataOrderList.Count; ik++)
            {
                BTeaOrderItems orderItem = _dataOrderList[ik];
                bExist = orderItem.CheckDuplicate(bItem);
                if (bExist == true)
                {
                    idxExist = ik;
                    break;
                }
            }

            if (bExist == false)
            {
                _dataOrderList.Add(bItem);
                _bteaOderItem = _dataOrderList[0];

                UpdateTotalPriceByKM();

                IsEnableOrderItem = true;

                //Update GUI
                OnPropertyChange("DataOrderList");
                OnPropertyChange("BTeaOrderSelectedItem");
                OnPropertyChange("BillSumPrice");
                OnPropertyChange("BillSumPriceNoKM");
            }
            else
            {
                string content = "Sản phẩm đã tồn tại.\nClick YES để tăng thêm số lượng.";
                MessageBoxResult msg = TConst.MsgYNQ(content);
                if (msg == MessageBoxResult.Yes)
                {
                    // get new number of new item
                    int nNumber = TConst.ConvertInt(bItem.OrderNum);
                    int nPrice = TConst.ConvertMoney(bItem.OrderPrice);

                    BTeaOrderItems orderItem = _dataOrderList[idxExist];
                    int tNumber = TConst.ConvertInt(orderItem.OrderNum);
                    int tPrice = TConst.ConvertMoney(orderItem.OrderPrice);

                    orderItem.OrderNum = (nNumber + tNumber).ToString();
                    orderItem.OrderPrice = (nPrice + tPrice).ToString(TConst.K_MONEY_FORMAT);

                    _dataOrderList[idxExist] = orderItem;

                    // Calculate sumary of price again.
                    UpdateTotalPriceByKM();

                    CollectionViewSource.GetDefaultView(_dataOrderList).Refresh();
                    OnPropertyChange("DataOrderList");
                    OnPropertyChange("BillSumPrice");
                    OnPropertyChange("BillSumPriceNoKM");
                }
            }
            ResetOrderInfo();
        }

        public ObservableCollection<BTeaItem> GetDataList()
        {
            _dataList.Clear();
            _originDataList.Clear();
            Tlog.GetInstance().WriteLog("Lay du lieu Order");
            if (_drinkCheck == true)
            {
                List<DrinkObject> dbDrinkList = DBConnection.GetInstance().GetDataDrink();
                for (int i = 0; i < dbDrinkList.Count; ++i)
                {
                    BTeaItem bItem = new BTeaItem();

                    DrinkObject drObject = dbDrinkList[i];
                    bItem.ImgId = "DR" + drObject.BId;
                    bItem.Name = drObject.BName;
                    bItem.Price = drObject.BPrice.ToString(TConst.K_MONEY_FORMAT);
                    bItem.Note = drObject.BNote;

                    _dataList.Add(bItem);
                    _originDataList.Add(bItem);
                }
            }

            if (_foodCheck == true)
            {
                List<FoodObject> data_list = DBConnection.GetInstance().GetDataFood();
                for (int i = 0; i < data_list.Count; ++i)
                {
                    BTeaItem bItem = new BTeaItem();
                    FoodObject fObject = data_list[i];
                    bItem.ImgId = "F" + fObject.BId;
                    bItem.Name = fObject.BName;
                    bItem.Price = fObject.BPrice.ToString(TConst.K_MONEY_FORMAT);
                    bItem.Note = fObject.BNote;

                    _dataList.Add(bItem);
                    _originDataList.Add(bItem);
                }
            }

            if (_foodOtherCheck == true)
            {
                List<OtherFoodObject> data_list = DBConnection.GetInstance().GetDataOtherFood();
                for (int i = 0; i < data_list.Count; ++i)
                {
                    BTeaItem bItem = new BTeaItem();
                    OtherFoodObject fObject = data_list[i];
                    bItem.ImgId = "OF" + fObject.BId;
                    bItem.Name = fObject.BName;
                    bItem.Price = fObject.BPrice.ToString(TConst.K_MONEY_FORMAT);
                    bItem.Note = fObject.BNote;

                    _dataList.Add(bItem);
                    _originDataList.Add(bItem);
                }
            }

            if (_toppingCheck == true)
            {
                List<ToppingObject> data_list = DBConnection.GetInstance().GetDataTopping();
                for (int i = 0; i < data_list.Count; ++i)
                {
                    BTeaItem bItem = new BTeaItem();
                    ToppingObject tpObject = data_list[i];
                    bItem.ImgId = "TP" + tpObject.BId;
                    bItem.Name = tpObject.BName;
                    bItem.Price = tpObject.BPrice.ToString(TConst.K_MONEY_FORMAT);
                    bItem.Note = tpObject.BNote;

                    _dataList.Add(bItem);
                    _originDataList.Add(bItem);
                }
            }

            if (_dataList.Count > 0)
            {
                _bTeaSelectedItem = _dataList[0];
            }

            Tlog.GetInstance().WriteLog("Ket thuc lay du lieu Order");
            return _dataList;
        }

        public void UpdateItemList()
        {
            _dataList.Clear();
            if (_findItem == string.Empty)
            {
                foreach (BTeaItem item in _originDataList)
                {
                    _dataList.Add(item);
                }

                if (_dataList.Count > 0)
                {
                    BTeaSelectedItem = _dataList[0];
                }
                OnPropertyChange("DataList");
            }
            else
            {
                List<BTeaItem> findList = new List<BTeaItem>();
                for (int i = 0; i < _originDataList.Count; i++)
                {
                    BTeaItem item = _originDataList[i];
                    string sName = item.Name;
                    if (sName.ToUpper().Contains(_findItem.ToUpper()))
                    {
                        findList.Add(item);
                    }
                }

                foreach (BTeaItem item in findList)
                {
                    _dataList.Add(item);
                }

                if (_dataList.Count > 0)
                {
                    BTeaSelectedItem = _dataList[0];
                }

                OnPropertyChange("DataList");
                findList.Clear();
            }
        }

        public void CreateBillName()
        {
            Tlog.GetInstance().WriteLog("Start: Tao ten Hoa Don.");

            List<BillObject> dbBill = DBConnection.GetInstance().GetDataBillObject();
            int billIdx = dbBill.Count + 1;

            _billName = "HOA_DON_" + billIdx.ToString();

            string strDay = _billStartDate.Day.ToString();
            string strMonth = _billStartDate.Month.ToString();
            string strYear = _billStartDate.Year.ToString();
            _billName += "_" + strDay + strMonth + strYear;

            Tlog.GetInstance().WriteLog("End: Tao ten Hoa Don.");
        }

#endregion

#region Property

        public bool KMVNDType
        {
            get { return _kmVNDType; }
            set
            {
                _kmVNDType = value;
                if (_kmVNDType == true)
                {
                    _kmItemType = TConst.K_KM_VND;
                }

                _orderItemKM = 0;
                OnPropertyChange("KMVNDType");
                OnPropertyChange("OrderItemKM");
            }
        }

        public bool KMPercentType
        {
            get { return _kmPercentType; }
            set
            {
                _kmPercentType = value;
                if (_kmPercentType == true)
                {
                    _kmItemType = TConst.K_KM_PERCENT;
                }

                _orderItemKM = 0;
                OnPropertyChange("KMPercentType");
                OnPropertyChange("OrderItemKM");
            }
        }

        public bool KMTotalVNDType
        {
            get { return _kmTotalVNDType; }
            set
            {
                _kmTotalVNDType = value;
                if (_kmTotalVNDType == true)
                {
                    _kmTotalType = TConst.K_KM_VND;
                }

                _kMSumBill = 0;
                OnPropertyChange("KMTotalVNDType");
                OnPropertyChange("KMSumBill");
                UpdateTotalPriceByKM();
            }
        }

        public bool KMTotalPercentType
        {
            get { return _kmTotalPercentType; }
            set
            {
                _kmTotalPercentType = value;
                if (_kmTotalPercentType == true)
                {
                    _kmTotalType = TConst.K_KM_PERCENT;
                }

                _kMSumBill = 0;
                OnPropertyChange("KMSumBill");
                OnPropertyChange("KMTotalPercentType");
                UpdateTotalPriceByKM();
            }
        }

        public string KMSumBill
        {
            get
            {
                if (_kmTotalType == TConst.K_KM_PERCENT)
                {
                    return _kMSumBill.ToString();
                }
                return _kMSumBill.ToString(TConst.K_MONEY_FORMAT);
            }
            set
            {
                int iVal = TConst.ConvertMoney(value);
                if (_kmTotalType == TConst.K_KM_PERCENT)
                {
                    if (iVal < 0 || iVal > 100)
                    {
                        _kMSumBill = 0;
                    }
                    else
                    {
                        _kMSumBill = iVal;
                    }
                }
                else
                {
                    _kMSumBill = iVal;
                }

                OnPropertyChange("KMSumBill");
                UpdateTotalPriceByKM();
            }
        }

        public string OrderItemKM
        {
            get
            {
                if (_kmItemType == TConst.K_KM_PERCENT)
                {
                    return _orderItemKM.ToString();
                }

                return _orderItemKM.ToString(TConst.K_MONEY_FORMAT);
            }
            set
            {
                int iVal = TConst.ConvertMoney(value);
                if (_kmItemType == TConst.K_KM_PERCENT)
                {
                    if (iVal < 0 || iVal > 100)
                    {
                        _orderItemKM = 0;
                    }
                    else
                    {
                        _orderItemKM = iVal;
                    }
                }
                else
                {
                    _orderItemKM = iVal;
                }
                
                OnPropertyChange("OrderItemKM");
            }
        }

        public bool IsEnableOrderItem
        {
            get { return _isEnableOrderItem; }
            set
            {
                _isEnableOrderItem = value;
                OnPropertyChange("IsEnableOrderItem");
            }
        }

        public bool StateMinus
        {
            get { return _stateMinus; }
            set
            {
                _stateMinus = value;
                OnPropertyChange("StateMinus");
            }
        }

        public int OrderItemNum
        {
            get { return _orderItemNum; }
            set
            {
                if (value < 1)
                {
                    _orderItemNum = 1;
                }
                else
                {
                    _orderItemNum = value;
                }

                OnPropertyChange("OrderItemNum");
            }
        }

        public string StatusBarDateInfo
        {
            get { return _statusBarDateInfo; }
            set { _statusBarDateInfo = value; OnPropertyChange("StatusBarDateInfo"); }
        }

        public string StatusBarText
        {
            get { return _statusBarText; }
            set { _statusBarText = value;  OnPropertyChange("StatusBarText"); }
        }
        public string BillNote
        {
            get { return _billNote; }
            set { _billNote = value; OnPropertyChange("BillNote"); }
        }

        public string BillAddress
        {
            get { return _billAddress; }
            set { _billAddress = value; OnPropertyChange("BillAddress"); }
        }

        public string BillPhone
        {
            get { return _billPhone; }
            set { _billPhone = value; OnPropertyChange("BillPhone"); }
        }
        public string BillSumPrice
        {
            get
            {
                return _billPrice;
            }
            set
            {
                _billPrice = value;
                OnPropertyChange("BillSumPrice");
            }
        }

        public string BillSumPriceNoKM
        {
            get
            {
                return _billPriceNoKM;
            }
            set
            {
                _billPriceNoKM = value;
                OnPropertyChange("BillSumPriceNoKM");
            }
        }

        public ObservableCollection<BTeaOrderItems> DataOrderList
        {
            get { return _dataOrderList; }
            set
            {
                _dataOrderList = value;
                OnPropertyChange("DataOrderList");
            }
        }

        public BTeaOrderItems BTeaOrderSelectedItem
        {
            get { return _bteaOderItem; }
            set
            {
                _bteaOderItem = value;
                OnPropertyChange("BTeaOrderSelectedItem");
            }
        }

        public DateTime BillStartDate
        {
            get { return _billStartDate; }
            set
            { _billStartDate = value;
                OnPropertyChange("BillStartDate");
            }
        }

        public string BillName
        {
            get { return _billName; }
            set { _billName = value; OnPropertyChange("BillName"); }
        }

        public string BillTableNumber
        {
            get { return _billTableNumber; }
            set
            {
                _billTableNumber = value;
                OnPropertyChange("BillTableNumber");
            }
        }
        public string BillCreator
        {
            get { return _billCreator; }
            set { _billCreator = value; OnPropertyChange("BillCreator"); }
        }

        public bool BillMoreInfo
        {
            get { return _billMoreInfo; }
            set { _billMoreInfo = value; OnPropertyChange("BillMoreInfo"); }
        }
        public List<ToppingItemCheck> ToppingItems
        {
            get { return _toppingItemList; }
            set
            {
                _toppingItemList = value;
                OnPropertyChange("ToppingItems");
            }
        }
        public List<SizeItemData> SizeItems
        {
            get { return _sizeItems; }
            set
            {
                _sizeItems = value;
                OnPropertyChange("SizeItems");
            }
        }

        public List<SugarItemData> SugarItems
        {
            get { return _sugarItems; }
            set
            {
                _sugarItems = value;
                OnPropertyChange("SugarItems");
            }
        }

        public List<IceItemData> IceItems
        {
            get { return _iceItems; }
            set
            {
                _iceItems = value;
                OnPropertyChange("IceItems");
            }
        }

        private SizeItemData _selectedSizeItem;
        public SizeItemData SelectedSizeItem
        {
            set
            {
                _selectedSizeItem = value;
                OnPropertyChange("SelectedSizeItem");
            }
            get
            {
                return _selectedSizeItem;
            }
        }

        private SugarItemData _selectedSugarItem;
        public SugarItemData SelectedSugarItem
        {
            set
            {
                _selectedSugarItem = value;
                OnPropertyChange("SelectedSugarItem");
            }
            get
            {
                return _selectedSugarItem;
            }
        }

        private IceItemData _selectedIceItem;
        public IceItemData SelectedIceItem
        {
            set
            {
                _selectedIceItem = value;
                OnPropertyChange("SelectedIceItem");
            }
            get
            {
                return _selectedIceItem;
            }
        }
        public bool StateSize
        {
            get { return _stateSize; }
            set
            {
                _stateSize = value;
                OnPropertyChange("StateSize");
            }
        }

        public bool StateSugarRate
        {
            get { return _stateSugarRate; }
            set
            {
                _stateSugarRate = value;
                OnPropertyChange("StateSugarRate");
            }
        }

        public bool StateIceRate
        {
            get { return _stateIceRate; }
            set
            {
                _stateIceRate = value;
                OnPropertyChange("StateIceRate");
            }
        }

        public bool StateTopping
        {
            get { return _stateTopping; }
            set
            {
                _stateTopping = value;
                OnPropertyChange("StateTopping");
            }
        }

        public string FindItem
        {
            get { return _findItem; }
            set
            {
                if (_findItem != value)
                {
                    _findItem = value;
                    OnPropertyChange("FindItem");
                    UpdateItemList();
                }
            }
        }
        public ObservableCollection<BTeaItem> DataList
        {
            get { return _dataList; }
            set
            {
                _dataList = value;
                OnPropertyChange("DataList");
            }
        }
        public BTeaItem BTeaSelectedItem
        {
            get { return _bTeaSelectedItem; }
            set
            {
                if (_bTeaSelectedItem != value)
                {
                    _bTeaSelectedItem = value;
                    OnPropertyChange("BTeaSelectedItem");
                    ResetOrderInfo();
                }
            }
        }
        public bool DrinkCheck
        {
            get { return _drinkCheck; }
            set
            {
                if (_drinkCheck == false && value == true)
                {
                    ResetOrderInfo();
                }

                _drinkCheck = value;
                if (value == true)
                {
                    _foodCheck = false;
                    _toppingCheck = false;
                    _foodOtherCheck = false;

                    StateSize = true;
                    StateSugarRate = true;
                    StateTopping = true;
                    StateIceRate = true;

                    GetDataList();
                }
                OnPropertyChange("DrinkCheck");
                OnPropertyChange("DataList");
            }
        }

        public bool FoodCheck
        {
            get { return _foodCheck; }
            set
            {
                if (_foodCheck == false && value == true)
                {
                    ResetOrderInfo();
                }

                _foodCheck = value;
                if (value == true)
                {
                    _drinkCheck = false;
                    _toppingCheck = false;
                    _foodOtherCheck = false;

                    StateSize = false;
                    StateSugarRate = false;
                    StateTopping = false;
                    StateIceRate = false;

                    GetDataList();
                }

                OnPropertyChange("FoodCheck");
                OnPropertyChange("DataList");
            }
        }

        public bool ToppingCheck
        {
            get { return _toppingCheck; }
            set
            {
                if (_toppingCheck == false && value == true)
                {
                    ResetOrderInfo();
                }

                _toppingCheck = value;
                if (value == true)
                {
                    _drinkCheck = false;
                    _foodCheck = false;
                    _foodOtherCheck = false;

                    StateSize = false;
                    StateSugarRate = false;
                    StateTopping = false;
                    StateIceRate = false;

                    GetDataList();
                }

                OnPropertyChange("ToppingCheck");
                OnPropertyChange("DataList");
            }
        }

        public bool FoodOtherCheck
        {
            get { return _foodOtherCheck; }
            set
            {
                if (_foodOtherCheck == false && value == true)
                {
                    ResetOrderInfo();
                }

                _foodOtherCheck = value;
                if (value == true)
                {
                    _drinkCheck = false;
                    _foodCheck = false;
                    _toppingCheck = false;

                    StateSize = false;
                    StateSugarRate = false;
                    StateTopping = false;
                    StateIceRate = false;

                    GetDataList();
                }

                OnPropertyChange("FoodOtherCheck");
                OnPropertyChange("DataList");
            }
        }
#endregion
    }
}
