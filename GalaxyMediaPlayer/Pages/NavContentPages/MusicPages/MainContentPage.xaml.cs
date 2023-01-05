using GalaxyMediaPlayer.Helpers;
using GalaxyMediaPlayer.Models;
using GalaxyMediaPlayer.Pages.NavContentPages.MusicPages;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace GalaxyMediaPlayer.Pages.NavContentPages.MusicPage
{
    /// <summary>
    /// Interaction logic for MainContentPage.xaml
    /// </summary>
    public partial class MainContentPage : Page
    {
        private readonly string DATABASE_PATH = AppDomain.CurrentDomain.BaseDirectory + "Databases\\MusicPage\\Database.txt";
        public class ArtistAndAlbumListItem
        {
            public string Name { get; set; }

            // Nam: use this property to get image
            public string FirstSongPath { get; set; }

            public ArtistAndAlbumListItem(string name, string firstSongPath)
            {
                Name = name;
                FirstSongPath = firstSongPath;
            }
        }

        // Nam: showing all music being contained
        private ObservableCollection<SongInfor> musicList = new ObservableCollection<SongInfor>();

        private ObservableCollection<ArtistAndAlbumListItem> artistsList = new ObservableCollection<ArtistAndAlbumListItem>();
        private ObservableCollection<ArtistAndAlbumListItem> albumList = new ObservableCollection<ArtistAndAlbumListItem>();

        private int currentPageIndex = 1; // Nam: 1 is Artist, 2 is Album, 3 is Songs

        public MainContentPage()
        {
            InitializeComponent();

            GetDataFromDatabase();
            ResetArtistsList();
            ResetAlbumsList();

            artirstListBox.ItemsSource = artistsList;
            albumsListBox.ItemsSource = albumList;
            songsDataGrid.ItemsSource = musicList;

            if (musicList.Count == 0)
            {
                emptyMusicBorder.Visibility = Visibility.Visible;
            }
        }

        public void ResetPageData()
        {
            musicList.Clear();
            GetDataFromDatabase();
            ResetAlbumsList();
            ResetArtistsList();
        }
        private void GetDataFromDatabase()
        {
            CreateDatabaseFileIfNotExist();
            string[] lines = File.ReadAllLines(DATABASE_PATH);

            foreach (string line in lines) if (line != null && line.Length != 0) OpenFolder(new DirectoryInfo(line));
        }

        private void AddNewBtn_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SaveFolderToDatabase(dialog.SelectedPath);
                OpenFolder(new DirectoryInfo(dialog.SelectedPath));
                ResetAlbumsList();
                ResetArtistsList();
            }

            if (musicList.Count > 0)
            {
                emptyMusicBorder.Visibility = Visibility.Collapsed;

                if (currentPageIndex == 1) artirstListBox.Visibility = Visibility.Visible;
                else if (currentPageIndex == 2) albumsListBox.Visibility = Visibility.Visible;
                else if (currentPageIndex == 3) songsDataGrid.Visibility = Visibility.Visible;
            }
        }

        private void CreateDatabaseFileIfNotExist()
        {
            if (!File.Exists(DATABASE_PATH))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(DATABASE_PATH));

                FileStream fs = File.Create(DATABASE_PATH);
                fs.Close();
            }
        }

        private void SaveFolderToDatabase(string newFolderPath)
        {
            CreateDatabaseFileIfNotExist();

            // Nam: check if it existed, if yes then exit, not saving
            string[] folders = File.ReadAllLines(DATABASE_PATH);
            foreach (string folderPath in folders) if (folderPath == newFolderPath) return; 

            File.AppendAllText(DATABASE_PATH, newFolderPath + Environment.NewLine);
        }

        private void showByArtirsts_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (currentPageIndex != 1)
            {
                showByArtirsts.BorderBrush = Brushes.White;
                showByAlbums.BorderBrush = Brushes.Transparent;
                showBySongs.BorderBrush = Brushes.Transparent;
                currentPageIndex = 1;

                if (musicList.Count > 0)
                {
                    artirstListBox.Visibility = Visibility.Visible;
                    songsDataGrid.Visibility = Visibility.Collapsed;
                    albumsListBox.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void showByAlbums_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (currentPageIndex != 2)
            {
                showByArtirsts.BorderBrush = Brushes.Transparent;
                showByAlbums.BorderBrush = Brushes.White;
                showBySongs.BorderBrush = Brushes.Transparent;
                currentPageIndex = 2;

                if (musicList.Count > 0)
                {
                    artirstListBox.Visibility = Visibility.Collapsed;
                    songsDataGrid.Visibility = Visibility.Collapsed;
                    albumsListBox.Visibility = Visibility.Visible;
                }
            }
        }

        private void showBySongs_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (currentPageIndex != 3)
            {
                showByArtirsts.BorderBrush = Brushes.Transparent;
                showByAlbums.BorderBrush = Brushes.Transparent;
                showBySongs.BorderBrush = Brushes.White;
                currentPageIndex = 3;

                if (musicList.Count > 0)
                {
                    songsDataGrid.Visibility = Visibility.Visible;
                    artirstListBox.Visibility = Visibility.Collapsed;
                    albumsListBox.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void OpenFolder(DirectoryInfo di)
        {
            try
            {
                foreach (DirectoryInfo direcInfo in di.EnumerateDirectories())
                {
                    if (direcInfo.Exists)
                    {
                        OpenFolder(direcInfo);
                    }
                }

                // add media files and pass every music to MyMediaPlayer
                foreach (FileInfo fi in di.EnumerateFiles("*.*"))
                {
                    if (fi.Exists)
                    {
                        var fileExtension = fi.Extension.TrimStart('.').ToLowerInvariant();

                        if (SupportedExtensions.MUSIC_EXTENSION.Contains(fileExtension))
                        {
                            TagLib.File music = TagLib.File.Create(fi.FullName);

                            // Nam: if we cant get the song title, we use its name shown on disk
                            string songName;
                            if (music.Tag.Title == "" || music.Tag.Title == null) songName = Path.GetFileName(fi.FullName);
                            else songName = music.Tag.Title;

                            string albumName = music.Tag.Album;
                            if (albumName == null || albumName == "") albumName = "Unknown Album";

                            string artistsName = music.Tag.JoinedAlbumArtists;
                            if (artistsName == null || artistsName == "") artistsName = music.Tag.JoinedArtists;
                            if (artistsName == null || artistsName == "") artistsName = "Unknown Artists";

                            // Nam: get media file's length
                            IShellProperty prop = Microsoft.WindowsAPICodePack.Shell.ShellObject.FromParsingName(fi.FullName).Properties.System.Media.Duration;
                            var t = (ulong)prop.ValueAsObject;
                            double secondsDuration = TimeSpan.FromTicks((long)t).TotalSeconds;
                            string durationFormat = DurationFormatHelper.GetDurationFormatFromTotalSeconds(secondsDuration);
                            string length = TimeSpan.FromSeconds(secondsDuration).ToString(durationFormat);

                            // Nam: SongInfor contains songplaylistId WHICH IS WE DON'T NEED, we put garbage value to it
                            musicList.Add(new SongInfor(
                                playlistId: "xxx_id_kekw",
                                name: songName,
                                album: albumName,
                                artist: artistsName,
                                performer: music.Tag.JoinedPerformers,
                                length: length,
                                path: fi.FullName
                            ));
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        private void ResetArtistsList()
        {
            artistsList.Clear();

            foreach (SongInfor song in musicList)
            {
                bool isAdded = false;
                for (int i = 0; i < artistsList.Count; i++)
                {
                    if (artistsList[i].Name == song.Artist)
                    {
                        isAdded = true;
                        break;
                    }
                }

                if (!isAdded)
                {
                    artistsList.Add(new ArtistAndAlbumListItem(song.Artist, song.Path));
                }
            }
        }

        private void ResetAlbumsList()
        {
            albumList.Clear();

            foreach (SongInfor song in musicList)
            {
                bool isAdded = false;
                for (int i = 0; i < albumList.Count; i++)
                {
                    if (albumList[i].Name == song.Album)
                    {
                        isAdded = true;
                        break;
                    }
                }

                if (!isAdded)
                {
                    albumList.Add(new ArtistAndAlbumListItem(song.Album, song.Path));
                }
            }
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

        private void listboxItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender == null) return;

            // Nam: which is artist listbox
            if (currentPageIndex == 1)
            {
                if (artirstListBox.SelectedIndex == -1 || artirstListBox.SelectedItem == null) return;

                ArtistAndAlbumListItem chosenItem = (ArtistAndAlbumListItem)artirstListBox.SelectedItem;
                List<SongInfor> chosenListboxSongs = musicList.Where(x => x.Artist == chosenItem.Name).ToList();

                ListBoxItemContentShowPage showPage = new ListBoxItemContentShowPage(chosenListboxSongs);
                MusicPages.MainPage.contentFrame.Navigate(showPage);
            }
            // Nam: which is album listbox
            else if (currentPageIndex == 2)
            {
                if (albumsListBox.SelectedIndex == -1 || albumsListBox.SelectedItem == null) return;

                ArtistAndAlbumListItem chosenItem = (ArtistAndAlbumListItem)albumsListBox.SelectedItem;
                List<SongInfor> chosenListboxSongs = musicList.Where(x => x.Artist == chosenItem.Name).ToList();

                ListBoxItemContentShowPage showPage = new ListBoxItemContentShowPage(chosenListboxSongs);
                MusicPages.MainPage.contentFrame.Navigate(showPage);
            }
        }
    }
}
