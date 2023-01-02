using System;
using System.Windows.Controls;
namespace GalaxyMediaPlayer.Pages.NavContentPages.MusicPages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public static Frame contentFrame;

        public MainPage()
        {
            InitializeComponent();

            contentFrame = this.ContentFrame;
            ContentFrame.Navigate(new Uri("Pages/NavContentPages/MusicPages/MainContentPage.xaml", UriKind.Relative));
        }
    }
}
