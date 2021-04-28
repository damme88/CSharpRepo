using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTea
{
    class BillObject
    {
        public BillObject()
        {
            _billId = 0;
            _billName = string.Empty;
            _billAddress = string.Empty;
            _billPhone = string.Empty;
            _billCreator = string.Empty;
            _billTableNumber = string.Empty;
            _billPrice = 0;
            _billDate = DateTime.Now;
            _billNote = string.Empty;
            _btList = new List<BTBaseObject>();
        }

        #region MEMBER
        private int _billId;
        private string _billName;
        private int _billPrice;
        private string _billCreator;
        private string _billTableNumber;
        private DateTime _billDate;
        private string _billPhone;
        private string _billAddress;
        private string _billOrderItem;
        private string _billNote;
        private int _kmValue;
        private int _kmType;
        List<BTBaseObject> _btList;
        #endregion

        #region METHOD
        #endregion

        #region PROPERTY

        public int BillId
        {
            get { return _billId; }
            set
            {
                _billId = value;
            }
        }
        public string BillName
        {
            get { return _billName; }
            set
            {
                _billName = value;
            }
        }

        public string BillTableNumber
        {
            get { return _billTableNumber; }
            set
            {
                _billTableNumber = value;
            }
        }

        public string BillAddress
        {
            get { return _billAddress; }
            set
            {
                _billAddress = value;
            }
        }

        public string BillPhone
        {
            get { return _billPhone; }
            set
            {
                _billPhone = value;
            }
        }
        public string BillCreator
        {
            get { return _billCreator; }
            set
            {
                _billCreator = value;
            }
        }

        public int BillPrice
        {
            get { return _billPrice; }
            set
            {
                _billPrice = value;
            }
        }

        public DateTime BillDate
        {
            get { return _billDate; }
            set
            {
                _billDate = value;
            }
        }

        public string BillOrderItem
        {
            get { return _billOrderItem; }
            set
            {
                _billOrderItem = value;
            }
        }

        public string BillNote
        {
            get { return _billNote; }
            set
            {
                _billNote = value;
            }
        }

        public int KMValue
        {
            get { return _kmValue; }
            set
            {
                _kmValue = value;
            }
        }

        public int KMType
        {
            get { return _kmType; }
            set
            {
                _kmType = value;
            }
        }
        #endregion
    }
}
