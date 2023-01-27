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
using Wpf.Ui.Controls;
using GalaxyMediaPlayer.Databases.VideoPage;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using ExifLib;
using System.Diagnostics;
using Wpf.Ui.Interop.WinDef;
using Xabe.FFmpeg;
using System.Drawing;
using MediaToolkit.Model;
using MediaToolkit.Options;
using MediaToolkit;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using GalaxyMediaPlayer.Models;

//using System.Windows.Forms;
namespace GalaxyMediaPlayer.Pages
{
    /// <summary>
    /// Interaction logic for VideoPage.xaml
    /// </summary>
    public partial class VideoPage : Page
    {
        VideoPaths videoPaths;
        ImageList imageList;
        ObservableCollection<VideoDisplay> source;
        public VideoPage()
        {

            InitializeComponent();
            DataBaseInit();
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
                xmlDocument.Save(AppDomain.CurrentDomain.BaseDirectory + "Databases\\VideoPage\\VideoPath.xml");
            }
            videoPaths = new VideoPaths();
            if (!videoPaths.IsEmpty())
            {
                ChangeBtnVisibility();
            }
            source = new ObservableCollection<VideoDisplay>();
             source = videoPaths.GetAllPathsObs();
            VideoListView.ItemsSource =source ;
           // videoPaths.DeletePath(@"C:\Users\GIGA\Videos\How To Create an SRT File - Detailed Subtitling Tutorial.mp4");
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            bool CreateList =false;
            if (videoPaths.IsEmpty()) { CreateList = true; }
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = ("Video Files|*.mp4");
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                List<string> paths = videoPaths.GetAllPaths();
                foreach( string filename in openFileDialog.FileNames)
                {
                    
                    bool flag = true;
                    foreach( string path in paths)
                    {
                        
                        if (path.Contains(filename))
                        {
                            flag = false;
                        }
                    }
                    if (flag) {
                
                     videoPaths.AddPath(filename);
                     VideoDisplay videoDisplay = new VideoDisplay(filename);
                        source.Add(videoDisplay);
                    }

                }
                if(CreateList)
                ChangeBtnVisibility();                  
            }
            else
            {
                if(videoPaths.IsEmpty())
                ChangeBtnVisibility();
            }
        }

        private bool IsDataBaseExists()
        { 
            return (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Databases\\VideoPage\\VideoPath.xml"));
        }
        private void test_Click(object sender, RoutedEventArgs e)
        {
            string pathToVideoFile = @"C:\Users\GIGA\Videos\Ben Tren Tang Lau - Uyen Linh.mp4";
            using (var engine = new Engine())
            {
                var mp4 = new MediaFile { Filename = pathToVideoFile };
                engine.GetMetadata(mp4);
                var options = new ConversionOptions { Seek = TimeSpan.FromSeconds(1) };
                var outputFile = new MediaFile { Filename =  @"C:\Users\GIGA\Videos\123.jpeg" };
                engine.GetThumbnail(mp4, outputFile, options);
            }
        }
        private void ChangeBtnVisibility()
        { 
            var x = addPanel.Visibility;
            addPanel.Visibility = Bar.Visibility;
            Bar.Visibility = x;
            VideoListView.Visibility = x;
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            bool flag = false;
            foreach (VideoDisplay video in VideoListView.SelectedItems)
            {
                source[VideoListView.Items.IndexOf(video)].isSelected = true;
            }
            foreach (VideoDisplay video in source.ToList())
            {
                if (video.isSelected)
                {
                    source.Remove(video);
                  videoPaths.DeletePath(video.pathToVideo);
                    flag = true;
                }
            }
            if (flag )
            {
                if(videoPaths.IsEmpty())
                    ChangeBtnVisibility();
            }
        }
        private void Open_Video(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                MainWindow.Instance.MainFrame.Navigate( new VideoMediaPLayer(SelectedVideo()));
            }
        }
        private List<string> SelectedVideo()
        {
            List<string> result = new List<string>();
            foreach (VideoDisplay video in VideoListView.SelectedItems)
            {
                result.Add(video.pathToVideo);
            }
            return result;
        }
        private void Open_Video(object sender, RoutedEventArgs e)
        {

            if(VideoListView.SelectedItems.Count > 0)
            MainWindow.Instance.MainFrame.Navigate(new VideoMediaPLayer(SelectedVideo()));

        }
    }
}
