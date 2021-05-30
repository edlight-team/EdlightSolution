using Microsoft.Data.Sqlite;
using ServerModels.Models;
using System;
using System.Collections.Generic;

namespace SqliteDataExecuter.Entities
{
    public class TestResults : DBConnector
    {
        public IEnumerable<TestResultsModel> GetModels(string condition)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from TestResults where " + condition + ";";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<TestResultsModel> testResults = new();
                while (reader.Read())
                {
                    TestResultsModel testResult = new()
                    {
                        ID = new Guid(reader.GetString(0)),
                        TestID = new Guid(reader.GetString(1)),
                        UserID = new Guid(reader.GetString(2)),
                        StudentName = reader.GetString(3),
                        StudentSurname = reader.GetString(4),
                        CorrectAnswers = reader.GetInt32(5),
                        TestCompleted = reader.GetInt32(6) == 1,
                    };
                    testResults.Add(testResult);
                }
                return testResults;
            }
            finally
            {
                CloseConnection();
            }
        }
        public IEnumerable<TestResultsModel> GetModels()
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from TestResults;";
                SqliteDataReader reader = cmd.ExecuteReader();
                List<TestResultsModel> testResults = new();
                while (reader.Read())
                {
                    TestResultsModel testResult = new()
                    {
                        ID = new Guid(reader.GetString(0)),
                        TestID = new Guid(reader.GetString(1)),
                        UserID = new Guid(reader.GetString(2)),
                        StudentName = reader.GetString(3),
                        StudentSurname = reader.GetString(4),
                        CorrectAnswers = reader.GetInt32(5),
                        TestCompleted = reader.GetInt32(6) == 1,
                    };
                    testResults.Add(testResult);
                }
                return testResults;
            }
            finally
            {
                CloseConnection();
            }
        }
        public TestResultsModel PostModel(TestResultsModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                Guid generated = Guid.NewGuid();
                cmd.CommandText = "insert into TestResults values (@ID,@TestID,@UserID,@StudentName,@StudentSurname,@CorrectAnswers,@TestCompleted);";
                cmd.Parameters.Add(new SqliteParameter("@ID", generated.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@TestID", model.TestID.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@UserID", model.UserID.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@StudentName", model.StudentName));
                cmd.Parameters.Add(new SqliteParameter("@StudentSurname", model.StudentSurname));
                cmd.Parameters.Add(new SqliteParameter("@CorrectAnswers", model.CorrectAnswers));
                cmd.Parameters.Add(new SqliteParameter("@TestCompleted", model.TestCompleted ? 1 : 0));
                int result = cmd.ExecuteNonQuery();
                model.ID = generated;
                return model;
            }
            finally
            {
                CloseConnection();
            }
        }
        public TestResultsModel PutModel(TestResultsModel model)
        {
            try
            {
                SqliteConnection cnn = CreateConnection(DBPath());
                SqliteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "update TestResults set TestID = @TestID, UserID = @UserID, StudentName = @StudentName, StudentSurname = @StudentSurname, CorrectAnswers = @CorrectAnswers, TestCompleted = @TestCompleted where ID = @ID";
                cmd.Parameters.Add(new SqliteParameter("@TestID", model.TestID.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@UserID", model.UserID.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@StudentName", model.StudentName));
                cmd.Parameters.Add(new SqliteParameter("@StudentSurname", model.StudentSurname));
                cmd.Parameters.Add(new SqliteParameter("@CorrectAnswers", model.CorrectAnswers));
                cmd.Parameters.Add(new SqliteParameter("@TestCompleted", model.TestCompleted ? 1 : 0));
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
                cmd.CommandText = "delete from TestResults where ID = @ID";
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
                cmd.CommandText = "delete from TestResults";
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
