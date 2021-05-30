using Microsoft.Data.Sqlite;
using ServerModels.Models;
using System;
using System.Collections.Generic;

namespace SqliteDataExecuter.Entities
{
    public class UsersRoles : DBConnector
    {
        public IEnumerable<UsersRolesModel> GetModels(string condition)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from UsersRoles where " + condition + ";";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<UsersRolesModel> models = new();
                while (reader.Read())
                {
                    UsersRolesModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.IdUser = new Guid(reader.GetString(1));
                    model.IdRole = new Guid(reader.GetString(2));
                    models.Add(model);
                }
                return models;
            }
            finally
            {
                CloseConnection();
            }
        }
        public IEnumerable<UsersRolesModel> GetModels()
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from UsersRoles;";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<UsersRolesModel> models = new();
                while (reader.Read())
                {
                    UsersRolesModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.IdUser = new Guid(reader.GetString(1));
                    model.IdRole = new Guid(reader.GetString(2));
                    models.Add(model);
                }
                return models;
            }
            finally
            {
                CloseConnection();
            }
        }
        public UsersRolesModel PostModel(UsersRolesModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                Guid generated = Guid.NewGuid();
                cmd.CommandText = "insert into UsersRoles values (@id,@IdUser,@IdRole);";
                cmd.Parameters.Add(new SqliteParameter("@id", generated.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdUser", model.IdUser.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdRole", model.IdRole.ToString()));
                int result = cmd.ExecuteNonQuery();
                model.Id = generated;
                return model;
            }
            finally
            {
                CloseConnection();
            }
        }
        public UsersRolesModel PutModel(UsersRolesModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "update UsersRoles set IdUser = @IdUser, IdRole = @IdRole where ID = @id";
                cmd.Parameters.Add(new SqliteParameter("@IdUser", model.IdUser.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdRole", model.IdRole.ToString()));
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
                cmd.CommandText = "delete from UsersRoles where ID = @id";
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
                cmd.CommandText = "delete from UsersRoles";
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
