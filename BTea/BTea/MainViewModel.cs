using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
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
            _sugarItems.Add(new SugarItemData(3, "0%"));
            _selectedSugarItem = _sugarItems[0];

            _iceItems = new List<IceItemData>();
            _iceItems.Add(new IceItemData(0, "")); //100%
            _iceItems.Add(new IceItemData(1, "70%"));
            _iceItems.Add(new IceItemData(2, "50%"));
            _iceItems.Add(new IceItemData(3, "30%"));
            _iceItems.Add(new IceItemData(3, "0%"));
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
        public string OrderPrice { set; get; } // full price (include *number)
        public string OrderNote { set; get; }
        public string OrderKm { set; get; }
        public string OrderKmType { set; get; }

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
            int fullPrice = 0;
            int number = TConst.ConvertInt(OrderNum);

            if (_orderObject.Type == BTBaseObject.BTeaType.DRINK_TYPE)
            {
                int drPrice = 0;
                DrinkObject drObj = _orderObject as DrinkObject;
                string slPrice = ConfigurationManager.AppSettings["lprice"].ToString();

                int lPrice = TConst.ConvertMoney(slPrice);
                if (lPrice == 0)
                    lPrice = 10000;

                if (drObj != null)
                {
                    drPrice = drObj.BPrice;
                    if (drObj.DrinkSize == 1) // SIZE L
                    {
                        drPrice += lPrice;
                    }

                    int tpPrice = 0;
                    for (int ii = 0; ii < drObj.TPListObj.Count; ++ii)
                    {
                        int price = drObj.TPListObj[ii].BPrice;
                        tpPrice += price;
                    }

                    drPrice += tpPrice;
                    fullPrice = drPrice * number;
                }
            }
            else
            {
                fullPrice = _orderObject.BPrice * number;
            }

            int kmValue = TConst.ConvertMoney(OrderKm);
            if (OrderKmType == "%")
            {
                int offVal = fullPrice * kmValue / 100;
                fullPrice = fullPrice - offVal;
            }
            else
            {
                fullPrice = fullPrice - kmValue;
            }

            OrderPrice = fullPrice.ToString(TConst.K_MONEY_FORMAT);
            return fullPrice;
        }

        public string MakePrintDescription()
        {
            string strDes = "";
            BTBaseObject.BTeaType type = _orderObject.Type;
            if (type == BTBaseObject.BTeaType.DRINK_TYPE)
            {
                DrinkObject drObj = (DrinkObject)_orderObject;
                if (drObj != null)
                {
                    string sizeStr = drObj.SizeToString();
                    string SuStr = drObj.SugarToString();
                    string IceStr = drObj.IceToString();
                    string strNote = "";
                    strNote += "\nSize: " + sizeStr + "\n";
                    if (SuStr != string.Empty)
                    {
                        strNote += "Đường: " + SuStr + "\n";
                    }

                    if (IceStr != string.Empty)
                    { 
                       strNote += "Đá: " + IceStr + "\n";
                    }
                    string strTopping = drObj.ToppingToStringSingleLine();
                    strDes = _orderObject.BName + strNote + strTopping;
                }
            }
            else
            {
                strDes = _orderObject.BName;
            }

            return strDes;
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
                    DrinkObject pThis = _orderObject as DrinkObject;
                    DrinkObject pObj = obj.OrderObject as DrinkObject;

                    if (pThis.DrinkSize == pObj.DrinkSize && 
                        pThis.SugarRate == pObj.SugarRate &&
                        pThis.IceRate == pObj.IceRate)
                    {
                        int n1 = pThis.TPListObj.Count;
                        int n2 = pObj.TPListObj.Count;
                        if (n1 == n2)
                        {
                            bool bCheckTopping = true;
                            for (int j = 0; j < n1; j++)
                            {
                                if (pThis.TPListObj[j].BId != pObj.TPListObj[j].BId)
                                {
                                    bCheckTopping = false;
                                    break;
                                }
                            }

                            if (bCheckTopping == true)
                            {
                                bCheck2 = true;
                            }
                            else
                            {
                                bCheck2 = false;
                            }
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


            if (bCheck1 == true && bCheck2 == true)
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
            DrinkCmd = new RelayCommand(new Action<object>(DoDrink));
            ToppingCmd = new RelayCommand(new Action<object>(DoTopping));
            FoodCmd = new RelayCommand(new Action<object>(DoFood));
            OtherFoodCmd = new RelayCommand(new Action<object>(DoOtherFood));
            BillCmd = new RelayCommand(new Action<object>(DoBill));
            PrintBillCmd = new RelayCommand(new Action<object>(PrintBill));
            RevenueCmd = new RelayCommand(new Action<object>(DoRevenue));
            CmdSelectItem = new RelayCommand(new Action<object>(DoSelectItem));
            MakeBillCmd = new RelayCommand(new Action<object>(DoMakeBill));
            ClearBillCmd = new RelayCommand(new Action<object>(DoClearBill));
            OrderItemMinusCmd = new RelayCommand(new Action<object>(DoMinusOrderItem));
            OrderItemPlusCmd = new RelayCommand(new Action<object>(DoPlusOrderItem));
            EditOrderItemCmd = new RelayCommand(new Action<object>(DoEditOderItem));
            RemoveOrderItemCmd = new RelayCommand(new Action<object>(DoRemoveOderItem));

            //Command for file menu
            FileAboutCmd  = new RelayCommand(new Action<object>(DoAboutMenu));
            FileSettingCmd = new RelayCommand(new Action<object>(DoSettingMenu));
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
            _sugarItems.Add(new SugarItemData(3, "0%"));
            _selectedSugarItem = _sugarItems[0];

            _iceItems = new List<IceItemData>();
            _iceItems.Add(new IceItemData(0, "")); //100%
            _iceItems.Add(new IceItemData(1, "70%"));
            _iceItems.Add(new IceItemData(2, "50%"));
            _iceItems.Add(new IceItemData(3, "30%"));
            _iceItems.Add(new IceItemData(3, "0%"));

            _selectedIceItem = _iceItems[0];

            _billPrice = "0.0000";
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

                int sumPrice = 0;
                for (int i = 0; i < _dataOrderList.Count; i++)
                {
                    BTeaOrderItems orderItem = _dataOrderList[i];
                    string strPrice = orderItem.OrderPrice;
                    try
                    {
                        int oPrice = TConst.ConvertMoney(strPrice);
                        sumPrice += oPrice;
                    }
                    catch
                    {
                        sumPrice = 0;
                    }
                }
                BillSumPrice = sumPrice.ToString(TConst.K_MONEY_FORMAT);
            }
            else
            {
                IsEnableOrderItem = false;
                BillSumPrice = "0.0000";
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
            double sumPrice = 0.0;
            for (int i = 0; i < _dataOrderList.Count; i++)
            {
                BTeaOrderItems orderItem = _dataOrderList[i];
                string strPrice = orderItem.OrderPrice;
                double oPrice = Convert.ToDouble(strPrice);
                sumPrice += oPrice;
            }

            if ( _kmTotalType == TConst.K_KM_VND)
            {
                if (sumPrice > 0)
                {
                    double billPriceEnd = sumPrice - _kMSumBill;
                    if (billPriceEnd < 0)
                    {
                        billPriceEnd = 0;
                    }
                    BillSumPrice = billPriceEnd.ToString(TConst.K_MONEY_FORMAT);
                }
            }
            else
            {
                if (_kMSumBill >= 0 && _kMSumBill <= 100)
                {
                    if (sumPrice > 0)
                    {
                        double value_off = sumPrice * _kMSumBill / 100.0;
                        double billPriceEnd = sumPrice - value_off;
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
            OnPropertyChange("DataOrderList");

            double sumPrice = 0.0;
            for (int i = 0; i < _dataOrderList.Count; i++)
            {
                BTeaOrderItems orderItem = _dataOrderList[i];
                string strPrice = orderItem.OrderPrice;
                double oPrice = Convert.ToDouble(strPrice);
                sumPrice += oPrice;
            }
            _billPrice = sumPrice.ToString(TConst.K_MONEY_FORMAT);
            OnPropertyChange("BillSumPrice");
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

        public void PrintBill(object obj)
        {
            if (_dataOrderList.Count <= 0)
            {
                MessageBox.Show("Danh sach trong", "Thong bao", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            FrmPrintBill printDlg = new FrmPrintBill();
            FrmPrintBillVM _printVM = new FrmPrintBillVM();
            int totalNumber = 0;
            int totalNumDrink = 0;
            double totalPriceDrink = 0;

            for (int i = 0; i < _dataOrderList.Count; ++i)
            {
                BTeaOrderItems orderItem = _dataOrderList[i];
                PrintBillIData dataItem = new PrintBillIData();
                dataItem.NumberProduct = orderItem.OrderNum;
                dataItem.SumPrice = orderItem.OrderPrice;
                dataItem.NameProduct = orderItem.MakePrintDescription();
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
            }

            if (_printVM.PrintBillItemsDrink.Count == 0)
            {
                _printVM.SetHideDrinkBill();
            }

            _printVM.SetInfo(_billName, _billTableNumber, _billCreator, 
                            _billStartDate.ToString(), _billPrice,
                            totalNumber.ToString());

            _printVM.SetInfoDrink(totalNumDrink, totalPriceDrink);
            printDlg.DataContext = _printVM;
            printDlg.ShowDialog();

            string strContent = "Hoa Don nay ban chua luu vao DataBase.\n Ban co muon luu khong?";
            string strInfo = "Thong bao";
            MessageBoxResult msg = MessageBox.Show(strContent, strInfo, MessageBoxButton.YesNo, MessageBoxImage.Question);
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
                MessageBox.Show("Danh sach trong", "Thông báo",
                               MessageBoxButton.OK, 
                               MessageBoxImage.Information);
                return;
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
            if (_kmItemType == TConst.K_KM_VND)
            {
                billObject.KMType = TConst.K_KM_VND;
            }
            else
            {
                billObject.KMType = TConst.K_KM_VND;
            }
            billObject.BillTableNumber = _billTableNumber;

            bool bAddItemOrder = false;
            for (int i = 0; i < _dataOrderList.Count; i++)
            {
                BTBaseObject orderBaseObj = _dataOrderList[i].OrderObject;
                BTeaOrderObject orderObject = new BTeaOrderObject();
                orderObject.BOrderName = orderBaseObj.BName;
                orderObject.BOrderPrice = _dataOrderList[i].MakeSummaryPrice();
                orderObject.BOrderNum = TConst.ConvertInt(_dataOrderList[i].OrderNum);
                orderObject.Type = orderBaseObj.Type;
                orderObject.BOrderIdItem = orderBaseObj.BId;
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

                orderObject.BOrderBillId = _billName;
                orderObject.BOrderDate = _billStartDate;
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

                bool bRet2 = DBConnection.GetInstance().AddOrderItem(orderObject);
                if (bRet2 == false)
                {
                    bAddItemOrder = false;
                    break;
                }
                else
                {
                    bAddItemOrder = true;
                }
            }

            if (bAddItemOrder == true)
            {
                string strOrderItem = "";
                List<BTeaOrderObject> listOrder = DBConnection.GetInstance().GetDataOrderObject();
                for (int i = 0; i < listOrder.Count; i++)
                {
                    if (listOrder[i].BOrderBillId == _billName)
                    {
                        strOrderItem = strOrderItem + listOrder[i].BOrderId + ",";
                    }
                }

                billObject.BillOrderItem = strOrderItem;
                bool bRet = DBConnection.GetInstance().AddBillItem(billObject);
                if (bRet == true)
                {
                    MessageBox.Show("Thành công!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    CreateBillName();
                    OnPropertyChange("BillName");
                }
                else
                {
                    MessageBox.Show("Thất bại", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            DataOrderList.Clear();
            BillPhone = "";
            BillAddress = "";
            BillNote = "";
            BillSumPrice = "0.000";
            BillStartDate = DateTime.Now;

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

            OrderItemNum = "1";
            OrderItemKM = "0";
            KMPercentType = true;
            KMVNDType = false;
        }

        public void DoClearBill(object obj)
        {
            if (_dataOrderList.Count > 0)
            {
                MessageBox.Show("Bạn có chắc chắn xóa hóa đơn?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                BillAddress = "";
                BillPhone = "";
                BillNote = "";
                BillSumPrice = "0.000";
                BillTableNumber = "";
                DataOrderList.Clear();
                IsEnableOrderItem = false;
                KMSumBill = "0";
            }
            else
            {
                MessageBox.Show("Danh sach trong", "Thông báo", MessageBoxButton.OK,MessageBoxImage.Information);
            }
        }

        public BTBaseObject MakeOrderObject()
        {
            BTBaseObject btObject = null;
            if (_drinkCheck == true)
            {
                List<DrinkObject> baseList = DBConnection.GetInstance().GetDataDrink();
                
                DrinkObject drObject = new DrinkObject();
                drObject.BId = _bTeaSelectedItem.ImgId;
                drObject.BName = _bTeaSelectedItem.Name;

                string id = drObject.BId.Replace("DR", "");
                DrinkObject baseObj = baseList.Find(x => x.BId == id);
                if (baseObj != null)
                {
                    drObject.BPrice = baseObj.BPrice;
                }

                drObject.BNote = _bTeaSelectedItem.Note;
                drObject.DrinkSize = SelectedSizeItem.Index;
                drObject.SugarRate = SelectedSugarItem.Index;
                drObject.IceRate = SelectedIceItem.Index;

                List<ToppingObject> dataTopping = DBConnection.GetInstance().GetDataTopping();
                int nCountTopping = ToppingItems.Count;
                for (int i = 0; i < nCountTopping; ++i)
                {
                    ToppingItemCheck checkTp = ToppingItems[i];
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
                                drObject.TPListObj.Add(newTpObj);
                            }
                        }
                    }
                }

                btObject = drObject;
            }
            else if (_foodCheck == true)
            {
                FoodObject fObject = new FoodObject();
                fObject.BId = _bTeaSelectedItem.ImgId;
                fObject.BName = _bTeaSelectedItem.Name;
                fObject.BPrice = TConst.ConvertMoney(_bTeaSelectedItem.Price);
                fObject.BNote = _bTeaSelectedItem.Note;
                btObject = fObject;
            }
            else if (_foodOtherCheck == true)
            {
                OtherFoodObject ofObject = new OtherFoodObject();
                ofObject.BId = _bTeaSelectedItem.ImgId;
                ofObject.BName = _bTeaSelectedItem.Name;
                ofObject.BPrice = TConst.ConvertMoney(_bTeaSelectedItem.Price);
                ofObject.BNote = _bTeaSelectedItem.Note;
                btObject = ofObject;
            }
            else if (_toppingCheck == true)
            {
                ToppingObject tpObject = new ToppingObject();
                tpObject.BId = _bTeaSelectedItem.ImgId;
                tpObject.BName = _bTeaSelectedItem.Name;
                tpObject.BPrice = TConst.ConvertMoney(_bTeaSelectedItem.Price);
                tpObject.BNote = _bTeaSelectedItem.Note;
                btObject = tpObject;
            }

            return btObject;
        }
        public void DoSelectItem(object obj)
        {
            if (_bTeaSelectedItem == null)
                return;
            BTBaseObject btObject = MakeOrderObject();

            //Make order item;
            BTeaOrderItems orderObjItem = new BTeaOrderItems();

            orderObjItem.OrderName = btObject.BName;
            orderObjItem.OrderNum = _orderItemNum.ToString();
            orderObjItem.OrderObject = btObject;
            orderObjItem.OrderKm = _orderItemKM.ToString(TConst.K_MONEY_FORMAT);
            if (_kmItemType == TConst.K_KM_PERCENT)
            {
                orderObjItem.OrderKmType = "%";
            }
            else if (_kmItemType == TConst.K_KM_VND)
            {
                orderObjItem.OrderKmType = " ";
            }
            else
            {
                orderObjItem.OrderKmType = "%";
            }

            orderObjItem.MakeSummaryPrice();
            orderObjItem.MakeNoteSumary();

            bool bCheckExist = false;
            int idxDuplicate = -1;
            for (int ik = 0; ik < _dataOrderList.Count; ik++)
            {
                BTeaOrderItems orderItem = _dataOrderList[ik];
                bCheckExist = orderItem.CheckDuplicate(orderObjItem);
                if (bCheckExist == true)
                {
                    idxDuplicate = ik;
                    break;
                }
            }

            if (bCheckExist == false)
            {
                _dataOrderList.Add(orderObjItem);
                _bteaOderItem = _dataOrderList[0];
                //Calculate summary of price
                int sumPrice = 0;
                for (int i = 0; i < _dataOrderList.Count; i++)
                {
                    BTeaOrderItems orderItem = _dataOrderList[i];
                    string strPrice = orderItem.OrderPrice;
                    try
                    {
                        int oPrice = TConst.ConvertMoney(strPrice);
                        sumPrice += oPrice;
                    }
                    catch
                    {
                        sumPrice = 0;
                    }
                }
                _billPrice = sumPrice.ToString(TConst.K_MONEY_FORMAT);

                IsEnableOrderItem = true;
                //Update GUI
                OnPropertyChange("DataOrderList");
                OnPropertyChange("BTeaOrderSelectedItem");
                OnPropertyChange("BillSumPrice");
            }
            else
            {
                string content = "Thông tin sản phẩm này đã được order!.\nBạn có muốn tăng thêm số lượng?";
                MessageBoxResult msg = MessageBox.Show(content, "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (msg == MessageBoxResult.Yes)
                {
                    try
                    {
                        int nNumber = Convert.ToInt32(orderObjItem.OrderNum);
                        double nPrice = Convert.ToDouble(orderObjItem.OrderPrice);

                        BTeaOrderItems orderItem = _dataOrderList[idxDuplicate];
                        int tNumber = Convert.ToInt32(orderItem.OrderNum);
                        double tPrice = Convert.ToDouble(orderItem.OrderPrice);

                        orderItem.OrderNum = (nNumber + tNumber).ToString();
                        orderItem.OrderPrice = (nPrice + tPrice).ToString(TConst.K_MONEY_FORMAT);

                        double currentBillPrice = Convert.ToDouble(_billPrice);
                        currentBillPrice += nPrice;

                        CollectionViewSource.GetDefaultView(_dataOrderList).Refresh();
                        OnPropertyChange("DataOrderList");

                        _billPrice = currentBillPrice.ToString(TConst.K_MONEY_FORMAT);
                        OnPropertyChange("BillSumPrice");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi hệ thống!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            
        }

        public ObservableCollection<BTeaItem> GetDataList()
        {
            _dataList.Clear();
            _originDataList.Clear();
            Tlog.GetInstance().WriteLog("Lay du lieu Order");
            if (_drinkCheck == true)
            {
                List<DrinkObject> data_list = DBConnection.GetInstance().GetDataDrink();
                for (int i = 0; i < data_list.Count; ++i)
                {
                    BTeaItem bItem = new BTeaItem();
                    DrinkObject drObject = data_list[i];
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
                    bItem.ImgId = "F" + tpObject.BId;
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
                    if (item.Name.Contains(_findItem))
                    {
                        findList.Add(item);
                    }
                }

                foreach (BTeaItem item in findList)
                {
                    _dataList.Add(item);
                }
                findList.Clear();

                if (_dataList.Count > 0)
                {
                    _bTeaSelectedItem = _dataList[0];
                    OnPropertyChange("BTeaSelectedItem");
                }
                OnPropertyChange("DataList");
            }
        }

        public void CreateBillName()
        {
            Tlog.GetInstance().WriteLog("Start: Tao ten Hoa Don.");
            List<BillObject> data_list = DBConnection.GetInstance().GetDataBillObject();
            int billIdx = 1;
            if (data_list.Count > 0)
            {
                billIdx = data_list.Count + 1;
            }
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
                OnPropertyChange("KMVNDType");
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
                OnPropertyChange("KMPercentType");
                _orderItemKM = 0;
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
            set { _isEnableOrderItem = value; OnPropertyChange("IsEnableOrderItem"); }
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

        public string OrderItemNum
        {
            get { return _orderItemNum.ToString(); }
            set
            {
                int nCount = Convert.ToInt32(value);
                _orderItemNum = nCount;
                if (nCount < 1)
                {
                    _orderItemNum = 1;
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
            get { return _billPrice; }
            set { _billPrice = value; OnPropertyChange("BillSumPrice"); }
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
            set { _bteaOderItem = value; OnPropertyChange("BTeaOrderSelectedItem"); }
        }
        public DateTime BillStartDate
        {
            get { return _billStartDate; }
            set
            { _billStartDate = value;
                OnPropertyChange("BillStartDate");
            }
        }

        public string BillName {
            get { return _billName; }
            set { _billName = value; OnPropertyChange("BillName"); }
        }

        public string BillTableNumber
        {
            get { return _billTableNumber; }
            set { _billTableNumber = value; OnPropertyChange("BillTableNumber"); }
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
                _bTeaSelectedItem = value;
                OnPropertyChange("BTeaSelectedItem");
            }
        }
        public bool DrinkCheck
        {
            get { return _drinkCheck; }
            set
            {
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
