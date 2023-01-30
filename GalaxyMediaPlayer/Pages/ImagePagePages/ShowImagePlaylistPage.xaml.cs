using GalaxyMediaPlayer.Models;
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
    public partial class ShowImagePlaylistPage : Page
    {

        public ShowImagePlaylistPage(ImageModel img, List<ImageModel> list)
        {
            InitializeComponent();
            _Images = list;
            currentImage = img;
            imgPath = img.path;
            RunPlaylistImage();
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



        private void btnMinimizeApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void btnMaximizeApp_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
        }

        private void btnCloseApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnLeftArrow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.Instance.MainFrame.NavigationService.GoBack();

            Pages.NavContentPages.PlaylistPage.pageFrame.Navigate(new Uri("/Pages/PlaylistPagePages/ImagePlaylistPage.xaml", UriKind.Relative));
            //Pages.NavContentPages.PlaylistPage.showImagesPlaylistsBtn_MouseDown(sender, e);
        }
    }
}
