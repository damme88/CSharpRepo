using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccountManagement
{
    public partial class loginForm : Form
    {

        ServiceReference1.Service1Client obj = new ServiceReference1.Service1Client();

        public loginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            DataSet ds = obj.GetUserInfo();
            string dbName = ds.Tables[0].Rows[0][0].ToString();
            string dbPassword = ds.Tables[0].Rows[0][1].ToString();

            string strName = txtUser.Text;
            string strPass = txtPass.Text;

            //bool loginSucces = false;
            if (dbName.ToUpper() == strName.ToUpper() && strPass == dbPassword)
            {
                //loginSucces = true;
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loginForm_Load(object sender, EventArgs e)
        {
            this.AcceptButton = btnLogin;
        }
    }
}
