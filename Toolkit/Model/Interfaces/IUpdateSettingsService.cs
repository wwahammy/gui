using System.Threading.Tasks;

namespace CoApp.Gui.Toolkit.Model.Interfaces
{
    public interface IUpdateSettingsService
    {
        Task<UpdateChoice> UpdateChoice { get; }

        Task SetUpdateChoice(UpdateChoice choice);

        Task<UpdateTimeAndDay> UpdateTimeAndDay { get; }

        Task SetUpdateTimeAndDay(int hour, UpdateDayOfWeek day);

        Task<bool> AutoTrim { get; }
        Task SetAutoTrim(bool autotrim);
 
    }
}