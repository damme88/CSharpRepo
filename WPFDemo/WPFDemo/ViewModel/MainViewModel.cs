using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using WPFDemo.Base;
using Microsoft.Win32;
using System.Collections.ObjectModel;

namespace WPFDemo.ViewModel
{

    public class PersonInfo
    {
        public PersonInfo(string name, string age, string place)
        {
            _name = name;
            _age = age;
            _place = place;
        }

        private string _name="";
        public string Name
        {
            get { return _name; }
            set { value = _name; }
        }

        private string _age = "";
        public string Age
        {
            get { return _age; }
            set { value = _age; }
        }

        private string _place = "";
        public string Place
        {
            get { return _place; }
            set { value = Place; }
        }

    }

    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertiyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public RelayCommand NewCommand { set; get; }
        public RelayCommand OpenCommand { get; set; }
        public RelayCommand CalculateCommand { get; set; }
        public RelayCommand InputInfoCommand { get; set; }

        public MainViewModel()
        {
            NewCommand = new RelayCommand(new Action<object>(ImpNewFunction));
            OpenCommand = new RelayCommand(new Action<object>(ImpOpenFunction));
            CalculateCommand = new RelayCommand(new Action<object>(ImpCalculateFunction));
            InputInfoCommand = new RelayCommand(new Action<object>(ImpInputInfoFunction));

            _listItem = new ObservableCollection<PersonInfo>();
            _listItem.Add(new PersonInfo("Pham van a", "45", "HAI DUONG"));
            _listItem.Add(new PersonInfo("Pham van b", "25", "HAI DUONG"));
            _listItem.Add(new PersonInfo("Pham van c", "29", "HAI DUONG"));
        }

        public void ImpNewFunction(object obj)
        {
            MessageBox.Show("Welcome to phat trien phan mem 123az");
        }

        public void ImpOpenFunction(object obj)
        {
            OpenFileDialog fileDlg = new OpenFileDialog();
            if (fileDlg.ShowDialog() == true)
            {
                 string file_name = fileDlg.FileName;
                 MessageBox.Show(file_name);
            }
        }

        public void ImpCalculateFunction(object obj)
        {
            CalculateDialog dlg = new CalculateDialog();
            if (dlg.ShowDialog() == true)
            {
                MessageBox.Show("OK");
            }
        }

        public void ImpInputInfoFunction(object obj)
        {
            InputInfo dlg = new InputInfo();
            if (dlg.ShowDialog() == true)
            {
                MessageBox.Show("OK");
            }
        }

        private ObservableCollection<PersonInfo> _listItem;
        public ObservableCollection<PersonInfo> ListItem
        {
            get { return _listItem;}
            set { value = _listItem; }
        }

        private PersonInfo _peronItem;
        public PersonInfo PersonItem
        {
            get {return _peronItem;}
            set {value = _peronItem;}
        }
    }
}
