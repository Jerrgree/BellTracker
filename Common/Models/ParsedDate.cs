using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Common.Models
{
    public class ParsedDate
    {
        public DayOfWeek DayOfWeek { get; private set; }
        public string Year { get; private set; }
        public bool IsMorning { get; private set; }
        public int Week { get; private set; }

        public ParsedDate(DateTime dateTime)
        {
            DayOfWeek = dateTime.DayOfWeek;
            Year = dateTime.Year.ToString();
            IsMorning = dateTime.TimeOfDay < TimeSpan.FromHours(12);
            Week = CultureInfo
                .CurrentCulture
                .Calendar
                .GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }
    }
}