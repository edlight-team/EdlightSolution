using Microsoft.Data.Sqlite;
using ServerModels.Models;
using System;
using System.Collections.Generic;

namespace SqliteDataExecuter.Entities
{
    public class Roles : DBConnector
    {
        public IEnumerable<RolesModel> GetModels(string condition)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from Roles where " + condition + ";";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<RolesModel> models = new();
                while (reader.Read())
                {
                    RolesModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.RoleName = reader.GetString(1);
                    model.RoleDescription = reader.GetString(2);
                    models.Add(model);
                }
                return models;
            }
            finally
            {
                CloseConnection();
            }
        }
        public IEnumerable<RolesModel> GetModels()
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from Roles;";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<RolesModel> models = new();
                while (reader.Read())
                {
                    RolesModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.RoleName = reader.GetString(1);
                    model.RoleDescription = reader.GetString(2);
                    models.Add(model);
                }
                return models;
            }
            finally
            {
                CloseConnection();
            }
        }
        public RolesModel PostModel(RolesModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                Guid generated = Guid.NewGuid();
                cmd.CommandText = "insert into Roles values (@id,@RoleName, @RoleDescription);";
                cmd.Parameters.Add(new SqliteParameter("@id", generated.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@RoleName", model.RoleName));
                cmd.Parameters.Add(new SqliteParameter("@RoleDescription", model.RoleDescription));
                int result = cmd.ExecuteNonQuery();
                model.Id = generated;
                return model;
            }
            finally
            {
                CloseConnection();
            }
        }
        public RolesModel PutModel(RolesModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "update Roles set \"RoleName\" = @RoleName, \"RoleDescription\" = @RoleDescription where ID = @id";
                cmd.Parameters.Add(new SqliteParameter("@RoleName", model.RoleName));
                cmd.Parameters.Add(new SqliteParameter("@RoleDescription", model.RoleDescription));
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
                cmd.CommandText = "delete from Roles where ID = @id";
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
                cmd.CommandText = "delete from Roles";
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
