using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace GalaxyMediaPlayer
{
    internal class MyMediaPlayer
    {
        public static MediaPlayer mediaPlayer = new MediaPlayer();
        public static bool isLooping = false;
        public static bool isSongOpened = false; // Nam: determine if a song is currently opened in MediaPlayer, if true, we resume or start it, if false, we open new file and start
        public static bool isSongPlaying = false; // Nam: this is used for continue and pause function

        public static void Initialize()
        {
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
        }

        public static void OpenAndPlay(string songPath)
        {
            mediaPlayer.Open(new Uri(songPath, UriKind.Absolute));
            mediaPlayer.Play();
        }

        public static void Play()
        {
            mediaPlayer.Play();
        }

        public static void Pause()
        {
            if (mediaPlayer.CanPause) mediaPlayer.Pause();
        }

        public static void SetVolumn(double vol)
        {
            mediaPlayer.Volume = vol;
        }

        public static double GetVolumn => mediaPlayer.Volume;

        public static double GetTotalTimeInSecond() 
        {
            return mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
        }

        private static void MediaPlayer_MediaEnded(object? sender, EventArgs e)
        {
            // Nam: looping song function
            if (isLooping)
            {
                mediaPlayer.Position = TimeSpan.Zero;
                mediaPlayer.Play();
            }
            else
            {
                // Nam: THIS IS NOT SECURED, CHECK IT LATER
                isSongOpened = false;
            }
        }
    }
}
