using Microsoft.Data.Sqlite;
using ServerModels.Models;
using System;
using System.Collections.Generic;

namespace SqliteDataExecuter.Entities
{
    public class ManualsFiles : DBConnector
    {
        public IEnumerable<ManualsFilesModel> GetModels(string condition)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from ManualsFiles where " + condition + ";";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<ManualsFilesModel> testResults = new();
                while (reader.Read())
                {
                    ManualsFilesModel testResult = new()
                    {
                        ID = new Guid(reader.GetString(0)),
                        CreatorID = new Guid(reader.GetString(1)),
                        GroupID = new Guid(reader.GetString(2)),
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
        public IEnumerable<ManualsFilesModel> GetModels()
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from ManualsFiles;";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<ManualsFilesModel> testResults = new();
                while (reader.Read())
                {
                    ManualsFilesModel testResult = new()
                    {
                        ID = new Guid(reader.GetString(0)),
                        CreatorID = new Guid(reader.GetString(1)),
                        GroupID = new Guid(reader.GetString(2)),
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
        public ManualsFilesModel PostModel(ManualsFilesModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                Guid generated = Guid.NewGuid();
                cmd.CommandText = "insert into ManualsFiles values (@ID,@CreatorID,@GroupID,@FileName);";
                cmd.Parameters.Add(new SqliteParameter("@ID", generated.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@CreatorID", model.CreatorID.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@GroupID", model.GroupID.ToString()));
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
        public ManualsFilesModel PutModel(ManualsFilesModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "update ManualsFiles set CreatorID = @CreatorID, GroupID = @GroupID, FileName = @FileName where ID = @ID";
                cmd.Parameters.Add(new SqliteParameter("@CreatorID", model.CreatorID.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@GroupID", model.GroupID.ToString()));
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
                cmd.CommandText = "delete from ManualsFiles where ID = @ID";
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
                cmd.CommandText = "delete from ManualsFiles";
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
