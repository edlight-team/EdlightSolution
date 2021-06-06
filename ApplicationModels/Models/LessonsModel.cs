using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;

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
    public class RecoursiveModel : BindableBase
    {
        #region fields

        private DateTime _startDate;
        private bool _isMondaySelect;
        private bool _isTuesdaySelect;
        private bool _isWednesdaySelect;
        private bool _isThursdaySelect;
        private bool _isFridaySelect;
        private bool _isSaturdaySelect;
        private int _weekCount;
        private WeekCheckingMode _weekChecking;

        #endregion
        #region props

        /// <summary>
        /// Дата начала рекурсии
        /// </summary>
        public DateTime StartDate { get => _startDate; set => SetProperty(ref _startDate, value); }
        /// <summary>
        /// Понедельник
        /// </summary>
        public bool IsMondaySelect { get => _isMondaySelect; set => SetProperty(ref _isMondaySelect, value); }
        /// <summary>
        /// Вторник
        /// </summary>
        public bool IsTuesdaySelect { get => _isTuesdaySelect; set => SetProperty(ref _isTuesdaySelect, value); }
        /// <summary>
        /// Среда
        /// </summary>
        public bool IsWednesdaySelect { get => _isWednesdaySelect; set => SetProperty(ref _isWednesdaySelect, value); }
        /// <summary>
        /// Четверг
        /// </summary>
        public bool IsThursdaySelect { get => _isThursdaySelect; set => SetProperty(ref _isThursdaySelect, value); }
        /// <summary>
        /// Пятница
        /// </summary>
        public bool IsFridaySelect { get => _isFridaySelect; set => SetProperty(ref _isFridaySelect, value); }
        /// <summary>
        /// Суббота
        /// </summary>
        public bool IsSaturdaySelect { get => _isSaturdaySelect; set => SetProperty(ref _isSaturdaySelect, value); }
        /// <summary>
        /// Количество недель
        /// </summary>
        public int WeekCount { get => _weekCount; set => SetProperty(ref _weekCount, value); }
        /// <summary>
        /// Режим проверки недели
        /// </summary>
        public WeekCheckingMode WeekChecking { get => _weekChecking; set => SetProperty(ref _weekChecking, value); }

        #endregion
        #region additional

        /// <summary>
        /// Все дни в состоянии выключен
        /// </summary>
        public bool IsAllDeselected => !IsMondaySelect && !IsTuesdaySelect && !IsWednesdaySelect && !IsThursdaySelect && !IsFridaySelect && !IsSaturdaySelect;
        /// <summary>
        /// Получить список дней по выбранным галочкам
        /// </summary>
        /// <returns></returns>
        public List<DayOfWeek> GetDaysList()
        {
            List<DayOfWeek> days = new();
            if (IsMondaySelect) days.Add(DayOfWeek.Monday);
            if (IsTuesdaySelect) days.Add(DayOfWeek.Tuesday);
            if (IsWednesdaySelect) days.Add(DayOfWeek.Wednesday);
            if (IsThursdaySelect) days.Add(DayOfWeek.Thursday);
            if (IsFridaySelect) days.Add(DayOfWeek.Friday);
            if (IsSaturdaySelect) days.Add(DayOfWeek.Saturday);
            return days;
        }
        public enum WeekCheckingMode
        {
            None, UpWeek, DownWeek
        } 

        #endregion
    }
    public class PairTimeModel : BindableBase
    {
        private string _pairName;
        private string _startTime;
        private string _endTime;
        private string _breakTime;

        public string PairName { get => _pairName; set => SetProperty(ref _pairName, value); }
        public string StartTime { get => _startTime; set => SetProperty(ref _startTime, value); }
        public string EndTime { get => _endTime; set => SetProperty(ref _endTime, value); }
        public string BreakTime { get => _breakTime; set => SetProperty(ref _breakTime, value); }
    }
}
