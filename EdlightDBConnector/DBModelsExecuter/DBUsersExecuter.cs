using ApplicationModels.Models;
using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace EdlightDBConnector.DBModelsExecuter
{
    public class DBUsersExecuter
    {
        public UserModel GetUser(OdbcConnection connection, Guid ID)
        {
            return null;
        }
        public IEnumerable<UserModel> GetUsers(OdbcConnection connection)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Users; ";
            var reader = cmd.ExecuteReader();
            List<UserModel> users = new List<UserModel>();
            while (reader.Read())
            {
                UserModel user = new UserModel
                {
                    ID = new Guid(reader.GetValue(0).ToString()),
                    Login = reader.GetValue(1).ToString(),
                    Password = reader.GetValue(2).ToString(),
                    Name = reader.GetValue(3).ToString(),
                    Surname = reader.GetValue(4).ToString(),
                    Patrnymic = reader.GetValue(5).ToString(),
                    Sex = reader.GetInt32(6),
                    Age = reader.GetInt32(7)
                };
                users.Add(user);
            }
            return users;
        }
    }
}
