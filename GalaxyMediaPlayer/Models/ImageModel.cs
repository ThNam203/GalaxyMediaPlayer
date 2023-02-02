using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyMediaPlayer.Models
{
    public class ImageModel
    {
        private string _PlaylistId = "";
        private string _id = "";
        private string _Name = "";
        private string _path = "";
        private string _dateCreated = "";
        private long _length;
        private string _size = "";

        public string PlaylistId
        {
            get { return _PlaylistId; }
            set { _PlaylistId = value; }
        }
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        
        public string Name
        {
            get { return _Name; }
            set 
            {
                _Name = value;
                if (_Name == null) _Name = "";
            }
        }

        
        public string path { 
            get { return _path; } 
            set { 
                _path = value;
            } 
        }

        
        public string dateCreated
        {
            get { return _dateCreated; }
            set { _dateCreated = value; }
        }
        
        public long length
        {
            get { return _length; }
            set { _length = value; }
        }
        
        public string size
        {
            get { return _size; }
            set { _size = value; }
        }

        public ImageModel(string id, string name, string path, string dateCreated, string size)
        {
            _Name = name;
            _path = path;
            _dateCreated = dateCreated;
            _size = size;
            _length = _length = long.Parse(_size);
            _id = id;
        }

        public ImageModel(string Playlistid, string id, string name ,string path, string dateCreated, string size)
        {
            _PlaylistId = Playlistid;
            _id = id;
            _Name = name;
            _path = path;
            _dateCreated = dateCreated;
            _size= size;
            _length = _length = long.Parse(_size);
        }

        public int CompareDate(ImageModel model)
        {
            string date1 = this.dateCreated;
            string date2 = model.dateCreated;

            DateTime.TryParse(date1, out DateTime dt1);
            DateTime.TryParse(date2, out DateTime dt2);

            if (dt1 > dt2) return 1;
            if (dt1 < dt2) return -1;
            else return 0;
        }
    }
}
