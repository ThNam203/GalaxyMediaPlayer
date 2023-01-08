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
        public static Button backBtn;
        private MainContentPage _contentPage = new MainContentPage();

        public MainPage()
        {
            InitializeComponent();

            backBtn = this.BackBtn;
            contentFrame = this.ContentFrame;
            ContentFrame.Navigate(_contentPage);
        }

        private void openSettingBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            bool? value = new SettingWindow(SettingWindow.SettingPage.Music).ShowDialog();
            if (value == true)
            {
                _contentPage.ResetPageData();
            }
        }

        private void BackBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ContentFrame.Navigate(_contentPage);
            Pages.MainPage.currentMusicBrowsingFolder = "MusicPage";
            backBtn.Visibility = System.Windows.Visibility.Collapsed;

            Pages.MainPage.Instance.ChangeButtonsViewOnOpenFolder(forceDisable: true);
            Pages.MainPage.Instance.ChangeAdditionControlVisibilityInInforGrid(forceShow: true);
        }
    }
}
