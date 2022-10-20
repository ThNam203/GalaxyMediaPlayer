using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GalaxyMediaPlayer.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private double totalTimeInSecond;
        private bool isDragging = false; // Nam: if user is dragging, we are not updating the slider value, see more below
        private bool isMuted = false;
        private double volumnBeforeMute; // Nam: this property hold the volumn before mute
        // Nam: format string for each duration
        private string durationFormat = "";
        private string dayFormat = "dd.hh\\:mm\\:ss";
        private string hourFormat = "hh\\:mm\\:ss";
        private string minuteFormat = "mm\\:ss";
        private string secondFormat = "ss";

        // Nam: use this value when a button is not active
        float opacityNotActiveValue = 0.5f;

        public MainPage()
        {
            InitializeComponent();
            InitializeMediaControlButtonsView(); // Nam: if buttons are not active, we grey them out and change volumn slider position
            MyMediaPlayer.Initialize();
            MyMediaPlayer.mediaPlayer.MediaOpened += MediaPlayer_MediaOpened;
        }

        private void MediaPlayer_MediaOpened(object? sender, EventArgs e)
        {
            MyMediaPlayer.isSongOpened = true;
            MyMediaPlayer.isSongPlaying = true;
            SongSliderPanel.Visibility = Visibility.Visible;
            totalTimeInSecond = MyMediaPlayer.GetTotalTimeInSecond();

            AddSongInformationToInfoGrid();
            changeBtnPlayPauseBackgroundImage();

            // Nam: set durationFormat for beautiful ui
            if (totalTimeInSecond < 60) durationFormat = secondFormat;
            else if (totalTimeInSecond < 3600) durationFormat = minuteFormat;
            else if (totalTimeInSecond < 86400) durationFormat = hourFormat;
            else durationFormat = dayFormat;

            tbSongDuration.Text = MyMediaPlayer.mediaPlayer.NaturalDuration.TimeSpan.ToString(durationFormat);
            tbCurrentSongPosition.Text = TimeSpan.FromSeconds(0).ToString(durationFormat);

            DispatcherTimer timerVideoTime = new DispatcherTimer();
            timerVideoTime.Interval = TimeSpan.FromSeconds(1);
            timerVideoTime.Tick += TimerVideoTime_Tick;
            timerVideoTime.Start();
        }

        private void TimerVideoTime_Tick(object? sender, EventArgs e)
        {
            // Nam: If user is dragging slider, we are not updating the slider value
            if (!isDragging) SongDurationSlider.Value = (MyMediaPlayer.mediaPlayer.Position.TotalSeconds / totalTimeInSecond) * 100;
        }

        private void mediaListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavButton? selectedItem = mediaListBox.SelectedItem as NavButton;
            if (selectedItem != null) ContentFrame.Navigate(selectedItem.NavLink);
        }

        private void SongDurationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tbCurrentSongPosition.Text = TimeSpan.FromSeconds(SongDurationSlider.Value / 100 * totalTimeInSecond).ToString(durationFormat);
        }
        
        private void btnPlayPause_Click(object sender, RoutedEventArgs e)
        {
            MyMediaPlayer.isSongPlaying = !MyMediaPlayer.isSongPlaying;
            changeBtnPlayPauseBackgroundImage();

            if (MyMediaPlayer.isSongPlaying)
            {
                if (MyMediaPlayer.isSongOpened)
                {
                    MyMediaPlayer.Continue();
                }
                else
                {
                    MyMediaPlayer.SetPositionInPlaylist(0);
                    MyMediaPlayer.PlayCurrentSong();
                }
            }
            else
            {
                MyMediaPlayer.Pause();
            }
        }

        private void btnRepeat_Click(object sender, RoutedEventArgs e)
        {
            MyMediaPlayer.ChangeRepeatingOptionOnClick();
            SetBtnRepeatViewOnRepeatingOption();
        }

        private void SetBtnRepeatViewOnRepeatingOption()
        {
            if (MyMediaPlayer.repeatingOptions == MyMediaPlayer.RepeatingOption.NoRepeat)
            {
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/repeat_32.png"));
                btnRepeat.Background = brush;
                btnRepeat.Background.Opacity = opacityNotActiveValue;
            }
            else if (MyMediaPlayer.repeatingOptions == MyMediaPlayer.RepeatingOption.RepeatOne)
            {
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/repeat_one_32.png"));
                btnRepeat.Background = brush;
                btnRepeat.Background.Opacity = 1;
            }
            else if (MyMediaPlayer.repeatingOptions == MyMediaPlayer.RepeatingOption.RepeatPlaylist)
            {
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/repeat_32.png"));
                btnRepeat.Background = brush;
                btnRepeat.Background.Opacity = 1;
            }
        }

        // Nam: change playPauseButton's background image
        private void changeBtnPlayPauseBackgroundImage()
        {
            if (MyMediaPlayer.isSongPlaying)
            {
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/pause_32.png"));
                btnPlayPause.Background = brush;
            }
            else
            {
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/play_32.png"));
                btnPlayPause.Background = brush;
            }
        }

        // Nam: add song image, title, artist to displayGrid
        private void AddSongInformationToInfoGrid()
        {
            string songPath = MyMediaPlayer.mediaPlayer.Source.AbsolutePath.Replace("%20", " ");

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
            VolumeSlider.Value = MyMediaPlayer.GetVolumn;
        }

        private void SongDurationSlider_Thumb_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            isDragging = false;
            try
            {
                MyMediaPlayer.mediaPlayer.Position = TimeSpan.FromSeconds(totalTimeInSecond * (sender as Slider).Value / 100);
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

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
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

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnVolumn_Click(object sender, RoutedEventArgs e)
        {
            isMuted = !isMuted;
            if (isMuted)
            {
                volumnBeforeMute = MyMediaPlayer.GetVolumn;
                MyMediaPlayer.SetVolumn(0);
                VolumeSlider.Value = 0;
            }
            else
            {
                MyMediaPlayer.SetVolumn(volumnBeforeMute);
                VolumeSlider.Value = volumnBeforeMute;
            }
            SetVolumnIcon();
        }

        private void SetVolumnIcon()
        {
            if (MyMediaPlayer.GetVolumn == 0) isMuted = true;
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

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MyMediaPlayer.SetVolumn(e.NewValue);
            SetVolumnIcon();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            MyMediaPlayer.PlayNextSong();
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            MyMediaPlayer.PlayPreviousSong();
        }
    }
}