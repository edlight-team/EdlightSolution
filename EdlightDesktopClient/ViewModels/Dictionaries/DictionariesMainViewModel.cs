using ApplicationModels.Models;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.NotificationService;
using EdlightDesktopClient.Views.Dictionaries;
using HandyControl.Controls;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Styles.Models;
using System;
using System.Collections.ObjectModel;

namespace EdlightDesktopClient.ViewModels.Dictionaries
{
    public class DictionariesMainViewModel : BindableBase
    {
        #region services

        private readonly IRegionManager manager;
        private readonly IWebApiService api;
        private readonly INotificationService notification;

        #endregion
        #region fields

        private LoaderModel _loader;

        private ObservableCollection<AcademicDisciplinesModel> _disciplines;
        private ObservableCollection<AudiencesModel> _audiences;

        #endregion
        #region props

        public LoaderModel Loader { get => _loader; set => SetProperty(ref _loader, value); }

        public ObservableCollection<AcademicDisciplinesModel> Disciplines { get => _disciplines; set => SetProperty(ref _disciplines, value); }
        public ObservableCollection<AudiencesModel> Audiences { get => _audiences; set => SetProperty(ref _audiences, value); }

        #endregion
        #region commands

        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand AddDisciplineCommand { get; private set; }

        #endregion
        #region ctor and loading

        public DictionariesMainViewModel(IRegionManager manager, IWebApiService api, INotificationService notification)
        {
            Loader = new();
            this.manager = manager;
            this.api = api;
            this.notification = notification;
            LoadedCommand = new DelegateCommand(OnLoaded);
            AddDisciplineCommand = new DelegateCommand(OnAddDiscipline);
        }
        private async void OnLoaded()
        {
            Loader.SetDefaultLoadingInfo();
            Disciplines = new ObservableCollection<AcademicDisciplinesModel>(await api.GetModels<AcademicDisciplinesModel>(WebApiTableNames.AcademicDisciplines));
            Audiences = new ObservableCollection<AudiencesModel>(await api.GetModels<AudiencesModel>(WebApiTableNames.Audiences));
            await Loader.Clear();
            foreach (AcademicDisciplinesModel discipline in Disciplines)
            {
                discipline.EditCommand = new DelegateCommand<object>(OnEditDiscipline);
                discipline.DeleteCommand = new DelegateCommand<object>(OnDeleteDiscipline);
            }
        }

        #endregion
        #region methods

        private void OnAddDiscipline() => manager.RequestNavigate(BaseMethods.RegionNames.ModalRegion, nameof(EditDisciplinesView));
        private void OnEditDiscipline(object disciplineModel)
        {

        }
        private async void OnDeleteDiscipline(object disciplineModel)
        {
            bool confirm = notification.ShowQuestion("Восстановить дисциплину невозможно, продолжить действие?");
            if (!confirm) return;
            try
            {
                Loader.SetDefaultLoadingInfo();
                if (disciplineModel is AcademicDisciplinesModel model)
                {
                    await api.DeleteModel(model.Id, WebApiTableNames.AcademicDisciplines);
                    Disciplines.Remove(model);
                    Growl.Info("Дисциплина успешно Удалена", "Global");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await Loader.Clear();
            }
        }

        #endregion
    }
}
