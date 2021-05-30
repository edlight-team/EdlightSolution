using Microsoft.Data.Sqlite;
using System.Data;

namespace SqliteDataExecuter
{
    public class DBConnector
    {
        public static bool UseTestBase;
        public static string DBToken = "5B6253853ACCF8B8E4FEE1F67C46D";
#if DEBUG
        public static string URLAddress = "http://192.168.0.100:600";
        //public static string URLAddress = "http://192.168.0.164:600";
#else
        public static string URLAddress = "http://62.173.154.96:600";
#endif
        public static string DBPath() => UseTestBase ? "./EdlightTestDB.db" : "./EdlightDB.db";
        public static SqliteConnection Connection { get; private set; }
        public SqliteConnection CreateConnection(string db_path, bool try_connect = true)
        {
            Connection = new SqliteConnection($"Data Source={db_path};");
            SQLitePCL.Batteries.Init();

            if (try_connect && Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
            return Connection;
        }
        public bool CloseConnection(SqliteConnection sqliteConnection = null)
        {
            if (sqliteConnection != null && sqliteConnection.State == ConnectionState.Open)
            {
                sqliteConnection.Close();
                return true;
            }
            if (Connection != null && Connection.State == ConnectionState.Open)
            {
                Connection.Close();
                return true;
            }
            return false;
        }
    }
}
