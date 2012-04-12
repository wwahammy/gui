using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CoApp.Gui.Toolkit.ViewModels;
using CoApp.PackageManager.Model;
using CoApp.PackageManager.Model.Interfaces;
using GalaSoft.MvvmLight;


namespace CoApp.PackageManager.ViewModel
{
    public class HomeViewModel : ScreenViewModel
    {
        internal IFeaturedService Featured;
        public HomeViewModel()
        {
            Loaded += OnLoaded;
            Featured = new LocalServiceLocator().FeaturedService;
        }

        private void OnLoaded()
        {
            Title = "Home";
            ScreenWidth = ScreenWidth.FullWidth;
            PanoramaItems.Clear();
            foreach (var a in Featured.GetSections().Result)
            {
                var panItem = new PanoramaItemSource
                                  {
                                      Name = a.Name,
                                      TopLeft = a.TopLeft,
                                      BottomCenter = a.BottomCenter,
                                      BottomLeft = a.BottomLeft,
                                      BottomRight = a.BottomRight
                                  };
                PanoramaItems.Add(panItem);
            }
        }

        private ObservableCollection<PanoramaItemSource> _panoramaItems = new ObservableCollection<PanoramaItemSource>();

        public ObservableCollection<PanoramaItemSource> PanoramaItems
        {
            get { return _panoramaItems; }
            set
            {
                _panoramaItems = value;
                RaisePropertyChanged("PanoramaItems");
            }
        }

        
    }

    public class PanoramaItemSource : ViewModelBase
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }


        public ICommand GoToSource { get; set; }

        private ProductInfo _topLeft;

        public ProductInfo TopLeft
        {
            get { return _topLeft; }
            set
            {
                _topLeft = value;
                RaisePropertyChanged("TopLeft");
            }
        }

   

        private ProductInfo _bottomLeft;

        public ProductInfo BottomLeft
        {
            get { return _bottomLeft; }
            set
            {
                _bottomLeft = value;
                RaisePropertyChanged("BottomLeft");
            }
        }

        private ProductInfo _bottomCenter;

        public ProductInfo BottomCenter
        {
            get { return _bottomCenter; }
            set
            {
                _bottomCenter = value;
                RaisePropertyChanged("BottomCenter");
            }
        }


        private ProductInfo _bottomRight;

        public ProductInfo BottomRight
        {
            get { return _bottomRight; }
            set
            {
                _bottomRight = value;
                RaisePropertyChanged("BottomRight");
            }
        }

        

        
    }

    
}
