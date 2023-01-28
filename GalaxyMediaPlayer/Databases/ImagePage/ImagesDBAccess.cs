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
                var output = connStr.Query<ImageModel>("select * from ImagesTable");
                return output.ToList();
            }    
        }

        public static int SaveImage(ImageModel newImage)
        {
            using (IDbConnection connStr = new SQLiteConnection(GetConnectionStr()))
            {
                bool isExisted = connStr.ExecuteScalar<bool>("select count(1) from ImagesTable where Id=@Id", newImage);
                if (!isExisted)
                {
                    int rowAffected = connStr.Execute("insert into ImagesTable (Id,Path, DateCreated) values (@Id, @path, @dateCreated)", newImage);
                    return rowAffected;
                }
                else
                    return -1;
            }
        }

        public static int DeleteImage(ImageModel newImage)
        {
            using (IDbConnection connStr = new SQLiteConnection(GetConnectionStr()))
            {
                int rowAffected = connStr.Execute("Delete from ImagesTable where Id=@Id", newImage);
                return rowAffected;
            }
        }

        private static string GetConnectionStr()
        {
            return ConfigurationManager.ConnectionStrings["ImagesDBConnStr"].ConnectionString;
        }
    }
}
