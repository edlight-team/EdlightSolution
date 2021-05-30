using Microsoft.Data.Sqlite;
using ServerModels.Models;
using System;
using System.Collections.Generic;

namespace SqliteDataExecuter.Entities
{
    public class Tests : DBConnector
    {
        public IEnumerable<TestsModel> GetModels(string condition)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from tests where " + condition + ";";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<TestsModel> tests = new();
                while (reader.Read())
                {
                    TestsModel test = new()
                    {
                        ID = new Guid(reader.GetString(0)),
                        Questions = reader.GetString(1)
                    };
                    tests.Add(test);
                }
                return tests;
            }
            finally
            {
                CloseConnection();
            }
        }
        public IEnumerable<TestsModel> GetModels()
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from tests;";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<TestsModel> tests = new();
                while (reader.Read())
                {
                    TestsModel test = new()
                    {
                        ID = new Guid(reader.GetString(0)),
                        Questions = reader.GetString(1)
                    };
                    tests.Add(test);
                }
                return tests;
            }
            finally
            {
                CloseConnection();
            }
        }
        public TestsModel PostModel(TestsModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                Guid generated = Guid.NewGuid();
                cmd.CommandText = "insert into Tests values (@id,@questions);";
                cmd.Parameters.Add(new SqliteParameter("@id", generated.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@questions", model.Questions));
                int result = cmd.ExecuteNonQuery();
                model.ID = generated;
                return model;
            }
            finally
            {
                CloseConnection();
            }
        }
        public TestsModel PutModel(TestsModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "update Tests set Questions = @questions where ID = @id";
                cmd.Parameters.Add(new SqliteParameter("@questions", model.Questions));
                cmd.Parameters.Add(new SqliteParameter("@id", model.ID.ToString()));
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
                cmd.CommandText = "delete from Tests where ID = @id";
                cmd.Parameters.Add(new SqliteParameter("@id", id.ToString()));
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
                cmd.CommandText = "delete from Tests";
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
