using ApplicationModels.Config;
using ApplicationServices.PermissionService;
using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.IO;
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

        private Visibility _canManageSchedule;
        private Visibility _canCreateScheduleRecord;
        private Visibility _canEditScheduleRecord;
        private Visibility _canSetScheduleStatus;
        private Visibility _deleteScheduleRecord;

        private Visibility _canGetScheduleComments;
        private Visibility _canCreateScheduleComments;
        private Visibility _canEditScheduleComments;
        private Visibility _canDeleteScheduleComments;

        private Visibility _canGetMaterial;
        private Visibility _canCreateMaterial;
        private Visibility _canDeleteMaterial;

        public Visibility CanManageSchedule { get => _canManageSchedule; set => SetProperty(ref _canManageSchedule, value); }
        public Visibility CanCreateScheduleRecord { get => _canCreateScheduleRecord; set => SetProperty(ref _canCreateScheduleRecord, value); }
        public Visibility CanEditScheduleRecord { get => _canEditScheduleRecord; set => SetProperty(ref _canEditScheduleRecord, value); }
        public Visibility CanSetScheduleStatus { get => _canSetScheduleStatus; set => SetProperty(ref _canSetScheduleStatus, value); }
        public Visibility CanDeleteScheduleRecord { get => _deleteScheduleRecord; set => SetProperty(ref _deleteScheduleRecord, value); }

        public Visibility CanGetScheduleComments { get => _canGetScheduleComments; set => SetProperty(ref _canGetScheduleComments, value); }
        public Visibility CanCreateScheduleComments { get => _canCreateScheduleComments; set => SetProperty(ref _canCreateScheduleComments, value); }
        public Visibility CanEditScheduleComments { get => _canEditScheduleComments; set => SetProperty(ref _canEditScheduleComments, value); }
        public Visibility CanDeleteScheduleComments { get => _canDeleteScheduleComments; set => SetProperty(ref _canDeleteScheduleComments, value); }

        public Visibility CanGetMaterial { get => _canGetMaterial; set => SetProperty(ref _canGetMaterial, value); }
        public Visibility CanCreateMaterial { get => _canCreateMaterial; set => SetProperty(ref _canCreateMaterial, value); }
        public Visibility CanDeleteMaterial { get => _canDeleteMaterial; set => SetProperty(ref _canDeleteMaterial, value); }

        #endregion
        #region Цвета в расписании

        private TypeClassColors _typeClassColors;

        public TypeClassColors TypeClassColors { get => _typeClassColors ??= new(); set => SetProperty(ref _typeClassColors, value); }

        #endregion
    }
    public static class ShceduleConfigExtension
    {
        private static string TypeClassColorsPath = Environment.CurrentDirectory + "\\ScheduleTypeClassColorsConfig.json";

        /// <summary>
        /// Установить видимость объектов конфига
        /// </summary>
        /// <param name="config">Конфиг (расширение)</param>
        /// <param name="permissionService">Сервис разрешений</param>
        public static async Task SetVisibilities(this ScheduleConfig config, IPermissionService permissionService)
        {
            config.CanManageSchedule = await permissionService.IsInPermission(PermissionNames.GetScheduleManaging) ? Visibility.Visible : Visibility.Collapsed;
            config.CanEditScheduleRecord = await permissionService.IsInPermission(PermissionNames.EditScheduleRecords) ? Visibility.Visible : Visibility.Collapsed;
            config.CanCreateScheduleRecord = await permissionService.IsInPermission(PermissionNames.CreateScheduleRecords) ? Visibility.Visible : Visibility.Collapsed;
            config.CanSetScheduleStatus = await permissionService.IsInPermission(PermissionNames.SetScheduleStatus) ? Visibility.Visible : Visibility.Collapsed;
            config.CanDeleteScheduleRecord = await permissionService.IsInPermission(PermissionNames.DeleteScheduleRecord) ? Visibility.Visible : Visibility.Collapsed;

            config.CanGetScheduleComments = await permissionService.IsInPermission(PermissionNames.GetScheduleComments) ? Visibility.Visible : Visibility.Collapsed;
            config.CanCreateScheduleComments = await permissionService.IsInPermission(PermissionNames.CreateScheduleComments) ? Visibility.Visible : Visibility.Collapsed;
            config.CanEditScheduleComments = await permissionService.IsInPermission(PermissionNames.EditScheduleComments) ? Visibility.Visible : Visibility.Collapsed;
            config.CanDeleteScheduleComments = await permissionService.IsInPermission(PermissionNames.DeleteScheduleComments) ? Visibility.Visible : Visibility.Collapsed;

            config.CanGetMaterial = await permissionService.IsInPermission(PermissionNames.GetFile) ? Visibility.Visible : Visibility.Collapsed;
            config.CanCreateMaterial = await permissionService.IsInPermission(PermissionNames.PushFile) ? Visibility.Visible : Visibility.Collapsed;
            config.CanDeleteMaterial = await permissionService.IsInPermission(PermissionNames.DeleteFile) ? Visibility.Visible : Visibility.Collapsed;
        }
        public static async Task ReadColors(this ScheduleConfig config)
        {
            using StreamReader reader = new(TypeClassColorsPath);
            string buffer = await reader.ReadToEndAsync();
            TypeClassColors colors = JsonConvert.DeserializeObject<TypeClassColors>(buffer);
            config.TypeClassColors = colors;
        }
        public static async Task WriteColors(this ScheduleConfig config)
        {
            string json = JsonConvert.SerializeObject(config.TypeClassColors, Formatting.Indented);
            using StreamWriter writer = new(TypeClassColorsPath);
            await writer.WriteAsync(json);
        }
    }
}
