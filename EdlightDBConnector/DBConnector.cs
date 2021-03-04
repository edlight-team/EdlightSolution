using System.Data;
using System.Data.Odbc;

namespace EdlightDBConnector
{
    public class DBConnector
    {
        private static OdbcConnection connection;
        public OdbcConnection GetConnection() => connection;
        public OdbcConnection CreateConnection(string dbPath, bool doOpenConnection = false)
        {
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)}; Dbq=" + dbPath + ";";
            connection = new OdbcConnection(connectionString);
            if (doOpenConnection && connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            return connection;
        }
        public bool OpenDBConnection()
        {
            connection.Open();
            if (connection.State == ConnectionState.Open)
            {
                return true;
            }
            return false;
        }
        public bool CloseDBConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
                return true;
            }
            return false;
        }
    }
}
