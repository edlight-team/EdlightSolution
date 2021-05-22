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
        #region Основные вкладки

        /// <summary>
        /// Управлять группами
        /// </summary>
        [PermissionDescription("Управлять группами")]
        public const string ManageGroups = nameof(ManageGroups);

        /// <summary>
        /// Управлять справочниками
        /// </summary>
        [PermissionDescription("Управлять справочниками")]
        public const string ManageDictionaries = nameof(ManageDictionaries);

        #endregion
        #region Записи расписания

        /// <summary>
        /// Управлять расписанием
        /// </summary>
        [PermissionDescription("Управлять расписанием")]
        public const string GetScheduleManaging = nameof(GetScheduleManaging);

        /// <summary>
        /// Создать записи расписания
        /// </summary>
        [PermissionDescription("Создать записи расписания")]
        public const string CreateScheduleRecords = nameof(CreateScheduleRecords);

        /// <summary>
        /// Редактировать запись расписания
        /// </summary>
        [PermissionDescription("Редактировать запись расписания")]
        public const string EditScheduleRecords = nameof(EditScheduleRecords);

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
        #region Файлы

        /// <summary>
        /// Получить файл с сервера
        /// </summary>
        [PermissionDescription("Получить файл с сервера")]
        public const string GetFile = nameof(GetFile);

        /// <summary>
        /// Добавить файл на сервер
        /// </summary>
        [PermissionDescription("Добавить файл на сервер")]
        public const string PushFile = nameof(PushFile);

        /// <summary>
        /// Удалить файл на сервере
        /// </summary>
        [PermissionDescription("Удалить файл на сервере")]
        public const string DeleteFile = nameof(DeleteFile);

        #endregion
        #region Тесты

        /// <summary>
        /// Создать тест
        /// </summary>
        [PermissionDescription("Создать тест")]
        public const string CreateTestRecords = nameof(CreateTestRecords);

        /// <summary>
        /// Изменить тест
        /// </summary>
        [PermissionDescription("Изменить тест")]
        public const string UpdateTestRecord = nameof(UpdateTestRecord);

        /// <summary>
        /// Удалить тест
        /// </summary>
        [PermissionDescription("Удалить тест")]
        public const string DeleteTestRecord = nameof(DeleteTestRecord);

        /// <summary>
        /// Установить фильтер тестов
        /// </summary>
        [PermissionDescription("Установить фильтер тестов")]
        public const string SetTestFilter = nameof(SetTestFilter);

        /// <summary>
        /// Пройти тест
        /// </summary>
        [PermissionDescription("Пройти тест")]
        public const string TakeTest = nameof(TakeTest);

        /// <summary>
        /// Просмотреть результаты теста всех студентов
        /// </summary>
        [PermissionDescription("Просмотреть результаты теста всех студентов")]
        public const string ViewStudentTestResults = nameof(ViewStudentTestResults);

        /// <summary>
        /// Просмотреть свои результаты теста
        /// </summary>
        [PermissionDescription("Просмотреть свои результаты теста")]
        public const string ViewSelfTestResults = nameof(ViewSelfTestResults);

        #endregion
    }

    #endregion
}
