using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using CoApp.Updater.Model.Interfaces;

namespace CoApp.Updater.Support.Converters
{
    public class DaysOfWeekToTextConverter : IValueConverter
    {

        private readonly static Dictionary<UpdateDayOfWeek, string> DayToText = new Dictionary<UpdateDayOfWeek, string>
                                                                             {
                                                                                 {
                                                                                     UpdateDayOfWeek.Everyday,
                                                                                     Resources.Strings_en_US.Everyday
                                                                                     //ObjectExtensions.GetUiString(null,"CoApp.Updater:Strings:Everyday")
                                                                                     },
                                                                                 {
                                                                                     UpdateDayOfWeek.Sunday,
                                                                                     Resources.Strings_en_US.Sunday
                                                                                     //ObjectExtensions.GetUiString(null,"CoApp.Updater:Strings:Sunday")
                                                                                     },
                                                                                 {
                                                                                     UpdateDayOfWeek.Monday,
                                                                                     Resources.Strings_en_US.Monday
                                                                                     //ObjectExtensions.GetUiString(null,"CoApp.Updater:Strings:Monday")
                                                                                     },
                                                                                 {
                                                                                     UpdateDayOfWeek.Tuesday,
                                                                                     Resources.Strings_en_US.Tuesday
                                                                                     //ObjectExtensions.GetUiString(null,"CoApp.Updater:Strings:Tuesday")
                                                                                     },
                                                                                 {
                                                                                     UpdateDayOfWeek.Wednesday,
                                                                                     Resources.Strings_en_US.Wednesday
                                                                                     //ObjectExtensions.GetUiString(null,"CoApp.Updater:Strings:Wednesday")
                                                                                     },
                                                                                 {
                                                                                     UpdateDayOfWeek.Thursday,
                                                                                     Resources.Strings_en_US.Thursday
                                                                                     //ObjectExtensions.GetUiString(null,"CoApp.Updater:Strings:Thursday")
                                                                                     },
                                                                                 {
                                                                                     UpdateDayOfWeek.Friday,
                                                                                     Resources.Strings_en_US.Friday
                                                                                     //ObjectExtensions.GetUiString(null,"CoApp.Updater:Strings:Friday")
                                                                                     },
                                                                                 {
                                                                                     UpdateDayOfWeek.Saturday,
                                                                                     Resources.Strings_en_US.Saturday
                                                                                     //ObjectExtensions.GetUiString(null,"CoApp.Updater:Strings:Saturday")
                                                                                     }
                                                                             };

        private static readonly Dictionary<string, UpdateDayOfWeek> TextToDay;
 
        static DaysOfWeekToTextConverter()
        {
            TextToDay = DayToText.ToDictionary(x => x.Value, x => x.Key);

        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is UpdateDayOfWeek)
            {
                var day = (UpdateDayOfWeek) value;
                if (DayToText.ContainsKey(day))
                    return DayToText[day];

            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                var day = (string)value;
                if (TextToDay.ContainsKey(day))
                    return TextToDay[day];

            }
            return value;
        }
    }
}
