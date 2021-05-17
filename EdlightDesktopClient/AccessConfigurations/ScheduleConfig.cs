using ApplicationServices.PermissionService;
using Prism.Mvvm;
using System.Threading.Tasks;
using System.Windows;

namespace EdlightDesktopClient.AccessConfigurations
{
    /// <summary>
    /// Конфигурация сетки расписания
    /// </summary>
    public class ScheduleConfig : BindableBase
    {
        #region Видимость кнопок / Контролов

        private Visibility _canCreateScheduleRecord;
        private Visibility _canSetScheduleStatus;
        private Visibility _deleteScheduleRecord;

        private Visibility _canGetScheduleComments;
        private Visibility _canCreateScheduleComments;
        private Visibility _canEditScheduleComments;
        private Visibility _canDeleteScheduleComments;

        public Visibility CanCreateScheduleRecord { get => _canCreateScheduleRecord; set => SetProperty(ref _canCreateScheduleRecord, value); }
        public Visibility CanSetScheduleStatus { get => _canSetScheduleStatus; set => SetProperty(ref _canSetScheduleStatus, value); }
        public Visibility DeleteScheduleRecord { get => _deleteScheduleRecord; set => SetProperty(ref _deleteScheduleRecord, value); }

        public Visibility CanGetScheduleComments { get => _canGetScheduleComments; set => SetProperty(ref _canGetScheduleComments, value); }
        public Visibility CanCreateScheduleComments { get => _canCreateScheduleComments; set => SetProperty(ref _canCreateScheduleComments, value); }
        public Visibility CanEditScheduleComments { get => _canEditScheduleComments; set => SetProperty(ref _canEditScheduleComments, value); }
        public Visibility CanDeleteScheduleComments { get => _canDeleteScheduleComments; set => SetProperty(ref _canDeleteScheduleComments, value); }

        #endregion
        #region Инициализация

        /// <summary>
        /// Создать конфиг с помощью сервиса
        /// </summary>
        /// <param name="permissionService">Сервис проверки разрешений</param>
        /// <returns>Конфиг расписания</returns>
        public static async Task<ScheduleConfig> InitializeByPermissionService(IPermissionService permissionService)
        {
            ScheduleConfig config = new();

            config.CanCreateScheduleRecord = await permissionService.IsInPermission(PermissionNames.CreateScheduleRecords) ? Visibility.Visible : Visibility.Collapsed;
            config.CanSetScheduleStatus = await permissionService.IsInPermission(PermissionNames.SetScheduleStatus) ? Visibility.Visible : Visibility.Collapsed;
            config.DeleteScheduleRecord = await permissionService.IsInPermission(PermissionNames.DeleteScheduleRecord) ? Visibility.Visible : Visibility.Collapsed;

            config.CanGetScheduleComments = await permissionService.IsInPermission(PermissionNames.GetScheduleComments) ? Visibility.Visible : Visibility.Collapsed;
            config.CanCreateScheduleComments = await permissionService.IsInPermission(PermissionNames.CreateScheduleComments) ? Visibility.Visible : Visibility.Collapsed;
            config.CanEditScheduleComments = await permissionService.IsInPermission(PermissionNames.EditScheduleComments) ? Visibility.Visible : Visibility.Collapsed;
            config.CanDeleteScheduleComments = await permissionService.IsInPermission(PermissionNames.DeleteScheduleComments) ? Visibility.Visible : Visibility.Collapsed;

            return config;
        } 

        #endregion
    }
}
