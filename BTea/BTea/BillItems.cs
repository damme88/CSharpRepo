using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TApp.Base;

namespace BTea
{
    class BillItem
    {
        public BillItem()
        {
            BillDetailCmd = new RelayCommand(new Action<object>(BillDetailInfo));
        }

        public string BillId {set; get;}
        public string BillName { set; get; }
        public string BillPrice { set; get; }
        public string BillCreator { set; get; }
        public string BillDate { set; get; }
        public string BillPhone { set; get; }
        public string BillAddress { set; get; }
        public string BillNote { set; get; }

        public RelayCommand BillDetailCmd { set; get; }

        public void BillDetailInfo(object sender)
        {
            int a = 5;
        }
    }
}
