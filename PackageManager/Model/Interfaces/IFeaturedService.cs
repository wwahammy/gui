using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoApp.PackageManager.Model.Interfaces
{
    public interface IFeaturedService
    {
        Task<IEnumerable<SectionFeature>> GetSections();
    }
}