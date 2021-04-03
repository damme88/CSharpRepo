using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Data;

namespace BTea
{
    class DBConnection
    {
        private DBConnection()
        {
            _instance = null;

            _mServer = "127.0.0.1";
            _mDBName = "bteadb";
            _mUserName = "root";
            _mPassword = "123456";
            _mPort = "3306";
            string connectionString = "Server=" + _mServer +
                               ";Database=" + _mDBName +
                               ";port=" + _mPort +
                               ";User Id=" + _mUserName +
                               ";password=" + _mPassword +
                               ";CharSet = utf8";
            _connection = new MySqlConnection(connectionString);
        }
        private static DBConnection _instance;
        public static DBConnection GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DBConnection();
            }
            return _instance;
        }

        #region Member
        private string _mServer;
        private string _mDBName;
        private string _mUserName;
        private string _mPassword;
        private string _mPort;
        private MySqlConnection _connection;
        #endregion

        public bool ConnectionDB()
        {
            try
            {
                _connection.Open();
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return true;
        }

        public void CloseDB()
        {
            try
            {
                _connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public List<DrinkObject> GetDataDrink()
        {
            string query = "SELECT * FROM drinktbl";

            List<DrinkObject> drObjectList = new List<DrinkObject>();

            //Open connection
            if (this.ConnectionDB() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, _connection);

                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                   string sId    = dataReader["Id"]     + "";
                   string sName  = dataReader["Name"]   + "";
                   string sPrice = dataReader["Price"]  + "";
                   string sNote  = dataReader["Note"]   + "";
                   DrinkObject drObj = new DrinkObject();
                   drObj.BId       = sId;
                   drObj.BName     = sName;
                   drObj.BPrice    = Convert.ToDouble(sPrice);
                   drObj.BNote     = sNote;

                   drObjectList.Add(drObj);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                CloseDB();
            }
            return drObjectList;
        }
        
        public bool AddDrinkItem(DrinkObject drItem)
        {
            bool bRet = false;
            if (this.ConnectionDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand("AddDrinkItem", _connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("inName", drItem.BName));
                cmd.Parameters.Add(new MySqlParameter("inPrice", drItem.BPrice));
                cmd.Parameters.Add(new MySqlParameter("inNote", drItem.BNote));

                try
                {
                    int result = cmd.ExecuteNonQuery();
                    bRet = true;
                }
                catch (System.Exception ex)
                {
                    bRet = false;
                }
            }

            CloseDB();
            return bRet;
        }

        public bool EditDrinkItem(DrinkObject drItem)
        {
            bool bRet = false;
            if (this.ConnectionDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand("EditDrinkItem", _connection);
                cmd.CommandType = CommandType.StoredProcedure;

                int drId = Convert.ToInt32(drItem.BId);
                cmd.Parameters.Add(new MySqlParameter("inId", drId));
                cmd.Parameters.Add(new MySqlParameter("inName", drItem.BName));
                cmd.Parameters.Add(new MySqlParameter("inPrice", drItem.BPrice));
                cmd.Parameters.Add(new MySqlParameter("inNote", drItem.BNote));
                int result = 0;
                try
                {
                    result = cmd.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    bRet = false;
                }

                if (result == 1)
                {
                    bRet = true;
                }
                else
                {
                    bRet = false;
                }
            }

            CloseDB();
            return bRet;
        }

        public bool DeleteDrinkItem(int Id)
        {
            bool bRet = false;
            if (this.ConnectionDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand("DeleteDrinkItem", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("inId", Id));

                int result = 0;
                try
                {
                    result = cmd.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    bRet = false;
                }

                if (result == 1)
                {
                    bRet = true;
                }
                else
                {
                    bRet = false;
                }
            }
            CloseDB();
            return bRet;
        }

        public List<ToppingObject> GetDataTopping()
        {
            string query = "SELECT * FROM toppingtbl";

            List<ToppingObject> tpObjectList = new List<ToppingObject>();

            //Open connection
            if (this.ConnectionDB() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, _connection);

                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    string sId = dataReader["Id"] + "";
                    string sName = dataReader["Name"] + "";
                    string sPrice = dataReader["Price"] + "";
                    string sNote = dataReader["Note"] + "";
                    ToppingObject tpObj = new ToppingObject();
                    tpObj.BId = sId;
                    tpObj.BName = sName;
                    tpObj.BPrice = Convert.ToDouble(sPrice);
                    tpObj.BNote = sNote;

                    tpObjectList.Add(tpObj);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                CloseDB();
            }
            return tpObjectList;
        }

        public bool AddToppingItem(ToppingObject tpItem)
        {
            bool bRet = false;
            if (this.ConnectionDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand("AddToppingItem", _connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("inName", tpItem.BName));
                cmd.Parameters.Add(new MySqlParameter("inPrice", tpItem.BPrice));
                cmd.Parameters.Add(new MySqlParameter("inNote", tpItem.BNote));

                try
                {
                    int result = cmd.ExecuteNonQuery();
                    bRet = true;
                }
                catch (System.Exception ex)
                {
                    bRet = false;
                }
            }

            CloseDB();
            return bRet;
        }

        public bool EditToppingItem(ToppingObject tpItem)
        {
            bool bRet = false;
            if (this.ConnectionDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand("EditToppingItem", _connection);
                cmd.CommandType = CommandType.StoredProcedure;

                int tpId = Convert.ToInt32(tpItem.BId);
                cmd.Parameters.Add(new MySqlParameter("inId", tpId));
                cmd.Parameters.Add(new MySqlParameter("inName", tpItem.BName));
                cmd.Parameters.Add(new MySqlParameter("inPrice", tpItem.BPrice));
                cmd.Parameters.Add(new MySqlParameter("inNote", tpItem.BNote));
                int result = 0;
                try
                {
                    result = cmd.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    bRet = false;
                }

                if (result == 1)
                {
                    bRet = true;
                }
                else
                {
                    bRet = false;
                }
            }

            CloseDB();
            return bRet;
        }

        public bool DeleteToppingItem(int Id)
        {
            bool bRet = false;
            if (this.ConnectionDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand("DeleteToppingItem", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("inId", Id));

                int result = 0;
                try
                {
                    result = cmd.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    bRet = false;
                }

                if (result == 1)
                {
                    bRet = true;
                }
                else
                {
                    bRet = false;
                }
            }
            CloseDB();
            return bRet;
        }

        //Order Btea Item
        public List<BillObject> GetDataBillObject()
        {
            string query = "SELECT * FROM billtbl";

            List<BillObject> billObjectList = new List<BillObject>();

            //Open connection
            if (this.ConnectionDB() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, _connection);

                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    string sId = dataReader["Id"] + "";
                    string sName = dataReader["Name"] + "";
                    string sPrice = dataReader["Price"] + "";
                    string sCreator = dataReader["Creator"] + "";
                    string sDate = dataReader["Date"] + "";
                    string sPhone = dataReader["Phone"] + "";
                    string sAddress = dataReader["Address"] + "";
                    string sOrderItem = dataReader["OrderItem"] + "";
                    string sNote = dataReader["Note"] + "";

                    BillObject billObj = new BillObject();
                    billObj.BillId = Convert.ToInt32(sId);
                    billObj.BillName = sName;
                    billObj.BillPrice = Convert.ToDouble(sPrice);
                    billObj.BillCreator = sCreator;
                    billObj.BillDate = Convert.ToDateTime(sDate);
                    billObj.BillPhone = sPhone;
                    billObj.BillAddress = sAddress;
                    billObj.BillNote = sNote;

                    billObj.BillOrderItem = sOrderItem;
                    billObjectList.Add(billObj);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                CloseDB();
            }
            return billObjectList;
        }

        public bool AddBillItem(BillObject billItem)
        {
            bool bRet = false;
            if (this.ConnectionDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand("AddBillItem", _connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("inName", billItem.BillName));
                cmd.Parameters.Add(new MySqlParameter("inPrice", billItem.BillPrice));
                cmd.Parameters.Add(new MySqlParameter("inCreator", billItem.BillCreator));
                cmd.Parameters.Add(new MySqlParameter("inDate", billItem.BillDate));
                cmd.Parameters.Add(new MySqlParameter("inPhone", billItem.BillPhone));
                cmd.Parameters.Add(new MySqlParameter("inAddress", billItem.BillAddress));
                cmd.Parameters.Add(new MySqlParameter("inOrderItem", billItem.BillOrderItem));
                cmd.Parameters.Add(new MySqlParameter("inNote", billItem.BillNote));

                try
                {
                    int result = cmd.ExecuteNonQuery();
                    bRet = true;
                }
                catch (System.Exception ex)
                {
                    bRet = false;
                }
            }

            CloseDB();
            return bRet;
        }

        public List<BTeaOrderObject> GetDataOrderObject()
        {
            string query = "SELECT * FROM bteaordertbl";

            List<BTeaOrderObject> OderList = new List<BTeaOrderObject>();

            //Open connection
            if (this.ConnectionDB() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, _connection);

                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    string sId = dataReader["Id"] + "";
                    string sName = dataReader["Name"] + "";
                    string sPrice = dataReader["Price"] + "";
                    string sSugar = dataReader["SugarRate"] + "";
                    string sIce = dataReader["IceRate"] + "";
                    string sTopping = dataReader["Topping"] + "";
                    string sBillId = dataReader["BillId"] + "";
                    string sOrderDate = dataReader["OrderDate"] + "";

                    BTeaOrderObject OrderObj = new BTeaOrderObject();
                    OrderObj.BOrderId = sId;
                    OrderObj.BOrderName = sName;
                    OrderObj.BOrderPrice = Convert.ToDouble(sPrice);
                    OrderObj.BOrderSugarRate = Convert.ToInt32(sSugar);
                    OrderObj.BOrderIceRate = Convert.ToInt32(sIce);
                    OrderObj.BOrderTopping = sTopping;
                    OrderObj.BOrderBillId = sBillId;
                    OrderObj.BOrderDate = Convert.ToDateTime(sOrderDate);

                    if (sId.Contains("DR"))
                    {
                        OrderObj.Type = BTBaseObject.BTeaType.DRINK_TYPE;
                    }
                    else if (sId.Contains("TP"))
                    {
                        OrderObj.Type = BTBaseObject.BTeaType.TOPPING_TYPE;
                    }

                    OderList.Add(OrderObj);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                CloseDB();
            }
            return OderList;
        }

        public bool AddOrderItem(BTeaOrderObject bOrderItem)
        {
            bool bRet = false;
            if (this.ConnectionDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand("AddBteaOrderItem", _connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("inId", bOrderItem.BOrderId));
                cmd.Parameters.Add(new MySqlParameter("inName", bOrderItem.BOrderName));
                cmd.Parameters.Add(new MySqlParameter("inPrice", bOrderItem.BOrderPrice));
                cmd.Parameters.Add(new MySqlParameter("inSize", bOrderItem.BOrderSize));
                cmd.Parameters.Add(new MySqlParameter("inSugarRate", bOrderItem.BOrderSugarRate));
                cmd.Parameters.Add(new MySqlParameter("inIceRate", bOrderItem.BOrderIceRate));
                cmd.Parameters.Add(new MySqlParameter("inTopping", bOrderItem.BOrderTopping));
                cmd.Parameters.Add(new MySqlParameter("inBillId", bOrderItem.BOrderBillId));
                cmd.Parameters.Add(new MySqlParameter("inOrderDate", bOrderItem.BOrderDate));
                try
                {
                    int result = cmd.ExecuteNonQuery();
                    bRet = true;
                }
                catch (System.Exception ex)
                {
                    bRet = false;
                }
            }

            CloseDB();
            return bRet;
        }
    }
}
