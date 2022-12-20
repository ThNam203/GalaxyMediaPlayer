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
using Microsoft.Win32;
using System.IO;
using System.Collections.ObjectModel;
using GalaxyMediaPlayer.Models;
using System.Globalization;
using Microsoft.WindowsAPICodePack.Shell;
using System.Drawing.Imaging;
using System.Reflection;

namespace GalaxyMediaPlayer.Pages
{
    /// <summary>
    /// Interaction logic for OpenImageWindow.xaml
    /// </summary>
    public partial class OpenImageWindow : Window
    {

        private double WINDOW_WIDTH;
        private double WINDOW_HEIGHT;

        public double WindowWidth { get { return WINDOW_WIDTH; } set { WINDOW_WIDTH = value; } }
        public double WindowHeight { get { return WINDOW_HEIGHT; } set { WINDOW_HEIGHT = value; } }


        public OpenImageWindow(string img)
        {
            InitializeComponent();
            this.TitleOfWindow.Text = System.IO.Path.GetFileName(img);
            WindowWidth = 1000;
            WindowHeight = 800;


            imgPath = img;
            InitOpenImg();
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

        void InitOpenImg()
        {
            OpenImg.MouseWheel += OpenImg_MouseWheel;
            OpenImg.MouseLeftButtonDown += OpenImg_MouseLeftButtonDown;
            OpenImg.MouseLeftButtonUp += OpenImg_MouseLeftButtonUp;
            OpenImg.MouseMove += OpenImg_MouseMove;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && !OpenImg.IsMouseCaptured)
                this.DragMove();
        }

        private void btnMinimizeApp_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnMaximizeApp_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void btnCloseApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            Point center = new Point(this.Width / 2, (this.Height - 40) / 2);

            Matrix m = OpenImg.RenderTransform.Value;
            m.ScaleAtPrepend(1.1, 1.1, center.X, center.Y);

            OpenImg.RenderTransform = new MatrixTransform(m);
        }
        private void btnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            Point center = new Point(this.Width / 2, (this.Height - 40) / 2);

            Matrix m = OpenImg.RenderTransform.Value;
            m.ScaleAtPrepend(1 / 1.1, 1 / 1.1, center.X, center.Y);

            OpenImg.RenderTransform = new MatrixTransform(m);
        }

        Point PointWheel = new Point();
        private void OpenImg_MouseWheel(object sender, MouseWheelEventArgs e)
        {

            PointWheel = e.MouseDevice.GetPosition(OpenImg);

            Matrix m = OpenImg.RenderTransform.Value;
            if (e.Delta > 0)
                m.ScaleAtPrepend(1.1, 1.1, PointWheel.X, PointWheel.Y);
            else
                m.ScaleAtPrepend(1 / 1.1, 1 / 1.1, PointWheel.X, PointWheel.Y);

            OpenImg.RenderTransform = new MatrixTransform(m);
        }

        private Point PointStart;   // Start Position of the mouse on image

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
                Point PointNow = e.MouseDevice.GetPosition(OpenImg);

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
