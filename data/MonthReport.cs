using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAccounting.data
{
    public class MonthReport
    {
        private int month;
        private int income;
        public int Month
        {
            get { return month; }
            set { month = value; }
        }

        public int Income
        {
            get { return income; }
            set { income = value; }
        }
    }
}
