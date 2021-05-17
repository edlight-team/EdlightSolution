using ApplicationModels.Models;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.MemoryService;
using EdlightDesktopClient.Views;
using EdlightDesktopClient.Views.Schedule;
using HandyControl.Controls;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EdlightDesktopClient.ViewModels.Schedule
{
    public class ScheduleMainViewModel : BindableBase
    {
        #region services

        private readonly IRegionManager manager;
        private readonly IMemoryService memory;

        #endregion
        #region fields

        private DateTime _currentDate;
        private UserModel _currentUser;
        private ObservableCollection<LessonsModel> _models;

        #endregion
        #region props

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
        public ObservableCollection<LessonsModel> Models { get => _models; set => SetProperty(ref _models, value); }


        #endregion
        #region commands

        #region Date clicks

        public DelegateCommand PrevDayCommand { get; private set; }
        public DelegateCommand TodayCommand { get; private set; }
        public DelegateCommand NextDayCommand { get; private set; }

        #endregion
        #region Schedule managing

        public DelegateCommand AddCardCommand { get; private set; }

        #endregion

        #endregion
        #region ctor

        public ScheduleMainViewModel(IRegionManager manager, IMemoryService memory, IWebApiService api)
        {
            this.manager = manager;
            this.memory = memory;

            Models = new();
            memory.StoreItem("TimeLessons", Models);
            CurrentUser = memory.GetItem<UserModel>(MemoryAlliases.CurrentUser);
            CurrentDate = DateTime.Now;

            #region Date clicks

            PrevDayCommand = new DelegateCommand(OnPrevCLick);
            TodayCommand = new DelegateCommand(OnTodayClick);
            NextDayCommand = new DelegateCommand(OnNextClick);

            #endregion
            #region Schedule managing

            AddCardCommand = new DelegateCommand(OnAddScheduleCard);

            #endregion
        }

        #endregion
        #region methods

        #region Date clicks

        private void OnPrevCLick() => CurrentDate = CurrentDate.AddDays(-1);
        private void OnTodayClick() => CurrentDate = CurrentDate = DateTime.Now;
        private void OnNextClick() => CurrentDate = CurrentDate.AddDays(1);
        private void OnLoadModelsByDate(DateTime date)
        {
            Models.Clear();

            if (date.ToLongDateString() == Convert.ToDateTime("16.05.2021").ToLongDateString())
            {
                LessonsModel lm = new();
                lm.Day = Convert.ToDateTime("16.05.2021");
                lm.Teacher = CurrentUser;
                lm.TypeClass = new() { Title = "Лекция" };
                lm.AcademicDiscipline = new() { Title = "Безопасность ЖД" };
                lm.Group = new() { Group = "ВИС - 41" };

                lm.TimeLessons = new()
                {
                    StartTime = "09:15",
                    EndTime = "10:50",
                    BreakTime = "10"
                };
                Models.Add(lm);
            }
            if (date.ToLongDateString() == Convert.ToDateTime("17.05.2021").ToLongDateString())
            {
                LessonsModel lm = new();
                lm.Day = Convert.ToDateTime("14.05.2021");
                lm.Teacher = CurrentUser;
                lm.TypeClass = new() { Title = "Лабораторная работа" };
                lm.Audience = new() { NumberAudience = "Лаборатория" };
                lm.AcademicDiscipline = new() { Title = "Моделирование информационных процессов" };
                lm.Group = new() { Group = "ВКТ - 31" };

                lm.TimeLessons = new()
                {
                    StartTime = "09:15",
                    EndTime = "09:45",
                    BreakTime = "10"
                };
                Models.Add(lm);

                LessonsModel lm2 = new();
                lm2.Day = Convert.ToDateTime("14.05.2021");
                lm2.Teacher = CurrentUser;
                lm2.TypeClass = new() { Title = "Лекция" };
                lm2.Audience = new() { NumberAudience = "Каб.208" };
                lm2.Group = new() { Group = "ВТМ - 41" };

                lm2.TimeLessons = new()
                {
                    StartTime = "11:00",
                    EndTime = "11:45",
                    BreakTime = "25"
                };
                Models.Add(lm2);
            }
            OnDateNavigated();
        }
        private void OnDateNavigated()
        {
            if (!manager.Regions.ContainsRegionWithName(BaseMethods.RegionNames.ScheduleDateViewRegion)) return;
            manager.Regions[BaseMethods.RegionNames.ScheduleDateViewRegion].RemoveAll();
            manager.RequestNavigate(BaseMethods.RegionNames.ScheduleDateViewRegion, nameof(ScheduleDateViewer));
        }

        #endregion
        #region Schedule managing

        private void OnAddScheduleCard()
        {
            manager.RequestNavigate(BaseMethods.RegionNames.ModalRegion, nameof(AddScheduleView));
        }

        #endregion

        #endregion
    }
}
