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

namespace GalaxyMediaPlayer.Pages
{
    /// <summary>
    /// Interaction logic for OpenImagePage.xaml
    /// </summary>
    public partial class OpenImagePage : Page
    {
        public OpenImagePage(string img)
        {
            InitializeComponent();
            this.TitleOfWindow.Text = System.IO.Path.GetFileName(img);
            imgPath = img;
            ZoomScale = 1;
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


                    UpdatePercentageZooming();
                }
            }
        }

        Point PointWheel = new Point(); // position to zoom image by wheeling mouse
        private Point PointStart;   // Start Position of the mouse on image

        public double ZoomScale;
        public string PercentageZoomingString;
        public bool IsDoubleClick = false;
        void UpdatePercentageZooming()
        {
            PercentageZoomingString = ((int)(ZoomScale * 100)).ToString() + "%";
            if (ZoomScale <= 0.5)
            {
                PercentageZoomingString = "50%";
            }
            if (ZoomScale >= 50)
            {
                PercentageZoomingString = "5000%";
            }
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

        private void btnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (ZoomScale * 1.1 <= 50)
            {
                Point center = new Point(OpenImg.ActualWidth / 2, OpenImg.ActualHeight / 2);

                Matrix m = OpenImg.RenderTransform.Value;
                m.ScaleAtPrepend(1.1, 1.1, center.X, center.Y);
                ZoomScale *= 1.1;
                UpdatePercentageZooming();
                tbPercentZooming.Text = PercentageZoomingString;
                OpenImg.RenderTransform = new MatrixTransform(m);
            }
        }
        private void btnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (ZoomScale / 1.1 >= 0.5)
            {
                Point center = new Point(OpenImg.ActualWidth / 2, OpenImg.ActualHeight / 2);

                Matrix m = OpenImg.RenderTransform.Value;
                m.ScaleAtPrepend(1 / 1.1, 1 / 1.1, center.X, center.Y);
                ZoomScale /= 1.1;
                UpdatePercentageZooming();
                tbPercentZooming.Text = PercentageZoomingString;
                OpenImg.RenderTransform = new MatrixTransform(m);
            }
        }


        private void OpenImg_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            PointWheel = e.MouseDevice.GetPosition(OpenImg);

            Matrix m = OpenImg.RenderTransform.Value;
            if (e.Delta > 0 && ZoomScale * 1.1 <= 50)
            {
                m.ScaleAtPrepend(1.1, 1.1, PointWheel.X, PointWheel.Y);
                ZoomScale *= 1.1;
            }
            else if (ZoomScale / 1.1 >= 0.5)
            {
                m.ScaleAtPrepend(1 / 1.1, 1 / 1.1, PointWheel.X, PointWheel.Y);
                ZoomScale /= 1.1;
            }
            UpdatePercentageZooming();
            tbPercentZooming.Text = PercentageZoomingString;
            OpenImg.RenderTransform = new MatrixTransform(m);
        }



        private void OpenImg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            mainWindow.CanDrag = false;
            IsDoubleClick = false;
            PointStart = e.GetPosition(OpenImg);
            OpenImg.CaptureMouse();
        }

        private void OpenImg_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OpenImg.ReleaseMouseCapture();
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            mainWindow.CanDrag = true;
        }

        private void OpenImg_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && IsDoubleClick == false)
            {
                Point PointNow = e.GetPosition(OpenImg);

                Point origin = new Point();
                origin.X = OpenImg.RenderTransform.Value.OffsetX;
                origin.Y = OpenImg.RenderTransform.Value.OffsetY;

                Matrix m = OpenImg.RenderTransform.Value;
                m.OffsetX = origin.X + (PointNow.X - PointStart.X);
                m.OffsetY = origin.Y + (PointNow.Y - PointStart.Y);

                OpenImg.RenderTransform = new MatrixTransform(m);
            }   
        }

        private void btnLeftArrow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.NavigationService.GoBack();
        }
    }
}
