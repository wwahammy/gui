using System;
using CoApp.Gui.Toolkit.Model.Interfaces;

namespace CoApp.Gui.Toolkit.Support.Converters
{
    public static class UpdateDayOfWeekConverter
    {
        public static DayOfWeek? Convert(UpdateDayOfWeek updateDayOfWeek)
        {
            switch(updateDayOfWeek)
            {
                case UpdateDayOfWeek.Monday: return DayOfWeek.Monday;
                case UpdateDayOfWeek.Tuesday: return DayOfWeek.Tuesday;
                case UpdateDayOfWeek.Wednesday: return DayOfWeek.Wednesday;
                case UpdateDayOfWeek.Thursday: return DayOfWeek.Thursday;
                case UpdateDayOfWeek.Friday: return DayOfWeek.Friday;
                case UpdateDayOfWeek.Saturday: return DayOfWeek.Saturday;
                case UpdateDayOfWeek.Sunday: return DayOfWeek.Sunday;
                
            }
            
            return null;
        }


        public static UpdateDayOfWeek ConvertBack(DayOfWeek? dayOfWeek)
        {
            if (dayOfWeek == null)
            {
                return UpdateDayOfWeek.Everyday;
            }
            else
            {


                switch ((DayOfWeek) dayOfWeek)
                {
                    case DayOfWeek.Monday:
                        return UpdateDayOfWeek.Monday;
                    case DayOfWeek.Tuesday:
                        return UpdateDayOfWeek.Tuesday;
                    case DayOfWeek.Wednesday:
                        return UpdateDayOfWeek.Wednesday;
                    case DayOfWeek.Thursday:
                        return UpdateDayOfWeek.Thursday;
                    case DayOfWeek.Friday:
                        return UpdateDayOfWeek.Friday;
                    case DayOfWeek.Saturday:
                        return UpdateDayOfWeek.Saturday;
                    case DayOfWeek.Sunday:
                        return UpdateDayOfWeek.Sunday;
                }

                return UpdateDayOfWeek.Everyday;
            }
        }
    }


}
