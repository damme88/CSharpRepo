using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BTea
{
    /// <summary>
    /// Interaction logic for FrmEditBill.xaml
    /// </summary>
    public partial class FrmEditBill : Window
    {
        public FrmEditBill()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FrmEditBillVM pVm = DataContext as FrmEditBillVM;
            if (pVm != null)
            {
                pVm.FreeDelList();
            }
            this.Close();
        }
    }
}
