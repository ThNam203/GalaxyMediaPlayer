using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GalaxyMediaPlayer.Converters.HomePageConverters
{
    internal class VideoPathToImageSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = (string)value;

            try
            {
                ShellFile shellFile = ShellFile.FromFilePath(path);
                Bitmap bm = shellFile.Thumbnail.Bitmap;

                MemoryStream ms = new MemoryStream();
                bm.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                ms.Seek(0, SeekOrigin.Begin);
                image.StreamSource = ms;
                image.EndInit();
                image.Freeze();
                return image;
            }
            catch (Exception)
            {
                return new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/ComputerPageIcons/ic_film.png"));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
