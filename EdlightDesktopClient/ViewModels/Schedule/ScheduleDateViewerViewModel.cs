using ApplicationEventsWPF.Events.ScheduleEvents;
using ApplicationEventsWPF.Events.Signal;
using ApplicationModels.Models;
using ApplicationServices.SignalClientSerivce;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.MemoryService;
using HandyControl.Controls;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace EdlightDesktopClient.ViewModels.Schedule
{
    public class ScheduleDateViewerViewModel : BindableBase, INavigationAware
    {
        #region services

        private readonly IMemoryService memory;
        private readonly IEventAggregator aggregator;
        private readonly IWebApiService api;
        private readonly ISignalClientService signal;

        #endregion
        #region fields

        private DateTime _currentDate;
        private GroupsModel _selectedGroup;
        private List<string> _timeZones;
        private ObservableCollection<LessonsModel> _models;
        private ObservableCollection<CommentModel> _comments;

        #endregion
        #region props

        public DateTime CurrentDate { get => _currentDate; set => SetProperty(ref _currentDate, value); }
        public GroupsModel SelectedGroup { get => _selectedGroup; set => SetProperty(ref _selectedGroup, value); }
        public List<string> TimeZones { get => _timeZones; set => SetProperty(ref _timeZones, value); }
        public ObservableCollection<LessonsModel> Models { get => _models; set => SetProperty(ref _models, value); }
        public ObservableCollection<CommentModel> Comments { get => _comments; set => SetProperty(ref _comments, value); }

        #endregion
        #region commands

        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand UnloadedCommand { get; private set; }

        #endregion
        #region ctor

        public ScheduleDateViewerViewModel(IMemoryService memory, IEventAggregator aggregator, IWebApiService api, ISignalClientService signal)
        {
            this.memory = memory;
            this.aggregator = aggregator;
            this.api = api;
            this.signal = signal;
            Models = memory.GetItem<ObservableCollection<LessonsModel>>(nameof(Models));
            Comments = memory.GetItem<ObservableCollection<CommentModel>>(nameof(Comments));
            FillTimes();

            LoadedCommand = new DelegateCommand(OnLoaded);
            UnloadedCommand = new DelegateCommand(OnUnloaded);
        }

        #endregion
        #region methods

        #region Signal received

        private void SignalEntityChanged(string serialized)
        {
            try
            {
                EntitySignalModel model = JsonConvert.DeserializeObject<EntitySignalModel>(serialized);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (model.SendType == "POST")
                    {
                        if (model.ModelType == typeof(LessonsModel))
                        {
                            LessonsModel lm = JsonConvert.DeserializeObject<LessonsModel>(model.SerializedModel);
                            if (CurrentDate == lm.Day && SelectedGroup.Id == lm.IdGroup)
                            {
                                Models.Add(lm);
                                CreateCard(lm);
                            }
                        }
                        return;
                    }
                    else if (model.SendType == "PUT")
                    {
                        if (model.ModelType == typeof(LessonsModel))
                        {
                            LessonsModel lm = JsonConvert.DeserializeObject<LessonsModel>(model.SerializedModel);

                            LessonsModel remove = Models.FirstOrDefault(m => m.Id == lm.Id);
                            if (remove != null)
                            {
                                Models.Remove(remove);
                                DeleteCard(lm);
                            }
                            if (CurrentDate == lm.Day && SelectedGroup.Id == lm.IdGroup)
                            {
                                Models.Add(lm);
                                CreateCard(lm);
                            }
                        }
                        return;
                    }
                    else if (model.SendType == "DELETE")
                    {
                        if (model.ModelType == typeof(LessonsModel))
                        {
                            LessonsModel lm = JsonConvert.DeserializeObject<LessonsModel>(model.SerializedModel);

                            LessonsModel remove = Models.FirstOrDefault(m => m.Id == lm.Id);
                            if (remove != null)
                            {
                                Models.Remove(remove);
                                DeleteCard(lm);
                            }
                        }
                        return;
                    }
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
        #region Загрузка и выгрузка

        private void OnLoaded()
        {
            CreateCards();
            aggregator.GetEvent<CardMoveOrResizeEvent>().Subscribe(UpdateCard);
            signal.SubscribeEntityChanged(SignalEntityChanged);
        }
        private void OnUnloaded()
        {
            aggregator.GetEvent<CardMoveOrResizeEvent>().Unsubscribe(UpdateCard);
            signal.UnsubscribeEntityChanged();
        }

        #endregion
        #region Заполнение времени и карточек

        private void FillTimes()
        {
            TimeZones = new();
            int[] intervals = new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };

            for (int i = 8; i < 20; i++)
            {
                TimeZones.Add(i.ToString() + ":" + 0.ToString("D2"));
                foreach (int interval in intervals)
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
                card.Width = 550;
                card.Height = CalculateHeightByStartTimeAndEndTime(model.TimeLessons.StartTime, model.TimeLessons.EndTime);
                card.VerticalAlignment = VerticalAlignment.Top;
                card.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(model.TypeClass.ColorHex));
                card.Margin = CalculateTopMarginByStartTime(model.TimeLessons.StartTime);

                IEnumerable<CommentModel> cardComments = Comments.Where(c => c.IdLesson.ToString().ToUpper() == card.Uid.ToString().ToUpper());

                aggregator.GetEvent<GridChildChangedEvent>().Publish(new object[] { card, model, cardComments });
            }
        }
        private async void CreateCard(LessonsModel lm)
        {
            lm.Teacher = memory.GetItem<ObservableCollection<UserModel>>("Teachers").FirstOrDefault(t=>t.ID == lm.IdTeacher);
            lm.AcademicDiscipline = memory.GetItem<ObservableCollection<AcademicDisciplinesModel>>("Disciplines").FirstOrDefault(t=>t.Id == lm.IdAcademicDiscipline);
            lm.Audience = memory.GetItem<ObservableCollection<AudiencesModel>>("Audiences").FirstOrDefault(t=>t.Id == lm.IdAudience);
            lm.TypeClass = memory.GetItem<ObservableCollection<TypeClassesModel>>("TypeClasses").FirstOrDefault(t=>t.Id == lm.IdTypeClass);
            lm.Group = memory.GetItem<ObservableCollection<GroupsModel>>("Groups").FirstOrDefault(t => t.Id == lm.IdGroup);
            ObservableCollection<TimeLessonsModel> time_lessons = new ObservableCollection<TimeLessonsModel>(await api.GetModels<TimeLessonsModel>(WebApiTableNames.TimeLessons));
            lm.TimeLessons = time_lessons.FirstOrDefault(t=>t.Id == lm.IdTimeLessons);

            Card card = new();
            card.Uid = lm.Id.ToString().ToUpper();
            card.Width = 550;
            card.Height = CalculateHeightByStartTimeAndEndTime(lm.TimeLessons.StartTime, lm.TimeLessons.EndTime);
            card.VerticalAlignment = VerticalAlignment.Top;
            card.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(lm.TypeClass.ColorHex));
            card.Margin = CalculateTopMarginByStartTime(lm.TimeLessons.StartTime);

            IEnumerable<CommentModel> cardComments = Comments.Where(c => c.IdLesson.ToString().ToUpper() == card.Uid.ToString().ToUpper());

            aggregator.GetEvent<GridChildChangedEvent>().Publish(new object[] { card, lm, cardComments });
        }
        private void DeleteCard(LessonsModel lm) => aggregator.GetEvent<GridChildChangedEvent>().Publish(new object[] { lm, true });

        #endregion
        #region Вычисления размеров сетки

        private string CalculateStartTimeByTopMargin(double topMargin)
        {
            int index = (int)topMargin / 17;
            if (index > 0) return TimeZones[index];
            else return TimeZones.FirstOrDefault();
        }
        private string CalculateEndTimeByStartTimeAndHeight(string startTime, double height)
        {
            int different = (int)height / 17;
            int startIndexTime = TimeZones.IndexOf(startTime);
            return TimeZones[startIndexTime + different];
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
            LessonsModel target = Models.FirstOrDefault(m => m.Id.ToString().ToUpper() == card.Uid);
            if (target == null) return;
            target.TimeLessons.StartTime = CalculateStartTimeByTopMargin(card.Margin.Top);
            target.TimeLessons.EndTime = CalculateEndTimeByStartTimeAndHeight(target.TimeLessons.StartTime, card.Height);

            await api.PutModel(target.TimeLessons, WebApiTableNames.TimeLessons);
            //ToDo: Добавть здесь метод Signal R
        }

        #endregion
        #region Navigation

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey(nameof(CurrentDate)) && navigationContext.Parameters[nameof(CurrentDate)] is DateTime date)
            {
                CurrentDate = date;
            }
            if (navigationContext.Parameters.ContainsKey(nameof(SelectedGroup)) && navigationContext.Parameters[nameof(SelectedGroup)] is GroupsModel gr)
            {
                SelectedGroup = gr;
            }
        }
        public void OnNavigatedFrom(NavigationContext navigationContext) { }
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        #endregion

        #endregion
    }
}
