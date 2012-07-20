using System.Collections.Generic;
using System.Collections.ObjectModel;
using CoApp.Gui.Toolkit.Controls;
using GalaSoft.MvvmLight.Command;

namespace CoApp.Gui.Toolkit.ViewModels.Modal
{
    public class BasicModalViewModel : ModalViewModel
    {
        private ObservableCollection<ButtonDescription> _buttons = new ObservableCollection<ButtonDescription>();
        private string _content;

        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                RaisePropertyChanged("Content");
            }
        }

        public ObservableCollection<ButtonDescription> Buttons
        {
            get { return _buttons; }
            set
            {
                _buttons = value;
                RaisePropertyChanged("Buttons");
            }
        }


        public void SetViaButtonDescriptions(IEnumerable<ButtonDescription> buttons)
        {
            Buttons.Clear();
            /*  if (buttons.Count == 2)
            {*/
            foreach (ButtonDescription b in buttons)
            {
                if (b is ElevateButtonDescription)
                {
                    ButtonDescription tempB = b;
                    var desc = new ElevateButtonDescription

                                   {
                                       Title = b.Title,
                                       Command = new RelayCommand<object>((o) =>
                                                                              {
                                                                                  FireModalClose();
                                                                                  if (tempB.Command != null)
                                                                                      tempB.Command.Execute(o);
                                                                              }),
                                       CommandParameter = b.CommandParameter
                                   };
                    Buttons.Add(desc);
                }
                else
                {
                    ButtonDescription tempB = b;
                    var desc = new ButtonDescription
                                   {
                                       Title = b.Title,
                                       Command = new RelayCommand<object>((o) =>
                                                                              {
                                                                                  FireModalClose();
                                                                                  if (tempB.Command != null)
                                                                                      tempB.Command.Execute(o);
                                                                              }),
                                       CommandParameter = b.CommandParameter
                                   };
                    Buttons.Add(desc);
                }
            }


            /* }
            else
            {
                Logger.Error("We're trying to create buttons via button descriptions with {0} descriptions",
                             buttons.Count);

                //TODO what to do here?
            }*/
        }
    }
}