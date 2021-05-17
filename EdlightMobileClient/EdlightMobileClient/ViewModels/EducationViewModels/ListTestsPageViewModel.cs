using ApplicationModels.Models;
using ApplicationServices.WebApiService;
using ApplicationXamarinService.MemoryService;
using ApplicationXamarinService.PermissionService;
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

            TestHeaders = new();

            NavigateCommand = new DelegateCommand<object>(NavidateToStarEndTestPage);

            Task onloaded_task = Task.Run(async () => await OnLoaded());
            onloaded_task.Wait();

            Task addHeader_task;
            if (IsTeacher)
                addHeader_task = Task.Run(async () => await AddTeaherTestHeaders());
            else
                addHeader_task = Task.Run(async () => await AddStudentsTestHeaders());
            addHeader_task.Wait();
        }
        #endregion
        #region methods
        public async void NavidateToStarEndTestPage(object parametr)
        {
            if (parametr is TestHeadersModel testHeader)
            {
                if (isTeacher)
                {
                    NavigationParameters navigationParams = new()
                    {
                        { "header", testHeader }
                    };
                    await NavigationService.NavigateAsync("ListTestResult", navigationParams);
                }
                else
                {
                    List<TestResultsModel> testResults = await api.GetModels<TestResultsModel>(WebApiTableNames.TestResults, $"TestID = '{testHeader.TestID}' and UserID = '{userModel.ID}'");
                    NavigationParameters navigationParams = new()
                    {
                        { "header", testHeader },
                        { "result", testResults[0] }
                    };
                    await NavigationService.NavigateAsync("StartEndTestPage", navigationParams);
                }
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

        private async Task AddTeaherTestHeaders()
        {
            try
            {
                List<TestHeadersModel> testHeaders = await api.GetModels<TestHeadersModel>(WebApiTableNames.TestHeaders, $"TeacherID = '{userModel.ID}'");
                foreach (var item in testHeaders)
                    this.testHeaders.Add(item);
            }
            catch (Exception ex)
            {

            }
        }

        private async Task AddStudentsTestHeaders()
        {
            try
            {
                List<StudentsGroupsModel> studentsGroups = await api.GetModels<StudentsGroupsModel>(WebApiTableNames.StudentsGroups, $"IdStudent = '{userModel.ID}'");
                List<TestHeadersModel> testHeaders = await api.GetModels<TestHeadersModel>(WebApiTableNames.TestHeaders, $"GroupID = '{studentsGroups[0].IdGroup}'");
                foreach (var item in testHeaders)
                    this.testHeaders.Add(item);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
