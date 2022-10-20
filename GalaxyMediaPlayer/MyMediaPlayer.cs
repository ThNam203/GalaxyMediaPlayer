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
        public enum RepeatingOption {
            NoRepeat,
            RepeatOne,
            RepeatPlaylist
        }

        public static MediaPlayer mediaPlayer = new MediaPlayer();
        private static List<string> playlist = new List<string>();
        private static int positionInPlaylist = 0;
        // Nam: first is no loop, second is looping one, and last is loop over the playlist
        public static RepeatingOption repeatingOptions = RepeatingOption.NoRepeat;
        // Nam: determine if a song is currently opened in MediaPlayer,
        // if true, we resume or start it, if false, we open new file and start
        // but currently there is nowhere that it turn false
        public static bool isSongOpened = false; 
        public static bool isSongPlaying = false; // Nam: this is used for continue and pause function

        public static void Initialize()
        {
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
        }
        private static void OpenAndPlay(string songPath)
        {
            mediaPlayer.Stop();
            mediaPlayer.Open(new Uri(songPath, UriKind.Absolute));
            mediaPlayer.Play();
        }

        public static void PlayCurrentSong()
        {
            OpenAndPlay(playlist[positionInPlaylist]);
        }

        public static void Continue()
        {
            mediaPlayer.Play();
        }

        public static void Pause()
        {
            if (mediaPlayer.CanPause) mediaPlayer.Pause();
        }

        public static void SetPlaylist(List<string> songPaths)
        {
            playlist.Clear();
            playlist.AddRange(songPaths);
            SetPositionInPlaylist(0);
        }

        public static void PlayNextSong()
        {
            if (positionInPlaylist >= playlist.Count - 1)
            {
                positionInPlaylist = 0;
            }
            else { positionInPlaylist++; }
            PlayCurrentSong();
        }

        public static void PlayPreviousSong()
        {
            if (positionInPlaylist == 0)
            {
                positionInPlaylist = playlist.Count - 1;
            }
            else { positionInPlaylist--; }
            PlayCurrentSong();
        }

        public static void ChangeRepeatingOptionOnClick()
        {
            if (repeatingOptions == RepeatingOption.NoRepeat)
            {
                repeatingOptions = RepeatingOption.RepeatOne;
            }
            else if (repeatingOptions == RepeatingOption.RepeatOne)
            {
                repeatingOptions = RepeatingOption.RepeatPlaylist;
            }
            else if (repeatingOptions == RepeatingOption.RepeatPlaylist)
            {
                repeatingOptions = RepeatingOption.NoRepeat;
            }
        }

        public static void AddSongToPlaylist(string songPath)
        {
            playlist.Add(songPath);
        }

        public static void SetPositionInPlaylist(int pos)
        {
            positionInPlaylist = pos;
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
            if (repeatingOptions == RepeatingOption.RepeatOne)
            {
                mediaPlayer.Position = TimeSpan.Zero;
                mediaPlayer.Play();
            }
            else if (repeatingOptions == RepeatingOption.RepeatPlaylist)
            {
                positionInPlaylist++;
                if (positionInPlaylist >= playlist.Count) positionInPlaylist = 0;

                OpenAndPlay(playlist[positionInPlaylist]);
            }
            else if (repeatingOptions == RepeatingOption.NoRepeat)
            {
                // Doing nothing
            }
        }
    }
}
