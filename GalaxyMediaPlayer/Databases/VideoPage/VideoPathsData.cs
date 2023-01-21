using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.CompilerServices;
using System.Windows;

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
    }
}
