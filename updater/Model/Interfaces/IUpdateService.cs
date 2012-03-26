using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoApp.Updater.Model.Interfaces
{
    public interface IUpdateService
    {
        /// <summary>
        /// Checks for all possible updates
        /// </summary>
        /// <returns>A task representing the checking</returns>
        
        Task CheckForUpdates();

        Task CheckForUpdates(CancellationToken token);

        Task<DateTime?> LastTimeChecked { get; }
        
        Task<DateTime?> LastTimeInstalled { get; }
        
        //IEnumerable<string> Warnings { get; }
        int NumberOfProducts { get; }

        int NumberOfProductsSelected { get; }

        /// <summary>
        /// bool is whether the product is selected
        /// </summary>
        IDictionary<Product, bool> AllPossibleProducts
        {
            get; 
        }

        void SelectProduct(Product p);
        void UnselectProduct(Product p);

        Product CurrentInstallingProduct { get; }
        

        Task PerformInstallation();


        Task BlockProduct(Product product);

        Task<UpdateChoice> UpdateChoice();

        Task SetUpdateChoice(UpdateChoice choice);

        Task<UpdateTimeAndDay> UpdateTimeAndDay { get; }

        Task SetUpdateTimeAndDay(int hour, UpdateDayOfWeek day);
        
        Task<bool> AutoTrim();
        Task SetAutoTrim(bool autotrim);
    }

    public enum UpdateDayOfWeek
    {
        Everyday,
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday
    }

    public enum UpdateChoice
    {
        AutoInstallAll,
        AutoInstallJustUpdates,
        Notify,
        Dont
    }
}
