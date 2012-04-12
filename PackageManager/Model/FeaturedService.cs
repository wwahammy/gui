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
using CoApp.Toolkit.Engine.Client;
using CoApp.Toolkit.Extensions;

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
           

            feeds.ContinueOnFail(e => { throw e.Unwrap();  }).RethrowWhenFaulted();

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
            var bitmapSource = LoadBitmap(p.Icon);
            return new ProductInfo
                       {
                           CanonicalName = p.CanonicalName,
                           Name = p.Name,
                           
                           Icon = bitmapSource,
                           Posted = DateTime.Parse(p.PublishDate),
                           Summary = p.Summary
                       };
        }

        private BitmapImage LoadBitmap(string base64String)
        {
            var bitmapSource = new BitmapImage();
            using (var mem = new MemoryStream(Convert.FromBase64String(base64String)))
            {

            bitmapSource.BeginInit();
            bitmapSource.CacheOption = BitmapCacheOption.OnLoad;
          
                bitmapSource.StreamSource = mem;
                bitmapSource.EndInit();
            }

            return bitmapSource;

        }
    }

   
}
