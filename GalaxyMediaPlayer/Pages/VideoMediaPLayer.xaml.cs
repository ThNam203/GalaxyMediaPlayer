using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TagLib.Id3v2;
using Wpf.Ui.Controls;
using System.Windows.Threading;
using System.Drawing;
using System.IO;
using Microsoft.VisualBasic.ApplicationServices;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

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

                    }); ;
                    //H.nam The block numbers of SRT like 1, 2, 3, ... and so on
                    segment++;
                    //H.Nam Iterate one block at a time
                    item += 3;
                }
            }
            return content;
        }
        List<SrtContent> test = new List<SrtContent>();

        bool repeatIsOn =false;
        DispatcherTimer timer;
        string subtiles;
        public VideoMediaPLayer()
        {
            InitializeComponent();
            timer = new DispatcherTimer(); //H.Nam: DispatcherTimer for displaying video current position
            timer.Interval= TimeSpan.FromMilliseconds(200); 
            timer.Tick += new EventHandler(Timer_Tick);
        }
        private void Timer_Tick(object sender,EventArgs e)
        {
            bool flag=true;
            try
            {
                SliderSeek.Value = media.Position.TotalSeconds;//update the current video position to progress bar
                string a=TimeSpan.FromMinutes(SliderSeek.Value).ToString();//H.Nam: remove miliseconds from Timespan
                string b=TimeSpan.FromMinutes(SliderSeek.Maximum).ToString();    
                Video_Duration.Content = a.Substring(0, a.LastIndexOf(':'))+" / "+  b.Substring(0,b.LastIndexOf(':')); 
                    foreach(SrtContent testItem in test)
                    {
                        TimeSpan start = TimeSpan.Parse(testItem.StartTime.Substring(0,testItem.StartTime.IndexOf(',')));
                         TimeSpan end = TimeSpan.Parse(testItem.EndTime.Substring(0, testItem.StartTime.IndexOf(',')));

                    if (end >media.Position && start<media.Position)
                    {
                        Sub.Background.Opacity = 0.3;
                        Sub.Text = testItem.Text.ToString();
                        flag = false;
                    }
                }
                if (flag==true)
                {
                    Sub.Background.Opacity=0;
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

                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    media.Source = new Uri(ofd.FileName);
                    media.LoadedBehavior = MediaState.Play;
                    Uri uri = new Uri(ofd.FileName);
                    DirectoryInfo directory = new DirectoryInfo(ofd.FileName);
                    string[] x = Directory.GetFiles(@"C:\Users\GIGA\Videos", "*.srt",SearchOption.AllDirectories);;
                    foreach (string item in x)
                    {
                        if (ofd.FileName.Contains(System.IO.Path.GetFileName(ofd.FileName)))
                        {
                            test = ParseSRT(item);
                        }
                    }


                }
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void btnPlayPause_Click(object sender, RoutedEventArgs e)
        {
         
            if (media.LoadedBehavior != MediaState.Play)
            {   

                media.LoadedBehavior= MediaState.Play;
                Play_PauseIcon.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/pause_32.png"));
                btnPlayPause.Opacity = 1;

            }
            else
            {
                media.LoadedBehavior= MediaState.Pause;
                Play_PauseIcon.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/MediaControlIcons/play_32.png"));

            }
        }


        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media.Volume = e.NewValue;//H.Nam: update volume
        }

        private void btnVolumn_Click(object sender, RoutedEventArgs e) //H.Nam: Mute funtion
        {
            if(VolumeSlider.IsEnabled == true)
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

        private void btnRepeat_Click(object sender, RoutedEventArgs e)//H.Nam: Repear Function
        {
            if (repeatIsOn)
            {
                repeatIsOn = false;
                btnRepeat.Opacity = 0.5;
            }
            else
            {
                repeatIsOn = true;
                btnRepeat.Opacity = 1;
            }
         
        }

        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (repeatIsOn)
            {
                media.Position = TimeSpan.Zero;
            }
        }


        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            TimeSpan ts = media.NaturalDuration.TimeSpan;
            SliderSeek.Maximum = ts.TotalSeconds;
            timer.Start();
        }


        private void SliderSeek_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            while (false)
            {
                media.LoadedBehavior = MediaState.Pause;
            }
            media.LoadedBehavior = MediaState.Play;
        }

        private void SliderSeek_GotMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
                timer.Stop();
        }

        private void SliderSeek_LostMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            media.Position = TimeSpan.FromSeconds(SliderSeek.Value);//H.Nam: Allow user to change the video position according to progress bar
            timer.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // System.Windows.Forms.MessageBox.Show(this.ToString());
            try
            {
                GalaxyMediaPlayer.MainWindow.GetWindow(this).Close();
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }

        }

        private void VideoPlayerGrid_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            control_panel.Visibility = Visibility.Hidden;
            VolumeControlPanel.Visibility = Visibility.Hidden;
            VideoPlayerGrid.Background.Opacity = 0;
        }

        private void VideoPlayerGrid_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            control_panel.Visibility= Visibility.Visible;
            VolumeControlPanel.Visibility = Visibility.Visible;
            VideoPlayerGrid.Background.Opacity = 0.12;

        }

 
   

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GalaxyMediaPlayer.MainWindow.GetWindow(this).WindowState = System.Windows.WindowState.Maximized;
        }

        private void Minimize_Button_Click(object sender, RoutedEventArgs e)
        {
            GalaxyMediaPlayer.MainWindow.GetWindow(this).WindowState = System.Windows.WindowState.Normal;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            GalaxyMediaPlayer.MainWindow.GetWindow(this).WindowState = System.Windows.WindowState.Minimized;

        }


    }
}
