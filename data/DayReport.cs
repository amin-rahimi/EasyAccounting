using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAccounting.data
{
    public class DayReport
    {
        private int day;
        private int income;

        public int Day
        {
            get { return day; }
            set { day = value; }
        }
        

        public int Income
        {
            get { return income; }
            set { income = value; }
        }

    }
}
