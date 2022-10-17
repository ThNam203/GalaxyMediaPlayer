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
        public MediaPlayer mediaPlayer = new MediaPlayer();
        private double totalTime;
        private bool isDragging = false; // Nam: if user is dragging, we are not updating the slider value, see more below
        private bool isLooping = false;
        private bool isSongOpened = false; // Nam: determine if a song is currently opened in MediaPlayer, if true, we resume or start it, if false, we open new file and start
        private bool isSongPlaying = false; // Nam: this is used for continue and pause function
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
            mediaPlayer.MediaOpened += MediaPlayer_MediaOpened;
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
            InitializeMediaControlButtonsColor(); // Nam: if buttons are not active, we grey them out
        }
        
        private void MediaPlayer_MediaEnded(object? sender, EventArgs e)
        {
            // Nam: looping song function
            if (isLooping)
            {
                mediaPlayer.Position = TimeSpan.Zero;
                mediaPlayer.Play();
            }
            else
            {
                // Nam: THIS IS NOT SECURED, CHECK IT LATER
                isSongOpened = false;
            }
        }

        private void SongDurationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tbCurrentSongPosition.Text = TimeSpan.FromSeconds(SongDurationSlider.Value / 100 * totalTime).ToString(durationFormat);
        }

        private void MediaPlayer_MediaOpened(object? sender, EventArgs e)
        {
            isSongOpened = true;
            SongSliderPanel.Visibility = Visibility.Visible;
            totalTime = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;

            // Nam: set durationFormat for beautiful ui
            if (totalTime < 60) durationFormat = secondFormat;
            else if (totalTime < 3600) durationFormat = minuteFormat;
            else if (totalTime < 86400) durationFormat = hourFormat;
            else durationFormat = dayFormat;

            tbSongDuration.Text = mediaPlayer.NaturalDuration.TimeSpan.ToString(durationFormat);
            tbCurrentSongPosition.Text = TimeSpan.FromSeconds(0).ToString(durationFormat);

            DispatcherTimer timerVideoTime = new DispatcherTimer();
            timerVideoTime.Interval = TimeSpan.FromSeconds(1);
            timerVideoTime.Tick += TimerVideoTime_Tick;
            timerVideoTime.Start();
        }

        private void TimerVideoTime_Tick(object? sender, EventArgs e)
        {
            // Nam: If user is dragging slider, we are not updating the slider value
            if (!isDragging) SongDurationSlider.Value = (mediaPlayer.Position.TotalSeconds / totalTime) * 100;
        }
        private void btnPlayPause_Click(object sender, RoutedEventArgs e)
        {
            Button playPauseButton = (Button)sender;
            isSongPlaying = !isSongPlaying;

            if (isSongPlaying)
            {
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/pause_32.png"));
                playPauseButton.Background = brush;

                string songPath = "C:\\Users\\hthna\\Downloads\\Die For You - VALORANT Champions 2021 -.mp3";

                if (isSongOpened)
                {
                    mediaPlayer.Play();
                }
                else
                {
                    // Nam: add song image, title, artist to displayGrid
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
                        imgSongImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/music_note_64.png"));
                    }

                    // song title and artist
                    tbSongTitle.Text = Path.GetFileName(songPath);
                    string albumArtist = string.Join(", ", songFile.Tag.Performers);
                    if (albumArtist.Length == 0 || albumArtist.Trim().Length == 0) tbSongArtist.Text = "No artist found";
                    else tbSongArtist.Text = albumArtist;

                    mediaPlayer.Open(new Uri(songPath, UriKind.Absolute));
                    mediaPlayer.Play();
                }
            }
            else
            {
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/play_32.png"));
                playPauseButton.Background = brush;

                if (mediaPlayer.CanPause) mediaPlayer.Pause();
            }
        }

        private void btnLoop_Click(object sender, RoutedEventArgs e)
        {
            isLooping = !isLooping;
            if (isLooping)
            {
                btnLoop.Background.Opacity = 1;
            }
            else
            {
                btnLoop.Background.Opacity = opacityNotActiveValue;
            }
        }

        // Nam: the initial value for opacity is 1, if it's not active, we set it lower
        private void InitializeMediaControlButtonsColor()
        {
            if (!isLooping) btnLoop.Background.Opacity = opacityNotActiveValue;
        }

        private void SongDurationSlider_Thumb_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            isDragging = false;
            try
            {
                mediaPlayer.Position = TimeSpan.FromSeconds(totalTime * (sender as Slider).Value / 100);
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
    }
}