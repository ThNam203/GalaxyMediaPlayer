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
    public class ImagePathToImageSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string model = (string)value;
            if (model != null)
            {

                // for images media files
                Image image = Image.FromFile(model);

                int scaleHeight = 0;
                int scaleWidth = 0;
                int width = image.Width;
                int height = image.Height;

                // Nam: ImageControl have 100 x 100 size
                if (width == height) scaleHeight = scaleWidth = 100;
                else if (height > width)
                {
                    scaleHeight = 100;
                    scaleWidth = (int)Math.Round(((double)100 / height) * width);
                }
                else if (height < width)
                {
                    scaleWidth = 100;
                    scaleHeight = (int)Math.Round(((double)100 / width) * height);
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
            return new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/ComputerPageIcons/ic_folder_128.png"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
