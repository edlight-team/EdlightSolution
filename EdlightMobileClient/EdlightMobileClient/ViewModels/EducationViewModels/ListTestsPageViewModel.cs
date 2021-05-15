using ApplicationModels.Models;
using ApplicationServices.WebApiService;
using ApplicationXamarinServices.MemoryService;
using ApplicationXamarinServices.PermissionService;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace EdlightMobileClient.ViewModels.EducationViewModels
{
    public class ListTestsPageViewModel : ViewModelBase
    {
        #region services
        private IWebApiService api;
        private IMemoryService memory;
        private IPermissionService accessManager;
        #endregion
        #region field
        private ObservableCollection<TestHeadersModel> testHeaders;
        private UserModel userModel;
        private bool isTeacher;
        #endregion
        #region props
        public ObservableCollection<TestHeadersModel> TestHeaders { get => testHeaders; set => SetProperty(ref testHeaders, value); }
        public bool IsTeacher { get => isTeacher; set => SetProperty(ref isTeacher, value); }
        #endregion
        #region commands
        public DelegateCommand<object> NavigateCommand { get; private set; }
        #endregion
        #region constructor
        public ListTestsPageViewModel(INavigationService navigationService,IWebApiService api, IMemoryService memory, IPermissionService accessManager) : base(navigationService)
        {
            this.api = api;
            this.memory = memory;
            this.accessManager = accessManager;

            NavigateCommand = new DelegateCommand<object>(NavidateToStarEndTestPage);

            Task.Run(async () => await OnLoaded());
        }
        #endregion
        #region methods
        public async void NavidateToStarEndTestPage(object parametr)
        {
            if (parametr is TestHeadersModel testHeader)
            {
                NavigationParameters navigationParams = new()
                {
                    { "header", testHeader },
                    { "result", new TestResultsModel() { CorrectAnswers=0,TestCompleted=false} }
                };
                await NavigationService.NavigateAsync("StartEndTestPage", navigationParams);
            }
        }

        public async Task OnLoaded()
        {
            userModel = memory.GetItem<UserModel>(MemoryAlliases.CurrentUser);
            await accessManager.ConfigureService(api, userModel);
            RolesModel teacher_role = await accessManager.GetRoleByName("teacher");

            if (await accessManager.IsInRole(teacher_role))
                IsTeacher = true;
        }

        private async Task GetTeaherTestHeader()
        {
            List<TestHeadersModel> testHeaders = await api.GetModels<TestHeadersModel>(WebApiTableNames.TestHeaders);
            foreach (var item in testHeaders)
            {
                if (item.TeacherID == userModel.ID)
                    this.testHeaders.Add(item);
            }
        }

        private async Task GetStudentsTestHeader()
        {
            List<StudentsGroupsModel> studentsGroups = await api.GetModels<StudentsGroupsModel>(WebApiTableNames.StudentsGroups);
            StudentsGroupsModel groupID = studentsGroups.Where(s => s.IdStudent == userModel.ID).FirstOrDefault();
            List<TestHeadersModel> testHeaders = await api.GetModels<TestHeadersModel>(WebApiTableNames.TestHeaders);
            foreach (var item in testHeaders)
            {
                if (item.GroupID == groupID.IdGroup)
                    this.testHeaders.Add(item);
            }
        }
        #endregion
    }
}
