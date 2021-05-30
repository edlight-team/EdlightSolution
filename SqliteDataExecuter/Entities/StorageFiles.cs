using Microsoft.Data.Sqlite;
using ServerModels.Models;
using System;
using System.Collections.Generic;

namespace SqliteDataExecuter.Entities
{
    public class StorageFiles : DBConnector
    {
        public IEnumerable<StorageFilesModel> GetModels(string condition)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from StorageFiles where " + condition + ";";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<StorageFilesModel> testResults = new();
                while (reader.Read())
                {
                    StorageFilesModel testResult = new()
                    {
                        ID = new Guid(reader.GetString(0)),
                        StorageID = new Guid(reader.GetString(1)),
                        StudentID = new Guid(reader.GetString(2)),
                        FileName = reader.GetString(3)
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
        public IEnumerable<StorageFilesModel> GetModels()
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from StorageFiles;";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<StorageFilesModel> testResults = new();
                while (reader.Read())
                {
                    StorageFilesModel testResult = new()
                    {
                        ID = new Guid(reader.GetString(0)),
                        StorageID = new Guid(reader.GetString(1)),
                        StudentID = new Guid(reader.GetString(2)),
                        FileName = reader.GetString(3)
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
        public StorageFilesModel PostModel(StorageFilesModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                Guid generated = Guid.NewGuid();
                cmd.CommandText = "insert into StorageFiles values (@ID,@StorageID,@StudentID,@FileName);";
                cmd.Parameters.Add(new SqliteParameter("@ID", generated.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@StorageID", model.StorageID.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@StudentID", model.StudentID.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@FileName", model.FileName));
                int result = cmd.ExecuteNonQuery();
                model.ID = generated;
                return model;
            }
            finally
            {
                CloseConnection();
            }
        }
        public StorageFilesModel PutModel(StorageFilesModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "update StorageFiles set StorageID = @StorageID, StudentID = @StudentID, FileName = @FileName where ID = @ID";
                cmd.Parameters.Add(new SqliteParameter("@StorageID", model.StorageID.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@StudentID", model.StudentID.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@FileName", model.FileName));
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
                cmd.CommandText = "delete from StorageFiles where ID = @ID";
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
                cmd.CommandText = "delete from StorageFiles";
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
