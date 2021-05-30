using Microsoft.Data.Sqlite;
using ServerModels.Models;
using System;
using System.Collections.Generic;

namespace SqliteDataExecuter.Entities
{
    public class Materials : DBConnector
    {
        public IEnumerable<MaterialsModel> GetModels(string condition)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from Materials where " + condition + ";";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<MaterialsModel> models = new();
                while (reader.Read())
                {
                    MaterialsModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.IdLesson = new Guid(reader.GetString(1));
                    model.IdUser = new Guid(reader.GetString(2));
                    model.Title = reader.GetString(3);
                    model.Description = reader.GetString(4);
                    model.MaterialPath = reader.GetString(5);
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
        public IEnumerable<MaterialsModel> GetModels()
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from Materials;";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<MaterialsModel> models = new();
                while (reader.Read())
                {
                    MaterialsModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.IdLesson = new Guid(reader.GetString(1));
                    model.IdUser = new Guid(reader.GetString(2));
                    model.Title = reader.GetString(3);
                    model.Description = reader.GetString(4);
                    model.MaterialPath = reader.GetString(5);
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
        public MaterialsModel PostModel(MaterialsModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                Guid generated = Guid.NewGuid();
                cmd.CommandText = "insert into Materials values (@id,@IdLesson,@IdUser,@Title,@Description,@MaterialPath);";
                cmd.Parameters.Add(new SqliteParameter("@id", generated.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdLesson", model.IdLesson.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdUser", model.IdUser.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@Title", model.Title ??= string.Empty));
                cmd.Parameters.Add(new SqliteParameter("@Description", model.Description ??= string.Empty));
                cmd.Parameters.Add(new SqliteParameter("@MaterialPath", model.MaterialPath ??= string.Empty));
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
        public MaterialsModel PutModel(MaterialsModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "update Materials set IdLesson = @IdLesson, IdUser = @IdUser, Title = @Title, Description = @Description, MaterialPath = @MaterialPath where ID = @id";
                cmd.Parameters.Add(new SqliteParameter("@IdLesson", model.IdLesson.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdUser", model.IdUser.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@Title", model.Title ??= string.Empty));
                cmd.Parameters.Add(new SqliteParameter("@Description", model.Description ??= string.Empty));
                cmd.Parameters.Add(new SqliteParameter("@MaterialPath", model.MaterialPath ??= string.Empty));
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
                cmd.CommandText = "delete from Materials where ID = @id";
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
                cmd.CommandText = "delete from Materials";
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
