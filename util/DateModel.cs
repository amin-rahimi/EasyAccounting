using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAccounting.util
{
    public class DateModel
    {
        private int year;
        private int day;
        private int month;

        public DateModel(int year, int month, int day)
        {
            this.year = year;
            this.month = month;
            this.day = day;
        }

        public int Day
        {
            get { return day; }
            set { day = value; }
        }
        
        public int Month
        {
            get { return month; }
            set { month = value; }
        }
       
        public int Year
        {
            get { return year; }
            set { year = value; }
        }

       
       


    }
}
