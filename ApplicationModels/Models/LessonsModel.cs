using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace ApplicationModels.Models
{
    public class LessonsModel : BindableBase
    {
        #region fields

        private Guid id;
        private DateTime day;
        private Guid idTimeLessons;
        private Guid idTeacher;
        private Guid idAcademicDiscipline;
        private Guid idTypeClass;
        private Guid idAudience;
        private Guid idGroup;

        #endregion
        #region props

        /// <summary>
        /// ИД
        /// </summary>
        [JsonProperty(nameof(Id))]
        public Guid Id { get => id; set => SetProperty(ref id, value); }

        /// <summary>
        /// Дата занятия
        /// </summary>
        [JsonProperty(nameof(Day))]
        public DateTime Day { get => day; set => SetProperty(ref day, value); }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(nameof(IdTimeLessons))]
        public Guid IdTimeLessons { get => idTimeLessons; set => SetProperty(ref idTimeLessons, value); }

        /// <summary>
        /// ИД преподавателя
        /// </summary>
        [JsonProperty(nameof(IdTeacher))]
        public Guid IdTeacher { get => idTeacher; set => SetProperty(ref idTeacher, value); }

        /// <summary>
        /// ИД дисциплины
        /// </summary>
        [JsonProperty(nameof(IdAcademicDiscipline))]
        public Guid IdAcademicDiscipline { get => idAcademicDiscipline; set => SetProperty(ref idAcademicDiscipline, value); }

        /// <summary>
        /// ИД типа дисциплины
        /// </summary>
        [JsonProperty(nameof(IdTypeClass))]
        public Guid IdTypeClass { get => idTypeClass; set => SetProperty(ref idTypeClass, value); }

        /// <summary>
        /// ИД аудитории
        /// </summary>
        [JsonProperty(nameof(IdAudience))]
        public Guid IdAudience { get => idAudience; set => SetProperty(ref idAudience, value); }

        /// <summary>
        /// ИД группы
        /// </summary>
        [JsonProperty(nameof(IdGroup))]
        public Guid IdGroup { get => idGroup; set => SetProperty(ref idGroup, value); }

        #endregion
    }
}
