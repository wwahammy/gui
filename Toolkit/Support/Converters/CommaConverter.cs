using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;

namespace CoApp.Gui.Toolkit.Support.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(IEnumerable<string>), typeof(Inline))]
    public class CommaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var vals = value as IEnumerable<string>;


            //ooooh type safety
            return Convert(vals);

        }


        private IEnumerable<Inline> Convert(IEnumerable<string> licences)
        {
            if (licences == null || !licences.Any())
            {
                yield return new Run("");
                yield break;
            }

            var result = licences.Aggregate(new Span(), (accumulate, next) =>
            {

                accumulate.Inlines.Add(new Run(next));
                //accumulate.Inlines.Add(ConvertToHyperlink(next.Package, next.Navigate));
                accumulate.Inlines.Add(", ");
                return accumulate;
            });
            //remove last comma
            result.Inlines.Remove(result.Inlines.LastInline);


            yield return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
    
    
}
