using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using CoApp.Updater.Model.Interfaces;

namespace CoApp.Updater.Support.Converters
{
    [ValueConversion(typeof(UpdateChoice), typeof(string))]
    public class ChoiceToTextConverter : IValueConverter
    {


        private static readonly Dictionary<UpdateChoice, string> ChoiceToText;

       

        private static readonly Dictionary<string, UpdateChoice> TextToChoice;
 
        static ChoiceToTextConverter()
        {
         
            ChoiceToText =
                new Dictionary<UpdateChoice, string>
                    {
                        {UpdateChoice.AutoInstallAll,  Resources.Strings_en_US.AutoInstallAll
                            //ObjectExtensions.GetUiString(null,"CoApp.Updater:Strings:AutoInstallAll")
                            },
                        {UpdateChoice.AutoInstallJustUpdates, Resources.Strings_en_US.AutoInstallJustUpdates
                            //ObjectExtensions.GetUiString(null,"CoApp.Updater:Strings:AutoInstallJustUpdates")
                            },
                        {UpdateChoice.Notify, Resources.Strings_en_US.NotifyUpdates
//                            ObjectExtensions.GetUiString(null,"CoApp.Updater:StringsNotifyUpdates")
                            },
                        {UpdateChoice.Dont, Resources.Strings_en_US.NeverCheckForUpdates
                            
                            //ObjectExtensions.GetUiString(null,"CoApp.Updater:StringsNeverCheckForUpdates")
                            }
                    };

            TextToChoice = ChoiceToText.ToDictionary(x => x.Value, x => x.Key);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is UpdateChoice)
            {
                var day = (UpdateChoice)value;
                if (ChoiceToText.ContainsKey(day))
                    return ChoiceToText[day];

            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                var day = (string)value;
                if (TextToChoice.ContainsKey(day))
                    return TextToChoice[day];

            }
            return value;
        }
    }
}
