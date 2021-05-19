using ApplicationEventsWPF.Events;
using ApplicationModels.Config;
using ApplicationModels.Models;
using ApplicationServices.PermissionService;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.MemoryService;
using ApplicationWPFServices.NotificationService;
using EdlightDesktopClient.AccessConfigurations;
using EdlightDesktopClient.Views.Schedule;
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
using System.Windows;

namespace EdlightDesktopClient.ViewModels.Schedule
{
    public class ScheduleMainViewModel : BindableBase
    {
        #region services

        private readonly IRegionManager manager;
        private readonly IMemoryService memory;
        private readonly IWebApiService api;
        private readonly INotificationService notification;
        private readonly IPermissionService permissionService;
        private readonly IEventAggregator aggregator;

        #endregion
        #region fields

        private bool _firstRun = true;
        private bool _isCardActionsEnabled;

        private ScheduleConfig _config;
        private LoaderModel _scheduleLoader;
        private DateTime _currentDate;
        private UserModel _currentUser;
        private GroupsModel _selectedGroup;
        private ObservableCollection<LessonsModel> _models;

        private ObservableCollection<UserModel> _teachers;
        private ObservableCollection<AcademicDisciplinesModel> _disciplines;
        private ObservableCollection<AudiencesModel> _audiences;
        private ObservableCollection<TypeClassesModel> _typeClasses;
        private ObservableCollection<GroupsModel> _groups;
        private ObservableCollection<TimeLessonsModel> _timeLessons;

        #endregion
        #region props

        public bool IsCardActionsEnabled { get => _isCardActionsEnabled; set => SetProperty(ref _isCardActionsEnabled, value); }

        public ScheduleConfig Config { get => _config ??= new(); set => SetProperty(ref _config, value); }
        public LoaderModel ScheduleLoader { get => _scheduleLoader; set => SetProperty(ref _scheduleLoader, value); }
        public DateTime CurrentDate
        {
            get => _currentDate;
            set
            {
                if (_currentDate.ToLongDateString() != value.ToLongDateString())
                {
                    OnLoadModelsByDate(value);
                }
                _currentDate = value;
                RaisePropertyChanged(nameof(CurrentDate));
            }
        }
        public UserModel CurrentUser { get => _currentUser; set => SetProperty(ref _currentUser, value); }
        public GroupsModel SelectedGroup { get => _selectedGroup; set => SetProperty(ref _selectedGroup, value); }
        public ObservableCollection<LessonsModel> Models { get => _models; set => SetProperty(ref _models, value); }

        public ObservableCollection<UserModel> Teachers { get => _teachers; set => SetProperty(ref _teachers, value); }
        public ObservableCollection<AcademicDisciplinesModel> Disciplines { get => _disciplines; set => SetProperty(ref _disciplines, value); }
        public ObservableCollection<AudiencesModel> Audiences { get => _audiences; set => SetProperty(ref _audiences, value); }
        public ObservableCollection<TypeClassesModel> TypeClasses { get => _typeClasses; set => SetProperty(ref _typeClasses, value); }
        public ObservableCollection<GroupsModel> Groups { get => _groups; set => SetProperty(ref _groups, value); }
        public ObservableCollection<TimeLessonsModel> TimeLessons { get => _timeLessons; set => SetProperty(ref _timeLessons, value); }


        #endregion
        #region commands

        #region Date clicks

        public DelegateCommand PrevDayCommand { get; private set; }
        public DelegateCommand TodayCommand { get; private set; }
        public DelegateCommand NextDayCommand { get; private set; }
        public DelegateCommand RefreshDayCommand { get; private set; }

        private DelegateCommand _groupsChangedCommand;
        public DelegateCommand GroupsChangedCommand { get => _groupsChangedCommand; set => SetProperty(ref _groupsChangedCommand, value); }

        #endregion
        #region Schedule managing

        public DelegateCommand LoadedCommand { get; private set; }

        public DelegateCommand AddCardCommand { get; private set; }
        public DelegateCommand CancelCardCommand { get; private set; }
        public DelegateCommand DeleteCardCommand { get; private set; }

        public DelegateCommand AddMaterialCommand { get; private set; }
        public DelegateCommand DeleteMaterialCommand { get; private set; }

        public DelegateCommand AddCommentCommand { get; private set; }
        public DelegateCommand EditCommentCommand { get; private set; }
        public DelegateCommand DeleteCommentCommand { get; private set; }

        #endregion

        #endregion
        #region ctor

        public ScheduleMainViewModel(
            IRegionManager manager,
            IMemoryService memory,
            IWebApiService api,
            INotificationService notification,
            IEventAggregator aggregator,
            IPermissionService permissionService)
        {
            this.manager = manager;
            this.memory = memory;
            this.api = api;
            this.notification = notification;
            this.permissionService = permissionService;
            this.aggregator = aggregator;
            aggregator.GetEvent<DateChangedEvent>().Subscribe(() => OnLoadModelsByDate(CurrentDate));
            aggregator.GetEvent<CardSelectingEvent>().Subscribe(OnCardSelecting);

            Models = new();
            memory.StoreItem("TimeLessons", Models);
            CurrentUser = memory.GetItem<UserModel>(MemoryAlliases.CurrentUser);
            CurrentDate = DateTime.Now;

            #region Date clicks

            PrevDayCommand = new DelegateCommand(OnPrevCLick);
            TodayCommand = new DelegateCommand(OnTodayClick);
            NextDayCommand = new DelegateCommand(OnNextClick);
            RefreshDayCommand = new DelegateCommand(OnRefreshDay);

            #endregion
            #region Schedule managing

            AddCardCommand = new DelegateCommand(OnAddScheduleCard);
            CancelCardCommand = new DelegateCommand(OnCancelCard);
            DeleteCardCommand = new DelegateCommand(OnDeleteCard);

            AddMaterialCommand = new DelegateCommand(OmAddMaterial);
            DeleteMaterialCommand = new DelegateCommand(OnDeleteMaterial);

            AddCommentCommand = new DelegateCommand(OnAddComment);
            EditCommentCommand = new DelegateCommand(OnEditComment);
            DeleteCommentCommand = new DelegateCommand(OnDeleteComment);

            #endregion
        }

        #endregion
        #region methods

        #region Загрузка данных

        private async Task LoadingData()
        {
            if(_firstRun)
            {
                Groups = new ObservableCollection<GroupsModel>(await api.GetModels<GroupsModel>(WebApiTableNames.Groups));
                SelectedGroup = Groups.FirstOrDefault();
                GroupsChangedCommand = new DelegateCommand(OnGroupsChanged);

                _firstRun = false;
            }
            Teachers = new ObservableCollection<UserModel>(await api.GetModels<UserModel>(WebApiTableNames.Users));
            Disciplines = new ObservableCollection<AcademicDisciplinesModel>(await api.GetModels<AcademicDisciplinesModel>(WebApiTableNames.AcademicDisciplines));
            Audiences = new ObservableCollection<AudiencesModel>(await api.GetModels<AudiencesModel>(WebApiTableNames.Audiences));
            TypeClasses = new ObservableCollection<TypeClassesModel>(await api.GetModels<TypeClassesModel>(WebApiTableNames.TypeClasses));
            TimeLessons = new ObservableCollection<TimeLessonsModel>(await api.GetModels<TimeLessonsModel>(WebApiTableNames.TimeLessons));

            await Config.SetVisibilities(permissionService);
            await Config.ReadColors();
            memory.StoreItem<TypeClassColors>(nameof(TypeClassColors), Config.TypeClassColors);
        } 

        #endregion
        #region Date clicks

        private void OnPrevCLick() => CurrentDate = CurrentDate.AddDays(-1);
        private void OnTodayClick() => CurrentDate = CurrentDate = DateTime.Now;
        private void OnNextClick() => CurrentDate = CurrentDate.AddDays(1);
        private void OnRefreshDay() => OnLoadModelsByDate(_currentDate);
        private void OnGroupsChanged() => OnLoadModelsByDate(_currentDate);

        private async void OnLoadModelsByDate(DateTime date)
        {
            try
            {
                ScheduleLoader = new("Выполняется загрузка");

                Task load = Task.Run(async () =>
                {
                    Task loading = Task.Run(async () => await LoadingData());
                    await Task.WhenAll(loading);

                    Models.Clear();

                    string condition = SelectedGroup == null ? $" Day = '{date.ToShortDateString()}'" : $" Day = '{date.ToShortDateString()}' and IdGroup = '{SelectedGroup.Id}'";
                    List <LessonsModel> lessons = await api.GetModels<LessonsModel>(WebApiTableNames.Lessons, condition);

                    foreach (LessonsModel lesson in lessons)
                    {
                        LessonsModel lm = new();
                        lm.Id = lesson.Id;
                        lm.Day = lesson.Day;
                        lm.Teacher = Teachers.FirstOrDefault(d => d.ID == lesson.IdTeacher);
                        lm.AcademicDiscipline = Disciplines.FirstOrDefault(d => d.Id == lesson.IdAcademicDiscipline);
                        lm.Audience = Audiences.FirstOrDefault(d => d.Id == lesson.IdAudience);
                        lm.TypeClass = TypeClasses.FirstOrDefault(d => d.Id == lesson.IdTypeClass);
                        lm.Group = Groups.FirstOrDefault(d => d.Id == lesson.IdGroup);
                        lm.TimeLessons = TimeLessons.FirstOrDefault(d => d.Id == lesson.IdTimeLessons);

                        Models.Add(lm);
                    }
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        OnDateNavigated();
                    });

                    await Task.Delay(500);
                });
                await Task.WhenAll(load);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ScheduleLoader = new();
            }
        }
        private void OnDateNavigated()
        {
            if (!manager.Regions.ContainsRegionWithName(BaseMethods.RegionNames.ScheduleDateViewRegion)) return;
            manager.Regions[BaseMethods.RegionNames.ScheduleDateViewRegion].RemoveAll();
            manager.RequestNavigate(BaseMethods.RegionNames.ScheduleDateViewRegion, nameof(ScheduleDateViewer));
        }

        #endregion
        #region Schedule managing

        #region Метод выбора карточки

        private void OnCardSelecting(KeyValuePair<string, bool> pair)
        {
            LessonsModel card = Models.FirstOrDefault(c => c.Id.ToString().ToUpper() == pair.Key.ToUpper());
            card.IsSelected = pair.Value;
            IsCardActionsEnabled = !Models.All(m => !m.IsSelected);
        }

        #endregion
        #region Управление карточками

        private void OnAddScheduleCard() => manager.RequestNavigate(BaseMethods.RegionNames.ModalRegion, nameof(AddScheduleView));
        private void OnCancelCard()
        {
        }
        private async void OnDeleteCard()
        {
            bool confirm = notification.ShowQuestion("Восстановить запись невозможно, продолжить действие?");
            if (!confirm) return;
            ScheduleLoader = new("Выполняется загрузка");

            LessonsModel model = Models.FirstOrDefault(m => m.IsSelected);
            await api.DeleteModel(model.Id, WebApiTableNames.Lessons);
            aggregator.GetEvent<GridChildChangedEvent>().Publish(new object[] { model, true });
            Models.Remove(model);

            ScheduleLoader = new();
        }

        #endregion
        #region Управление материалами

        private void OmAddMaterial()
        {
        }
        private void OnDeleteMaterial()
        {
        }

        #endregion
        #region Управление комментариями

        private void OnAddComment()
        {
        }
        private void OnEditComment()
        {
        }
        private void OnDeleteComment()
        {
        }

        #endregion

        #endregion

        #endregion
    }
}
