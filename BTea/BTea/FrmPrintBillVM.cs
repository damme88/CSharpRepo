using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _printBillName = "";
            _printBillCreator = "";
            _printBillDate = "";
        }

        #region MEMBERS
        private ObservableCollection<PrintBillIData> _printBillItems;
        #endregion


        #region METHOD
        public void AddData(PrintBillIData item)
        {
            _printBillItems.Add(item);
        }

        public void SetInfo(string pName, string pCreator, string pDate, string totalPrice, string totalNumber)
        {
            _printBillName = pName;
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

        private string _printBillCreator;
        public string PrintBillCreator
        {
            get { return _printBillCreator; }
            set { _printBillCreator = value; OnPropertyChange("PrintBillCreator"); }
        }

        string _printTotalPrice;
        public string PrintTotalPrice
        {
            get { return _printTotalPrice; }
            set { _printTotalPrice = value; OnPropertyChange("PrintTotalPrice"); }
        }

        public string PrintTotalNumber { set; get; }
        public string BillShopName { set; get; }

        public string BillShopPhone { set; get; }

        public string BillShopAddress { set; get; }

        public string BillShopFanpge { set; get; }
        #endregion
    }
}
