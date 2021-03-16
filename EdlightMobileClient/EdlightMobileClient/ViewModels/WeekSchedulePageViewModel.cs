using EdlightMobileClient.Collections;
using EdlightMobileClient.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EdlightMobileClient.ViewModels
{
    public class WeekSchedulePageViewModel : ViewModelBase
    {
        #region field
        private IndexableObservableCollection<DayModel> selectedWeek;
        private string selectedWeekHeaderDates;
        private bool selectedWeekIsEven;
        #endregion

        #region prop
        public IndexableObservableCollection<DayModel> SelectedWeek
        {
            set { SetProperty(ref selectedWeek, value); }
            get { return selectedWeek; }
        }

        public string SelectedWeekHeaderDates
        {
            set { SetProperty(ref selectedWeekHeaderDates, value); }
            get { return selectedWeekHeaderDates; }
        }

        public bool SelectedWeekIsEven
        {
            set { SetProperty(ref selectedWeekIsEven, value); }
            get { return selectedWeekIsEven; }
        }

        #endregion

        #region command
        public DelegateCommand NextWeekCommand { get; set; }
        private void ExecuteNextWeekCommand()
        {

        }

        public DelegateCommand LastWeekCommand { get; set; }
        private void ExecuteLastWeekCommand()
        {

        }

        public DelegateCommand<object> NavigateCommand { get; set; }
        private async void ExecuteNavigateCommand(object parameter)
        {
            var navigationParams = new NavigationParameters
            {
                { "model", SelectedWeek},
                { "index", (int)parameter}
            };
            await NavigationService.NavigateAsync("DaySchedulePage", navigationParams);
        }
        #endregion

        #region testMethods
        private IndexableObservableCollection<DayModel> GetWeek()
        {
            var schedule = new Schedule()
            {
                DayWeek = "Понедельник",
                TimeLesson = new TimeLessons()
                {
                    StartClass = new DateTime(2021, 3, 1, 8, 30, 0),
                    StartBreak = new DateTime(2021, 3, 1, 9, 15, 0),
                    EndBreak = new DateTime(2021, 3, 1, 9, 20, 0),
                    EndClass = new DateTime(2021, 3, 1, 10, 5, 0)
                },
                EvenNumberedWeek = true,
                AcademicDescipline = "Базы данных",
                Teacher = "Иванов И.И.",
                TypeClass = "Лек",
                Audience = "101"
            };
            var schedules = new List<Schedule>();
            schedules.Add(schedule);
            schedules.Add(schedule);
            schedules.Add(schedule);
            schedules.Add(schedule);
            schedules.Add(schedule);
            schedules.Add(schedule);
            DayModel dayModel;
            var dayModels = new IndexableObservableCollection<DayModel>();
            dayModel = new DayModel()
            {
                Date = new DateTime(2021, 3, 1),
                StartClasses = schedules[0].TimeLesson.StartClass,
                EndClasses = schedules[schedules.Count - 1].TimeLesson.EndClass,
                Schedule = schedules
            };
            dayModels.Add(dayModel);
            dayModel = new DayModel()
            {
                Date = new DateTime(2021, 3, 2),
                StartClasses = schedules[0].TimeLesson.StartClass,
                EndClasses = schedules[schedules.Count - 1].TimeLesson.EndClass,
                Schedule = schedules
            };
            dayModels.Add(dayModel);
            dayModel = new DayModel()
            {
                Date = new DateTime(2021, 3, 3),
                StartClasses = schedules[0].TimeLesson.StartClass,
                EndClasses = schedules[schedules.Count - 1].TimeLesson.EndClass,
                Schedule = schedules
            };
            dayModels.Add(dayModel);
            dayModel = new DayModel()
            {
                Date = new DateTime(2021, 3, 4),
                StartClasses = schedules[0].TimeLesson.StartClass,
                EndClasses = schedules[schedules.Count - 1].TimeLesson.EndClass,
                Schedule = schedules
            };
            dayModels.Add(dayModel);
            dayModel = new DayModel()
            {
                Date = new DateTime(2021, 3, 5),
                StartClasses = schedules[0].TimeLesson.StartClass,
                EndClasses = schedules[schedules.Count - 1].TimeLesson.EndClass,
                Schedule = schedules
            };
            dayModels.Add(dayModel);
            dayModel = new DayModel()
            {
                Date = new DateTime(2021, 3, 6),
                StartClasses = schedules[0].TimeLesson.StartClass,
                EndClasses = schedules[schedules.Count - 1].TimeLesson.EndClass,
                Schedule = schedules
            };
            dayModels.Add(dayModel);
            return dayModels;
        }
        #endregion

        public WeekSchedulePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            SelectedWeek = GetWeek();
            SelectedWeekHeaderDates = $"{SelectedWeek[0].Date.ToString("dd.MM")}-{SelectedWeek[SelectedWeek.Count - 1].Date.ToString("dd.MM")}";
            SelectedWeekIsEven = true;
            NextWeekCommand = new DelegateCommand(ExecuteNextWeekCommand);
            LastWeekCommand = new DelegateCommand(ExecuteLastWeekCommand);
            NavigateCommand = new DelegateCommand<object>(ExecuteNavigateCommand);
        }
    }
}
