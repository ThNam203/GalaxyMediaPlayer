using GalaxyMediaPlayer.Models;
using GalaxyMediaPlayer.Pages.NavContentPages.MusicPage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace GalaxyMediaPlayer.Pages.NavContentPages.MusicPages
{
    /// <summary>
    /// Interaction logic for ListBoxItemContentShowPage.xaml
    /// </summary>
    public partial class ListBoxItemContentShowPage : Page
    {
        public ListBoxItemContentShowPage(List<SongInfor> songsToShow)
        {
            InitializeComponent();

            songsDataGrid.ItemsSource = songsToShow;
        }


        // DataGridRow hold songs that is currently in the chosen playlist
        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SongInfor? chosenSong;
            chosenSong = songsDataGrid.SelectedItem as SongInfor;

            // Nam: indicates that a song is chosen (not outside)
            if (chosenSong != null)
            {
                List<string> songs = songsDataGrid.Items.Cast<SongInfor>().Select(s => s.Path).ToList();
                MyMediaPlayer.SetNewPlaylist(songs);
                MyMediaPlayer.SetPositionInPlaylist(songsDataGrid.SelectedIndex);
                MyMediaPlayer.PlayCurrentSong();
                e.Handled = true;
            }
        }
    }
}
