using Microsoft.Data.Sqlite;
using ServerModels.Models;
using System;
using System.Collections.Generic;

namespace SqliteDataExecuter.Entities
{
    public class Users : DBConnector
    {
        public IEnumerable<UserModel> GetModels(string condition)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from users where " + condition + ";";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<UserModel> users = new();
                while (reader.Read())
                {
                    UserModel user = new()
                    {
                        ID = new Guid(reader.GetString(0)),
                        Login = reader.GetString(1),
                        Password = reader.GetString(2),
                        Name = reader.GetString(3),
                        Surname = reader.GetString(4),
                        Patrnymic = reader.GetString(5),
                        Sex = reader.GetInt32(6),
                        Age = reader.GetInt32(7),
                        DaysPriority = reader.GetValue(8).ToString().AsList()
                    };
                    users.Add(user);
                }
                return users;
            }
            finally
            {
                CloseConnection();
            }
        }
        public IEnumerable<UserModel> GetModels()
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from users;";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<UserModel> users = new();
                while (reader.Read())
                {
                    UserModel user = new()
                    {
                        ID = new Guid(reader.GetString(0)),
                        Login = reader.GetString(1),
                        Password = reader.GetString(2),
                        Name = reader.GetString(3),
                        Surname = reader.GetString(4),
                        Patrnymic = reader.GetString(5),
                        Sex = reader.GetInt32(6),
                        Age = reader.GetInt32(7),
                        DaysPriority = reader.GetValue(8).ToString().AsList()
                    };
                    users.Add(user);
                }
                return users;
            }
            finally
            {
                CloseConnection();
            }
        }
        public UserModel PostModel(UserModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                Guid generated = Guid.NewGuid();
                cmd.CommandText = "insert into Users values (@id,@login,@password,@name,@surname,@patronymic,@sex,@age,@daysPriority);";
                cmd.Parameters.Add(new SqliteParameter("@id", generated.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@login", model.Login));
                cmd.Parameters.Add(new SqliteParameter("@password", model.Password));
                cmd.Parameters.Add(new SqliteParameter("@name", model.Name));
                cmd.Parameters.Add(new SqliteParameter("@surname", model.Surname));
                cmd.Parameters.Add(new SqliteParameter("@patronymic", model.Patrnymic));
                cmd.Parameters.Add(new SqliteParameter("@sex", model.Sex));
                cmd.Parameters.Add(new SqliteParameter("@age", model.Age));
                cmd.Parameters.Add(new SqliteParameter("@daysPriority", model.DaysPriority.AsString()));
                int result = cmd.ExecuteNonQuery();
                model.ID = generated;
                return model;
            }
            finally
            {
                CloseConnection();
            }
        }
        public UserModel PutModel(UserModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "update Users set Login = @login, Password = @password, Name = @name, Surname = @surname, Patronymic = @patronymic, Sex = @sex, Age = @age, DaysPriority = @daysPriority where ID = @id";
                cmd.Parameters.Add(new SqliteParameter("@login", model.Login));
                cmd.Parameters.Add(new SqliteParameter("@password", model.Password));
                cmd.Parameters.Add(new SqliteParameter("@name", model.Name));
                cmd.Parameters.Add(new SqliteParameter("@surname", model.Surname));
                cmd.Parameters.Add(new SqliteParameter("@patronymic", model.Patrnymic));
                cmd.Parameters.Add(new SqliteParameter("@sex", model.Sex));
                cmd.Parameters.Add(new SqliteParameter("@age", model.Age));
                cmd.Parameters.Add(new SqliteParameter("@daysPriority", model.DaysPriority.AsString()));
                cmd.Parameters.Add(new SqliteParameter("@id", model.ID.ToString()));
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
                cmd.CommandText = "delete from Users where ID = @id";
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
                cmd.CommandText = "delete from Users";
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
