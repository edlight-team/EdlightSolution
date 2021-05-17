using ApplicationModels.Models;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.MemoryService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EdlightDesktopClient.ViewModels.Schedule
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class AddScheduleViewModel : BindableBase
    {
        #region services

        private readonly IRegionManager manager;

        #endregion
        #region fields

        private UserModel _currentUser;
        private ObservableCollection<AcademicDisciplinesModel> _disciplines;
        private AcademicDisciplinesModel _selectedDiscipline;
        private ObservableCollection<AudiencesModel> _audiences;
        private AudiencesModel _selectedAudience;
        private ObservableCollection<TypeClassesModel> _typeClasses;
        private TypeClassesModel _selectedTypeClass;

        #endregion
        #region props

        public UserModel CurrentUser { get => _currentUser; set => SetProperty(ref _currentUser, value); }
        public ObservableCollection<AcademicDisciplinesModel> Disciplines { get => _disciplines; set => SetProperty(ref _disciplines, value); }
        public AcademicDisciplinesModel SelectedDiscipline { get => _selectedDiscipline; set => SetProperty(ref _selectedDiscipline, value); }
        public ObservableCollection<AudiencesModel> Audiences { get => _audiences; set => SetProperty(ref _audiences, value); }
        public AudiencesModel SelectedAudience { get => _selectedAudience; set => SetProperty(ref _selectedAudience, value); }
        public ObservableCollection<TypeClassesModel> TypeClasses { get => _typeClasses; set => SetProperty(ref _typeClasses, value); }
        public TypeClassesModel SelectedTypeClass { get => _selectedTypeClass; set => SetProperty(ref _selectedTypeClass, value); }

        #endregion
        #region commands

        public DelegateCommand CreateRecordCommand { get; private set; }
        public DelegateCommand CloseModalCommand { get; private set; }

        #endregion
        #region ctor

        public AddScheduleViewModel(IRegionManager manager, IMemoryService memory, IWebApiService api)
        {
            this.manager = manager;
            CurrentUser = memory.GetItem<UserModel>(MemoryAlliases.CurrentUser);
            LoadingData(api);

            CloseModalCommand = new DelegateCommand(OnCloseModal);
            CreateRecordCommand = new DelegateCommand(OnCreateSchedule);
        }
        private async void LoadingData(IWebApiService api)
        {
            Disciplines = new ObservableCollection<AcademicDisciplinesModel>(await api.GetModels<AcademicDisciplinesModel>(WebApiTableNames.AcademicDisciplines));
            if (Disciplines.Count != 0) SelectedDiscipline = Disciplines.FirstOrDefault();
            Audiences = new ObservableCollection<AudiencesModel>(await api.GetModels<AudiencesModel>(WebApiTableNames.Audiences));
            if (Audiences.Count != 0) SelectedAudience = Audiences.FirstOrDefault();
            TypeClasses = new ObservableCollection<TypeClassesModel>(await api.GetModels<TypeClassesModel>(WebApiTableNames.TypeClasses));
            if (TypeClasses.Count != 0) SelectedTypeClass = TypeClasses.FirstOrDefault();
        }

        #endregion
        #region methods

        private void OnCloseModal()
        {
            manager.Regions[BaseMethods.RegionNames.ModalRegion].RemoveAll();
        }
        private void OnCreateSchedule()
        {
            LessonsModel tms = new();

        }

        #endregion
    }
}
