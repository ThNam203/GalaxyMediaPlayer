using GalaxyMediaPlayer.Pages.NavContentPages.MusicPage;
using GalaxyMediaPlayer.Windows;
using System.Windows.Controls;

namespace GalaxyMediaPlayer.Pages.NavContentPages.MusicPages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public static Frame contentFrame;
        private MainContentPage _contentPage = new MainContentPage();

        public MainPage()
        {
            InitializeComponent();

            contentFrame = this.ContentFrame;
            ContentFrame.Navigate(_contentPage);
        }

        private void openSettingBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            bool? value = new SettingWindow().ShowDialog();
            if (value == true)
            {
                _contentPage.ResetPageData();
            }
        }
    }
}
