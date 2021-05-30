using Microsoft.AspNetCore.Mvc;
using ServerModels.Models;
using SqliteDataExecuter.Entities;
using System;
using System.Linq;

namespace EdlightApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Users db;
        public UsersController() => db = new();

        [HttpGet]
        public object Get([FromHeader] string login, [FromHeader] string condition)
        {
            try
            {
                if (!string.IsNullOrEmpty(condition))
                {
                    return db.GetModels(condition);
                }
                if (!string.IsNullOrEmpty(login))
                {
                    UserModel model = db.GetModels().FirstOrDefault(m => m.Login == login);
                    if (model == null) return NotFound("Пользователь не найден");
                    else return model;
                }
                else
                    return db.GetModels();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Произошла ошибка при получении данных. " + ex.Message);
            }
        }
        [HttpPost]
        public object Post([FromBody] UserModel model)
        {
            try
            {
                if (db.GetModels().Any(u => u.Login == model.Login)) return BadRequest($"Пользователь с логином {model.Login} уже имеется в БД");
                UserModel posted = db.PostModel(model);
                return posted;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Произошла ошибка при добавлении данных. " + ex.Message);
            }
        }
        [HttpPut]
        public object Put([FromBody] UserModel model)
        {
            try
            {
                if (!db.GetModels().Any(u => u.ID == model.ID)) return NotFound($"Пользователь не найден в БД");
                return db.PutModel(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Произошла ошибка при обновлении данных. " + ex.Message);
            }
        }
        [HttpDelete]
        public object Delete([FromHeader] string id)
        {
            try { return db.DeleteModel(new Guid(id)); }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Произошла ошибка при удалении данных. " + ex.Message);
            }
        }
    }
}
