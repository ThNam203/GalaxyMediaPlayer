﻿using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GalaxyMediaPlayer.Converters.HomePageConverters
{
    internal class PathToImageSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = (string)value;
            string extension = Path.GetExtension(path);
            
            if (Helpers.SupportedExtensions.IMAGE_EXTENSION.Contains(extension))
            {
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
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/PlaylistPageIcons/album.png"));
                }
            } 
            else
            {
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
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
