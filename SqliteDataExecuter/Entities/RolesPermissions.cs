using Microsoft.Data.Sqlite;
using ServerModels.Models;
using System;
using System.Collections.Generic;

namespace SqliteDataExecuter.Entities
{
    public class RolesPermissions : DBConnector
    {
        public IEnumerable<RolesPermissionsModel> GetModels(string condition)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from RolesPermissions where " + condition + ";";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<RolesPermissionsModel> models = new();
                while (reader.Read())
                {
                    RolesPermissionsModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.IdRole = new Guid(reader.GetString(1));
                    model.IdPermission = new Guid(reader.GetString(2));
                    models.Add(model);
                }
                return models;
            }
            finally
            {
                CloseConnection();
            }
        }
        public IEnumerable<RolesPermissionsModel> GetModels()
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from RolesPermissions;";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<RolesPermissionsModel> models = new();
                while (reader.Read())
                {
                    RolesPermissionsModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.IdRole = new Guid(reader.GetString(1));
                    model.IdPermission = new Guid(reader.GetString(2));
                    models.Add(model);
                }
                return models;
            }
            finally
            {
                CloseConnection();
            }
        }
        public RolesPermissionsModel PostModel(RolesPermissionsModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                Guid generated = Guid.NewGuid();
                cmd.CommandText = "insert into RolesPermissions values (@id,@IdRole,@IdPermission);";
                cmd.Parameters.Add(new SqliteParameter("@id", generated.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdRole", model.IdRole.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdPermission", model.IdPermission.ToString()));
                int result = cmd.ExecuteNonQuery();
                model.Id = generated;
                return model;
            }
            finally
            {
                CloseConnection();
            }
        }
        public RolesPermissionsModel PutModel(RolesPermissionsModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "update RolesPermissions set IdRole = @IdRole, IdPermission = @IdPermission where ID = @id";
                cmd.Parameters.Add(new SqliteParameter("@IdRole", model.IdRole.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdPermission", model.IdPermission.ToString()));
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
                cmd.CommandText = "delete from RolesPermissions where ID = @id";
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
                cmd.CommandText = "delete from RolesPermissions";
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
