using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Collections.ObjectModel;
using MediaToolkit.Model;
using MediaToolkit.Options;
using MediaToolkit;
using System.IO;
using System.DirectoryServices.ActiveDirectory;

namespace GalaxyMediaPlayer.Databases.VideoPage
{

    public class VideoPaths
    {
        static string fileLocation = AppDomain.CurrentDomain.BaseDirectory + "Databases\\VideoPage\\VideoPath.xml";
        public XmlElement root;

        XmlDocument xmlDocument = new XmlDocument();

        public VideoPaths()
        {
            xmlDocument.Load(fileLocation);
            root = xmlDocument.DocumentElement;
            CreateThumbnailFolderIfNotExist();
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
                }
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

        public VideoDisplay(string path)
        {
            using (var engine = new Engine())
            {
                title = System.IO.Path.GetFileNameWithoutExtension(path);
                pathToImg = AppDomain.CurrentDomain.BaseDirectory + "Databases\\VideoThumbNail\\" + title +".jpeg";
                var mp4 = new MediaFile { Filename = path };
                engine.GetMetadata(mp4);
                var options = new ConversionOptions { Seek = TimeSpan.FromSeconds(1) };
                var outputFile = new MediaFile { Filename = pathToImg};
                
                engine.GetThumbnail(mp4, outputFile, options);
            }

        }

    }

}
