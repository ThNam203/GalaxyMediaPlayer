using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GalaxyMediaPlayer.Converters
{
    internal class EntityIsSelectedToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isSelected = (bool)value;
            if (isSelected) return Visibility.Visible;
            else return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility vis = (Visibility)value;
            if (vis == Visibility.Visible) return true;
            else return false;
        }
    }
}
