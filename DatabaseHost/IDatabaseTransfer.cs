using System.Collections.Generic;
using System.ServiceModel;
using DatabaseModels;

namespace DatabaseHost
{
    [ServiceContract]
    public interface IDatabaseTransfer
    {
        #region users

        [OperationContract]
        IEnumerable<UserModel> GetUsers();

        [OperationContract]
        UserModel GetUserByLogin(string login);

        #endregion
    }
}
