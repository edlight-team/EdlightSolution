﻿using Microsoft.AspNetCore.Mvc;
using ServerModels.Models;
using SqliteDataExecuter.Entities;
using System;
using System.Linq;

namespace EdlightApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageFilesController : ControllerBase
    {
        private readonly StorageFiles db;
        public StorageFilesController() => db = new();

        [HttpGet]
        public object Get([FromHeader] string id, [FromHeader] string condition)
        {
            try
            {
                if (!string.IsNullOrEmpty(condition))
                {
                    return db.GetModels(condition);
                }
                if (!string.IsNullOrEmpty(id))
                {
                    StorageFilesModel model = db.GetModels().FirstOrDefault(m => m.ID == new Guid(id));
                    if (model == null) return NotFound("Результат теста не найден");
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
        public object Post([FromBody] StorageFilesModel model)
        {
            try
            {
                StorageFilesModel posted = db.PostModel(model);
                return posted;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Произошла ошибка при добавлении данных. " + ex.Message);
            }
        }
        [HttpPut]
        public object Put([FromBody] StorageFilesModel model)
        {
            try
            {
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
