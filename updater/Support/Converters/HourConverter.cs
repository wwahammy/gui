using System;
using System.Globalization;
using System.Windows.Data;

namespace CoApp.Updater.Support.Converters
{
    [ValueConversion(typeof(int), typeof(string))]
    public class HourConverter : IValueConverter
    {

        //private const string SIMPLETIMEFORMAT = "h tt";

        private static readonly DateTimeFormatInfo dtfi = new DateTimeFormatInfo() {ShortTimePattern = "h tt"};
        /// <summary>
        /// take in an hour in 24 hour time and spit out a nice time result, ie: 2 AM
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           
            if (value is int)
            {
                var hour = (int) value;
                if (hour < 0 || hour > 23)
                    return value;

                var datetime = new DateTime(1, 1, 1, hour, 0, 0);
                return datetime.ToString("t", dtfi);

            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var time = value as string;
            if (time != null)
            {
                DateTime dateHour;
                var worked = DateTime.TryParse(time, dtfi, DateTimeStyles.None, out dateHour);
                if (!worked)
                {
                    return value;
                }

                return dateHour.Hour;
            }

            return value;
        }
    }
}
