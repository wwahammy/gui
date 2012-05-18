using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoApp.PackageManager.Model.Interfaces
{
    public interface IRatingCommentService
    {
        Task<double> GetProductRating(string product);
        Task<double> GetPackageRating(string package);
        Task<IEnumerable<ItemComment>> GetCommentsForProduct(string product);
        Task<IEnumerable<ItemComment>> GetCommentsForProduct(string product, string earliestVersion, string latestVersion);
        
        Task<IEnumerable<ItemComment>> GetCommentsForPackage(string package);

        Task SetPackageRating(double rating);

    }

    
}