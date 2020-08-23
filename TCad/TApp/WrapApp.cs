using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBaseWrap;

namespace TApp
{
    public sealed class  WrapApp
    {
        private static readonly WrapApp _instance = new WrapApp();
        private GlWrapperData _wrappData;

        static WrapApp()
        {

        }

        private WrapApp()
        {
            _wrappData = new GlWrapperData();
        }

        public static WrapApp Instance
        {
            get { return _instance; }
        }

        public GlWrapperData WrappGl
        {
            get { return _wrappData; }
        }
    }
}
