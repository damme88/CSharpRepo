using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TApp.Base;
using TBaseWrap;
namespace TApp.ViewModel
{
    class MainViewModel : TBaseVM
    {
        public RelayCommand TestCmd { set; get; }
        public MainViewModel()
        {
            _ribbonVM = new RibbonViewModel();
        }

        public void UpdateData()
        {
        }

        protected RibbonViewModel _ribbonVM;
        public RibbonViewModel RibbonVM
        {
            get
            {
                return _ribbonVM;
            }
            set
            {
                if (_ribbonVM != value)
                {
                    _ribbonVM = value;
                    OnPropertyChange("RibbonVM");
                }
            }
        }
    }
}
