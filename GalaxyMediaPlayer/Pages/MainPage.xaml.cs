using GalaxyMediaPlayer.Pages.NavContentPages;
using GalaxyMediaPlayer.Helpers;
using GalaxyMediaPlayer.Windows;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using GalaxyMediaPlayer.Databases.HomePage;
using System.Linq;
using System.Collections.Generic;
using GalaxyMediaPlayer.Pages.ImagePagePages;
using GalaxyMediaPlayer.Pages.PlaylistPagePages;
using GalaxyMediaPlayer.Models;

namespace GalaxyMediaPlayer.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public static MainPage Instance { get; set; }

        // Nam: use to hold information about where user are navigating (which is for showing MUSIC AdditionalGridInfor)
        public static string currentMusicBrowsingFolder;


        private double totalTimeInSecond;
        private bool isDragging = false; // Nam: if user is dragging, we are not updating the slider value, see more below
        private bool isMuted = false;
        private double volumnBeforeMute; // Nam: this property hold the volumn before mute

        // Nam: format string for song duration
        private string durationFormat = "";

        // Nam: use this value when a button is not active
        private float opacityNotActiveValue = 0.5f;


        public MainPage()
        {
            Instance = this;
            InitializeComponent();
            this.DataContext = this;
            InitializeMediaControlButtonsView(); // Nam: if buttons are not active, we grey them out and change volumn slider position
            MyMusicMediaPlayer.Initialize();
            MyMusicMediaPlayer.mediaPlayer.MediaOpened += MediaPlayer_MediaOpened;
            this.ContentFrame.JournalOwnership = System.Windows.Navigation.JournalOwnership.OwnsJournal;

            // Nam: disable 'backspace' button can go back in frame's stack
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();

            navButtonsListBox.SelectedIndex = 0;
        }

        private void MediaPlayer_MediaOpened(object? sender, EventArgs e)
        {
            MyMusicMediaPlayer.isSongOpened = true;
            MyMusicMediaPlayer.isSongPlaying = true;
            totalTimeInSecond = MyMusicMediaPlayer.GetTotalTimeInSecond();

            // Nam: keeping track of most listened song
            HomePageDatabaseAccess.SaveDataOnListeningMusic(Uri.UnescapeDataString(MyMusicMediaPlayer.mediaPlayer.Source.AbsolutePath));

            SongSliderPanel.Visibility = Visibility.Visible;

            AddSongInformationToInfoGrid();
            changeAllBtnPlayPauseBackgroundImage();
            ActivateControlButtons();
            ChangeAdditionControlVisibilityInInforGrid(false);

            durationFormat = DurationFormatHelper.GetDurationFormatFromTotalSeconds(totalTimeInSecond);
            tbSongDuration.Text = MyMusicMediaPlayer.mediaPlayer.NaturalDuration.TimeSpan.ToString(durationFormat);
            tbCurrentSongPosition.Text = TimeSpan.FromSeconds(0).ToString(durationFormat);

            // Slider update timer
            DispatcherTimer timerVideoTime = new DispatcherTimer();
            timerVideoTime.Interval = TimeSpan.FromSeconds(0.05);
            timerVideoTime.Tick += TimerVideoTime_Tick;
            timerVideoTime.Start();
        }

        private void TimerVideoTime_Tick(object? sender, EventArgs e)
        {
            // Nam: If user is dragging slider, we are not updating the slider value
            if (!isDragging && MyMusicMediaPlayer.isSongPlaying)
            {
                SongDurationSlider.Value = (MyMusicMediaPlayer.mediaPlayer.Position.TotalSeconds / totalTimeInSecond) * 100;
            }
        }

        // Nam: Computer.isUserBrowsing mainly use for playing music outside the computer page
        // which prevent SetPlaylistFromTempPlaylist method to provoke in MyMusicMediaPlayer
        private void navButtonsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavButton? selectedItem = navButtonsListBox.SelectedItem as NavButton;
            if (selectedItem != null)
            {
                ContentFrame.Navigate(selectedItem.NavLink);
            }
        }

        private void SongDurationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tbCurrentSongPosition.Text = TimeSpan.FromSeconds(SongDurationSlider.Value / 100 * totalTimeInSecond).ToString(durationFormat);
        }
        
        private void btnPlayPause_Click(object sender, RoutedEventArgs e)
        {
            if (MainPage.currentMusicBrowsingFolder.StartsWith("__@@##OnImagePlaylist"))
            {
                if(MainWindow.IdPlaylistRunning != null && MainWindow.IdPlaylistRunning != "")
                {
                    ImagePlaylistModel imagePlaylistModel = PlaylistPagePages.ImagePlaylistPage.listBoxImagePlaylist.Items[MainWindow.IndexPlaylistRunning] as ImagePlaylistModel;
                    if (imagePlaylistModel != null)
                    {
                        ShowImageInPlaylistPage showImagePlaylistPage = new ShowImageInPlaylistPage(imagePlaylistModel.Images);
                        MainWindow.Instance.MainFrame.Navigate(showImagePlaylistPage);
                    }
                }
                return;
            }

            if (MainPage.currentMusicBrowsingFolder.StartsWith("ComputerBrowse") && !MyMusicMediaPlayer.isSongOpened)
            {
                if (Computer.selectedPlayableEntities.Count > 0)
                {
                    List<string> entityPaths = Computer.selectedPlayableEntities.Select(x => x.Path).ToList();
                    if (Computer.selectedPlayableEntities[0].Type == EntityType.Video)
                    {
                        MainWindow.Instance.MainFrame.Navigate(new VideoMediaPLayer(entityPaths));
                    }
                    else
                    {
                        MyMusicMediaPlayer.SetNewPlaylist(entityPaths);
                        MyMusicMediaPlayer.PlayCurrentSong();
                    }

                    changeAllBtnPlayPauseBackgroundImage();
                    return;
                }
            }

            MyMusicMediaPlayer.isSongPlaying = !MyMusicMediaPlayer.isSongPlaying;
            changeAllBtnPlayPauseBackgroundImage();

            if (MyMusicMediaPlayer.pathCurrentlyInUse != currentMusicBrowsingFolder)
            {
                MyMusicMediaPlayer.SetPlaylistFromTempPlaylist();
                MyMusicMediaPlayer.PlayCurrentSong();
            }
            else if (MyMusicMediaPlayer.isSongPlaying)
            {
                if (MyMusicMediaPlayer.pathCurrentlyInUse == currentMusicBrowsingFolder)
                {
                    if (MyMusicMediaPlayer.isSongOpened)
                    {
                        MyMusicMediaPlayer.Continue();
                    }
                    else
                    {
                        MyMusicMediaPlayer.SetPlaylistFromTempPlaylist();
                        MyMusicMediaPlayer.PlayCurrentSong();
                    }
                }
            }
            else
            {
                MyMusicMediaPlayer.Pause();
            }
        }

        private void btnPlayPauseInGridInfo_Click(object sender, RoutedEventArgs e)
        {
            MyMusicMediaPlayer.isSongPlaying = !MyMusicMediaPlayer.isSongPlaying;
            if (MyMusicMediaPlayer.pathCurrentlyInUse == currentMusicBrowsingFolder) changeAllBtnPlayPauseBackgroundImage();
            else changeBtnPlayPauseBackgroundInGridInfo();

            if (MyMusicMediaPlayer.isSongPlaying) MyMusicMediaPlayer.Continue();
            else MyMusicMediaPlayer.Pause();
        }

        private void btnRepeat_Click(object sender, RoutedEventArgs e)
        {
            MyMusicMediaPlayer.ChangeRepeatingOptionOnClick();
            SetBtnRepeatViewOnRepeatingOption();
        }

        private void SetBtnRepeatViewOnRepeatingOption()
        {
            if (MyMusicMediaPlayer.repeatingOptions == MyMusicMediaPlayer.RepeatingOption.NoRepeat)
            {
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/repeat_32.png"));
                btnRepeat.Background = brush;
                btnRepeat.Background.Opacity = opacityNotActiveValue;
            }
            else if (MyMusicMediaPlayer.repeatingOptions == MyMusicMediaPlayer.RepeatingOption.RepeatOne)
            {
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/repeat_one_32.png"));
                btnRepeat.Background = brush;
                btnRepeat.Background.Opacity = 1;
            }
            else if (MyMusicMediaPlayer.repeatingOptions == MyMusicMediaPlayer.RepeatingOption.RepeatPlaylist)
            {
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/repeat_32.png"));
                btnRepeat.Background = brush;
                btnRepeat.Background.Opacity = 1;
            }
        }

        // Nam: change playPauseButton's background image
        // which are the default and the one in grid infor
        private void changeAllBtnPlayPauseBackgroundImage()
        {
            ImageBrush brush = new ImageBrush();
            double previousOpacity = btnPlayPause.Background.Opacity;

            if (MyMusicMediaPlayer.isSongPlaying)
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/pause_32.png"));
            else
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/play_32.png"));

            btnPlayPause.Background = brush;
            btnPlayPauseInGridInfo.Background = brush;
            btnPlayPauseInGridInfo.Background.Opacity = previousOpacity;
        }
        private void changeBtnPlayPauseBackgroundInGridInfo()
        {
            ImageBrush brush = new ImageBrush();

            if (MyMusicMediaPlayer.isSongPlaying)
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/pause_32.png"));
            else
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/play_32.png"));

            btnPlayPauseInGridInfo.Background = brush;
        }

        // Nam: add song image, title, artist to displayGrid
        private void AddSongInformationToInfoGrid()
        {
            string songPath = Uri.UnescapeDataString(MyMusicMediaPlayer.mediaPlayer.Source.AbsolutePath);

            SongInfoDisplayGrid.Visibility = Visibility.Visible;
            TagLib.File songFile = TagLib.File.Create(songPath);
            try // song image
            {
                var pic = songFile.Tag.Pictures[0];
                MemoryStream ms = new MemoryStream(pic.Data.Data);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();
                imgSongImage.Source = bitmapImage;
            }
            catch (IndexOutOfRangeException)
            {
                imgSongImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/music_note_64.png"));
            }

            // song title and artist
            tbSongTitle.Text = Path.GetFileName(songPath);
            string albumArtist = string.Join(", ", songFile.Tag.Performers);
            if (albumArtist.Length == 0 || albumArtist.Trim().Length == 0) tbSongArtist.Text = "No artist found";
            else tbSongArtist.Text = albumArtist;
        }

        // Nam: the initial value for opacity is 1, if it's not active, we set it lower
        private void InitializeMediaControlButtonsView()
        {
            SetBtnRepeatViewOnRepeatingOption();
            VolumeSlider.Value = MyMusicMediaPlayer.GetVolumn;
        }

        // Nam: when we browse to another folder,
        // we need to separate the infor grid to the previous played folder
        public void ChangeAdditionControlVisibilityInInforGrid(bool forceShow)
        {
            if (MyMusicMediaPlayer.pathCurrentlyInUse == currentMusicBrowsingFolder && forceShow == false)
            {
                ExtraControlGridInfo.Visibility = Visibility.Collapsed;
            }
            else if (MyMusicMediaPlayer.pathCurrentlyInUse != currentMusicBrowsingFolder || forceShow)
            {
                ExtraControlGridInfo.Visibility = Visibility.Visible;
            }
        }

        private void SetVolumnIcon()
        {
            if (MyMusicMediaPlayer.GetVolumn == 0) isMuted = true;
            else isMuted = false;
            if (isMuted)
            {
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/no_sound_32.png"));
                btnVolumn.Background = brush;
            }
            else
            {
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/volume_32.png"));
                btnVolumn.Background = brush;
            }
        }

        // Nam: change prev, next, playPause, shuffle(random) btn activeness and opacity
        // base on number of songs in current folder

        // Nam: forceShow is use to indicate if we are on the default sreen (drives and fav folders)
        // which is always not active
        public void ChangeButtonsViewOnOpenFolder(bool forceDisable)
        {
            int number = MyMusicMediaPlayer.GetTempPlaylistSize();

            ImageBrush brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/play_32.png"));
            btnPlayPause.Background = brush;

            if (forceDisable)
            {
                DisableControlButtons();
            }
            else if (MyMusicMediaPlayer.isSongOpened && MyMusicMediaPlayer.isSongPlaying && MyMusicMediaPlayer.pathCurrentlyInUse == currentMusicBrowsingFolder)
            {
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/pause_32.png"));
                btnPlayPause.Background = brush;
                ActivateControlButtons();
            }
            // Nam: read the comment of the function, forceShow is the name which is easy to misunderstand
            else if (number <= 0)
            {
                DisableControlButtons();
            }
            else
            {
                btnPlayPause.Background.Opacity = 1;
                btnPrevious.Background.Opacity = opacityNotActiveValue;
                btnNext.Background.Opacity = opacityNotActiveValue;
                btnStop.Background.Opacity = opacityNotActiveValue;

                btnPlayPause.IsEnabled = true;
                btnPrevious.IsEnabled = false;
                btnNext.IsEnabled = false;
                btnStop.IsEnabled = false;
            }
        }

        private void DisableControlButtons()
        {
            btnPlayPause.Background.Opacity = opacityNotActiveValue;
            btnPrevious.Background.Opacity = opacityNotActiveValue;
            btnNext.Background.Opacity = opacityNotActiveValue;
            btnStop.Background.Opacity = opacityNotActiveValue;

            btnPlayPause.IsEnabled = false;
            btnPrevious.IsEnabled = false;
            btnNext.IsEnabled = false;
            btnStop.IsEnabled = false;
        }

        public void ActivateControlButtons()
        {
            btnPlayPause.Background.Opacity = 1;
            btnPrevious.Background.Opacity = 1;
            btnNext.Background.Opacity = 1;
            btnStop.Background.Opacity = 1;

            btnPlayPause.IsEnabled = true;
            btnPrevious.IsEnabled = true;
            btnNext.IsEnabled = true;
            btnStop.IsEnabled = true;
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MyMusicMediaPlayer.SetVolumn(e.NewValue);
            SetVolumnIcon();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            MyMusicMediaPlayer.PlayNextSong();
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            MyMusicMediaPlayer.PlayPreviousSong();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyMusicMediaPlayer.Stop();
            SongInfoDisplayGrid.Visibility = Visibility.Collapsed;
            SongSliderPanel.Visibility = Visibility.Collapsed;

            if (MyMusicMediaPlayer.pathCurrentlyInUse == currentMusicBrowsingFolder) changeAllBtnPlayPauseBackgroundImage();
            else changeBtnPlayPauseBackgroundInGridInfo();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            StopMusic();
        }

        public void StopMusic()
        {
            MyMusicMediaPlayer.Stop();
            SongInfoDisplayGrid.Visibility = Visibility.Collapsed;
            SongSliderPanel.Visibility = Visibility.Collapsed;

            if (MyMusicMediaPlayer.pathCurrentlyInUse == currentMusicBrowsingFolder) changeAllBtnPlayPauseBackgroundImage();
            else changeBtnPlayPauseBackgroundInGridInfo();

            btnStop.Background.Opacity = opacityNotActiveValue;
            btnPrevious.Background.Opacity = opacityNotActiveValue;
            btnNext.Background.Opacity = opacityNotActiveValue;

            btnStop.IsEnabled = false;
            btnPrevious.IsEnabled = false;
            btnNext.IsEnabled = false;
        }

        private void btnRandom_Click(object sender, RoutedEventArgs e)
        {
            MyMusicMediaPlayer.isRandoming = !MyMusicMediaPlayer.isRandoming;

            if (MyMusicMediaPlayer.isRandoming) btnRandom.Background.Opacity = 1;
            else btnRandom.Background.Opacity = opacityNotActiveValue;
        }

        private void SongInfoDisplayGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SongInfoDisplayGrid.Visibility = Visibility.Collapsed;
            MusicDetailPage newPage = new MusicDetailPage();
            ContentFrame.Navigate(newPage);
        }

        private void minimizeImageBorder_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
            MainWindow.Instance.Visibility = Visibility.Hidden;
            SongMinimizedWindow newWindow = new SongMinimizedWindow();
            newWindow.Show();
        }

        private void SongInfoDisplayGrid_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            minimizeImageBorder.Visibility = Visibility.Visible;
        }

        private void SongInfoDisplayGrid_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            minimizeImageBorder.Visibility = Visibility.Hidden;
        }

        private void SongDurationSlider_Thumb_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            isDragging = false;
            try
            {
                MyMusicMediaPlayer.mediaPlayer.Position = TimeSpan.FromSeconds(totalTimeInSecond * (sender as Slider).Value / 100);
            }
            catch (Exception)
            {
                MessageBox.Show("There is something wrong, please feed it back");
            }
        }

        private void SongDurationSlider_Thumb_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            isDragging = true;
        }

        private void btnMinimizeApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void btnMaximizeApp_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;

            }
        }

        private void btnCloseApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnVolumn_Click(object sender, RoutedEventArgs e)
        {
            isMuted = !isMuted;
            if (isMuted)
            {
                volumnBeforeMute = MyMusicMediaPlayer.GetVolumn;
                MyMusicMediaPlayer.SetVolumn(0);
                VolumeSlider.Value = 0;
            }
            else
            {
                MyMusicMediaPlayer.SetVolumn(volumnBeforeMute);
                VolumeSlider.Value = volumnBeforeMute;
            }
            SetVolumnIcon();
        }

        private void ContentFrame_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            var p = ((Frame)sender).Content as Page;
            if (p != null)
            {
                // Nam: we doing this non-sense thing to remember the name to set it later in 
                // actual context (when press back button on playlist list, we use "PlaylistPage" to set
                // the currentMusic... to "PlaylistPage" again
                if (p.Title == "PlaylistPage") currentMusicBrowsingFolder = p.Title;
                else if (p.Title == "MusicPage") currentMusicBrowsingFolder = p.Title;
                else if (p.Title == "ComputerBrowse") currentMusicBrowsingFolder = p.Title;
                else if (p.Title == "HomePage") currentMusicBrowsingFolder= p.Title;

                if (p.Title == "MusicDetailPage")
                {
                    ActivateControlButtons();
                } 
                else if (p.Title == "HomePage")
                {
                    ChangeButtonsViewOnOpenFolder(false);
                    ChangeAdditionControlVisibilityInInforGrid(false);
                }
                else if(p.Title == "PlaylistPage")
                {
                    if(PlaylistPage.CurrentPlaylistType == PlaylistPage.PlaylistPageType.Image)
                    {
                        currentMusicBrowsingFolder = "__@@##OnImagePlaylist";
                    }
                }
                else
                {
                    ChangeButtonsViewOnOpenFolder(true);
                    ChangeAdditionControlVisibilityInInforGrid(true);
                }
            }
        }
    }
}