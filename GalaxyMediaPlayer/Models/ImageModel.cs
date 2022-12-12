using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyMediaPlayer.Models
{
    public class ImageModel
    {
        private string _path;
        public string path { get { return _path; } set { _path = value; } }
        public ImageModel(string fileName)
        {
            _path = fileName;
        }
    }
}
