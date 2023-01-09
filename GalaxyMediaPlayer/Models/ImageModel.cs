﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyMediaPlayer.Models
{
    public class ImageModel
    {
        private string _path = "";
        public string path { get { return _path; } set { _path = value; } }

        private bool _IsSelected = false;
        public bool imgIsSelected 
        { 
            get
            {
                return _IsSelected;
            } 
            set
            {
                _IsSelected = value;
            }
        }



        private string _dateCreated = "";
        public string dateCreated
        {
            get
            {
                return _dateCreated;
            }
            set
            {
                _dateCreated = value;
            }
        }

        public ImageModel()
        {
            path = "";
            dateCreated = "";
        }

        public ImageModel(string fileName="", string date="")
        {
            path = fileName;
            dateCreated = date;
        }
    }
}
