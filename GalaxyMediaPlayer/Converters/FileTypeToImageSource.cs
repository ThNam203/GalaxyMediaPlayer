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
    public class FileTypeToImageSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SystemEntityModel model = (SystemEntityModel)value;
            if (model != null)
            {
                if (model.entityType == EntityType.Folder) 
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/ComputerPageIcons/ic_folder_128.png"));
                else {
                    if (model.entityType == EntityType.Music)
                    {
                        TagLib.File file = TagLib.File.Create(model.entityPath);
                        try
                        {
                            TagLib.IPicture pic = file.Tag.Pictures[0];
                            MemoryStream ms = new MemoryStream(pic.Data.Data);
                            BitmapImage bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.StreamSource = ms;
                            bitmapImage.EndInit();
                            return bitmapImage;
                        }
                        catch (IndexOutOfRangeException)
                        {
                            return new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/ComputerPageIcons/ic_audio_file.png"));
                        }
                    } 
                    else if (model.entityType == EntityType.Image)
                    {
                        // for images media files
                        Image image = Image.FromFile(model.entityPath);
                        Image thumb = image.GetThumbnailImage(100, 100, () => false, IntPtr.Zero);
                        Bitmap bitmap = new Bitmap(thumb);

                        using (MemoryStream memory = new MemoryStream())
                        {
                            image.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                            memory.Position = 0;
                            BitmapImage bitmapimage = new BitmapImage();
                            bitmapimage.BeginInit();
                            bitmapimage.StreamSource = memory;
                            bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapimage.EndInit();

                            return bitmapimage;
                        }
                    }
                    else if (model.entityType == EntityType.Video)
                    {
                        // for video files
                        ShellFile shellFile = ShellFile.FromFilePath(model.entityPath);
                        Bitmap bm = shellFile.Thumbnail.Bitmap;

                        using (MemoryStream memory = new MemoryStream())
                        {
                            bm.Save(memory, ImageFormat.Png);
                            memory.Position = 0;
                            BitmapImage bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = memory;
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.EndInit();
                            return bitmapImage;
                        }
                    }
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
