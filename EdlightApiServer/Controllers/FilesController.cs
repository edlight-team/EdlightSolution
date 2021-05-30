using Microsoft.AspNetCore.Mvc;
using ServerModels;
using System;

namespace EdlightApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private static readonly string FilesFolderPath = Environment.CurrentDirectory + "\\Files\\";

        [HttpGet]
        public object GetFile([FromHeader] string Path)
        {
            if (string.IsNullOrEmpty(Path)) return BadRequest("Параметры запроса заданы неверно");

            if (!System.IO.File.Exists(FilesFolderPath + Path)) return NotFound("Запрашиваемый файл не найден");

            byte[] data = System.IO.File.ReadAllBytes(FilesFolderPath + Path);

            JsonFileModel jsonFile = new() { FileName = "Loaded", Data = data };

            return jsonFile;
        }
        [HttpPost]
        public object PostFile([FromHeader] string Path, [FromBody] JsonFileModel fileModel)
        {
            if (string.IsNullOrEmpty(Path) || fileModel == null) return BadRequest("Параметры запроса заданы неверно");

            try
            {
                System.IO.Directory.CreateDirectory(FilesFolderPath + Path);

                System.IO.File.WriteAllBytes(FilesFolderPath + Path + fileModel.FileName, fileModel.Data);
            }
            catch (Exception ex)
            {
                return BadRequest("При попытке записи файла произошла ошибка : " + ex.Message);
            }

            return Ok("Файл успешно сохранен");
        }
        [HttpDelete]
        public object DeleteFile([FromHeader] string Path)
        {
            if (string.IsNullOrEmpty(Path)) return BadRequest("Параметры запроса заданы неверно");

            System.Text.RegularExpressions.Regex regex = new(@"\w*\\\w*\.\w*$");

            if (regex.IsMatch(Path))
            {
                if (!System.IO.File.Exists(FilesFolderPath + Path)) return NotFound("Запрашиваемый файл не найден");

                try
                {
                    System.IO.File.Delete(FilesFolderPath + Path);
                }
                catch (Exception ex)
                {
                    return BadRequest("При попытке удаления файла произошла ошибка : " + ex.Message);
                }

                return Ok("Файл успешно удален");
            }
            else
            {
                if (!System.IO.Directory.Exists(FilesFolderPath + Path)) return NotFound("Запрашиваемая папка не найдена");

                try
                {
                    System.IO.Directory.Delete(FilesFolderPath + Path, true);
                }
                catch (Exception ex)
                {
                    return BadRequest("При попытке удаления папки произошла ошибка : " + ex.Message);
                }

                return Ok("Папка успешно удалена");
            }
        }
    }
}
