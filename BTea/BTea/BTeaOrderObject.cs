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
        public int BOrderPrice { set; get; }
        public int BOrderSize { set; get; }
        public int BOrderSugarRate { set; get; }
        public int BOrderIceRate { set; get; }
        public string BOrderTopping { set; get; }
        public string BOrderBillId { set; get; }
        public DateTime BOrderDate { set; get; }
        public int BOrderKm { set; get; }

        public int BOrderKmType { set; get; }
        public BTBaseObject.BTeaType Type { set; get; }

        public string SizeToString()
        {
            string str = "M";
            if (BOrderSize == 1)
            {
                str = "L";
            }
            return str;
        }

        public string SugarToString()
        {
            string str = "";
            if (BOrderSugarRate == 1) str = "70%";
            else if (BOrderSugarRate == 2) str = "50%";
            else if (BOrderSugarRate == 3) str = "30%";
            else if (BOrderSugarRate == 4) str = "0%";
            return str;
        }

        public string IceToString()
        {
            string str = "";
            if (BOrderIceRate == 1) str = "70%";
            else if (BOrderIceRate == 2) str = "50%";
            else if (BOrderIceRate == 3) str = "30%";
            else if (BOrderIceRate == 4) str = "0%";
            return str;
        }

        public string ToppingToString()
        {
            string strTp = "";
            string sTopping = BOrderTopping;
            string[] itemsTp = sTopping.Split(',');

            List<ToppingObject> toppingObj = DBConnection.GetInstance().GetDataTopping();
            for (int i1 = 0; i1 < itemsTp.Length; i1++)
            {
                string idTp = itemsTp[i1];
                for (int i2 = 0; i2 < toppingObj.Count; i2++)
                {
                    ToppingObject tpObj = toppingObj[i2];
                    string nId = tpObj.BId;
                    if (idTp == nId)
                    {
                        strTp += tpObj.BName + ",";
                    }
                }
            }

            return strTp;
        }
    }
}
