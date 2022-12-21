using GalaxyMediaPlayer.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace GalaxyMediaPlayer.Pages.NavContentPages
{
    /// <summary>
    /// Interaction logic for MusicPage.xaml
    /// </summary>
    public partial class MusicPage : Page
    {
        public class ArtistListItem
        {
            public string ArtistsName { get; set; }
            public List<SystemEntityModel> Songs { get; set; }

            public ArtistListItem(string artistsName, SystemEntityModel firstModel)
            {
                ArtistsName = artistsName;
                Songs = new List<SystemEntityModel> { firstModel };
            }
        }

        private ObservableCollection<SystemEntityModel> musicList = new ObservableCollection<SystemEntityModel>();
        private ObservableCollection<ArtistListItem> artistsList = new ObservableCollection<ArtistListItem>();

        private const string dateFormat = "MM/dd/yyyy hh:mm tt";

        public MusicPage()
        {
            InitializeComponent();
        }

        private void AddNewBtn_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                OpenFolder(new DirectoryInfo(dialog.SelectedPath));
                SetArtirstsListBox();
            }
            
            emptyMusicBorder.Visibility = Visibility.Collapsed;
            artirstListBox.Visibility = Visibility.Visible;
        }

        private void showByArtirsts_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void showByAlbums_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void showBySongs_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void OpenFolder(DirectoryInfo di)
        {
            MainPage.currentMusicBrowsingFolder = di.FullName;

            if (!MyMediaPlayer.isSongOpened) MyMediaPlayer.pathCurrentlyInUse = di.FullName;

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
                            musicList.Add(new SystemEntityModel(
                                name: fi.Name,
                                type: EntityType.Music,
                                path: fi.FullName,
                                dateCreated: fi.CreationTime.ToString(dateFormat),
                                size: fi.Length,
                                extension: fi.Extension));
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        private void SetArtirstsListBox()
        {
            foreach (SystemEntityModel model in musicList)
            {
                TagLib.File file = TagLib.File.Create(model.Path);

                string artistsName = file.Tag.JoinedAlbumArtists;

                if (artistsName == null || artistsName == "") artistsName = file.Tag.JoinedArtists;
                if (artistsName == null || artistsName == "") artistsName = "Unknown Artists";

                bool isAdded = false;
                for (int i = 0; i < artistsList.Count; i++)
                {
                    if (artistsList[i].ArtistsName == artistsName)
                    {
                        artistsList[i].Songs.Add(model);
                        isAdded = true;
                        break;
                    }
                }

                if (!isAdded)
                {
                    artistsList.Add(new ArtistListItem(artistsName, model));
                }
            }

            artirstListBox.ItemsSource = artistsList;
        }
    }
}
