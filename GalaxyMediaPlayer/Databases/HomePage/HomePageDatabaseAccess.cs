using System.Collections.Generic;
using Dapper;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace GalaxyMediaPlayer.Databases.HomePage
{
    internal class HomePageDatabaseAccess
    {
        public static List<Pages.HomePage.MainPage.MostWatchEntity> GetMostWatchedEntities()
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionStr()))
            {
                var output = connection.Query<Pages.HomePage.MainPage.MostWatchEntity>("select * from MostWatched order by Count DESC limit 10");
                return output.ToList();
            }
        }

        public static List<Pages.HomePage.MainPage.MostWatchEntity> GetMostListenedEntities()
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionStr()))
            {
                var output = connection.Query<Pages.HomePage.MainPage.MostWatchEntity>("select * from MostListened order by Count DESC limit 10");
                return output.ToList();
            }
        }

        // Nam: return number is the number of rows affected when saving
        public static int SaveDataOnWatchingVideo(string videoPath)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionStr()))
            {
                List<Pages.HomePage.MainPage.MostWatchEntity> queryResult = 
                    connection.Query<Pages.HomePage.MainPage.MostWatchEntity>("select * from MostWatched where (Path)=@Path", new { Path = videoPath }).ToList();

                if (queryResult.Count == 1)
                {
                    queryResult[0].Count++;
                    int rowAffected = connection.Execute("update MostWatched set Count=@Count where Path=@Path", queryResult[0]);
                    return rowAffected;
                } 
                else
                {
                    int rowAffected = connection.Execute("insert into MostWatched (Path, Count) values (@Path, @Count)", new Pages.HomePage.MainPage.MostWatchEntity(videoPath, 1));
                    return rowAffected;
                }
            }
        }

        // Nam: return number is the number of rows affected when saving
        public static int SaveDataOnListeningMusic(string musicPath)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionStr()))
            {
                List<Pages.HomePage.MainPage.MostWatchEntity> queryResult =
                    connection.Query<Pages.HomePage.MainPage.MostWatchEntity>("select * from MostListened where (Path)=@Path", new { Path = musicPath }).ToList();

                if (queryResult.Count == 1)
                {
                    queryResult[0].Count++;
                    int rowAffected = connection.Execute("update MostListened set Count=@Count where Path=@Path", queryResult[0]);
                    return rowAffected;
                }
                else
                {
                    int rowAffected = connection.Execute("insert into MostListened (Path, Count) values (@Path, @Count)", new Pages.HomePage.MainPage.MostWatchEntity(musicPath, 1));
                    return rowAffected;
                }
            }
        }

        private static string GetConnectionStr()
        {
            return ConfigurationManager.ConnectionStrings["HomePageDbConnStr"].ConnectionString;
        }
    }
}
