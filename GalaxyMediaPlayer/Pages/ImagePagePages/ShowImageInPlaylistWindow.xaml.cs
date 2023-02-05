using GalaxyMediaPlayer.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GalaxyMediaPlayer.Pages.ImagePagePages
{
    /// <summary>
    /// Interaction logic for ShowImageInPlaylistWindow.xaml
    /// </summary>
    public partial class ShowImageInPlaylistWindow : Window
    {
        public ShowImageInPlaylistWindow(List<ImageModel> list, double width, double height, double left, double top)
        {
            InitializeComponent();
            _Images = list;
            imgPath = _Images[0].path;
            currentImage = _Images[0];
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            RunPlaylistImage();

            this.Width = width;
            this.Height = height;
            this.Left = left;
            this.Top = top;

            this.WindowStartupLocation = WindowStartupLocation.Manual;
        }
        private static ImageModel _currentImage;
        public static ImageModel currentImage
        {
            get { return _currentImage; }
            set { _currentImage = value; }
        }

        private static List<ImageModel> _Images;
        public static List<ImageModel> Images
        {
            get { return _Images; }
            set { _Images = value; }
        }

        public string _imgPath { get; set; }
        public string imgPath
        {
            get { return _imgPath; }
            set
            {
                _imgPath = value;
                if (_imgPath != null)
                {
                    OpenImg.Source = new BitmapImage(new Uri(_imgPath));
                }
            }
        }

        private Cursor _cursor = Cursors.Hand;
        public Cursor cursor { get { return _cursor; } set { _cursor = value; CanvasImg.Cursor = _cursor; } }

        System.Windows.Threading.DispatcherTimer dispatcherTimer;

        private void btnMinimizeApp_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnMaximizeApp_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                MainWindow.Instance.WindowState = WindowState.Normal;
                this.WindowState = WindowState.Normal;
                cursor = Cursors.Hand;
            }
            else
            {
                MainWindow.Instance.WindowState = WindowState.Maximized;
                this.WindowState = WindowState.Maximized;
                cursor = Cursors.Arrow;
            }
        }

        private void btnCloseApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        
        private void RunPlaylistImage()
        {
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
            dispatcherTimer.Start();
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            int currentIndex = Images.IndexOf(currentImage);
            int TargetIndex;
            if (currentIndex == Images.Count - 1)
            {
                TargetIndex = 0;
            }
            else
            {
                TargetIndex = currentIndex + 1;
            }
            imgPath = Images[TargetIndex].path;

            currentImage = Images[TargetIndex];
        }

        private void PageShowImagePlaylist_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
            MainWindow.Instance.Left = this.Left;
            MainWindow.Instance.Top = this.Top;
            if (dispatcherTimer.IsEnabled == false)
                dispatcherTimer.Start();
        }

        private void PageShowImagePlaylist_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.WindowState != WindowState.Maximized)
            {
                MainWindow.Instance.Width = this.Width;
                MainWindow.Instance.Height = this.Height;
            }
        }

        private void btnLeftArrow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Left = this.Left;
            MainWindow.Instance.Top = this.Top;

            Application.Current.MainWindow.Visibility = Visibility.Visible;
            dispatcherTimer.Stop();
            this.Close();
        }
    }
}
