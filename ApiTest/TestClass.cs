using ApplicationModels.Models;
using ApplicationServices.WebApiService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ApiTest
{
    [TestClass]
    public class TestClass
    {
        #region test data
        private static readonly string[] TestStrings = new string[] {
            "TestStringA", "TestStringB", "TestStringC", "TestStringD", "TestStringE", "TestStringF", "TestStringG", "TestStringH"
        };
        private static readonly Guid[] TestGuids = new Guid[] {
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
        };
        private static readonly DateTime[] TestDateTimes = new DateTime[] {
            Convert.ToDateTime("01.01.2020 14:50:55"), Convert.ToDateTime("01.02.2020 02:12:22"),
        };
        private static readonly DateTime[] TestDates = new DateTime[] {
            Convert.ToDateTime("01.01.2020 00:00:00"), Convert.ToDateTime("01.02.2020 00:00:00"),
        };
        private static readonly string[] TestTimes = new string[] {
            Convert.ToDateTime("01.01.2000 8:00:35").ToLongTimeString(),
            Convert.ToDateTime("01.01.2000 10:00:35").ToLongTimeString(),
            Convert.ToDateTime("01.01.2000 12:00:35").ToLongTimeString(),
            Convert.ToDateTime("01.01.2000 14:00:35").ToLongTimeString(),
            Convert.ToDateTime("01.01.2000 16:00:35").ToLongTimeString(),
            Convert.ToDateTime("01.01.2000 18:00:35").ToLongTimeString(),
            Convert.ToDateTime("01.01.2000 20:00:35").ToLongTimeString(),
            Convert.ToDateTime("01.01.2000 22:00:35").ToLongTimeString(),
        };
        private static readonly bool[] TestBooleans = new bool[] { true, false };
        #endregion
        #region service and constructor
        private readonly IWebApiService api;
        public TestClass() => api = new WebApiServiceImplementation();
        #endregion
        #region test methods
        [TestMethod]
        public void TestUsers()
        {
            UserModel model = new();
            model.Login = TestStrings[0];
            model.Password = TestStrings[1];

            var posted = api.PostModel(model, WebApiTableNames.Users).Result;

            Assert.AreEqual(posted.Login, TestStrings[0]);
            Assert.AreEqual(posted.Password, TestStrings[1]);

            posted.Login = TestStrings[2];

            var putted = api.PutModel(posted, WebApiTableNames.Users).Result;

            Assert.AreEqual(posted.Login, TestStrings[2]);

            var deleted_count = api.DeleteModel(putted.ID, WebApiTableNames.Users).Result;

            Assert.AreEqual(deleted_count, 1);

            var models = new List<UserModel>(api.GetModels<UserModel>(WebApiTableNames.Users).Result);

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestAcademicDisciplines()
        {
            AcademicDisciplinesModel model = new();
            model.Title = TestStrings[0];

            var posted = api.PostModel(model, WebApiTableNames.AcademicDisciplines).Result;

            Assert.AreEqual(posted.Title, TestStrings[0]);

            posted.Title = TestStrings[1];

            var putted = api.PutModel(posted, WebApiTableNames.AcademicDisciplines).Result;

            Assert.AreEqual(putted.Title, TestStrings[1]);

            var deleted_count = api.DeleteModel(putted.Id, WebApiTableNames.AcademicDisciplines).Result;

            Assert.AreEqual(deleted_count, 1);

            var models = new List<AcademicDisciplinesModel>(api.GetModels<AcademicDisciplinesModel>(WebApiTableNames.AcademicDisciplines).Result);

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestAudiences()
        {
            AudiencesModel model = new();
            model.NumberAudience = TestStrings[0];

            var posted = api.PostModel(model, WebApiTableNames.Audiences).Result;

            Assert.AreEqual(posted.NumberAudience, TestStrings[0]);

            posted.NumberAudience = TestStrings[1];

            var putted = api.PutModel(posted, WebApiTableNames.Audiences).Result;

            Assert.AreEqual(putted.NumberAudience, TestStrings[1]);

            var deleted_count = api.DeleteModel(putted.Id, WebApiTableNames.Audiences).Result;

            Assert.AreEqual(deleted_count, 1);

            var models = new List<AudiencesModel>(api.GetModels<AudiencesModel>(WebApiTableNames.Audiences).Result);

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestDialogs()
        {
            DialogsModel model = new();
            model.TitleDialog = TestStrings[0];

            var posted = api.PostModel(model, WebApiTableNames.Dialogs).Result;

            Assert.AreEqual(posted.TitleDialog, TestStrings[0]);

            posted.TitleDialog = TestStrings[1];

            var putted = api.PutModel(posted, WebApiTableNames.Dialogs).Result;

            Assert.AreEqual(putted.TitleDialog, TestStrings[1]);

            var deleted_count = api.DeleteModel(putted.Id, WebApiTableNames.Dialogs).Result;

            Assert.AreEqual(deleted_count, 1);

            var models = new List<DialogsModel>(api.GetModels<DialogsModel>(WebApiTableNames.Dialogs).Result);

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestGroups()
        {
            GroupsModel model = new();
            model.Group = TestStrings[0];

            var posted = api.PostModel(model, WebApiTableNames.Groups).Result;

            Assert.AreEqual(posted.Group, TestStrings[0]);

            posted.Group = TestStrings[1];

            var putted = api.PutModel(posted, WebApiTableNames.Groups).Result;

            Assert.AreEqual(putted.Group, TestStrings[1]);

            var deleted_count = api.DeleteModel(putted.Id, WebApiTableNames.Groups).Result;

            Assert.AreEqual(deleted_count, 1);

            var models = new List<GroupsModel>(api.GetModels<GroupsModel>(WebApiTableNames.Groups).Result);

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestLessons()
        {
            LessonsModel model = new();
            model.Day = TestDates[0];
            model.IdTimeLessons = TestGuids[1];
            model.IdTeacher = TestGuids[2];
            model.IdAcademicDiscipline = TestGuids[3];
            model.IdTypeClass = TestGuids[4];
            model.IdAudience = TestGuids[5];

            var posted = api.PostModel(model, WebApiTableNames.Lessons).Result;

            Assert.AreEqual(posted.Day, TestDates[0]);

            posted.Day = TestDates[1];

            var putted = api.PutModel(posted, WebApiTableNames.Lessons).Result;

            Assert.AreEqual(putted.Day, TestDates[1]);

            var deleted_count = api.DeleteModel(putted.Id, WebApiTableNames.Lessons).Result;

            Assert.AreEqual(deleted_count, 1);

            var models = new List<LessonsModel>(api.GetModels<LessonsModel>(WebApiTableNames.Lessons).Result);

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestMessages()
        {
            MessagesModel model = new();
            model.IdDialog = TestGuids[0];
            model.IdUserSender = TestGuids[1];
            model.TextMessage = TestStrings[0];
            model.IsRead = TestBooleans[0];
            model.SendingTime = TestDateTimes[0];

            var posted = api.PostModel(model, WebApiTableNames.Messages).Result;

            Assert.AreEqual(posted.IdDialog, TestGuids[0]);
            Assert.AreEqual(posted.TextMessage, TestStrings[0]);
            Assert.AreEqual(posted.IsRead, TestBooleans[0]);
            Assert.AreEqual(posted.SendingTime, TestDateTimes[0]);

            posted.IdDialog = TestGuids[1];
            posted.TextMessage = TestStrings[1];
            posted.IsRead = TestBooleans[1];
            posted.SendingTime = TestDateTimes[1];

            var putted = api.PutModel(posted, WebApiTableNames.Messages).Result;

            Assert.AreEqual(putted.IdDialog, TestGuids[1]);
            Assert.AreEqual(putted.TextMessage, TestStrings[1]);
            Assert.AreEqual(putted.IsRead, TestBooleans[1]);
            Assert.AreEqual(putted.SendingTime, TestDateTimes[1]);

            var deleted_count = api.DeleteModel(putted.Id, WebApiTableNames.Messages).Result;

            Assert.AreEqual(deleted_count, 1);

            var models = new List<MessagesModel>(api.GetModels<MessagesModel>(WebApiTableNames.Messages).Result);

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestPermissions()
        {
            PermissionsModel model = new();
            model.PermissionName = TestStrings[0];
            model.PermissionDescription = TestStrings[1];

            var posted = api.PostModel(model, WebApiTableNames.Permissions).Result;

            Assert.AreEqual(posted.PermissionName, TestStrings[0]);
            Assert.AreEqual(posted.PermissionDescription, TestStrings[1]);

            posted.PermissionName = TestStrings[2];
            posted.PermissionDescription = TestStrings[3];

            var putted = api.PutModel(posted, WebApiTableNames.Permissions).Result;

            Assert.AreEqual(putted.PermissionName, TestStrings[2]);
            Assert.AreEqual(putted.PermissionDescription, TestStrings[3]);

            var deleted_count = api.DeleteModel(putted.Id, WebApiTableNames.Permissions).Result;

            Assert.AreEqual(deleted_count, 1);

            var models = new List<PermissionsModel>(api.GetModels<PermissionsModel>(WebApiTableNames.Permissions).Result);

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestRoles()
        {
            RolesModel model = new();
            model.RoleName = TestStrings[0];
            model.RoleDescription = TestStrings[1];

            var posted = api.PostModel(model, WebApiTableNames.Roles).Result;

            Assert.AreEqual(posted.RoleName, TestStrings[0]);
            Assert.AreEqual(posted.RoleDescription, TestStrings[1]);

            posted.RoleName = TestStrings[2];
            posted.RoleDescription = TestStrings[3];

            var putted = api.PutModel(posted, WebApiTableNames.Roles).Result;

            Assert.AreEqual(putted.RoleName, TestStrings[2]);
            Assert.AreEqual(putted.RoleDescription, TestStrings[3]);

            var deleted_count = api.DeleteModel(putted.Id, WebApiTableNames.Roles).Result;

            Assert.AreEqual(deleted_count, 1);

            var models = new List<RolesModel>(api.GetModels<RolesModel>(WebApiTableNames.Roles).Result);

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestRolePermissions()
        {
            RolesPermissionsModel model = new();

            model.IdRole = TestGuids[0];
            model.IdPermission = TestGuids[1];

            var posted = api.PostModel(model, WebApiTableNames.RolesPermissions).Result;

            Assert.AreEqual(posted.IdRole, TestGuids[0]);
            Assert.AreEqual(posted.IdPermission, TestGuids[1]);

            posted.IdRole = TestGuids[2];

            var putted = api.PutModel(posted, WebApiTableNames.RolesPermissions).Result;

            Assert.AreEqual(putted.IdRole, TestGuids[2]);

            var deleted_count = api.DeleteModel(putted.Id, WebApiTableNames.RolesPermissions).Result;

            Assert.AreEqual(deleted_count, 1);

            var models = new List<RolesPermissionsModel>(api.GetModels<RolesPermissionsModel>(WebApiTableNames.RolesPermissions).Result);

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestStudentsGroups()
        {
            StudentsGroupsModel model = new();

            model.IdGroup = TestGuids[0];
            model.IdStudent = TestGuids[1];

            var posted = api.PostModel(model, WebApiTableNames.StudentsGroups).Result;

            Assert.AreEqual(posted.IdGroup, TestGuids[0]);
            Assert.AreEqual(posted.IdStudent, TestGuids[1]);

            posted.IdGroup = TestGuids[2];

            var putted = api.PutModel(posted, WebApiTableNames.StudentsGroups).Result;

            Assert.AreEqual(putted.IdGroup, TestGuids[2]);

            var deleted_count = api.DeleteModel(putted.Id, WebApiTableNames.StudentsGroups).Result;

            Assert.AreEqual(deleted_count, 1);

            var models = new List<StudentsGroupsModel>(api.GetModels<StudentsGroupsModel>(WebApiTableNames.StudentsGroups).Result);

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestTimeLessons()
        {
            TimeLessonsModel model = new();
            model.StartTime = TestTimes[0];
            model.EndTime = TestTimes[1];
            model.BreakTime = TestTimes[2];

            var posted = api.PostModel(model, WebApiTableNames.TimeLessons).Result;

            Assert.AreEqual(posted.StartTime, TestTimes[0]);
            Assert.AreEqual(posted.EndTime, TestTimes[1]);
            Assert.AreEqual(posted.BreakTime, TestTimes[2]);

            posted.StartTime = TestTimes[3];
            posted.EndTime = TestTimes[4];
            posted.BreakTime = TestTimes[5];

            var putted = api.PutModel(posted, WebApiTableNames.TimeLessons).Result;

            Assert.AreEqual(putted.StartTime, TestTimes[3]);
            Assert.AreEqual(putted.EndTime, TestTimes[4]);
            Assert.AreEqual(putted.BreakTime, TestTimes[5]);


            var deleted_count = api.DeleteModel(putted.Id, WebApiTableNames.TimeLessons).Result;

            Assert.AreEqual(deleted_count, 1);

            var models = new List<TimeLessonsModel>(api.GetModels<TimeLessonsModel>(WebApiTableNames.TimeLessons).Result);

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestTypeClasses()
        {
            TypeClassesModel model = new();
            model.Title = TestStrings[0];

            var posted = api.PostModel(model, WebApiTableNames.TypeClasses).Result;

            Assert.AreEqual(posted.Title, TestStrings[0]);

            posted.Title = TestStrings[1];

            var putted = api.PutModel(posted, WebApiTableNames.TypeClasses).Result;

            Assert.AreEqual(putted.Title, TestStrings[1]);

            var deleted_count = api.DeleteModel(putted.Id, WebApiTableNames.TypeClasses).Result;

            Assert.AreEqual(deleted_count, 1);

            var models = new List<TypeClassesModel>(api.GetModels<TypeClassesModel>(WebApiTableNames.TypeClasses).Result);

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestUsersDialogs()
        {
            UsersDialogsModel model = new();
            model.IdUser = TestGuids[0];
            model.IdDialog = TestGuids[1];

            var posted = api.PostModel(model, WebApiTableNames.UsersDialogs).Result;

            Assert.AreEqual(posted.IdUser, TestGuids[0]);
            Assert.AreEqual(posted.IdDialog, TestGuids[1]);

            posted.IdUser = TestGuids[2];
            posted.IdDialog = TestGuids[3];

            var putted = api.PutModel(posted, WebApiTableNames.UsersDialogs).Result;

            Assert.AreEqual(putted.IdUser, TestGuids[2]);
            Assert.AreEqual(putted.IdDialog, TestGuids[3]);


            var deleted_count = api.DeleteModel(putted.Id, WebApiTableNames.UsersDialogs).Result;

            Assert.AreEqual(deleted_count, 1);

            var models = new List<UsersDialogsModel>(api.GetModels<UsersDialogsModel>(WebApiTableNames.UsersDialogs).Result);

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestUsersRoles()
        {
            UsersRolesModel model = new();
            model.IdUser = TestGuids[0];
            model.IdRole = TestGuids[1];

            var posted = api.PostModel(model, WebApiTableNames.UsersRoles).Result;

            Assert.AreEqual(posted.IdUser, TestGuids[0]);
            Assert.AreEqual(posted.IdRole, TestGuids[1]);

            posted.IdUser = TestGuids[2];
            posted.IdRole = TestGuids[3];

            var putted = api.PutModel(posted, WebApiTableNames.UsersRoles).Result;

            Assert.AreEqual(putted.IdUser, TestGuids[2]);
            Assert.AreEqual(putted.IdRole, TestGuids[3]);


            var deleted_count = api.DeleteModel(putted.Id, WebApiTableNames.UsersRoles).Result;

            Assert.AreEqual(deleted_count, 1);

            var models = new List<UsersRolesModel>(api.GetModels<UsersRolesModel>(WebApiTableNames.UsersRoles).Result);

            Assert.AreEqual(models.Count, 0);
        }

        [TestMethod]
        public void TestTestHeaders()
        {
            TestHeadersModel model = new();
            model.GroupID = TestGuids[0];
            model.TeacherID = TestGuids[1];
            model.TestID = TestGuids[2];
            model.TestName = TestStrings[0];
            model.TestTime = TestStrings[1];
            model.TestType = TestStrings[2];
            model.CountQuestions = 1;

            var posted = api.PostModel(model, WebApiTableNames.TestHeaders).Result;

            Assert.AreEqual(posted.GroupID, TestGuids[0]);
            Assert.AreEqual(posted.TeacherID, TestGuids[1]);
            Assert.AreEqual(posted.TestID, TestGuids[2]);
            Assert.AreEqual(posted.TestName, TestStrings[0]);
            Assert.AreEqual(posted.TestTime, TestStrings[1]);
            Assert.AreEqual(posted.TestType, TestStrings[2]);
            Assert.AreEqual(posted.CountQuestions, 1);

            posted.GroupID = TestGuids[3];
            posted.TeacherID = TestGuids[4];
            posted.TestID = TestGuids[5];
            posted.TestName = TestStrings[3];
            posted.TestTime = TestStrings[4];
            posted.TestType = TestStrings[5];
            posted.CountQuestions = 2;

            var putted = api.PutModel(posted, WebApiTableNames.TestHeaders).Result;

            Assert.AreEqual(putted.GroupID, TestGuids[3]);
            Assert.AreEqual(putted.TeacherID, TestGuids[4]);
            Assert.AreEqual(putted.TestID, TestGuids[5]);
            Assert.AreEqual(putted.TestName, TestStrings[3]);
            Assert.AreEqual(putted.TestTime, TestStrings[4]);
            Assert.AreEqual(putted.TestType, TestStrings[5]);
            Assert.AreEqual(putted.CountQuestions, 2);

            var deleted_count = api.DeleteModel(putted.ID, WebApiTableNames.TestHeaders).Result;

            Assert.AreEqual(deleted_count, 1);

            var models = new List<TestHeadersModel>(api.GetModels<TestHeadersModel>(WebApiTableNames.TestHeaders).Result);

            Assert.AreEqual(models.Count, 0);
        }

        [TestMethod]
        public void TestTestResults()
        {
            TestResultsModel model = new();
            model.StudentName= TestStrings[0];
            model.StudentSurname= TestStrings[1];
            model.TestID= TestGuids[0];
            model.UserID= TestGuids[1];
            model.CorrectAnswers = 1;
            model.TestCompleted = TestBooleans[0];

            var posted = api.PostModel(model, WebApiTableNames.TestResults).Result;

            Assert.AreEqual(posted.StudentName, TestStrings[0]);
            Assert.AreEqual(posted.StudentSurname, TestStrings[1]);
            Assert.AreEqual(posted.TestID, TestGuids[0]);
            Assert.AreEqual(posted.UserID, TestGuids[1]);
            Assert.AreEqual(posted.CorrectAnswers, 1);
            Assert.AreEqual(posted.TestCompleted, TestBooleans[0]);

            posted.StudentName= TestStrings[2];
            posted.StudentSurname= TestStrings[3];
            posted.TestID= TestGuids[2];
            posted.UserID= TestGuids[3];
            posted.CorrectAnswers = 2;
            posted.TestCompleted = TestBooleans[1];

            var putted = api.PutModel(posted, WebApiTableNames.TestResults).Result;

            Assert.AreEqual(putted.StudentName, TestStrings[2]);
            Assert.AreEqual(putted.StudentSurname, TestStrings[3]);
            Assert.AreEqual(putted.TestID, TestGuids[2]);
            Assert.AreEqual(putted.UserID, TestGuids[3]);
            Assert.AreEqual(putted.CorrectAnswers, 2);
            Assert.AreEqual(putted.TestCompleted, TestBooleans[1]);

            var deleted_count = api.DeleteModel(putted.ID, WebApiTableNames.TestResults).Result;

            Assert.AreEqual(deleted_count, 1);

            var models = new List<TestResultsModel>(api.GetModels<TestResultsModel>(WebApiTableNames.TestResults).Result);

            Assert.AreEqual(models.Count, 0);
        }

        [TestMethod]
        public void TestTests()
        {
            TestsModel model = new();
            model.Questions = TestStrings[0];

            var posted = api.PostModel(model, WebApiTableNames.Tests).Result;

            Assert.AreEqual(posted.Questions, TestGuids[0]);

            posted.Questions = TestStrings[1];

            var putted = api.PutModel(posted, WebApiTableNames.Tests).Result;

            Assert.AreEqual(putted.Questions, TestStrings[1]);

            var deleted_count = api.DeleteModel(putted.ID, WebApiTableNames.Tests).Result;

            Assert.AreEqual(deleted_count, 1);

            var models = new List<TestsModel>(api.GetModels<TestsModel>(WebApiTableNames.Tests).Result);

            Assert.AreEqual(models.Count, 0);
        }
        #endregion
    }
}
