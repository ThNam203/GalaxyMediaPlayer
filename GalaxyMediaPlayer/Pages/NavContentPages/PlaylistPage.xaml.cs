using GalaxyMediaPlayer.Databases.SongPlaylist;
using GalaxyMediaPlayer.Helpers;
using GalaxyMediaPlayer.Models;
using GalaxyMediaPlayer.UserControls;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GalaxyMediaPlayer.Pages.NavContentPages
{
    public partial class PlaylistPage : Page
    {
        public static Button NewPlaylistBtn;
        public static Button BackBtn;
        public static Button AddNewSongToPlaylistBtn;
        public static TextBlock PlaylistNameHeader;
        public static ComboBox CbSortPlaylistBy;
        public static StackPanel ChooseCategoryPanel;

        public static Action<object, RoutedEventArgs> NewPlaylistBtn_Click;

        public enum PlaylistPageType
        {
            Music,
            Image,
            Video,
        }

        PlaylistPageType currentPlaylistType = PlaylistPageType.Music;

        public PlaylistPage()
        {
            InitializeComponent();

            NewPlaylistBtn = newPlaylistBtn;
            PlaylistNameHeader = playlistNameHeader;
            AddNewSongToPlaylistBtn = addNewSongToPlaylistBtn;
            CbSortPlaylistBy = cbSortPlaylistBy;
            BackBtn = backBtn;
            ChooseCategoryPanel = chooseCategoryPanel;
            NewPlaylistBtn_Click = this.newPlaylistBtn_Click;

            PageFrame.Navigate(new Uri("/Pages/PlaylistPagePages/MusicPlaylistPage.xaml", UriKind.Relative));
        }

        private void showMusicPlaylistsBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            showMusicPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.White;
            showVideosPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.Transparent;
            showImagesPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.Transparent;

            currentPlaylistType = PlaylistPageType.Music;
            PageFrame.Navigate(new Uri("/Pages/PlaylistPagePages/MusicPlaylistPage.xaml", UriKind.Relative));
        }

        private void showVideosPlaylistsBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            showVideosPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.White;
            showMusicPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.Transparent;
            showImagesPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.Transparent;

            currentPlaylistType = PlaylistPageType.Video;
            PageFrame.Navigate(new Uri("/Pages/PlaylistPagePages/Page1.xaml", UriKind.Relative));
        }

        private void showImagesPlaylistsBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            showImagesPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.White;
            showMusicPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.Transparent;
            showVideosPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.Transparent;

            currentPlaylistType = PlaylistPageType.Image;
            PageFrame.Navigate(new Uri("/Pages/PlaylistPagePages/Page1.xaml", UriKind.Relative));
        }

        // Show rename, delete table
        private void newPlaylistBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentPlaylistType == PlaylistPageType.Music)
            {
                NewPlaylistControl newPlaylistControl = new NewPlaylistControl(AddNewMusicPlaylist);
                MainWindow.ShowCustomMessageBoxInMiddle(newPlaylistControl);
            }
            else if (currentPlaylistType == PlaylistPageType.Video)
            {

            } 
            else if (currentPlaylistType == PlaylistPageType.Image)
            {

            }

            e.Handled = true;
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            playlistNameHeader.Text = "Playlist";
            cbSortPlaylistBy.Visibility = Visibility.Visible;
            newPlaylistBtn.Visibility = Visibility.Visible;
            chooseCategoryPanel.Visibility = Visibility.Visible;
            backBtn.Visibility = Visibility.Collapsed;

            if (currentPlaylistType == PlaylistPageType.Music)
            {
                addNewSongToPlaylistBtn.Visibility = Visibility.Collapsed;

                PlaylistPagePages.MusicPlaylistPage.PlaylistListBox.Visibility = Visibility.Visible;
                PlaylistPagePages.MusicPlaylistPage.PlaylistSongsDataGrid.Visibility = Visibility.Collapsed;
            }
            else if (currentPlaylistType == PlaylistPageType.Video)
            {

            }
            else if (currentPlaylistType == PlaylistPageType.Image)
            {

            }

            MainPage.currentMusicBrowsingFolder = "PlaylistPage";
            MainPage.Instance.ChangeButtonsViewOnOpenFolder(forceDisable: true);
            MainPage.Instance.ChangeAdditionControlVisibilityInInforGrid(forceShow: true);
        }

        private void AddNewMusicPlaylist(string playlistName)
        {
            SongPlaylistModel playlist = new SongPlaylistModel(playlistName);
            PlaylistPagePages.MusicPlaylistPage.playlists.Add(playlist);

            PlaylistDatabaseAccess.SavePlaylist(playlist);

            MainWindow.ClearAllMessageBox();

            if (PlaylistPagePages.MusicPlaylistPage.playlists.Count > 0)
            {
                PlaylistPagePages.MusicPlaylistPage.EmptyPlaylistBorder.Visibility = Visibility.Collapsed;
                PlaylistPage.NewPlaylistBtn.Visibility = Visibility.Visible;
            }
        }

        private void cbSortPlaylistBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSortPlaylistBy.SelectedItem != null)
            {
                int sortIndex = cbSortPlaylistBy.SelectedIndex;
                if (sortIndex == 0)
                {
                    List<SongPlaylistModel> tempPlaylists = new List<SongPlaylistModel>(PlaylistPagePages.MusicPlaylistPage.playlists);
                    tempPlaylists.Sort((x, y) => x.Name.CompareTo(y.Name));

                    PlaylistPagePages.MusicPlaylistPage.playlists.Clear();
                    foreach (SongPlaylistModel song in tempPlaylists) PlaylistPagePages.MusicPlaylistPage.playlists.Add(song);
                } 
                else if (sortIndex == 1)
                {
                    List<SongPlaylistModel> tempPlaylists = new List<SongPlaylistModel>(PlaylistPagePages.MusicPlaylistPage.playlists);
                    tempPlaylists.Sort((x, y) => 
                        DateTime.Parse(y.TimeCreated, CultureInfo.InvariantCulture)
                        .CompareTo(DateTime.Parse(x.TimeCreated, CultureInfo.InvariantCulture)));

                    PlaylistPagePages.MusicPlaylistPage.playlists.Clear();
                    foreach (SongPlaylistModel song in tempPlaylists) PlaylistPagePages.MusicPlaylistPage.playlists.Add(song);
                }
            }
        }


        private void addNewSongToPlaylistBtn_Click(object sender, RoutedEventArgs e)
        {
            SongPlaylistModel? playlist;
            playlist = PlaylistPagePages.MusicPlaylistPage.PlaylistListBox.SelectedItem as SongPlaylistModel;

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
                    string errorMessages = "";

                    foreach (string musicPath in openFileDialog.FileNames)
                    {
                        TagLib.File music = TagLib.File.Create(musicPath);

                        //double secondsDuration = music.Properties.Duration.TotalSeconds; is wrong (some return only 70% of the real duration)

                        // Nam: get media file's length
                        IShellProperty prop = Microsoft.WindowsAPICodePack.Shell.ShellObject.FromParsingName(musicPath).Properties.System.Media.Duration;
                        var t = (ulong)prop.ValueAsObject;
                        double secondsDuration = TimeSpan.FromTicks((long)t).TotalSeconds;
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

                        if (PlaylistSongsDatabaseAccess.SaveSong(newSongInfor) == 1)
                        {
                            playlist.Songs.Add(newSongInfor);
                            PlaylistPagePages.MusicPlaylistPage.currentChosenPlaylistSongs.Add(newSongInfor);
                        }
                        else errorMessages += newSongInfor.Name + " already exists in playlist\n";

                    }

                    if (errorMessages != "")
                    {
                        new ShowMessageControl("Errors", errorMessages).Show();
                    }
                }
            }
        }

        private void PageFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }
    }
}
