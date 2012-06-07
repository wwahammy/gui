using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoApp.Gui.Toolkit.Model.Interfaces;

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

       
        Task<DateTime> LastScheduledTaskRealRun { get; }
        Task SetScheduledTaskToRunNow();

        Task<bool> IsSchedulerSet { get;  }

        Task SetDefaultScheduledTask();
    }
}
