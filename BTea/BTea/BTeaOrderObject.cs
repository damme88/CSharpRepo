﻿using System;
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

        public int BOrderId { set; get; }
        public string BOrderName { set; get; }
        public double BOrderPrice { set; get; }
        public int BOrderSize { set; get; }
        public int BOrderSugarRate { set; get; }
        public int BOrderIceRate { set; get; }
        public string BOrderTopping { set; get; }
    }
}
