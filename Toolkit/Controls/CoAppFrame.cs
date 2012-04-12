using System;
using System.Windows;
using System.Windows.Controls;

namespace CoApp.Gui.Toolkit.Controls
{
    public class CoAppFrame : ContentControl
    {
        public static readonly DependencyProperty CanGoBackProperty =
            DependencyProperty.Register("CanGoBack", typeof (bool), typeof (CoAppFrame), new PropertyMetadata(default(bool)));

        public bool CanGoBack
        {
            get { return (bool) GetValue(CanGoBackProperty); }
            set { SetValue(CanGoBackProperty, value); }
        }

        public static readonly DependencyProperty CurrentSourceBindProperty =
            DependencyProperty.Register("CurrentSourceBind", typeof (Uri), typeof (CoAppFrame), new PropertyMetadata());

        public Uri CurrentSourceBind
        {
            get { return (Uri)GetValue(CurrentSourceBindProperty); }
            set { SetValue(CurrentSourceBindProperty, value); }
        }

        /*
        public static readonly DependencyProperty SoundsProperty =
            DependencyProperty.Register("Sounds", typeof (bool?), typeof (CoAppFrame), new PropertyMetadata(SoundsChangedCallback));

        private static void SoundsChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            InternetSetFeatureEnabled(InternetFeaturelist.DISABLE_NAVIGATION_SOUNDS, SetFeatureOn.THREAD,true);
        
        }

        public bool Sounds
        {
            get { return (bool) GetValue(SoundsProperty); }
            set { SetValue(SoundsProperty, value); }
        }
         * */
        public CoAppFrame()
        {
            
            //Navigated += (o, n) => this.SetValue(CurrentSourceBindProperty, CurrentSource);
            /*Navigating += (o, n) => this.SetValue(CurrentSourceBindProperty, CurrentSource);
            NavigationFailed += (o, n) => this.SetValue(CurrentSourceBindProperty, CurrentSource);
            NavigationProgress += (o, n) => this.SetValue(CurrentSourceBindProperty, CurrentSource);*/
        }
        

       
    }
}
