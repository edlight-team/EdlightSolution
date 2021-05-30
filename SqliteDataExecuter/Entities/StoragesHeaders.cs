using Microsoft.Data.Sqlite;
using ServerModels.Models;
using System;
using System.Collections.Generic;

namespace SqliteDataExecuter.Entities
{
    public class StoragesHeaders : DBConnector
    {
        public IEnumerable<StoragesHeadersModel> GetModels(string condition)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from StoragesHeaders where " + condition + ";";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<StoragesHeadersModel> testResults = new();
                while (reader.Read())
                {
                    StoragesHeadersModel testResult = new()
                    {
                        ID = new Guid(reader.GetString(0)),
                        CreatorID = new Guid(reader.GetString(1)),
                        GroupID = new Guid(reader.GetString(2)),
                        StorageName = reader.GetString(3),
                        DateCloseStorage = reader.GetString(4)
                    };
                    testResults.Add(testResult);
                }
                return testResults;
            }
            finally
            {
                CloseConnection();
            }
        }
        public IEnumerable<StoragesHeadersModel> GetModels()
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from StoragesHeaders;";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<StoragesHeadersModel> testResults = new();
                while (reader.Read())
                {
                    StoragesHeadersModel testResult = new()
                    {
                        ID = new Guid(reader.GetString(0)),
                        CreatorID = new Guid(reader.GetString(1)),
                        GroupID = new Guid(reader.GetString(2)),
                        StorageName = reader.GetString(3),
                        DateCloseStorage = reader.GetString(4)
                    };
                    testResults.Add(testResult);
                }
                return testResults;
            }
            finally
            {
                CloseConnection();
            }
        }
        public StoragesHeadersModel PostModel(StoragesHeadersModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                Guid generated = Guid.NewGuid();
                cmd.CommandText = "insert into StoragesHeaders values (@ID,@CreatorID,@GroupID,@StorageName,@DateCloseStorage);";
                cmd.Parameters.Add(new SqliteParameter("@ID", generated.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@CreatorID", model.CreatorID.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@GroupID", model.GroupID.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@StorageName", model.StorageName));
                cmd.Parameters.Add(new SqliteParameter("@DateCloseStorage", model.DateCloseStorage));
                int result = cmd.ExecuteNonQuery();
                model.ID = generated;
                return model;
            }
            finally
            {
                CloseConnection();
            }
        }
        public StoragesHeadersModel PutModel(StoragesHeadersModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "update StoragesHeaders set CreatorID = @CreatorID, GroupID = @GroupID, StorageName = @StorageName, DateCloseStorage = @DateCloseStorage where ID = @ID";
                cmd.Parameters.Add(new SqliteParameter("@CreatorID", model.CreatorID.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@GroupID", model.GroupID.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@StorageName", model.StorageName));
                cmd.Parameters.Add(new SqliteParameter("@DateCloseStorage", model.DateCloseStorage));
                cmd.Parameters.Add(new SqliteParameter("@ID", model.ID.ToString()));
                int result = cmd.ExecuteNonQuery();
                return model;
            }
            finally
            {
                CloseConnection();
            }
        }
        public int DeleteModel(Guid id)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "delete from StoragesHeaders where ID = @ID";
                cmd.Parameters.Add(new SqliteParameter("@ID", id.ToString()));
                int result = cmd.ExecuteNonQuery();
                return result;
            }
            finally
            {
                CloseConnection();
            }
        }
        public int DeleteAll()
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "delete from StoragesHeaders";
                int result = cmd.ExecuteNonQuery();
                return result;
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
