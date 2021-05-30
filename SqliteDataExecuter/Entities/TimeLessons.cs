using Microsoft.Data.Sqlite;
using ServerModels.Models;
using System;
using System.Collections.Generic;

namespace SqliteDataExecuter.Entities
{
    public class TimeLessons : DBConnector
    {
        public IEnumerable<TimeLessonsModel> GetModels(string condition)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from TimeLessons where " + condition + ";";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<TimeLessonsModel> models = new();
                while (reader.Read())
                {
                    TimeLessonsModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.StartTime = Convert.ToDateTime(reader.GetString(1)).ToLongTimeString();
                    model.EndTime = Convert.ToDateTime(reader.GetString(2)).ToLongTimeString();
                    model.BreakTime = Convert.ToDateTime(reader.GetString(3)).ToLongTimeString();
                    models.Add(model);
                }
                return models;
            }
            finally
            {
                CloseConnection();
            }
        }
        public IEnumerable<TimeLessonsModel> GetModels()
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from TimeLessons;";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<TimeLessonsModel> models = new();
                while (reader.Read())
                {
                    TimeLessonsModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.StartTime = Convert.ToDateTime(reader.GetString(1)).ToShortTimeString();
                    model.EndTime = Convert.ToDateTime(reader.GetString(2)).ToShortTimeString();
                    model.BreakTime = reader.GetString(3);
                    models.Add(model);
                }
                return models;
            }
            finally
            {
                CloseConnection();
            }
        }
        public TimeLessonsModel PostModel(TimeLessonsModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                Guid generated = Guid.NewGuid();
                cmd.CommandText = "insert into TimeLessons values (@id,@StartTime,@EndTime,@BreakTime);";
                cmd.Parameters.Add(new SqliteParameter("@id", generated.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@StartTime", Convert.ToDateTime(model.StartTime).ToShortTimeString()));
                cmd.Parameters.Add(new SqliteParameter("@EndTime", Convert.ToDateTime(model.EndTime).ToShortTimeString()));
                cmd.Parameters.Add(new SqliteParameter("@BreakTime", model.BreakTime ??= string.Empty));
                int result = cmd.ExecuteNonQuery();
                model.Id = generated;
                return model;
            }
            finally
            {
                CloseConnection();
            }
        }
        public TimeLessonsModel PutModel(TimeLessonsModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "update TimeLessons set " +
                    "StartTime = @StartTime, " +
                    "EndTime = @EndTime, " +
                    "BreakTime = @BreakTime " +
                    "where ID = @id";
                cmd.Parameters.Add(new SqliteParameter("@StartTime", Convert.ToDateTime(model.StartTime).ToShortTimeString()));
                cmd.Parameters.Add(new SqliteParameter("@EndTime", Convert.ToDateTime(model.EndTime).ToShortTimeString()));
                cmd.Parameters.Add(new SqliteParameter("@BreakTime", model.BreakTime ??= string.Empty));
                cmd.Parameters.Add(new SqliteParameter("@id", model.Id.ToString()));
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
                cmd.CommandText = "delete from TimeLessons where ID = @id";
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
                cmd.CommandText = "delete from TimeLessons";
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
