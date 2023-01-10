using Dapper;
using GalaxyMediaPlayer.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace GalaxyMediaPlayer.Databases.ImagePage
{
    public class ImagesDBAccess
    {
        public static List<ImageModel> LoadImageList()
        {
            using(IDbConnection connStr = new SQLiteConnection(GetConnectionStr()))
            {
                var output = connStr.Query<ImageModel>("select distinct * from ImagesTable");
                return output.ToList();
            }    
        }

        public static int SaveImage(ImageModel newImage)
        {
            using (IDbConnection connStr = new SQLiteConnection(GetConnectionStr()))
            {
                int rowAffected = connStr.Execute("insert into ImagesTable (Path, DateCreated) values (@path, @dateCreated)",newImage);
                return rowAffected;
            }
        }

        public static int DeleteImage(ImageModel newImage)
        {
            using (IDbConnection connStr = new SQLiteConnection(GetConnectionStr()))
            {
                int rowAffected = connStr.Execute("Delete from ImagesTable where Path=@path", newImage);
                return rowAffected;
            }
        }

        private static string GetConnectionStr()
        {
            return ConfigurationManager.ConnectionStrings["ImagesDBConnStr"].ConnectionString;
        }
    }
}
