using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ObjectData;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace AccountManagement
{
    public partial class FormData : Form
    {
        private int type_form;

        public int TypeForm
        {
            set { type_form = value; }
            get { return type_form; }
        }

        public FormData()
        {
            InitializeComponent();
            type_form = -1;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = ConnectionDB.GetConnectionDB();
            conn.Open();


            AccountInfo objInfo = new AccountInfo(); // add type reference

            if (txtAddress.Text.Length > 0 && txtNickName.Text.Length > 0 && txtPassword.Text.Length > 0)
            {
                objInfo.Address = txtAddress.Text;
                objInfo.NickName = txtNickName.Text;
                objInfo.Email = txtEmail.Text;
                objInfo.Password = txtPassword.Text;
                objInfo.Description = txtNote.Text;

                if (type_form == 0)
                {
                    string strRet = ConnectionDB.InsertAccountInfo(objInfo);
                }
                else if (type_form == 1)
                {
                    objInfo.Id = Convert.ToInt16(txtId.Text);
                    string strRet = ConnectionDB.UpdateAccountInfo(objInfo);
                }

                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Data Invalid", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FormData_Load(object sender, EventArgs e)
        {

        }

        public void SetData(AccountInfo acInfo)
        {
            txtId.Text = acInfo.Id.ToString();
            txtAddress.Text = acInfo.Address;
            txtNickName.Text = acInfo.NickName;
            txtEmail.Text = acInfo.Email;
            txtPassword.Text = acInfo.Password;
            txtNote.Text = acInfo.Description;
        }
    }
}
