using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TApp.Dialog;
using TApp.ViewModel;

namespace TApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer updateTimer = new DispatcherTimer();
        private MainViewModel _mainVM = null;
        HwndHost _mainHost;
        public MainWindow()
        {
            InitializeComponent();
            _mainVM = new MainViewModel();
            this.DataContext = _mainVM;
            Rbcontrol.DataContext = _mainVM.RibbonVM;
            _mainHost = new TBaseWrap.GlWrapperHwnd();
        }

        public override void BeginInit()
        {
            updateTimer.Interval = new TimeSpan(160000);
            updateTimer.Tick += new EventHandler(updateTimer_Tick);
            updateTimer.Start();
            base.BeginInit();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            glView.Child = _mainHost;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                this.Close();
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!e.Cancel)
            {
                if (null != updateTimer)
                {
                    updateTimer.Stop();
                    updateTimer = null;
                }
            }
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            if (null != glView &&
                null != glView.Child)
            {
                glView.Child.InvalidateVisual();
            }
        }
    }
}
