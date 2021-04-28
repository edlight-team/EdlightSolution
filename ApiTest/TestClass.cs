using ApplicationServices.WebApiService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ApiTest
{
    [TestClass]
    public class TestClass
    {
        private readonly IWebApiService api;
        public TestClass() => api = new WebApiServiceImplementation();
        [TestMethod]
        public async void TestUsers()
        {
            //api.GetModels()
        }
    }
}
