using GalaxyMediaPlayer.Helpers;
using MuxicMatchApi;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace GalaxyMediaPlayer.Models
{
    public class MusicModel
    {
        public const int DEFAULT_TITLE_TEXTBLOCK_WIDTH = 500;

        private string _songTitle;
        public string SongTitle 
        { 
            get { return _songTitle; }
            set { _songTitle = value; }
        }

        private string _songLyrics;
        public string? SongLyrics
        {
            get { return _songLyrics; }
            set { _songLyrics = value; }
        }

        private string _songArtists;
        public string SongArtists 
        { 
            get { return _songArtists;}
            set { _songArtists = value; }
        }

        private string _songFirstArtist;
        public string SongFirstArtist
        {
            get { return _songFirstArtist; }
            set { _songFirstArtist = value; }
        }

        private string _songPath;
        public string SongPath
        {
            get { return _songPath; }
            set
            {
                _songPath = value;

                TagLib.File songFile = TagLib.File.Create(_songPath);

                SongTitle = songFile.Tag.Title;
                if (SongTitle == "" || SongTitle == null)
                {
                    SongTitle = new FileInfo(_songPath).Name;
                }

                SongLyrics = songFile.Tag.Lyrics;
                if (SongLyrics == null || SongLyrics == "") { SongLyrics = "No lyrics for this song is found"; }

                SongArtists = songFile.Tag.JoinedAlbumArtists;
                if (SongArtists == null || SongArtists == "") { SongArtists = "No infomation"; };

                SongFirstArtist = songFile.Tag.FirstArtist;
                if (SongFirstArtist == null || SongFirstArtist == "") { SongFirstArtist = "No infomation"; };

                SongPerformers = songFile.Tag.JoinedPerformers;
                if (SongPerformers == null || SongPerformers == "") { SongPerformers = "No infomation"; };

                SongComposers = songFile.Tag.JoinedComposers;
                if (SongComposers == null || SongComposers == "") { SongComposers = "No infomation"; };

                SongGenres = songFile.Tag.JoinedGenres;
                if (SongGenres == null || SongGenres == "") { SongGenres = "No infomation"; };

                SongCopyright = songFile.Tag.Copyright;
                if (SongCopyright == null || SongCopyright == "") { SongCopyright = "No infomation"; };

                SongAlbum = songFile.Tag.Album;
                if (SongAlbum == null || SongAlbum == "") { SongAlbum = "No infomation"; };

                // Nam: Not using method below cause it's meeting a bug where duration return only about 70%
                // SongDurationInString = songFile.Length.Duration.ToString(DurationFormatHelper.GetDurationFormatFromTotalSeconds(songFile.Properties.Duration.TotalSeconds));
                SongDurationInString = MyMusicMediaPlayer.mediaPlayer.NaturalDuration.TimeSpan.ToString(DurationFormatHelper.GetDurationFormatFromTotalSeconds(MyMusicMediaPlayer.GetTotalTimeInSecond()));

                try
                {
                    TagLib.IPicture pic = songFile.Tag.Pictures[0];
                    MemoryStream ms = new MemoryStream(pic.Data.Data);
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = ms;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                    SongImage = bitmapImage;
                }
                catch (IndexOutOfRangeException)
                {
                    SongImage = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/ComputerPageIcons/ic_audio_file.png"));
                }
            }
        }

        private BitmapImage _songImage;
        public BitmapImage SongImage
        {
            get { return _songImage; }
            set { _songImage = value; }
        }

        private string _songPerformers;
        public string SongPerformers 
        { 
            get { return _songPerformers; }
            set { _songPerformers = value; }
        }

        private string _songComposers;
        public string SongComposers 
        { 
            get { return _songComposers; }
            set { _songComposers = value; }
        }

        private string _songGenres;
        public string SongGenres 
        { 
            get { return _songGenres; }
            set { _songGenres = value; }
        }

        private string _songCopyright;
        public string SongCopyright
        { 
            get { return _songCopyright; }
            set { _songCopyright = value; }
        }

        private string _songAlbum;
        public string SongAlbum
        { 
            get { return _songAlbum; }
            set { _songAlbum = value; }
        }

        private string _songDurationInString;
        public string SongDurationInString 
        { 
            get { return _songDurationInString; }
            set { _songDurationInString = value; }
        }

        public MusicModel(string songPath)
        {
            SongPath = songPath;
        }
    }
}