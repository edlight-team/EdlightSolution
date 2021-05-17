using System;

namespace ApplicationServices.PermissionService
{
    #region Атрибут

    /// <summary>
    /// Атрибут описания разрешения
    /// </summary>
    public class PermissionDescription : Attribute
    {
        /// <summary>
        /// Описание разрешения
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Атрибут устанавливает описание разрешения
        /// </summary>
        /// <param name="description">Описание</param>
        public PermissionDescription(string description) => Description = description;
    }

    #endregion
    #region Разрешения

    public static class PermissionNames
    {
        #region Записи расписания

        /// <summary>
        /// Создать записи расписания
        /// </summary>
        [PermissionDescription("Создать записи расписания")]
        public const string CreateScheduleRecords = nameof(CreateScheduleRecords);

        /// <summary>
        /// Установить статус записи расписания (Обычный / Отменен)
        /// </summary>
        [PermissionDescription("Установить статус записи расписания (Обычный / Отменен)")]
        public const string SetScheduleStatus = nameof(SetScheduleStatus);

        /// <summary>
        /// Удалить запись расписания
        /// </summary>
        [PermissionDescription("Удалить запись расписания")]
        public const string DeleteScheduleRecord = nameof(DeleteScheduleRecord);

        #endregion
        #region Комменты расписания

        /// <summary>
        /// Получить комментарии в записи расписания
        /// </summary>
        [PermissionDescription("Получить комментарии в записи расписания")]
        public const string GetScheduleComments = nameof(GetScheduleComments);

        /// <summary>
        /// Создавать коментарии в записи расписания
        /// </summary>
        [PermissionDescription("Создавать коментарии в записи расписания")]
        public const string CreateScheduleComments = nameof(CreateScheduleComments);

        /// <summary>
        /// Изменять коментарии в записи расписания
        /// </summary>
        [PermissionDescription("Изменять коментарии в записи расписания")]
        public const string EditScheduleComments = nameof(EditScheduleComments);

        /// <summary>
        /// Удалять коментарии в записи расписания
        /// </summary>
        [PermissionDescription("Удалять коментарии в записи расписания")]
        public const string DeleteScheduleComments = nameof(DeleteScheduleComments);

        #endregion
    } 

    #endregion
}
