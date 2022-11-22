using System;
using System.Collections.Generic;

namespace GalaxyMediaPlayer.Models
{
    public class SongInfor
    {
        public string PlaylistId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public string Performer { get; set; }
        public string Length { get; set; }
        public string Path { get; set; }

        public SongInfor(SongInfor songInfor)
        {
            this.PlaylistId = songInfor.PlaylistId;
            this.Id = Guid.NewGuid().ToString();
            this.Name = songInfor.Name;
            this.Album = songInfor.Album;
            this.Artist = songInfor.Artist;
            this.Performer = songInfor.Performer;
            this.Length = songInfor.Length;
            this.Path = songInfor.Path;
        }
        
        public SongInfor(string playlistId, string name, string album, string artist, string performer, string length, string path)
        {
            this.PlaylistId = playlistId;
            this.Id = Guid.NewGuid().ToString();
            this.Name = name;
            this.Album = album;
            this.Artist = artist;
            this.Performer = performer;
            this.Length = length;
            this.Path = path;
        }
        
        private SongInfor(string playlistId, string id, string name, string album, string artist, string performer, string length, string path)
        {
            this.PlaylistId = playlistId;
            this.Id = id;
            this.Name = name;
            this.Album = album;
            this.Artist = artist;
            this.Performer = performer;
            this.Length = length;
            this.Path = path;
        }
    }
    public class SongPlaylistModel
    {
        public SongPlaylistModel(string playlistName)
        {
            this._id = Guid.NewGuid().ToString();
            this._name = playlistName;
            this._songs = new List<SongInfor>();
            this._timeCreated = DateTime.Now.ToString();
        }
        
        private SongPlaylistModel(string id, string name, string timeCreated)
        {
            this._id = id;
            this._name = name;
            this._timeCreated = timeCreated;
            this._songs = new List<SongInfor>();
        }

        public SongPlaylistModel(SongPlaylistModel model)
        {
            this._id = model.Id;
            this._name = model.Name;
            this._songs = model.Songs;
            this._timeCreated = DateTime.Now.ToString();
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

        private string _timeCreated;
        public string TimeCreated
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
