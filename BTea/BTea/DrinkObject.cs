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

        private List<ToppingObject> _tpListObj;

        private int _size;
        private int _surgarRate;
        private int _iceRate;

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
