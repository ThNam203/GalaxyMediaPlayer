using System.Collections.Generic;

namespace GalaxyMediaPlayer.Helpers
{
    public class SupportedExtensions
    {

        // Nam: this is for media file extension filter
        public static List<string> MUSIC_EXTENSION = new List<string> { "wma", "wax", "mp3", "m4a", "mpa", "mp2", "m3u", "mid", "midi", "rmi",
                                                                 "aif", "aifc", "aiff", "au", "snd", "wav", "cda", "aac", "adts", "m2ts", "flac" };
        public static List<string> VIDEO_EXTENSION = new List<string> { "asf", "wmv", "wm", "asx", "wvx", "wmx", "wpl", "dvr-ms",
                                                                 "wmd", "avi", "mpg", "mpeg", "m1v", "mpe", "ivf", "mov",
                                                                 "m4v", "mp4v", "3g2", "3gp2", "3gp", "3gpp", "mp4" };
        public static List<string> IMAGE_EXTENSION = new List<string> { "jpg", "gif", "png", "jpeg" };
    }
}
