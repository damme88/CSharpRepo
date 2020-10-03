using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TApp.Base;
using TApp.Dialog;
using TBaseWrap;
namespace TApp.ViewModel
{
    class MainViewModel : TBaseVM
    {
        public MainViewModel()
        {
            _ribbonVM = new RibbonViewModel(RibbonCommand);
            _frSampleVM = new FrmSampleVM();
            TabItem sampleItem = new TabItem();
            sampleItem.Header = "Sample";
            FrmSample fLineObject = new FrmSample();
            fLineObject.DataContext = _frSampleVM;
            sampleItem.Content = fLineObject;

            _listItems = new ObservableCollection<TabItem>();
            _listItems.Add(sampleItem);
            _selectedItem = _listItems[0];
        }

        #region MEMBERS
        public RelayCommand TestCmd { set; get; }
        protected FrmSampleVM _frSampleVM;
        protected RibbonViewModel _ribbonVM;
        protected ObservableCollection<TabItem> _listItems;
        protected TabItem _selectedItem;
        #endregion

        #region PROPERTY
        public FrmSampleVM FrSampleVM
        {
            get
            {
                return _frSampleVM;
            }
            set
            {
                if (_frSampleVM != value)
                {
                    _frSampleVM = value;
                    OnPropertyChange("FrSampleVM");
                }
            }
        }


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


        public ObservableCollection<TabItem> ListItems
        {
            get { return _listItems; }
            set
            {
                if (_listItems != value)
                {
                    _listItems = value;
                    OnPropertyChange("ListItems");
                }
            }
        }


        public TabItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChange("SelectedItem");
                }
            }
        }
        #endregion

        #region METHOD
        public void RibbonCommand()
        {
            int cmdType = _ribbonVM.ComdType;
            if (cmdType == (int)RibbonViewModel.CommandType.CMD_LINE_FRM)
            {
                TabItem lineItem = new TabItem();
                lineItem.Header = "Line Form";
                lineItem.Content = new FrmLineObject();

                var item = _listItems.FirstOrDefault(x => x.Header == lineItem.Header);
                if (item == null)
                {
                    _listItems.Add(lineItem);
                    SelectedItem = _listItems.ElementAt(_listItems.Count - 1);
                }
                
            }
        }
        #endregion
    }
}
