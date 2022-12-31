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

namespace GalaxyMediaPlayer.Pages
{
    /// <summary>
    /// Interaction logic for VideoMediaPLayer.xaml
    /// </summary>
    public partial class VideoMediaPLayer : Page
    {
        bool repeatIsOn=false;
        DispatcherTimer timer;
        public VideoMediaPLayer()
        {
            InitializeComponent();
            timer = new DispatcherTimer(); //H.Nam: DispatcherTimer for displaying video current position
            timer.Interval= TimeSpan.FromMilliseconds(500); 
            timer.Tick += new EventHandler(Timer_Tick);
        }
        private void Timer_Tick(object sender,EventArgs e)
        {
            try
            {
                
                SliderSeek.Value = media.Position.TotalSeconds;//update the current video position to progress bar
                string a=TimeSpan.FromMinutes(SliderSeek.Value).ToString();//H.Nam: remove miliseconds from Timespan
                string b=TimeSpan.FromMinutes(SliderSeek.Maximum).ToString();    
                Video_Duration.Content = a.Substring(0, a.LastIndexOf(':'))+" / "+  b.Substring(0,b.LastIndexOf(':')); 
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
                    media.LoadedBehavior=MediaState.Play;
                    var text = new Uri(@"C:\Users\GIGA\Videos\[English] How To Create an SRT File - Detailed Subtitling Tutorial [DownSub.com].srt");
                    
                }
            }catch(Exception ex)
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
