using Dapper;
using GalaxyMediaPlayer.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                    int rowAffected = connStr.Execute("insert into ImagePlaylistTable (Id, PlaylistName,TimeCreated) values (@Id, @PlaylistName,@TimeCreated)", newImagePlaylist);
                    return rowAffected;
                }
                else
                    return -1;
            }
        }

        public static int DeleteImagePlaylist(ImagePlaylistModel ImagePlaylist)
        {
            using (IDbConnection connStr = new SQLiteConnection(GetConnectionStr()))
            {
                connStr.Execute("Delete from ImageInPlaylistTable where PlaylistId=@Id", ImagePlaylist);
                int rowAffected = connStr.Execute("Delete from ImagePlaylistTable where Id=@Id", ImagePlaylist);
                return rowAffected;
            }
        }

        public static int RenameImagePlaylist(ImagePlaylistModel newPlaylist)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionStr()))
            {
                int rowAffected = connection.Execute("update ImagePlaylistTable set PlaylistName=@PlaylistName where Id=@Id", newPlaylist);
                return rowAffected;
            }
        }

        private static string GetConnectionStr()
        {
            return ConfigurationManager.ConnectionStrings["ImagesDBConnStr"].ConnectionString;
        }
    }
}
