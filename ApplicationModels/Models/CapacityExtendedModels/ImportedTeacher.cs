using Prism.Commands;
using Prism.Mvvm;

namespace ApplicationModels.Models.CapacityExtendedModels
{
    public class ImportedTeacher : BindableBase
    {
        private string _teacherInitials;
        private bool _isConfirmedOrSkipped;
        private bool _isConfirmEnabled;

        private bool _isLookUpOnDB;
        private UserModel _lookUpUser;

        private DelegateCommand<object> _startConfirming;
        private DelegateCommand<object> _confirmCommand;
        private DelegateCommand<object> _createTeacherCommand;

        /// <summary>
        /// Инициалы пользователя
        /// </summary>
        public string TeacherInitials { get => _teacherInitials; set => SetProperty(ref _teacherInitials, value); }
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
        public UserModel LookUpUser { get => _lookUpUser; set => SetProperty(ref _lookUpUser, value); }

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
        public DelegateCommand<object> CreateTeacherCommand { get => _createTeacherCommand; set => SetProperty(ref _createTeacherCommand, value); }
    }
}
