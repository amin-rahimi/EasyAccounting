using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAccounting.util
{
    public class MonthModel
    {
        private int monthInteger;

        public int MonthInteger
        {
            get { return monthInteger; }
            set { monthInteger = value; }
        }
        private string monthString;

        public string MonthString
        {
            get { return monthString; }
            set { monthString = value; }
        }

        public MonthModel(int monthInteger, string monthString)
        {
            this.monthInteger = monthInteger;
            this.monthString = monthString;
        }
    }
}
