﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TApp.Base;

namespace BTea
{
    class PrintBillIData
    {
        public PrintBillIData()
        {
            ;
        }
        public PrintBillIData(string name, string num, string sPrice, string tPrice)
        {
            NameProduct = name;
            NumberProduct = num;
            SinglePrice = sPrice;
            SumPrice = tPrice;
        }

        public string NameProduct { set; get; }
        public string NumberProduct { set; get; }

        public string SinglePrice { set; get; }

        public string SumPrice { set; get; }
    }
    class FrmPrintBillVM : TBaseVM
    {
        public FrmPrintBillVM()
        {
            _printBillItems = new ObservableCollection<PrintBillIData>();
            _printBillItemsDrink = new ObservableCollection<PrintBillIData>();
            _printBillName = "";
            _printBillTableNumber = "";
            _printBillCreator = "";
            _printBillDate = "";

            _isPrintDrinkBill = 1;
            if (_isPrintDrinkBill == 1)
            {
                _isVisibleDrinkBill = Visibility.Visible;
            }
            else
            {
                _isVisibleDrinkBill = Visibility.Hidden;
            }
        }

        #region MEMBERS
        private ObservableCollection<PrintBillIData> _printBillItems;
        private ObservableCollection<PrintBillIData> _printBillItemsDrink;
        #endregion


        #region METHOD
        public void AddData(PrintBillIData item)
        {
            _printBillItems.Add(item);
        }

        public void AddDataDrink(PrintBillIData item)
        {
            _printBillItemsDrink.Add(item);
        }

        public void SetInfoDrink(int numberDrink, double priceDrink)
        {
            _printTotalPriceDrink = priceDrink.ToString(TConst.K_MONEY_FORMAT);
            PrintTotalNumberDrink = numberDrink.ToString();
        }

        public void SetHideDrinkBill()
        {
            _isVisibleDrinkBill = Visibility.Collapsed;
        }

        public void SetInfo(string pName, string pTableNumber, string pCreator, string pDate, string totalPrice, string totalNumber)
        {
            _printBillName = pName;
            _printBillTableNumber = pTableNumber;
            _printBillDate = pDate;
            _printBillCreator = pCreator;
            _printTotalPrice = totalPrice;
            PrintTotalNumber = totalNumber;

            BillShopName = ConfigurationManager.AppSettings["shopname"].ToString();
            BillShopPhone = ConfigurationManager.AppSettings["shopphone"].ToString();
            BillShopAddress = ConfigurationManager.AppSettings["shopaddress"].ToString();
            BillShopFanpge = ConfigurationManager.AppSettings["shopface"].ToString();
        }
        #endregion

        #region PROPERTY
        public ObservableCollection<PrintBillIData> PrintBillItems
        {
            get
            {
                return _printBillItems;
            }
            set
            {
                _printBillItems = value;
                OnPropertyChange("PrintBillItems");
            }
        }

        public ObservableCollection<PrintBillIData> PrintBillItemsDrink
        {
            get
            {
                return _printBillItemsDrink;
            }
            set
            {
                _printBillItemsDrink = value;
                OnPropertyChange("PrintBillItemsDrink");
            }
        }

        private string _printBillName;
        public string PrintBillName
        {
            get { return _printBillName; }
            set { _printBillName = value; OnPropertyChange("PrintBillName"); }
        }

        private string _printBillDate;
        public string PrintBillDate
        {
            get { return _printBillDate; }
            set { _printBillDate = value; OnPropertyChange("PrintBillDate"); }
        }

        private string _printBillTableNumber;
        public string PrintBillTableNumber
        {
            get { return _printBillTableNumber; }
            set { _printBillTableNumber = value; OnPropertyChange("PrintBillTableNumber"); }
        }

        private string _printBillCreator;
        public string PrintBillCreator
        {
            get { return _printBillCreator; }
            set { _printBillCreator = value; OnPropertyChange("PrintBillCreator"); }
        }

        private string _printTotalPrice;
        public string PrintTotalPrice
        {
            get { return _printTotalPrice; }
            set { _printTotalPrice = value; OnPropertyChange("PrintTotalPrice"); }
        }

        private string _printTotalPriceDrink;
        public string PrintTotalPriceDrink
        {
            get { return _printTotalPriceDrink; }
            set { _printTotalPriceDrink = value; OnPropertyChange("PrintTotalPriceDrink"); }
        }

        private int _isPrintDrinkBill;
        public int IsPrintDrinkBill
        {
            set
            { _isPrintDrinkBill = value;
                OnPropertyChange("IsPrintDrinkBill");

                if (_isPrintDrinkBill == 1)
                {
                   IsVisibleDrinkBill = Visibility.Visible;
                }
                else
                {
                    IsVisibleDrinkBill = Visibility.Hidden;
                }
            }
            get { return _isPrintDrinkBill; }
        }

        private Visibility _isVisibleDrinkBill;
        public Visibility IsVisibleDrinkBill
        {
            get { return _isVisibleDrinkBill; }
            set
            {
                _isVisibleDrinkBill = value;
                OnPropertyChange("IsVisibleDrinkBill");
            }
        }


        public string PrintTotalNumber { set; get; }

        public string PrintTotalNumberDrink { set; get; }

        public string BillShopName { set; get; }

        public string BillShopPhone { set; get; }

        public string BillShopAddress { set; get; }

        public string BillShopFanpge { set; get; }
        #endregion
    }
}
