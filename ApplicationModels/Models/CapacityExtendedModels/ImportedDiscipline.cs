using Prism.Commands;
using Prism.Mvvm;

namespace ApplicationModels.Models.CapacityExtendedModels
{
    public class ImportedDiscipline : BindableBase
    {
        private string _disciplineTitle;
        private string _inputTitle;
        private bool _isConfirmedOrSkipped;
        private bool _isConfirmEnabled;

        private bool _isLookUpOnDB;
        private AcademicDisciplinesModel _lookUpDiscipline;

        private DelegateCommand<object> _startConfirming;
        private DelegateCommand<object> _confirmCommand;
        private DelegateCommand<object> _createDisciplineCommand;

        /// <summary>
        /// Название дисциплины
        /// </summary>
        public string DisciplineTitle { get => _disciplineTitle; set => SetProperty(ref _disciplineTitle, value); }
        /// <summary>
        /// Вводимое название
        /// </summary>
        public string InputTitle { get => _inputTitle; set => SetProperty(ref _inputTitle, value); }
        /// <summary>
        /// Подтвержден или пропущен
        /// </summary>
        public bool IsConfirmedOrSkipped { get => _isConfirmedOrSkipped; set => SetProperty(ref _isConfirmedOrSkipped, value); }
        /// <summary>
        /// Включен режим подтверждения
        /// </summary>
        public bool IsConfirmEnabled { get => _isConfirmEnabled; set => SetProperty(ref _isConfirmEnabled, value); }

        /// <summary>
        /// Найден в БД или нет
        /// </summary>
        public bool IsLookUpOnDB { get => _isLookUpOnDB; set => SetProperty(ref _isLookUpOnDB, value); }
        /// <summary>
        /// Найдено в бд
        /// </summary>
        public AcademicDisciplinesModel LookUpDiscipline { get => _lookUpDiscipline; set => SetProperty(ref _lookUpDiscipline, value); }

        /// <summary>
        /// Команда для входа в подтверждение
        /// </summary>
        public DelegateCommand<object> StartConfirming { get => _startConfirming; set => SetProperty(ref _startConfirming, value); }
        /// <summary>
        /// Команда подтверждение
        /// </summary>
        public DelegateCommand<object> ConfirmCommand { get => _confirmCommand; set => SetProperty(ref _confirmCommand, value); }
        /// <summary>
        /// Команда для создания пользователя
        /// </summary>
        public DelegateCommand<object> CreateDisciplineCommand { get => _createDisciplineCommand; set => SetProperty(ref _createDisciplineCommand, value); }
    }
}
