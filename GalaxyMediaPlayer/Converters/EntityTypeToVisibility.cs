using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GalaxyMediaPlayer.Converters
{
    internal class EntityTypeToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            EntityType entityType = (EntityType)value;
            if (entityType == EntityType.Folder || entityType == EntityType.Image) return Visibility.Hidden;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
