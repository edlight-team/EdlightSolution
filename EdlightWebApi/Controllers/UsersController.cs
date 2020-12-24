using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EdlightWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UsersController>/5
        [HttpGet("login={login}&auth_token={AuthToken}")]
        public object Get(string login, string AuthToken)
        {
            if (AuthToken != "5B6253853ACCF8B8E4FEE1F67C46D")
            {
                return BadRequest("Неверный токен авторизации");
            }
            IMongoCollection<UserModel> collection = GetMongoCollection();
            UserModel user = collection.Find(u => u.Login == login).FirstOrDefault();
            if (user is null)
            {
                return NotFound("Пользователь не найден");
            }
            return user;
        }

        [HttpPost]
        public int PostUser([FromBody] UserModel user)
        {
            IMongoCollection<UserModel> collection = GetMongoCollection();
            try
            {
                collection.InsertOne(user);
            }
            catch (Exception)
            {
                return 0;
            }
            return 1;
        }

        private IMongoCollection<UserModel> GetMongoCollection()
        {
            string connectionString = "mongodb://localhost:27017";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase("EdlightDB");
            return database.GetCollection<UserModel>("Users");
        }
    }
}
