using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Collections.ObjectModel;
using MediaToolkit.Model;
using MediaToolkit.Options;
using MediaToolkit;
using System.IO;
using System.DirectoryServices.ActiveDirectory;
using static ScrapySharp.Core.Token;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GalaxyMediaPlayer.Databases.VideoPage
{

    public class VideoPaths
    {
        string fileLocation;
        public string playlistThumbnail { get; set; }
        public string playlistName { get; set; }
        public XmlElement root;

        XmlDocument xmlDocument = new XmlDocument();

        public VideoPaths(string fileLocation)
        {
            this.fileLocation = fileLocation.Trim();
            xmlDocument.Load(this.fileLocation);
            root = xmlDocument.DocumentElement;
            CreateThumbnailFolderIfNotExist();
            playlistName = System.IO.Path.GetFileNameWithoutExtension(fileLocation);
            playlistThumbnail = "pack://application:,,,/Resources/Icons/PlaylistPageIcons/film.png";
            if (root.ChildNodes.Count >= 1)
            {
                VideoDisplay display = new VideoDisplay(root.FirstChild.InnerText);
                playlistThumbnail = display.pathToImg;
            }
        }

        public void AddPath(string path)
        {
            XmlElement xml = xmlDocument.CreateElement("link");
            xml.InnerText = path;
            root.AppendChild(xml);
            xmlDocument.Save(fileLocation);
        }
        public void DeletePath(string path)
        {
            XmlNodeList xmlNodeList = root.ChildNodes;
            foreach (XmlNode item in xmlNodeList)
            {
                if (item.InnerText == path)
                {
                    root.RemoveChild(item);
                    xmlDocument.Save(fileLocation);
                }
            }
            if (root.ChildNodes.Count == 0)
            {
                playlistThumbnail = "pack://application:,,,/Resources/Icons/PlaylistPageIcons/film.png";

            }
        }
        public bool IsEmpty()
        {
            return root.ChildNodes.Count == 0;
        }
        public int Count()
        {
            return root.ChildNodes.Count;
        }
        public List<string> GetAllPaths()
        {
            List<string> list = new List<string>();
            XmlNodeList xmlNodeList = root.ChildNodes;
            foreach (XmlNode item in xmlNodeList)
            {
                list.Add(item.InnerText);
            }
            return list.Distinct().ToList();
        }
        public ObservableCollection<VideoPaths> GetAllPlaylistPaths()
        {
            ObservableCollection<VideoPaths> list = new ObservableCollection<VideoPaths>();
            string[] oDirectories = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Databases\\VideoPlayListPage", "*.xml", SearchOption.AllDirectories);
            foreach (string file in oDirectories)
            {
                VideoPaths video = new VideoPaths(file);
                list.Add(video);
            }
            return list;
        }
        public ObservableCollection<VideoDisplay> GetAllPathsObs()
        {
            ObservableCollection<VideoDisplay> list = new ObservableCollection<VideoDisplay>();
            XmlNodeList xmlNodeList = root.ChildNodes;
            foreach (XmlNode item in xmlNodeList)
            {
                VideoDisplay videoDisplay = new VideoDisplay(item.InnerText);
                list.Add(videoDisplay);
            }
            return list;
        }
        private static void CreateThumbnailFolderIfNotExist()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Databases\\VideoThumbNail";
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }


    }
    public class VideoDisplay
    {
        public string pathToImg { get; set; }
        public string title { get; set; }
        public string pathToVideo { get; set; }
        public bool isSelected { get; set; }
        public VideoDisplay(string path)
        {
            pathToVideo = path;
            using (var engine = new Engine())
            {
                title = System.IO.Path.GetFileNameWithoutExtension(path);
                pathToImg = AppDomain.CurrentDomain.BaseDirectory + "Databases\\VideoThumbNail\\" + title + ".jpeg";
                var mp4 = new MediaFile { Filename = path };
                engine.GetMetadata(mp4);
                var options = new ConversionOptions { Seek = TimeSpan.FromSeconds(1) };
                var outputFile = new MediaFile { Filename = pathToImg };
                engine.GetThumbnail(mp4, outputFile, options);
            }

        }

    }

}
