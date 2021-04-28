using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BTea
{
    class TConst
    {
        public const string K_MONEY_FORMAT = "#,##0;(#,##0)";
        public const string K_MONEY_FORMAT2 = "#,##0.00;(#,##0.00)";
        public const int K_KM_PERCENT = 0;
        public const int K_KM_VND = 1;
        public const int K_TK_TOTAL = 0;
        public const int K_TK_DAY = 1;
        public const int K_TK_MONTH = 2;
        public const int K_TK_YEAR = 3;
        static public int ConvertInt(string strVal)
        {
            int val = 0;
            try
            {
                val = Convert.ToInt32(strVal);
            }
            catch
            {
                val = 0;
            }
            return val;
        }

        static public int ConvertMoney(string input)
        {
            int iVal = 0;
            try
            {
                decimal dVal = decimal.Parse(Regex.Replace(input, @"[^\d.]", ""));
                iVal = Convert.ToInt32(dVal);
            }
            catch
            {
                iVal = 0;
            }

            return iVal;
        }
    }
}
