using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUIApp.Basic;
using MidWrapper;
using System.Windows;

namespace GUIApp.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        WrapperBasic pWrapper = new WrapperBasic();
        public RelayCommand TestCommand { set; get; }

        public MainViewModel()
        {
            TestCommand = new RelayCommand(new Action<object>(TestFunction));
            //pWrapper = new WrapperBasic();
        }

        public void TestFunction(object obj)
        {
            float a = 5;
            float b = 5;
            float value = pWrapper.CallSum(a, b);
            MessageBox.Show("Sum = " + value.ToString());
        }
    }
}
