using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTea
{
    class BTeaOrderObject
    {
        public BTeaOrderObject()
        {

        }

        public string BOrderId { set; get; }
        public string BOrderName { set; get; }
        public int BOrderNum { set; get; }
        public double BOrderPrice { set; get; }
        public int BOrderSize { set; get; }
        public int BOrderSugarRate { set; get; }
        public int BOrderIceRate { set; get; }
        public string BOrderTopping { set; get; }
        public string BOrderBillId { set; get; }
        public DateTime BOrderDate { set; get; }

        public BTBaseObject.BTeaType Type { set; get; }
    }
}
