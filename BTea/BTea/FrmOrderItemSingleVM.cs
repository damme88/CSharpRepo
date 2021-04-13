﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TApp.Base;

namespace BTea
{

    class FrmOrderItemSingleVM : TBaseVM
    {
        public FrmOrderItemSingleVM(Action parentAction)
        {

            _pMainMethod = parentAction;
            _orderSingleName = "";
            _orderSingleNum = 1;
            _orderSinglePrice = 0.0;
            OrderSingleUpdateCmd = new RelayCommand(new Action<object>(DoUpdateItem));
            _sizeItemSingle = new List<SizeItemData>();

            _sizeItemSingle.Add(new SizeItemData(0, "M"));
            _sizeItemSingle.Add(new SizeItemData(1, "L"));

            _surgarItemSingle = new List<SugarItemData>();
            _surgarItemSingle.Add(new SugarItemData(0, "100%"));
            _surgarItemSingle.Add(new SugarItemData(1, "30%"));
            _surgarItemSingle.Add(new SugarItemData(2, "50%"));
            _surgarItemSingle.Add(new SugarItemData(3, "70%"));

            _iceItemSingle = new List<IceItemData>();
            _iceItemSingle.Add(new IceItemData(0, "100%"));
            _iceItemSingle.Add(new IceItemData(1, "30%"));
            _iceItemSingle.Add(new IceItemData(2, "50%"));
            _iceItemSingle.Add(new IceItemData(3, "70%"));

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
        }

        #region MEMBERS
        private List<ToppingItemCheck> _toppingItemList;
        private string _orderSingleName;
        private int _orderSingleNum;
        private double _orderSinglePrice;
        private List<SizeItemData> _sizeItemSingle;
        private List<SugarItemData> _surgarItemSingle;
        private List<IceItemData> _iceItemSingle;

        private SizeItemData _selectedSizeItem;
        private SugarItemData _selectedSugarItem;
        private IceItemData _selectedIceItem;

        private readonly Action _pMainMethod;
        public RelayCommand OrderSingleUpdateCmd { set; get; }
        #endregion

        #region METHOD
        public void SetInfo1(string name, double price, int number)
        {
            _orderSingleName = name;
            _orderSinglePrice = price;
            _orderSingleNum = number;
        }

        public void SetInfo2(int idxSize, int idxSugar, int idxIce)
        {
            _selectedSizeItem = SizeItemSingle[idxSize];
            _selectedSugarItem = SurgarItemSingle[idxSugar];
            _selectedIceItem = IceItemSingle[idxIce];
        }

        public void SetTopping(int idx)
        {
            _toppingItemList[idx].IsSelected = true;
        }

        public void DoUpdateItem(object obj)
        {
            _pMainMethod.Invoke();
        }
        #endregion

        #region PROPERTY
        public SizeItemData SelectedSizeItem
        {
            get { return _selectedSizeItem; }
            set { _selectedSizeItem = value;  OnPropertyChange("SelectedSizeItem"); }
        }

        public SugarItemData SelectedSugarItem
        {
            get { return _selectedSugarItem; }
            set { _selectedSugarItem = value; OnPropertyChange("SelectedSugarItem"); }
        }
        public IceItemData SelectedIceItem
        {
            get { return _selectedIceItem; }
            set { _selectedIceItem = value; OnPropertyChange("SelectedIceItem"); }
        }

        public string OrderSingleName
        {
            get { return _orderSingleName; }
            set { _orderSingleName = value; OnPropertyChange("OrderSingleName"); }
        }

        public int OrderSingleNum
        {
            get { return _orderSingleNum; }
            set
            {
                if (value < 1)
                {
                    value = 1;
                }
                _orderSingleNum = value;
                OnPropertyChange("OrderSingleNum");
            }
        }

        public string OrderSinglePrice
        {
            get { return _orderSinglePrice.ToString("#,##0.00;(#,##0.00)"); }
            set
            {
                double val = Convert.ToDouble(value);
                _orderSinglePrice = val;
                OnPropertyChange("OrderSinglePrice");
            }
        }

        public List<ToppingItemCheck> SingleToppingItems
        {
            get { return _toppingItemList; }
            set
            {
                _toppingItemList = value;
                OnPropertyChange("SingleToppingItems");
            }
        }

        public List<SizeItemData> SizeItemSingle
        {
            get { return _sizeItemSingle; }
            set { _sizeItemSingle = value; OnPropertyChange("SizeItemSingle"); }
        }

        public List<SugarItemData> SurgarItemSingle
        {
            get { return _surgarItemSingle; }
            set { _surgarItemSingle = value; OnPropertyChange("SurgarItemSingle"); }
        }

        public List<IceItemData> IceItemSingle
        {
            get { return _iceItemSingle; }
            set { _iceItemSingle = value; OnPropertyChange("IceItemSingle"); }
        }

        #endregion
    }
}
