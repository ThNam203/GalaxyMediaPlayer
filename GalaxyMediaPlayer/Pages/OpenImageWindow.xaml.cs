﻿using System;
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
using System.Security.Policy;

namespace GalaxyMediaPlayer.Pages
{
    /// <summary>
    /// Interaction logic for OpenImageWindow.xaml
    /// </summary>
    public partial class OpenImageWindow : Window
    {
        public OpenImageWindow(string img)
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
            if (ZoomScale <= 1)
            {
                PercentageZoomingString = "100%";
            }
            if (ZoomScale >= 50)
            {
                PercentageZoomingString = "5000%";
            }
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
            if(ZoomScale < 50)
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
            if(ZoomScale > 1)
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
            else if(ZoomScale > 1)
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

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (OpenImg.IsMouseOver)
                return;
            PointWheel = new Point(this.Width / 2, (this.Height - 40) / 2);

            Matrix m = OpenImg.RenderTransform.Value;
            if (e.Delta > 0 && ZoomScale < 50)
            {
                m.ScaleAtPrepend(1.1, 1.1, PointWheel.X, PointWheel.Y);
                ZoomScale *= 1.1;
            }
            else if (ZoomScale > 1)
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
