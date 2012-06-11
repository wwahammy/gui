using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using CoApp.Gui.Toolkit.Messages;
using CoApp.Gui.Toolkit.Support;
using CoApp.Gui.Toolkit.ViewModels;
using GalaSoft.MvvmLight.Messaging;

namespace CoApp.Gui.Toolkit.Controls
{
    public class ModalManager
    {
        private MetroDialogBoxMessage _dialogBoxInfo;
        private readonly object _modelDialogBoxLock = new object();

        private readonly CoAppWindow _window;

        public ModalManager(CoAppWindow win)
        {
            _window = win;
            Messenger.Default.Register<MetroDialogBoxMessage>(this, HandleDialogBoxMessage);
        }

        private void HandleDialogBoxMessage(MetroDialogBoxMessage metroDialogBoxMessage)
        {
            lock (_modelDialogBoxLock)
            {
                if (_modalDialogQueue.Count == 0)
                {
                    DisplayDialogBoxMessage(metroDialogBoxMessage);
                }
                else
                {
                    _modalDialogQueue.Enqueue(metroDialogBoxMessage);
                }
            }
        }

        private void ModalViewModelOnModalClosed()
        {
            lock (_modelDialogBoxLock)
            {
               
                //Remove the handler
                if (PopUp != null && PopUpGrid != null && PopUp.DataContext is ModalViewModel)
                {
                    var timer = new Timer { AutoReset = false, Interval = 250 };

                    timer.Elapsed += TimerOnElapsed;
                    _window.Dispatcher.Invoke(new Action(() =>
                                                             {
                                                                 var vm = PopUp.DataContext as ModalViewModel;
                    
                                                                 vm.ModalClosed -= ModalViewModelOnModalClosed;
                                                                 vm.FireUnload();
                                                                 
                                                                 VisualStateManager.GoToState(PopUp, "Base", true);
                                                                 timer.Start();
                                                             }));
                    

                    

                   
                }

            }

        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            Task.Factory.StartNew(() =>
                                      {
                                          _window.Dispatcher.Invoke(
                                              new Action(() => PopUpGrid.Visibility = Visibility.Collapsed));
                if (_modalDialogQueue.Count > 0)
                {
                    DisplayDialogBoxMessage(_modalDialogQueue.Dequeue());
                }
            });
        }


        private void TimeOnTick(object sender)
        {
           
        }


        readonly Queue<MetroDialogBoxMessage> _modalDialogQueue = new Queue<MetroDialogBoxMessage>();

        private void DisplayDialogBoxMessage(MetroDialogBoxMessage metroDialogBoxMessage)
        {
            var modal = metroDialogBoxMessage.ModalViewModel;



            modal.ModalClosed += ModalViewModelOnModalClosed;
            modal.FireLoad();
            if (PopUp != null || PopUpGrid != null)
            {
                _window.Dispatcher.Invoke(new Action(() =>
                {
                    PopUp.DataContext = metroDialogBoxMessage.ModalViewModel;
                    PopUpGrid.Visibility = Visibility.Visible;
                    VisualStateManager.GoToState(PopUp, "Open", true);

                }));
            }

        }

        private PopUpControl PopUp
        {
            get
            {
                return _window.Dispatcher.Invoke(new Func<PopUpControl>(() =>
                                                                        _window.GetTemplateChild("PopupControl") as
                                                                        PopUpControl)) as PopUpControl;
            }
        }

        private Grid PopUpGrid
        {
            get
            {
                if (PopUp != null)
                {
                    return _window.Dispatcher.Invoke(new Func<Grid>(PopUp.FindVisualParent<Grid>)) as Grid;
                }
                return null;
            }
        }

       

        

    }
}
