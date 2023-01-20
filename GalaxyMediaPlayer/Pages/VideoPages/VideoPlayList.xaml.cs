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
using System.Xml;

namespace GalaxyMediaPlayer.Pages.VideoPages
{
    /// <summary>
    /// Interaction logic for VideoPlayList.xaml
    /// </summary>
    public partial class VideoPlayList : Page
    {
        string temp;
        public VideoPlayList(string s)
        {

            InitializeComponent();
            temp = s;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "Databases\\VideoPage\\VideoPath.xml");
            Button a = sender as Button;
            foreach(var item in xmlDocument)
            {
                
            }
        }
    }
}
