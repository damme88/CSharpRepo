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
    class FrmWaitOrderVM : TBaseVM
    {
        public FrmWaitOrderVM(Action parentAction)
        {
            _pMainMethod = parentAction;
            WClearItemCmd = new RelayCommand(new Action<object>(DoClearItem));
            WReOrderItemCmd = new RelayCommand(new Action<object>(DoReOrderItem));
            _billItem = new ObservableCollection<BTea.BillItem>();

            GetDataWaitBillFromDB();
        }

        #region PROPERTY
        private ObservableCollection<BillItem> _billItem;
        public ObservableCollection<BillItem> BillItems
        {
            get { return _billItem; }
            set
            {
                _billItem = value;
                OnPropertyChange("BillItems");
            }
        }

        private BillItem _selectedBillItem;
        public BillItem SelectedBillItem
        {
            get { return _selectedBillItem; }
            set
            {
                _selectedBillItem = value;
                OnPropertyChange("SelectedBillItem");
            }
        }


        public RelayCommand WClearItemCmd { set; get; }
        public RelayCommand WReOrderItemCmd { set; get; }

        private readonly Action _pMainMethod;
        #endregion

        #region METHOD
        public void DoClearItem(object obj)
        {
            string sCap = "Dữ liệu sẽ được xóa trong Database(xóa vĩnh viễn).";
            sCap += "\nBạn có chắc chắn muốn xóa hóa đơn luu tam này?";

            MessageBoxResult msg = TConst.MsgYNQ(sCap);
            if (msg == MessageBoxResult.No)
            {
                return;
            }

            BillItem bItem = _selectedBillItem;
            if (bItem != null)
            {
                string strBillId = bItem.BillId;
                strBillId = strBillId.Replace("WB", "");
                int nId = TConst.ConvertInt(strBillId);
                if (nId > 0)
                {
                    bool bRet = DBConnection.GetInstance().DeleteWaitBillItem(nId);
                }

                List<BTeaOrderObject> dbOrderList = DBConnection.GetInstance().GetWaitItemObject();
                for (int i = 0; i < dbOrderList.Count; ++i)
                {
                    BTeaOrderObject objData = dbOrderList[i];
                    if (objData.BOrderBillName == bItem.BillName)
                    {
                        int id = objData.BOrderId;
                        bool bRet = DBConnection.GetInstance().DeleteWaitItem(nId);
                    }
                }
            }

            GetDataWaitBillFromDB();
        }

        public void DoReOrderItem(object obj)
        {
            _pMainMethod.Invoke();
        }

        public void GetDataWaitBillFromDB()
        {
            List<BillObject> data_list = DBConnection.GetInstance().GetWaitBillObject();
            if (_billItem != null)
            {
                _billItem.Clear();
            }

            for (int i = 0; i < data_list.Count; ++i)
            {
                BillItem bItem = new BillItem();
                BillObject billObject = data_list[i];

                bItem.BillId = "WB" + billObject.BillId;
                bItem.BillName = billObject.BillName;
                bItem.BillCreator = billObject.BillCreator;
                bItem.BillDate = billObject.BillDate.ToString("dd-MMM-yyyy");
                bItem.BillTableNumber = billObject.BillTableNumber;
                bItem.BillPhone = billObject.BillPhone;
                bItem.BillAddress = billObject.BillAddress;
                bItem.BillNote = billObject.BillNote;
                bItem.BillPrice = billObject.BillPrice.ToString(TConst.K_MONEY_FORMAT);
                bItem.TypeData = 1;
                bItem.BillKM = billObject.KMValue.ToString();

                if(billObject.KMType == TConst.K_KM_PERCENT)
                {
                    bItem.BillKMType = "%";
                }
                else
                {
                    bItem.BillKMType = "đ";
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

            if (_billItem.Count > 0)
            {
                _selectedBillItem = _billItem[0];
            }
        }


        public string GetBillName()
        {
            string strName = "";
            if (_selectedBillItem != null)
            {
                strName = _selectedBillItem.BillName;
            }

            return strName;
        }

        public string GetBillPrice()
        {
            string strName = "";
            if (_selectedBillItem != null)
            {
                strName = _selectedBillItem.BillPrice;
            }

            return strName;
        }

        public string GetBillCreator()
        {
            string strCreator = "";
            if (_selectedBillItem != null)
            {
                strCreator = _selectedBillItem.BillCreator;
            }

            return strCreator;
        }

        public string GetBillDate()
        {
            string date= "";
            if (_selectedBillItem != null)
            {
                date = _selectedBillItem.BillDate;
            }

            return date;
        }

        public string GetTable()
        {
            string sTable = "";
            if (_selectedBillItem != null)
            {
                sTable = _selectedBillItem.BillTableNumber;
            }

            return sTable;
        }

        public string GetPhone()
        {
            string sPhone = "";
            if (_selectedBillItem != null)
            {
                sPhone = _selectedBillItem.BillPhone;
            }

            return sPhone;
        }

        public string GetAddress()
        {
            string sAddress = "";
            if (_selectedBillItem != null)
            {
                sAddress = _selectedBillItem.BillAddress;
            }

            return sAddress;
        }

        public string GetNote()
        {
            string sNote = "";
            if (_selectedBillItem != null)
            {
                sNote = _selectedBillItem.BillNote;
            }

            return sNote;
        }

        public string GetKMType()
        {
            string sKMType = "";
            if (_selectedBillItem != null)
            {
                sKMType = _selectedBillItem.BillKMType;
            }

            return sKMType;
        }

        public string GetKMValue()
        {
            string sKMVal= "";
            if (_selectedBillItem != null)
            {
                sKMVal = _selectedBillItem.BillKM;
            }

            return sKMVal;
        }

        public List<BTeaOrderObject> GetListOrderItem()
        {
            List<BTeaOrderObject> billItemsObj = new List<BTeaOrderObject>();
            if (_selectedBillItem != null)
            {
                List<BTeaOrderObject> listOrder = DBConnection.GetInstance().GetWaitItemObject();

                for (int i = 0; i < _selectedBillItem.OrderItemList.Count; i++)
                {
                    for (int j = 0; j < listOrder.Count; ++j)
                    {
                        BTeaOrderObject orderObj = listOrder[j];
                        int idItem = _selectedBillItem.OrderItemList[i];
                        if (idItem == orderObj.BOrderId)
                        {
                            billItemsObj.Add(orderObj);
                        }
                    }
                }
            }

            return billItemsObj;
        }

        public void RemoveOrderItem()
        {
            BillItem bItem = _selectedBillItem;
            if (bItem != null)
            {
                string billName = bItem.BillName;
                List<int> orderList = bItem.OrderItemList;
                for (int i = 0; i < orderList.Count; ++i)
                {
                    int id = orderList[i];
                    bool bRet = DBConnection.GetInstance().DeleteWaitItem(id);
                }

                string strBillId = bItem.BillId;
                strBillId = strBillId.Replace("WB", "");
                int nId = TConst.ConvertInt(strBillId);
                if (nId > 0)
                {
                    bool bRet = DBConnection.GetInstance().DeleteWaitBillItem(nId);
                }
            }
        }
#endregion
    }
}
