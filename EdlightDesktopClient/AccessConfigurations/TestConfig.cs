using ApplicationServices.PermissionService;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EdlightDesktopClient.AccessConfigurations
{
    public class TestConfig : BindableBase
    {
        #region Видимость кнопок / Контролов
        private Visibility canCreateTestRecord;
        private Visibility canUpdateTestRecord;
        private Visibility canDeleteTestRecord;
        private Visibility canTakeTest;
        private Visibility canViewStudentTestResults;
        private Visibility canViewSelfTestResults;

        public Visibility CanCreateTestRecord { get => canCreateTestRecord; set => SetProperty(ref canCreateTestRecord, value); }
        public Visibility CanUpdateTestRecord { get => canUpdateTestRecord; set => SetProperty(ref canUpdateTestRecord, value); }
        public Visibility CanDeleteTestRecord { get => canDeleteTestRecord; set => SetProperty(ref canDeleteTestRecord, value); }
        public Visibility CanTakeTest { get => canTakeTest; set => SetProperty(ref canTakeTest, value); }
        public Visibility CanViewStudentTestResults { get => canViewStudentTestResults; set => SetProperty(ref canViewStudentTestResults, value); }
        public Visibility CanViewSelfTestResults { get => canViewSelfTestResults; set => SetProperty(ref canViewSelfTestResults, value); }
        #endregion
        #region button / controls enabled
        private bool canSetTestFilter;

        public bool CanSetTestFilter { get => canSetTestFilter; set => SetProperty(ref canSetTestFilter, value); }
        #endregion
        #region Инициализация
        public static async Task<TestConfig> InitializeByPermissionService(IPermissionService permissionService)
        {
            TestConfig config = new();

            config.CanCreateTestRecord = await permissionService.IsInPermission(PermissionNames.CreateTestRecords) ? Visibility.Visible : Visibility.Collapsed;
            config.CanUpdateTestRecord = await permissionService.IsInPermission(PermissionNames.UpdateTestRecord) ? Visibility.Visible : Visibility.Collapsed;
            config.CanDeleteTestRecord = await permissionService.IsInPermission(PermissionNames.DeleteTestRecord) ? Visibility.Visible : Visibility.Collapsed;
            config.CanViewStudentTestResults = await permissionService.IsInPermission(PermissionNames.ViewStudentTestResults) ? Visibility.Visible : Visibility.Collapsed;
            config.CanViewSelfTestResults = await permissionService.IsInPermission(PermissionNames.ViewSelfTestResults) ? Visibility.Visible : Visibility.Collapsed;
            config.CanSetTestFilter = await permissionService.IsInPermission(PermissionNames.SetTestFilter) ? true : false;
            config.CanTakeTest = await permissionService.IsInPermission(PermissionNames.TakeTest) ? Visibility.Visible : Visibility.Collapsed;

            return config;
        }
        #endregion
    }
}
