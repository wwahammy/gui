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

namespace CoApp.PackageManager.Support.Converters
{
    [ValueConversion(typeof(IEnumerable<PackageToCommand>), typeof(Inline))]
    public class CommaDelimitedPackageListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var vals = value as IEnumerable<PackageToCommand>;
            


            //ooooh type safety
            return Convert(vals);

        }


        private IEnumerable<Inline> Convert(IEnumerable<PackageToCommand> products)
        {
            if (products == null ||!products.Any())
            {
                yield return new Run("");
                yield break;
            }

            var result = products.Aggregate(new Span(), (accumulate, next) =>
                                                        {

                                                            accumulate.Inlines.Add(ConvertToHyperlink(next.Package, next.Navigate));
                                                            accumulate.Inlines.Add(", ");
                                                            return accumulate;
                                                        });
            //remove last comma
            result.Inlines.Remove(result.Inlines.LastInline);


            yield return result;
        }

        private static Hyperlink ConvertToHyperlink(ProductInfo next, ICommand command)
        {
            var h = new Hyperlink {Command = command};

            h.Inlines.Add(new Run(next.Name + "(" + next.Version.ToString() + ")"));

            return h;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
