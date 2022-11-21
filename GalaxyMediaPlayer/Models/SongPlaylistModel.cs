using System;
using System.Collections.Generic;

namespace GalaxyMediaPlayer.Models
{
    public class SongInfor
    {
        public string Name { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public string Performer { get; set; }
        public string Length { get; set; }
        public string Path { get; set; }

        public SongInfor(SongInfor songInfor)
        {
            this.Name = songInfor.Name;
            this.Album = songInfor.Album;
            this.Artist = songInfor.Artist;
            this.Performer = songInfor.Performer;
            this.Length = songInfor.Length;
            this.Path = songInfor.Path;
        }
        
        public SongInfor(string Name, string Album, string Artist, string Performer, string Length, string Path)
        {
            this.Name = Name;
            this.Album = Album;
            this.Artist = Artist;
            this.Performer = Performer;
            this.Length = Length;
            this.Path = Path;
        }
    }
    public class SongPlaylistModel
    {
        public SongPlaylistModel(string playlistName)
        {
            this._id = Guid.NewGuid().ToString();
            this._name = playlistName;
            this._songs = new List<SongInfor>();
            this._timeCreated = DateTime.Now;
        }

        public SongPlaylistModel(SongPlaylistModel model)
        {
            this._id = model.Id;
            this._name = model.Name;
            this._songs = model.Songs;
            this._timeCreated = DateTime.Now;
        }

        private string _id;
        public string Id { 
            get { return _id; } 
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private DateTime _timeCreated;
        public DateTime TimeCreated
        {
            get { return _timeCreated; }
        }

        private List<SongInfor> _songs;
        public List<SongInfor> Songs
        {
            get { return _songs; }
            set
            {  _songs = value; }
            
        }

        public void AddNewSongToPlaylist(SongInfor songInfor)
        {
            _songs.Add(songInfor);
        }
    }
}
