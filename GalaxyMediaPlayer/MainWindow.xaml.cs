using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace GalaxyMediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance;
        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
           MainFrame.Navigate(new Uri("/Pages/VideoMediaPlayer.xaml", UriKind.Relative));
          //  MainFrame.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.Relative));
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }
    }
}