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
            imgPath = img;
        }

        public string _imgPath;
        public string imgPath
        {
            get { return _imgPath; }
            set
            {
                _imgPath = value;
                if(_imgPath != null)
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
    }
}
