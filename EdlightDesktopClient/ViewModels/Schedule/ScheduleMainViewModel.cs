using ApplicationEventsWPF.Events.ScheduleEvents;
using ApplicationModels;
using ApplicationModels.Config;
using ApplicationModels.Models;
using ApplicationServices.PermissionService;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.MemoryService;
using ApplicationWPFServices.NotificationService;
using EdlightDesktopClient.AccessConfigurations;
using EdlightDesktopClient.Views.Schedule;
using HandyControl.Controls;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
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

        #endregion
        #region fields

        private bool _firstRun = true;
        private bool _isCardActionsEnabled;
        private bool _isCardCancelingEnabled;
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
            IPermissionService permissionService)
        {
            HelpTipText = $"Для начала работы необходимо выбрать запись в расписании или создать новую.{Environment.NewLine}(двойной клик левой кнопки мыши)";
            Loader = new();

            this.manager = manager;
            this.memory = memory;
            this.api = api;
            this.notification = notification;
            this.permissionService = permissionService;
            this.aggregator = aggregator;
            aggregator.GetEvent<DateChangedEvent>().Subscribe(() => OnLoadModelsByDate(CurrentDate));
            aggregator.GetEvent<CardSelectingEvent>().Subscribe(OnCardSelecting);
            FilteredMaterials.Filter += MaterialsFilter;
            FilteredComments.Filter += CommentsFilter;

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
            await Config.ReadColors();

            SetHelpTipVisibility();

            memory.StoreItem(nameof(TypeClassColors), Config.TypeClassColors);
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
            manager.RequestNavigate(BaseMethods.RegionNames.ScheduleDateViewRegion, nameof(ScheduleDateViewer));
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
                Models.Remove(model);

                IsCardActionsEnabled = false;
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

            await api.PostModel(com, WebApiTableNames.Comments);

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
