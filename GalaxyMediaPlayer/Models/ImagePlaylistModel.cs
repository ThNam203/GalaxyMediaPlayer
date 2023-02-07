using System;
using System.Collections.Generic;

namespace GalaxyMediaPlayer.Models
{
    public class ImagePlaylistModel
    {

        public ImagePlaylistModel(string playlistName)
        {
            this._id = Guid.NewGuid().ToString();
            this._playlistName = playlistName;
            this._images = new List<ImageModel>();
            this._timeCreated = DateTime.Now.ToString();
        }

        private ImagePlaylistModel(string id, string PlaylistName, string timeCreated)
        {
            this._id = id;
            this._playlistName = PlaylistName;
            this._timeCreated = timeCreated;
            this._images = new List<ImageModel>();
        }

        public ImagePlaylistModel(ImagePlaylistModel model)
        {
            this._id = model.Id;
            this._playlistName = model.PlaylistName;
            this._images = model.Images;
            this._timeCreated = model._timeCreated;
        }

        private string _id;
        public string Id
        {
            get { return _id; }
        }

        private string _playlistName;
        public string PlaylistName
        {
            get { return _playlistName; }
            set { _playlistName = value; }
        }

        private string _timeCreated;
        public string TimeCreated
        {
            get { return _timeCreated; }
        }

        private List<ImageModel> _images;
        public List<ImageModel> Images
        {
            get { return _images; }
            set
            { _images = value; }

        }

        public void AddNewImageToPlaylist(ImageModel imageModel)
        {
            Images.Add(imageModel);
        }
    }
}
