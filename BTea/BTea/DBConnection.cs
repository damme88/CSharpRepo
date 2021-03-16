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
            string query = "SELECT * FROM drinkTbl";

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
                   drObj.DRId       = sId;
                   drObj.DRName     = sName;
                   drObj.DRPrice    = sPrice;
                   drObj.DRNote     = sNote;

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

                cmd.Parameters.Add(new MySqlParameter("inName", drItem.DRName));
                cmd.Parameters.Add(new MySqlParameter("inPrice", drItem.DRPrice));
                cmd.Parameters.Add(new MySqlParameter("inNote", drItem.DRNote));

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

                int drId = Convert.ToInt32(drItem.DRId);
                cmd.Parameters.Add(new MySqlParameter("inId", drId));
                cmd.Parameters.Add(new MySqlParameter("inName", drItem.DRName));
                cmd.Parameters.Add(new MySqlParameter("inPrice", drItem.DRPrice));
                cmd.Parameters.Add(new MySqlParameter("inNote", drItem.DRNote));
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
    }
}
