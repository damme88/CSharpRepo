using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using ObjectData;
using System.Configuration;
using System.Data.SqlClient;

namespace AccountManagement
{
    public class ConnectionDB
    {
        static public MySqlConnection GetConnectionDB()
        {
            MySqlConnection conn;
            String conString;

            string host = ConfigurationManager.AppSettings.Get("Host");
            string database = ConfigurationManager.AppSettings.Get("DBName");
            string port = ConfigurationManager.AppSettings.Get("Port");
            string username = ConfigurationManager.AppSettings.Get("User");
            string password = ConfigurationManager.AppSettings.Get("Password");
            if (host == string.Empty || 
                database == string.Empty ||
                port == string.Empty ||
                username == string.Empty ||
                password == string.Empty)
            {
                conString = "Server=127.0.0.1" +
                               ";Database=passdb" +
                               ";port=3306" +
                               ";User Id=root" +
                               ";password=123456" +
                               ";CharSet = utf8";
            }
            else
            {
                conString = "Server=" + host +
                               ";Database=" + database +
                               ";port=" + port +
                               ";User Id=" + username +
                               ";password=" + password +
                               ";CharSet = utf8";
            }
            
            conn = new MySqlConnection(conString);

            return conn;
        }

        static public DataSet GetInfoList()
        {
            MySqlConnection conn = ConnectionDB.GetConnectionDB();
            conn.Open();

            DataSet ds = new DataSet();
            MySqlCommand cmd = new MySqlCommand("GetInfoList", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);

            sda.Fill(ds);
            cmd.ExecuteNonQuery();
            conn.Close();

            return ds;
        }

        static public DataSet GetUserInfo()
        {
            DataSet ds = new DataSet();
            MySqlConnection conn = ConnectionDB.GetConnectionDB();
            conn.Open();

            MySqlCommand cmd = new MySqlCommand("GetUserInfo", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);

            sda.Fill(ds);
            cmd.ExecuteNonQuery();
            conn.Close();

            return ds;
        }

        static public string DeleteAccount(int id)
        {
            string Message;
            MySqlConnection conn = GetConnectionDB();
            conn.Open();

            MySqlCommand cmd = new MySqlCommand("DeleteInfo", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter("inId", id));

            int result = 0;
            try
            {
                result = cmd.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {

            }

            if (result == 1)
            {
                Message = "Successfully";
            }
            else
            {
                Message = " Failed";
            }
            conn.Close();
            return Message;
        }

        static public string UpdateAccountInfo(AccountInfo acInfo)
        {
            string Message;
            MySqlConnection conn = GetConnectionDB();
            conn.Open();

            MySqlCommand cmd = new MySqlCommand("UpdateInfo", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new MySqlParameter("inAddress", acInfo.Address));
            cmd.Parameters.Add(new MySqlParameter("inNickName", acInfo.NickName));
            cmd.Parameters.Add(new MySqlParameter("inEmail", acInfo.Email));
            cmd.Parameters.Add(new MySqlParameter("inPassword", acInfo.Password));
            cmd.Parameters.Add(new MySqlParameter("inNote", acInfo.Description));
            cmd.Parameters.Add(new MySqlParameter("inId", acInfo.Id));
            int result = 0;
            try
            {
                result = cmd.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
            }

            if (result == 1)
            {
                Message = "Successfully";
            }
            else
            {
                Message = " Failed";
            }
            conn.Close();
            return Message;

        }

        static public string InsertAccountInfo(AccountInfo acInfo)
        {
            string Message;
            MySqlConnection conn = GetConnectionDB();
            conn.Open();

            MySqlCommand cmd = new MySqlCommand("AddInfo", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new MySqlParameter("inAddress", acInfo.Address));
            cmd.Parameters.Add(new MySqlParameter("inNickName", acInfo.NickName));
            cmd.Parameters.Add(new MySqlParameter("inEmail", acInfo.Email));
            cmd.Parameters.Add(new MySqlParameter("inPassword", acInfo.Password));
            cmd.Parameters.Add(new MySqlParameter("inNote", acInfo.Description));

            int result = 0;
            try
            {
                result = cmd.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
            }

            if (result == 1)
            {
                Message = "Successfully";
            }
            else
            {
                Message = " Failed";
            }
            conn.Close();
            return Message;
        }

        static public DataTable FindAddress(string text)
        {
            MySqlConnection conn = GetConnectionDB();
            MySqlDataAdapter adapt;
            DataTable dt;

            adapt = new MySqlDataAdapter("select * from accountinfo where Address like '" + text + "%'", conn);
            dt = new DataTable();
            adapt.Fill(dt);
            return dt;
        }
    }
}
