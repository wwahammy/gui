using System.Collections.Generic;
using System.Threading.Tasks;
using CoApp.Packaging.Client;

namespace CoApp.PackageManager.Model.Interfaces
{
    public interface IFeaturedService
    {
        Task<SectionFeature> GetSectionFeatureForFeed(Feed feed);

        Task<IEnumerable<SectionFeature>> GetSections();

        IEnumerable<Feed> SortFeedsToFinalOrder(IEnumerable<Feed> feeds);
    }
}