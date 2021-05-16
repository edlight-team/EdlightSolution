using ApplicationModels.Models;
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
        private ObservableCollection<TimeLessonsModel> _models;

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
        public ObservableCollection<TimeLessonsModel> Models { get => _models; set => SetProperty(ref _models, value); }


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

        public ScheduleMainViewModel(IRegionManager manager, IMemoryService memory)
        {
            this.manager = manager;
            this.memory = memory;
            Models = new();
            memory.StoreItem("TimeLessons", Models);
            CurrentDate = DateTime.Now;


            #region Date clicks

            PrevDayCommand = new DelegateCommand(OnPrevCLick);
            TodayCommand = new DelegateCommand(OnTodayClick);
            NextDayCommand = new DelegateCommand(OnNextClick);

            #endregion
            #region Schedule managing

            AddCardCommand = new DelegateCommand(OnAddScheduleCard);

            #endregion
            OnDateNavigated();
        }

        #endregion
        #region methods

        #region Date clicks

        private void OnPrevCLick() => CurrentDate = CurrentDate.AddDays(-1);
        private void OnTodayClick() => CurrentDate = CurrentDate = DateTime.Now;
        private void OnNextClick() => CurrentDate = CurrentDate.AddDays(1);
        private void OnLoadModelsByDate(DateTime date)
        {
            if (date.ToLongDateString() == Convert.ToDateTime("14.05.2021").ToLongDateString())
            {
                Models.Clear();

            }
            if (date.ToLongDateString() == Convert.ToDateTime("15.05.2021").ToLongDateString())
            {
                Models.Clear();
            }
        }
        private void OnDateNavigated() => manager.RequestNavigate(BaseMethods.RegionNames.ScheduleDateViewRegion, nameof(ScheduleDateViewer), new NavigationParameters() { { "Lessons", Models } });

        #endregion
        #region Schedule managing

        private void OnAddScheduleCard()
        {
            manager.RegisterViewWithRegion(BaseMethods.RegionNames.ModalRegion, typeof(AddScheduleView));
            manager.RequestNavigate(BaseMethods.RegionNames.ModalRegion, nameof(AddScheduleView));
        }

        #endregion

        #endregion
    }
}
