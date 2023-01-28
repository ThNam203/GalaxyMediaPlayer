using Dapper;
using GalaxyMediaPlayer.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;


namespace GalaxyMediaPlayer.Databases.ImagePage
{
    public class ImagesPlaylistDBAccess
    {
        public static List<ImagePlaylistModel> LoadImagePlayList()
        {
            using (IDbConnection connStr = new SQLiteConnection(GetConnectionStr()))
            {
                var output = connStr.Query<ImagePlaylistModel>("select * from ImagePlaylistTable");
                return output.ToList();
            }
        }

        public static int SaveImagePlaylist(ImagePlaylistModel newImagePlaylist)
        {
            using (IDbConnection connStr = new SQLiteConnection(GetConnectionStr()))
            {
                bool isExisted = connStr.ExecuteScalar<bool>("select count(1) from ImagePlaylistTable where Id=@Id", newImagePlaylist);

                if (!isExisted)
                {
                    int rowAffected = connStr.Execute("insert into ImagePlaylistTable (Id, PlaylistName) values (@Id, @PlaylistName)", newImagePlaylist);
                    return rowAffected;
                }
                else
                    return -1;
            }
        }

        public static int DeleteImagePlaylist(ImagePlaylistModel newImagePlaylist)
        {
            using (IDbConnection connStr = new SQLiteConnection(GetConnectionStr()))
            {
                int rowAffected = connStr.Execute("Delete from ImagePlaylistTable where Id=@Id", newImagePlaylist);
                return rowAffected;
            }
        }

        private static string GetConnectionStr()
        {
            return ConfigurationManager.ConnectionStrings["ImagesDBConnStr"].ConnectionString;
        }
    }
}
