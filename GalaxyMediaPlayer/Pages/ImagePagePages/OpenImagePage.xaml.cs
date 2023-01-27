using GalaxyMediaPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
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

        public double ZoomScale;
        public string PercentageZoomingString;
        public bool IsDoubleClick = false;

        void UpdatePercentageZooming()
        {
            PercentageZoomingString = ((int)(ZoomScale * 100)).ToString() + "%";
            if (ZoomScale <= 0.1)
            {
                PercentageZoomingString = "10%";
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
            if (ZoomScale / 1.1 >= 0.1)
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
            else if (ZoomScale / 1.1 >= 0.1)
            {
                m.ScaleAtPrepend(1 / 1.1, 1 / 1.1, PointWheel.X, PointWheel.Y);
                ZoomScale /= 1.1;
            }
            UpdatePercentageZooming();
            tbPercentZooming.Text = PercentageZoomingString;
            OpenImg.RenderTransform = new MatrixTransform(m);
        }

        private void btnLeftArrow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.NavigationService.GoBack();
        }

        private void btnRotateRight_Click(object sender, RoutedEventArgs e)
        {
            Point center = new Point(OpenImg.ActualWidth / 2, OpenImg.ActualHeight / 2);
            Matrix m = OpenImg.RenderTransform.Value;
            m.RotateAt(90, center.X, center.Y);
            OpenImg.RenderTransform = new MatrixTransform(m);
        }

        private void btnRotateLeft_Click(object sender, RoutedEventArgs e)
        {
            Point center = new Point(OpenImg.ActualWidth / 2, OpenImg.ActualHeight / 2);
            Matrix m = OpenImg.RenderTransform.Value;
            m.RotateAt(-90, center.X, center.Y);
            OpenImg.RenderTransform = new MatrixTransform(m);
        }

        private Point? mousePosition;

        private void CanvasImg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (CanvasImg.CaptureMouse())
            {
                MainWindow.Instance.CanDrag = false;
                mousePosition = e.GetPosition(CanvasImg); // position in Canvas
            }
        }

        private void CanvasImg_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CanvasImg.ReleaseMouseCapture();
            mousePosition = null;
            MainWindow.Instance.CanDrag = true;
        }

        private void CanvasImg_MouseMove(object sender, MouseEventArgs e)
        {
            if (mousePosition.HasValue)
            {
                var position = e.GetPosition(CanvasImg); // position in Canvas
                var translation = position - mousePosition.Value;
                mousePosition = position;

                var transform = (MatrixTransform)OpenImg.RenderTransform;
                var matrix = transform.Matrix;
                matrix.Translate(translation.X, translation.Y);
                OpenImg.RenderTransform = new MatrixTransform(matrix);
            }
        }

        private void PageOpenImage_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            PointWheel = new Point(OpenImg.ActualWidth / 2, OpenImg.ActualHeight / 2);

            Matrix m = OpenImg.RenderTransform.Value;
            if (e.Delta > 0 && ZoomScale * 1.1 <= 50)
            {
                m.ScaleAtPrepend(1.1, 1.1, PointWheel.X, PointWheel.Y);
                ZoomScale *= 1.1;
            }
            else if (ZoomScale / 1.1 >= 0.1)
            {
                m.ScaleAtPrepend(1 / 1.1, 1 / 1.1, PointWheel.X, PointWheel.Y);
                ZoomScale /= 1.1;
            }
            UpdatePercentageZooming();
            tbPercentZooming.Text = PercentageZoomingString;
            OpenImg.RenderTransform = new MatrixTransform(m);
        }
    }
}
