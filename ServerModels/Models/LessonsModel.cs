using Newtonsoft.Json;
using System;

namespace ServerModels.Models
{
    public class LessonsModel
    {
        #region props

        /// <summary>
        /// ИД
        /// </summary>
        [JsonProperty(nameof(Id))]
        public Guid Id { get; set; }

        /// <summary>
        /// Дата занятия
        /// </summary>
        [JsonProperty(nameof(Day))]
        public DateTime Day { get; set; }

        /// <summary>
        /// ИД занятий
        /// </summary>
        [JsonProperty(nameof(IdTimeLessons))]
        public Guid IdTimeLessons { get; set; }

        /// <summary>
        /// ИД преподавателя
        /// </summary>
        [JsonProperty(nameof(IdTeacher))]
        public Guid IdTeacher { get; set; }

        /// <summary>
        /// ИД дисциплины
        /// </summary>
        [JsonProperty(nameof(IdAcademicDiscipline))]
        public Guid IdAcademicDiscipline { get; set; }

        /// <summary>
        /// ИД типа дисциплины
        /// </summary>
        [JsonProperty(nameof(IdTypeClass))]
        public Guid IdTypeClass { get; set; }

        /// <summary>
        /// ИД аудитории
        /// </summary>
        [JsonProperty(nameof(IdAudience))]
        public Guid IdAudience { get; set; }

        /// <summary>
        /// ИД группы
        /// </summary>
        [JsonProperty(nameof(IdGroup))]
        public Guid IdGroup { get; set; }

        /// <summary>
        /// Причина отмены занятия
        /// </summary>
        [JsonProperty(nameof(CanceledReason))]
        public string CanceledReason { get; set; }

        #endregion
    }
}
