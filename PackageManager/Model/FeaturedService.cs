using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.PackageManager.Model.Interfaces;
using CoApp.Packaging.Client;
using CoApp.Packaging.Common;
using CoApp.Toolkit.Linq;
using CollectionFilter =
    CoApp.Toolkit.Collections.XList
        <
            System.Linq.Expressions.Expression
                <
                    System.Func
                        <System.Collections.Generic.IEnumerable<CoApp.Packaging.Common.IPackage>,
                            System.Collections.Generic.IEnumerable<CoApp.Packaging.Common.IPackage>>>>;

namespace CoApp.PackageManager.Model
{
    public class FeaturedService : IFeaturedService
    {
        internal ICoAppService CoApp;

        public FeaturedService()
        {
            CoApp = new LocalServiceLocator().CoAppService;
        }

        #region IFeaturedService Members

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
                                                 CollectionFilter collectionFilter = null;
                                                 collectionFilter = collectionFilter.Then(p => p.HighestPackages());

                                                 var f = new FeedAndPackages
                                                             {
                                                                 Feed = feed,
                                                                 Packages =
                                                                     CoApp.GetPackages("*",
                                                                                       collectionFilter:
                                                                                           collectionFilter,
                                                                                       locationFeed: feed)
                                                             };
                                                 return CreateSectionFeature(f);
                                             });
        }

        #endregion

        private SectionFeature CreateSectionFeature(FeedAndPackages f)
        {
            if (!f.Packages.IsFaulted && !f.Packages.IsCanceled && f.Packages.Result.Any())
            {
                IEnumerable<Package> packages = f.Packages.Result;
                switch (packages.Count())
                {
                    case 1:
                        return new SectionFeature {Name = f.Feed, TopLeft = ProductInfo.FromIPackage(packages.First())};

                    case 2:
                        return new SectionFeature
                                   {
                                       Name = f.Feed,
                                       TopLeft = ProductInfo.FromIPackage(packages.First()),
                                       BottomLeft = ProductInfo.FromIPackage(packages.Skip(1).First())
                                   };

                    case 3:
                        return new SectionFeature
                                   {
                                       Name = f.Feed,
                                       TopLeft = ProductInfo.FromIPackage(packages.First()),
                                       BottomLeft = ProductInfo.FromIPackage(packages.Skip(1).First()),
                                       BottomCenter = ProductInfo.FromIPackage(packages.Skip(2).First())
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

        #region Nested type: FeedAndPackages

        private class FeedAndPackages
        {
            public string Feed { get; set; }

            public Task<IEnumerable<Package>> Packages { get; set; }
        }

        #endregion
    }
}