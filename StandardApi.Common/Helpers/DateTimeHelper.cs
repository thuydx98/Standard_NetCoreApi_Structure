using System;
using System.Globalization;

namespace StandardApi.Common.Helpers
{
    public static class DateTimeHelper
    {
        public static int WorkDayOfMonth(int year, int month)
        {
            int days = 0;
            if (year > 0 && year < 10000 && month >= 1 && month <= 12)
                days = DateTime.DaysInMonth(year, month);
            else
                return -1;

            DateTime d;

            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                d = new DateTime(year, month, i);

                if ((d.ToString("ddd") == "Sat") || (d.ToString("ddd") == "Sun"))
                    days--;

            }
            return days;
        }

        public static decimal CurrentWorkDayOfMonth(int year, int month, DateTime startDate, DateTime endDate)
        {
            Decimal result = 0;

            // in year
            if (year == startDate.Year && year == endDate.Year)
            {
                if (month == startDate.Month)
                {

                    if (month == endDate.Month) result = CalculateWorkingDay(year, month, startDate.Day, endDate.Day);
                    else result = CalculateWorkingDay(year, month, startDate.Day, DateTime.DaysInMonth(year, month));
                }
                else
                {
                    if (month > startDate.Month && month < endDate.Month) result = CalculateWorkingDay(year, month, 1, DateTime.DaysInMonth(year, month));
                    else if (month == endDate.Month) result = CalculateWorkingDay(year, month, 1, endDate.Day);
                }
            }
            else
            {
                if (year == startDate.Year && year < endDate.Year)
                {
                    if (month == startDate.Month) result = CalculateWorkingDay(year, month, startDate.Day, DateTime.DaysInMonth(year, month));
                    else if (month > startDate.Month) result = CalculateWorkingDay(year, month, 1, DateTime.DaysInMonth(year, month));
                }
                else if (year == endDate.Year && year > startDate.Year)
                {
                    if (month == endDate.Month) result = CalculateWorkingDay(year, month, 1, endDate.Day);
                    else if (month < endDate.Month) result = CalculateWorkingDay(year, month, 1, DateTime.DaysInMonth(year, month));
                }
                else if (year > startDate.Year && year < endDate.Year) result = CalculateWorkingDay(year, month, 1, DateTime.DaysInMonth(year, month));
            }



            return result;
        }

        public static decimal CalculateWorkingDay(int year, int month, int start, int end)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            decimal result = 0;
            if (year > 0 && year < 10000 && month >= 1 && month <= 12)
            {
                for (int i = start; i <= end; i++)
                {
                    string a = month + "/" + i + "/" + year;
                    DateTime d = new DateTime(year, month, i);
                    //DateTime d = DateTime.ParseExact(a,"mmDDYYYY", provider );
                    if ((d.ToString("ddd") != "Sat") && (d.ToString("ddd") != "Sun"))
                        result++;
                }
            }
            else
                return -1;
            return result;

        }

        public static DateTime GetFirstDayOfWeek(this DateTime dayInWeek)
        {
            CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek.AddDays(1);
        }

        public static int GetOverlapDays(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            return new TimeSpan(Math.Max(Math.Min(end1.Ticks, end2.Ticks) - Math.Max(start1.Ticks, start2.Ticks) + TimeSpan.TicksPerDay, 0)).Days;
        }

        public static int NumberOfWeek(DateTime firstDayOfStartWeek, DateTime firstDayOfEndWeek)
        {
            return (firstDayOfEndWeek - firstDayOfStartWeek).Days / 7 + 1;
        }

        public static DateTime StartDateOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime EndDateOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
        }

        public static int NumberDayWeekendInRange(DateTime startDate, DateTime endDate)
        {
            int dayWeekend = 0;
            while (startDate <= endDate)
            {
                if (startDate.DayOfWeek == DayOfWeek.Saturday || startDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    dayWeekend++;
                }

                startDate = startDate.AddDays(1);
            }

            return dayWeekend;
        }
    }
}
