using Microsoft.Data.Sqlite;
using ServerModels.Models;
using System;
using System.Collections.Generic;

namespace SqliteDataExecuter.Entities
{
    public class Comments : DBConnector
    {
        public IEnumerable<CommentModel> GetModels(string condition)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from Comments where " + condition + ";";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<CommentModel> models = new();
                while (reader.Read())
                {
                    CommentModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.IdLesson = new Guid(reader.GetString(1));
                    model.IdUser = new Guid(reader.GetString(2));
                    model.Date = Convert.ToDateTime(reader.GetString(3));
                    model.Message = reader.GetString(4);
                    models.Add(model);
                }
                CloseConnection();
                return models;
            }
            finally
            {
                CloseConnection();
            }
        }
        public IEnumerable<CommentModel> GetModels()
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from Comments;";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<CommentModel> models = new();
                while (reader.Read())
                {
                    CommentModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.IdLesson = new Guid(reader.GetString(1));
                    model.IdUser = new Guid(reader.GetString(2));
                    model.Date = Convert.ToDateTime(reader.GetString(3));
                    model.Message = reader.GetString(4);
                    models.Add(model);
                }
                CloseConnection();
                return models;
            }
            finally
            {
                CloseConnection();
            }
        }
        public CommentModel PostModel(CommentModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                Guid generated = Guid.NewGuid();
                cmd.CommandText = "insert into Comments values (@id,@IdLesson,@IdUser,@Date,@Message);";
                cmd.Parameters.Add(new SqliteParameter("@id", generated.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdLesson", model.IdLesson.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdUser", model.IdUser.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@Date", model.Date.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@Message", model.Message ??= string.Empty));
                int result = cmd.ExecuteNonQuery();
                CloseConnection();
                model.Id = generated;
                return model;
            }
            finally
            {
                CloseConnection();
            }
        }
        public CommentModel PutModel(CommentModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "update Comments set IdLesson = @IdLesson, IdUser = @IdUser, Date = @Date, Message = @Message where ID = @id";
                cmd.Parameters.Add(new SqliteParameter("@IdLesson", model.IdLesson.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdUser", model.IdUser.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@Date", model.Date.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@Message", model.Message ??= string.Empty));
                cmd.Parameters.Add(new SqliteParameter("@id", model.Id.ToString()));
                int result = cmd.ExecuteNonQuery();
                CloseConnection();
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
                cmd.CommandText = "delete from Comments where ID = @id";
                cmd.Parameters.Add(new SqliteParameter("@id", id.ToString()));
                int result = cmd.ExecuteNonQuery();
                CloseConnection();
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
                cmd.CommandText = "delete from Comments";
                int result = cmd.ExecuteNonQuery();
                CloseConnection();
                return result;
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
