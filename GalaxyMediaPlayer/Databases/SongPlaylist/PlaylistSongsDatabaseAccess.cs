using Dapper;
using GalaxyMediaPlayer.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Windows.Documents;

namespace GalaxyMediaPlayer.Databases.SongPlaylist
{
    public class PlaylistSongsDatabaseAccess
    {
        public static List<SongInfor> LoadSongsFromPlaylistId(string playlistId)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionStr()))
            {
                var parameters = new { PlaylistId = playlistId };
                var output = connection.Query<SongInfor>("select * from PlaylistSongs where PlaylistId=@PlaylistId", parameters);
                return output.ToList();
            }
        }

        // Nam: return number is the number of rows affected when saving
        public static int SaveSong(SongInfor songInfor)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionStr()))
            {
                int rowAffected = connection
                    .Execute("insert into PlaylistSongs (PlaylistId, Id, Name, Album, Artist, Performer, Length, Path) " +
                    "values (@PlaylistId, @Id, @Name, @Album, @Artist, @Performer, @Length, @Path)", songInfor);
                return rowAffected;
            }
        }

        // Nam: return number is the number of rows affected when saving
        public static int DeleteSong(SongInfor songInfor)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionStr()))
            {
                int rowAffected = connection.Execute("delete from PlaylistSongs where Id=@Id", songInfor);
                return rowAffected;
            }
        }

        private static string GetConnectionStr()
        {
            return ConfigurationManager.ConnectionStrings["PlaylistDbConnStr"].ConnectionString;
        }
    }
}
