using GalaxyMediaPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SQLite;
using System.Data;
using Dapper;
using System.Collections.ObjectModel;

namespace GalaxyMediaPlayer.Databases.ImagePage
{
    public class ImagesDBAccess
    {
        public static List<ImageModel> LoadImageList()
        {
            using(IDbConnection connStr = new SQLiteConnection(GetConnectionStr()))
            {
                var output = connStr.Query<ImageModel>("select * from ImagesInfo", new DynamicParameters());
                return output.ToList();
            }    
        }

        public static int SaveImage(ImageModel newImage)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionStr()))
            {
                int rowAffected = connection.Execute("insert into ImagesInfo (ImagePath, DateCreated) values (@path, @dateCreated)", newImage);
                return rowAffected;
            }
        }

        private static string GetConnectionStr()
        {
            return ConfigurationManager.ConnectionStrings["ImagesDBConnStr"].ConnectionString;
        }
    }
}
