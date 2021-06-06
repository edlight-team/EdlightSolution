using ApplicationEventsWPF.Events.ScheduleEvents;
using ApplicationEventsWPF.Events.Signal;
using ApplicationModels.Models;
using ApplicationServices.IdentificatorService;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.MemoryService;
using HandyControl.Controls;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Styles.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static ApplicationModels.Models.RecoursiveModel;

namespace EdlightDesktopClient.ViewModels.Schedule.EditSchedule
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class AddRecoursiveScheduleViewModel : BindableBase, INavigationAware
    {
        #region services

        private readonly IRegionManager manager;
        private readonly IMemoryService memory;
        private readonly IWebApiService api;
        private readonly IEventAggregator aggregator;
        private readonly IIdentificatorService identificator;

        #endregion
        #region fields

        private readonly GregorianCalendar _calendar = new();
        private NavigationParameters _onNavigatedToParameters;
        private LoaderModel _loader;
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
        private DateTime _currentDate;
        private string _saveButtonText;

        #endregion
        #region props

        public LoaderModel Loader { get => _loader; set => SetProperty(ref _loader, value); }
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
        public DateTime CurrentDate { get => _currentDate; set => SetProperty(ref _currentDate, value); }
        public string SaveButtonText { get => _saveButtonText; set => SetProperty(ref _saveButtonText, value); }

        #endregion
        #region errors

        private bool _hasErrors;
        private string _timeToSmallError;

        public bool HasErrors { get => _hasErrors; set => SetProperty(ref _hasErrors, value); }
        public string Errors { get => _timeToSmallError ??= string.Empty; set => SetProperty(ref _timeToSmallError, value); }

        #endregion
        #region commands

        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand CreateRecordCommand { get; private set; }
        public DelegateCommand CloseModalCommand { get; private set; }

        #endregion
        #region ctor

        public AddRecoursiveScheduleViewModel(IRegionManager manager, IMemoryService memory, IWebApiService api, IEventAggregator aggregator, IIdentificatorService identificator)
        {
            SaveButtonText = "Создать запись";
            Recoursive = new();
            Loader = new();
            this.manager = manager;
            this.memory = memory;
            this.api = api;
            this.aggregator = aggregator;
            this.identificator = identificator;

            CloseModalCommand = new DelegateCommand(OnCloseModal);
            CreateRecordCommand = new DelegateCommand(OnCreateSchedule, CanCreateSchedule)
                .ObservesProperty(() => Recoursive.StartDate)
                .ObservesProperty(() => Recoursive.IsMondaySelect)
                .ObservesProperty(() => Recoursive.IsTuesdaySelect)
                .ObservesProperty(() => Recoursive.IsWednesdaySelect)
                .ObservesProperty(() => Recoursive.IsThursdaySelect)
                .ObservesProperty(() => Recoursive.IsFridaySelect)
                .ObservesProperty(() => Recoursive.IsSaturdaySelect);
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

                if (_onNavigatedToParameters.ContainsKey("EditModel") && _onNavigatedToParameters["EditModel"] is LessonsModel lm)
                {
                    FromNavigationModel = lm;

                    int.TryParse(lm.TimeLessons.BreakTime, out int parsedBreakTime);

                    CurrentDate = lm.Day;
                    SelectedDiscipline = Disciplines.FirstOrDefault(d => d.Id == lm.AcademicDiscipline.Id);
                    SelectedAudience = Audiences.FirstOrDefault(a => a.Id == lm.Audience.Id);
                    SelectedGroup = Groups.FirstOrDefault(g => g.Id == lm.Group.Id);
                    SelectedTeacher = Teachers.FirstOrDefault(t => t.ID == lm.Teacher.ID);
                    SelectedTypeClass = TypeClasses.FirstOrDefault(t => t.Id == lm.TypeClass.Id);

                    SaveButtonText = "Сохранить запись";
                }
                else
                {
                    if (_onNavigatedToParameters.ContainsKey(nameof(CurrentDate)) && _onNavigatedToParameters[nameof(CurrentDate)] is DateTime date)
                    {
                        CurrentDate = date;
                    }
                    if (_onNavigatedToParameters.ContainsKey(nameof(SelectedGroup)) && _onNavigatedToParameters[nameof(SelectedGroup)] is GroupsModel gr)
                    {
                        SelectedGroup = Groups.FirstOrDefault(g => g.Id == gr.Id);
                    }
                    SaveButtonText = "Создать запись";
                }
                Recoursive.WeekCount = 1;
                Recoursive.StartDate = CurrentDate;
                SelectedWeekMode = 0;

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

        private bool CanCreateSchedule()
        {
            Errors = string.Empty;
            HasErrors = false;
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
        private async void OnCreateSchedule()
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
                aggregator.GetEvent<DateChangedEvent>().Publish();
                aggregator.GetEvent<SignalEntitySendEvent>().Publish(new EntitySignalModel()
                {
                    SendType = "POST",
                    ModelType = typeof(LessonsModel),
                    SerializedModel = JsonConvert.SerializeObject(postedLM)
                });
            }
            await Loader.Clear();
            Growl.Info($"Создано {loader_count} записей", "Global");
            OnCloseModal();
        }
        private void OnCloseModal() => manager.Regions[BaseMethods.RegionNames.ModalRegion].RemoveAll();

        #endregion
        #region navigation
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters != null)
            {
                _onNavigatedToParameters = navigationContext.Parameters;
            }
        }
        public void OnNavigatedFrom(NavigationContext navigationContext) { }
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;
        #endregion
    }
}
