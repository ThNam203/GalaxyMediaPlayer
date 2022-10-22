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

                if (songFile.Tag.Lyrics == null) { SongLyrics = "No lyrics for this song is found"; }
                else SongLyrics = songFile.Tag.Lyrics;

                SongArtists = songFile.Tag.JoinedAlbumArtists;
                if (SongArtists == null) { SongArtists = ""; };

                SongFirstArtist = songFile.Tag.FirstArtist;
                if (SongFirstArtist == null) { SongFirstArtist = ""; };

                SongPerformers = songFile.Tag.JoinedPerformers;
                if (SongPerformers == null) { SongPerformers = ""; };

                SongComposers = songFile.Tag.JoinedComposers;
                if (SongComposers == null) { SongComposers = ""; };

                SongGenres = songFile.Tag.JoinedGenres;
                if (SongGenres == null) { SongGenres = ""; };

                SongCreatedYear = songFile.Tag.Year;

                // Nam: Not using method below cause it's meeting a bug where duration return only about 70%
                // SongDurationInString = songFile.Length.Duration.ToString(DurationFormatHelper.GetDurationFormatFromTotalSeconds(songFile.Properties.Duration.TotalSeconds));
                SongDurationInString = MyMediaPlayer.mediaPlayer.NaturalDuration.TimeSpan.ToString(DurationFormatHelper.GetDurationFormatFromTotalSeconds(MyMediaPlayer.GetTotalTimeInSecond()));

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

        private uint _songCreatedYear;
        public uint SongCreatedYear 
        { 
            get { return _songCreatedYear; }
            set { _songCreatedYear = value; }
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
