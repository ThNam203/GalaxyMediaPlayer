using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using GalaxyMediaPlayer.Helpers;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Drawing;

namespace GalaxyMediaPlayer.Windows
{
    /// <summary>
    /// Interaction logic for SongMinimizedWindow.xaml
    /// </summary>
    public partial class SongMinimizedWindow : Window
    {
        private string durationFormat;
        private double totalTimeInSecond;
        private bool isDragging = false; // Nam: if user is dragging, we are not updating the slider value, see more below
        public SongMinimizedWindow()
        {
            InitializeComponent();
            SetUpView();
            SetUpSlider();
            SetBtnRepeatViewOnRepeatingOption();
        }

        private void SetUpSlider()
        {
            totalTimeInSecond = MyMediaPlayer.GetTotalTimeInSecond();
            durationFormat = DurationFormatHelper.GetDurationFormatFromTotalSeconds(totalTimeInSecond);

            tbSongDuration.Text = MyMediaPlayer.mediaPlayer.NaturalDuration.TimeSpan.ToString(durationFormat);
            tbCurrentSongPosition.Text = TimeSpan.FromSeconds(0).ToString(durationFormat);

            DispatcherTimer timerVideoTime = new DispatcherTimer();
            timerVideoTime.Interval = TimeSpan.FromSeconds(0.5);
            timerVideoTime.Tick += TimerVideoTime_Tick;
            timerVideoTime.Start();
        }

        private void SetUpView()
        {
            ImageBrush brush = new ImageBrush();
            if (MyMediaPlayer.isSongPlaying)
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/pause_32.png"));
            else
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/play_32.png"));
            btnPlayPause.Background = brush;

            string songPath = Uri.UnescapeDataString(MyMediaPlayer.GetSource.AbsolutePath);
            tbSongTitle.Text = Path.GetFileNameWithoutExtension(songPath);

            TagLib.File song = TagLib.File.Create(songPath);
            try
            {
                TagLib.IPicture pic = song.Tag.Pictures[0];
                MemoryStream ms = new MemoryStream(pic.Data.Data);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                imgSongImage.Source = bitmapImage;
            }
            catch (IndexOutOfRangeException)
            {
                imgSongImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/ComputerPageIcons/ic_audio_file.png"));
            }
        }

        private void TimerVideoTime_Tick(object? sender, EventArgs e)
        {
            // Nam: If user is dragging slider, we are not updating the slider value
            if (!isDragging) SongDurationSlider.Value = (MyMediaPlayer.mediaPlayer.Position.TotalSeconds / totalTimeInSecond) * 100;
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

        private void SongDurationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tbCurrentSongPosition.Text = TimeSpan.FromSeconds(SongDurationSlider.Value / 100 * totalTimeInSecond).ToString(durationFormat);
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            MyMediaPlayer.PlayNextSong();
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            MyMediaPlayer.PlayPreviousSong();
        }

        private void btnRepeat_Click(object sender, RoutedEventArgs e)
        {
            MyMediaPlayer.ChangeRepeatingOptionOnClick();
            SetBtnRepeatViewOnRepeatingOption();
        }

        private void btnPlayPause_Click(object sender, RoutedEventArgs e)
        {
            ImageBrush brush = new ImageBrush();
            MyMediaPlayer.isSongPlaying = !MyMediaPlayer.isSongPlaying;
            if (MyMediaPlayer.isSongPlaying)
            {
                MyMediaPlayer.Continue();
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/pause_32.png"));
            }
            else
            {
                MyMediaPlayer.Pause();
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/play_32.png"));
            }

            btnPlayPause.Background = brush;
        }

        private void btnRandom_Click(object sender, RoutedEventArgs e)
        {
            MyMediaPlayer.isRandoming = !MyMediaPlayer.isRandoming;

            if (MyMediaPlayer.isRandoming) btnRandom.Background.Opacity = 1;
            else btnRandom.Background.Opacity = 0.5;
        }

        private void SetBtnRepeatViewOnRepeatingOption()
        {
            if (MyMediaPlayer.repeatingOptions == MyMediaPlayer.RepeatingOption.NoRepeat)
            {
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/repeat_32.png"));
                btnRepeat.Background = brush;
                btnRepeat.Background.Opacity = 0.5;
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

        private void btnMinimizeApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void btnCloseApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainWindow.Instance.Visibility = Visibility.Visible;
        }

        private void btnMaximizeApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainWindow.Instance.Visibility = Visibility.Visible;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }
    }
}
