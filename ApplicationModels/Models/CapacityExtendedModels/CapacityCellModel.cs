using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace ApplicationModels.Models.CapacityExtendedModels
{
    public class CapacityCellModel : BindableBase
    {
        #region Дата

        private DateTime _cellDate;

        /// <summary>
        /// Дата ячейки
        /// </summary>
        public DateTime CellDate { get => _cellDate; set => SetProperty(ref _cellDate, value); }

        #endregion
        #region Верхняя неделя

        //private ObservableCollection<LessonsModel> _UpPair1;
        //private ObservableCollection<LessonsModel> _UpPair2;
        //private ObservableCollection<LessonsModel> _UpPair3;
        //private ObservableCollection<LessonsModel> _UpPair4;
        //private ObservableCollection<LessonsModel> _UpPair5;
        //private ObservableCollection<LessonsModel> _UpPair6;

        //public ObservableCollection<LessonsModel> UpPair1 { get => _UpPair1 ??= new(); set => SetProperty(ref _UpPair1, value); }
        //public ObservableCollection<LessonsModel> UpPair2 { get => _UpPair2 ??= new(); set => SetProperty(ref _UpPair2, value); }
        //public ObservableCollection<LessonsModel> UpPair3 { get => _UpPair3 ??= new(); set => SetProperty(ref _UpPair3, value); }
        //public ObservableCollection<LessonsModel> UpPair4 { get => _UpPair4 ??= new(); set => SetProperty(ref _UpPair4, value); }
        //public ObservableCollection<LessonsModel> UpPair5 { get => _UpPair5 ??= new(); set => SetProperty(ref _UpPair5, value); }
        //public ObservableCollection<LessonsModel> UpPair6 { get => _UpPair6 ??= new(); set => SetProperty(ref _UpPair6, value); } 

        #endregion
        #region Нижняя неделя

        //private ObservableCollection<LessonsModel> _DownPair1;
        //private ObservableCollection<LessonsModel> _DownPair2;
        //private ObservableCollection<LessonsModel> _DownPair3;
        //private ObservableCollection<LessonsModel> _DownPair4;
        //private ObservableCollection<LessonsModel> _DownPair5;
        //private ObservableCollection<LessonsModel> _DownPair6;

        //public ObservableCollection<LessonsModel> DownPair1 { get => _DownPair1 ??= new(); set => SetProperty(ref _DownPair1, value); }
        //public ObservableCollection<LessonsModel> DownPair2 { get => _DownPair2 ??= new(); set => SetProperty(ref _DownPair2, value); }
        //public ObservableCollection<LessonsModel> DownPair3 { get => _DownPair3 ??= new(); set => SetProperty(ref _DownPair3, value); }
        //public ObservableCollection<LessonsModel> DownPair4 { get => _DownPair4 ??= new(); set => SetProperty(ref _DownPair4, value); }
        //public ObservableCollection<LessonsModel> DownPair5 { get => _DownPair5 ??= new(); set => SetProperty(ref _DownPair5, value); }
        //public ObservableCollection<LessonsModel> DownPair6 { get => _DownPair6 ??= new(); set => SetProperty(ref _DownPair6, value); } 

        #endregion
        #region Верхняя неделя

        private CapacityDayModel _UpDay;
        public CapacityDayModel UpDay { get => _UpDay ??= new(); set => SetProperty(ref _UpDay, value); }

        #endregion
        #region Нижняя неделя

        private CapacityDayModel _DownDay;
        public CapacityDayModel DownDay { get => _DownDay ??= new(); set => SetProperty(ref _DownDay, value); }

        #endregion
    }
    public class CapacityDayModel : BindableBase
    {
        private ObservableCollection<CapacityPairModel> _pairs;
        public ObservableCollection<CapacityPairModel> Pairs { get => _pairs; set => SetProperty(ref _pairs, value); }

        public CapacityDayModel()
        {
            Pairs = new()
            {
                new CapacityPairModel() { PairName = "1 пара, с 8:30 до 10:05", Lessons = new() },
                new CapacityPairModel() { PairName = "2 пара, с 10:15 до 11:50", Lessons = new() },
                new CapacityPairModel() { PairName = "3 пара, с 12:30 до 14:05", Lessons = new() },
                new CapacityPairModel() { PairName = "4 пара, с 14:30 до 15:50", Lessons = new() },
                new CapacityPairModel() { PairName = "5 пара, с 16:00 до 17:35", Lessons = new() },
                new CapacityPairModel() { PairName = "6 пара, с 17:45 до 19:20", Lessons = new() },
            };
        }
        #region commented

        //private ObservableCollection<LessonsModel> _Pair1;
        //private ObservableCollection<LessonsModel> _Pair2;
        //private ObservableCollection<LessonsModel> _Pair3;
        //private ObservableCollection<LessonsModel> _Pair4;
        //private ObservableCollection<LessonsModel> _Pair5;
        //private ObservableCollection<LessonsModel> _Pair6;

        ///// <summary>
        ///// 1 пара
        ///// </summary>
        //public ObservableCollection<LessonsModel> Pair1 { get => _Pair1 ??= new(); set => SetProperty(ref _Pair1, value); }
        ///// <summary>
        ///// 2 пара
        ///// </summary>
        //public ObservableCollection<LessonsModel> Pair2 { get => _Pair2 ??= new(); set => SetProperty(ref _Pair2, value); }
        ///// <summary>
        ///// 3 пара
        ///// </summary>
        //public ObservableCollection<LessonsModel> Pair3 { get => _Pair3 ??= new(); set => SetProperty(ref _Pair3, value); }
        ///// <summary>
        ///// 4 пара
        ///// </summary>
        //public ObservableCollection<LessonsModel> Pair4 { get => _Pair4 ??= new(); set => SetProperty(ref _Pair4, value); }
        ///// <summary>
        ///// 5 пара
        ///// </summary>
        //public ObservableCollection<LessonsModel> Pair5 { get => _Pair5 ??= new(); set => SetProperty(ref _Pair5, value); }
        ///// <summary>
        ///// 6 пара
        ///// </summary>
        //public ObservableCollection<LessonsModel> Pair6 { get => _Pair6 ??= new(); set => SetProperty(ref _Pair6, value); } 

        #endregion
    }
    public class CapacityPairModel : BindableBase
    {
        private string _PairName;
        public string PairName { get => _PairName; set => SetProperty(ref _PairName, value); }

        private ObservableCollection<LessonsModel> _Lessons;
        public ObservableCollection<LessonsModel> Lessons { get => _Lessons ??= new(); set => SetProperty(ref _Lessons, value); }
    }
    public class CapacityPeriodModel : BindableBase
    {
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private ObservableCollection<CapacityCellModel> _cells;

        public DateTime DateFrom
        {
            get => _dateFrom;
            set => SetProperty(ref _dateFrom, value);
        }
        public DateTime DateTo
        {
            get => _dateTo;
            set => SetProperty(ref _dateTo, value);
        }
        public ObservableCollection<CapacityCellModel> Cells
        {
            get => _cells;
            set => SetProperty(ref _cells, value);
        }

        public string FullPeriod { get => $"Период : от {DateFrom:dd.MM.yyyy} до {DateTo:dd.MM.yyyy}"; }
    }
}
