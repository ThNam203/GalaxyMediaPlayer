using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GalaxyMediaPlayer.Converters
{
    internal class SongPathToImageSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = (string)value;

            TagLib.File file = TagLib.File.Create(path);
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
                return bitmapImage;
            }
            catch (IndexOutOfRangeException)
            {
                return new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/PlaylistPageIcons/user.png"));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
