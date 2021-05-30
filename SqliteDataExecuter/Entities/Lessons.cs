using Microsoft.Data.Sqlite;
using ServerModels.Models;
using System;
using System.Collections.Generic;

namespace SqliteDataExecuter.Entities
{
    public class Lessons : DBConnector
    {
        public IEnumerable<LessonsModel> GetModels(string condition)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from Lessons where " + condition + ";";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<LessonsModel> models = new();
                while (reader.Read())
                {
                    LessonsModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.Day = Convert.ToDateTime(reader.GetString(1));
                    model.IdTimeLessons = new Guid(reader.GetString(2));
                    model.IdTeacher = new Guid(reader.GetString(3));
                    model.IdAcademicDiscipline = new Guid(reader.GetString(4));
                    model.IdTypeClass = new Guid(reader.GetString(5));
                    model.IdAudience = new Guid(reader.GetString(6));
                    model.IdGroup = new Guid(reader.GetString(7));
                    model.CanceledReason = reader.GetValue(8).ToString();
                    models.Add(model);
                }
                return models;
            }
            finally
            {
                CloseConnection();
            }
        }
        public IEnumerable<LessonsModel> GetModels()
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from Lessons;";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<LessonsModel> models = new();
                while (reader.Read())
                {
                    LessonsModel model = new();
                    model.Id = new Guid(reader.GetString(0));
                    model.Day = Convert.ToDateTime(reader.GetString(1));
                    model.IdTimeLessons = new Guid(reader.GetString(2));
                    model.IdTeacher = new Guid(reader.GetString(3));
                    model.IdAcademicDiscipline = new Guid(reader.GetString(4));
                    model.IdTypeClass = new Guid(reader.GetString(5));
                    model.IdAudience = new Guid(reader.GetString(6));
                    model.IdGroup = new Guid(reader.GetString(7));
                    models.Add(model);
                }
                return models;
            }
            finally
            {
                CloseConnection();
            }
        }
        public LessonsModel PostModel(LessonsModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                Guid generated = Guid.NewGuid();
                cmd.CommandText = "insert into Lessons values " +
                    "( @id,@Day,@IdTimeLessons,@IdTeacher,@IdAcademicDiscipline,@IdTypeClass,@IdAudience,@IdGroup,@CanceledReason );";
                cmd.Parameters.Add(new SqliteParameter("@id", generated.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@Day", model.Day.ToShortDateString()));
                cmd.Parameters.Add(new SqliteParameter("@IdTimeLessons", model.IdTimeLessons.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdTeacher", model.IdTeacher.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdAcademicDiscipline", model.IdAcademicDiscipline.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdTypeClass", model.IdTypeClass.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdAudience", model.IdAudience.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdGroup", model.IdGroup.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@CanceledReason", model.CanceledReason ??= string.Empty));
                int result = cmd.ExecuteNonQuery();
                model.Id = generated;
                return model;
            }
            finally
            {
                CloseConnection();
            }
        }
        public LessonsModel PutModel(LessonsModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "update Lessons set " +
                    "\"Day\" = @Day, " +
                    "\"IdTimeLessons\" = @IdTimeLessons, " +
                    "\"IdTeacher\" = @IdTeacher, " +
                    "\"IdAcademicDiscipline\" = @IdAcademicDiscipline, " +
                    "\"IdTypeClass\" = @IdTypeClass, " +
                    "\"IdAudience\" = @IdAudience, " +
                    "\"IdGroup\" = @IdGroup, " +
                    "\"CanceledReason\" = @CanceledReason " +
                    "where ID = @id";
                cmd.Parameters.Add(new SqliteParameter("@Day", model.Day.ToShortDateString()));
                cmd.Parameters.Add(new SqliteParameter("@IdTimeLessons", model.IdTimeLessons.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdTeacher", model.IdTeacher.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdAcademicDiscipline", model.IdAcademicDiscipline.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdTypeClass", model.IdTypeClass.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdAudience", model.IdAudience.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@IdGroup", model.IdGroup.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@CanceledReason", model.CanceledReason ??= string.Empty));
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
                cmd.CommandText = "delete from Lessons where ID = @id";
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
                cmd.CommandText = "delete from Lessons";
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
