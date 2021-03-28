using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TApp.Base;

namespace BTea
{
    public class DrinkItem
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Price { get; set; }

        public string Note { get; set; }
    }
    class FrmDrinkMainVM : TBaseVM
    {
        public FrmDrinkMainVM()
        {
            CodeDrink = "DR";
            GetDataDrinkFromDB();

            CmdAddDrink = new RelayCommand(new Action<object>(DoAddDrink));
            CmdEditDrink = new RelayCommand(new Action<object>(DoEditDrink));
            CmdDeleteDrink = new RelayCommand(new Action<object>(DoDeleteDrink));

            _frmDrinkVM = new FrmDrinkVM(DrinkAddEditDoneCmd);
        }

        #region MEMBERS
        public RelayCommand CmdAddDrink { set; get; }
        public RelayCommand CmdEditDrink { set; get; }
        public RelayCommand CmdDeleteDrink { set; get; }

        private FrmDrinkVM _frmDrinkVM;
        private string CodeDrink;
        private FrmDrink frmDrink;
        private int _drinkCount;
        #endregion

        #region Properties

        public int DrinkCount
        {
            get { return _drinkCount; }
            set
            {
                _drinkCount = value;
                OnPropertyChange("DrinkCount");
            }
        }
        private DrinkItem _selecteItem;
        public DrinkItem SelectedItemDrink
        {
            get { return _selecteItem; }
            set
            {
                _selecteItem = value;
                OnPropertyChange("SelectedItemDrink");
            }
        }

        private ObservableCollection<DrinkItem> _drinkItem;
        public ObservableCollection<DrinkItem> DrinkItems
        {
            get { return _drinkItem; }
            set
            {
                _drinkItem = value;
                OnPropertyChange("DrinkItems");
            }
        }
        #endregion

        #region Method
        public void DoAddDrink(object sender)
        {
            if (_frmDrinkVM != null)
            {
                _frmDrinkVM.DrinkStateAdd = System.Windows.Visibility.Visible;
                _frmDrinkVM.DrinkStateEdit = System.Windows.Visibility.Collapsed;

                _frmDrinkVM.SetData(string.Empty, string.Empty, string.Empty, string.Empty);
                frmDrink = new FrmDrink();
                frmDrink.DataContext = _frmDrinkVM;
                if (frmDrink.ShowDialog() == true)
                {
                    frmDrink.Close();
                    GetDataDrinkFromDB();
                    OnPropertyChange("DrinkItems");
                    OnPropertyChange("DrinkCount");
                }
            }
        }
        public void DoEditDrink(object sender)
        {
            DrinkItem drItem = _selecteItem;
            if (drItem != null && _frmDrinkVM != null)
            {
                frmDrink = new FrmDrink();
                frmDrink.DataContext = _frmDrinkVM;

                _frmDrinkVM.DrinkStateAdd = System.Windows.Visibility.Collapsed;
                _frmDrinkVM.DrinkStateEdit = System.Windows.Visibility.Visible;

                _frmDrinkVM.SetData(drItem.Id, drItem.Name, drItem.Price, drItem.Note);

                if (frmDrink.ShowDialog() == true)
                {
                    frmDrink.Close();
                    GetDataDrinkFromDB();
                    OnPropertyChange("DrinkItems");
                }
            }
        }

        public void DoDeleteDrink(object sender)
        {
            DrinkItem drItem = _selecteItem;
            if (drItem != null)
            {
                string strId = drItem.Id;
                string subId = strId.Replace("DR", "");
                int nId = 0;
                try
                {
                    nId = Convert.ToInt32(subId);
                }
                catch (Exception ex)
                {
                    nId = -1;
                }

                if (nId > 0)
                {
                    bool bRet = DBConnection.GetInstance().DeleteDrinkItem(nId);
                    if (bRet == true)
                    {
                        GetDataDrinkFromDB();
                        OnPropertyChange("DrinkItems");
                        OnPropertyChange("DrinkCount");
                    }
                }
            }
        }
        public void DrinkAddEditDoneCmd()
        {
            if (frmDrink != null)
            {
                frmDrink.DialogResult = true;
            }
        }
        public void GetDataDrinkFromDB()
        {
            List<DrinkObject> data_list = DBConnection.GetInstance().GetDataDrink();
            _drinkItem = new ObservableCollection<DrinkItem>();
            for (int i = 0; i < data_list.Count; ++i)
            {
                DrinkItem drItem = new DrinkItem();
                DrinkObject drObject = data_list[i];
                drItem.Id = CodeDrink + drObject.BId;
                drItem.Name = drObject.BName;
                drItem.Price = drObject.BPrice.ToString();
                drItem.Note = drObject.BNote;

                _drinkItem.Add(drItem);
            }

            _drinkCount = data_list.Count;
        }
        #endregion
    }
}
