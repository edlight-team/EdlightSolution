using ApplicationServices.PermissionService;
using Prism.Mvvm;
using System.Threading.Tasks;
using System.Windows;

namespace EdlightDesktopClient.AccessConfigurations
{
    /// <summary>
    /// Конфигурация основной вьюхи
    /// </summary>
    public class MainConfig : BindableBase
    {
        private Visibility _canManageGroups;
        private Visibility _canManageDictionaries;

        public Visibility CanManageGroups { get => _canManageGroups; set => SetProperty(ref _canManageGroups, value); }
        public Visibility CanManageDictionaries { get => _canManageDictionaries; set => SetProperty(ref _canManageDictionaries, value); }
    }
    public static class MainConfigExtension
    {
        /// <summary>
        /// Установить видимость объектов конфига
        /// </summary>
        /// <param name="config">Конфиг (расширение)</param>
        /// <param name="permissionService">Сервис разрешений</param>
        public static async Task SetVisibilities(this MainConfig config, IPermissionService permission)
        {
            config.CanManageGroups = await permission.IsInPermission(PermissionNames.ManageGroups) ? Visibility.Visible : Visibility.Collapsed;
            config.CanManageDictionaries = await permission.IsInPermission(PermissionNames.ManageDictionaries) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
