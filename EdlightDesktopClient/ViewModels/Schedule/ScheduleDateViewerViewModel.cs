using ApplicationEventsWPF.Events;
using ApplicationModels.Models;
using ApplicationWPFServices.MemoryService;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace EdlightDesktopClient.ViewModels.Schedule
{
    public class ScheduleDateViewerViewModel : BindableBase
    {
        #region services

        private readonly IEventAggregator aggregator;

        #endregion

        private List<string> _timeZones;
        private ObservableCollection<TimeLessonsModel> _models;

        public List<string> TimeZones { get => _timeZones; set => SetProperty(ref _timeZones, value); }
        public ObservableCollection<TimeLessonsModel> Models { get => _models; set => SetProperty(ref _models, value); }

        public DelegateCommand LoadedCommand { get; private set; }


        public ScheduleDateViewerViewModel(IMemoryService memory, IEventAggregator aggregator)
        {
            this.aggregator = aggregator;
            Models = memory.GetItem<ObservableCollection<TimeLessonsModel>>("TimeLessons");
            FillTimes();
            aggregator.GetEvent<DateChangedEvent>().Subscribe(CreateCards);
            LoadedCommand = new DelegateCommand(OnLoaded);
        }
        private void OnLoaded() => CreateCards();
        private void FillTimes()
        {
            TimeZones = new();
            string[] intervals = new string[] { "5", "10", "15", "20", "25", "30", "35", "40", "45", "50", "55" };

            for (int i = 7; i < 20; i++)
            {
                TimeZones.Add(i.ToString("D2") + ":" + 0.ToString("D2"));
                TimeZones.AddRange(intervals);
            }
        }
        private void CreateCards()
        {
            aggregator.GetEvent<GridChildChangedEvent>().Publish(null);

            HandyControl.Controls.Card card = new();
            card.Width = 400;
            card.Height = 250;
            card.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            card.Background = new SolidColorBrush(Colors.AliceBlue);
            card.Margin = new System.Windows.Thickness(0, 100, 0, 0);

            HandyControl.Controls.Card card2 = new();
            card2.Width = 400;
            card2.Height = 250;
            card2.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            card2.Background = new SolidColorBrush(Colors.AliceBlue);
            card2.Margin = new System.Windows.Thickness(0, 400, 0, 0);

            aggregator.GetEvent<GridChildChangedEvent>().Publish(card);
            aggregator.GetEvent<GridChildChangedEvent>().Publish(card2);
        }
    }
}
