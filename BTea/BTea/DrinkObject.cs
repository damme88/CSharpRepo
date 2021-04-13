using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTea
{
    class DrinkObject : BTBaseObject
    {
        public DrinkObject()
        {
            _tpListObj = new List<ToppingObject>();
            Type = BTBaseObject.BTeaType.DRINK_TYPE;
        }

        #region MEMBERS
        private List<ToppingObject> _tpListObj;

        private int _size;
        private int _surgarRate;
        private int _iceRate;
        #endregion

        #region METHOD
        public string  SizeToString()
        {
            string str = "M";
            if (_size == 1)
            {
                str = "L";
            }
            return str;
        }

        public string SugarToString()
        {
            string str = "100%";
            if (_surgarRate == 1)       str = "30%";
            else if (_surgarRate == 2)  str = "50%";
            else if (_surgarRate == 3)  str = "70%";
            return str;
        }

        public string IceToString()
        {
            string str = "100%";
            if (_iceRate == 1)      str = "30%";
            else if (_iceRate == 2) str = "50%";
            else if (_iceRate == 3) str = "70%";
            return str;
        }

        public string ToppingToString()
        {
            string strTp = "";
            for (int j = 0; j < _tpListObj.Count; ++j)
            {
                ToppingObject tpObj = _tpListObj[j];
                strTp += tpObj.BName;
                if (j < _tpListObj.Count - 1)
                {
                    strTp += ",";
                }
            }
            return strTp;
        }
        #endregion
        #region PROPERTY
        public List<ToppingObject> TPListObj
        {
            get { return _tpListObj; }
            set { _tpListObj = value; }
        }

        public int DrinkSize
        {
            get { return _size; }
            set { _size = value; }
        }

        public int SugarRate
        {
            get { return _surgarRate; }
            set { _surgarRate = value; }
        }

        public int IceRate
        {
            get { return _iceRate; }
            set { _iceRate = value; }
        }
        #endregion
    }
}
