using Microsoft.Data.Sqlite;
using ServerModels.Models;
using System;
using System.Collections.Generic;

namespace SqliteDataExecuter.Entities
{
    public class Messages : DBConnector
    {
        public IEnumerable<MessagesModel> GetModels(string condition)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from Messages where " + condition + ";";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<MessagesModel> models = new();
                while (reader.Read())
                {
                    MessagesModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.IdDialog = new Guid(reader.GetString(1));
                    model.IdUserSender = new Guid(reader.GetString(2));
                    model.TextMessage = reader.GetString(3);
                    model.IsRead = reader.GetInt32(4) == 1;
                    model.SendingTime = Convert.ToDateTime(reader.GetString(5));
                    models.Add(model);
                }
                return models;
            }
            finally
            {
                CloseConnection();
            }
        }
        public IEnumerable<MessagesModel> GetModels()
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from Messages;";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<MessagesModel> models = new();
                while (reader.Read())
                {
                    MessagesModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.IdDialog = new Guid(reader.GetString(1));
                    model.IdUserSender = new Guid(reader.GetString(2));
                    model.TextMessage = reader.GetString(3);
                    model.IsRead = reader.GetInt32(4) == 1;
                    model.SendingTime = Convert.ToDateTime(reader.GetString(5));
                    models.Add(model);
                }
                return models;
            }
            finally
            {
                CloseConnection();
            }
        }
        public MessagesModel PostModel(MessagesModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                Guid generated = Guid.NewGuid();
                cmd.CommandText = "insert into Messages values " +
                    "( @id,@IdDialog,@IdUserSender,@TextMessage,@IsRead,@SendingTime );";
                cmd.Parameters.Add(new SqliteParameter("@id", generated.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdDialog", model.IdDialog.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdUserSender", model.IdUserSender.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@TextMessage", model.TextMessage));
                cmd.Parameters.Add(new SqliteParameter("@IsRead", model.IsRead ? 1 : 0));
                cmd.Parameters.Add(new SqliteParameter("@SendingTime", model.SendingTime.ToString()));
                int result = cmd.ExecuteNonQuery();
                model.Id = generated;
                return model;
            }
            finally
            {
                CloseConnection();
            }
        }
        public MessagesModel PutModel(MessagesModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "update Messages set " +
                    "\"IdDialog\" = @IdDialog, " +
                    "\"IdUserSender\" = @IdUserSender, " +
                    "\"TextMessage\" = @TextMessage, " +
                    "\"IsRead\" = @IsRead, " +
                    "\"SendingTime\" = @SendingTime " +
                    "where ID = @id";
                cmd.Parameters.Add(new SqliteParameter("@IdDialog", model.IdDialog.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdUserSender", model.IdUserSender.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@TextMessage", model.TextMessage));
                cmd.Parameters.Add(new SqliteParameter("@IsRead", model.IsRead ? 1 : 0));
                cmd.Parameters.Add(new SqliteParameter("@SendingTime", model.SendingTime.ToString()));
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
                cmd.CommandText = "delete from Messages where ID = @id";
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
                cmd.CommandText = "delete from Messages";
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
