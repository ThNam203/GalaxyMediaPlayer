using GalaxyMediaPlayer.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;

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
        private static List<string> tempPlaylist = new List<string>();
        private static int positionInPlaylist = 0;

        // if currentPath is the same as the folder holding the playing song
        // show extra control buttons in info grid AND
        // we update playPauseButtonInGridInfo along with playPauseButton 
        // if it's false, then we update separately
        public static string pathCurrentlyInUse = "";

        // Nam: first is no loop, second is looping one, and last is loop over the playlist
        public static RepeatingOption repeatingOptions = RepeatingOption.NoRepeat;

        // Nam: determine if a song is currently opened in MediaPlayer,
        // if true, we resume or start it, if false, we open new file and start
        // this property is like isSongStopped
        public static bool isSongOpened = false;

        // Nam: this is used for continue and pause function
        public static bool isSongPlaying = false;

        public static bool isRandoming = false;

        public static void Initialize()
        {
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
        }
        private static void OpenAndPlay(string songPath)
        {
            // Nam: mediaPlayer.MediaOpen() wont invoke if it open the same songPath with the previous songPath
            // so we need to add a garbage songPath to open first
            // We don't know if it's a good work-around
            mediaPlayer.Open(new Uri("C:\\dd", UriKind.Absolute));
            mediaPlayer.Open(new Uri(songPath, UriKind.Absolute));

            string? songFolder = Path.GetDirectoryName(songPath);
            if (string.IsNullOrEmpty(songFolder)) songFolder = songPath.Substring(0, songPath.LastIndexOf("\\"));

            mediaPlayer.Play();
        }

        public static void PlayCurrentSong()
        {
            // Nam: it's not songPath, folderPath can be "Playlist/Playlist.Name"
            pathCurrentlyInUse = MainPage.currentMusicBrowsingFolder;
            isSongPlaying = true;
            isSongOpened = true;
            OpenAndPlay(playlist[positionInPlaylist]);
        }

        public static void Continue()
        {
            isSongPlaying = true;
            isSongOpened = true;
            mediaPlayer.Play();
        }

        public static void Pause()
        {
            if (mediaPlayer.CanPause)
            {
                mediaPlayer.Pause();
                isSongPlaying = false;
            }
        }

        public static void Stop()
        {
            mediaPlayer.Stop();
            isSongPlaying = false;
            isSongOpened = false;
        }

        public static int GetPlaylistSize() => playlist.Count;
        public static int GetTempPlaylistSize() => tempPlaylist.Count;

        public static void SetPlaylistFromTempPlaylist()
        {
            playlist.Clear();
            playlist.AddRange(tempPlaylist);
            SetPositionInPlaylist(0);
        }

        public static void SetNewPlaylist(List<string> songPaths)
        {
            playlist.Clear();
            playlist.AddRange(songPaths);
            SetPositionInPlaylist(0);
        }

        public static void SetTempPlaylist(List<string> songPaths)
        {
            tempPlaylist.Clear();
            tempPlaylist.AddRange(songPaths);
        }

        public static void PlayNextSong()
        {
            if (isRandoming)
            {
                int random = new Random().Next(0, GetPlaylistSize());
                while (random == positionInPlaylist) random = new Random().Next(0, GetPlaylistSize());
                positionInPlaylist = random;
            }
            else if (positionInPlaylist >= playlist.Count - 1)
            {
                positionInPlaylist = 0;
            }
            else { positionInPlaylist++; }
            PlayCurrentSong();
        }

        public static void PlayPreviousSong()
        {
            if (isRandoming)
            {
                int random = new Random().Next(0, GetPlaylistSize());
                while (random == positionInPlaylist) random = new Random().Next(0, GetPlaylistSize());
                positionInPlaylist = random;
            }
            else if (positionInPlaylist == 0)
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

        public static Uri GetSource => mediaPlayer.Source;

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
            else if (isRandoming)
            {
                int random = new Random().Next(0, GetPlaylistSize());
                while (random == positionInPlaylist) random = new Random().Next(0, GetPlaylistSize());

                positionInPlaylist = random;
                PlayCurrentSong();
            }
            else if (repeatingOptions == RepeatingOption.RepeatPlaylist)
            {
                positionInPlaylist++;
                if (positionInPlaylist >= playlist.Count) positionInPlaylist = 0;
                PlayCurrentSong();
            }
            else if (repeatingOptions == RepeatingOption.NoRepeat)
            {
                positionInPlaylist++;
                if (positionInPlaylist < playlist.Count) PlayCurrentSong();
            }
        }
    }
}
