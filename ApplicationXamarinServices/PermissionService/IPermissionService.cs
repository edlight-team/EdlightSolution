using ApplicationModels.Models;
using ApplicationServices.WebApiService;
using System.Threading.Tasks;

namespace ApplicationXamarinServices.PermissionService
{
    public interface IPermissionService
    {
        /// <summary>
        /// Конфигурация сервиса
        /// </summary>
        /// <param name="api">Апи</param>
        /// <param name="currentUser">Текущий пользователь</param>
        /// <returns></returns>
        Task ConfigureService(IWebApiService api, UserModel currentUser);
        /// <summary>
        /// Проверка пользователь находится в роли
        /// </summary>
        /// <param name="role">Роль</param>
        /// <returns>True если у пользователя совпадает роль</returns>
        Task<bool> IsInRole(RolesModel role);
        /// <summary>
        /// Проверка у роли разрешения
        /// </summary>
        /// <param name="role">Роль</param>
        /// <param name="permission">Разрешение</param>
        /// <returns>True если у роли есть разрешение</returns>
        Task<bool> IsInPermission(RolesModel role, PermissionsModel permission);
        /// <summary>
        /// Получить роль из загруженных по имени
        /// </summary>
        /// <returns>Роль</returns>
        Task<RolesModel> GetRoleByName(string name);
    }
}
