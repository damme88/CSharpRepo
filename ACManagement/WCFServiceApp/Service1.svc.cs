using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using ObjectData;
using MySql.Data.MySqlClient;


namespace WCFServiceApp
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public static MySqlConnection GetDBConnection()
        {
            string host = "127.0.0.1";
            string database = "passdb";
            string port = "3306";
            string username = "root";
            string password = "123456";

            String conString = "Server=" + host + ";Database=" + database
                + ";port=" + port + ";User Id=" + username + ";password=" + password + ";CharSet = utf8";
            MySqlConnection con = new MySqlConnection(conString);

            return con;
        }

        public DataSet GetUserInfo()
        {
            MySqlConnection conn = GetDBConnection();
            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("GetUserInfo", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);

                sda.Fill(ds);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (System.Exception ex)
            {

            }

            return ds;
        }


        public DataSet SelectAccountInfo()
        {
            MySqlConnection conn = GetDBConnection();
            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("GetInfoList", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);

                sda.Fill(ds);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (System.Exception ex)
            {
            	
            }
            
            return ds;
        }

        public string UpdateAccountInfo(AccountInfo acInfo)
        {
            string Message;
            MySqlConnection conn = GetDBConnection();
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

        public string InsertAccountInfo(AccountInfo acInfo)
        {
            string Message;
            MySqlConnection conn = GetDBConnection();
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


        public string DeleteAccountInfo(Int32 idInfo)
        {
            string Message;
            MySqlConnection conn = GetDBConnection();
            conn.Open();

            MySqlCommand cmd = new MySqlCommand("DeleteInfo", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter("inId", idInfo));

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
    }
}
