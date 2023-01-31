using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.IO;
using GalaxyMediaPlayer.Pages.NavContentPages.MusicPage;
using System.Windows.Shapes;
using Xabe.FFmpeg;

namespace GalaxyMediaPlayer.Pages
{
    /// <summary>
    /// Interaction logic for VideoMediaPLayer.xaml
    /// </summary>
    public partial class VideoMediaPLayer : Page
    {

        public class SrtContent
        {
            public string Text { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }
            public string Segment { get; set; }
        }
        private List<SrtContent> ParseSRT(string srtFilePath)
        {
            var fileContent = File.ReadAllLines(srtFilePath);
            var content = new List<SrtContent>();
            var segment = 1;
            for (int item = 0; item < fileContent.Length; item++)
            {
                if (segment.ToString() == fileContent[item])
                {
                    content.Add(new SrtContent
                    {
                        Segment = segment.ToString(),
                        StartTime = fileContent[item + 1].Substring(0, fileContent[item + 1].LastIndexOf("-->")).Trim(),
                        EndTime = fileContent[item + 1].Substring(fileContent[item + 1].LastIndexOf("-->") + 3).Trim(),
                        Text = fileContent[item + 2] + fileContent[item + 3]

                    });
                    //H.nam The block numbers of SRT like 1, 2, 3, ... and so on
                    segment++;
                    //H.Nam Iterate one block at a time
                    item += 3;
                }
            }
            return content;

        }
        List<SrtContent> test = new List<SrtContent>();

        bool repeatIsOn = false;
        bool subtitlesIsOn = false;
        bool randomIsOn = false;
        bool fullSizeIsOn = true;
        bool sliderSeekIsDown = false;
        bool mouseCaptured = false;
        DispatcherTimer timer;
        DispatcherTimer timer1;
        System.Windows.Forms.Timer timer2;
        string subtiles;
        List<string> videoPaths;
        List<string> subtitlePaths;
        int VideoPathIndex = 0;
        Thickness thickness = new Thickness(0, 40, 0, 60);

        public VideoMediaPLayer(List<string> videoPath, int startIndex = 0)
        {
            InitializeComponent();
            videoPaths = new List<string>();
            this.videoPaths = videoPath;
            VideoPathIndex = startIndex;
            // Nam: if there is music playing, stop it
            if (MyMediaPlayer.isSongPlaying) MyMediaPlayer.Pause();

            Load();
        }

        private void Load()
        {
            timer = new DispatcherTimer(); //H.Nam: DispatcherTimer for displaying video current position and subtitles
            timer1 = new DispatcherTimer(); //H.nam: Create fade effect in FullScreen mode
            timer1.Interval = TimeSpan.FromMilliseconds(20);
            timer1.Tick += new EventHandler(Timer1_Tick);
            timer2 = new System.Windows.Forms.Timer();
            timer2.Interval = 30;
            timer2.Tick += Timer2_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Tick += new EventHandler(Timer_Tick);
            btnSubtitles.Opacity = 0.5;
            btnRandom.Opacity = 0.5;
            btnRepeat.Opacity = 0.5;
            subtitlePaths = new List<string>();
            if (videoPaths.Count > 0)
            {
                media.Source = new Uri(videoPaths[0]);
                labelVideoTitle.Content = System.IO.Path.GetFileNameWithoutExtension(videoPaths[VideoPathIndex]);
            }
            getSrtPath();
        }
        private void Timer2_Tick(object? sender, EventArgs e) //For animation transform
        {
            bool flag = false;
            if (btnPlayPause_Copy.Opacity > 0)
            {
                btnPlayPause_Copy.Opacity -= 0.1; flag = true;
            }
            if (Forward15seconds.Opacity > 0)
            {
                Forward15seconds.Opacity -= 0.1; flag = true;
            }
            if (Backward15seconds.Opacity > 0)
            {
                Backward15seconds.Opacity -= 0.1; flag = true;
            }
            if (!fullSizeIsOn)
            {
                if (media.Margin.Top < 40)
                {
                    media.Margin = new Thickness(0, media.Margin.Top + 4, 0, media.Margin.Bottom + 70 / 10);
                    flag = true;
                }

            }
            else
            {
                if (media.Margin.Top > 0)
                {
                    media.Margin = new Thickness(0, media.Margin.Top - 4, 0, media.Margin.Bottom - 70 / 10);
                    flag = true;
                }
            }
            if (flag == false)
            {
                timer2.Stop();
            }
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
            bool flag = false;
            if (VideoPlayerGrid.Height > 0)
            {
                VideoPlayerGrid.Height -= 5;
                flag = true;
            }
            if (labelVideoTitle.Height > 0)
            {
                StackPanel1.Height -= 5;
                labelVideoTitle.Height -= 5;
                flag = true;
            }
            if (mouseCaptured || !fullSizeIsOn)
            {
                labelVideoTitle.Height = 40;
                StackPanel1.Height = 40;
                VideoPlayerGrid.Height = 70;
                flag = false;
            }
            if (!flag)
            {
                timer1.Stop();
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            bool flag = true;
            try
            {
                if (!sliderSeekIsDown)
                    SliderSeek.Value = media.Position.TotalSeconds;//update the current video position to progress bar
                string a = TimeSpan.FromMinutes(SliderSeek.Value).ToString();//H.Nam: remove miliseconds from Timespan
                string b = TimeSpan.FromMinutes(SliderSeek.Maximum).ToString();
                Video_Duration.Content = a.Substring(0, a.LastIndexOf(':')) + " / " + b.Substring(0, b.LastIndexOf(':'));
                if (subtitlesIsOn)
                {
                    foreach (SrtContent testItem in test)
                    {
                        TimeSpan start = TimeSpan.Parse(testItem.StartTime.Substring(0, testItem.StartTime.IndexOf(',')));
                        TimeSpan end = TimeSpan.Parse(testItem.EndTime.Substring(0, testItem.StartTime.IndexOf(',')));

                        if (end > media.Position && start < media.Position)
                        {
                            Sub.Background.Opacity = 0.3;
                            Sub.Text = testItem.Text.ToString();
                            flag = false;
                        }
                    }
                    if (flag == true)
                    {
                        Sub.Background.Opacity = 0;
                        Sub.Text = "";
                    }
                }
                else
                {
                    Sub.Background.Opacity = 0;
                    Sub.Text = "";
                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        private void btnMore_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                ofd.Multiselect = true;
                ofd.Filter = "Video files |*.wmv; *.3g2; *.3gp; *.3gp2; *.3gpp; *.amv; *.asf;  *.avi; *.bin; *.cue; *.divx; *.dv; *.flv; *.gxf; *.iso; *.m1v; *.m2v; *.m2t; *.m2ts; *.m4v; " +
                 " *.mkv; *.mov; *.mp2; *.mp2v; *.mp4; *.mp4v; *.mpa; *.mpe; *.mpeg; *.mpeg1; *.mpeg2; *.mpeg4; *.mpg; *.mpv2; *.mts; *.nsv; *.nuv; *.ogg; *.ogm;" +
                 " *.ogv; *.ogx; *.ps; *.rec; *.rm; *.rmvb; *.tod; *.ts; *.tts; *.vob; *.vro; *.webm; *.dat; ";

                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                    videoPaths = ofd.FileNames.ToList();
                    VideoPathIndex = 0;
                    media.Source = new Uri(videoPaths[VideoPathIndex]);
                    labelVideoTitle.Content = System.IO.Path.GetFileNameWithoutExtension(videoPaths[VideoPathIndex]);
                    media.LoadedBehavior = MediaState.Play;
                    getSrtPath();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        private string getSRTLanguage(string videopath, string srtpath)
        {
            videopath = System.IO.Path.GetFileNameWithoutExtension(videopath);
            srtpath = System.IO.Path.GetFileNameWithoutExtension(srtpath);
            int start = srtpath.IndexOf(videopath);
            int end = start + videopath.Length;
            return srtpath.Substring(0, start) + srtpath.Substring(end).Trim(); ;
        }
        private void getSrtPath()
        {
            subtitlePaths.Clear();
            foreach (string videoPath in videoPaths)
            {
                string[] x = Directory.GetFiles(System.IO.Path.GetDirectoryName(videoPath), "*.srt", SearchOption.AllDirectories); ;
                foreach (string item in x)
                {
                    if (System.IO.Path.GetFileName(item).Contains(System.IO.Path.GetFileNameWithoutExtension(videoPath)))
                    {
                        subtitlePaths.Add(item);

                        // subtitlesCbb.Items.Add(System.IO.Path.GetFileName(item));
                        // subtitlesCbb.Visibility= Visibility.Visible;
                    }
                }
            }
        }
        private void btnPlayPause_Click(object sender, RoutedEventArgs e)
        {
            btnPlayPause_Copy.Opacity = 1;
            timer2.Start();
            if (media.LoadedBehavior != MediaState.Play)
            {

                Play_PauseIcon.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/pause_32.png"));
                Play_PauseIcon2.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/pause_32.png"));
                media.LoadedBehavior = MediaState.Play;
            }
            else
            {

                Play_PauseIcon.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/play_32.png"));
                Play_PauseIcon2.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/play_32.png"));
                media.LoadedBehavior = MediaState.Pause;
            }

        }


        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media.Volume = e.NewValue;//H.Nam: update volume
        }

        private void btnVolumn_Click(object sender, RoutedEventArgs e) //H.Nam: Mute funtion
        {
            if (VolumeSlider.IsEnabled == true)
            {
                VolumeSlider.IsEnabled = false;
                VolumeSlider.Opacity = 0.5;
                media.Volume = 0;
                VolumeIcon.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/no_sound_32.png"));
            }
            else
            {
                VolumeSlider.IsEnabled = true;
                VolumeSlider.Opacity = 1;
                media.Volume = VolumeSlider.Value;
                VolumeIcon.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/volume_32.png"));
            }
        }


        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {

            if (randomIsOn)
            {
                Random random = new Random();
                media.Source = new Uri(videoPaths[random.Next(0, videoPaths.Count())]);
            }
            if (repeatIsOn)
            {
                media.Position = TimeSpan.Zero;
            }
        }


        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            TimeSpan ts = media.NaturalDuration.TimeSpan;
            SliderSeek.Maximum = ts.TotalSeconds;

            // Nam: update most watched in home page
            GalaxyMediaPlayer.Databases.HomePage.HomePageDatabaseAccess.SaveDataOnWatchingVideo(videoPaths[VideoPathIndex]);

            timer.Start();
        }


        private void SliderSeek_GotMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            sliderSeekIsDown = true;
            media.Volume = 0;
        }

        private void SliderSeek_LostMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            media.Position = TimeSpan.FromSeconds(SliderSeek.Value);//H.Nam: Allow user to change the video position according to progress bar
            sliderSeekIsDown = false;
            media.Volume = VolumeSlider.Value;
        }

        private void VideoPlayerGrid_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            mouseCaptured = false;
            if (fullSizeIsOn)
            {
                wait(3000);
                timer1.Start();
            }
        }

        private void VideoPlayerGrid_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            mouseCaptured = true;
            labelVideoTitle.Height = 40;
            VideoPlayerGrid.Height = 70;
            StackPanel1.Height = 40;
        }
        private void btnSkip15Seconds_Click(object sender, RoutedEventArgs e)
        {
            Forward15seconds.Opacity = 1;
            timer.Stop();
            media.Position = TimeSpan.FromSeconds(SliderSeek.Value + 15);
            timer.Start();
            timer2.Start();
        }

        private void btnSkip15Seconds_Copy_Click(object sender, RoutedEventArgs e)
        {
            Backward15seconds.Opacity = 1;
            timer.Stop();
            media.Position = TimeSpan.FromSeconds(SliderSeek.Value - 15);
            timer.Start();
            timer2.Start();
        }

        private void Video_Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.D)
            {
                media.Position = TimeSpan.FromSeconds(SliderSeek.Value + 15);
            }
            if (e.Key == Key.A)
            {
                btnSkip15Seconds_Copy_Click(sender, e);
            }
        }


        private void btnNext_Click(object sender, RoutedEventArgs e)
        {

            if (VideoPathIndex < videoPaths.Count() - 1)
                media.Source = new Uri(videoPaths[++VideoPathIndex]);
            else
            {
                VideoPathIndex = 0;
                media.Source = new Uri(videoPaths[VideoPathIndex]);
            }
            labelVideoTitle.Content = System.IO.Path.GetFileNameWithoutExtension(videoPaths[VideoPathIndex]);
        }
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (VideoPathIndex > 0)
                media.Source = new Uri(videoPaths[--VideoPathIndex]);
            else
            {
                VideoPathIndex = videoPaths.Count();
                media.Source = new Uri(videoPaths[--VideoPathIndex]);
            }
            labelVideoTitle.Content = System.IO.Path.GetFileNameWithoutExtension(videoPaths[VideoPathIndex]);
        }
        private void btnRandom_Click(object sender, RoutedEventArgs e)
        {
            randomIsOn = !randomIsOn;
            if (randomIsOn)
            {
                btnRandom.Opacity = 1;
                if (repeatIsOn)
                {
                    btnRepeat_Click(sender, e);
                }
            }
            else
            {
                btnRandom.Opacity = 0.5;
            }

        }
        private void btnRepeat_Click(object sender, RoutedEventArgs e)//H.Nam: Repeat Function
        {
            if (repeatIsOn)
            {
                repeatIsOn = false;
                btnRepeat.Opacity = 0.5;
            }
            else
            {

                repeatIsOn = true;
                if (randomIsOn)
                {
                    btnRandom_Click(sender, e);
                }
                btnRepeat.Opacity = 1;
            }

        }

        private void btnFullScreen_Click(object sender, RoutedEventArgs e)
        {
            if (fullSizeIsOn)
                ToggleScreenSizeIcon.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/minimize_32.png"));

            else
            {
                ToggleScreenSizeIcon.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/full_screen_64.png"));
            }
            fullSizeIsOn = !fullSizeIsOn;
            timer2.Start();
        }

        private void btnSubtitles_Click_1(object sender, RoutedEventArgs e)
        {
            if (subtitlesIsOn)
            {
                btnSubtitles.Opacity = 0.5;
                subtitlesIsOn = false;
                subtitlesCbb.Visibility = Visibility.Collapsed;
            }
            else
            {
                bool flag = false;
                subtitlesCbb.Items.Clear();
                foreach (string subPath in subtitlePaths)
                {
                    if (System.IO.Path.GetFileName(subPath).Contains(System.IO.Path.GetFileNameWithoutExtension(videoPaths[VideoPathIndex])))
                    {
                        if(!flag)
                        test = ParseSRT(subPath);
                        subtitlesIsOn = true;
                        btnSubtitles.Opacity = 1;
                        subtitlesCbb.Items.Add(getSRTLanguage(videoPaths[VideoPathIndex], subPath));
                        flag = true;

                    }
                }
                if (flag)
                {
                    subtitlesCbb.Text = subtitlesCbb.Items[0].ToString();
                    subtitlesCbb.Visibility = Visibility.Visible;
                }
            }

        }
        public void wait(int milliseconds)
        {
            var timer1 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;

            // Console.WriteLine("start wait timer");
            timer1.Interval = milliseconds;
            timer1.Enabled = true;
            timer1.Start();

            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
                // Console.WriteLine("stop wait timer");
            };

            while (timer1.Enabled)
            {
                System.Windows.Forms.Application.DoEvents();
            }
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
            MainWindow.Instance.MainFrame.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.Relative));
        }

        private void media_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btnPlayPause_Click(sender, e);
        }

        private void _SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            foreach (string subPath in subtitlePaths)
            {

                if (System.IO.Path.GetFileName(subPath).Contains(System.IO.Path.GetFileNameWithoutExtension(videoPaths[VideoPathIndex])))
                {

                    if (subtitlesCbb.Items[subtitlesCbb.SelectedIndex].ToString() == getSRTLanguage(videoPaths[VideoPathIndex], subPath))
                    {
                        test = ParseSRT(subPath); break;
                    }
                }
            }
        }

    }
}