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

        void UpdatePercentageZooming()
        {
            PercentageZoomingString = ((int)(ZoomScale * 100)).ToString() + "%";
            if (ZoomScale <= 0.6)
            {
                PercentageZoomingString = "60%";
            }
            if (ZoomScale >= 50)
            {
                PercentageZoomingString = "5000%";
            }
        }

        private void btnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (ZoomScale < 50)
            {
                Point center = new Point(this.Width / 2, (this.Height - 40) / 2);

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
            if (ZoomScale > 0.6)
            {
                Point center = new Point(this.Width / 2, (this.Height - 40) / 2);

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
            if (e.Delta > 0 && ZoomScale < 50)
            {
                m.ScaleAtPrepend(1.1, 1.1, PointWheel.X, PointWheel.Y);
                ZoomScale *= 1.1;
            }
            else if (ZoomScale > 0.6)
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
            PointStart = e.GetPosition(OpenImg);
            OpenImg.CaptureMouse();
        }

        private void OpenImg_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OpenImg.ReleaseMouseCapture();
        }

        private void OpenImg_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
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
    }
}
