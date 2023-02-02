using Dapper;
using GalaxyMediaPlayer.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace GalaxyMediaPlayer.Databases.ImagePage
{
    public class ImagesInPlaylistDBAccess
    {
        public static List<ImageModel> LoadImageInPlayList(string PlaylistId)
        {
            using (IDbConnection connStr = new SQLiteConnection(GetConnectionStr()))
            {
                var imagePlaylistModel = new {PlaylistId = PlaylistId };
                var output = connStr.Query<ImageModel>("select * from ImageInPlaylistTable where PlaylistId=@PlaylistId",imagePlaylistModel);
                return output.ToList();
            }
        }

        public static int SaveImageIntoPlaylist(ImageModel newImageModel)
        {
            using (IDbConnection connStr = new SQLiteConnection(GetConnectionStr()))
            {
                bool isExisted = connStr.ExecuteScalar<bool>("select count(1) from ImageInPlaylistTable where PlaylistId=@PlaylistId and Path=@path", newImageModel);

                if (!isExisted)
                {
                    int rowAffected = connStr.Execute("insert into ImageInPlaylistTable (PlaylistId, Id, Name, Path, DateCreated, Size) values (@PlaylistId, @Id,@Name, @path, @dateCreated, @Size)", newImageModel);
                    return rowAffected;
                }
                else
                    return -1;
            }
        }

        public static int DeleteImagePlaylist(ImageModel imageModel)
        {
            using (IDbConnection connStr = new SQLiteConnection(GetConnectionStr()))
            {
                int rowAffected = connStr.Execute("Delete from ImageInPlaylistTable where PlaylistId=@PlaylistId and Id=@Id", imageModel);
                return rowAffected;
            }
        }

        private static string GetConnectionStr()
        {
            return ConfigurationManager.ConnectionStrings["ImagesDBConnStr"].ConnectionString;
        }
    }
}
