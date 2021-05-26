using ApplicationEventsWPF.Events.ScheduleEvents;
using ApplicationEventsWPF.Events.Signal;
using ApplicationModels.Models;
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
using System.Linq;
using System.Threading.Tasks;

namespace EdlightDesktopClient.ViewModels.Schedule
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class AddScheduleViewModel : BindableBase, INavigationAware
    {
        #region services

        private readonly IRegionManager manager;
        private readonly IMemoryService memory;
        private readonly IWebApiService api;
        private readonly IEventAggregator aggregator;

        #endregion
        #region fields

        private NavigationParameters _onNavigatedToParameters;
        private LoaderModel _loader;
        private LessonsModel _fromNavigationModel;
        private ObservableCollection<UserModel> _teachers;
        private ObservableCollection<AcademicDisciplinesModel> _disciplines;
        private ObservableCollection<AudiencesModel> _audiences;
        private ObservableCollection<TypeClassesModel> _typeClasses;
        private ObservableCollection<GroupsModel> _groups;
        private ObservableCollection<string> _timeZonesFrom;
        private ObservableCollection<string> _timeZonesTo;
        private UserModel _selectedTeacher;
        private AcademicDisciplinesModel _selectedDiscipline;
        private AudiencesModel _selectedAudience;
        private TypeClassesModel _selectedTypeClass;
        private GroupsModel _selectedGroup;
        private int _indexTimeZonesFrom;
        private int _indexTimeZonesTo;
        private DateTime _currentDate;
        private int _breakTime;
        private string _saveButtonText;

        #endregion
        #region props

        public LoaderModel Loader { get => _loader; set => SetProperty(ref _loader, value); }
        public LessonsModel FromNavigationModel { get => _fromNavigationModel; set => SetProperty(ref _fromNavigationModel, value); }
        public ObservableCollection<UserModel> Teachers { get => _teachers; set => SetProperty(ref _teachers, value); }
        public ObservableCollection<AcademicDisciplinesModel> Disciplines { get => _disciplines; set => SetProperty(ref _disciplines, value); }
        public ObservableCollection<AudiencesModel> Audiences { get => _audiences; set => SetProperty(ref _audiences, value); }
        public ObservableCollection<TypeClassesModel> TypeClasses { get => _typeClasses; set => SetProperty(ref _typeClasses, value); }
        public ObservableCollection<GroupsModel> Groups { get => _groups; set => SetProperty(ref _groups, value); }
        public ObservableCollection<string> TimeZonesFrom { get => _timeZonesFrom; set => SetProperty(ref _timeZonesFrom, value); }
        public ObservableCollection<string> TimeZonesTo { get => _timeZonesTo; set => SetProperty(ref _timeZonesTo, value); }
        public UserModel SelectedTeacher { get => _selectedTeacher ??= new(); set => SetProperty(ref _selectedTeacher, value); }
        public AcademicDisciplinesModel SelectedDiscipline { get => _selectedDiscipline ??= new(); set => SetProperty(ref _selectedDiscipline, value); }
        public AudiencesModel SelectedAudience { get => _selectedAudience ??= new(); set => SetProperty(ref _selectedAudience, value); }
        public TypeClassesModel SelectedTypeClass { get => _selectedTypeClass ??= new(); set => SetProperty(ref _selectedTypeClass, value); }
        public GroupsModel SelectedGroup { get => _selectedGroup ??= new(); set => SetProperty(ref _selectedGroup, value); }
        public int IndexTimeZonesFrom { get => _indexTimeZonesFrom; set => SetProperty(ref _indexTimeZonesFrom, value); }
        public int IndexTimeZonesTo { get => _indexTimeZonesTo; set => SetProperty(ref _indexTimeZonesTo, value); }
        public DateTime CurrentDate { get => _currentDate; set => SetProperty(ref _currentDate, value); }
        public int BreakTime { get => _breakTime; set => SetProperty(ref _breakTime, value); }
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

        public AddScheduleViewModel(IRegionManager manager, IMemoryService memory, IWebApiService api, IEventAggregator aggregator)
        {
            SaveButtonText = "Создать запись";
            Loader = new();
            this.manager = manager;
            this.memory = memory;
            this.api = api;
            this.aggregator = aggregator;

            CloseModalCommand = new DelegateCommand(OnCloseModal);
            CreateRecordCommand = new DelegateCommand(OnCreateSchedule, CanCreateSchedule)
                .ObservesProperty(() => IndexTimeZonesFrom)
                .ObservesProperty(() => IndexTimeZonesTo)
                .ObservesProperty(() => CurrentDate);
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

                TimeZonesFrom = new();
                TimeZonesTo = new();

                int[] intervals = new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };
                for (int i = 8; i < 20; i++)
                {
                    TimeZonesFrom.Add(i.ToString() + ":" + 0.ToString("D2"));
                    TimeZonesTo.Add(i.ToString() + ":" + 0.ToString("D2"));
                    foreach (int interval in intervals)
                    {
                        TimeZonesFrom.Add(i.ToString() + ":" + interval.ToString("D2"));
                        TimeZonesTo.Add(i.ToString() + ":" + interval.ToString("D2"));
                    }
                }

                IndexTimeZonesFrom = 0;
                IndexTimeZonesTo = 4;

                if (_onNavigatedToParameters.ContainsKey("EditModel") && _onNavigatedToParameters["EditModel"] is LessonsModel lm)
                {
                    FromNavigationModel = lm;

                    IndexTimeZonesFrom = TimeZonesFrom.IndexOf(lm.TimeLessons.StartTime);
                    IndexTimeZonesTo = TimeZonesFrom.IndexOf(lm.TimeLessons.EndTime);
                    int.TryParse(lm.TimeLessons.BreakTime, out int parsedBreakTime);
                    BreakTime = parsedBreakTime;

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
            HasErrors = false;
            if (IndexTimeZonesTo - IndexTimeZonesFrom < 4)
            {
                Errors = "Минимальное время занятия составляет 20 минут, выберите корректный интервал\r\n";
                HasErrors = true;
            }
            if (CurrentDate == null || CurrentDate == new DateTime())
            {
                Errors += "Не выбрана дата занятия";
                HasErrors = true;
            }
            return !HasErrors;
        }
        private async void OnCreateSchedule()
        {
            if (SaveButtonText == "Создать запись")
            {
                TimeLessonsModel tlm = new();
                tlm.StartTime = TimeZonesFrom[IndexTimeZonesFrom];
                tlm.EndTime = TimeZonesTo[IndexTimeZonesTo];
                tlm.BreakTime = BreakTime.ToString();
                TimeLessonsModel postedTLM = await api.PostModel(tlm, WebApiTableNames.TimeLessons);

                LessonsModel lm = new();
                lm.Day = CurrentDate;
                lm.IdAcademicDiscipline = SelectedDiscipline.Id;
                lm.IdAudience = SelectedAudience.Id;
                lm.IdGroup = SelectedGroup.Id;
                lm.IdTeacher = SelectedTeacher.ID;
                lm.IdTypeClass = SelectedTypeClass.Id;
                lm.IdTimeLessons = postedTLM.Id;

                LessonsModel postedLM = await api.PostModel(lm, WebApiTableNames.Lessons);

                aggregator.GetEvent<DateChangedEvent>().Publish();
                aggregator.GetEvent<SignalEntitySendEvent>().Publish(new EntitySignalModel()
                {
                    SendType = "POST",
                    ModelType = typeof(LessonsModel),
                    SerializedModel = JsonConvert.SerializeObject(postedLM)
                });
                Growl.Info("Запись успешно создана", "Global");
                OnCloseModal();
            }
            else
            {
                TimeLessonsModel tlm = new();
                tlm.Id = FromNavigationModel.TimeLessons.Id;
                tlm.StartTime = TimeZonesFrom[IndexTimeZonesFrom];
                tlm.EndTime = TimeZonesTo[IndexTimeZonesTo];
                tlm.BreakTime = BreakTime.ToString();
                TimeLessonsModel postedTLM = await api.PutModel(tlm, WebApiTableNames.TimeLessons);

                LessonsModel lm = new();
                lm.Id = FromNavigationModel.Id;
                lm.Day = CurrentDate;
                lm.IdAcademicDiscipline = SelectedDiscipline.Id;
                lm.IdAudience = SelectedAudience.Id;
                lm.IdGroup = SelectedGroup.Id;
                lm.IdTeacher = SelectedTeacher.ID;
                lm.IdTypeClass = SelectedTypeClass.Id;
                lm.IdTimeLessons = postedTLM.Id;

                LessonsModel postedLM = await api.PutModel(lm, WebApiTableNames.Lessons);

                aggregator.GetEvent<DateChangedEvent>().Publish();
                aggregator.GetEvent<SignalEntitySendEvent>().Publish(new EntitySignalModel()
                {
                    SendType = "PUT",
                    ModelType = typeof(LessonsModel),
                    SerializedModel = JsonConvert.SerializeObject(postedLM)
                });
                Growl.Info("Запись успешно сохранена", "Global");
                OnCloseModal();
            }
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
