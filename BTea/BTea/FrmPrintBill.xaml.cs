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
    /// Interaction logic for FrmPrintBill.xaml
    /// </summary>
    public partial class FrmPrintBill : Window
    {
        public FrmPrintBill()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FrmPrintBillVM printVM = DataContext as FrmPrintBillVM;
            try
            {
                myScrollViewer1.ScrollToTop();

                if (printVM != null && printVM.IsPrintDrinkBill == 1)
                {
                    myScrollViewer2.ScrollToTop();
                }
                
                this.IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(printBill, "Total_Bill");

                    if (printVM != null && printVM.IsPrintDrinkBill == 1)
                    {
                        printDialog.PrintVisual(printBill2, "Drink Bill");
                    }
                }
            }
            finally
            {
                this.IsEnabled = true;
            }
        }
    }
}
