using System;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using ApplicationModels.Models;
using EdlightDBConnector;
using EdlightDBConnector.DBModelsExecuter;
using Microsoft.AspNetCore.Mvc;

namespace EdlightWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET api/<UsersController>/5
        [HttpGet("login={login}&auth_token={AuthToken}")]
        public object Get(string login, string AuthToken)
        {
            if (AuthToken != "5B6253853ACCF8B8E4FEE1F67C46D")
            {
                return BadRequest("Неверный токен авторизации");
            }
            DBConnector dbConnector = new DBConnector();
            var connection = dbConnector.CreateConnection(Environment.CurrentDirectory + "\\EdlightDB.accdb", doOpenConnection: true);
            DBUsersExecuter dBUsers = new DBUsersExecuter();
            var users = dBUsers.GetUsers(connection);
            var userByLogin = users.FirstOrDefault(u => u.Login == login);
            if (userByLogin is null)
            {
                return NotFound("Пользователь не найден");
            }
            return userByLogin;
        }

        [HttpPost]
        public int PostUser([FromBody] UserModel user)
        {
            try
            {

            }
            catch (Exception)
            {
                return 0;
            }
            return 1;
        }
    }
}
