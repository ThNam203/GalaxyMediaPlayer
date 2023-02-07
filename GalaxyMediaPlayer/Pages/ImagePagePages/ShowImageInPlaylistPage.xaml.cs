using GalaxyMediaPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace GalaxyMediaPlayer.Pages.ImagePagePages
{
    /// <summary>
    /// Interaction logic for ShowImageInPlaylistPage.xaml
    /// </summary>
    public partial class ShowImageInPlaylistPage : Page
    {
        public ShowImageInPlaylistPage(List<ImageModel> list)
        {
            InitializeComponent();
            _Images = list;
            currentImage = _Images[0];
            LoadElement(currentImage.path);
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            MainWindow.IsRuningImagePlaylist = true;
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

        void LoadElement(string path)
        {
            OpenImg.Source = new BitmapImage(new Uri(path));
        }

        private Cursor _cursor = Cursors.Hand;
        public Cursor cursor { get { return _cursor; } set { _cursor = value; CanvasImg.Cursor = _cursor; } }

        System.Windows.Threading.DispatcherTimer dispatcherTimer ;

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

            currentImage = Images[TargetIndex];
            LoadElement(currentImage.path);
        }

        private void btnLeftArrow_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
            MainWindow.Instance.MainFrame.NavigationService.GoBack();
        }

        private void btnPreviousImage_Click(object sender, RoutedEventArgs e)
        {
            int currentIndex = Images.IndexOf(currentImage);
            int TargetIndex;

            if (currentIndex == 0)
                TargetIndex = Images.Count - 1;
            else
                TargetIndex = currentIndex - 1;

            currentImage = Images[TargetIndex];
            LoadElement(currentImage.path);
        }

        private void btnPlayImagePlaylist_Click(object sender, RoutedEventArgs e)
        {
            if (dispatcherTimer.IsEnabled)
            {
                ImagePlayBtn.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/play_32.png"));
                dispatcherTimer.Stop();
            }
            else
            {
                ImagePlayBtn.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/pause_32.png"));
                dispatcherTimer.Start();
            }
        }

        private void btnNextImage_Click(object sender, RoutedEventArgs e)
        {
            int currentIndex = Images.IndexOf(currentImage);
            int TargetIndex;

            if (currentIndex == Images.Count - 1)
                TargetIndex = 0;
            else
                TargetIndex = currentIndex + 1;

            currentImage = Images[TargetIndex];
            LoadElement(currentImage.path);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            OpenImg.Width = MainWindow.Instance.Width;
            OpenImg.Height = MainWindow.Instance.Height - 80;
        }
    }
}
