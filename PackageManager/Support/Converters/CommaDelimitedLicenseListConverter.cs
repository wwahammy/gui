using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using CoApp.PackageManager.Model;
using CoApp.PackageManager.ViewModel;
using CoApp.Packaging.Common.Model;

namespace CoApp.PackageManager.Support.Converters
{
    [ValueConversion(typeof(IEnumerable<License>), typeof(Inline))]
    public class CommaDelimitedLicenseListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var vals = value as IEnumerable<License>;
            


            //ooooh type safety
            return Convert(vals);

        }


        private IEnumerable<Inline> Convert(IEnumerable<License> licences)
        {
            if (licences == null || !licences.Any())
            {
                yield return new Run("");
                yield break;
            }

            var result = licences.Aggregate(new Span(), (accumulate, next) =>
                                                            {

                                                                accumulate.Inlines.Add(new Run(next.Name));
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
