using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace AccountManagement
{
    public partial class loginForm : Form
    {
        public loginForm()
        {
            InitializeComponent();
            InitData();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = ConnectionDB.GetUserInfo();
                string dbName = ds.Tables[0].Rows[0][0].ToString();
                string dbPassword = ds.Tables[0].Rows[0][1].ToString();

                string strName = txtUser.Text;
                string strPass = txtPass.Text;

                //bool loginSucces = false;
                if (dbName.ToUpper() == strName.ToUpper() && strPass == dbPassword)
                {
                    SaveData();
                    this.Visible = false;
                    FrmInfo frmInfo = new FrmInfo();
                    DialogResult ret = frmInfo.ShowDialog();
                    if (ret == DialogResult.Cancel)
                    {
                        this.Visible = true;
                    }
                }
                else
                {
                    MessageBox.Show("Login User Information is not correct!", "Login Result", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (System.Exception ex)
            {

            }
        }

        private static void SaveSetting(string key, string value)
        {
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loginForm_Load(object sender, EventArgs e)
        {
            this.AcceptButton = btnLogin;
        }



        private void InitData()
        {
            if (Properties.Settings.Default.UserName != string.Empty)
            {
                if (Properties.Settings.Default.Remember == "yes")
                {
                    txtUser.Text = Properties.Settings.Default.UserName;
                    txtPass.Text = Properties.Settings.Default.Password;
                    cbRemember.Checked = true;
                }
                else
                {
                    txtUser.Text = Properties.Settings.Default.UserName;
                }
            }
        }

        private void SaveData()
        {
            if (cbRemember.Checked)
            {
                Properties.Settings.Default.UserName = txtUser.Text;
                Properties.Settings.Default.Password = txtPass.Text;
                Properties.Settings.Default.Remember = "yes";
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.UserName = txtUser.Text;
                Properties.Settings.Default.Password = "";
                Properties.Settings.Default.Remember = "no";
                Properties.Settings.Default.Save();
            }
        }
    }
}
