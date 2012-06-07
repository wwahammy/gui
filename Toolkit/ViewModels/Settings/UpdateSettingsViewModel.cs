using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CoApp.Gui.Toolkit.Model;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Toolkit.Extensions;
using GalaSoft.MvvmLight.Command;

namespace CoApp.Gui.Toolkit.ViewModels.Settings
{
  
    public class UpdateSettingsViewModel : ScreenViewModel
    {
        internal IUpdateSettingsService UpdateService;
        private bool _autoTrim;
        private UpdateDayOfWeek _dayOfWeek;
        private UpdateChoice _updateChoice;
        internal ICoAppService CoApp;


        private int _updateTime;

        public UpdateSettingsViewModel()
        {
            Title = "Update";
            Loaded += OnLoaded;
            Save = new RelayCommand(ExecuteSave);
            var loc = new LocalServiceLocator();
            UpdateService = loc.UpdateSettingsService;
            CoApp = loc.CoAppService;
            ElevateSave = new RelayCommand(ExecuteElevateSave);
            

        }

      
        private void ExecuteElevateSave()
        {
            var task = CoApp.Elevate();
            task.Continue(() => Save.Execute(null));
            //TODO what happens when fails
        }
        

        
        public UpdateDayOfWeek DayOfWeek
        {
            get { return _dayOfWeek; }
            set
            {
                _dayOfWeek = value;
                RaisePropertyChanged("DayOfWeek");
            }
        }
        
        public int UpdateTime
        {
            get { return _updateTime; }
            set
            {
                _updateTime = value;
                RaisePropertyChanged("UpdateTime");
            }
        }
        
        public UpdateChoice UpdateChoice
        {
            get { return _updateChoice; }
            set
            {
                _updateChoice = value;
                RaisePropertyChanged("UpdateChoice");
            }
        }
        
        public bool AutoTrim
        {
            get { return _autoTrim; }
            set
            {
                _autoTrim = value;
                RaisePropertyChanged("AutoTrim");
            }
        }

        
        public ICommand Save { get; set; }

        
        public ICommand ElevateSave { get; set; }

        

        private void ExecuteSave()
        {
            var tasks = new []
                           {
                               UpdateService.SetUpdateChoice(UpdateChoice),
                               UpdateService.SetUpdateTimeAndDay(UpdateTime, DayOfWeek),
                               UpdateService.SetAutoTrim(AutoTrim),

                           };
            Task.Factory.ContinueWhenAll(tasks, t => UpdateOnUI(OnLoaded));

        }

        private void OnLoaded()
        {
            _policyService.CanChangeSettings.ContinueAlways(t => UpdateOnUI( () => CanChangeSettings = t.Result));

            UpdateService.UpdateChoice.ContinueWith(t => UpdateOnUI(() => UpdateChoice = t.Result));
            UpdateService.UpdateTimeAndDay.ContinueWith(t => UpdateOnUI(() =>
                                                                            {
                                                                                DayOfWeek = t.Result.DayOfWeek;
                                                                                UpdateTime = t.Result.Time;
                                                                            }));

 
            UpdateService.AutoTrim.ContinueWith(t => UpdateOnUI(() => AutoTrim = t.Result));
        }

        #region Choices for comboboxes

        private ObservableCollection<UpdateDayOfWeek> _allDaysOfWeek = new ObservableCollection<UpdateDayOfWeek>(
            Enum.GetNames(typeof (UpdateDayOfWeek))
                .Select(v => (UpdateDayOfWeek) Enum.Parse(typeof (UpdateDayOfWeek), v)));

        private ObservableCollection<UpdateChoice> _allUpdateChoices = new ObservableCollection<UpdateChoice>(
            Enum.GetNames(typeof (UpdateChoice))
                .Select(v => (UpdateChoice) Enum.Parse(typeof (UpdateChoice), v)));

        private ObservableCollection<int> _hourChoices = new ObservableCollection<int>(Enumerable.Range(0, 24));


        
        public ObservableCollection<int> HourChoices
        {
            get { return _hourChoices; }
            set
            {
                _hourChoices = value;
                RaisePropertyChanged("HourChoices");
            }
        }

        
        public ObservableCollection<UpdateChoice> AllUpdateChoices
        {
            get { return _allUpdateChoices; }
            set
            {
                _allUpdateChoices = value;
                RaisePropertyChanged("AllUpdateChoices");
            }
        }

        
        public ObservableCollection<UpdateDayOfWeek> AllDaysOfWeek
        {
            get { return _allDaysOfWeek; }
            set
            {
                _allDaysOfWeek = value;
                RaisePropertyChanged("AllDaysOfWeek");
            }
        }

        #endregion

    }
}