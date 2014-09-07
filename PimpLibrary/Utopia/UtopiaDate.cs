using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PimpLibrary.Utopia
{
    public class UtopiaDate
    {
        public UtopiaDate() { }
        public UtopiaDate(int year, int month, int day)
        {
            Year = year;
            Month = month;
            Day = day;
        }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }
}
