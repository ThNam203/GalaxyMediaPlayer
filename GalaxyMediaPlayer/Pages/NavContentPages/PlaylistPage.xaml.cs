using GalaxyMediaPlayer.Databases.SongPlaylist;
using GalaxyMediaPlayer.Helpers;
using GalaxyMediaPlayer.Models;
using GalaxyMediaPlayer.UserControls.PlaylistControls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
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
        private ObservableCollection<SongPlaylistModel> playlists;
        private ObservableCollection<SongInfor> currentChosenPlaylistSongs = new ObservableCollection<SongInfor>();
        public PlaylistPage()
        {
            InitializeComponent();

            playlists = new ObservableCollection<SongPlaylistModel>(PlaylistDatabaseAccess.LoadPlaylists());

            playlistListBox.ItemsSource = playlists;
            playlistSongsDataGrid.ItemsSource = currentChosenPlaylistSongs;
        }

        private void listBoxItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SongPlaylistModel? playlist;
            playlist = playlistListBox.SelectedItem as SongPlaylistModel;

            if (playlist != null && e.ClickCount == 2)
            {
                MainPage.currentMusicBrowsingFolder = "Playlist/" + playlist.Name;

                currentChosenPlaylistSongs.Clear();
                foreach (SongInfor songInfor in PlaylistSongsDatabaseAccess.LoadSongsFromPlaylistId(playlist.Id))
                {
                    if (File.Exists(songInfor.Path))
                        currentChosenPlaylistSongs.Add(songInfor);
                    else PlaylistSongsDatabaseAccess.DeleteSong(songInfor);
                }

                MainPage.Instance.ChangeButtonsViewOnOpenFolder(forceShow: false);
                MainPage.Instance.ChangeAdditionControlVisibilityInInforGrid(false);

                playlistNameHeader.Text = playlist.Name;
                playlistListBox.Visibility = Visibility.Collapsed;
                cbSortPlaylistBy.Visibility = Visibility.Collapsed;
                playlistSongsDataGrid.Visibility = Visibility.Visible;
                BackBtn.Visibility = Visibility.Visible;
                addNewSongToPlaylistBtn.Visibility = Visibility.Visible;
                newPlaylistBtn.Visibility = Visibility.Collapsed;
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
                MyMediaPlayer.SetNewPlaylist(songs);
                MyMediaPlayer.SetPositionInPlaylist(playlistSongsDataGrid.SelectedIndex);
                MyMediaPlayer.PlayCurrentSong();
                e.Handled = true;
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

            PlaylistDatabaseAccess.SavePlaylist(playlist);

            MainWindow.ClearAllMessageBox();
        }

        private void RemovePlaylist()
        {
            SongPlaylistModel? playlist;
            playlist = playlistListBox.SelectedItem as SongPlaylistModel;

            if (playlist != null)
            {
                this.playlists.Remove(playlist);
                PlaylistDatabaseAccess.DeletePlaylist(playlist);
            }
        }

        private void RenamePlaylist(string newName)
        {
            SongPlaylistModel? playlist;
            playlist = playlistListBox.SelectedItem as SongPlaylistModel;

            if (playlist != null)
            {
                SongPlaylistModel renamedPlaylist = new SongPlaylistModel(playlist);
                PlaylistDatabaseAccess.RenamePlaylist(renamedPlaylist);
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

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            playlistNameHeader.Text = "Playlist";
            BackBtn.Visibility = Visibility.Collapsed;
            playlistListBox.Visibility = Visibility.Visible;
            cbSortPlaylistBy.Visibility = Visibility.Visible;
            playlistSongsDataGrid.Visibility = Visibility.Collapsed;
            addNewSongToPlaylistBtn.Visibility = Visibility.Collapsed;
            newPlaylistBtn.Visibility = Visibility.Visible;

            MainPage.currentMusicBrowsingFolder = "Playlist";
            MainPage.Instance.ChangeButtonsViewOnOpenFolder(forceShow: true);
            MainPage.Instance.ChangeAdditionControlVisibilityInInforGrid(false);
        }

        private void addNewSongToPlaylistBtn_Click(object sender, RoutedEventArgs e)
        {
            SongPlaylistModel? playlist;
            playlist = playlistListBox.SelectedItem as SongPlaylistModel;

            if (playlist != null)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                string filter = "SupportedFormat|";
                foreach (string extenstion in SupportedExtensions.MUSIC_EXTENSION)
                {
                    filter += "*." + extenstion + ";";
                }
                openFileDialog.Filter = filter;
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == true)
                {
                    foreach (string musicPath in openFileDialog.FileNames)
                    {
                        TagLib.File music = TagLib.File.Create(musicPath);

                        double secondsDuration = music.Properties.Duration.TotalSeconds;
                        string durationFormat = DurationFormatHelper.GetDurationFormatFromTotalSeconds(secondsDuration);
                        string length = TimeSpan.FromSeconds(secondsDuration).ToString(durationFormat);

                        // Nam: if we cant get the song title, we use its name shown on disk
                        string songName;
                        if (music.Tag.Title == "" || music.Tag.Title == null)
                        {
                            songName = Path.GetFileName(musicPath);
                        }
                        else songName = music.Tag.Title;

                        SongInfor newSongInfor = new SongInfor(
                            playlistId: playlist.Id,
                            name: songName,
                            album: music.Tag.Album,
                            artist: music.Tag.JoinedAlbumArtists,
                            performer: music.Tag.JoinedPerformers,
                            length: length,
                            path: musicPath);

                        PlaylistSongsDatabaseAccess.SaveSong(newSongInfor);
                        playlist.Songs.Add(newSongInfor);
                        currentChosenPlaylistSongs.Add(newSongInfor);
                    }
                }
            }
        }

        private void showMusicPlaylistsBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            showMusicPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.White;
            showVideosPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.Transparent;
            showImagesPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.Transparent;
        }

        private void showVideosPlaylistsBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {

            showMusicPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.Transparent;
            showVideosPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.White;
            showImagesPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.Transparent;
        }

        private void showImagesPlaylistsBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {

            showMusicPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.Transparent;
            showVideosPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.Transparent;
            showImagesPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.White;
        }

        private void cbSortPlaylistBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSortPlaylistBy.SelectedItem != null)
            {
                int sortIndex = cbSortPlaylistBy.SelectedIndex;
                if (sortIndex == 0)
                {
                    List<SongPlaylistModel> tempPlaylists = new List<SongPlaylistModel>(playlists);
                    tempPlaylists.Sort((x, y) => x.Name.CompareTo(y.Name));

                    playlists.Clear();
                    foreach (SongPlaylistModel song in tempPlaylists) playlists.Add(song);
                } 
                else if (sortIndex == 1)
                {
                    List<SongPlaylistModel> tempPlaylists = new List<SongPlaylistModel>(playlists);
                    tempPlaylists.Sort((x, y) => 
                        DateTime.Parse(y.TimeCreated, CultureInfo.InvariantCulture)
                        .CompareTo(DateTime.Parse(x.TimeCreated, CultureInfo.InvariantCulture)));

                    playlists.Clear();
                    foreach (SongPlaylistModel song in tempPlaylists) playlists.Add(song);
                }
            }
        }
    }
}
