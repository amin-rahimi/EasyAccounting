using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAccounting.data
{
    public class YearReport
    {
        private int year;
        private int income;

        public int Year
        {
            get { return year; }
            set { year = value; }
        }

        public int Income
        {
            get { return income; }
            set { income = value; }
        }
    }
}
