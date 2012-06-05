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
using CoApp.Packaging.Common;
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

            return null;
            //return Task.Factory.StartNew(() => GetSectionsInternal());
           

        }
        /*
        private IEnumerable<SectionFeature> GetSectionsInternal()
        {
            var feeds = CoApp.SystemFeeds;


            feeds.ContinueOnFail(e =>
                                     {
                                         throw e.Unwrap();
                                     });

            return feeds.Continue(enumerable =>
                {
                    var hmmm = new List<FeedAndPackages>()
                    foreach 
                    var getFeedPackages =
                        enumerable.Select(s =>new FeedAndPackages {Feed=s,  
                            Packages= CoApp.GetPackages("*", collectionFilter: p => p.HighestPackages(), locationFeed: s)});

                    return getFeedPackages.Select(t => t.Packages).ContinueAlways(
                        tasks => FeedsAndPackages(getFeedPackages));


                }
               ).Result;
        }*/

        public Task<SectionFeature> GetSectionFeatureForFeed(string feed)
        {
            return Task.Factory.StartNew(() =>
                                      {
                                          var f = new FeedAndPackages
                                                      {
                                                          Feed = feed,
                                                          Packages =
                                                              CoApp.GetPackages("*",
                                                                                collectionFilter:
                                                                                    p => p.HighestPackages(),
                                                                                locationFeed: feed)
                                                      };
                                          return CreateSectionFeature(f);

                                      });

        }

       

        
        class FeedAndPackages
        {
            public string Feed { get; set; }

            public Task<IEnumerable<Package>>  Packages { get; set; }
        }
        
     

        private SectionFeature CreateSectionFeature(FeedAndPackages f)
        {
          
                if (!f.Packages.IsFaulted && !f.Packages.IsCanceled && f.Packages.Result.Any())
                {
                    var packages = f.Packages.Result;
                    switch (packages.Count())
                    {
                        case 1:
                            return new SectionFeature { Name = f.Feed, TopLeft = ProductInfo.FromIPackage((IPackage)packages.First()) };
                           
                        case 2:
                            return new SectionFeature { Name = f.Feed, TopLeft = ProductInfo.FromIPackage((IPackage)packages.First()), 
                                BottomLeft = ProductInfo.FromIPackage((IPackage)packages.Skip(1).First()) };
                           
                        case 3:
                            return new SectionFeature
                                             {
                                                 Name = f.Feed,
                                                 TopLeft = ProductInfo.FromIPackage((IPackage)packages.First()),
                                                 BottomLeft = ProductInfo.FromIPackage((IPackage)packages.Skip(1).First()),
                                                 BottomCenter = ProductInfo.FromIPackage((IPackage)packages.Skip(2).First())
                                             };
                           
                        default:
                            return new SectionFeature
                                             {
                                                 Name = f.Feed,
                                                 TopLeft = ProductInfo.FromIPackage(packages.First()),
                                                 BottomLeft = ProductInfo.FromIPackage(packages.Skip(1).First()),
                                                 BottomCenter = ProductInfo.FromIPackage(packages.Skip(2).First()),
                                                 BottomRight = ProductInfo.FromIPackage(packages.Skip(3).First())
                                             };
                            

                    }
                }
            return null;

        }



       
    }

   
}
