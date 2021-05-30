using Microsoft.Data.Sqlite;
using ServerModels.Models;
using System;
using System.Collections.Generic;

namespace SqliteDataExecuter.Entities
{
    public class Permissions : DBConnector
    {
        public IEnumerable<PermissionsModel> GetModels(string condition)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from Permissions where " + condition + ";";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<PermissionsModel> models = new();
                while (reader.Read())
                {
                    PermissionsModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.PermissionName = reader.GetString(1);
                    model.PermissionDescription = reader.GetString(2);
                    models.Add(model);
                }
                return models;
            }
            finally
            {
                CloseConnection();
            }
        }
        public IEnumerable<PermissionsModel> GetModels()
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from Permissions;";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<PermissionsModel> models = new();
                while (reader.Read())
                {
                    PermissionsModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.PermissionName = reader.GetString(1);
                    model.PermissionDescription = reader.GetString(2);
                    models.Add(model);
                }
                return models;
            }
            finally
            {
                CloseConnection();
            }
        }
        public PermissionsModel PostModel(PermissionsModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                Guid generated = Guid.NewGuid();
                cmd.CommandText = "insert into Permissions values (@id,@PermissionName, @PermissionDescription);";
                cmd.Parameters.Add(new SqliteParameter("@id", generated.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@PermissionName", model.PermissionName));
                cmd.Parameters.Add(new SqliteParameter("@PermissionDescription", model.PermissionDescription));
                int result = cmd.ExecuteNonQuery();
                model.Id = generated;
                return model;
            }
            finally
            {
                CloseConnection();
            }
        }
        public PermissionsModel PutModel(PermissionsModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "update Permissions set \"PermissionName\" = @PermissionName, \"PermissionDescription\" = @PermissionDescription where ID = @id";
                cmd.Parameters.Add(new SqliteParameter("@PermissionName", model.PermissionName));
                cmd.Parameters.Add(new SqliteParameter("@PermissionDescription", model.PermissionDescription));
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
                cmd.CommandText = "delete from Permissions where ID = @id";
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
                cmd.CommandText = "delete from Permissions";
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
