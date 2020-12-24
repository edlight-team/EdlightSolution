using System;
using System.Collections.Generic;
using DatabaseModels;

namespace DatabaseHost
{
    public class DatabaseTransfer : IDatabaseTransfer
    {
        public UserModel GetUserByLogin(string login)
        {
            return new UserModel
            {
                ID = Guid.NewGuid(),
                Login = "admin",
                Password = "admin"
            };
        }
        public IEnumerable<UserModel> GetUsers() => throw new System.NotImplementedException();
    }
}
