using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace GalaxyMediaPlayer.Pages
{
    /// <summary>
    /// Interaction logic for VideoPlayerPage.xaml
    /// </summary>
    public partial class VideoPlayerPage : Page
    {
        public VideoPlayerPage()
        {
            InitializeComponent();

        }
        //play button - H.Nam
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            MediaState ms = MediaState.Play;
            video_play.LoadedBehavior = ms;
        }

        //stop button - H.Nam
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MediaState uc = MediaState.Pause;
            video_play.LoadedBehavior = uc;
        }
        //previous song - H.Nam

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
        //next song - H.Nam
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }
        //browse button - H.Nam
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog fd = new OpenFileDialog();
                //set the filter - H.Nam
                fd.Filter = "MP3# Files (*.mp3)|*.mp3|MP4# Files (*.mp4)|*.mp4";
                //set the initial directory - H.Nam
                fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                //show the dialog - H.Nam
                fd.ShowDialog();
                //store the currently video path to variable string - H.Nam
                string filename = fd.FileName;
                if (filename != "")
                {
                    Uri uri = new Uri(filename);
                    //set video url to media element
                    video_play.Source = uri;
                    //start playing the selected video 
                    MediaState opt = MediaState.Play;
                    video_play.LoadedBehavior = opt;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("No seletion", string.Empty);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error: " + ex.Message);
            }
        }
        string videoURL = "\"C:\\Users\\GIGA\\Downloads\\Video\\TheFatRat - Monody (feat. Laura Brehm).mp4\"";
        private void windows_loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Uri obj = new Uri(videoURL);
                video_play.Source = obj;
                MediaState opt = MediaState.Play;
                video_play.LoadedBehavior = opt;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("error: " + ex.Message);
            }
        }
    }
}
