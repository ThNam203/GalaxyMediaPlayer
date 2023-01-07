using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace GalaxyMediaPlayer.ConnectDB
{
    public class DataConfig
    {
        private static string ConnectionString = "Data Source=DAT-PHAM\\SQLEXPRESS;Initial Catalog=ImageDB;Integrated Security=True";
        private static SqlConnection SqlConnect;

        public DataConfig()
        {

        }

        private static void OpenConnect()
        {
            SqlConnect = new SqlConnection(ConnectionString);
            SqlConnect.Open();
            if(SqlConnect.State == ConnectionState.Open)
                SqlConnect.Close();
        }

        public static DataTable DataTransport(string sSQL)
        {
            OpenConnect();
            SqlDataAdapter adapter = new SqlDataAdapter(sSQL, SqlConnect);
            DataTable dtReturn = new DataTable();
            dtReturn.Clear();
            adapter.Fill(dtReturn);
            return dtReturn;
        }

        public static int DataExecution(string sSQL)
        {
            int result = 0;
            OpenConnect();
            if(SqlConnect.State == ConnectionState.Closed)
                SqlConnect.Open();

            SqlCommand command = new SqlCommand(sSQL);
            command.Connection= SqlConnect;
            command.CommandType = CommandType.Text;
            command.CommandText = sSQL;
            result = command.ExecuteNonQuery();
            return result;
        }
    }
}
