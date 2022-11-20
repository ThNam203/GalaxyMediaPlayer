using GalaxyMediaPlayer.Models;
using GalaxyMediaPlayer.UserControls.PlaylistControls;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GalaxyMediaPlayer.Pages.NavContentPages
{
    /// <summary>
    /// Interaction logic for PlaylistPage.xaml
    /// </summary>
    public partial class PlaylistPage : Page
    {
        private ObservableCollection<SongPlaylistModel> playlists = new ObservableCollection<SongPlaylistModel>();
        private ObservableCollection<SongInfor> playlistSongs = new ObservableCollection<SongInfor>();
        public PlaylistPage()
        {
            InitializeComponent();

            playlistListBox.ItemsSource = playlists;
            playlistSongsDataGrid.ItemsSource = playlistSongs;

            playlists.Add(new SongPlaylistModel("Test"));
            playlistListBox.MouseDoubleClick += PlaylistListBox_MouseDoubleClick;
            playlistListBox.PreviewMouseRightButtonDown += PlaylistListBox_PreviewMouseRightButtonDown;
        }

        private void PlaylistListBox_PreviewMouseRightButtonDown(
            object sender, 
            MouseButtonEventArgs e)
        {
            SongPlaylistModel? playlist;
            playlist = playlistListBox.SelectedItem as SongPlaylistModel;

            if (playlist != null)
            {
                PlaylistRightClickDialog dialog = new PlaylistRightClickDialog(onRenameButtonClick: RenamePlaylist, onDeleteButtonClick: RemovePlaylist);

                int left = Convert.ToInt32(e.GetPosition(MainWindow.Instance as IInputElement).X);
                int top = Convert.ToInt32(e.GetPosition(MainWindow.Instance as IInputElement).Y);
                MainWindow.ShowCustomMessageBox(dialog, left: left, top: top);
                e.Handled = true;
            }
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void PlaylistListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SongPlaylistModel? playlist;
            playlist = playlistListBox.SelectedItem as SongPlaylistModel;

            if (playlist != null)
            {
                playlistListBox.Visibility = Visibility.Collapsed;
                playlistSongsDataGrid.Visibility = Visibility.Visible;
            }
        }

        // Show rename, delete table
        private void newPlaylistBtn_Click(object sender, RoutedEventArgs e)
        {
            UserControls.NewPlaylistControl newPlaylistControl = new UserControls.NewPlaylistControl(AddNewPlaylist);

            MainWindow.ShowCustomMessageBoxInMiddle(newPlaylistControl);
            e.Handled = true;
        }

        private void AddNewPlaylist(string playlistName)
        {
            SongPlaylistModel playlist = new SongPlaylistModel(playlistName);
            this.playlists.Add(playlist);
            MainWindow.ClearAllMessageBox();
        }

        private void RemovePlaylist()
        {
            SongPlaylistModel? playlist;
            playlist = playlistListBox.SelectedItem as SongPlaylistModel;

            if (playlist != null)
            {
                this.playlists.Remove(playlist);
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

                for (int i = 0; i < this.playlists.Count; i++)
                {
                    if (playlist.Id == this.playlists[i].Id)
                    {
                        playlists.RemoveAt(i);
                        playlists.Insert(i, renamedPlaylist);
                        break;
                    }
                }
            }
        }
    }
}
