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
        private TimeLessonsModel timeLessons;
        private UserModel teacher;
        private AcademicDisciplinesModel academicDiscipline;
        private TypeClassesModel typeClass;
        private AudiencesModel audience;
        private GroupsModel group;

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
        /// ИД занятия
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

        /// <summary>
        /// Занятие
        /// </summary>
        [JsonProperty(nameof(TimeLessons))]
        public TimeLessonsModel TimeLessons { get => timeLessons; set => SetProperty(ref timeLessons, value); }

        /// <summary>
        /// Преподаватель
        /// </summary>
        [JsonProperty(nameof(Teacher))]
        public UserModel Teacher { get => teacher; set => SetProperty(ref teacher, value); }

        /// <summary>
        /// Дисциплина
        /// </summary>
        [JsonProperty(nameof(AcademicDiscipline))]
        public AcademicDisciplinesModel AcademicDiscipline { get => academicDiscipline; set => SetProperty(ref academicDiscipline, value); }

        /// <summary>
        /// Тип дисциплины
        /// </summary>
        [JsonProperty(nameof(TypeClass))]
        public TypeClassesModel TypeClass { get => typeClass; set => SetProperty(ref typeClass, value); }

        /// <summary>
        /// Аудитория
        /// </summary>
        [JsonProperty(nameof(Audience))]
        public AudiencesModel Audience { get => audience; set => SetProperty(ref audience, value); }

        /// <summary>
        /// Группа
        /// </summary>
        [JsonProperty(nameof(Group))]
        public GroupsModel Group { get => group; set => SetProperty(ref group, value); }

        #endregion
    }
}
