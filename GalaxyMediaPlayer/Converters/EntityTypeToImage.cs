using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GalaxyMediaPlayer.Converters
{
    internal class EntityTypeToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            EntityType entityType = (EntityType)value;
            if (entityType == EntityType.Folder) return new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/ComputerPageIcons/folder_16.png"));
            else if (entityType == EntityType.Music) return new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/ComputerPageIcons/music_16.png"));
            else if (entityType == EntityType.Image) return new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/ComputerPageIcons/photo_16.png"));
            else if (entityType == EntityType.Video) return new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/ComputerPageIcons/video_16.png"));

            return new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/ComputerPageIcons/unknown_file_16.png"));

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
