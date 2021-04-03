using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TApp.Base;

namespace BTea
{

    class BTeaOrderItems
    {
        public BTeaOrderItems()
        {
            OrderName = "";
            OrderPrice = "";
            OrderNote = "";
        }
        public string OrderName { set; get; }
        public string OrderPrice { set; get; }
        public string OrderNote { set; get; }
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
            BillCmd = new RelayCommand(new Action<object>(DoBill));
            RevenueCmd = new RelayCommand(new Action<object>(DoRevenue));
            CmdSelectItem = new RelayCommand(new Action<object>(DoSelectItem));
            MakeBillCmd = new RelayCommand(new Action<object>(DoMakeBill));
            ClearBillCmd = new RelayCommand(new Action<object>(DoClearBill));
            SaveBillCmd = new RelayCommand(new Action<object>(DoSaveBill));
            _drinkCheck = true;
            _foodCheck = false;
            _toppingCheck = false;
            _foodOtherCheck = false;

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

            _sizeItems = new List<string>();
            _sizeItems.Add("M");
            _sizeItems.Add("L");

            SelectedSizeItem = _sizeItems[0];

            _sugarItems = new List<string>();
            _sugarItems.Add(" ");
            _sugarItems.Add("30%");
            _sugarItems.Add("50%");
            _sugarItems.Add("70%");
            SelectedSugarItem = _sugarItems[0];

            _iceItems = new List<string>();
            _iceItems.Add(" ");
            _iceItems.Add("30%");
            _iceItems.Add("50%");
            _iceItems.Add("70%");

            SelectedIceItem = _iceItems[0];

            _billPrice = "0.0000";
            _toppingItemList = new List<ToppingItemCheck>();
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

            _billMoreInfo = false;
            _billCreator = "BuiTea";
            _billStartDate = DateTime.Now;
            _statusBarDateInfo = "Ngày: " + _billStartDate.ToString("dd-MM-yyyy");

            _dataOrderList = new ObservableCollection<BTeaOrderItems>();
            _dataOrderObjectList = new List<BTBaseObject>();

            CreateBillName();
        }

        #region Member
        public RelayCommand DrinkCmd { set; get; }
        public RelayCommand ToppingCmd { set; get; }
        public RelayCommand BillCmd { set; get; }
        public RelayCommand RevenueCmd { set; get; }
        public RelayCommand CmdSelectItem { set; get; }
        public RelayCommand MakeBillCmd { set; get; }
        public RelayCommand ClearBillCmd { set; get; }
        public RelayCommand SaveBillCmd { set; get; }

        private FrmDrinkMainVM _frmDrinkVm;
        private FrmToppingMainVM _frmToppingVm;
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

        private List<string> _sizeItems;
        private List<string> _sugarItems;
        private List<string> _iceItems;
        private List<ToppingItemCheck> _toppingItemList;

        private bool _billMoreInfo;
        private string _billName;
        private string _billPrice;
        private string _billCreator;
        private DateTime _billStartDate;
        private string _billPhone;
        private string _billAddress;
        private string _billNote;

        private ObservableCollection<BTeaOrderItems> _dataOrderList;
        private List<BTBaseObject> _dataOrderObjectList;

        private string _statusBarText;
        private string _statusBarDateInfo { set; get; }
        #endregion

        #region Method
        public void DoDrink(object obj)
        {
            FrmDrinkMain frmDrinkMain = new FrmDrinkMain();
            _frmDrinkVm = new FrmDrinkMainVM();
            frmDrinkMain.DataContext = _frmDrinkVm;
            frmDrinkMain.ShowDialog();
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

        public void DoRevenue(object obj)
        {
            FrmOrderBTeaItem frmOrderItem = new FrmOrderBTeaItem();
            _frmOderVM = new FrmOrderBTeaItemVM();
            frmOrderItem.DataContext = _frmOderVM;
            frmOrderItem.ShowDialog();
        }

        public void DoMakeBill(object obj)
        {
            if (_dataOrderObjectList.Count == 0)
            {
                MessageBox.Show("Chưa có sản phẩm nào được order!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            BillObject billObject = new BillObject();
            billObject.BillName = _billName;
            billObject.BillPrice = Convert.ToDouble(_billPrice);
            billObject.BillCreator = _billCreator;
            billObject.BillDate = _billStartDate;
            billObject.BillPhone = _billPhone;
            billObject.BillAddress = _billAddress;
            billObject.BillNote = _billNote;

            string strOrderItem = "";
            bool bAddItemOrder = false;
            for (int i = 0; i < _dataOrderObjectList.Count; i++)
            {
                BTBaseObject orderBaseObj = _dataOrderObjectList[i];
                BTeaOrderObject orderObject = new BTeaOrderObject();
                orderObject.BOrderId = orderBaseObj.BId;
                orderObject.BOrderName = orderBaseObj.BName;
                orderObject.BOrderPrice = orderBaseObj.BPrice;

                strOrderItem = strOrderItem + orderObject.BOrderId + ",";
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

            billObject.BillOrderItem = strOrderItem;
            bool bRet = DBConnection.GetInstance().AddBillItem(billObject);

            if (bRet == true && bAddItemOrder == true)
            {
                MessageBox.Show("Thành công!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
                CreateBillName();
                OnPropertyChange("BillName");
            }
            else
            {
                MessageBox.Show("Thất bại", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            _dataOrderList.Clear();
            _billPhone = "";
            _billAddress = "";
            _billNote = "";
            _billPrice = "0.000";
            _billStartDate = DateTime.Now;
            OnPropertyChange("BillPhone");
            OnPropertyChange("BillAddress");
            OnPropertyChange("BillNote");
            OnPropertyChange("BillStartDate");
        }

        public void DoClearBill(object obj)
        {

        }

        public void DoSaveBill(object obj)
        {

        }

        public void DoSelectItem(object obj)
        {
            if (_bTeaSelectedItem == null)
                return;
            BTBaseObject btObject = null;
            if (_drinkCheck == true)
            {
                DrinkObject drObject = new DrinkObject();
                drObject.BId = _bTeaSelectedItem.ImgId;
                drObject.BName = _bTeaSelectedItem.Name;
                drObject.BPrice = Convert.ToDouble(_bTeaSelectedItem.Price);
                drObject.BNote = _bTeaSelectedItem.Note;
                if (SelectedSizeItem == "M")
                {
                    drObject.DrinkSize = 0;
                }
                else
                {
                    drObject.DrinkSize = 1;
                    drObject.BPrice += 10000;
                }


                if (SelectedSugarItem == "30%")
                {
                    drObject.SugarRate = 1;
                }
                else if (SelectedSugarItem == "50%")
                {
                    drObject.SugarRate = 2;
                }
                else if (SelectedSugarItem == "70%")
                {
                    drObject.SugarRate = 3;
                }
                else
                {
                    drObject.SugarRate = 0;
                }

                if (SelectedIceItem == "30%")
                {
                    drObject.IceRate = 1;
                }
                else if (SelectedIceItem == "50%")
                {
                    drObject.IceRate = 2;
                }
                else if (SelectedIceItem == "70%")
                {
                    drObject.IceRate = 3;
                }
                else
                {
                    drObject.IceRate = 0;
                }

                double toppingPriceSum = 0.0;
                List<ToppingObject> dataTopping = DBConnection.GetInstance().GetDataTopping();
                int nCountTopping = ToppingItems.Count;
                for (int i = 0; i < nCountTopping; ++i)
                {
                    ToppingItemCheck objTopping = ToppingItems[i];
                    if (objTopping.IsSelected == true)
                    {
                        ToppingObject tpObj = new ToppingObject();
                        tpObj.BId = objTopping.Id.ToString();
                        tpObj.BName = objTopping.Content;
                        drObject.TPListObj.Add(tpObj);

                        for (int ii = 0; ii < dataTopping.Count; ++ii)
                        {
                            if (tpObj.BId == dataTopping[ii].BId &&
                                tpObj.BName == dataTopping[ii].BName)
                            {
                                double price = dataTopping[ii].BPrice;
                                toppingPriceSum += price;
                            }
                        }
                    }
                }

                drObject.BPrice += toppingPriceSum;

                btObject = drObject;
            }
            else if (_foodCheck == true)
            {

            }
            else if (_foodOtherCheck == true)
            {

            }
            else if (_toppingCheck == true)
            {
                ToppingObject tpObject = new ToppingObject();
                tpObject.BId = _bTeaSelectedItem.ImgId;
                tpObject.BName = _bTeaSelectedItem.Name;
                tpObject.BPrice = Convert.ToDouble(_bTeaSelectedItem.Price);
                tpObject.BNote = _bTeaSelectedItem.Note;

                btObject = tpObject;
            }


            //Make order item;
            BTeaOrderItems orderObjItem = new BTeaOrderItems();
            BTBaseObject btBaseObj = btObject;
            orderObjItem.OrderName = btBaseObj.BName;
            orderObjItem.OrderPrice = btBaseObj.BPrice.ToString("#,##0.00;(#,##0.00)");
            if (_drinkCheck == true) 
            {
                DrinkObject drObj = (DrinkObject)btBaseObj;
                if (drObj != null)
                {
                    int nSize = drObj.DrinkSize;
                    string sizeStr = "";
                    if (nSize == 0)
                    {
                        sizeStr = "M";
                    }
                    else
                    {
                        sizeStr = "L";
                    }

                    int nSuRate = drObj.SugarRate;
                    string SuStr = "";
                    if (nSuRate == 1)
                    {
                        SuStr = "30%";
                    }
                    else if (nSuRate == 2)
                    {
                        SuStr = "50%";
                    }
                    else if (nSuRate == 3)
                    {
                        SuStr = "70%";
                    }
                    else
                    {
                        SuStr = "100%";
                    }

                    int nIcRate = drObj.IceRate;
                    string IceStr = "";
                    if (nSuRate == 1)
                    {
                        IceStr = "30%";
                    }
                    else if (nSuRate == 2)
                    {
                        IceStr = "50%";
                    }
                    else if (nSuRate == 3)
                    {
                        IceStr = "70%";
                    }
                    else
                    {
                        IceStr = "100%";
                    }

                    string tpContent = "";
                    for (int j = 0; j < drObj.TPListObj.Count; ++j)
                    {
                        ToppingObject tpObj = drObj.TPListObj[j];
                        tpContent += tpObj.BName;
                        if (j < drObj.TPListObj.Count - 1)
                        {
                            tpContent += ",";
                        }
                    }

                    string strNote = "";
                    strNote += "Size: " + sizeStr + " ";
                    strNote += "Đường: " + SuStr + " ";
                    strNote += "Đá: " + IceStr + " ";
                    if (tpContent != string.Empty)
                    {
                        strNote += "\nTopping: " + tpContent;
                    }
                    
                    orderObjItem.OrderNote = strNote;
                }
            }

            _dataOrderList.Add(orderObjItem);
            _dataOrderObjectList.Add(btObject);

            OnPropertyChange("DataOrderList");

            double sumPrice = 0.0;
            for (int i = 0; i < _dataOrderList.Count; i++)
            {
                BTeaOrderItems orderItem = _dataOrderList[i];
                string strPrice = orderItem.OrderPrice;
                double oPrice = Convert.ToDouble(strPrice);
                sumPrice += oPrice;
            }

            _billPrice = sumPrice.ToString("#,##0.00;(#,##0.00)");
            OnPropertyChange("BillSumPrice");
        }

        public ObservableCollection<BTeaItem> GetDataList()
        {
            _dataList.Clear();
            if (_drinkCheck == true)
            {
                List<DrinkObject> data_list = DBConnection.GetInstance().GetDataDrink();
                for (int i = 0; i < data_list.Count; ++i)
                {
                    BTeaItem bItem = new BTeaItem();
                    DrinkObject drObject = data_list[i];
                    bItem.ImgId = "DR" + drObject.BId;
                    bItem.Name = drObject.BName;
                    bItem.Price = drObject.BPrice.ToString("#,##0.00;(#,##0.00)");
                    bItem.Note = drObject.BNote;

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
                    bItem.Price = tpObject.BPrice.ToString();
                    bItem.Note = tpObject.BNote;

                    _dataList.Add(bItem);
                    _originDataList.Add(bItem);
                }
            }

            if (_dataList.Count > 0)
            {
                _bTeaSelectedItem = _dataList[0];
            }
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
        }
        #endregion

        #region Property
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
        public List<string> SizeItems
        {
            get { return _sizeItems; }
            set
            {
                _sizeItems = value;
                OnPropertyChange("SizeItems");
            }
        }

        public List<string> SugarItems
        {
            get { return _sugarItems; }
            set
            {
                _sugarItems = value;
                OnPropertyChange("SugarItems");
            }
        }

        public List<string> IceItems
        {
            get { return _iceItems; }
            set
            {
                _iceItems = value;
                OnPropertyChange("IceItems");
            }
        }

        public string SelectedSizeItem { set; get; }
        public string SelectedSugarItem { set; get; }
        public string SelectedIceItem { set; get; }
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
