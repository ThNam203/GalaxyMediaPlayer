using GalaxyMediaPlayer.Databases.VideoPage;
using GalaxyMediaPlayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Xml;

namespace GalaxyMediaPlayer.Pages.PlaylistPagePages
{
    /// <summary>
    /// Interaction logic for VideoPlaylistPage.xaml
    /// </summary>
    public partial class VideoPlaylistPage : Page
    {
        public static ObservableCollection<VideoDisplay> source;
        VideoPaths videoPaths = new VideoPaths();
        public VideoPlaylistPage()
        {
            InitializeComponent();
            DataBaseInit();
            playlistVideosDataGrid.ItemsSource=source;
            playlistListBox.ItemsSource = source;
        }
        private void DataBaseInit()
        {
            if (!IsDataBaseExists())//H.Nam if the database is not exists, create new one
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlElement root = xmlDocument.CreateElement("root");
                xmlDocument.AppendChild(root);
                string path = AppDomain.CurrentDomain.BaseDirectory + "Databases\\VideoPage";
                if (!File.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                xmlDocument.Save(AppDomain.CurrentDomain.BaseDirectory + "Databases\\VideoPage\\VideoPlayListPath.xml");
            }
            videoPaths = new VideoPaths();
            source = new ObservableCollection<VideoDisplay>();
            source = videoPaths.GetAllPathsObs();
           
        }
        public bool IsDataBaseExists()
        {
            return (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Databases\\VideoPage\\VideoPlayListPath.xml"));
        }
        private void DeleteCheckBoxClick(object sender, RoutedEventArgs e)
        {
          
        }

        private void secondaryNewPlaylistBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void listBoxItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
        private void listBoxItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
