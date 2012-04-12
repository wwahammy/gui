using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CoApp.PackageManager.Controls
{
    
    public class TileGridControl : Control
    {
        static TileGridControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (TileGridControl),
                                                     new FrameworkPropertyMetadata(typeof (TileGridControl)));
        }

        #region Top Left

        public static readonly DependencyProperty TopLeftContentProperty =
            DependencyProperty.Register("TopLeftContent", typeof (object), typeof (TileGridControl),
                                        new PropertyMetadata(default(object)));

        public static readonly DependencyProperty TopLeftBorderThicknessProperty =
            DependencyProperty.Register("TopLeftBorderThickness", typeof (Thickness), typeof (TileGridControl),
                                        new PropertyMetadata(default(Thickness)));

        public static readonly DependencyProperty TopLeftBorderBrushProperty =
            DependencyProperty.Register("TopLeftBorderBrush", typeof (Brush), typeof (TileGridControl),
                                        new PropertyMetadata(default(Brush)));

        public object TopLeftContent
        {
            get { return GetValue(TopLeftContentProperty); }
            set { SetValue(TopLeftContentProperty, value); }
        }

        public Thickness TopLeftBorderThickness
        {
            get { return (Thickness) GetValue(TopLeftBorderThicknessProperty); }
            set { SetValue(TopLeftBorderThicknessProperty, value); }
        }

        public Brush TopLeftBorderBrush
        {
            get { return (Brush) GetValue(TopLeftBorderBrushProperty); }
            set { SetValue(TopLeftBorderBrushProperty, value); }
        }

        #endregion

        #region Bottom Left

        public static readonly DependencyProperty BottomLeftContentProperty =
            DependencyProperty.Register("BottomLeftContent", typeof (object), typeof (TileGridControl),
                                        new PropertyMetadata(default(object)));

        public static readonly DependencyProperty BottomLeftBorderThicknessProperty =
            DependencyProperty.Register("BottomLeftBorderThickness", typeof (Thickness), typeof (TileGridControl),
                                        new PropertyMetadata(default(Thickness)));

        public static readonly DependencyProperty BottomLeftBorderBrushProperty =
            DependencyProperty.Register("BottomLeftBorderBrush", typeof (Brush), typeof (TileGridControl),
                                        new PropertyMetadata(default(Brush)));

        public object BottomLeftContent
        {
            get { return GetValue(BottomLeftContentProperty); }
            set { SetValue(BottomLeftContentProperty, value); }
        }

        public Thickness BottomLeftBorderThickness
        {
            get { return (Thickness) GetValue(BottomLeftBorderThicknessProperty); }
            set { SetValue(BottomLeftBorderThicknessProperty, value); }
        }

        public Brush BottomLeftBorderBrush
        {
            get { return (Brush) GetValue(BottomLeftBorderBrushProperty); }
            set { SetValue(BottomLeftBorderBrushProperty, value); }
        }

        #endregion

        #region Top Right

        public static readonly DependencyProperty TopRightContentProperty =
            DependencyProperty.Register("TopRightContent", typeof (object), typeof (TileGridControl),
                                        new PropertyMetadata(default(object)));

        public static readonly DependencyProperty TopRightBorderBrushProperty =
            DependencyProperty.Register("TopRightBorderBrush", typeof (Brush), typeof (TileGridControl),
                                        new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty TopRightBorderThicknessProperty =
            DependencyProperty.Register("TopRightBorderThickness", typeof (Thickness), typeof (TileGridControl),
                                        new PropertyMetadata(default(Thickness)));

        public object TopRightContent
        {
            get { return GetValue(TopRightContentProperty); }
            set { SetValue(TopRightContentProperty, value); }
        }

        public Brush TopRightBorderBrush
        {
            get { return (Brush) GetValue(TopRightBorderBrushProperty); }
            set { SetValue(TopRightBorderBrushProperty, value); }
        }

        public Thickness TopRightBorderThickness
        {
            get { return (Thickness) GetValue(TopRightBorderThicknessProperty); }
            set { SetValue(TopRightBorderThicknessProperty, value); }
        }

        #endregion

        #region Bottom Right

        public static readonly DependencyProperty BottomRightContentProperty =
            DependencyProperty.Register("BottomRightContent", typeof (object), typeof (TileGridControl),
                                        new PropertyMetadata(default(object)));

        public static readonly DependencyProperty BottomRightBorderThicknessProperty =
            DependencyProperty.Register("BottomRightBorderThickness", typeof (Thickness), typeof (TileGridControl),
                                        new PropertyMetadata(default(Thickness)));

        public static readonly DependencyProperty BottomRightBorderBrushProperty =
            DependencyProperty.Register("BottomRightBorderBrush", typeof (Brush), typeof (TileGridControl),
                                        new PropertyMetadata(default(Brush)));

        public object BottomRightContent
        {
            get { return GetValue(BottomRightContentProperty); }
            set { SetValue(BottomRightContentProperty, value); }
        }

        public Thickness BottomRightBorderThickness
        {
            get { return (Thickness) GetValue(BottomRightBorderThicknessProperty); }
            set { SetValue(BottomRightBorderThicknessProperty, value); }
        }

        public Brush BottomRightBorderBrush
        {
            get { return (Brush) GetValue(BottomRightBorderBrushProperty); }
            set { SetValue(BottomRightBorderBrushProperty, value); }
        }

        #endregion

        #region Bottom Middle

        public static readonly DependencyProperty BottomMiddleContentProperty =
            DependencyProperty.Register("BottomMiddleContent", typeof (object), typeof (TileGridControl),
                                        new PropertyMetadata(default(object)));

        public static readonly DependencyProperty BottomMiddleBorderThicknessProperty =
            DependencyProperty.Register("BottomMiddleBorderThickness", typeof (Thickness), typeof (TileGridControl),
                                        new PropertyMetadata(default(Thickness)));

        public static readonly DependencyProperty BottomMiddleBorderBrushProperty =
            DependencyProperty.Register("BottomMiddleBorderBrush", typeof (Brush), typeof (TileGridControl),
                                        new PropertyMetadata(default(Brush)));

        public object BottomMiddleContent
        {
            get { return GetValue(BottomMiddleContentProperty); }
            set { SetValue(BottomMiddleContentProperty, value); }
        }

        public Thickness BottomMiddleBorderThickness
        {
            get { return (Thickness) GetValue(BottomMiddleBorderThicknessProperty); }
            set { SetValue(BottomMiddleBorderThicknessProperty, value); }
        }

        public Brush BottomMiddleBorderBrush
        {
            get { return (Brush) GetValue(BottomMiddleBorderBrushProperty); }
            set { SetValue(BottomMiddleBorderBrushProperty, value); }
        }

        #endregion
    }
}