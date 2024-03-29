﻿using GalaxyMediaPlayer.Databases.ImagePage;
using GalaxyMediaPlayer.Databases.SongPlaylist;
using GalaxyMediaPlayer.Databases.VideoPage;
using GalaxyMediaPlayer.Helpers;
using GalaxyMediaPlayer.Models;
using GalaxyMediaPlayer.Pages.PlaylistPagePages;
using GalaxyMediaPlayer.UserControls;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml;

namespace GalaxyMediaPlayer.Pages.NavContentPages
{
    public partial class PlaylistPage : Page
    {
        public static Button NewPlaylistBtn;
        public static Button BackBtn;
        public static Button AddNewSongToPlaylistBtn;
        public static Button AddNewImageToPlaylistBtn;
        public static Button AddNewVideoToPlaylistBtn;

        public static TextBlock PlaylistNameHeader;
        public static ComboBox CbSortPlaylistBy;
        public static ComboBox CbSortPlaylistInImagePlaylist;
        public static StackPanel ChooseCategoryPanel;
        public static Image browseStyleImage;
        public static PlaylistPageType CurrentPlaylistType;

        public static Action<object, RoutedEventArgs> NewPlaylistBtn_Click;

        private bool _IsImagePlaylistPageUsingGridStyle;
        public bool IsImagePlaylistPageUsingGridStyle
        {
            get { return _IsImagePlaylistPageUsingGridStyle; }
            set
            {
                _IsImagePlaylistPageUsingGridStyle = value;
                if (_IsImagePlaylistPageUsingGridStyle)
                {
                    BrowseStyleImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/ComputerPageIcons/list_32.png"));
                }
                else
                {
                    BrowseStyleImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/ComputerPageIcons/four_squares_32.png"));
                }
            }
        }

        private bool _IsRuningImagePlaylist;
        public bool IsRuningImagePlaylist
        {
            get { return _IsRuningImagePlaylist; }
            set
            {
                _IsRuningImagePlaylist = value;
                if (_IsRuningImagePlaylist)
                {
                    currentPlaylistType = PlaylistPageType.Image;
                    ChangeToPageImage();
                    _IsRuningImagePlaylist = false;
                }
            }
        }



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
            AddNewVideoToPlaylistBtn = addNewVideoToPlaylistBtn;
            AddNewImageToPlaylistBtn = addNewImageToPlaylistBtn;
            CbSortPlaylistBy = cbSortPlaylistBy;
            CbSortPlaylistInImagePlaylist = cbSortPlaylistInImagePlaylist;
            BackBtn = backBtn;
            ChooseCategoryPanel = chooseCategoryPanel;
            NewPlaylistBtn_Click = this.newPlaylistBtn_Click;
            browseStyleImage = BrowseStyleImage;
            IsRuningImagePlaylist = MainWindow.IsRuningImagePlaylist;
            CurrentPlaylistType = currentPlaylistType;

            PageFrame.Navigate(new Uri("/Pages/PlaylistPagePages/MusicPlaylistPage.xaml", UriKind.Relative));
        }

        private void showMusicPlaylistsBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            showMusicPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.White;
            showVideosPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.Transparent;
            showImagesPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.Transparent;

            currentPlaylistType = PlaylistPageType.Music;
            MainPage.currentMusicBrowsingFolder = "PlaylistPage";
            PageFrame.Navigate(new Uri("/Pages/PlaylistPagePages/MusicPlaylistPage.xaml", UriKind.Relative));
        }

        private void showVideosPlaylistsBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            showVideosPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.White;
            showMusicPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.Transparent;
            showImagesPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.Transparent;

            currentPlaylistType = PlaylistPageType.Video;

            MainPage.currentMusicBrowsingFolder = "__@@##OnVideoPlaylist";
            PageFrame.Navigate(new Uri("/Pages/PlaylistPagePages/VideoPlaylistPage.xaml", UriKind.Relative));
        }

        public void showImagesPlaylistsBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            showImagesPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.White;
            showMusicPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.Transparent;
            showVideosPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.Transparent;

            currentPlaylistType = PlaylistPageType.Image;
            cbSortPlaylistBy.SelectedIndex = -1;
            IsImagePlaylistPageUsingGridStyle = MainWindow.IsImagePlaylistPageUsingGridStyle = false;
            MainWindow.IsImagePlaylistPageUsingGridStyle = false;

            MainPage.currentMusicBrowsingFolder = "__@@##OnImagePlaylist";
            PageFrame.Navigate(new Uri("/Pages/PlaylistPagePages/ImagePlaylistPage.xaml", UriKind.Relative));
        }

        void ChangeToPageImage()
        {
            showImagesPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.White;
            showMusicPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.Transparent;
            showVideosPlaylistsBtn.BorderBrush = System.Windows.Media.Brushes.Transparent;

            currentPlaylistType = PlaylistPageType.Image;
            cbSortPlaylistBy.SelectedIndex = -1;
            IsImagePlaylistPageUsingGridStyle = MainWindow.IsImagePlaylistPageUsingGridStyle = false;
            MainWindow.IsImagePlaylistPageUsingGridStyle = false;

            MainPage.currentMusicBrowsingFolder = "__@@##OnImagePlaylist";
            PageFrame.Navigate(new Uri("/Pages/PlaylistPagePages/ImagePlaylistPage.xaml", UriKind.Relative));
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
                NewPlaylistControl newPlaylistControl = new NewPlaylistControl(AddNewVideoPlaylist);
                MainWindow.ShowCustomMessageBoxInMiddle(newPlaylistControl);
            }
            else if (currentPlaylistType == PlaylistPageType.Image)
            {
                NewPlaylistControl newPlaylistControl = new NewPlaylistControl(AddNewImagePlaylist);
                MainWindow.ShowCustomMessageBoxInMiddle(newPlaylistControl);
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


                MainPage.currentMusicBrowsingFolder = "PlaylistPage";  
            }
            else if (currentPlaylistType == PlaylistPageType.Video)
            {

                PlaylistPagePages.VideoPlaylistPage.PlaylistListBox.Visibility = Visibility.Visible;
                PlaylistPagePages.VideoPlaylistPage.PlaylistVideosDataGrid.Visibility = Visibility.Collapsed;
                addNewVideoToPlaylistBtn.Visibility = Visibility.Collapsed;
                PlaylistPage.BackBtn.Visibility = Visibility.Collapsed;


                MainPage.currentMusicBrowsingFolder = "__@@##OnVideoPlaylist";
            }
            else if (currentPlaylistType == PlaylistPageType.Image)
            {
                CbSortPlaylistBy.SelectedIndex = -1;
                IsImagePlaylistPageUsingGridStyle = MainWindow.IsImagePlaylistPageUsingGridStyle = false;
                PlaylistPagePages.ImagePlaylistPage.ShowBtnOfPage1();


                MainPage.currentMusicBrowsingFolder = "__@@##OnImagePlaylist";
            }

            
            MainPage.Instance.ChangeButtonsViewOnOpenFolder(forceDisable: true);
            MainPage.Instance.ChangeAdditionControlVisibilityInInforGrid(forceShow: true);
        }

        private void AddNewMusicPlaylist(string playlistName)
        {
            SongPlaylistModel playlist = new SongPlaylistModel(playlistName);
            MusicPlaylistPage.playlists.Add(playlist);

            PlaylistDatabaseAccess.SavePlaylist(playlist);

            MainWindow.ClearAllMessageBox();

            if (MusicPlaylistPage.playlists.Count > 0)
            {
                MusicPlaylistPage.EmptyPlaylistBorder.Visibility = Visibility.Collapsed;
                NewPlaylistBtn.Visibility = Visibility.Visible;
            }
        }
        private void AddNewVideoPlaylist(string playlistName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement root = xmlDocument.CreateElement("root");
            xmlDocument.AppendChild(root);
            string path = AppDomain.CurrentDomain.BaseDirectory + "Databases\\VideoPlayListPage";
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            xmlDocument.Save(path + "\\" + playlistName + ".xml");
            VideoPaths videoPaths = new VideoPaths(path + "\\" + playlistName + ".xml");
            VideoPlaylistPage.playlistSource.Add(videoPaths);
            MainWindow.ClearAllMessageBox();
            if (PlaylistPagePages.VideoPlaylistPage.PlaylistListBox.Items.Count > 0)
            {
                PlaylistPage.NewPlaylistBtn.Visibility = Visibility.Visible;
                VideoPlaylistPage.EmptyPlaylistBorder.Visibility = Visibility.Collapsed;
            }

            NewPlaylistBtn.Visibility = Visibility.Visible;
        }
        private bool IsDataBaseExists(StringProperty playlistName)
        {
            return (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Databases\\VideoPage\\" + playlistName + ".xml"));
        }

        private void AddNewImagePlaylist(string PlaylistName)
        {
            ImagePlaylistModel imagePlaylistModel = new ImagePlaylistModel(PlaylistName);
            ImagePlaylistPage.ImagePlaylists.Add(imagePlaylistModel);
            ImagePlaylistPage.listBoxImagePlaylist.Items.Add(imagePlaylistModel);

            ImagesPlaylistDBAccess.SaveImagePlaylist(imagePlaylistModel);
            MainWindow.ClearAllMessageBox();
            if (ImagePlaylistPage.ImagePlaylists.Count > 0)
            {
                ImagePlaylistPage.BorderListView.Visibility = Visibility.Collapsed;
                ImagePlaylistPage.listBoxImagePlaylist.Visibility = Visibility.Visible;
            }
        }

        private void cbSortPlaylistBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currentPlaylistType == PlaylistPageType.Music)
            {
                if (cbSortPlaylistBy.SelectedItem != null)
                {
                    int sortIndex = cbSortPlaylistBy.SelectedIndex;
                    if (sortIndex == 0)
                    {
                        List<SongPlaylistModel> tempPlaylists = new List<SongPlaylistModel>(MusicPlaylistPage.playlists);
                        tempPlaylists.Sort((x, y) => x.Name.CompareTo(y.Name));

                        MusicPlaylistPage.playlists.Clear();
                        foreach (SongPlaylistModel song in tempPlaylists) MusicPlaylistPage.playlists.Add(song);
                    }
                    else if (sortIndex == 1)
                    {
                        List<SongPlaylistModel> tempPlaylists = new List<SongPlaylistModel>(MusicPlaylistPage.playlists);
                        tempPlaylists.Sort((x, y) =>
                            DateTime.Parse(y.TimeCreated, CultureInfo.InvariantCulture)
                            .CompareTo(DateTime.Parse(x.TimeCreated, CultureInfo.InvariantCulture)));

                        MusicPlaylistPage.playlists.Clear();
                        foreach (SongPlaylistModel song in tempPlaylists) MusicPlaylistPage.playlists.Add(song);
                    }
                }
            }
            else if (currentPlaylistType == PlaylistPageType.Video)
            {

            }
            else if (currentPlaylistType == PlaylistPageType.Image)
            {
                if (cbSortPlaylistBy.SelectedItem != null)
                {
                    int sortIndex = cbSortPlaylistBy.SelectedIndex;

                    if (sortIndex == 0)
                    {
                        List<ImagePlaylistModel> list = new List<ImagePlaylistModel>(ImagePlaylistPage.ImagePlaylists);
                        list.Sort((x, y) => x.PlaylistName.CompareTo(y.PlaylistName));

                        ImagePlaylistPage.listBoxImagePlaylist.Items.Clear();
                        foreach (ImagePlaylistModel model in list)
                        {
                            ImagePlaylistPage.listBoxImagePlaylist.Items.Add(model);
                        }
                    }
                    else if (sortIndex == 1)
                    {
                        List<ImagePlaylistModel> list = new List<ImagePlaylistModel>(ImagePlaylistPage.ImagePlaylists);
                        list.Sort((x, y) => x.TimeCreated.CompareTo(y.TimeCreated));

                        ImagePlaylistPage.listBoxImagePlaylist.Items.Clear();
                        foreach (ImagePlaylistModel model in list)
                        {
                            ImagePlaylistPage.listBoxImagePlaylist.Items.Add(model);
                        }
                    }
                }
            }
        }

        private void cbSortPlaylistInImagePlaylist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currentPlaylistType == PlaylistPageType.Image)
            {
                if (cbSortPlaylistInImagePlaylist.SelectedItem != null)
                {
                    int sortIndex = cbSortPlaylistInImagePlaylist.SelectedIndex;

                    if (sortIndex == 0)
                    {
                        ImagePlaylistModel? playlist = PlaylistPagePages.ImagePlaylistPage.listBoxImagePlaylist.SelectedItem as ImagePlaylistModel;
                        if (playlist != null)
                        {
                            List<ImageModel> list = new List<ImageModel>(playlist.Images);
                            list.Sort((x, y) => x.Name.CompareTo(y.Name));

                            PlaylistPagePages.ImagePlaylistPage.ListViewImage.Items.Clear();
                            PlaylistPagePages.ImagePlaylistPage.BrowseDataGrid.Items.Clear();
                            foreach (ImageModel model in list)
                            {
                                PlaylistPagePages.ImagePlaylistPage.ListViewImage.Items.Add(model);
                                PlaylistPagePages.ImagePlaylistPage.BrowseDataGrid.Items.Add(model);
                            }
                        }
                    }
                    else if (sortIndex == 1)
                    {
                        ImagePlaylistModel? playlist = PlaylistPagePages.ImagePlaylistPage.listBoxImagePlaylist.SelectedItem as ImagePlaylistModel;
                        if (playlist != null)
                        {
                            List<ImageModel> list = new List<ImageModel>(playlist.Images);
                            list.Sort((x, y) => x.dateCreated.CompareTo(y.dateCreated));

                            PlaylistPagePages.ImagePlaylistPage.ListViewImage.Items.Clear();
                            PlaylistPagePages.ImagePlaylistPage.BrowseDataGrid.Items.Clear();
                            foreach (ImageModel model in list)
                            {
                                PlaylistPagePages.ImagePlaylistPage.ListViewImage.Items.Add(model);
                                PlaylistPagePages.ImagePlaylistPage.BrowseDataGrid.Items.Add(model);
                            }
                        }
                    }
                    else if (sortIndex == 2)
                    {
                        ImagePlaylistModel? playlist = PlaylistPagePages.ImagePlaylistPage.listBoxImagePlaylist.SelectedItem as ImagePlaylistModel;
                        if (playlist != null)
                        {
                            List<ImageModel> list = new List<ImageModel>(playlist.Images);
                            list.Sort((x, y) => x.length.CompareTo(y.length));

                            PlaylistPagePages.ImagePlaylistPage.ListViewImage.Items.Clear();
                            PlaylistPagePages.ImagePlaylistPage.BrowseDataGrid.Items.Clear();
                            foreach (ImageModel model in list)
                            {
                                PlaylistPagePages.ImagePlaylistPage.ListViewImage.Items.Add(model);
                                PlaylistPagePages.ImagePlaylistPage.BrowseDataGrid.Items.Add(model);
                            }
                        }
                    }
                }
            }
        }

        private void addNewSongToPlaylistBtn_Click(object sender, RoutedEventArgs e)
        {
            SongPlaylistModel? playlist;
            playlist = MusicPlaylistPage.PlaylistListBox.SelectedItem as SongPlaylistModel;

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
                            MusicPlaylistPage.currentChosenPlaylistSongs.Add(newSongInfor);

                            MyMusicMediaPlayer.SetTempPlaylist(MusicPlaylistPage.currentChosenPlaylistSongs.Select(s => s.Path).ToList());
                            MainPage.Instance.ChangeButtonsViewOnOpenFolder(forceDisable: false);
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
        private void addNewVideoToPlaylistBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "Video files |*.wmv; *.3g2; *.3gp; *.3gp2; *.3gpp; *.amv; *.asf;  *.avi; *.bin; *.cue; *.divx; *.dv; *.flv; *.gxf; *.iso; *.m1v; *.m2v; *.m2t; *.m2ts; *.m4v; " +
             " *.mkv; *.mov; *.mp2; *.mp2v; *.mp4; *.mp4v; *.mpa; *.mpe; *.mpeg; *.mpeg1; *.mpeg2; *.mpeg4; *.mpg; *.mpv2; *.mts; *.nsv; *.nuv; *.ogg; *.ogm;" +
             " *.ogv; *.ogx; *.ps; *.rec; *.rm; *.rmvb; *.tod; *.ts; *.tts; *.vob; *.vro; *.webm; *.dat; ";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string file in ofd.FileNames)
                {
                    VideoDisplay videoDisplay = new VideoDisplay(file);
                    VideoPlaylistPage.source.Add(videoDisplay);
                    var x = VideoPlaylistPage.PlaylistListBox.SelectedItem as VideoPaths;
                    x.AddPath(file);
                }
            }
        }
        private void addNewImageToPlaylistBtn_Click(object sender, RoutedEventArgs e)
        {
            ImagePlaylistModel? playlist;
            playlist = ImagePlaylistPage.listBoxImagePlaylist.SelectedItem as ImagePlaylistModel;

            if (playlist != null)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                string filter = "SupportedFormat|";
                foreach (string extenstion in SupportedExtensions.IMAGE_EXTENSION)
                {
                    filter += "*." + extenstion + ";";
                }
                openFileDialog.Filter = filter;
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == true)
                {
                    foreach (string file in openFileDialog.FileNames)
                    {
                        //add filePath to listview
                        FileInfo fi = new FileInfo(file);
                        string name = Path.GetFileName(file);
                        string id = Guid.NewGuid().ToString();
                        string date = fi.CreationTime.ToString();
                        string size = fi.Length.ToString();
                        ImageModel imgModel = new ImageModel(playlist.Id, id, name, fi.FullName, date, size);

                        if (ImagesInPlaylistDBAccess.SaveImageIntoPlaylist(imgModel) != -1)
                        {
                            playlist.Images.Add(imgModel);

                            ImagePlaylistPage.ListViewImage.Items.Add(imgModel);
                            ImagePlaylistPage.BrowseDataGrid.Items.Add(imgModel);

                            // Nam: activate playPauseBtn if there is an image
                            MainPage.Instance.btnPlayPause.Background.Opacity = 1;
                            MainPage.Instance.btnPlayPause.IsEnabled = true;

                        }
                    }
                }
            }
        }

        private void BrowseStyleImage_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (currentPlaylistType == PlaylistPageType.Image)
            {
                MainWindow.IsImagePlaylistPageUsingGridStyle = !MainWindow.IsImagePlaylistPageUsingGridStyle;
                IsImagePlaylistPageUsingGridStyle = MainWindow.IsImagePlaylistPageUsingGridStyle;
                if (IsImagePlaylistPageUsingGridStyle)
                {
                    PlaylistPagePages.ImagePlaylistPage.ShowBtnOfPage3();
                    BrowseStyleImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/ComputerPageIcons/list_32.png"));
                }
                else
                {
                    PlaylistPagePages.ImagePlaylistPage.ShowBtnOfPage2();
                    BrowseStyleImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/ComputerPageIcons/four_squares_32.png"));
                }
            }
        }

        private void PageFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }


    }
}
