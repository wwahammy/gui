using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Gui.Toolkit.Support;
using CoApp.PackageManager.Model.Interfaces;
using CoApp.PackageManager.Properties;
using CoApp.Packaging.Client;
using CoApp.Toolkit.Extensions;
using CoApp.PackageManager.Support;

namespace CoApp.PackageManager.Model
{
    public class FeaturedService : IFeaturedService
    {
        internal ICoAppService CoApp;
        public FeaturedService()
        {
            CoApp = new LocalServiceLocator().CoAppService;
        }

        public Task<IEnumerable<SectionFeature>> GetSections()
        {

            return Task.Factory.StartNew(() => GetSectionsInternal());
           

        }

        private IEnumerable<SectionFeature> GetSectionsInternal()
        {
            var feeds = CoApp.SystemFeeds;


            feeds.ContinueOnFail(e =>
                                     {
                                         throw e.Unwrap();
                                     });

            return feeds.Continue(enumerable =>
                {
                    
                    var getFeedPackages =
                        enumerable.Select(s =>new FeedAndPackages {Feed=s,  Packages= CoApp.GetPackages("*", locationFeed: s)});
                    return getFeedPackages.Select(t => t.Packages).ContinueAlways(
                        tasks => FeedsAndPackages(getFeedPackages));


                }
               ).Result;
        }

        
        class FeedAndPackages
        {
            public string Feed { get; set; }

            public Task<IEnumerable<Package>>  Packages { get; set; }
        }
        
        private IEnumerable<SectionFeature> FeedsAndPackages(IEnumerable<FeedAndPackages> feeds)
        {
            foreach (var f in feeds)
            {
                if (!f.Packages.IsFaulted && !f.Packages.IsCanceled && f.Packages.Result.Any())
                {
                    var packages = f.Packages.Result;
                    switch (packages.Count())
                    {
                        case 1:
                            yield return new SectionFeature {Name = f.Feed, TopLeft = ConvertPackageToProductInfo( packages.First())};
                            break;
                        case 2:
                            yield return new SectionFeature {Name = f.Feed, TopLeft = ConvertPackageToProductInfo( packages.First()), BottomLeft = ConvertPackageToProductInfo( packages.Skip(1).First())};
                            break;
                        case 3:
                            yield return new SectionFeature
                                             {
                                                 Name = f.Feed,
                                                 TopLeft = ConvertPackageToProductInfo(packages.First()),
                                                 BottomLeft = ConvertPackageToProductInfo(packages.Skip(1).First()),
                                                 BottomCenter = ConvertPackageToProductInfo(packages.Skip(2).First())
                                             };
                            break;
                        default:
                            yield return new SectionFeature
                                             {
                                                 Name = f.Feed,
                                                 TopLeft = ConvertPackageToProductInfo(packages.First()),
                                                 BottomLeft = ConvertPackageToProductInfo(packages.Skip(1).First()),
                                                 BottomCenter = ConvertPackageToProductInfo(packages.Skip(2).First()),
                                                 BottomRight = ConvertPackageToProductInfo(packages.Skip(3).First())
                                             };
                            break;

                    }
                }
            }
        }

        private ProductInfo ConvertPackageToProductInfo(Package p)
        {
            //var bitmapSource = LoadBitmap(p.PackageDetails.Icons[0]);
            return new ProductInfo
                       {
                           CanonicalName = p.CanonicalName,
                           Name = p.Name,
                           
                         // Icon = bitmapSource,
                           Posted = p.PackageDetails.PublishDate,
                           Summary = p.PackageDetails.SummaryDescription
                       };
        }

        private BitmapSource LoadBitmap(string base64String)
        {


            try
            {
                var array = Convert.FromBase64String(base64String);
                using (var mem = new MemoryStream(array))
                {
                    var bitmapSource = new BitmapImage();
                    bitmapSource.BeginInit();
                    bitmapSource.CacheOption = BitmapCacheOption.OnLoad;

                    bitmapSource.StreamSource = mem;
                    bitmapSource.EndInit();
                    bitmapSource.Freeze();
                    return bitmapSource;
                }
                

            }
            catch (Exception)
            {
                 
                using (var mem = new MemoryStream(Resources.software))
                {
                    var bitmapSource = new BitmapImage();
                    bitmapSource.BeginInit();
                    bitmapSource.CacheOption = BitmapCacheOption.OnLoad;

                    bitmapSource.StreamSource = mem;
                    bitmapSource.EndInit();
                    bitmapSource.Freeze();
                    return bitmapSource;
                }
            }

        }
    }

   
}
