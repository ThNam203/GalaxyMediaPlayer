using GalaxyMediaPlayer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace GalaxyMediaPlayer.Pages.NavContentPages.MusicPages
{
    /// <summary>
    /// Interaction logic for ListBoxItemContentShowPage.xaml
    /// </summary>
    public partial class ListBoxItemContentShowPage : Page
    {
        // Nam: currentBrowsingName is stupid, keeping track of what the songs playing is located
        public ListBoxItemContentShowPage(List<SongInfor> songsToShow, string currentBrowsingName)
        {
            InitializeComponent();

            songsDataGrid.ItemsSource = songsToShow;
            Pages.MainPage.currentMusicBrowsingFolder += currentBrowsingName;

            MyMusicMediaPlayer.SetTempPlaylist(songsToShow.Select(x => x.Path).ToList());
            Pages.MainPage.Instance.ChangeButtonsViewOnOpenFolder(forceDisable: false);
            Pages.MainPage.Instance.ChangeAdditionControlVisibilityInInforGrid(forceShow: false);
        }


        // DataGridRow hold songs that is currently in the chosen playlist
        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SongInfor? chosenSong;
            chosenSong = songsDataGrid.SelectedItem as SongInfor;

            // Nam: indicates that a song is chosen (not outside)
            if (chosenSong != null)
            {
                MyMusicMediaPlayer.SetPlaylistFromTempPlaylist();
                MyMusicMediaPlayer.SetPositionInPlaylist(songsDataGrid.SelectedIndex);
                MyMusicMediaPlayer.PlayCurrentSong();
                e.Handled = true;
            }
        }
    }
}
