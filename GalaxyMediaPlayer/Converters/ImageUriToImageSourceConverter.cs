using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GalaxyMediaPlayer.Converters
{
    public class ImageUriToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? imageUri = value as string;

            if (imageUri == null)
                return new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/NavIcons/ic_home.png"));

            return new BitmapImage(new Uri($"pack://application:,,,/Resources/Icons/NavIcons/{imageUri}"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? str = value as string;

            if (str == null) return "blank_square.png";
            int lastIndex = str.LastIndexOf("/");
            return str.Substring(lastIndex, str.Length);
        }
    }
}