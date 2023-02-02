using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyMediaPlayer.Models
{
    public class ImageModel
    {
        private string _PlaylistId;
        public string PlaylistId
        {
            get { return _PlaylistId; }
            set { _PlaylistId = value; }
        }
        private string _id;
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
        private string _path = "";
        public string path { get { return _path; } set { _path = value; } }

        private string _dateCreated = "";
        public string dateCreated
        {
            get { return _dateCreated; }
            set { _dateCreated = value; }
        }

        public ImageModel()
        {
            _PlaylistId = "";
            _id = Guid.NewGuid().ToString();
            _path = "";
            _dateCreated = "";
        }

        public ImageModel(string fileName, string date)
        {
            _PlaylistId = "";
            _id = Guid.NewGuid().ToString();
            _path = fileName;
            dateCreated = date;
        }

        public ImageModel(string Playlistid ,string fileName, string date)
        {
            _PlaylistId = Playlistid;
            _id = Guid.NewGuid().ToString();
            _path = fileName;
            dateCreated = date;
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
