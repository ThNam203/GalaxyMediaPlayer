using System;
using System.Globalization;
using System.Drawing;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Microsoft.WindowsAPICodePack.Shell;
using System.Drawing.Imaging;
namespace GalaxyMediaPlayer.Converters
{
    public class ImagePathToThumbnailImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string imagePath = (string)value;
            if (imagePath != null && imagePath!="")
            {
                // for images media files
                Image image = Image.FromFile(imagePath);

                int scaleHeight = 0;
                int scaleWidth = 0;
                int width = image.Width;
                int height = image.Height;

                // Nam: ImageControl have 200 x 200 size
                if (width == height) scaleHeight = scaleWidth = 200;
                else if (height > width)
                {
                    scaleHeight = 200;
                    scaleWidth = (int)Math.Round(((double)200 / height) * width);
                }
                else if (height < width)
                {
                    scaleWidth = 200;
                    scaleHeight = (int)Math.Round(((double)200 / width) * height);
                }

                Image thumb = image.GetThumbnailImage(scaleWidth, scaleHeight, () => false, IntPtr.Zero);

                using (MemoryStream memory = new MemoryStream())
                {
                    thumb.Save(memory, ImageFormat.Png);
                    memory.Position = 0;
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memory;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                    return bitmapImage;
                }
            }
            return new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/ComputerPageIcons/unknown_fle_16.png"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
