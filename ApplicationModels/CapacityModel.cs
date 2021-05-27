using Prism.Mvvm;
using System;

namespace ApplicationModels
{
    public class CapacityModel : BindableBase
    {
        #region fields

        private double _numberA;
        private double _numberB;
        private string _syllabus;
        private string _faculty;
        private string _block;
        private string _disciplineOrWorkType;
        private string _assignedDepartment;
        private string _courseOrSemesterAndSession;
        private string _group;
        private double _studentsCount;
        private double _weekCount;
        private string _classType;
        private double _hoursOnStreamOrGroupOrStudent;
        private string _controlsType;
        private double _KSR;
        private double _individualCount;
        private double _controlsCount;
        private double _ratingCount;
        private double _referates;
        private double _essay;
        private double _RGR;
        private double _controlWorksForAbsentia;
        private double _consultingSPO;
        private double _audithoryCapacity;
        private double _otherCapacity;
        private string _otherCapacityWithInfo;
        private double _totalCapacity;
        private string _teacherFio;
        private string _teacherPosition;
        private string _teacherRange;
        private double _flowNumber;
        private double _firstGroupFlowIndicator;
        private string _audRecommendSyllabus;
        private double _additionalHourStudent;
        private double _additionalHourGroup;
        private double _inFactDoneA;
        private double _inFactDoneB;
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private double _budget;
        private double _extraBudget;
        private double _foreign;
        private double _extraBudgetDole;
        private double _foreignDole;
        private string _totalContractForeign;
        private string _educationLevel;
        private string _learnType;
        private double _examinationHour;
        private double _ownWorkHour;
        private double _electronicHour;
        private double _normCoef;
        private string _note;
        private double _ZET;
        private double _questionCount;
        private double _hourAtWeek;

        #endregion
        #region props

        /// <summary>
        /// Номер 1
        /// </summary>
        public double NumberA { get => _numberA; set => SetProperty(ref _numberA, value); }
        /// <summary>
        /// Номер 2
        /// </summary>
        public double NumberB { get => _numberB; set => SetProperty(ref _numberB, value); }
        /// <summary>
        /// Учебный план
        /// </summary>
        public string Syllabus { get => _syllabus ??= string.Empty; set => SetProperty(ref _syllabus, value); }
        /// <summary>
        /// Факультет группы
        /// </summary>
        public string Faculty { get => _faculty ??= string.Empty; set => SetProperty(ref _faculty, value); }
        /// <summary>
        /// Блок
        /// </summary>
        public string Block { get => _block ??= string.Empty; set => SetProperty(ref _block, value); }
        /// <summary>
        /// Дисциплина, вид учебной работы 
        /// </summary>
        public string DisciplineOrWorkType { get => _disciplineOrWorkType ??= string.Empty; set => SetProperty(ref _disciplineOrWorkType, value); }
        /// <summary>
        /// Закреплённая кафедра
        /// </summary>
        public string AssignedDepartment { get => _assignedDepartment ??= string.Empty; set => SetProperty(ref _assignedDepartment, value); }
        /// <summary>
        /// Курс/Семестр или Курс/Сессия
        /// </summary>
        public string CourseOrSemesterAndSession { get => _courseOrSemesterAndSession ??= string.Empty; set => SetProperty(ref _courseOrSemesterAndSession, value); }
        /// <summary>
        /// Группа
        /// </summary>
        public string Group { get => _group ??= string.Empty; set => SetProperty(ref _group, value); }
        /// <summary>
        /// Кол-во студентов
        /// </summary>
        public double StudentsCount { get => _studentsCount; set => SetProperty(ref _studentsCount, value); }
        /// <summary>
        /// Недель
        /// </summary>
        public double WeekCount { get => _weekCount; set => SetProperty(ref _weekCount, value); }
        /// <summary>
        /// Вид занятий
        /// </summary>
        public string ClassType { get => _classType ??= string.Empty; set => SetProperty(ref _classType, value); }
        /// <summary>
        /// Часов (на поток, группу, студента)
        /// </summary>
        public double HoursOnStreamOrGroupOrStudent { get => _hoursOnStreamOrGroupOrStudent; set => SetProperty(ref _hoursOnStreamOrGroupOrStudent, value); }
        /// <summary>
        /// Виды контроля
        /// </summary>
        public string ControlsType { get => _controlsType ??= string.Empty; set => SetProperty(ref _controlsType, value); }
        /// <summary>
        /// КСР
        /// </summary>
        public double KSR { get => _KSR; set => SetProperty(ref _KSR, value); }
        /// <summary>
        /// Индивидуальные занятия
        /// </summary>
        public double IndividualCount { get => _individualCount; set => SetProperty(ref _individualCount, value); }
        /// <summary>
        /// Контрольные
        /// </summary>
        public double ControlsCount { get => _controlsCount; set => SetProperty(ref _controlsCount, value); }
        /// <summary>
        /// Оценка по рейтигу
        /// </summary>
        public double RatingCount { get => _ratingCount; set => SetProperty(ref _ratingCount, value); }
        /// <summary>
        /// Рефераты
        /// </summary>
        public double Referates { get => _referates; set => SetProperty(ref _referates, value); }
        /// <summary>
        /// Эссе
        /// </summary>
        public double Essay { get => _essay; set => SetProperty(ref _essay, value); }
        /// <summary>
        /// РГР
        /// </summary>
        public double RGR { get => _RGR; set => SetProperty(ref _RGR, value); }
        /// <summary>
        /// Контрольных работ (заоч)
        /// </summary>
        public double ControlWorksForAsentia { get => _controlWorksForAbsentia; set => SetProperty(ref _controlWorksForAbsentia, value); }
        /// <summary>
        /// Консультации (СПО)
        /// </summary>
        public double ConsultingSPO { get => _consultingSPO; set => SetProperty(ref _consultingSPO, value); }
        /// <summary>
        /// Нагрузка, час (Аудиторная)
        /// </summary>
        public double AudithoryCapacity { get => _audithoryCapacity; set => SetProperty(ref _audithoryCapacity, value); }
        /// <summary>
        /// Нагрузка, час (Другое)
        /// </summary>
        public double OtherCapacity { get => _otherCapacity; set => SetProperty(ref _otherCapacity, value); }
        /// <summary>
        /// Нагрузка, час (Другое с описанием)
        /// </summary>
        public string OtherCapacityWithInfo { get => _otherCapacityWithInfo ??= string.Empty; set => SetProperty(ref _otherCapacityWithInfo, value); }
        /// <summary>
        /// Нагрузка, час (Итого)
        /// </summary>
        public double TotalCapacity { get => _totalCapacity; set => SetProperty(ref _totalCapacity, value); }
        /// <summary>
        /// Преподаватель ФИО
        /// </summary>
        public string TeacherFio { get => _teacherFio ??= string.Empty; set => SetProperty(ref _teacherFio, value); }
        /// <summary>
        /// Должность преподавателя
        /// </summary>
        public string TeacherPosition { get => _teacherPosition ??= string.Empty; set => SetProperty(ref _teacherPosition, value); }
        /// <summary>
        /// Звание преподавателя
        /// </summary>
        public string TeacherRange { get => _teacherRange ??= string.Empty; set => SetProperty(ref _teacherRange, value); }
        /// <summary>
        /// Номер потока
        /// </summary>
        public double FlowNumber { get => _flowNumber; set => SetProperty(ref _flowNumber, value); }
        /// <summary>
        /// Индикатор первой группы потока
        /// </summary>
        public double FirstGroupFlowIndicator { get => _firstGroupFlowIndicator; set => SetProperty(ref _firstGroupFlowIndicator, value); }
        /// <summary>
        /// Ауд. рекомендуемая каф.
        /// </summary>
        public string AudRecommendSyllabus { get => _audRecommendSyllabus ??= string.Empty; set => SetProperty(ref _audRecommendSyllabus, value); }
        /// <summary>
        /// Дополнительно часов студента
        /// </summary>
        public double AdditionalHourStudent { get => _additionalHourStudent; set => SetProperty(ref _additionalHourStudent, value); }
        /// <summary>
        /// Дополнительно часов (п/) группу
        /// </summary>
        public double AdditionalHourGroup { get => _additionalHourGroup; set => SetProperty(ref _additionalHourGroup, value); }
        /// <summary>
        /// Фактически выполнено 1
        /// </summary>
        public double InFactDoneA { get => _inFactDoneA; set => SetProperty(ref _inFactDoneA, value); }
        /// <summary>
        /// Фактически выполнено 2
        /// </summary>
        public double InFactDoneB { get => _inFactDoneB; set => SetProperty(ref _inFactDoneB, value); }
        /// <summary>
        /// Время проведения занятий по графику С
        /// </summary>
        public DateTime DateFrom { get => _dateFrom; set => SetProperty(ref _dateFrom, value); }
        /// <summary>
        /// Время проведения занятий по графику ПО
        /// </summary>
        public DateTime DateTo { get => _dateTo; set => SetProperty(ref _dateTo, value); }
        /// <summary>
        /// Распределение нагрузки, час (Бюджет)
        /// </summary>
        public double Budget { get => _budget; set => SetProperty(ref _budget, value); }
        /// <summary>
        /// Распределение нагрузки, час (Внебюджет)
        /// </summary>
        public double ExtraBudget { get => _extraBudget; set => SetProperty(ref _extraBudget, value); }
        /// <summary>
        /// Распределение нагрузки, час (Иностр)
        /// </summary>
        public double Foreign { get => _foreign; set => SetProperty(ref _foreign, value); }
        /// <summary>
        /// Доля внебюджет
        /// </summary>
        public double ExtraBudgetDole { get => _extraBudgetDole; set => SetProperty(ref _extraBudgetDole, value); }
        /// <summary>
        /// Доля иностр
        /// </summary>
        public double ForeignDole { get => _foreignDole; set => SetProperty(ref _foreignDole, value); }
        /// <summary>
        /// Всего/Договор/Иностр
        /// </summary>
        public string TotalContractForeign { get => _totalContractForeign ??= string.Empty; set => SetProperty(ref _totalContractForeign, value); }
        /// <summary>
        /// Уровень образования
        /// </summary>
        public string EducationLevel { get => _educationLevel ??= string.Empty; set => SetProperty(ref _educationLevel, value); }
        /// <summary>
        /// Форма обучения
        /// </summary>
        public string LearnType { get => _learnType ??= string.Empty; set => SetProperty(ref _learnType, value); }
        /// <summary>
        /// Часов на экзамены
        /// </summary>
        public double ExaminationHour { get => _examinationHour; set => SetProperty(ref _examinationHour, value); }
        /// <summary>
        /// Сам. работа
        /// </summary>
        public double OwnWorkHour { get => _ownWorkHour; set => SetProperty(ref _ownWorkHour, value); }
        /// <summary>
        /// Электронные часы
        /// </summary>
        public double ElectronicHour { get => _electronicHour; set => SetProperty(ref _electronicHour, value); }
        /// <summary>
        /// Нормирующий коэффициент
        /// </summary>
        public double NormCoef { get => _normCoef; set => SetProperty(ref _normCoef, value); }
        /// <summary>
        /// Примечание
        /// </summary>
        public string Note { get => _note ??= string.Empty; set => SetProperty(ref _note, value); }
        /// <summary>
        /// ЗЕТ
        /// </summary>
        public double ZET { get => _ZET; set => SetProperty(ref _ZET, value); }
        /// <summary>
        /// Число вопросов
        /// </summary>
        public double QuestionCount { get => _questionCount; set => SetProperty(ref _questionCount, value); }
        /// <summary>
        /// Часов в неделю
        /// </summary>
        public double HourAtWeek { get => _hourAtWeek; set => SetProperty(ref _hourAtWeek, value); }

        #endregion
    }
}
