using System.Drawing.Imaging;
using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GalaxyMediaPlayer.UserControls.HomePageControls
{
    /// <summary>
    /// Interaction logic for MostWatchMusic.xaml
    /// </summary>
    public partial class MostWatchMusic : UserControl
    {
        public MostWatchMusic(string musicPath)
        {
            InitializeComponent();

            TagLib.File file = TagLib.File.Create(musicPath);
            try
            {
                TagLib.IPicture pic = file.Tag.Pictures[0];
                MemoryStream ms = new MemoryStream(pic.Data.Data);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                image.Source = bitmapImage;
            }
            catch (Exception)
            {
                image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/ComputerPageIcons/ic_audio_file.png"));
            }

            title.Text = Path.GetFileNameWithoutExtension(musicPath);
        }
    }
}
