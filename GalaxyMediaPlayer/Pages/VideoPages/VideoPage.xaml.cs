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
using GalaxyMediaPlayer.Pages.VideoPages;
using ExifLib;
using System.Diagnostics;

//using System.Windows.Forms;
namespace GalaxyMediaPlayer.Pages
{
    /// <summary>
    /// Interaction logic for VideoPage.xaml
    /// </summary>
    public partial class VideoPage : Page
    {
        VideoPaths videoPaths;
        public VideoPage()
        {
            InitializeComponent();
            if (isVideoListEmplty())//H.Nam if the database is not exists, create new one
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlElement root = xmlDocument.CreateElement("root");
                xmlDocument.AppendChild(root);
                xmlDocument.Save(AppDomain.CurrentDomain.BaseDirectory + "Databases\\VideoPage\\VideoPath.xml");
            }
           videoPaths = new VideoPaths();
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = ("Video Files|*.mp4");
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                videoPaths.AddPath(openFileDialog.FileName);
                addPanel.Visibility=Visibility.Hidden;
                foreach(string str in videoPaths.GetAllPaths())
                {
                   System.Windows.Forms.MessageBox.Show(str);//media.Source = new Uri(str);
                }
                //CC.Content = new VideoPlayList("df");
                //video.AddPath(openFileDialog.FileName);

            }
            else
            {
                addPanel.Visibility = Visibility.Visible;
                //System.Windows.Forms.MessageBox.Show("");
            }
        }

        private bool isVideoListEmplty() {
            return (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Databases\\VideoPage\\VideoPath.xml"));
        }

     
    }
}
