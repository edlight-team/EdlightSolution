using Microsoft.Data.Sqlite;
using ServerModels.Models;
using System;
using System.Collections.Generic;

namespace SqliteDataExecuter.Entities
{
    public class LearnPlanes : DBConnector
    {
        public IEnumerable<LearnPlanesModel> GetModels(string condition)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from LearnPlanes where " + condition + ";";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<LearnPlanesModel> models = new();
                while (reader.Read())
                {
                    LearnPlanesModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.Name = reader.GetString(1);
                    model.Path = reader.GetString(2);
                    models.Add(model);
                }
                return models;
            }
            finally
            {
                CloseConnection();
            }
        }
        public IEnumerable<LearnPlanesModel> GetModels()
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from LearnPlanes;";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<LearnPlanesModel> models = new();
                while (reader.Read())
                {
                    LearnPlanesModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.Name = reader.GetString(1);
                    model.Path = reader.GetString(2);
                    models.Add(model);
                }
                return models;
            }
            finally
            {
                CloseConnection();
            }
        }
        public LearnPlanesModel PostModel(LearnPlanesModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                Guid generated = Guid.NewGuid();
                cmd.CommandText = "insert into LearnPlanes values (@id,@Name,@Path);";
                cmd.Parameters.Add(new SqliteParameter("@id", generated.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@Name", model.Name ??= string.Empty));
                cmd.Parameters.Add(new SqliteParameter("@Path", model.Path ??= string.Empty));
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
        public LearnPlanesModel PutModel(LearnPlanesModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "update LearnPlanes set Name = @Name, Path = @Path where ID = @id";
                cmd.Parameters.Add(new SqliteParameter("@Name", model.Name ??= string.Empty));
                cmd.Parameters.Add(new SqliteParameter("@Path", model.Path ??= string.Empty));
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
                cmd.CommandText = "delete from LearnPlanes where ID = @id";
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
                cmd.CommandText = "delete from LearnPlanes";
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
