using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CoApp.Updater.Controls;
using CoApp.Updater.Messages;
using CoApp.Updater.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace CoApp.Updater.Support.Errors
{
    public class ProductToUnblock : ObservableObject
    {

        public ProductToUnblock(ScreenViewModel parent)
        {
            parent.PropertyChanged += ParentOnPropertyChanged;

            /*UnblockCommand = new RelayCommand(() =>
                                                  {
                                                      WaitingOnUnblock = true;
                                                      _coappService.UnblockPackage(canonicalName).ContinueWith(
                                                          AfterUnblockAttempt);
                                                  });*/

            ShowErrorMessage = new RelayCommand(RunSendErrorMessage);
        }

        private void ParentOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "CanBlock" && sender is ScreenViewModel)
            {
                var vm = (ScreenViewModel)sender;
                CanBlock = vm.CanBlock;
            }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }


        private ICommand _unelevated;

        public ICommand Unelevated
        {
            get { return _unelevated; }
            set
            {
                _unelevated = value;
                RaisePropertyChanged("Unelevated");
            }
        }

        



        private ICommand _unblockCommand;

        public ICommand Unblock
        {
            get { return _unblockCommand; }
            set
            {
                _unblockCommand = value;
                RaisePropertyChanged("UnblockCommand");
            }
        }


        private ICommand _showErrorMessage;

        public ICommand ShowErrorMessage
        {
            get { return _showErrorMessage; }
            set
            {
                _showErrorMessage = value;
                RaisePropertyChanged("ShowErrorMessage");
            }
        }




        private bool? _error;

        public bool? Error
        {
            get { return _error; }
            set
            {
                _error = value;
                RaisePropertyChanged("Error");
            }
        }

        private bool _waitingOnUnblock;

        public bool WaitingOnUnblock
        {
            get { return _waitingOnUnblock; }
            set
            {
                _waitingOnUnblock = value;
                RaisePropertyChanged("WaitingOnUnblock");
            }
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                RaisePropertyChanged("ErrorMessage");
            }
        }

        private bool? _canBlock;

        public bool? CanBlock
        {
            get { return _canBlock; }
            set
            {
                _canBlock = value;
                RaisePropertyChanged("CanBlock");
            }
        }




        private void AfterUnblockAttempt(Task unblockTask)
        {
            if (unblockTask.IsFaulted)
            {
                Error = true;
                //TODO create better error
                ErrorMessage = unblockTask.Exception.ToString();
            }
            else
            {
                Error = false;
            }

            WaitingOnUnblock = false;
        }



        private void RunSendErrorMessage()
        {
            Messenger.Default.Send(new MetroDialogBoxMessage
            {
                Buttons = new ObservableCollection<ButtonDescription>{ new ButtonDescription { IsCancel = true, Title = "Close" } },
                Content = ErrorMessage,
                Title = "Unblock Failed"
            });
        }
    }
}
