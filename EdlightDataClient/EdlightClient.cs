using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using DatabaseHost;
using DatabaseModels;

namespace EdlightDataClient
{
    public class EdlightClient : ClientBase<IDatabaseTransfer>, IDatabaseTransfer
    {
        public EdlightClient(Binding binding, EndpointAddress address) : base(binding, address) { }

        public UserModel GetUserByLogin(string login) => Channel.GetUserByLogin(login);
        public IEnumerable<UserModel> GetUsers() => Channel.GetUsers();
    }
}
