using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TApp.Base;

namespace BTea
{
    class FrmBillMainVM : TBaseVM
    {
        public FrmBillMainVM()
        {
            _billItem = new ObservableCollection<BillItem>();
            GetDataBillFromDB();
        }

        private ObservableCollection<BillItem> _billItem;
        private int _billCount;

        public int BillCount
        {
            get { return _billCount; }
            set
            {
                _billCount = value;
                OnPropertyChange("BillCount");
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
        public void GetDataBillFromDB()
        {
            List<BillObject> data_list = DBConnection.GetInstance().GetDataBillObject();
            _billItem = new ObservableCollection<BillItem>();
            for (int i = 0; i < data_list.Count; ++i)
            {
                BillItem bItem = new BillItem();
                BillObject billObject = data_list[i];
                bItem.BillId = "BL" + billObject.BillId;
                bItem.BillName = billObject.BillName;
                bItem.BillPrice = billObject.BillPrice.ToString(TConst.K_MONEY_FORMAT);
                bItem.BillCreator = billObject.BillCreator;
                bItem.BillDate = billObject.BillDate.ToString("dd-MMM-yyyy");
                bItem.BillPhone = billObject.BillPhone;
                bItem.BillAddress = billObject.BillAddress;
                bItem.BillNote = billObject.BillNote;
                bItem.BillKM = billObject.KMValue.ToString();

                string strItemOrder = billObject.BillOrderItem;
                string[] itemsArr = strItemOrder.Split(',');
                for (int ii = 0; ii < itemsArr.Length; ii++)
                {
                    string strValue = itemsArr[ii];
                    bItem.AddItemOrder(strValue);
                }
                _billItem.Add(bItem);
            }

            _billCount = data_list.Count;
        }
    }
}
