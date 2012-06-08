using System.Threading.Tasks;

namespace CoApp.Gui.Toolkit.Model.Interfaces
{
    public interface IUpdateSettingsService
    {

        Task SetTask(int hour, UpdateDayOfWeek day, bool autoTrim, UpdateChoice choice);

        Task SetTaskToDefault();

        Task<UpdateTaskDTO> GetTask();

        Task<bool> IsTaskSet();

    }

    public class UpdateTaskDTO
    {
        public int Hour { get; set; }
        public UpdateDayOfWeek DayOfWeek { get; set; }
        public bool AutoTrim { get; set; }
        public UpdateChoice UpdateChoice { get; set; }
    }
}