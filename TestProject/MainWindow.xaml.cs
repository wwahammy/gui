using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Command;

namespace TestProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WhoCares = new RelayCommand(() => MessageBox.Show("Yay!!"));
        }

        public static readonly DependencyProperty WhoCaresProperty =
            DependencyProperty.Register("WhoCares", typeof (ICommand), typeof (MainWindow), new PropertyMetadata(default(ICommand)));

        public ICommand WhoCares
        {
            get { return (ICommand) GetValue(WhoCaresProperty); }
            set { SetValue(WhoCaresProperty, value); }
        }
    }
}
