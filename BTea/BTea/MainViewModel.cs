using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TApp.Base;

namespace BTea
{
    class MainViewModel : TBaseVM
    {
        public MainViewModel()
        {
            DrinkCmd = new RelayCommand(new Action<object>(DoDrink));
            CmdSelectItem = new RelayCommand(new Action<object>(DoSelectItem));
            _drinkCheck = true;
            _foodCheck = false;
            _toppingCheck = false;
            _foodOtherCheck = false;

            _dataList = new ObservableCollection<BTeaItem>();
            _originDataList = new ObservableCollection<BTeaItem>();
            _bTeaSelectedItem = null;
            _dataList = GetDataList();
        }

        #region Member
        public RelayCommand DrinkCmd { set; get; }
        public RelayCommand CmdSelectItem { set; get; }
        private FrmDrinkMainVM _frmDrinkVm;
        private ObservableCollection<BTeaItem> _dataList;
        private ObservableCollection<BTeaItem> _originDataList;
        private BTeaItem _bTeaSelectedItem;
        private bool _drinkCheck;
        private bool _foodCheck;
        private bool _toppingCheck;
        private bool _foodOtherCheck;
        private string _findItem;
        #endregion

        #region Method
        public void DoDrink(object obj)
        {
            FrmDrinkMain frmDrinkMain = new FrmDrinkMain();
            _frmDrinkVm = new FrmDrinkMainVM();
            frmDrinkMain.DataContext = _frmDrinkVm;
            frmDrinkMain.ShowDialog();
        }

        public void DoSelectItem(object obj)
        {
            int a = 5;
        }
        public ObservableCollection<BTeaItem> GetDataList()
        {
            if (_drinkCheck == true)
            {
                List<DrinkObject> data_list = DBConnection.GetInstance().GetDataDrink();
                for (int i = 0; i < data_list.Count; ++i)
                {
                    BTeaItem bItem = new BTeaItem();
                    DrinkObject drObject = data_list[i];
                    bItem.ImgId = Convert.ToInt32(drObject.DRId);
                    bItem.Name = drObject.DRName;
                    bItem.Price = drObject.DRPrice;
                    bItem.Note = drObject.DRNote;

                    _dataList.Add(bItem);
                    _originDataList.Add(bItem);
                }
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
                OnPropertyChange("DataList");
            }
        }
        #endregion

        #region Property
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
                }
                OnPropertyChange("DrinkCheck");
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
                }
                OnPropertyChange("FoodCheck");
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
                }
                OnPropertyChange("ToppingCheck");
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
                }
                OnPropertyChange("FoodOtherCheck");
            }
        }
        #endregion
    }
}
