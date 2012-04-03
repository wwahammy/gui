using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CoApp.Gui.Toolkit.Controls
{
    public class DialogBox : Control
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof (string), typeof (DialogBox),
                                        new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof (object), typeof (DialogBox),
                                        new PropertyMetadata(default(object)));


        public static readonly DependencyProperty ButtonDescriptionsProperty =
            DependencyProperty.Register("ButtonDescriptions", typeof (ObservableCollection<ButtonDescription>),
                                        typeof (DialogBox),
                                        new PropertyMetadata(new ObservableCollection<ButtonDescription>()));

        public string Title
        {
            get { return (string) GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public ObservableCollection<ButtonDescription> ButtonDescriptions
        {
            get { return (ObservableCollection<ButtonDescription>) GetValue(ButtonDescriptionsProperty); }
            set { SetValue(ButtonDescriptionsProperty, value); }
        }
    }

    public class ButtonDescription
    {
        public string Title { get; set; }
        public bool IsCancel { get; set; }
        public ICommand Command { get; set; }
        public object CommandParameter { get; set; }
        public IInputElement CommandTarget { get; set; }
    }

    /// <summary>
    /// Created to make data templating slightly easier
    /// </summary>
    public class ElevateButtonDescription : ButtonDescription
    {
    }
}