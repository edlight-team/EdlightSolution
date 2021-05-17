using ApplicationEventsWPF.Events;
using ApplicationModels.Models;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.MemoryService;
using HandyControl.Controls;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace EdlightDesktopClient.ViewModels.Schedule
{
    public class ScheduleDateViewerViewModel : BindableBase
    {
        #region services

        private readonly IEventAggregator aggregator;
        private readonly IWebApiService api;

        #endregion
        #region fields

        private List<string> _timeZones;
        private ObservableCollection<LessonsModel> _models;

        #endregion
        #region props

        public List<string> TimeZones { get => _timeZones; set => SetProperty(ref _timeZones, value); }
        public ObservableCollection<LessonsModel> Models { get => _models; set => SetProperty(ref _models, value); }

        #endregion
        #region commands

        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand UnloadedCommand { get; private set; }

        #endregion
        #region ctor

        public ScheduleDateViewerViewModel(IMemoryService memory, IEventAggregator aggregator, IWebApiService api)
        {
            this.aggregator = aggregator;
            this.api = api;
            Models = memory.GetItem<ObservableCollection<LessonsModel>>("TimeLessons");
            FillTimes();

            LoadedCommand = new DelegateCommand(OnLoaded);
            UnloadedCommand = new DelegateCommand(OnUnloaded);
        }

        #endregion
        #region methods

        #region Загрузка и выгрузка

        private void OnLoaded()
        {
            CreateCards();
            aggregator.GetEvent<CardMoveOrResizeEvent>().Subscribe(UpdateCard);
        }
        private void OnUnloaded() => aggregator.GetEvent<CardMoveOrResizeEvent>().Unsubscribe(UpdateCard);

        #endregion
        #region Заполнение времени и карточек

        private void FillTimes()
        {
            TimeZones = new();
            int[] intervals = new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };

            for (int i = 8; i < 20; i++)
            {
                TimeZones.Add(i.ToString() + ":" + 0.ToString("D2"));
                foreach (var interval in intervals)
                {
                    TimeZones.Add(i.ToString() + ":" + interval.ToString("D2"));
                }
            }
        }
        private void CreateCards()
        {
            aggregator.GetEvent<GridChildChangedEvent>().Publish(null);

            foreach (LessonsModel model in Models)
            {
                Card card = new();
                card.Uid = model.Id.ToString().ToUpper();
                card.Width = 600;
                card.Height = CalculateHeightByStartTimeAndEndTime(model.TimeLessons.StartTime, model.TimeLessons.EndTime);
                card.VerticalAlignment = VerticalAlignment.Top;
                card.Background = new SolidColorBrush(Colors.DarkMagenta);
                card.Margin = CalculateTopMarginByStartTime(model.TimeLessons.StartTime);

                aggregator.GetEvent<GridChildChangedEvent>().Publish(new object[] { card, model });
            }
        } 

        #endregion
        #region Вычисления размеров сетки

        private string CalculateStartTimeByTopMargin(double topMargin)
        {
            var index = (int)Math.Round(topMargin / 17);
            if (index > 0) return TimeZones[index - 1];
            else return TimeZones.FirstOrDefault();
        }
        private string CalculateEndTimeByStartTimeAndHeight(string startTime, double height)
        {
            int different = (int)Math.Round(height / 17);
            int startIndexTime = TimeZones.IndexOf(startTime);
            return TimeZones[startIndexTime + different + 1];
        }
        private Thickness CalculateTopMarginByStartTime(string startTime)
        {
            Thickness margin = new(0);
            margin.Top = TimeZones.IndexOf(startTime) * 17;
            return margin;
        }
        private double CalculateHeightByStartTimeAndEndTime(string startTime, string endTime)
        {
            int startIndexTime = TimeZones.IndexOf(startTime);
            int endIndexTime = TimeZones.IndexOf(endTime);
            return (endIndexTime - startIndexTime) * 17;
        }

        #endregion
        #region Обновление карточек

        private async void UpdateCard(Card card)
        {
            var target = Models.FirstOrDefault(m => m.Id.ToString().ToUpper() == card.Uid);
            if (target == null) return;
            target.TimeLessons.StartTime = CalculateStartTimeByTopMargin(card.Margin.Top);
            target.TimeLessons.EndTime = CalculateEndTimeByStartTimeAndHeight(target.TimeLessons.StartTime, card.Height);

            await api.PutModel(target.TimeLessons, WebApiTableNames.TimeLessons);
            //ToDo: Добавть здесь метод Signal R
        } 

        #endregion

        #endregion
    }
}
