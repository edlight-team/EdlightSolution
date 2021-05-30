using Microsoft.Data.Sqlite;
using ServerModels.Models;
using System;
using System.Collections.Generic;

namespace SqliteDataExecuter.Entities
{
    public class TypeClasses : DBConnector
    {
        public IEnumerable<TypeClassesModel> GetModels(string condition)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from TypeClasses where " + condition + ";";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<TypeClassesModel> models = new();
                while (reader.Read())
                {
                    TypeClassesModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.Title = reader.GetString(1);
                    model.ShortTitle = reader.GetString(2);
                    model.ColorHex = reader.GetString(3);
                    models.Add(model);
                }
                return models;
            }
            finally
            {
                CloseConnection();
            }
        }
        public IEnumerable<TypeClassesModel> GetModels()
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from TypeClasses;";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<TypeClassesModel> models = new();
                while (reader.Read())
                {
                    TypeClassesModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.Title = reader.GetString(1);
                    model.ShortTitle = reader.GetString(2);
                    model.ColorHex = reader.GetString(3);
                    models.Add(model);
                }
                return models;
            }
            finally
            {
                CloseConnection();
            }
        }
        public TypeClassesModel PostModel(TypeClassesModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                Guid generated = Guid.NewGuid();
                cmd.CommandText = "insert into TypeClasses values (@id,@Title,@ShortTitle,@ColorHex);";
                cmd.Parameters.Add(new SqliteParameter("@id", generated.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@Title", model.Title));
                cmd.Parameters.Add(new SqliteParameter("@ShortTitle", model.ShortTitle));
                cmd.Parameters.Add(new SqliteParameter("@ColorHex", model.ColorHex));
                int result = cmd.ExecuteNonQuery();
                model.Id = generated;
                return model;
            }
            finally
            {
                CloseConnection();
            }
        }
        public TypeClassesModel PutModel(TypeClassesModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "update TypeClasses set " +
                    "Title = @Title, " +
                    "ShortTitle = @ShortTitle, " +
                    "ColorHex = @ColorHex " +
                    "where ID = @id";
                cmd.Parameters.Add(new SqliteParameter("@Title", model.Title));
                cmd.Parameters.Add(new SqliteParameter("@ShortTitle", model.ShortTitle));
                cmd.Parameters.Add(new SqliteParameter("@ColorHex", model.ColorHex));
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
                cmd.CommandText = "delete from TypeClasses where ID = @id";
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
                cmd.CommandText = "delete from TypeClasses";
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
