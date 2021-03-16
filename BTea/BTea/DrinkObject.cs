using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTea
{
    class DrinkObject
    {
        private string drId;
        private string drName;
        private string drPrice;
        private string drNote;
        public DrinkObject()
        {
            drId    = string.Empty;
            drName  = string.Empty;
            drPrice = string.Empty;
            drNote  = string.Empty;
        }
        #region Properties
        public string DRId
        {
            get { return drId; }
            set
            {
                drId = value;
            }
        }

        public string DRName
        {
            get { return drName; }
            set
            {
                drName = value;
            }
        }

        public string DRPrice
        {
            get { return drPrice; }
            set
            {
                drPrice = value;
            }
        }

        public string DRNote
        {
            get { return drNote; }
            set
            {
                drNote = value;
            }
        }
        #endregion
    }
}
