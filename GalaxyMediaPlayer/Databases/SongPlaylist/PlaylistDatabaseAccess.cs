using Dapper;
using GalaxyMediaPlayer.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace GalaxyMediaPlayer.Databases.SongPlaylist
{
    public class PlaylistDatabaseAccess
    {
        public static List<SongPlaylistModel> LoadPlaylists()
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionStr()))
            {
                var output = connection.Query<SongPlaylistModel>("select * from playlists");
                return output.ToList();
            }
        }

        // Nam: return number is the number of rows affected when saving
        public static int SavePlaylist(SongPlaylistModel newPlaylist)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionStr()))
            {
                int rowAffected = connection.Execute("insert into Playlists (Id, Name, TimeCreated) values (@Id, @Name, @TimeCreated)", newPlaylist);
                return rowAffected;
            }
        }

        // Nam: return number is the number of rows affected when saving
        public static int DeletePlaylist(SongPlaylistModel newPlaylist)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionStr()))
            {
                int rowAffected = connection.Execute("delete from Playlists where Id=@Id", newPlaylist);
                return rowAffected;
            }
        }

        // Nam: return number is the number of rows affected when saving
        public static int RenamePlaylist(SongPlaylistModel newPlaylist)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionStr()))
            {
                int rowAffected = connection.Execute("update Playlists set Name=@Name where Id=@Id", newPlaylist);
                return rowAffected;
            }
        }

        private static string GetConnectionStr()
        {
            return ConfigurationManager.ConnectionStrings["PlaylistDbConnStr"].ConnectionString;
        }
    }
}
