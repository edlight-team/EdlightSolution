using Microsoft.Data.Sqlite;
using ServerModels.Models;
using System;
using System.Collections.Generic;

namespace SqliteDataExecuter.Entities
{
    public class StudentsGroups : DBConnector
    {
        public IEnumerable<StudentsGroupsModel> GetModels(string condition)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from StudentsGroups where " + condition + ";";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<StudentsGroupsModel> models = new();
                while (reader.Read())
                {
                    StudentsGroupsModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.IdStudent = new Guid(reader.GetString(1));
                    model.IdGroup = new Guid(reader.GetString(2));
                    models.Add(model);
                }
                return models;
            }
            finally
            {
                CloseConnection();
            }
        }
        public IEnumerable<StudentsGroupsModel> GetModels()
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from StudentsGroups;";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<StudentsGroupsModel> models = new();
                while (reader.Read())
                {
                    StudentsGroupsModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.IdStudent = new Guid(reader.GetString(1));
                    model.IdGroup = new Guid(reader.GetString(2));
                    models.Add(model);
                }
                return models;
            }
            finally
            {
                CloseConnection();
            }
        }
        public StudentsGroupsModel PostModel(StudentsGroupsModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                Guid generated = Guid.NewGuid();
                cmd.CommandText = "insert into StudentsGroups values (@id,@IdStudent,@IdGroup);";
                cmd.Parameters.Add(new SqliteParameter("@id", generated.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdStudent", model.IdStudent.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdGroup", model.IdGroup.ToString()));
                int result = cmd.ExecuteNonQuery();
                model.Id = generated;
                return model;
            }
            finally
            {
                CloseConnection();
            }
        }
        public StudentsGroupsModel PutModel(StudentsGroupsModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "update StudentsGroups set IdStudent = @IdStudent, IdGroup = @IdGroup where ID = @id";
                cmd.Parameters.Add(new SqliteParameter("@IdStudent", model.IdStudent.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdGroup", model.IdGroup.ToString()));
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
                cmd.CommandText = "delete from StudentsGroups where ID = @id";
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
                cmd.CommandText = "delete from StudentsGroups";
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
