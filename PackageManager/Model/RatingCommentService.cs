using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoApp.PackageManager.Model.Interfaces;

namespace CoApp.PackageManager.Model
{
    public class RatingCommentService : IRatingCommentService
    {
        #region IRatingCommentService Members

        public Task<double> GetProductRating(string product)
        {
            throw new NotImplementedException();
        }

        public Task<double> GetPackageRating(string package)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ItemComment>> GetCommentsForProduct(string product)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ItemComment>> GetCommentsForProduct(string product, string earliestVersion,
                                                                    string latestVersion)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ItemComment>> GetCommentsForPackage(string package)
        {
            throw new NotImplementedException();
        }

        public Task SetPackageRating(double rating)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}