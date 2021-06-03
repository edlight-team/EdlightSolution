using ApplicationEventsWPF.Events.ScheduleEvents;
using ApplicationEventsWPF.Events.Signal;
using ApplicationModels;
using ApplicationModels.Models;
using ApplicationServices.PermissionService;
using ApplicationServices.SignalClientSerivce;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.MemoryService;
using ApplicationWPFServices.NotificationService;
using EdlightDesktopClient.AccessConfigurations;
using EdlightDesktopClient.Views.Schedule;
using HandyControl.Controls;
using Microsoft.Win32;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Styles.Extensions;
using Styles.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

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
        private readonly ISignalClientService signal;

        #endregion
        #region fields

        private bool _firstRun = true;
        private bool _isCardActionsEnabled;
        private bool _isCardCancelingEnabled;
        private bool _isFirstHalfYearTime;
        private bool _isUpWeek;
        private Visibility _helpTextVisibility;
        private string _helpTipText;
        private string _commentText;

        private ScheduleConfig _config;
        private LoaderModel _loader;
        private DateTime _currentDate;
        private UserModel _currentUser;
        private GroupsModel _selectedGroup;
        private ObservableCollection<LessonsModel> _models;
        private ObservableCollection<MaterialsModel> _materials;
        private CollectionViewSource _filteredMaterials;
        private ObservableCollection<CommentModel> _comments;
        private CollectionViewSource _filteredComments;

        private ObservableCollection<UserModel> _teachers;
        private ObservableCollection<AcademicDisciplinesModel> _disciplines;
        private ObservableCollection<AudiencesModel> _audiences;
        private ObservableCollection<TypeClassesModel> _typeClasses;
        private ObservableCollection<GroupsModel> _groups;
        private ObservableCollection<TimeLessonsModel> _timeLessons;

        #endregion
        #region props

        public bool IsCardActionsEnabled { get => _isCardActionsEnabled; set => SetProperty(ref _isCardActionsEnabled, value); }
        public bool IsCardCancelingEnabled { get => _isCardCancelingEnabled; set => SetProperty(ref _isCardCancelingEnabled, value); }
        public bool IsFirstHalfYearTime { get => _isFirstHalfYearTime; set => SetProperty(ref _isFirstHalfYearTime, value); }
        public bool IsUpWeek { get => _isUpWeek; set => SetProperty(ref _isUpWeek, value); }
        public Visibility HelpTextVisibility { get => _helpTextVisibility; set => SetProperty(ref _helpTextVisibility, value); }
        public string HelpTipText { get => _helpTipText; set => SetProperty(ref _helpTipText, value); }
        public string CommentText { get => _commentText; set => SetProperty(ref _commentText, value); }

        public ScheduleConfig Config { get => _config ??= new(); set => SetProperty(ref _config, value); }
        public LoaderModel Loader { get => _loader; set => SetProperty(ref _loader, value); }
        public DateTime CurrentDate
        {
            get => _currentDate;
            set
            {
                //Проверяем семестр
                DateTime autumnDateStart = new(value.Year, 09, 01);
                DateTime springDateStart = new(value.Year, 02, 09);
                IsFirstHalfYearTime = value.DayOfYear < autumnDateStart.DayOfYear;

                int week = value.DayOfYear / 7;
                int autumnWeekStart = autumnDateStart.DayOfYear / 7;
                int springWeekStart = springDateStart.DayOfYear / 7;

                if (IsFirstHalfYearTime)
                {
                    IsUpWeek = (week - springWeekStart) % 2 == 0;
                }
                else
                {
                    IsUpWeek = (week - autumnWeekStart) % 2 == 0;
                }

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
        public ObservableCollection<MaterialsModel> Materials { get => _materials; set => SetProperty(ref _materials, value); }
        public CollectionViewSource FilteredMaterials { get => _filteredMaterials ??= new(); set => SetProperty(ref _filteredMaterials, value); }
        public ObservableCollection<CommentModel> Comments { get => _comments; set => SetProperty(ref _comments, value); }
        public CollectionViewSource FilteredComments { get => _filteredComments ??= new(); set => SetProperty(ref _filteredComments, value); }

        public ObservableCollection<UserModel> Teachers { get => _teachers; set => SetProperty(ref _teachers, value); }
        public ObservableCollection<AcademicDisciplinesModel> Disciplines { get => _disciplines; set => SetProperty(ref _disciplines, value); }
        public ObservableCollection<AudiencesModel> Audiences { get => _audiences; set => SetProperty(ref _audiences, value); }
        public ObservableCollection<TypeClassesModel> TypeClasses { get => _typeClasses; set => SetProperty(ref _typeClasses, value); }
        public ObservableCollection<GroupsModel> Groups { get => _groups; set => SetProperty(ref _groups, value); }
        public ObservableCollection<TimeLessonsModel> TimeLessons { get => _timeLessons; set => SetProperty(ref _timeLessons, value); }


        #endregion
        #region commands

        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand UnloadedCommand { get; private set; }

        #region Date clicks

        public DelegateCommand PrevDayCommand { get; private set; }
        public DelegateCommand TodayCommand { get; private set; }
        public DelegateCommand NextDayCommand { get; private set; }
        public DelegateCommand RefreshDayCommand { get; private set; }

        private DelegateCommand _groupsChangedCommand;
        public DelegateCommand GroupsChangedCommand { get => _groupsChangedCommand; set => SetProperty(ref _groupsChangedCommand, value); }

        #endregion
        #region Schedule managing

        public DelegateCommand AddCardCommand { get; private set; }
        public DelegateCommand ImportCardsCommand { get; private set; }
        public DelegateCommand CapacityManagingCommand { get; private set; }
        public DelegateCommand EditCardCommand { get; private set; }
        public DelegateCommand CancelCardCommand { get; private set; }
        public DelegateCommand DeleteCardCommand { get; private set; }

        public DelegateCommand AddMaterialCommand { get; private set; }
        public DelegateCommand<object> LoadMaterialCommand { get; private set; }
        public DelegateCommand<object> DeleteMaterialCommand { get; private set; }

        public DelegateCommand AddCommentCommand { get; private set; }
        public DelegateCommand<object> DeleteCommentCommand { get; private set; }

        #endregion

        #endregion
        #region ctor

        public ScheduleMainViewModel(
            IRegionManager manager,
            IMemoryService memory,
            IWebApiService api,
            INotificationService notification,
            IEventAggregator aggregator,
            IPermissionService permissionService,
            ISignalClientService signal)
        {
            HelpTipText = $"Для начала работы необходимо выбрать запись в расписании или создать новую.{Environment.NewLine}(двойной клик левой кнопки мыши)";
            Loader = new();

            this.manager = manager;
            this.memory = memory;
            this.api = api;
            this.notification = notification;
            this.permissionService = permissionService;
            this.aggregator = aggregator;
            this.signal = signal;
            signal.SubscribeEntityChanged(SignalEntityChanged);
            aggregator.GetEvent<DateChangedEvent>().Subscribe(() => OnLoadModelsByDate(CurrentDate));
            aggregator.GetEvent<CardSelectingEvent>().Subscribe(OnCardSelecting);
            aggregator.GetEvent<SignalEntitySendEvent>().Subscribe(OnSignalEntitySend);
            FilteredMaterials.Filter += MaterialsFilter;
            FilteredComments.Filter += CommentsFilter;

            Models = new();
            memory.StoreItem(nameof(Models), Models);
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
            ImportCardsCommand = new DelegateCommand(OnImportCards);
            CapacityManagingCommand = new DelegateCommand(OnCapacityManagment);
            EditCardCommand = new DelegateCommand(OnEditScheduleCard);
            CancelCardCommand = new DelegateCommand(OnCancelCard);
            DeleteCardCommand = new DelegateCommand(OnDeleteCard);

            AddMaterialCommand = new DelegateCommand(OnAddMaterial);
            LoadMaterialCommand = new DelegateCommand<object>(OnLoadMaterial, CanLoadMaterial);
            DeleteMaterialCommand = new DelegateCommand<object>(OnDeleteMaterial, CanDeleteMaterial);

            AddCommentCommand = new DelegateCommand(OnAddComment, () => !string.IsNullOrEmpty(CommentText)).ObservesProperty(() => CommentText);
            DeleteCommentCommand = new DelegateCommand<object>(OnDeleteComment);

            #endregion
        }

        #endregion
        #region methods

        #region Signal received

        private void SignalEntityChanged(string serialized)
        {
        }

        #endregion
        #region Signal sending

        private void OnSignalEntitySend(EntitySignalModel model) => signal.SendEntityModel(JsonConvert.SerializeObject(model));

        #endregion
        #region Загрузка данных

        private async Task LoadingData()
        {
            if (_firstRun)
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

            memory.StoreItem(nameof(Teachers), Teachers);
            memory.StoreItem(nameof(Disciplines), Disciplines);
            memory.StoreItem(nameof(Audiences), Audiences);
            memory.StoreItem(nameof(TypeClasses), TypeClasses);
            memory.StoreItem(nameof(TimeLessons), TimeLessons);
            memory.StoreItem(nameof(Groups), Groups);

            Materials = new ObservableCollection<MaterialsModel>(await api.GetModels<MaterialsModel>(WebApiTableNames.Materials));
            foreach (MaterialsModel mat in Materials)
            {
                mat.LoadMaterialCommand = LoadMaterialCommand;
                mat.DeleteMaterialCommand = DeleteMaterialCommand;
            }
            FilteredMaterials.Source = Materials;

            Comments = new();
            List<CommentModel> api_comments = await api.GetModels<CommentModel>(WebApiTableNames.Comments);
            api_comments.Reverse();
            foreach (CommentModel comment in api_comments)
            {
                comment.User = Teachers.FirstOrDefault(t => t.ID == comment.IdUser);
                comment.ContextMenuVisibility = CurrentUser.ID == comment.IdUser ? Visibility.Visible : Visibility.Collapsed;
                comment.DeleteCommentCommand = DeleteCommentCommand;
                Comments.Add(comment);
            }
            FilteredComments.Source = Comments;

            await Config.SetVisibilities(permissionService);

            SetHelpTipVisibility();

            memory.StoreItem(nameof(ScheduleConfig), Config);
            memory.StoreItem(nameof(Comments), Comments);
            ClearSelected();
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
                Loader.SetDefaultLoadingInfo();
                await LoadingData();

                Models.Clear();

                string condition = SelectedGroup == null ? $" Day = '{date.ToShortDateString()}'" : $" Day = '{date.ToShortDateString()}' and IdGroup = '{SelectedGroup.Id}'";
                List<LessonsModel> lessons = await api.GetModels<LessonsModel>(WebApiTableNames.Lessons, condition);

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
                    lm.CanceledReason = lesson.CanceledReason;

                    Models.Add(lm);
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    OnDateNavigated();
                });
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
        private void OnDateNavigated()
        {
            if (!manager.Regions.ContainsRegionWithName(BaseMethods.RegionNames.ScheduleDateViewRegion)) return;
            manager.Regions[BaseMethods.RegionNames.ScheduleDateViewRegion].RemoveAll();
            manager.RequestNavigate(BaseMethods.RegionNames.ScheduleDateViewRegion, nameof(ScheduleDateViewer), new NavigationParameters()
            {
                { "CurrentDate", CurrentDate },
                { "SelectedGroup", SelectedGroup }
            });
        }

        #endregion
        #region Schedule managing

        #region Метод выбора карточки

        private void SetHelpTipVisibility() => HelpTextVisibility = IsCardActionsEnabled ? Visibility.Collapsed : Visibility.Visible;
        private void OnCardSelecting(KeyValuePair<string, bool> pair)
        {
            LessonsModel card = Models.FirstOrDefault(c => c.Id.ToString().ToUpper() == pair.Key.ToUpper());
            card.IsSelected = pair.Value;
            foreach (LessonsModel other in Models.Where(c => c.Id != card.Id))
            {
                other.IsSelected = false;
            }
            card.IsCanceled = !string.IsNullOrEmpty(card.CanceledReason);
            IsCardActionsEnabled = !Models.All(m => !m.IsSelected);
            IsCardCancelingEnabled = IsCardActionsEnabled && !card.IsCanceled;
            FilteredMaterials?.View?.Refresh();
            SetHelpTipVisibility();
        }
        private void ClearSelected()
        {
            foreach (LessonsModel card in Models)
            {
                card.IsSelected = false;
            }
            IsCardActionsEnabled = false;
            IsCardCancelingEnabled = false;
            SetHelpTipVisibility();
        }

        #endregion
        #region Управление карточками

        private void OnAddScheduleCard() => manager.RequestNavigate(BaseMethods.RegionNames.ModalRegion, nameof(AddScheduleView), new NavigationParameters()
            {
                { nameof(CurrentDate), CurrentDate },
                { nameof(SelectedGroup), SelectedGroup }
            });
        private void OnEditScheduleCard() => manager.RequestNavigate(BaseMethods.RegionNames.ModalRegion, nameof(AddScheduleView), new NavigationParameters()
            {
                { "EditModel", Models.FirstOrDefault(m=>m.IsSelected) }
            });
        private void OnCancelCard() => manager.RequestNavigate(BaseMethods.RegionNames.ModalRegion, nameof(CancelScheduleRecordView), new NavigationParameters()
            {
                { "RecordId", Models.FirstOrDefault(m=>m.IsSelected).Id }
            });
        private async void OnDeleteCard()
        {
            bool confirm = notification.ShowQuestion("Восстановить запись невозможно, продолжить действие?");
            if (!confirm) return;
            try
            {
                Loader.SetDefaultLoadingInfo();

                LessonsModel model = Models.FirstOrDefault(m => m.IsSelected);
                await api.DeleteModel(model.Id, WebApiTableNames.Lessons);
                await api.DeleteModel(model.TimeLessons.Id, WebApiTableNames.TimeLessons);
                aggregator.GetEvent<GridChildChangedEvent>().Publish(new object[] { model, true });
                aggregator.GetEvent<SignalEntitySendEvent>().Publish(new EntitySignalModel()
                {
                    SendType = "DELETE",
                    ModelType = typeof(LessonsModel),
                    SerializedModel = JsonConvert.SerializeObject(model)
                });
                Models.Remove(model);

                IsCardActionsEnabled = false;
                SetHelpTipVisibility();
                Growl.Info("Запись успешно Удалена", "Global");
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
        private async void OnImportCards()
        {
            OpenFileDialog ofd = new();
            ofd.Filter = "Edlight schedule table (*.xlsx)|*.xlsx";
            bool? result = ofd.ShowDialog();
            if (!result.Value) return;
            int rowCounter = 1;
            int importCounter = 0;
            try
            {
                using ExcelDataReader.IExcelDataReader reader = ExcelDataReader.ExcelReaderFactory.CreateReader(File.OpenRead(ofd.FileName));
                reader.Read();

                if (
                    reader.GetString(0) != @"№ П\П" ||
                    reader.GetString(1) != @"Преподаватель     Ф" ||
                    reader.GetString(2) != @"И" ||
                    reader.GetString(3) != @"О" ||
                    reader.GetString(4) != @"Дисциплина" ||
                    reader.GetString(5) != @"Аудитория" ||
                    reader.GetString(6) != @"Тип занятия" ||
                    reader.GetString(7) != @"Группа" ||
                    reader.GetString(8) != @"Начало занятия" ||
                    reader.GetString(9) != @"Конец занятия" ||
                    reader.GetString(10) != @"Дата занятия" ||
                    reader.GetString(11) != @"Перерыв(мин)"
                   )
                {
                    Growl.Error("При чтении таблицы импорта произошла ошибка", "Global");
                    return;
                }

                while (reader.Read())
                {
                    try
                    {
                        Loader.SetCountLoadingInfo(rowCounter++, reader.RowCount - 1);

                        string sName = reader.GetString(1);
                        string fName = reader.GetString(2);
                        string patr = reader.GetString(3);
                        UserModel teach = Teachers.FirstOrDefault(t => t.Name == fName && t.Surname == sName && t.Patrnymic == patr);
                        if (teach == null) continue;

                        string disc_name = reader.GetString(4);
                        AcademicDisciplinesModel disc = Disciplines.FirstOrDefault(d => d.Title == disc_name);
                        if (disc == null) continue;

                        string audience_name = reader.GetString(5);
                        AudiencesModel audience = Audiences.FirstOrDefault(a => a.NumberAudience == audience_name);
                        if (audience == null) continue;

                        string type_class_name = reader.GetString(6);
                        TypeClassesModel type_class = TypeClasses.FirstOrDefault(a => a.Title == type_class_name);
                        if (type_class == null) continue;

                        string group_name = reader.GetString(7);
                        GroupsModel group = Groups.FirstOrDefault(a => a.Group == group_name);
                        if (group == null) continue;

                        string start_time = reader.GetDateTime(8).ToString("hh:mm");
                        string end_time = reader.GetDateTime(9).ToString("hh:mm");
                        DateTime date = reader.GetDateTime(10);
                        string break_time = reader.GetDouble(11).ToString();

                        TimeLessonsModel tms = new()
                        {
                            StartTime = start_time,
                            EndTime = end_time,
                            BreakTime = break_time
                        };

                        TimeLessonsModel posted = await api.PostModel(tms, WebApiTableNames.TimeLessons);

                        LessonsModel lm = new()
                        {
                            Day = date,
                            IdTeacher = teach.ID,
                            IdAcademicDiscipline = disc.Id,
                            IdAudience = audience.Id,
                            IdTypeClass = type_class.Id,
                            IdGroup = group.Id,
                            IdTimeLessons = posted.Id
                        };

                        await api.PostModel(lm, WebApiTableNames.Lessons);
                        importCounter++;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                Growl.Info("Импорт успешно завершен, добавлено записей " + importCounter, "Global");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                OnLoadModelsByDate(_currentDate);
                await Loader.Clear();
            }
        }
        private async void OnCapacityManagment()
        {
            OpenFileDialog ofd = new();
            //ofd.Filter = "Edlight capacity table (*.xls)|*.xls";
            //bool? result = ofd.ShowDialog();
            //if (!result.Value) return;

            ofd.FileName = Environment.CurrentDirectory + "\\Нагрузка.xls";

            ObservableCollection<CapacityModel> capacities = new();
            ExcelDataReader.IExcelDataReader reader = null;
            int rowCounter = 1;
            try
            {
                reader = ExcelDataReader.ExcelReaderFactory.CreateReader(File.OpenRead(ofd.FileName));
                for (int i = 0; i < 5; i++)
                {
                    Loader.SetCountLoadingInfo(rowCounter++, reader.RowCount);
                    reader.Read();
                }
                while (reader.Read())
                {
                    Loader.SetCountLoadingInfo(rowCounter++, reader.RowCount);
                    CapacityModel capacity = new();

                    capacity.NumberA = reader[0]?.ToString().ToDouble();
                    capacity.NumberB = reader[1]?.ToString().ToDouble();
                    capacity.Syllabus = reader[2]?.ToString();
                    capacity.Faculty = reader[3]?.ToString();
                    capacity.Block = reader[4]?.ToString();
                    capacity.DisciplineOrWorkType = reader[5]?.ToString();
                    capacity.AssignedDepartment = reader[6]?.ToString();
                    capacity.CourseOrSemesterAndSession = reader[7]?.ToString();
                    capacity.Group = reader[8]?.ToString();
                    capacity.StudentsCount = reader[9]?.ToString().ToDouble();
                    capacity.WeekCount = reader[10]?.ToString().ToDouble();
                    capacity.ClassType = reader[11]?.ToString();
                    capacity.HoursOnStreamOrGroupOrStudent = reader[12]?.ToString().ToDouble();
                    capacity.ControlsType = reader[13]?.ToString();
                    capacity.KSR = reader[14]?.ToString().ToDouble();
                    capacity.IndividualCount = reader[15]?.ToString().ToDouble();
                    capacity.ControlsCount = reader[16]?.ToString().ToDouble();
                    capacity.RatingCount = reader[17]?.ToString().ToDouble();
                    capacity.Referates = reader[18]?.ToString().ToDouble();
                    capacity.Essay = reader[19]?.ToString().ToDouble();
                    capacity.RGR = reader[20]?.ToString().ToDouble();
                    capacity.ControlWorksForAsentia = reader[21]?.ToString().ToDouble();
                    capacity.ConsultingSPO = reader[22]?.ToString().ToDouble();
                    capacity.AudithoryCapacity = reader[23]?.ToString().ToDouble();
                    capacity.OtherCapacity = reader[24]?.ToString().ToDouble();
                    capacity.OtherCapacityWithInfo = reader[25]?.ToString();
                    capacity.TotalCapacity = reader[26]?.ToString().ToDouble();
                    capacity.TeacherFio = reader[27]?.ToString();
                    capacity.TeacherPosition = reader[28]?.ToString();
                    capacity.TeacherRange = reader[29]?.ToString();
                    capacity.FlowNumber = reader[30]?.ToString().ToDouble();
                    capacity.FirstGroupFlowIndicator = reader[31]?.ToString().ToDouble();
                    capacity.AudRecommendSyllabus = reader[32]?.ToString();
                    capacity.AdditionalHourStudent = reader[33]?.ToString().ToDouble();
                    capacity.AdditionalHourGroup = reader[34]?.ToString().ToDouble();
                    capacity.InFactDoneA = reader[35]?.ToString().ToDouble();
                    capacity.InFactDoneB = reader[36]?.ToString().ToDouble();
                    capacity.DateFrom = reader[37]?.ToString().ToDate();
                    capacity.DateTo = reader[38]?.ToString().ToDate();
                    capacity.Budget = reader[39]?.ToString().ToDouble();
                    capacity.ExtraBudget = reader[40]?.ToString().ToDouble();
                    capacity.Foreign = reader[41]?.ToString().ToDouble();
                    capacity.ExtraBudgetDole = reader[42]?.ToString().ToDouble();
                    capacity.ForeignDole = reader[43]?.ToString().ToDouble();
                    capacity.TotalContractForeign = reader[44]?.ToString();
                    capacity.EducationLevel = reader[45]?.ToString();
                    capacity.LearnType = reader[46]?.ToString();
                    capacity.ExaminationHour = reader[47]?.ToString().ToDouble();
                    capacity.OwnWorkHour = reader[48]?.ToString().ToDouble();
                    capacity.ElectronicHour = reader[49]?.ToString().ToDouble();
                    capacity.NormCoef = reader[50]?.ToString().ToDouble();
                    capacity.Note = reader[51]?.ToString();
                    capacity.ZET = reader[52]?.ToString().ToDouble();
                    capacity.QuestionCount = reader[53]?.ToString().ToDouble();
                    capacity.HourAtWeek = reader[54]?.ToString().ToDouble();

                    if (capacity.HourAtWeek <= 1) continue;
                    if (double.IsNaN(capacity.AudithoryCapacity.Value)) continue;
                    if (capacity.DateFrom == new DateTime() || capacity.DateFrom == null) continue;

                    capacities.Add(capacity);
                    //await Task.Delay(1);
                }
                await Loader.Clear();
                manager.RequestNavigate(BaseMethods.RegionNames.ModalRegion, nameof(CapacityManagmentView), new NavigationParameters() { { "Capacities", capacities } });
            }
            catch (Exception)
            {
                Growl.Error("При чтении файла нагрузки произошла ошибка", "Global");
                throw;
            }
            finally
            {
                if (Loader.IsActive)
                {
                    await Loader.Clear();
                }
                reader.Close();
            }
        }

        #endregion
        #region Управление материалами

        protected void MaterialsFilter(object sender, FilterEventArgs e)
        {
            MaterialsModel material = e.Item as MaterialsModel;
            if (Models.Any(m => m.IsSelected))
            {
                LessonsModel selected = Models.FirstOrDefault(m => m.IsSelected);
                if (material.IdLesson != selected.Id)
                {
                    e.Accepted = false;
                }
            }
        }
        private async void OnAddMaterial()
        {
            OpenFileDialog ofd = new();
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ofd.Multiselect = true;
            ofd.RestoreDirectory = true;
            ofd.Title = "Выберите файлы для отправки";
            bool? result = ofd.ShowDialog();
            if (result.Value)
            {
                try
                {
                    Loader.SetDefaultLoadingInfo();
                    LessonsModel targetModel = Models.FirstOrDefault(m => m.IsSelected);

                    int fileCounter = 0;
                    foreach (string fileName in ofd.FileNames)
                    {
                        FileInfo fi = new(fileName);

                        byte[] data = File.ReadAllBytes(fileName);

                        string api_response = await api.PushFile($"//{targetModel.Id}//", new ApplicationModels.JsonFileModel() { FileName = fi.Name, Data = data });
                        if (api_response == "Файл успешно сохранен") fileCounter++;


                        List<MaterialsModel> equal_mm = await api.GetModels<MaterialsModel>(WebApiTableNames.Materials, $"IdLesson = '{targetModel.Id}' and Title = '{fi.Name}'");
                        if (equal_mm.Count != 0)
                        {
                            MaterialsModel equal = equal_mm.FirstOrDefault();
                            equal.IdUser = targetModel.Teacher.ID;
                            equal.Title = fi.Name;
                            equal.MaterialPath = $"//{targetModel.Id}//";

                            await api.PutModel(equal, WebApiTableNames.Materials);
                        }
                        else
                        {
                            MaterialsModel mm = new();
                            mm.IdLesson = targetModel.Id;
                            mm.IdUser = targetModel.Teacher.ID;
                            mm.Title = fi.Name;
                            mm.MaterialPath = $"//{targetModel.Id}//";

                            await api.PostModel(mm, WebApiTableNames.Materials);
                        }
                    }
                    Materials.Clear();
                    List<MaterialsModel> api_materials = await api.GetModels<MaterialsModel>(WebApiTableNames.Materials);
                    foreach (MaterialsModel mat in api_materials)
                    {
                        mat.LoadMaterialCommand = LoadMaterialCommand;
                        mat.DeleteMaterialCommand = DeleteMaterialCommand;
                        Materials.Add(mat);
                    }
                    FilteredMaterials?.View?.Refresh();
                    Growl.Info($"Успешно добавлено {fileCounter} материалов", "Global");
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
        }

        private bool CanLoadMaterial(object material) => Config.CanGetMaterial == Visibility.Visible;
        private async void OnLoadMaterial(object material)
        {
            if (material is MaterialsModel mm)
            {
                SaveFileDialog sfd = new();
                sfd.FileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + mm.Title;
                sfd.Title = "Сохранение материала";
                string ext = mm.Title.Remove(0, mm.Title.IndexOf('.'));
                sfd.Filter = $"Edlight Material - Материал ({ext})|*{ext}";
                bool? result = sfd.ShowDialog();
                if (result.Value)
                {
                    try
                    {
                        Loader.SetDefaultLoadingInfo();
                        object j_data = await api.GetFile(mm.MaterialPath + mm.Title);
                        if (j_data is JsonFileModel file)
                        {
                            File.WriteAllBytes(sfd.FileName, file.Data);
                        }
                        Growl.Info($"Материал успешно загружен", "Global");
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
            }
        }

        private bool CanDeleteMaterial(object material) => Config.CanDeleteMaterial == Visibility.Visible;
        private async void OnDeleteMaterial(object material)
        {
            bool confirm = notification.ShowQuestion("Восстановить материал невозможно, продолжить действие?");
            if (!confirm) return;
            if (material is MaterialsModel mm)
            {
                try
                {
                    Loader.SetDefaultLoadingInfo();

                    string delete_result = await api.DeleteFile(mm.MaterialPath + mm.Title);
                    if (delete_result != "Файл успешно удален")
                    {
                        throw new Exception("Ошибка при удалении файла");
                    }

                    int delete_material = await api.DeleteModel(mm.Id, WebApiTableNames.Materials);
                    if (delete_material != 1)
                    {
                        throw new Exception("Ошибка при удалении материала");
                    }

                    Materials.Remove(mm);
                    Growl.Info($"Материал успешно удален", "Global");
                    FilteredMaterials?.View?.Refresh();
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
        }

        #endregion
        #region Управление комментариями

        protected void CommentsFilter(object sender, FilterEventArgs e)
        {
            CommentModel com = e.Item as CommentModel;
            if (Models.Any(m => m.IsSelected))
            {
                LessonsModel selected = Models.FirstOrDefault(m => m.IsSelected);
                if (com.IdLesson != selected.Id)
                {
                    e.Accepted = false;
                }
            }
        }
        private async void OnAddComment()
        {
            LessonsModel model = Models.FirstOrDefault(m => m.IsSelected);

            CommentModel com = new();
            com.IdLesson = model.Id;
            com.IdUser = CurrentUser.ID;
            com.Message = CommentText;
            com.Date = DateTime.Now;

            var posted = await api.PostModel(com, WebApiTableNames.Comments);

            Comments.Clear();
            List<CommentModel> api_comments = await api.GetModels<CommentModel>(WebApiTableNames.Comments);
            api_comments.Reverse();
            foreach (CommentModel comment in api_comments)
            {
                comment.User = Teachers.FirstOrDefault(t => t.ID == comment.IdUser);
                comment.ContextMenuVisibility = CurrentUser.ID == comment.IdUser ? Visibility.Visible : Visibility.Collapsed;
                comment.DeleteCommentCommand = DeleteCommentCommand;
                Comments.Add(comment);
            }
            FilteredComments?.View?.Refresh();

            CommentText = string.Empty;
            Growl.Info("Комментарий успешно добавлен", "Global");
            aggregator.GetEvent<CommentsChangedEvent>()
                .Publish(new KeyValuePair<string, IEnumerable<CommentModel>>(posted.IdLesson.ToString(), Comments.Where(c => c.IdLesson == posted.IdLesson)));
        }
        private async void OnDeleteComment(object commentModel)
        {
            if (commentModel is CommentModel com)
            {
                try
                {
                    int result = await api.DeleteModel(com.Id, WebApiTableNames.Comments);
                    if (result != 1)
                    {
                        throw new Exception("При попытке удаления комментария произошла ошибка");
                    }
                    Comments.Remove(com);
                    aggregator.GetEvent<CommentsChangedEvent>()
                        .Publish(new KeyValuePair<string, IEnumerable<CommentModel>>(com.IdLesson.ToString(), Comments.Where(c => c.IdLesson == com.IdLesson)));
                    Growl.Info("Комментарий успешно удален", "Global");
                    FilteredComments?.View?.Refresh();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
