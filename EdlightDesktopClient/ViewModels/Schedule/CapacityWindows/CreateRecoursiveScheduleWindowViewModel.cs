using ApplicationModels;
using ApplicationModels.Models;
using ApplicationServices.IdentificatorService;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.MemoryService;
using HandyControl.Controls;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Styles.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static ApplicationModels.Models.RecoursiveModel;

namespace EdlightDesktopClient.ViewModels.Schedule.CapacityWindows
{
    public class CreateRecoursiveScheduleWindowViewModel : BindableBase, IDialogAware
    {
        #region services

        private readonly IWebApiService api;
        private readonly IMemoryService memory;
        private readonly IIdentificatorService identificator;

        #endregion
        #region fields

        private LoaderModel _loader;
        private CapacityModel _capacity;

        private readonly GregorianCalendar _calendar = new();
        private LessonsModel _fromNavigationModel;
        private RecoursiveModel _recoursive;
        private ObservableCollection<UserModel> _teachers;
        private ObservableCollection<AcademicDisciplinesModel> _disciplines;
        private ObservableCollection<AudiencesModel> _audiences;
        private ObservableCollection<TypeClassesModel> _typeClasses;
        private ObservableCollection<GroupsModel> _groups;
        private ObservableCollection<PairTimeModel> _pairTimes;
        private UserModel _selectedTeacher;
        private AcademicDisciplinesModel _selectedDiscipline;
        private AudiencesModel _selectedAudience;
        private TypeClassesModel _selectedTypeClass;
        private GroupsModel _selectedGroup;
        private PairTimeModel _selectedPairTime;
        private int _selectedWeekMode;

        #endregion
        #region props

        public LoaderModel Loader { get => _loader; set => SetProperty(ref _loader, value); }
        public CapacityModel Capacity { get => _capacity; set => SetProperty(ref _capacity, value); }

        public LessonsModel FromNavigationModel { get => _fromNavigationModel; set => SetProperty(ref _fromNavigationModel, value); }
        public RecoursiveModel Recoursive { get => _recoursive; set => SetProperty(ref _recoursive, value); }
        public ObservableCollection<UserModel> Teachers { get => _teachers; set => SetProperty(ref _teachers, value); }
        public ObservableCollection<AcademicDisciplinesModel> Disciplines { get => _disciplines; set => SetProperty(ref _disciplines, value); }
        public ObservableCollection<AudiencesModel> Audiences { get => _audiences; set => SetProperty(ref _audiences, value); }
        public ObservableCollection<TypeClassesModel> TypeClasses { get => _typeClasses; set => SetProperty(ref _typeClasses, value); }
        public ObservableCollection<GroupsModel> Groups { get => _groups; set => SetProperty(ref _groups, value); }
        public ObservableCollection<PairTimeModel> PairTimes { get => _pairTimes ??= new(); set => SetProperty(ref _pairTimes, value); }
        public UserModel SelectedTeacher { get => _selectedTeacher ??= new(); set => SetProperty(ref _selectedTeacher, value); }
        public AcademicDisciplinesModel SelectedDiscipline { get => _selectedDiscipline ??= new(); set => SetProperty(ref _selectedDiscipline, value); }
        public AudiencesModel SelectedAudience { get => _selectedAudience ??= new(); set => SetProperty(ref _selectedAudience, value); }
        public TypeClassesModel SelectedTypeClass { get => _selectedTypeClass ??= new(); set => SetProperty(ref _selectedTypeClass, value); }
        public GroupsModel SelectedGroup { get => _selectedGroup ??= new(); set => SetProperty(ref _selectedGroup, value); }
        public PairTimeModel SelectedPairTime { get => _selectedPairTime ??= new(); set => SetProperty(ref _selectedPairTime, value); }
        public int SelectedWeekMode { get => _selectedWeekMode; set => SetProperty(ref _selectedWeekMode, value); }

        #endregion
        #region errors

        private bool _hasErrors;
        private string _errors;

        public bool HasErrors { get => _hasErrors; set => SetProperty(ref _hasErrors, value); }
        public string Errors { get => _errors ??= string.Empty; set => SetProperty(ref _errors, value); }

        #endregion
        #region commands

        private DelegateCommand _loadedCommand;
        private DelegateCommand _confirmCommand;
        private DelegateCommand<string> _closeDialogCommand;

        public DelegateCommand LoadedCommand { get => _loadedCommand; set => SetProperty(ref _loadedCommand, value); }
        public DelegateCommand ConfirmCommand { get => _confirmCommand; set => SetProperty(ref _confirmCommand, value); }
        public DelegateCommand<string> CloseDialogCommand { get => _closeDialogCommand; set => SetProperty(ref _closeDialogCommand, value); }

        #endregion
        #region ctor

        public CreateRecoursiveScheduleWindowViewModel(IWebApiService api, IMemoryService memory, IIdentificatorService identificator)
        {
            Recoursive = new();
            Loader = new();
            this.api = api;
            this.memory = memory;
            this.identificator = identificator;

            CloseDialogCommand = new DelegateCommand<string>(CloseDialog);
            ConfirmCommand = new DelegateCommand(OnConfirm, CanConfirm);
            LoadedCommand = new DelegateCommand(OnLoaded);
        }
        private async void OnLoaded() => await LoadingData(api, memory);
        private async Task LoadingData(IWebApiService api, IMemoryService memory)
        {
            try
            {
                Loader.SetDefaultLoadingInfo();
                RolesModel teacherRole = (await api.GetModels<RolesModel>(WebApiTableNames.Roles, $" RoleName = 'teacher'")).FirstOrDefault();
                List<UsersRolesModel> usersRoles = await api.GetModels<UsersRolesModel>(WebApiTableNames.UsersRoles, $" IdRole = '{teacherRole.Id}'");
                IEnumerable<UserModel> teachers = (await api.GetModels<UserModel>(WebApiTableNames.Users)).Where(u => usersRoles.Any(ur => ur.IdUser == u.ID));

                Teachers = new ObservableCollection<UserModel>(teachers);
                UserModel currentUser = memory.GetItem<UserModel>(MemoryAlliases.CurrentUser);
                if (currentUser != null && Teachers.Any(t => t.ID == currentUser.ID)) SelectedTeacher = currentUser;
                else if (Teachers.Count != 0) SelectedTeacher = Teachers.FirstOrDefault();

                Disciplines = new ObservableCollection<AcademicDisciplinesModel>(await api.GetModels<AcademicDisciplinesModel>(WebApiTableNames.AcademicDisciplines));
                if (Disciplines.Count != 0) SelectedDiscipline = Disciplines.FirstOrDefault();

                Audiences = new ObservableCollection<AudiencesModel>(await api.GetModels<AudiencesModel>(WebApiTableNames.Audiences));
                if (Audiences.Count != 0) SelectedAudience = Audiences.FirstOrDefault();

                TypeClasses = new ObservableCollection<TypeClassesModel>(await api.GetModels<TypeClassesModel>(WebApiTableNames.TypeClasses));
                if (TypeClasses.Count != 0) SelectedTypeClass = TypeClasses.FirstOrDefault();

                Groups = new ObservableCollection<GroupsModel>(await api.GetModels<GroupsModel>(WebApiTableNames.Groups));
                if (SelectedGroup == null) SelectedGroup = Groups.FirstOrDefault();

                //Ищем преподавателя из нагрузки
                string teacherInitials = Capacity.TeacherFio.Remove(0, 1);
                UserModel capacity_teacher = Teachers.FirstOrDefault(t => t.Initials == teacherInitials);
                if (capacity_teacher != null) SelectedTeacher = capacity_teacher;

                //Ищем дисциплину из нагрузки
                string discipline_name = Capacity.DisciplineOrWorkType;
                var capacity_discipline = Disciplines.FirstOrDefault(d => d.Title == discipline_name);
                if (capacity_discipline != null) SelectedDiscipline = capacity_discipline;

                //Выбираем аудиторию из приоритета дисциплины
                AudiencesModel capacity_audience = Audiences.FirstOrDefault(a => a.Id == SelectedDiscipline.IdPriorityAudience);
                if (capacity_audience != null) SelectedAudience = capacity_audience;

                //Ищем тип занятия из нагрузки
                TypeClassesModel capacity_type_class = TypeClasses.FirstOrDefault(t => t.ShortTitle == Capacity.ClassType);
                if (capacity_type_class != null) SelectedTypeClass = capacity_type_class;

                //Ищем группу из нагрузки
                GroupsModel capacity_group = Groups.FirstOrDefault(g => g.Group == Capacity.Group);
                if (capacity_group != null) SelectedGroup = capacity_group;

                //Устанавливаем количество недель из нагрузки
                Recoursive.WeekCount = (int)Capacity.WeekCount.Value;

                //Если есть дата в нагрузке устанавливаем
                if (Capacity.DateFrom.HasValue)
                {
                    Recoursive.StartDate = Capacity.DateFrom.Value;
                }
                else
                {
                    Recoursive.StartDate = DateTime.Now;
                }

                if (SelectedTypeClass.ShortTitle == "Лек")
                {
                    SelectedWeekMode = 1;
                }
                else
                {
                    SelectedWeekMode = 0;
                }

                PairTimes.Clear();

                PairTimes.Add(new PairTimeModel() { PairName = "1 пара", StartTime = "8:30", EndTime = "10:05", BreakTime = "5", });
                PairTimes.Add(new PairTimeModel() { PairName = "2 пара", StartTime = "10:15", EndTime = "11:50", BreakTime = "5", });
                PairTimes.Add(new PairTimeModel() { PairName = "3 пара", StartTime = "12:30", EndTime = "14:05", BreakTime = "5", });
                PairTimes.Add(new PairTimeModel() { PairName = "4 пара", StartTime = "14:30", EndTime = "15:50", BreakTime = "5", });
                PairTimes.Add(new PairTimeModel() { PairName = "5 пара", StartTime = "16:00", EndTime = "17:35", BreakTime = "5", });
                PairTimes.Add(new PairTimeModel() { PairName = "6 пара", StartTime = "17:45", EndTime = "19:20", BreakTime = "0", });

                SelectedPairTime = PairTimes.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await Loader.Clear();
            }
        }

        #endregion
        #region methods

        private bool CanConfirm()
        {
            HasErrors = false;
            Errors = string.Empty;
            if (Recoursive.StartDate == null || Recoursive.StartDate == new DateTime())
            {
                Errors += "Не выбрана дата занятия";
                HasErrors = true;
            }
            if (Recoursive.IsAllDeselected)
            {
                Errors += "Необходимо выбрать как минимум 1 день";
                HasErrors = true;
            }
            return !HasErrors;
        }
        private async void OnConfirm()
        {
            try
            {
                Loader.SetDefaultLoadingInfo();
                List<LessonsModel> lm_to_post = new();
                int RecourseID = identificator.GetIntID();

                DateTime end = Recoursive.StartDate.AddDays(7 * Recoursive.WeekCount);
                List<DayOfWeek> selectedDays = Recoursive.GetDaysList();
                int total_days = (int)(end - Recoursive.StartDate).TotalDays;
                for (int i = 0; i < total_days; i++)
                {
                    DateTime day = Recoursive.StartDate.AddDays(i);
                    if (!selectedDays.Any(sd => sd == day.DayOfWeek)) continue;

                    //Проверяем семестр
                    DateTime autumnDateStart = new(day.Year, 09, 01);
                    DateTime springDateStart = new(day.Year, 02, 09);
                    bool IsFirstHalfYearTime = day.DayOfYear < autumnDateStart.DayOfYear;

                    //Считаем неделю
                    int week = _calendar.GetWeekOfYear(day, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
                    int autumnWeekStart = _calendar.GetWeekOfYear(autumnDateStart, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
                    int springWeekStart = _calendar.GetWeekOfYear(springDateStart, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);

                    //Проверяем какой семестр
                    bool isUpWeek = (week - (IsFirstHalfYearTime ? springWeekStart : autumnWeekStart)) % 2 == 0;

                    if (SelectedWeekMode == (int)WeekCheckingMode.UpWeek && !isUpWeek) continue;
                    if (SelectedWeekMode == (int)WeekCheckingMode.DownWeek && isUpWeek) continue;

                    LessonsModel lm = new();
                    lm.Day = day;
                    lm.IdAcademicDiscipline = SelectedDiscipline.Id;
                    lm.IdAudience = SelectedAudience.Id;
                    lm.IdGroup = SelectedGroup.Id;
                    lm.IdTeacher = SelectedTeacher.ID;
                    lm.IdTypeClass = SelectedTypeClass.Id;
                    lm.RecoursiveId = RecourseID;
                    lm.TimeLessons = new()
                    {
                        StartTime = SelectedPairTime.StartTime,
                        EndTime = SelectedPairTime.EndTime,
                        BreakTime = SelectedPairTime.BreakTime,
                    };

                    lm_to_post.Add(lm);
                }
                int loader_count = 0;
                foreach (LessonsModel lm in lm_to_post)
                {
                    Loader.SetCountLoadingInfo(loader_count++, lm_to_post.Count);

                    TimeLessonsModel posted_tlm = await api.PostModel(lm.TimeLessons, WebApiTableNames.TimeLessons);
                    lm.IdTimeLessons = posted_tlm.Id;

                    LessonsModel postedLM = await api.PostModel(lm, WebApiTableNames.Lessons);
                }
                await Loader.Clear();
                Growl.Info($"Создано {loader_count} записей", "Global");
                CloseDialog(true.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await Loader.Clear();
            }
        }

        #endregion
        #region close dialog

        public string Title { get; } = "Создание занятия ( рекурсивно )";
        public event Action<IDialogResult> RequestClose;
        protected virtual void CloseDialog(string parameter)
        {
            ButtonResult result = ButtonResult.None;

            if (parameter?.ToLower() == "true")
            {
                Capacity.IsScheduleCreated = true;
                result = ButtonResult.OK;
            }
            else if (parameter?.ToLower() == "false")
                result = ButtonResult.Cancel;

            RaiseRequestClose(new DialogResult(result));
        }
        public virtual void RaiseRequestClose(IDialogResult dialogResult) => RequestClose?.Invoke(dialogResult);
        public bool CanCloseDialog() => true;

        #endregion
        #region dialog aware

        public void OnDialogClosed()
        {
        }
        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey(nameof(CapacityModel)) && parameters.GetValue<CapacityModel>(nameof(CapacityModel)) is CapacityModel capacity)
            {
                Capacity = capacity;
            }
        }

        #endregion
    }
}
