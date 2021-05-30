using Microsoft.Data.Sqlite;
using ServerModels.Models;
using System;
using System.Collections.Generic;

namespace SqliteDataExecuter.Entities
{
    public class TestHeaders : DBConnector
    {
        public IEnumerable<TestHeadersModel> GetModels(string condition)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from TestHeaders where " + condition + ";";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<TestHeadersModel> testHeaders = new();
                while (reader.Read())
                {
                    TestHeadersModel testHeader = new()
                    {
                        ID = new Guid(reader.GetString(0)),
                        TestID = new Guid(reader.GetString(1)),
                        GroupID = new Guid(reader.GetString(2)),
                        TeacherID = new Guid(reader.GetString(3)),
                        TestName = reader.GetString(4),
                        TestType = reader.GetString(5),
                        TestTime = reader.GetString(6),
                        CountQuestions = reader.GetInt32(7),
                        TestStartDate = reader.GetString(8),
                        TestEndDate = reader.GetString(9)
                    };
                    testHeaders.Add(testHeader);
                }
                return testHeaders;
            }
            finally
            {
                CloseConnection();
            }
        }
        public IEnumerable<TestHeadersModel> GetModels()
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from TestHeaders;";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<TestHeadersModel> testHeaders = new();
                while (reader.Read())
                {
                    TestHeadersModel testHeader = new()
                    {
                        ID = new Guid(reader.GetString(0)),
                        TestID = new Guid(reader.GetString(1)),
                        GroupID = new Guid(reader.GetString(2)),
                        TeacherID = new Guid(reader.GetString(3)),
                        TestName = reader.GetString(4),
                        TestType = reader.GetString(5),
                        TestTime = reader.GetString(6),
                        CountQuestions = reader.GetInt32(7),
                        TestStartDate = reader.GetString(8),
                        TestEndDate = reader.GetString(9),
                        CountPoints = reader.GetInt32(10)
                    };
                    testHeaders.Add(testHeader);
                }
                return testHeaders;
            }
            finally
            {
                CloseConnection();
            }
        }
        public TestHeadersModel PostModel(TestHeadersModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                Guid generated = Guid.NewGuid();
                cmd.CommandText = "insert into TestHeaders values (@ID,@TestID,@GroupID,@TeacherID,@TestName,@TestType,@TestTime,@CountQuestions,@TestStartDate,@TestEndDate,@CountPoints);";
                cmd.Parameters.Add(new SqliteParameter("@ID", generated.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@TestID", model.TestID.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@GroupID", model.GroupID.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@TeacherID", model.TeacherID.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@TestName", model.TestName ??= string.Empty));
                cmd.Parameters.Add(new SqliteParameter("@TestType", model.TestType ??= string.Empty));
                cmd.Parameters.Add(new SqliteParameter("@TestTime", model.TestTime ??= string.Empty));
                cmd.Parameters.Add(new SqliteParameter("@CountQuestions", model.CountQuestions));
                cmd.Parameters.Add(new SqliteParameter("@TestStartDate", model.TestStartDate ??= string.Empty));
                cmd.Parameters.Add(new SqliteParameter("@TestEndDate", model.TestEndDate ??= string.Empty));
                cmd.Parameters.Add(new SqliteParameter("@CountPoints", model.CountPoints));
                int result = cmd.ExecuteNonQuery();
                model.ID = generated;
                return model;
            }
            finally
            {
                CloseConnection();
            }
        }
        public TestHeadersModel PutModel(TestHeadersModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "update TestHeaders set TestID = @TestID, GroupID = @GroupID, TeacherID = @TeacherID, TestName = @TestName, TestType = @TestType, TestTime = @TestTime, CountQuestions = @CountQuestions, TestStartDate = @TestStartDate, TestEndDate = @TestEndDate, CountPoints = @CountPoints where ID = @ID";
                cmd.Parameters.Add(new SqliteParameter("@TestID", model.TestID.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@GroupID", model.GroupID.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@TeacherID", model.TeacherID.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@TestName", model.TestName ??= string.Empty));
                cmd.Parameters.Add(new SqliteParameter("@TestType", model.TestType ??= string.Empty));
                cmd.Parameters.Add(new SqliteParameter("@TestTime", model.TestTime ??= string.Empty));
                cmd.Parameters.Add(new SqliteParameter("@CountQuestions", model.CountQuestions));
                cmd.Parameters.Add(new SqliteParameter("@TestStartDate", model.TestStartDate ??= string.Empty));
                cmd.Parameters.Add(new SqliteParameter("@TestEndDate", model.TestEndDate ??= string.Empty));
                cmd.Parameters.Add(new SqliteParameter("@CountPoints", model.CountPoints));
                cmd.Parameters.Add(new SqliteParameter("@ID", model.ID.ToString()));
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
                cmd.CommandText = "delete from TestHeaders where ID = @ID";
                cmd.Parameters.Add(new SqliteParameter("@ID", id.ToString()));
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
                cmd.CommandText = "delete from TestHeaders";
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
