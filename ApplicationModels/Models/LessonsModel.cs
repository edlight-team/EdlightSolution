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
        private string canceledReason;
        private int recoursiveId;
        private bool isSelected;
        private bool isCanceled;

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
        /// Причина отмены занятия
        /// </summary>
        [JsonProperty(nameof(CanceledReason))]
        public string CanceledReason { get => canceledReason ??= string.Empty; set => SetProperty(ref canceledReason, value); }

        /// <summary>
        /// ИД если занятие относится к рекурсии
        /// </summary>
        [JsonProperty(nameof(RecoursiveId))]
        public int RecoursiveId { get => recoursiveId; set => SetProperty(ref recoursiveId, value); }

        /// <summary>
        /// Занятие
        /// </summary>
        [JsonIgnore]
        public TimeLessonsModel TimeLessons { get => timeLessons; set => SetProperty(ref timeLessons, value); }

        /// <summary>
        /// Преподаватель
        /// </summary>
        [JsonIgnore]
        public UserModel Teacher { get => teacher; set => SetProperty(ref teacher, value); }

        /// <summary>
        /// Дисциплина
        /// </summary>
        [JsonIgnore]
        public AcademicDisciplinesModel AcademicDiscipline { get => academicDiscipline; set => SetProperty(ref academicDiscipline, value); }

        /// <summary>
        /// Тип дисциплины
        /// </summary>
        [JsonIgnore]
        public TypeClassesModel TypeClass { get => typeClass; set => SetProperty(ref typeClass, value); }

        /// <summary>
        /// Аудитория
        /// </summary>
        [JsonIgnore]
        public AudiencesModel Audience { get => audience; set => SetProperty(ref audience, value); }

        /// <summary>
        /// Группа
        /// </summary>
        [JsonIgnore]
        public GroupsModel Group { get => group; set => SetProperty(ref group, value); }

        /// <summary>
        /// Модель выбрана да/нет
        /// </summary>
        [JsonIgnore]
        public bool IsSelected { get => isSelected; set => SetProperty(ref isSelected, value); }

        /// <summary>
        /// Модель отменена да/нет
        /// </summary>
        [JsonIgnore]
        public bool IsCanceled { get => isCanceled; set => SetProperty(ref isCanceled, value); }

        #endregion
    }
    public class LessonsModelExtended : LessonsModel
    {
        private DateTime? _dateFrom;
        public DateTime? DateFrom
        {
            get => _dateFrom;
            set => SetProperty(ref _dateFrom, value);
        }

        private DateTime? _dateTo;
        public DateTime? DateTo
        {
            get => _dateTo;
            set => SetProperty(ref _dateTo, value);
        }
    }
}
