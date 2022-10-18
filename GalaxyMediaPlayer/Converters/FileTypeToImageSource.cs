using System;
using System.Globalization;
using System.Drawing;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GalaxyMediaPlayer.Converters
{
    public class FileTypeToImageSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? rawSource = value as string;
            if (rawSource != null)
            {
                if (rawSource == "folder.folder") return new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/ComputerPageIcons/ic_folder_128.png"));
                else {
                    string extension = Path.GetExtension(rawSource);
                    extension = extension.TrimStart('.');
                    if (extension == "mp3")
                    {
                        TagLib.File file = TagLib.File.Create(rawSource);
                        try
                        {
                            var pic = file.Tag.Pictures[0];
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

                    // for images media files
                    var image = Image.FromFile(rawSource);
                    var thumb = image.GetThumbnailImage(100, 100, () => false, IntPtr.Zero);
                    var bitmap = new Bitmap(thumb);

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
            }

            return new BitmapImage(new Uri("pack://application:,,,/Resources/ic_folder_128.png"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
