using ApplicationEventsWPF.Events;
using ApplicationModels.Models;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.MemoryService;
using HandyControl.Controls;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EdlightDesktopClient.ViewModels.Schedule
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class AddScheduleViewModel : BindableBase
    {
        #region services

        private readonly IRegionManager manager;
        private readonly IWebApiService api;
        private readonly IEventAggregator aggregator;

        #endregion
        #region fields

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
        private DateTime _lessonDate;
        private int _breakTime;

        #endregion
        #region props

        public ObservableCollection<UserModel> Teachers { get => _teachers; set => SetProperty(ref _teachers, value); }
        public ObservableCollection<AcademicDisciplinesModel> Disciplines { get => _disciplines; set => SetProperty(ref _disciplines, value); }
        public ObservableCollection<AudiencesModel> Audiences { get => _audiences; set => SetProperty(ref _audiences, value); }
        public ObservableCollection<TypeClassesModel> TypeClasses { get => _typeClasses; set => SetProperty(ref _typeClasses, value); }
        public ObservableCollection<GroupsModel> Groups { get => _groups; set => SetProperty(ref _groups, value); }
        public ObservableCollection<string> TimeZonesFrom { get => _timeZonesFrom; set => SetProperty(ref _timeZonesFrom, value); }
        public ObservableCollection<string> TimeZonesTo { get => _timeZonesTo; set => SetProperty(ref _timeZonesTo, value); }
        public UserModel SelectedTeacher { get => _selectedTeacher; set => SetProperty(ref _selectedTeacher, value); }
        public AcademicDisciplinesModel SelectedDiscipline { get => _selectedDiscipline; set => SetProperty(ref _selectedDiscipline, value); }
        public AudiencesModel SelectedAudience { get => _selectedAudience; set => SetProperty(ref _selectedAudience, value); }
        public TypeClassesModel SelectedTypeClass { get => _selectedTypeClass; set => SetProperty(ref _selectedTypeClass, value); }
        public GroupsModel SelectedGroup { get => _selectedGroup; set => SetProperty(ref _selectedGroup, value); }
        public int IndexTimeZonesFrom { get => _indexTimeZonesFrom; set => SetProperty(ref _indexTimeZonesFrom, value); }
        public int IndexTimeZonesTo { get => _indexTimeZonesTo; set => SetProperty(ref _indexTimeZonesTo, value); }
        public DateTime LessonDate { get => _lessonDate; set => SetProperty(ref _lessonDate, value); }
        public int BreakTime { get => _breakTime; set => SetProperty(ref _breakTime, value); }

        #endregion
        #region errors

        private bool _hasErrors;
        private string _timeToSmallError;

        public bool HasErrors { get => _hasErrors; set => SetProperty(ref _hasErrors, value); }
        public string TimeToSmallError { get => _timeToSmallError ??= string.Empty; set => SetProperty(ref _timeToSmallError, value); }

        #endregion
        #region commands

        public DelegateCommand CreateRecordCommand { get; private set; }
        public DelegateCommand CloseModalCommand { get; private set; }

        #endregion
        #region ctor

        public AddScheduleViewModel(IRegionManager manager, IMemoryService memory, IWebApiService api, IEventAggregator aggregator)
        {
            this.manager = manager;
            this.api = api;
            this.aggregator = aggregator;

            Task loading = Task.Run(async () => await LoadingData(api, memory));
            Task.WhenAll(loading).Wait();

            CloseModalCommand = new DelegateCommand(OnCloseModal);
            CreateRecordCommand = new DelegateCommand(OnCreateSchedule, CanCreateSchedule)
                .ObservesProperty(() => IndexTimeZonesFrom)
                .ObservesProperty(() => IndexTimeZonesTo);
        }
        private async Task LoadingData(IWebApiService api, IMemoryService memory)
        {
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
            if (Groups.Count != 0) SelectedGroup = Groups.FirstOrDefault();

            TimeZonesFrom = new();
            TimeZonesTo = new();

            int[] intervals = new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };
            for (int i = 8; i < 20; i++)
            {
                TimeZonesFrom.Add(i.ToString("D2") + ":" + 0.ToString("D2"));
                TimeZonesTo.Add(i.ToString("D2") + ":" + 0.ToString("D2"));
                foreach (var interval in intervals)
                {
                    TimeZonesFrom.Add(i.ToString("D2") + ":" + interval.ToString("D2"));
                    TimeZonesTo.Add(i.ToString("D2") + ":" + interval.ToString("D2"));
                }
            }

            IndexTimeZonesFrom = 0;
            IndexTimeZonesTo = 4;
            LessonDate = DateTime.Now;
        }

        #endregion
        #region methods

        public bool CanCreateSchedule()
        {
            HasErrors = false;
            if (IndexTimeZonesTo - IndexTimeZonesFrom < 4)
            {
                TimeToSmallError = "Минимальное время занятия составляет 20 минут, выберите корректный интервал";
                HasErrors = true;
            }
            return !HasErrors;
        }
        private async void OnCreateSchedule()
        {
            TimeLessonsModel tlm = new();
            tlm.StartTime = TimeZonesFrom[IndexTimeZonesFrom];
            tlm.EndTime = TimeZonesTo[IndexTimeZonesTo];
            tlm.BreakTime = BreakTime.ToString();
            TimeLessonsModel postedTLM = await api.PostModel(tlm, WebApiTableNames.TimeLessons);

            LessonsModel lm = new();
            lm.Day = LessonDate;
            lm.IdAcademicDiscipline = SelectedDiscipline.Id;
            lm.IdAudience = SelectedAudience.Id;
            lm.IdGroup = SelectedGroup.Id;
            lm.IdTeacher = SelectedTeacher.ID;
            lm.IdTypeClass = SelectedTypeClass.Id;
            lm.IdTimeLessons = postedTLM.Id;

            LessonsModel postedLM = await api.PostModel(lm, WebApiTableNames.Lessons);

            aggregator.GetEvent<DateChangedEvent>().Publish();
            Growl.Info("Запись успешно создана", "Global");
            OnCloseModal();
        }
        private void OnCloseModal() => manager.Regions[BaseMethods.RegionNames.ModalRegion].RemoveAll();


        #endregion
    }
}
