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

        private LessonsModel _upPair1;
        private LessonsModel _upPair2;
        private LessonsModel _upPair3;
        private LessonsModel _upPair4;
        private LessonsModel _upPair5;
        private LessonsModel _upPair6;

        /// <summary>
        /// 1 пара по верхней неделе
        /// </summary>
        public LessonsModel UpPair1 { get => _upPair1; set => SetProperty(ref _upPair1, value); }
        /// <summary>
        /// 2 пара по верхней неделе
        /// </summary>
        public LessonsModel UpPair2 { get => _upPair2; set => SetProperty(ref _upPair2, value); }
        /// <summary>
        /// 3 пара по верхней неделе
        /// </summary>
        public LessonsModel UpPair3 { get => _upPair3; set => SetProperty(ref _upPair3, value); }
        /// <summary>
        /// 4 пара по верхней неделе
        /// </summary>
        public LessonsModel UpPair4 { get => _upPair4; set => SetProperty(ref _upPair4, value); }
        /// <summary>
        /// 5 пара по верхней неделе
        /// </summary>
        public LessonsModel UpPair5 { get => _upPair5; set => SetProperty(ref _upPair5, value); }
        /// <summary>
        /// 6 пара по верхней неделе
        /// </summary>
        public LessonsModel UpPair6 { get => _upPair6; set => SetProperty(ref _upPair6, value); }

        #endregion
        #region Нижняя неделя

        private LessonsModel _downPair1;
        private LessonsModel _downPair2;
        private LessonsModel _downPair3;
        private LessonsModel _downPair4;
        private LessonsModel _downPair5;
        private LessonsModel _downPair6;

        /// <summary>
        /// 1 пара по нижней неделе
        /// </summary>
        public LessonsModel DownPair1 { get => _downPair1; set => SetProperty(ref _downPair1, value); }
        /// <summary>
        /// 2 пара по нижней неделе
        /// </summary>
        public LessonsModel DownPair2 { get => _downPair2; set => SetProperty(ref _downPair2, value); }
        /// <summary>
        /// 3 пара по нижней неделе
        /// </summary>
        public LessonsModel DownPair3 { get => _downPair3; set => SetProperty(ref _downPair3, value); }
        /// <summary>
        /// 4 пара по нижней неделе
        /// </summary>
        public LessonsModel DownPair4 { get => _downPair4; set => SetProperty(ref _downPair4, value); }
        /// <summary>
        /// 5 пара по нижней неделе
        /// </summary>
        public LessonsModel DownPair5 { get => _downPair5; set => SetProperty(ref _downPair5, value); }
        /// <summary>
        /// 6 пара по нижней неделе
        /// </summary>
        public LessonsModel DownPair6 { get => _downPair6; set => SetProperty(ref _downPair6, value); }

        #endregion
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
    }
}
