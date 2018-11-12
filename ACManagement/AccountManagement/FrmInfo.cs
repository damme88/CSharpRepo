using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using ObjectData;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace AccountManagement
{
    public partial class FrmInfo : Form
    {

        public FrmInfo()
        {
            InitializeComponent();
            ShowData();
        }

        public void ShowData()
        {
            DataSet ds = ConnectionDB.GetInfoList();
            dataGrdInfo.DataSource = ds.Tables[0];
            dataGrdInfo.Columns[0].Width = 40;
            dataGrdInfo.Columns[1].Width = 100;
            dataGrdInfo.Columns[2].Width = 100;
            dataGrdInfo.Columns[3].Width = 150;
            dataGrdInfo.Columns[4].Width = 100;
            dataGrdInfo.Columns[5].Width = 250;
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            FormData dlgFromData = new FormData();
            dlgFromData.TypeForm = 0;
            DialogResult result = dlgFromData.ShowDialog();
            if (result == DialogResult.OK)
            {
                ShowData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult ret = MessageBox.Show("Are you sure for removing selected row ?", "Delete Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (ret == DialogResult.OK)
            {
                int idx = dataGrdInfo.CurrentCell.RowIndex;
                int id = Convert.ToInt32(dataGrdInfo.Rows[idx].Cells[0].Value);

                if (id > 0)
                {
                    string strRet = ConnectionDB.DeleteAccount(id);
                    ShowData();
                }
                else
                {
                    MessageBox.Show("Data Invalid", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

           UpdateInfo();
        }

        public void UpdateInfo()
        {
             AccountInfo acInfo = new AccountInfo();

            int currentIdx = dataGrdInfo.CurrentCell.RowIndex;
            string Id = dataGrdInfo.Rows[currentIdx].Cells[0].Value.ToString();
            acInfo.Id = Convert.ToInt16(Id);
            acInfo.Address = dataGrdInfo.Rows[currentIdx].Cells[1].Value.ToString();
            acInfo.NickName = dataGrdInfo.Rows[currentIdx].Cells[2].Value.ToString();
            acInfo.Email = dataGrdInfo.Rows[currentIdx].Cells[3].Value.ToString();
            acInfo.Password = dataGrdInfo.Rows[currentIdx].Cells[4].Value.ToString();
            acInfo.Description = dataGrdInfo.Rows[currentIdx].Cells[5].Value.ToString();


            FormData dlgFromData = new FormData();
            dlgFromData.TypeForm = 1;
            dlgFromData.SetData(acInfo);
            DialogResult result = dlgFromData.ShowDialog();
            if (result == DialogResult.OK)
            {
                ShowData();
            }
        }

        private void dataGrdInfo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            UpdateInfo();
        }

        private void FrmInfo_Load(object sender, EventArgs e)
        {

        }

        private void txtFind_Change(object sender, EventArgs e)
        {
            if (txtFind.Text != string.Empty)
            {
                SqlDataAdapter adapt;
                DataTable dt = ConnectionDB.FindAddress(txtFind.Text);
                dataGrdInfo.DataSource = dt; 
            }
            else
            {
                ShowData();
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        //No use
        private void ConvertTableToList()
        {
            DataSet ds = ConnectionDB.GetInfoList();
            List<AccountInfo> acList = new List<AccountInfo>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                acList.Add(new AccountInfo
                {
                    Address = Convert.ToString(dr["Address"]),
                    NickName = Convert.ToString(dr["NickName"]),
                    Email = Convert.ToString(dr["Email"]),
                    Password = Convert.ToString(dr["Password"]),
                    Description = Convert.ToString(dr["Note"])
                });
            }
        }
    }
}
