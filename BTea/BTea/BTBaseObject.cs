using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTea
{
    class BTBaseObject
    {
        protected string bId;
        protected string bName;
        protected int bPrice;
        protected string bNote;
        public BTBaseObject()
        {
            bId = string.Empty;
            bName = string.Empty;
            bPrice = 0;
            bNote = string.Empty;
        }

        public enum BTeaType
        {
            DRINK_TYPE = 0,
            FOOD_TYPE = 1,
            TOPPING_TYPE = 2,
            OTHER_TYPE = 3,
        }

        #region Properties
        public string BId
        {
            get { return bId; }
            set
            {
                bId = value;
            }
        }

        public string BName
        {
            get { return bName; }
            set
            {
                bName = value;
            }
        }

        public int BPrice  // gia goc 1 sp (base price)
        {
            get { return bPrice; }
            set
            {
                bPrice = value;
            }
        }

        public string BNote
        {
            get { return bNote; }
            set
            {
                bNote = value;
            }
        }

        public BTeaType Type { set; get; }
        #endregion
    }
}
