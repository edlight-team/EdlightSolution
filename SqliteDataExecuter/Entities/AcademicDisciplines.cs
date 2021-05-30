using Microsoft.Data.Sqlite;
using ServerModels.Models;
using System;
using System.Collections.Generic;

namespace SqliteDataExecuter.Entities
{
    public class AcademicDisciplines : DBConnector
    {
        public IEnumerable<AcademicDisciplinesModel> GetModels(string condition)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from AcademicDisciplines where " + condition + ";";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<AcademicDisciplinesModel> models = new();
                while (reader.Read())
                {
                    AcademicDisciplinesModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.Title = reader.GetString(1);
                    model.IdPriorityAudience = new Guid(reader.GetString(2));
                    models.Add(model);
                }
                return models;
            }
            finally
            {
                CloseConnection();
            }
        }
        public IEnumerable<AcademicDisciplinesModel> GetModels()
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from AcademicDisciplines;";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<AcademicDisciplinesModel> models = new();
                while (reader.Read())
                {
                    AcademicDisciplinesModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.Title = reader.GetString(1);
                    model.IdPriorityAudience = new Guid(reader.GetString(2));
                    models.Add(model);
                }
                return models;
            }
            finally
            {
                CloseConnection();
            }
        }
        public AcademicDisciplinesModel PostModel(AcademicDisciplinesModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                Guid generated = Guid.NewGuid();
                cmd.CommandText = "insert into AcademicDisciplines values (@id,@title,@IdPriorityAudience);";
                cmd.Parameters.Add(new SqliteParameter("@id", generated.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@title", model.Title));
                cmd.Parameters.Add(new SqliteParameter("@IdPriorityAudience", model.IdPriorityAudience.ToString()));
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
        public AcademicDisciplinesModel PutModel(AcademicDisciplinesModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "update AcademicDisciplines set Title = @title, IdPriorityAudience = @IdPriorityAudience where ID = @id";
                cmd.Parameters.Add(new SqliteParameter("@title", model.Title));
                cmd.Parameters.Add(new SqliteParameter("@IdPriorityAudience", model.IdPriorityAudience.ToString()));
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
                cmd.CommandText = "delete from AcademicDisciplines where ID = @id";
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
                cmd.CommandText = "delete from AcademicDisciplines";
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
