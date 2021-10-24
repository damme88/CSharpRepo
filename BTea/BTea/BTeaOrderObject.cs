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

        public int BOrderId { set; get; }
        public string BOrderIdItem { set; get; }
        public string BOrderName { set; get; }
        public int BOrderNum { set; get; }
        public int BOrderPrice { set; get; }  // giá = đơn giá * số lượng
        public int BOrderSize { set; get; }
        public int BOrderSugarRate { set; get; }
        public int BOrderIceRate { set; get; }
        public string BOrderTopping { set; get; }
        public string BOrderBillName { set; get; }
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

        public BTBaseObject MakeObject()
        {
            BTBaseObject btObject = null;

            if (Type == BTBaseObject.BTeaType.DRINK_TYPE)
            {
                DrinkObject drObject = new DrinkObject();
                drObject.BId = BOrderIdItem;
                drObject.BName = BOrderName;

                List<DrinkObject> baseList = DBConnection.GetInstance().GetDataDrink();
                string id = drObject.BId.Replace("DR", "");
                DrinkObject baseObj = baseList.Find(x => x.BId == id);
                if (baseObj != null)
                {
                    drObject.BPrice = baseObj.BPrice;
                }
                
                drObject.DrinkSize = BOrderSize;
                drObject.SugarRate = BOrderSugarRate;
                drObject.IceRate = BOrderSugarRate;

                List<ToppingObject> dataTopping = DBConnection.GetInstance().GetDataTopping();

                string sTopping = BOrderTopping;
                string[] itemsTp = sTopping.Split(',');

                for (int i = 0; i < itemsTp.Count(); ++i)
                {
                    for (int j = 0; j < dataTopping.Count; j++)
                    {
                        ToppingObject tpData = dataTopping[j];
                        if (itemsTp[i] == tpData.BId)
                        {
                            ToppingObject newTpObj = new ToppingObject();
                            newTpObj.BId = tpData.BId;
                            newTpObj.BName = tpData.BName;
                            newTpObj.BPrice = tpData.BPrice;
                            drObject.TPListObj.Add(newTpObj);
                        }
                    }
                }
                btObject = drObject;
                btObject.Type = BTBaseObject.BTeaType.DRINK_TYPE;
            }
            else if (Type == BTBaseObject.BTeaType.FOOD_TYPE)
            {
                FoodObject fObject = new FoodObject();
                fObject.BId = BOrderIdItem;
                fObject.BName = BOrderName;

                List<FoodObject> baseList = DBConnection.GetInstance().GetDataFood();
                string id = fObject.BId.Replace("F", "");
                FoodObject baseObj = baseList.Find(x => x.BId == id);
                if (baseObj != null)
                {
                    fObject.BPrice = baseObj.BPrice;
                }
                btObject = fObject;
                btObject.Type = BTBaseObject.BTeaType.FOOD_TYPE;
            }
            else if (Type == BTBaseObject.BTeaType.OTHER_TYPE)
            {
                OtherFoodObject ofObject = new OtherFoodObject();
                ofObject.BId = BOrderIdItem;
                ofObject.BName = BOrderName;

                List<OtherFoodObject> baseList = DBConnection.GetInstance().GetDataOtherFood();
                string id = ofObject.BId.Replace("OF", "");
                OtherFoodObject baseObj = baseList.Find(x => x.BId == id);
                if (baseObj != null)
                {
                    ofObject.BPrice = baseObj.BPrice;
                }

                btObject = ofObject;
                btObject.Type = BTBaseObject.BTeaType.OTHER_TYPE;
            }
            else if (Type == BTBaseObject.BTeaType.TOPPING_TYPE)
            {
                ToppingObject tpObject = new ToppingObject();
                tpObject.BId = BOrderIdItem;
                tpObject.BName = BOrderName;

                List<ToppingObject> baseList = DBConnection.GetInstance().GetDataTopping();
                string id = tpObject.BId.Replace("TP", "");
                ToppingObject baseObj = baseList.Find(x => x.BId == id);
                if (baseObj != null)
                {
                    tpObject.BPrice = baseObj.BPrice;
                }

                btObject = tpObject;
                btObject.Type = BTBaseObject.BTeaType.TOPPING_TYPE;
            }

            return btObject;
        }
    }
}
