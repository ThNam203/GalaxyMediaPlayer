using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using GalaxyMediaPlayer.Databases.VideoPage;
using System.Xml;
using System.IO;
using MediaToolkit.Model;
using MediaToolkit.Options;
using MediaToolkit;
using System.Collections.ObjectModel;
using System.DirectoryServices.ActiveDirectory;
using GalaxyMediaPlayer.Databases.MusicPage;

//using System.Windows.Forms;
namespace GalaxyMediaPlayer.Pages
{
    /// <summary>
    /// Interaction logic for VideoPage.xaml
    /// </summary>
    public partial class VideoPage : Page
    {
        VideoPaths videoPaths;
        ObservableCollection<VideoDisplay> source;
        public VideoPage()
        {
            InitializeComponent();
            DataBaseInit();
        }
        string videoExtension = "Video files |*.wmv; *.3g2; *.3gp; *.3gp2; *.3gpp; *.amv; *.asf;  *.avi; *.bin; *.cue; *.divx; *.dv; *.flv; *.gxf; *.iso; *.m1v; *.m2v; *.m2t; *.m2ts; *.m4v; " +
                 " *.mkv; *.mov; *.mp2; *.mp2v; *.mp4; *.mp4v; *.mpa; *.mpe; *.mpeg; *.mpeg1; *.mpeg2; *.mpeg4; *.mpg; *.mpv2; *.mts; *.nsv; *.nuv; *.ogg; *.ogm;" +
                 " *.ogv; *.ogx; *.ps; *.rec; *.rm; *.rmvb; *.tod; *.ts; *.tts; *.vob; *.vro; *.webm; *.dat; ";
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
            videoPaths = new VideoPaths(AppDomain.CurrentDomain.BaseDirectory + "Databases\\VideoPage\\VideoPath.xml");

            source = new ObservableCollection<VideoDisplay>();
             source = videoPaths.GetAllPathsObs();
            VideoListView.ItemsSource =source ;               
           // AddVideoInFolder();         
            if (source.Count>0)
            {
                ChangeBtnVisibility();
            }
        }
        private void AddVideoInFolder()
        {
            foreach (var folder in VideoPageDatabaseAccess.GetAllData())
            {
                string[] files = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    string extension = Path.GetExtension(file);
                    if(!videoExtension.Contains(extension))
                        continue;

                    List<string> check = videoPaths.GetAllPaths();
                    bool flag= true;
                    foreach (string path in check)
                    {
                        if(path==file)
                            flag= false;
                    }
                    if(flag)
                       source.Add(new VideoDisplay(file));
                }
            }
        }
        private void Add(object sender, RoutedEventArgs e)
        {
            bool CreateList =false;
            if (videoPaths.IsEmpty()) { CreateList = true; }
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = videoExtension;
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
        private void ChangeBtnVisibility()
        { 
            var x = addPanel.Visibility;
            addPanel.Visibility = Bar.Visibility;
            Bar.Visibility = x;
            VideoListView.Visibility = x;
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            videoPaths.GetAllPlaylistPaths();
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

        private void addFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                    VideoPageDatabaseAccess.SaveFolderToDatabase(dialog.SelectedPath);
                string[] files = Directory.GetFiles(dialog.SelectedPath, "*.*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    string extension = Path.GetExtension(file);
                    if (!videoExtension.Contains(extension))
                        continue;

                    List<string> check = videoPaths.GetAllPaths();
                    bool flag = true;
                    foreach (string path in check)
                    {
                        if (path == file)
                            flag = false;
                    }
                    if (flag)
                    {
                        source.Add(new VideoDisplay(file));
                        videoPaths.AddPath(file);
                    }
                }
            }
        }
    }
}
