﻿using Microsoft.AspNetCore.Mvc;
using ServerModels.Models;
using SqliteDataExecuter.Entities;
using System;
using System.Linq;

namespace EdlightApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DialogsController : ControllerBase
    {
        private readonly Dialogs db;
        public DialogsController() => db = new();

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
                    DialogsModel model = db.GetModels().FirstOrDefault(m => m.Id == new Guid(id));
                    if (model == null) return NotFound("Диалог не найден в БД");
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
        public object Post([FromBody] DialogsModel model)
        {
            try { return db.PostModel(model); }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Произошла ошибка при добавлении данных. " + ex.Message);
            }
        }
        [HttpPut]
        public object Put([FromBody] DialogsModel model)
        {
            try
            {
                if (!db.GetModels().Any(u => u.Id == model.Id)) return NotFound($"Диалог не найден в БД");
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
