using EdlightApiServer.Services.HashingService;
using Microsoft.AspNetCore.Mvc;
using ServerModels;
using System;

namespace EdlightApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IHashingService hashing;
        private static readonly string FilesFolderPath = Environment.CurrentDirectory + "\\Files\\";
        private static readonly string PlansFolderPath = Environment.CurrentDirectory + "\\Plans\\";

        public FilesController([FromServices] IHashingService hashing) => this.hashing = hashing;

        [HttpGet]
        public object GetFile([FromHeader] string Path, [FromHeader] string IsPlanFile)
        {
            if (string.IsNullOrEmpty(Path)) return BadRequest("Параметры запроса заданы неверно");

            Path = hashing.DecodeString(Path);

            if (!System.IO.File.Exists((IsPlanFile == true.ToString() ? PlansFolderPath : FilesFolderPath) + Path)) return NotFound("Запрашиваемый файл не найден");

            byte[] data = System.IO.File.ReadAllBytes((IsPlanFile == true.ToString() ? PlansFolderPath : FilesFolderPath) + Path);

            JsonFileModel jsonFile = new() { FileName = "Loaded", Data = data };

            return jsonFile;
        }
        [HttpPost]
        public object PostFile([FromHeader] string Path, [FromHeader] string IsPlanFile, [FromBody] JsonFileModel fileModel)
        {
            if (string.IsNullOrEmpty(Path) || fileModel == null) return BadRequest("Параметры запроса заданы неверно");

            try
            {
                string file_path = (IsPlanFile == true.ToString() ? PlansFolderPath : FilesFolderPath);
                if (Path != "/")
                {
                    System.IO.Directory.CreateDirectory(file_path + Path);
                    file_path += Path;
                }
                System.IO.File.WriteAllBytes(file_path + @"/" + fileModel.FileName, fileModel.Data);
            }
            catch (Exception ex)
            {
                return BadRequest("При попытке записи файла произошла ошибка : " + ex.Message);
            }

            return Ok("Файл успешно сохранен");
        }
        [HttpDelete]
        public object DeleteFile([FromBody] JsonFileModel fileModel, [FromHeader] string IsPlanFile)
        {
            if (fileModel == null) return BadRequest("Параметры запроса заданы неверно");

            //System.Text.RegularExpressions.Regex regex = new(@"\w*\\\w*\.\w*$");
            System.Text.RegularExpressions.Regex regex = new(@"\w*\.\w*$");

            if (regex.IsMatch(fileModel.FileName))
            {
                if (!System.IO.File.Exists((IsPlanFile == true.ToString() ? PlansFolderPath : FilesFolderPath) + fileModel.FileName)) return NotFound("Запрашиваемый файл не найден");

                try
                {
                    System.IO.File.Delete((IsPlanFile == true.ToString() ? PlansFolderPath : FilesFolderPath) + fileModel.FileName);
                }
                catch (Exception ex)
                {
                    return BadRequest("При попытке удаления файла произошла ошибка : " + ex.Message);
                }

                return Ok("Файл успешно удален");
            }
            else
            {
                if (!System.IO.Directory.Exists((IsPlanFile == true.ToString() ? PlansFolderPath : FilesFolderPath) + fileModel.FileName)) return NotFound("Запрашиваемая папка не найдена");

                try
                {
                    System.IO.Directory.Delete((IsPlanFile == true.ToString() ? PlansFolderPath : FilesFolderPath) + fileModel.FileName, true);
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
