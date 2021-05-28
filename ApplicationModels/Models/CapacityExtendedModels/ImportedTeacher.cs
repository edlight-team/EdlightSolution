using Prism.Commands;
using Prism.Mvvm;

namespace ApplicationModels.Models.CapacityExtendedModels
{
    public class ImportedTeacher : BindableBase
    {
        private string _teacherInitials;
        private bool _isLookUpOnDB;
        private UserModel _lookUpUser;
        private bool _isConfirmedOrSkipped;
        private bool _isConfirmEnabled;
        private DelegateCommand<object> _confirmCommand;

        /// <summary>
        /// Инициалы пользователя
        /// </summary>
        public string TeacherInitials { get => _teacherInitials; set => SetProperty(ref _teacherInitials, value); }
        /// <summary>
        /// Найден в БД или нет
        /// </summary>
        public bool IsLookUpOnDB { get => _isLookUpOnDB; set => SetProperty(ref _isLookUpOnDB, value); }
        /// <summary>
        /// Пользователь найденный в бд по инициалам
        /// </summary>
        public UserModel LookUpUser { get => _lookUpUser; set => SetProperty(ref _lookUpUser, value); }
        /// <summary>
        /// Подтвержден или пропущен
        /// </summary>
        public bool IsConfirmedOrSkipped { get => _isConfirmedOrSkipped; set => SetProperty(ref _isConfirmedOrSkipped, value); }
        /// <summary>
        /// Включен режим подтверждения
        /// </summary>
        public bool IsConfirmEnabled { get => _isConfirmEnabled; set => SetProperty(ref _isConfirmEnabled, value); }
        /// <summary>
        /// Команда для подтверждения
        /// </summary>
        public DelegateCommand<object> ConfirmCommand { get => _confirmCommand; set => SetProperty(ref _confirmCommand, value); }
    }
}
