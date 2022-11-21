using GalaxyMediaPlayer.Helpers;
using GalaxyMediaPlayer.Models;
using GalaxyMediaPlayer.UserControls.PlaylistControls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<SongPlaylistModel> playlists = new ObservableCollection<SongPlaylistModel>();
        private ObservableCollection<SongInfor> currentChosenPlaylistSongs = new ObservableCollection<SongInfor>();
        public PlaylistPage()
        {
            InitializeComponent();

            playlistListBox.ItemsSource = playlists;
            playlistSongsDataGrid.ItemsSource = currentChosenPlaylistSongs;
        }

        private void listBoxItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SongPlaylistModel? playlist;
            playlist = playlistListBox.SelectedItem as SongPlaylistModel;

            if (playlist != null && e.ClickCount == 2)
            {
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

            if (chosenSong != null)
            {
                MyMediaPlayer.SetNewPlaylist(new List<string> { chosenSong.Path });
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

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            playlistNameHeader.Text = "Playlist";
            BackBtn.Visibility = Visibility.Collapsed;
            playlistListBox.Visibility = Visibility.Visible;
            cbSortPlaylistBy.Visibility = Visibility.Visible;
            playlistSongsDataGrid.Visibility = Visibility.Collapsed;
            addNewSongToPlaylistBtn.Visibility = Visibility.Collapsed;
            newPlaylistBtn.Visibility = Visibility.Visible;
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
                            Name: songName,
                            Album: music.Tag.Album,
                            Artist: music.Tag.JoinedAlbumArtists,
                            Performer: music.Tag.JoinedPerformers,
                            Length: length,
                            Path: musicPath);

                        playlist.Songs.Add(newSongInfor);
                        currentChosenPlaylistSongs.Add(newSongInfor);
                    }
                }
            }
        }
    }
}
