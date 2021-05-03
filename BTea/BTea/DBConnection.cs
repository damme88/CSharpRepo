using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Data;
using System.Configuration;

namespace BTea
{
    class DBConnection
    {
        private DBConnection()
        {
            _instance = null;
            _mServer = ConfigurationManager.AppSettings["server"].ToString();
            _mDBName = ConfigurationManager.AppSettings["dbname"].ToString();
            _mUserName = ConfigurationManager.AppSettings["username"].ToString();
            _mPassword = ConfigurationManager.AppSettings["password"].ToString();
            _mPort = ConfigurationManager.AppSettings["port"].ToString();

            string connectionString = "Server=" + _mServer +
                               ";Database=" + _mDBName +
                               ";port=" + _mPort +
                               ";User Id=" + _mUserName +
                               ";password=" + _mPassword +
                               ";CharSet = utf8";
            try
            {
                _connection = new MySqlConnection(connectionString);
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                Tlog.GetInstance().WriteLog("Thong tin ket noi KO chinh xac(Co the la Port): " + ex.Message);
            }
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
                string msg = "Open Database Failed: ";
                msg += ex.Message;
                Tlog.GetInstance().WriteLog(msg);
                return false;
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
                string msg = "Close DB Error: ";
                msg += ex.Message;
                Tlog.GetInstance().WriteLog(msg);
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
                try
                {
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                    {
                        string sId = dataReader["Id"] + "";
                        string sName = dataReader["Name"] + "";
                        string sPrice = dataReader["Price"] + "";
                        string sNote = dataReader["Note"] + "";
                        DrinkObject drObj = new DrinkObject();
                        drObj.BId = sId;
                        drObj.BName = sName;
                        drObj.BPrice = TConst.ConvertMoney(sPrice);
                        drObj.BNote = sNote;

                        drObjectList.Add(drObj);
                    }
                    //close Data Reader
                    dataReader.Close();
                }
                catch(Exception ex)
                {
                    //close Connection
                    string strMsg = "Lay du do uong that bai : ";
                    strMsg += ex.Message;
                    Tlog.GetInstance().WriteLog(strMsg);
                    CloseDB();
                    return drObjectList;
                }

                //close Connection
                CloseDB();
            }
            else
            {
                Environment.Exit(0);
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
                    Tlog.GetInstance().WriteLog("Add Drink Thanh Cong");
                }
                catch (System.Exception ex)
                {
                    string msg = "Add Drink That bai";
                    msg += ex.Message;

                    Tlog.GetInstance().WriteLog(msg);
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

                try
                {
                    cmd.Parameters.Add(new MySqlParameter("inId", drId));
                    cmd.Parameters.Add(new MySqlParameter("inName", drItem.BName));
                    cmd.Parameters.Add(new MySqlParameter("inPrice", drItem.BPrice));
                    cmd.Parameters.Add(new MySqlParameter("inNote", drItem.BNote));
                }
                catch(Exception ex)
                {
                    string msg = "Edit Drink That bai: ";
                    msg += ex.Message;
                    Tlog.GetInstance().WriteLog(msg);

                    return false;
                }

                int result = 0;
                try
                {
                    result = cmd.ExecuteNonQuery();
                    string msg = "Edit Drink Thanh cong";
                    Tlog.GetInstance().WriteLog(msg);
                }
                catch (System.Exception ex)
                {
                    bRet = false;
                    string msg = "Edit Drink That bai: ";
                    msg += ex.Message;
                    Tlog.GetInstance().WriteLog(msg);
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
                    string msg = "Delete Drink Thanh cong: ";
                    Tlog.GetInstance().WriteLog(msg);
                }
                catch (System.Exception ex)
                {
                    string msg = "Delete Drink That bai: ";
                    msg += ex.Message;
                    Tlog.GetInstance().WriteLog(msg);
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

        public List<FoodObject> GetDataFood()
        {
            string query = "SELECT * FROM foodtbl";

            List<FoodObject> fObjectList = new List<FoodObject>();

            //Open connection
            if (this.ConnectionDB() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                try
                {
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                    {
                        string sId = dataReader["Id"] + "";
                        string sName = dataReader["Name"] + "";
                        string sPrice = dataReader["Price"] + "";
                        string sNote = dataReader["Note"] + "";
                        FoodObject foodObj = new FoodObject();
                        foodObj.BId = sId;
                        foodObj.BName = sName;
                        foodObj.BPrice = TConst.ConvertMoney(sPrice);
                        foodObj.BNote = sNote;

                        fObjectList.Add(foodObj);
                    }
                    //close Data Reader
                    dataReader.Close();
                }
                catch(Exception ex)
                {
                    string strMsg = "Get DB Do Nham nhi that bai : ";
                    strMsg += ex.Message;
                    Tlog.GetInstance().WriteLog(strMsg);
                    //close Connection
                    CloseDB();
                    return fObjectList;
                }
                
                //close Connection
                CloseDB();
            }
            return fObjectList;
        }

        public bool AddFoodItem(FoodObject foodItem)
        {
            bool bRet = false;
            if (this.ConnectionDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand("AddFoodItem", _connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("inName", foodItem.BName));
                cmd.Parameters.Add(new MySqlParameter("inPrice", foodItem.BPrice));
                cmd.Parameters.Add(new MySqlParameter("inNote", foodItem.BNote));

                try
                {
                    int result = cmd.ExecuteNonQuery();
                    bRet = true;
                    Tlog.GetInstance().WriteLog("Add food thanh cong");
                }
                catch (System.Exception ex)
                {
                    string msg = "Add food that bai: " + ex.Message;
                    Tlog.GetInstance().WriteLog(msg);
                    bRet = false;
                }
            }

            CloseDB();
            return bRet;
        }

        public bool EditFoodItem(FoodObject foodItem)
        {
            bool bRet = false;
            if (this.ConnectionDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand("EditFoodItem", _connection);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    int fId = Convert.ToInt32(foodItem.BId);
                    cmd.Parameters.Add(new MySqlParameter("inId", fId));
                    cmd.Parameters.Add(new MySqlParameter("inName", foodItem.BName));
                    cmd.Parameters.Add(new MySqlParameter("inPrice", foodItem.BPrice));
                    cmd.Parameters.Add(new MySqlParameter("inNote", foodItem.BNote));
                }
                catch(Exception ex)
                {
                    string msg = "Edit food that bai: " + ex.Message;
                    Tlog.GetInstance().WriteLog(msg);
                    return false;
                }
                
                int result = 0;
                try
                {
                    result = cmd.ExecuteNonQuery();
                    string msg = "Edit food thanh cong";
                    Tlog.GetInstance().WriteLog(msg);
                }
                catch (System.Exception ex)
                {
                    string msg = "Edit food that bai: " + ex.Message;
                    Tlog.GetInstance().WriteLog(msg);
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

        public bool DeleteFoodItem(int Id)
        {
            bool bRet = false;
            if (this.ConnectionDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand("DeleteFoodItem", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("inId", Id));

                int result = 0;
                try
                {
                    result = cmd.ExecuteNonQuery();
                    Tlog.GetInstance().WriteLog("Delete food thanh cong: ");
                }
                catch (System.Exception ex)
                {
                    Tlog.GetInstance().WriteLog("Delete food that bai: " + ex.Message);
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

        public List<OtherFoodObject> GetDataOtherFood()
        {
            string query = "SELECT * FROM otherfoodtbl";

            List<OtherFoodObject> OtherfObjectList = new List<OtherFoodObject>();

            //Open connection
            if (this.ConnectionDB() == true)
            {
                try
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
                        OtherFoodObject otherfoodObj = new OtherFoodObject();
                        otherfoodObj.BId = sId;
                        otherfoodObj.BName = sName;
                        otherfoodObj.BPrice = TConst.ConvertMoney(sPrice);
                        otherfoodObj.BNote = sNote;

                        OtherfObjectList.Add(otherfoodObj);
                    }

                    Tlog.GetInstance().WriteLog("Get DB Do lat vat thanh cong.");
                    //close Data Reader
                    dataReader.Close();
                }
                catch(Exception ex)
                {
                    string msg = "Get DB Do lat vat ko that bai: ";
                    msg += ex.Message;

                    Tlog.GetInstance().WriteLog(msg);
                    CloseDB();
                    return OtherfObjectList;
                }

                //close Connection
                CloseDB();
            }
            return OtherfObjectList;
        }

        public bool AddOtherFoodItem(OtherFoodObject otherfoodItem)
        {
            bool bRet = false;
            if (this.ConnectionDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand("AddOtherFoodItem", _connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("inName", otherfoodItem.BName));
                cmd.Parameters.Add(new MySqlParameter("inPrice", otherfoodItem.BPrice));
                cmd.Parameters.Add(new MySqlParameter("inNote", otherfoodItem.BNote));

                try
                {
                    int result = cmd.ExecuteNonQuery();
                    bRet = true;
                    Tlog.GetInstance().WriteLog("Add Otherfood thanh cong");
                }
                catch (System.Exception ex)
                {
                    Tlog.GetInstance().WriteLog("Add Otherfood that bai" + ex.Message);
                    bRet = false;
                }
            }

            CloseDB();
            return bRet;
        }

        public bool EditOtherFoodItem(OtherFoodObject otherfoodItem)
        {
            bool bRet = false;
            if (this.ConnectionDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand("EditOtherFoodItem", _connection);
                cmd.CommandType = CommandType.StoredProcedure;

                int tpId = Convert.ToInt32(otherfoodItem.BId);
                cmd.Parameters.Add(new MySqlParameter("inId", tpId));
                cmd.Parameters.Add(new MySqlParameter("inName", otherfoodItem.BName));
                cmd.Parameters.Add(new MySqlParameter("inPrice", otherfoodItem.BPrice));
                cmd.Parameters.Add(new MySqlParameter("inNote", otherfoodItem.BNote));
                int result = 0;
                try
                {
                    result = cmd.ExecuteNonQuery();
                    Tlog.GetInstance().WriteLog("Edit Otherfood thanh cong");
                }
                catch (System.Exception ex)
                {
                    Tlog.GetInstance().WriteLog("Edit Otherfood that bai" + ex.Message);
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

        public bool DeleteOtherFoodItem(int Id)
        {
            bool bRet = false;
            if (this.ConnectionDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand("DeleteOtherFoodItem", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("inId", Id));

                int result = 0;
                try
                {
                    result = cmd.ExecuteNonQuery();
                    Tlog.GetInstance().WriteLog("Delete Otherfood thanh cong");
                }
                catch (System.Exception ex)
                {
                    Tlog.GetInstance().WriteLog("Delete Otherfood that bai" + ex.Message);
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
                try
                {
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
                        tpObj.BPrice = TConst.ConvertMoney(sPrice);
                        tpObj.BNote = sNote;

                        tpObjectList.Add(tpObj);
                    }
                    Tlog.GetInstance().WriteLog("Get DB topping thanh cong.");
                    //close Data Reader
                    dataReader.Close();
                }
                catch(Exception ex)
                {
                    //close Connection
                    string msg = "Get DB Topping ko that bai: ";
                    msg += ex.Message;
                    Tlog.GetInstance().WriteLog(msg);
                    CloseDB();
                    return tpObjectList;
                }
                
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
                    Tlog.GetInstance().WriteLog("Add Topping thanh cong");
                }
                catch (System.Exception ex)
                {
                    bRet = false;
                    Tlog.GetInstance().WriteLog("Add Topping that bai: " + ex.Message);
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
                    Tlog.GetInstance().WriteLog("Edit Topping thanh cong: ");
                    result = cmd.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    bRet = false;
                    Tlog.GetInstance().WriteLog("Edit Topping that bai: " + ex.Message);
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
                    Tlog.GetInstance().WriteLog("Delete Topping thanh cong");
                }
                catch (System.Exception ex)
                {
                    bRet = false;
                    Tlog.GetInstance().WriteLog("Delete Topping that bai: " + ex.Message);
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

                try
                {
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
                        string sTableNum = dataReader["TableNumber"] + "";
                        string sPhone = dataReader["Phone"] + "";
                        string sAddress = dataReader["Address"] + "";
                        string sOrderItem = dataReader["OrderItem"] + "";
                        string sNote = dataReader["Note"] + "";
                        string sKm = dataReader["KM"] + "";
                        string sKmType = dataReader["KMType"] + "";

                        BillObject billObj = new BillObject();
                        billObj.BillId = TConst.ConvertInt(sId);
                        billObj.BillName = sName;
                        billObj.BillPrice = TConst.ConvertInt(sPrice);
                        billObj.BillCreator = sCreator;
                        billObj.BillDate = Convert.ToDateTime(sDate);
                        billObj.BillTableNumber = sTableNum;
                        billObj.BillPhone = sPhone;
                        billObj.BillAddress = sAddress;
                        billObj.BillNote = sNote;
                        billObj.KMType = TConst.ConvertInt(sKmType);
                        if (billObj.KMType == TConst.K_KM_PERCENT)
                        {
                            billObj.KMValue = TConst.ConvertInt(sKm);
                        }
                        else
                        {
                            billObj.KMValue = TConst.ConvertMoney(sKm);
                        }

                        billObj.BillOrderItem = sOrderItem;
                        billObjectList.Add(billObj);
                    }

                    Tlog.GetInstance().WriteLog("Get DB Hoa Don thanh cong.");
                    //close Data Reader
                    dataReader.Close();
                }
                catch(Exception ex)
                {
                    string msg = "Get DB Hoa Don that bai. ";
                    msg += ex;

                    Tlog.GetInstance().WriteLog(msg);
                    CloseDB();
                    return billObjectList;
                }
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
                cmd.Parameters.Add(new MySqlParameter("inTableNumber", billItem.BillTableNumber));
                cmd.Parameters.Add(new MySqlParameter("inPhone", billItem.BillPhone));
                cmd.Parameters.Add(new MySqlParameter("inAddress", billItem.BillAddress));
                cmd.Parameters.Add(new MySqlParameter("inOrderItem", billItem.BillOrderItem));
                cmd.Parameters.Add(new MySqlParameter("inNote", billItem.BillNote));
                cmd.Parameters.Add(new MySqlParameter("inKM", billItem.KMValue));
                cmd.Parameters.Add(new MySqlParameter("inKMType", billItem.KMType));
                try
                {
                    int result = cmd.ExecuteNonQuery();
                    bRet = true;
                    Tlog.GetInstance().WriteLog("Them Hoa Don thanh cong");
                }
                catch (System.Exception ex)
                {
                    Tlog.GetInstance().WriteLog("Them Hoa Don that bai");
                    bRet = false;
                }
            }

            CloseDB();
            return bRet;
        }

        public bool EditBillItem(BillObject billItem)
        {
            bool bRet = false;
            if (this.ConnectionDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand("EditBillItem", _connection);
                cmd.CommandType = CommandType.StoredProcedure;

                int bId = Convert.ToInt32(billItem.BillId);
                cmd.Parameters.Add(new MySqlParameter("inId", bId));
                cmd.Parameters.Add(new MySqlParameter("inName", billItem.BillName));
                cmd.Parameters.Add(new MySqlParameter("inPrice", billItem.BillPrice));
                cmd.Parameters.Add(new MySqlParameter("inCreator", billItem.BillCreator));
                cmd.Parameters.Add(new MySqlParameter("inDate", billItem.BillDate));
                cmd.Parameters.Add(new MySqlParameter("inTable", billItem.BillTableNumber));
                cmd.Parameters.Add(new MySqlParameter("inPhone", billItem.BillPhone));
                cmd.Parameters.Add(new MySqlParameter("inAddress", billItem.BillAddress));
                cmd.Parameters.Add(new MySqlParameter("inOrderItem", billItem.BillOrderItem));
                cmd.Parameters.Add(new MySqlParameter("inNote", billItem.BillNote));
                cmd.Parameters.Add(new MySqlParameter("inKm", billItem.KMType));
                cmd.Parameters.Add(new MySqlParameter("inKmType", billItem.KMType));
                int result = 0;
                try
                {
                    result = cmd.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    bRet = false;
                    Tlog.GetInstance().WriteLog("Edit Thong tin Bill that bai: " + ex.Message);
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

        public bool DeleteBillItem(int Id)
        {
            bool bRet = false;
            if (this.ConnectionDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand("DeleteBillItem", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("inId", Id));

                int result = 0;
                try
                {
                    result = cmd.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    Tlog.GetInstance().WriteLog("Delete Hoa Don that bai" + ex.Message);
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

        public List<BTeaOrderObject> GetDataOrderObject()
        {
            string query = "SELECT * FROM bteaordertbl";

            List<BTeaOrderObject> OderList = new List<BTeaOrderObject>();

            //Open connection
            if (this.ConnectionDB() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, _connection);

                try
                {
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    //Read the data and store them in the list
                    while (dataReader.Read())
                    {
                        string sId = dataReader["Id"] + "";
                        string sIdItem = dataReader["IdItem"] + "";
                        string sName = dataReader["Name"] + "";
                        string sNum = dataReader["Number"] + "";
                        string sPrice = dataReader["Price"] + "";
                        string sSugar = dataReader["SugarRate"] + "";
                        string sIce = dataReader["IceRate"] + "";
                        string sTopping = dataReader["Topping"] + "";
                        string sBillId = dataReader["BillId"] + "";
                        string sOrderDate = dataReader["OrderDate"] + "";
                        string sOrderKm = dataReader["Km"] + "";
                        string sOrderKmType = dataReader["KmType"] + "";

                        BTeaOrderObject OrderObj = new BTeaOrderObject();
                        OrderObj.BOrderId = TConst.ConvertInt(sId);
                        OrderObj.BOrderIdItem = sIdItem;
                        OrderObj.BOrderName = sName;
                        OrderObj.BOrderNum = TConst.ConvertInt(sNum);
                        OrderObj.BOrderPrice = TConst.ConvertMoney(sPrice);
                        OrderObj.BOrderSugarRate = TConst.ConvertInt(sSugar);
                        OrderObj.BOrderIceRate = TConst.ConvertInt(sIce);
                        OrderObj.BOrderTopping = sTopping;
                        OrderObj.BOrderBillId = sBillId;
                        OrderObj.BOrderDate = Convert.ToDateTime(sOrderDate);
                        OrderObj.BOrderKmType = TConst.ConvertInt(sOrderKmType);
                        if (OrderObj.BOrderKmType == TConst.K_KM_PERCENT)
                        {
                            OrderObj.BOrderKm = TConst.ConvertInt(sOrderKm);
                        }
                        else
                        {
                            OrderObj.BOrderKm = TConst.ConvertMoney(sOrderKm);
                        }

                        if (sIdItem.Contains("DR"))
                        {
                            OrderObj.Type = BTBaseObject.BTeaType.DRINK_TYPE;
                        }
                        else if (sIdItem.Contains("TP"))
                        {
                            OrderObj.Type = BTBaseObject.BTeaType.TOPPING_TYPE;
                        }
                        else if (sIdItem.Contains("OF"))
                        {
                            OrderObj.Type = BTBaseObject.BTeaType.OTHER_TYPE;
                        }
                        else if (sIdItem.Contains("F")  == true && sId.Contains("OF") == false)
                        {
                            OrderObj.Type = BTBaseObject.BTeaType.FOOD_TYPE;
                        }
    
                        OderList.Add(OrderObj);
                    }
                    //close Data Reader
                    dataReader.Close();
                }
                catch(Exception ex)
                {
                    Tlog.GetInstance().WriteLog("Get Thong tin sp da Order that bai: " + ex.Message);
                    CloseDB();
                    return OderList;
                }

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

                cmd.Parameters.Add(new MySqlParameter("inIdItem", bOrderItem.BOrderIdItem));
                cmd.Parameters.Add(new MySqlParameter("inName", bOrderItem.BOrderName));
                cmd.Parameters.Add(new MySqlParameter("inNumber", bOrderItem.BOrderNum));
                cmd.Parameters.Add(new MySqlParameter("inPrice", bOrderItem.BOrderPrice));
                cmd.Parameters.Add(new MySqlParameter("inSize", bOrderItem.BOrderSize));
                cmd.Parameters.Add(new MySqlParameter("inSugarRate", bOrderItem.BOrderSugarRate));
                cmd.Parameters.Add(new MySqlParameter("inIceRate", bOrderItem.BOrderIceRate));
                cmd.Parameters.Add(new MySqlParameter("inTopping", bOrderItem.BOrderTopping));
                cmd.Parameters.Add(new MySqlParameter("inBillId", bOrderItem.BOrderBillId));
                cmd.Parameters.Add(new MySqlParameter("inOrderDate", bOrderItem.BOrderDate));
                cmd.Parameters.Add(new MySqlParameter("inKm", bOrderItem.BOrderKm));
                cmd.Parameters.Add(new MySqlParameter("inKmType", bOrderItem.BOrderKmType));
                try
                {
                    int result = cmd.ExecuteNonQuery();
                    bRet = true;
                }
                catch (System.Exception ex)
                {
                    Tlog.GetInstance().WriteLog("Add OrderItem that bai: " + ex.Message);
                    bRet = false;
                }
            }

            CloseDB();
            return bRet;
        }

        public bool EditOrderItem(BTeaOrderObject orderItem)
        {
            bool bRet = false;
            if (this.ConnectionDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand("EditOrderItem", _connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("inId", orderItem.BOrderId));
                cmd.Parameters.Add(new MySqlParameter("inIdItem", orderItem.BOrderIdItem));
                cmd.Parameters.Add(new MySqlParameter("inName", orderItem.BOrderName));
                cmd.Parameters.Add(new MySqlParameter("inNumber", orderItem.BOrderNum));
                cmd.Parameters.Add(new MySqlParameter("inPrice", orderItem.BOrderPrice));
                cmd.Parameters.Add(new MySqlParameter("inSize", orderItem.BOrderSize));
                cmd.Parameters.Add(new MySqlParameter("inSugarRate", orderItem.BOrderSugarRate));
                cmd.Parameters.Add(new MySqlParameter("inIceRate", orderItem.BOrderIceRate));
                cmd.Parameters.Add(new MySqlParameter("inTopping", orderItem.BOrderTopping));
                cmd.Parameters.Add(new MySqlParameter("inBillId", orderItem.BOrderBillId));
                cmd.Parameters.Add(new MySqlParameter("inOrderDate", orderItem.BOrderDate));
                cmd.Parameters.Add(new MySqlParameter("inKm", orderItem.BOrderKm));
                cmd.Parameters.Add(new MySqlParameter("inKmType", orderItem.BOrderKmType));
                int result = 0;
                try
                {
                    result = cmd.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    bRet = false;
                    Tlog.GetInstance().WriteLog("Edit Thong tin OrderItem that bai: " + ex.Message);
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

        public bool DeleteOrderItem(int Id)
        {

            bool bRet = false;
            if (this.ConnectionDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand("DeleteBteaOrderItem", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("inId", Id));

                int result = 0;
                try
                {
                    result = cmd.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    Tlog.GetInstance().WriteLog("Delete OrderItem that bai: " + ex.Message);
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
    }
}
