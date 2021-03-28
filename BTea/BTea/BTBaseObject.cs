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
        protected double bPrice;
        protected string bNote;
        public BTBaseObject()
        {
            bId = string.Empty;
            bName = string.Empty;
            bPrice = 0.0;
            bNote = string.Empty;
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

        public double BPrice
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
        #endregion
    }
}
