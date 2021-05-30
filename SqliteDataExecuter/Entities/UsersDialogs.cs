using Microsoft.Data.Sqlite;
using ServerModels.Models;
using System;
using System.Collections.Generic;

namespace SqliteDataExecuter.Entities
{
    public class UsersDialogs : DBConnector
    {
        public IEnumerable<UsersDialogsModel> GetModels(string condition)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from UsersDialogs where " + condition + ";";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<UsersDialogsModel> models = new();
                while (reader.Read())
                {
                    UsersDialogsModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.IdUser = new Guid(reader.GetString(1));
                    model.IdDialog = new Guid(reader.GetString(2));
                    models.Add(model);
                }
                return models;
            }
            finally
            {
                CloseConnection();
            }
        }
        public IEnumerable<UsersDialogsModel> GetModels()
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from UsersDialogs;";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<UsersDialogsModel> models = new();
                while (reader.Read())
                {
                    UsersDialogsModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.IdUser = new Guid(reader.GetString(1));
                    model.IdDialog = new Guid(reader.GetString(2));
                    models.Add(model);
                }
                return models;
            }
            finally
            {
                CloseConnection();
            }
        }
        public UsersDialogsModel PostModel(UsersDialogsModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                Guid generated = Guid.NewGuid();
                cmd.CommandText = "insert into UsersDialogs values (@id,@IdUser,@IdDialog);";
                cmd.Parameters.Add(new SqliteParameter("@id", generated.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdUser", model.IdUser.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdDialog", model.IdDialog.ToString()));
                int result = cmd.ExecuteNonQuery();
                model.Id = generated;
                return model;
            }
            finally
            {
                CloseConnection();
            }
        }
        public UsersDialogsModel PutModel(UsersDialogsModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "update UsersDialogs set IdUser = @IdUser, IdDialog = @IdDialog where ID = @id";
                cmd.Parameters.Add(new SqliteParameter("@IdUser", model.IdUser.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdDialog", model.IdDialog.ToString()));
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
                cmd.CommandText = "delete from UsersDialogs where ID = @id";
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
                cmd.CommandText = "delete from UsersDialogs";
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
