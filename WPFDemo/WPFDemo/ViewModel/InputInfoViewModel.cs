using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDemo.Base;

namespace WPFDemo.ViewModel
{

    public class Place
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value;}
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }
    }

    class InputInfoViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertiyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public InputInfoViewModel()
        {
            _listPlace = new ObservableCollection<Place>()
            {
                new Place{ Id = 1, Name="Ha Noi"},
                new Place {Id = 2, Name="Hai Duong"},
                new Place {Id = 3, Name = "Thai Binh"},
                new Place {Id = 4, Name = "Nam Dinh"}
            };

            _splace = _listPlace.ElementAt(0);
        }


        string name = "";
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertiyChange("Name");
                }
            }
        }

        string age = "";
        public string Age
        {
            get
            {
                return age;
            }
            set
            {
                if (value != age)
                {
                    age = value;
                    OnPropertiyChange("Age");
                }
            }
        }

        public int is_check = 1;
        public int IsCheck
        {
            get
            {
                return is_check;
            }
            set
            {
                if (value != is_check)
                {
                    is_check = value;
                    OnPropertiyChange("IsCheck");
                }
            }
        }


        private ObservableCollection<Place> _listPlace;
        public ObservableCollection<Place> ListPlace
        {
            get { return _listPlace; }
            set
            {
                _listPlace = value;
            }
        }

        private Place _splace;

        public Place SPlace
        {
            get 
            { 
                return _splace; 
            }
            set 
            { 
                _splace = value; 
            }
        } 

        //Programming skill

        private bool _cppChecked = false;
        public bool CppChecked
        {
            get { return _cppChecked; }
            set { _cppChecked = value; }
        }

        private bool _csharpChecked = false;
        public bool CSharpChecked
        {
            get { return _csharpChecked; }
            set { _csharpChecked = value; }
        }

        private bool _javaChecked = false;
        public bool JavaChecked
        {
            get { return _javaChecked; }
            set { _javaChecked = value; }
        }

        private bool _pythonChecked = false;
        public bool PythonChecked
        {
            get { return _pythonChecked; }
            set { _pythonChecked = value; }
        }
    }
}
