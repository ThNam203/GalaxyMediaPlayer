﻿using GalaxyMediaPlayer.Pages.NavContentPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyMediaPlayer.Models
{
    public class ImagePlaylistModel
    {
        public ImagePlaylistModel()
        {
            this._id = Guid.NewGuid().ToString();
            this._playlistName = "";
            this._images = new List<ImageModel>();
            this._timeCreated = DateTime.Now.ToString();
        }

        public ImagePlaylistModel(string playlistName)
        {
            this._id = Guid.NewGuid().ToString();
            this._playlistName = playlistName;
            this._images = new List<ImageModel>();
            this._timeCreated = DateTime.Now.ToString();
        }

        private ImagePlaylistModel(string id, string name, string timeCreated)
        {
            this._id = id;
            this._playlistName = name;
            this._timeCreated = timeCreated;
            this._images = new List<ImageModel>();
        }

        public ImagePlaylistModel(ImagePlaylistModel model)
        {
            this._id = model.Id;
            this._playlistName = model.PlaylistName;
            this._images = model.Images;
            this._timeCreated = DateTime.Now.ToString();
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

        public void AddNewSongToPlaylist(ImageModel imageModel)
        {
            Images.Add(imageModel);
        }
    }
}
