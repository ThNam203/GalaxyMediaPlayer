using GalaxyMediaPlayer.Databases.SongPlaylist;
using GalaxyMediaPlayer.Models;
using GalaxyMediaPlayer.UserControls.PlaylistControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GalaxyMediaPlayer.Pages.NavContentPages;

namespace GalaxyMediaPlayer.Pages.PlaylistPagePages
{
    /// <summary>
    /// Interaction logic for MusicPlaylistPage.xaml
    /// </summary>
    public partial class MusicPlaylistPage : Page
    {
        public static ObservableCollection<SongPlaylistModel> playlists;
        public static ObservableCollection<SongInfor> currentChosenPlaylistSongs = new ObservableCollection<SongInfor>();
        private List<int> deleteIndices = new List<int>();

        public static ListBox PlaylistListBox;
        public static DataGrid PlaylistSongsDataGrid;
        public static Border EmptyPlaylistBorder;

        public MusicPlaylistPage()
        {
            InitializeComponent();
            playlists = new ObservableCollection<SongPlaylistModel>(PlaylistDatabaseAccess.LoadPlaylists());

            PlaylistListBox = playlistListBox;
            PlaylistSongsDataGrid = playlistSongsDataGrid;
            EmptyPlaylistBorder = emptyPlaylistBorder;

            playlistListBox.ItemsSource = playlists;
            playlistSongsDataGrid.ItemsSource = currentChosenPlaylistSongs;

            if (playlists.Count <= 0)
            {
                emptyPlaylistBorder.Visibility = Visibility.Visible;
                PlaylistPage.NewPlaylistBtn.Visibility = Visibility.Collapsed;
            }
        }

        private void listBoxItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SongPlaylistModel? playlist;
            playlist = playlistListBox.SelectedItem as SongPlaylistModel;
            if (playlist != null && e.ClickCount >= 2)
            {
                MainPage.currentMusicBrowsingFolder += playlist.Name;

                currentChosenPlaylistSongs.Clear();
                foreach (SongInfor songInfor in PlaylistSongsDatabaseAccess.LoadSongsFromPlaylistId(playlist.Id))
                {
                    if (File.Exists(songInfor.Path))
                        currentChosenPlaylistSongs.Add(songInfor);
                    else PlaylistSongsDatabaseAccess.DeleteSong(songInfor);
                }

                MyMediaPlayer.SetTempPlaylist(currentChosenPlaylistSongs.Select(s => s.Path).ToList());
                MainPage.Instance.ChangeButtonsViewOnOpenFolder(forceDisable: false);
                MainPage.Instance.ChangeAdditionControlVisibilityInInforGrid(false);
                playlistSongsDataGrid.Visibility = Visibility.Visible;
                playlistListBox.Visibility = Visibility.Collapsed;

                PlaylistPage.ChooseCategoryPanel.Visibility = Visibility.Collapsed;
                PlaylistPage.PlaylistNameHeader.Text = playlist.Name;
                PlaylistPage.CbSortPlaylistBy.Visibility = Visibility.Collapsed;
                PlaylistPage.BackBtn.Visibility = Visibility.Visible;
                PlaylistPage.AddNewSongToPlaylistBtn.Visibility = Visibility.Visible;
                PlaylistPage.NewPlaylistBtn.Visibility = Visibility.Collapsed;
            }
        }

        private void listBoxItem_MouseRightButtonDown(
            object sender,
            MouseButtonEventArgs e)
        {
            SongPlaylistModel? playlist;
            playlist = playlistListBox.SelectedItem as SongPlaylistModel;

            if (playlist != null)
            {
                PlaylistRightClickDialog dialog = new PlaylistRightClickDialog(
                    onRenameButtonClick: RenamePlaylist,
                    onDeleteButtonClick: RemovePlaylist,
                    currentName: playlist.Name);

                int left = Convert.ToInt32(e.GetPosition(MainWindow.Instance as IInputElement).X);
                int top = Convert.ToInt32(e.GetPosition(MainWindow.Instance as IInputElement).Y);
                MainWindow.ShowCustomMessageBox(dialog, left: left, top: top);
                e.Handled = true;
            }
        }

        private void RemovePlaylist()
        {
            SongPlaylistModel? playlist;
            playlist = playlistListBox.SelectedItem as SongPlaylistModel;

            if (playlist != null)
            {
                playlists.Remove(playlist);
                PlaylistDatabaseAccess.DeletePlaylist(playlist);
            }

            if (playlists.Count <= 0)
            {
                emptyPlaylistBorder.Visibility = Visibility.Visible;
                PlaylistPage.NewPlaylistBtn.Visibility = Visibility.Collapsed;
            }
        }

        private void RenamePlaylist(string newName)
        {
            SongPlaylistModel? playlist;
            playlist = playlistListBox.SelectedItem as SongPlaylistModel;

            if (playlist != null)
            {
                SongPlaylistModel renamedPlaylist = new SongPlaylistModel(playlist);
                renamedPlaylist.Name = newName;

                PlaylistDatabaseAccess.RenamePlaylist(renamedPlaylist);

                for (int i = 0; i < playlists.Count; i++)
                {
                    if (playlist.Id == playlists[i].Id)
                    {
                        playlists.RemoveAt(i);
                        playlists.Insert(i, renamedPlaylist);
                        break;
                    }
                }
            }
        }

        // DataGridRow hold songs that is currently in the chosen playlist
        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SongInfor? chosenSong;
            chosenSong = playlistSongsDataGrid.SelectedItem as SongInfor;

            // Nam: indicates that a song is chosen (not outside)
            if (chosenSong != null)
            {
                List<string> songs = playlistSongsDataGrid.Items.Cast<SongInfor>().Select(s => s.Path).ToList();
                MyMediaPlayer.SetPlaylistFromTempPlaylist();
                MyMediaPlayer.SetPositionInPlaylist(playlistSongsDataGrid.SelectedIndex);
                MyMediaPlayer.PlayCurrentSong();
                e.Handled = true;
            }
        }

        private void DeleteCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (sender == null) return;

            CheckBox cb = (CheckBox)sender;
            if ((bool)cb.IsChecked) deleteIndices.Add(playlistSongsDataGrid.SelectedIndex);
            else deleteIndices.Remove(playlistSongsDataGrid.SelectedIndex);
            e.Handled = true;
        }

        private void deleteIconHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (deleteIndices.Count > 0)
            {
                ConfirmDialog dialog = new ConfirmDialog("Delete Songs", "Are you sure to delete " + deleteIndices.Count + " songs", deleteSongsInPlaylist);

                dialog.Show();
            }
        }

        private void deleteSongsInPlaylist()
        {
            List<SongInfor> deleteSongs = new List<SongInfor>();
            foreach (int idx in deleteIndices.OrderByDescending(v => v))
            {
                deleteSongs.Add(currentChosenPlaylistSongs[idx]);
                currentChosenPlaylistSongs.RemoveAt(idx);
            }

            deleteIndices.Clear();
            PlaylistSongsDatabaseAccess.DeleteSongs(deleteSongs);
        }

        private void secondaryNewPlaylistBtn_Click(object sender, RoutedEventArgs e)
        {
            PlaylistPage.NewPlaylistBtn_Click(sender, e);
        }
    }
}
