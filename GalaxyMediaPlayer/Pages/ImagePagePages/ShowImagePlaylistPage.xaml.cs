using GalaxyMediaPlayer.Models;
using GalaxyMediaPlayer.Pages.NavContentPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GalaxyMediaPlayer.Pages.ImagePagePages
{
    /// <summary>
    /// Interaction logic for ShowImagePlaylistPage.xaml
    /// </summary>
    public partial class ShowImagePlaylistPage : Window
    {

        public ShowImagePlaylistPage(ImageModel img, List<ImageModel> list,double width, double height,double left, double top)
        {
            InitializeComponent();
            _Images = list;
            currentImage = img;
            imgPath = img.path;
            RunPlaylistImage();

            this.Width = width;
            this.Height = height;
            this.Left = left;
            this.Top = top;

            this.WindowStartupLocation= WindowStartupLocation.Manual;
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

        private void btnMinimizeApp_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnMaximizeApp_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                MainWindow.Instance.WindowState= WindowState.Normal;
                this.WindowState = WindowState.Normal;
            }
            else
            {
                MainWindow.Instance.WindowState = WindowState.Maximized;
                this.WindowState = WindowState.Maximized;
            }
        }

        private void btnCloseApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
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
            OpenImg.Source = new BitmapImage(new Uri(_imgPath));

            currentImage = Images[TargetIndex];
        }

        void getSizeOfNormalWindow(
            ref double normalWidth,
            ref double normalHeight,
            ref double normalLeft,
            ref double normalTop)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.Visibility = Visibility.Hidden;
                this.WindowState = WindowState.Normal;

                normalWidth = this.ActualWidth;
                normalHeight = this.ActualHeight;
                normalLeft = this.Left;
                normalTop = this.Top;

                this.WindowState = WindowState.Maximized;
                this.Visibility = Visibility.Visible;
            }
            else
            {
                normalWidth = this.ActualWidth;
                normalHeight = this.ActualHeight;
                normalLeft = this.Left;
                normalTop = this.Top;
            }
        }

        private void btnLeftArrow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            double width = 0, height = 0, left = 400, top = 400;
            getSizeOfNormalWindow(ref width,ref height,ref left, ref top);
            MainWindow.Instance.Width = width;
            MainWindow.Instance.Height = height;
            Application.Current.MainWindow.Visibility = Visibility.Visible;
            MainWindow.Instance.Left= left;
            MainWindow.Instance.Top= top;
            this.Close();
        }

        private void PageShowImagePlaylist_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();

            if (dispatcherTimer.IsEnabled==false)
                dispatcherTimer.Start();
        }
    }
}
