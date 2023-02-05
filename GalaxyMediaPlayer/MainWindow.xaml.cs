using GalaxyMediaPlayer.Pages;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GalaxyMediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance;
        public bool CanDrag = true;
        public static double WidthNormalSize = 1120;
        public static double HeightNormalSize = 630;

        //Dat: To know using listview or datagrid in ImagePage and ImagePlaylistPage
        public static bool IsImagePageUsingGridStyle;
        public static bool IsImagePlaylistPageUsingGridStyle;

        public static void ShowCustomMessageBoxInMiddle(UIElement messageBox)
        {
            Instance.MessageBoxGrid.Children.Add(messageBox);
            Instance.MessageBoxGrid.Visibility = Visibility.Visible;
        }
        
        public static void ShowCustomMessageBox(UIElement messageBox, int left, int top)
        {
            Canvas.SetLeft(messageBox, left);
            Canvas.SetTop(messageBox, top);
            Instance.MessageBoxCanvas.Children.Add(messageBox);
            Instance.MessageBoxCanvas.Visibility = Visibility.Visible;

        }
        
        public static void ClearAllMessageBox()
        {
            Instance.MessageBoxGrid.Children.Clear();
            Instance.MessageBoxCanvas.Children.Clear();
            Instance.MessageBoxCanvas.Visibility = Visibility.Collapsed;
            Instance.MessageBoxGrid.Visibility = Visibility.Collapsed;

        }
        public MainWindow()
        {
            Instance = this;
            InitializeComponent();

            // Nam: disable 'backspace' button can go back in frame's stack
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();

            IsImagePageUsingGridStyle = false;
            IsImagePlaylistPageUsingGridStyle = false;

            MainFrame.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.Relative));
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

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (CanDrag == false) return;
                DragMove();
            }
        }

        private void MessageBoxGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxGrid.Children.Clear();
            MessageBoxGrid.Visibility = Visibility.Collapsed;
        }

        private void MessageBoxCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxCanvas.Children.Clear();
            MessageBoxCanvas.Visibility = Visibility.Collapsed;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(this.WindowState != WindowState.Maximized)
            {
                WidthNormalSize = this.ActualWidth;
                HeightNormalSize = this.ActualHeight;
            }
        }
    }
}