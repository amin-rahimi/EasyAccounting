using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyAccounting.util
{
    public class PersianDateFormatter
    {
        public List<MonthModel> getPerisanMonthsNames()
        {
            List<MonthModel> months = new List<MonthModel>(); 
            months.Add(new MonthModel(1,"فروردین"));
            months.Add(new MonthModel(2,"اردیبهشت"));
            months.Add(new MonthModel(3,"خرداد"));
            months.Add(new MonthModel(4,"تیر"));
            months.Add(new MonthModel(5,"مرداد"));
            months.Add(new MonthModel(6,"شهریور"));
            months.Add(new MonthModel(7,"مهر"));
            months.Add(new MonthModel(8,"آبان"));
            months.Add(new MonthModel(9,"آذر"));
            months.Add(new MonthModel(10,"دی"));
            months.Add(new MonthModel(11,"بهمن"));
            months.Add(new MonthModel(12,"اسفند"));
            return months;
        }

        public int getDateInteger(DateTime dt)
        {
            PersianCalendar pc = new PersianCalendar();
            StringBuilder sb = new StringBuilder();
            int month = pc.GetMonth(dt);
            int day = pc.GetDayOfMonth(dt);
            int year = pc.GetYear(dt);
            sb.Append(year);
            if (month < 10)
            {
                sb.Append("0" + month);
            }
            else
            {
                sb.Append(month);
            }
            if (day < 10)
            {
                sb.Append("0" + day);
            }
            else
            {
                sb.Append(day);
            }
            return Convert.ToInt32(sb.ToString());
        }

        public String getDateString(DateTime dt)
        {
            PersianCalendar pc = new PersianCalendar();
            StringBuilder sb = new StringBuilder();
            int month = pc.GetMonth(dt);
            int day = pc.GetDayOfMonth(dt);
            int year = pc.GetYear(dt);
            sb.Append(year);
            sb.Append("/");
            sb.Append(month);
            sb.Append("/");
            sb.Append(day);
            return sb.ToString();
        }

        public String convert(int date)
        {
            String dateString = Convert.ToString(date);
            StringBuilder sb = new StringBuilder();
            String year = dateString.Substring(0, 4);
            String month = "";
            String day = "";
            if (dateString[4] == '0')
            {
                month = dateString.Substring(5, 1);
            }
            else
            {
                month = dateString.Substring(4, 2);
            }
            if (dateString[6] == '0')
            {
                day = dateString.Substring(7, 1);
            }
            else
            {
                day = dateString.Substring(6, 2);
            }
            sb.Append(year);
            sb.Append("/");
            sb.Append(month);
            sb.Append("/");
            sb.Append(day);

            return sb.ToString();
        }

        public int convert(String date)
        {
            StringBuilder sb = new StringBuilder();
            String[] dateSplited = date.Split('/');
            String year = dateSplited[0];
            String month = "";
            String day = "";
            if (dateSplited[1].Length < 2)
            {
                month = "0" + dateSplited[1];
            }
            else
            {
                month = dateSplited[1];
            }
            if (dateSplited[2].Length < 2)
            {
                day = "0" + dateSplited[2];
            }
            else
            {
                day = dateSplited[2];
            }
            sb.Append(year);
            sb.Append(month);
            sb.Append(day);

            return Convert.ToInt32(sb.ToString());
        }

        public DateModel getSplittedDateIntegers(int date)
        {

            int day = date % 100;
            int temp = date % 10000;
            int month = temp /100;
            int year = date / 10000;
            DateModel dm = new DateModel(year,month,day);
            return dm;
        }
    }
}
