using Microsoft.Data.Sqlite;
using ServerModels.Models;
using System;
using System.Collections.Generic;

namespace SqliteDataExecuter.Entities
{
    public class Audiences : DBConnector
    {
        public IEnumerable<AudiencesModel> GetModels(string condition)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from Audiences where " + condition + ";";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<AudiencesModel> models = new();
                while (reader.Read())
                {
                    AudiencesModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.NumberAudience = reader.GetString(1);
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
        public IEnumerable<AudiencesModel> GetModels()
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from Audiences;";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<AudiencesModel> models = new();
                while (reader.Read())
                {
                    AudiencesModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.NumberAudience = reader.GetString(1);
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
        public AudiencesModel PostModel(AudiencesModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                Guid generated = Guid.NewGuid();
                cmd.CommandText = "insert into Audiences values (@id,@NumberAudience);";
                cmd.Parameters.Add(new SqliteParameter("@id", generated.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@NumberAudience", model.NumberAudience));
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
        public AudiencesModel PutModel(AudiencesModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "update Audiences set NumberAudience = @NumberAudience where ID = @id";
                cmd.Parameters.Add(new SqliteParameter("@NumberAudience", model.NumberAudience));
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
                cmd.CommandText = "delete from Audiences where ID = @id";
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
                cmd.CommandText = "delete from Audiences";
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
