using System;
using System.Collections.Generic;
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

namespace CoApp.Gui.Toolkit
{
	/// <summary>
	/// Interaction logic for LoadingGrid.xaml
	/// </summary>
	
    
	public partial class LoadingGrid
	{
		public LoadingGrid()
		{
			this.InitializeComponent();
		}

	    public static readonly DependencyProperty IsLoadingProperty =
	        DependencyProperty.Register("IsLoading", typeof (bool), typeof (LoadingGrid), new PropertyMetadata(default(bool), IsLoadingChanged));

	    private static void IsLoadingChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
	    {
	        var lg = dependencyObject as LoadingGrid;
            if (lg != null)
            {
                VisualStateManager.GoToState(lg, lg.IsLoading ? "Loading" : "Normal", true);
            }
	    }

	    public bool IsLoading
	    {
	        get { return (bool) GetValue(IsLoadingProperty); }
	        set { SetValue(IsLoadingProperty, value); }
	    }
	}
}