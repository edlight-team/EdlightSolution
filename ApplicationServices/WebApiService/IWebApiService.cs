using ApplicationModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationServices.WebApiService
{
    public interface IWebApiService
    {
        Task<List<TData>> GetModels<TData>(string apiName, string condition);
        Task<List<TData>> GetModels<TData>(string apiName);
        Task<TData> PostModel<TData>(TData item, string apiName);
        Task<TData> PutModel<TData>(TData item, string apiName);
        Task<int> DeleteModel(Guid id, string apiName);
        Task<string> DeleteAll(string Target = null);

        /// <summary>
        /// Получить файл с сервера
        /// </summary>
        /// <param name="path">Путь к файлу (относительно корневой папки на сервере)</param>
        /// <returns>Json модель формата { "FileName" : "Имя файла", "Data" : "Набор байт файла" }</returns>
        Task<object> GetFile(string path);
        /// <summary>
        /// Запушить файл на сервер
        /// </summary
        /// <param name="path">Путь к файлу (относительно корневой папки на сервере)</param>
        /// <param name="FileModel">Json модель формата { "FileName" : "Имя файла", "Data" : "Набор байт файла" }</param>
        /// <returns></returns>
        Task<string> PushFile(string path, JsonFileModel FileModel);
        /// <summary>
        /// Удалить файл с сервера
        /// </summary>
        /// <param name="path">Путь к файлу (относительно корневой папки на сервере)</param>
        /// <returns></returns>
        Task<string> DeleteFile(string path);

        /// <summary>
        /// Получить учебный план с сервера
        /// </summary>
        /// <param name="path">Путь к плану (относительно корневой папки на сервере)</param>
        /// <returns>Json модель формата { "FileName" : "Имя файла", "Data" : "Набор байт файла" }</returns>
        Task<object> GetLearnPlan(string path);
        /// <summary>
        /// Запушить план на сервер
        /// </summary>
        /// <param name="path">Путь к плану (относительно корневой папки на сервере)</param>
        /// <param name="FileModel">Json модель формата { "FileName" : "Имя файла", "Data" : "Набор байт файла" }</param>
        /// <returns></returns>
        Task<string> PushLearnPlan(string path, JsonFileModel FileModel);
        /// <summary>
        /// Удалить план с сервера
        /// </summary>
        /// <param name="path">Путь к плану (относительно корневой папки на сервере)</param>
        /// <returns></returns>
        Task<string> DeleteLearnPlan(string path);
    }
}
