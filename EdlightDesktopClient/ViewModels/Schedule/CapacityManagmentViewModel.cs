using ApplicationModels;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Linq;
using Styles.Models;
using System.Collections.ObjectModel;
using ApplicationModels.Models.CapacityExtendedModels;
using ApplicationModels.Models;
using System.Collections.Generic;
using ApplicationServices.WebApiService;
using Prism.Services.Dialogs;
using EdlightDesktopClient.Views.Schedule.CapacityWindows;
using System;
using ApplicationWPFServices.DebugService;
using System.Globalization;

namespace EdlightDesktopClient.ViewModels.Schedule
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class CapacityManagmentViewModel : BindableBase, INavigationAware
    {
        #region pair times

        private readonly Dictionary<int, string> _StartTimes = new() { { 1, "8:30" }, { 2, "10:15" }, { 3, "12:30" }, { 4, "14:30" }, { 5, "16:00" }, { 6, "17:45" }, };
        private readonly Dictionary<int, string> _EndTimes = new() { { 1, "10:05" }, { 2, "11:50" }, { 3, "14:05" }, { 4, "15:50" }, { 5, "17:35" }, { 6, "19:20" }, };
        private readonly Dictionary<int, string> _BreakTimes = new() { { 1, "5" }, { 2, "5" }, { 3, "5" }, { 4, "5" }, { 5, "5" }, { 6, "0" }, };

        #endregion
        #region services

        private readonly IRegionManager manager;
        private readonly IWebApiService api;
        private readonly IDialogService dialog;
        private readonly IDebugService debug;

        #endregion
        #region fields

        private LoaderModel _loader;
        private ObservableCollection<CapacityModel> _capacities;

        private ObservableCollection<CapacityPeriodModel> _periods;
        private ObservableCollection<GroupsModel> _groups;
        private ObservableCollection<TypeClassesModel> _classTypes;
        private ObservableCollection<AudiencesModel> _audiences;
        private ObservableCollection<UserModel> _teachers;
        private ObservableCollection<AcademicDisciplinesModel> _disciplines;
        private ObservableCollection<ImportedTeacher> _importedTeachers;
        private ObservableCollection<ImportedDiscipline> _importedDisciplines;
        private bool _isAllTeachersConfirmed;
        private bool _isAllDisciplinesConfirmed;

        #endregion
        #region props

        public LoaderModel Loader { get => _loader; set => SetProperty(ref _loader, value); }
        public ObservableCollection<CapacityModel> Capacities { get => _capacities ??= new(); set => SetProperty(ref _capacities, value); }

        public ObservableCollection<CapacityPeriodModel> Periods { get => _periods ??= new(); set => SetProperty(ref _periods, value); }
        public ObservableCollection<GroupsModel> Groups { get => _groups ??= new(); set => SetProperty(ref _groups, value); }
        public ObservableCollection<TypeClassesModel> ClassTypes { get => _classTypes ??= new(); set => SetProperty(ref _classTypes, value); }
        public ObservableCollection<AudiencesModel> Audiences { get => _audiences ??= new(); set => SetProperty(ref _audiences, value); }
        public ObservableCollection<UserModel> Teachers { get => _teachers ??= new(); set => SetProperty(ref _teachers, value); }
        public ObservableCollection<AcademicDisciplinesModel> Disciplines { get => _disciplines ??= new(); set => SetProperty(ref _disciplines, value); }
        public ObservableCollection<ImportedTeacher> ImportedTeachers { get => _importedTeachers ??= new(); set => SetProperty(ref _importedTeachers, value); }
        public ObservableCollection<ImportedDiscipline> ImportedDisciplines { get => _importedDisciplines ??= new(); set => SetProperty(ref _importedDisciplines, value); }
        public bool IsAllTeachersConfirmed { get => _isAllTeachersConfirmed; set => SetProperty(ref _isAllTeachersConfirmed, value); }
        public bool IsAllDisciplinesConfirmed { get => _isAllDisciplinesConfirmed; set => SetProperty(ref _isAllDisciplinesConfirmed, value); }

        #endregion
        #region commands

        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand CloseModalCommand { get; private set; }

        public DelegateCommand AcceptAllEqualsTeachersCommand { get; private set; }
        public DelegateCommand AcceptAllEqualsDisciplinesCommand { get; private set; }
        public DelegateCommand GenerateScheduleCommand { get; private set; }
        public DelegateCommand CreateScheduleCommand { get; private set; }

        #endregion
        #region ctor

        public CapacityManagmentViewModel(IRegionManager manager, IWebApiService api, IDialogService dialog, IDebugService debug)
        {
            Loader = new();
            this.manager = manager;
            this.api = api;
            this.dialog = dialog;
            this.debug = debug;

            LoadedCommand = new DelegateCommand(OnLoaded);
            CloseModalCommand = new DelegateCommand(OnCloseModal);
            AcceptAllEqualsTeachersCommand = new DelegateCommand(OnAcceptAllEqualsTeachers);
            AcceptAllEqualsDisciplinesCommand = new DelegateCommand(OnAcceptAllEqualsDisciplines);
            GenerateScheduleCommand = new DelegateCommand(OnScheduleGenerating, () => IsAllTeachersConfirmed && IsAllDisciplinesConfirmed)
                .ObservesProperty(() => IsAllDisciplinesConfirmed)
                .ObservesProperty(() => IsAllTeachersConfirmed);
            CreateScheduleCommand = new DelegateCommand(OnCreateSchedule);
        }

        #endregion
        #region methods

        private async void OnLoaded()
        {
            try
            {
                Loader.SetDefaultLoadingInfo();
                Audiences.Clear();
                Audiences.AddRange(await api.GetModels<AudiencesModel>(WebApiTableNames.Audiences));

                Groups.Clear();
                Groups.AddRange(await api.GetModels<GroupsModel>(WebApiTableNames.Groups));

                ClassTypes.Clear();
                ClassTypes.AddRange(await api.GetModels<TypeClassesModel>(WebApiTableNames.TypeClasses));

                Teachers.Clear();
                RolesModel teach_role = (await api.GetModels<RolesModel>(WebApiTableNames.Roles, "RoleName = 'teacher'")).FirstOrDefault();
                List<UsersRolesModel> usersRoles = await api.GetModels<UsersRolesModel>(WebApiTableNames.UsersRoles, $"IdRole = '{teach_role.Id}'");
                List<UserModel> users = await api.GetModels<UserModel>(WebApiTableNames.Users);
                foreach (var item in usersRoles)
                {
                    var user = users.FirstOrDefault(u => u.ID == item.IdUser);
                    Teachers.Add(user);
                }
                Disciplines.Clear();
                Disciplines.AddRange(await api.GetModels<AcademicDisciplinesModel>(WebApiTableNames.AcademicDisciplines));

                ImportedTeachers.Clear();
                var teacher_group = Capacities.GroupBy(g => g.TeacherFio);
                foreach (var item in teacher_group)
                {
                    if (item.Key != string.Empty)
                    {
                        //Проверяем есть ли в преподавтелях найденные из нагрузки
                        ImportedTeacher imported = new();
                        imported.TeacherInitials = item.Key.Remove(0, 1);

                        var equal_by_initials = Teachers.FirstOrDefault(t => t.Initials == imported.TeacherInitials);
                        if (equal_by_initials != null)
                        {
                            imported.IsLookUpOnDB = true;
                            imported.LookUpUser = equal_by_initials;
                        }
                        else
                        {
                            imported.IsLookUpOnDB = false;
                        }
                        imported.IsConfirmedOrSkipped = false;

                        imported.StartConfirming = new DelegateCommand<object>(OnConfirmTeacherStarting);
                        imported.ConfirmCommand = new DelegateCommand<object>(OnConfirmImportTeacherModel);
                        imported.CreateTeacherCommand = new DelegateCommand<object>(OnCreateTeacher);

                        ImportedTeachers.Add(imported);
                    }
                }

                ImportedDisciplines.Clear();
                var disciplines_group = Capacities.GroupBy(g => g.DisciplineOrWorkType);
                List<string> disciplines_disctinct = new();
                foreach (var item in disciplines_group.ToList())
                {
                    if (string.IsNullOrEmpty(item.Key)) continue;
                    string discipline_name = item.Key.Trim();
                    int pg_index = discipline_name.IndexOf(", п/г ");
                    if (pg_index != -1) discipline_name = discipline_name.Remove(pg_index);
                    disciplines_disctinct.Add(discipline_name);
                }
                disciplines_disctinct = disciplines_disctinct.Distinct().ToList();
                foreach (var item in disciplines_disctinct)
                {
                    //Проверяем есть ли в преподавтелях найденные из нагрузки
                    ImportedDiscipline imported = new();
                    imported.DisciplineTitle = item;

                    var equal_by_initials = Disciplines.FirstOrDefault(t => t.Title == imported.DisciplineTitle);
                    if (equal_by_initials != null)
                    {
                        imported.IsLookUpOnDB = true;
                        imported.LookUpDiscipline = equal_by_initials;
                    }
                    else
                    {
                        imported.IsLookUpOnDB = false;
                    }
                    imported.IsConfirmedOrSkipped = false;

                    imported.StartConfirming = new DelegateCommand<object>(OnConfirmDisciplineStarting);
                    imported.ConfirmCommand = new DelegateCommand<object>(OnConfirmImportDisciplineModel);
                    imported.CreateDisciplineCommand = new DelegateCommand<object>(OnCreateDiscipline);

                    ImportedDisciplines.Add(imported);
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
        private void OnCloseModal() => manager.Regions[BaseMethods.RegionNames.ModalRegion].RemoveAll();

        #region Teachers

        private void OnConfirmTeacherStarting(object model)
        {
            if (model is ImportedTeacher it)
            {
                it.IsConfirmEnabled = true;
            }
        }
        private void OnConfirmImportTeacherModel(object model)
        {
            if (model is ImportedTeacher it)
            {
                it.IsConfirmedOrSkipped = true;
                IsAllTeachersConfirmed = ImportedTeachers.All(t => t.IsConfirmedOrSkipped);
            }
        }
        private void OnCreateTeacher(object model)
        {
            if (model is ImportedTeacher it)
            {
                dialog.ShowDialog(
                    nameof(CreateTeacherWindow),
                    new DialogParameters() { { nameof(ImportedTeacher), it } },
                    null
                    );
                IsAllTeachersConfirmed = ImportedTeachers.All(t => t.IsConfirmedOrSkipped);
            }
        }
        private void OnAcceptAllEqualsTeachers()
        {
            foreach (var item in ImportedTeachers)
            {
                if (item.IsLookUpOnDB)
                {
                    item.IsConfirmEnabled = true;
                    item.IsConfirmedOrSkipped = true;
                }
            }
            IsAllTeachersConfirmed = ImportedTeachers.All(t => t.IsConfirmedOrSkipped);
        }

        #endregion
        #region Disciplines

        private void OnConfirmDisciplineStarting(object model)
        {
            if (model is ImportedDiscipline it)
            {
                it.IsConfirmEnabled = true;
            }
        }
        private void OnConfirmImportDisciplineModel(object model)
        {
            if (model is ImportedDiscipline it)
            {
                it.IsConfirmedOrSkipped = true;
                IsAllDisciplinesConfirmed = ImportedDisciplines.All(t => t.IsConfirmedOrSkipped);
            }
        }
        private void OnCreateDiscipline(object model)
        {
            if (model is ImportedDiscipline it)
            {
                dialog.ShowDialog(
                    nameof(CreateDisciplineWindow),
                    new DialogParameters() { { nameof(ImportedDiscipline), it }, { nameof(Audiences), Audiences } },
                    null
                    );
                IsAllDisciplinesConfirmed = ImportedDisciplines.All(t => t.IsConfirmedOrSkipped);
            }
        }
        private void OnAcceptAllEqualsDisciplines()
        {
            foreach (var item in ImportedDisciplines)
            {
                if (item.IsLookUpOnDB)
                {
                    item.IsConfirmEnabled = true;
                    item.IsConfirmedOrSkipped = true;
                }
            }
            IsAllDisciplinesConfirmed = ImportedDisciplines.All(t => t.IsConfirmedOrSkipped);
        }

        #endregion
        #region Schedule

        //private async void OnScheduleGenerating()
        //{
        //    Loader.SetDefaultLoadingInfo();
        //    var date_froms = Capacities.GroupBy(g => g.DateFrom);

        //    Periods.Clear();
        //    ObservableCollection<CapacityCellModel> cells = new();

        //    //Создаем сетку для занятий
        //    foreach (var item in date_froms)
        //    {
        //        CapacityModel first = item.FirstOrDefault();
        //        cells = CreateRangeCells(first.DateFrom, first.DateTo);
        //        if (cells != null && cells.Count != 0)
        //            Periods.Add(new CapacityPeriodModel() { DateFrom = first.DateFrom.Value, DateTo = first.DateTo.Value, Cells = cells });
        //    }

        //    debug.Clear();

        //    //Проходим по сетке и создаем временные занятия без учета времени
        //    List<LessonsModel> temp_lessons = new();
        //    foreach (var capacity in Capacities)
        //    {
        //        try
        //        {
        //            if (string.IsNullOrEmpty(capacity.TeacherFio)) continue;
        //            string teacher_initials = capacity.TeacherFio.Remove(0, 1);
        //            UserModel target_teacher = Teachers.FirstOrDefault(t => t.Initials == teacher_initials);

        //            string group_name = capacity.Group;
        //            GroupsModel target_group = Groups.FirstOrDefault(g => g.Group == group_name);

        //            string type_class_short = capacity.ClassType;
        //            TypeClassesModel target_type_class = ClassTypes.FirstOrDefault(t => t.ShortTitle == type_class_short);

        //            string discipline_name = capacity.DisciplineOrWorkType.Trim();
        //            int pg_index = discipline_name.IndexOf(", п/г ");
        //            if (pg_index != -1) discipline_name = discipline_name.Remove(pg_index);
        //            AcademicDisciplinesModel target_discipline = Disciplines.FirstOrDefault(a => a.Title.Contains(discipline_name));

        //            AudiencesModel priority_audience = Audiences.FirstOrDefault(a => a.Id == target_discipline.IdPriorityAudience);

        //            LessonsModel lm = new();
        //            lm.AcademicDiscipline = target_discipline;
        //            if (priority_audience != null) lm.Audience = priority_audience;

        //            lm.Group = target_group;
        //            lm.Teacher = target_teacher;
        //            lm.TypeClass = target_type_class;
        //            temp_lessons.Add(lm);

        //            debug.Log("Группа " + target_group?.Group + ", Тип " + target_type_class?.Title + ", Название " + target_discipline?.Title);
        //        }
        //        catch (Exception)
        //        {
        //            continue;
        //        }
        //    }

        //    foreach (var period in Periods)
        //    {
        //        foreach (var cell in period.Cells)
        //        {
        //            foreach (var capacity in Capacities)
        //            {
        //                if (!capacity.DateFrom.HasValue || !capacity.DateTo.HasValue) continue;
        //                if (IsInRange(cell.CellDate, capacity.DateFrom.Value, capacity.DateTo.Value))
        //                {

        //                }
        //            }
        //        }
        //    }

        //    await Loader.Clear();
        //}
        private async void OnScheduleGenerating()
        {
            Loader.SetDefaultLoadingInfo();

            //Ищем первую дату От
            DateTime? first_date_from = Capacities.GroupBy(g => g.DateFrom)
                                                  .ToList()
                                                  .OrderBy(d => d.Key)
                                                  .ToList()
                                                  .Where(d => d.Key != null && d.Key != new DateTime())
                                                  .FirstOrDefault()
                                                  .Key;

            //Ищем последнюю дату До
            DateTime? last_date_to = Capacities.GroupBy(g => g.DateTo)
                                                .ToList()
                                                .OrderBy(d => d.Key)
                                                .ToList()
                                                .Where(d => d.Key != null && d.Key != new DateTime())
                                                .LastOrDefault()
                                                .Key;

            //ToDo: Возможно если нет дат нужно указать период в ручную

            //Создаем общую сетку от первой и до последней даты
            ObservableCollection<CapacityCellModel> cells = new();
            cells = CreateRangeCells(first_date_from, last_date_to);

            //Делим сетку на периоды (недели)
            Periods = CreatePeriodsBySplitting(cells);

            //Группируем каждую дисциплину и добавляем по очереди все занятия
            // Правило 1 выбирается первый приоритетный день для преподавателя
            // Правило 2 выбирается промежуток сетки если у записи нагрузки есть даты
            // Правило 3 если нагрузка общая больше чем недельная то занятия переносятся на нижнюю неделю
            // Правило 4 если это все не понравится в ДГТУ то расписание составляется в ручную

            await Loader.Clear();
        }

        /// <summary>
        /// Общая сетка расписания от даты до даты
        /// </summary>
        /// <param name="from">От</param>
        /// <param name="to">До</param>
        /// <returns>Сплошная сетка</returns>
        private ObservableCollection<CapacityCellModel> CreateRangeCells(DateTime? from, DateTime? to)
        {
            ObservableCollection<CapacityCellModel> result = new();

            if (!to.HasValue || !from.HasValue) return null;

            if ((to.Value - from.Value).TotalSeconds < 0)
            {
                return result;
            }

            int total_days = (int)Math.Round((to.Value - from.Value).TotalDays);
            int days_offset = 0;
            for (int i = 0; i < total_days; i++)
            {
                DateTime day = from.Value.AddDays(days_offset++);
                if (day.DayOfWeek != DayOfWeek.Sunday)
                {
                    CapacityCellModel cell = new();
                    cell.CellDate = day;
                    result.Add(cell);
                }
            }

            return result;
        }
        /// <summary>
        /// Создать коллекцию периодов разделив по неделям года
        /// </summary>
        /// <param name="cells">Коллекция общей сетки</param>
        /// <returns></returns>
        private ObservableCollection<CapacityPeriodModel> CreatePeriodsBySplitting(ObservableCollection<CapacityCellModel> cells)
        {
            ObservableCollection<CapacityPeriodModel> result = new();
            GregorianCalendar calendar = new();

            Dictionary<int, List<CapacityCellModel>> week_cells = new();
            foreach (var item in cells)
            {
                int week_number = calendar.GetWeekOfYear(item.CellDate, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
                if (week_cells.ContainsKey(week_number))
                {
                    week_cells[week_number].Add(item);
                }
                else
                {
                    week_cells.Add(week_number, new List<CapacityCellModel>());
                    week_cells[week_number].Add(item);
                }
            }

            foreach (var week in week_cells.Keys)
            {
                CapacityPeriodModel period = new();
                period.Cells = new();
                period.DateFrom = week_cells[week].FirstOrDefault().CellDate;
                period.DateTo = week_cells[week].LastOrDefault().CellDate;

                foreach (var value in week_cells[week])
                {
                    period.Cells.Add(value);
                }

                result.Add(period);
            }

            return result;
        }

        private bool IsInRange(DateTime target, DateTime start, DateTime end)
        {
            if ((target - start).TotalDays < 0)
            {
                return false;
            }
            if ((end - target).TotalDays < 0)
            {
                return false;
            }
            return true;
        }
        private void OnCreateSchedule()
        {

        }

        #endregion

        #endregion
        #region navigation
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey(nameof(Capacities)) && navigationContext.Parameters[nameof(Capacities)] is ObservableCollection<CapacityModel> oc)
            {
                Capacities = oc;
            }
        }
        public void OnNavigatedFrom(NavigationContext navigationContext) { }
        public bool IsNavigationTarget(NavigationContext navigationContext) => true; 
        #endregion
    }
}
