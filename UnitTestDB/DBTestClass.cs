using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServerModels.Models;
using SqliteDataExecuter;
using SqliteDataExecuter.Entities;
using System;
using System.Linq;

namespace UnitTestDB
{
    [TestClass]
    public class DBTestClass
    {
        #region test data
        private static readonly string[] TestStrings = new string[] {
            "TestStringA", "TestStringB", "TestStringC", "TestStringD", "TestStringE", "TestStringF", "TestStringG", "TestStringH"
        };
        private static readonly Guid[] TestGuids = new Guid[] {
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
        };
        private static readonly int[] TestIntegers = new int[]{
            11, 22, 33, 44, 55, 66, 77, 88
        };
        private static readonly DateTime[] TestDateTimes = new DateTime[] {
            Convert.ToDateTime("01.01.2020 14:50:55"), Convert.ToDateTime("01.02.2020 02:12:22"),
        };
        private static readonly DateTime[] TestDates = new DateTime[] {
            Convert.ToDateTime("01.01.2020 00:00:00"), Convert.ToDateTime("01.02.2020 00:00:00"),
        };
        private static readonly string[] TestTimes = new string[] {
            Convert.ToDateTime("01.01.2000 8:00:35").ToShortTimeString(),
            Convert.ToDateTime("01.01.2000 10:00:35").ToShortTimeString(),
            Convert.ToDateTime("01.01.2000 12:00:35").ToShortTimeString(),
            Convert.ToDateTime("01.01.2000 14:00:35").ToShortTimeString(),
            Convert.ToDateTime("01.01.2000 16:00:35").ToShortTimeString(),
            Convert.ToDateTime("01.01.2000 18:00:35").ToShortTimeString(),
            Convert.ToDateTime("01.01.2000 20:00:35").ToShortTimeString(),
            Convert.ToDateTime("01.01.2000 22:00:35").ToShortTimeString(),
        };
        private static readonly bool[] TestBooleans = new bool[] { true, false };
        #endregion
        #region test methods
        [TestMethod]
        public void TestUsers()
        {
            DBConnector.UseTestBase = true;
            UserModel model = new();
            model.Login = TestStrings[0];
            model.Password = TestStrings[1];
            model.Name = TestStrings[2];
            model.Surname = TestStrings[3];
            model.Patrnymic = TestStrings[4];
            model.Sex = TestIntegers[0];
            model.Age = TestIntegers[1];
            model.DaysPriority = new System.Collections.Generic.List<int>() { 0, 0, 0, 0, 0, 0 };

            Users db = new();
            UserModel posted = db.PostModel(model);

            Assert.AreEqual(posted.Login, TestStrings[0]);
            Assert.AreEqual(posted.Sex, TestIntegers[0]);

            UserModel getted_user = db.GetModels($"ID = '{posted.ID}'").FirstOrDefault();

            Assert.IsTrue(getted_user.DaysPriority.All(dp => dp == 0));

            System.Collections.Generic.List<UserModel> models = db.GetModels().ToList();

            Assert.AreEqual(models.FirstOrDefault().Login, posted.Login);

            posted.Login = TestStrings[1];
            posted.DaysPriority = new System.Collections.Generic.List<int>() { 1, 1, 1, 1, 1, 1 };

            UserModel putted = db.PutModel(posted);

            Assert.AreEqual(putted.Login, TestStrings[1]);
            Assert.IsTrue(putted.DaysPriority.All(dp => dp == 1));

            db.DeleteModel(posted.ID);

            models = db.GetModels().ToList();

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestAcademicDisciplines()
        {
            DBConnector.UseTestBase = true;
            AcademicDisciplinesModel model = new();
            model.Title = TestStrings[0];

            AcademicDisciplines db = new();
            AcademicDisciplinesModel posted = db.PostModel(model);

            Assert.AreEqual(posted.Title, TestStrings[0]);

            System.Collections.Generic.List<AcademicDisciplinesModel> models = db.GetModels().ToList();

            Assert.AreEqual(models.FirstOrDefault().Title, posted.Title);

            posted.Title = TestStrings[1];

            AcademicDisciplinesModel putted = db.PutModel(posted);

            Assert.AreEqual(putted.Title, TestStrings[1]);

            db.DeleteModel(posted.Id);

            models = db.GetModels().ToList();

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestAudiences()
        {
            DBConnector.UseTestBase = true;
            AudiencesModel model = new();
            model.NumberAudience = TestStrings[0];

            Audiences db = new();
            AudiencesModel posted = db.PostModel(model);

            Assert.AreEqual(posted.NumberAudience, TestStrings[0]);

            System.Collections.Generic.List<AudiencesModel> models = db.GetModels().ToList();

            Assert.AreEqual(models.FirstOrDefault().NumberAudience, posted.NumberAudience);

            posted.NumberAudience = TestStrings[1];

            AudiencesModel putted = db.PutModel(posted);

            Assert.AreEqual(putted.NumberAudience, TestStrings[1]);

            db.DeleteModel(posted.Id);

            models = db.GetModels().ToList();

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestDialogs()
        {
            DBConnector.UseTestBase = true;
            DialogsModel model = new();
            model.TitleDialog = TestStrings[0];

            Dialogs db = new();
            DialogsModel posted = db.PostModel(model);

            Assert.AreEqual(posted.TitleDialog, TestStrings[0]);

            System.Collections.Generic.List<DialogsModel> models = db.GetModels().ToList();

            Assert.AreEqual(models.FirstOrDefault().TitleDialog, posted.TitleDialog);

            posted.TitleDialog = TestStrings[1];

            DialogsModel putted = db.PutModel(posted);

            Assert.AreEqual(putted.TitleDialog, TestStrings[1]);

            db.DeleteModel(posted.Id);

            models = db.GetModels().ToList();

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestGroups()
        {
            DBConnector.UseTestBase = true;
            GroupsModel model = new();
            model.Group = TestStrings[0];

            Groups db = new();
            GroupsModel posted = db.PostModel(model);

            Assert.AreEqual(posted.Group, TestStrings[0]);

            System.Collections.Generic.List<GroupsModel> models = db.GetModels().ToList();

            Assert.AreEqual(models.FirstOrDefault().Group, posted.Group);

            posted.Group = TestStrings[1];

            GroupsModel putted = db.PutModel(posted);

            Assert.AreEqual(putted.Group, TestStrings[1]);

            db.DeleteModel(posted.Id);

            models = db.GetModels().ToList();

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestLearnPlanes()
        {
            DBConnector.UseTestBase = true;
            LearnPlanesModel model = new();
            model.Name = TestStrings[0];
            model.Path = TestStrings[1];

            LearnPlanes db = new();
            LearnPlanesModel posted = db.PostModel(model);

            Assert.AreEqual(posted.Name, TestStrings[0]);
            Assert.AreEqual(posted.Path, TestStrings[1]);

            System.Collections.Generic.List<LearnPlanesModel> models = db.GetModels().ToList();

            Assert.AreEqual(models.FirstOrDefault().Name, TestStrings[0]);
            Assert.AreEqual(models.FirstOrDefault().Path, TestStrings[1]);

            posted.Name = TestStrings[2];
            posted.Path = TestStrings[3];

            LearnPlanesModel putted = db.PutModel(posted);

            Assert.AreEqual(putted.Name, TestStrings[2]);
            Assert.AreEqual(putted.Path, TestStrings[3]);

            db.DeleteModel(posted.Id);

            models = db.GetModels().ToList();

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestLessons()
        {
            DBConnector.UseTestBase = true;
            LessonsModel model = new();
            model.Day = TestDates[0];
            model.IdTimeLessons = TestGuids[1];
            model.IdTeacher = TestGuids[2];
            model.IdAcademicDiscipline = TestGuids[3];
            model.IdTypeClass = TestGuids[4];
            model.IdAudience = TestGuids[5];

            Lessons db = new();
            LessonsModel posted = db.PostModel(model);

            Assert.AreEqual(posted.Day, TestDates[0]);
            Assert.AreEqual(posted.IdTimeLessons, TestGuids[1]);

            System.Collections.Generic.List<LessonsModel> models = db.GetModels().ToList();

            Assert.AreEqual(models.FirstOrDefault().Day, TestDates[0]);
            Assert.AreEqual(models.FirstOrDefault().IdTimeLessons, TestGuids[1]);

            posted.Day = TestDates[1];
            posted.IdTimeLessons = TestGuids[2];

            LessonsModel putted = db.PutModel(posted);

            Assert.AreEqual(putted.Day, TestDates[1]);
            Assert.AreEqual(putted.IdTimeLessons, TestGuids[2]);

            db.DeleteModel(posted.Id);

            models = db.GetModels().ToList();

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestMessages()
        {
            DBConnector.UseTestBase = true;
            MessagesModel model = new();
            model.IdDialog = TestGuids[0];
            model.IdUserSender = TestGuids[1];
            model.TextMessage = TestStrings[0];
            model.IsRead = TestBooleans[0];
            model.SendingTime = TestDateTimes[0];

            Messages db = new();
            MessagesModel posted = db.PostModel(model);

            Assert.AreEqual(posted.IdDialog, TestGuids[0]);
            Assert.AreEqual(posted.TextMessage, TestStrings[0]);
            Assert.AreEqual(posted.IsRead, TestBooleans[0]);
            Assert.AreEqual(posted.SendingTime, TestDateTimes[0]);

            System.Collections.Generic.List<MessagesModel> models = db.GetModels().ToList();

            Assert.AreEqual(models.FirstOrDefault().IdDialog, TestGuids[0]);
            Assert.AreEqual(models.FirstOrDefault().TextMessage, TestStrings[0]);
            Assert.AreEqual(models.FirstOrDefault().IsRead, TestBooleans[0]);
            Assert.AreEqual(models.FirstOrDefault().SendingTime, TestDateTimes[0]);

            posted.IdDialog = TestGuids[1];
            posted.TextMessage = TestStrings[1];
            posted.IsRead = TestBooleans[1];
            posted.SendingTime = TestDateTimes[1];

            MessagesModel putted = db.PutModel(posted);

            Assert.AreEqual(putted.IdDialog, TestGuids[1]);
            Assert.AreEqual(putted.TextMessage, TestStrings[1]);
            Assert.AreEqual(putted.IsRead, TestBooleans[1]);
            Assert.AreEqual(putted.SendingTime, TestDateTimes[1]);

            db.DeleteModel(posted.Id);

            models = db.GetModels().ToList();

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestPermissions()
        {
            DBConnector.UseTestBase = true;
            PermissionsModel model = new();
            model.PermissionName = TestStrings[0];
            model.PermissionDescription = TestStrings[1];

            Permissions db = new();
            PermissionsModel posted = db.PostModel(model);

            Assert.AreEqual(posted.PermissionName, TestStrings[0]);
            Assert.AreEqual(posted.PermissionDescription, TestStrings[1]);

            System.Collections.Generic.List<PermissionsModel> models = db.GetModels().ToList();

            Assert.AreEqual(models.FirstOrDefault().PermissionName, posted.PermissionName);
            Assert.AreEqual(models.FirstOrDefault().PermissionDescription, posted.PermissionDescription);

            posted.PermissionName = TestStrings[2];
            posted.PermissionDescription = TestStrings[3];

            PermissionsModel putted = db.PutModel(posted);

            Assert.AreEqual(putted.PermissionName, TestStrings[2]);
            Assert.AreEqual(putted.PermissionDescription, TestStrings[3]);

            db.DeleteModel(posted.Id);

            models = db.GetModels().ToList();

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestRoles()
        {
            DBConnector.UseTestBase = true;
            RolesModel model = new();
            model.RoleName = TestStrings[0];
            model.RoleDescription = TestStrings[1];

            Roles db = new();
            RolesModel posted = db.PostModel(model);

            Assert.AreEqual(posted.RoleName, TestStrings[0]);
            Assert.AreEqual(posted.RoleDescription, TestStrings[1]);

            System.Collections.Generic.List<RolesModel> models = db.GetModels().ToList();

            Assert.AreEqual(models.FirstOrDefault().RoleName, posted.RoleName);
            Assert.AreEqual(models.FirstOrDefault().RoleDescription, posted.RoleDescription);

            posted.RoleName = TestStrings[2];
            posted.RoleDescription = TestStrings[3];

            RolesModel putted = db.PutModel(posted);

            Assert.AreEqual(putted.RoleName, TestStrings[2]);
            Assert.AreEqual(putted.RoleDescription, TestStrings[3]);

            db.DeleteModel(posted.Id);

            models = db.GetModels().ToList();

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestRolePermissions()
        {
            DBConnector.UseTestBase = true;
            RolesPermissionsModel model = new();
            model.IdRole = TestGuids[0];
            model.IdPermission = TestGuids[1];

            RolesPermissions db = new();
            RolesPermissionsModel posted = db.PostModel(model);

            Assert.AreEqual(posted.IdRole, TestGuids[0]);
            Assert.AreEqual(posted.IdPermission, TestGuids[1]);

            System.Collections.Generic.List<RolesPermissionsModel> models = db.GetModels().ToList();

            Assert.AreEqual(models.FirstOrDefault().IdRole, TestGuids[0]);
            Assert.AreEqual(models.FirstOrDefault().IdPermission, TestGuids[1]);

            posted.IdRole = TestGuids[2];
            posted.IdPermission = TestGuids[3];

            RolesPermissionsModel putted = db.PutModel(posted);

            Assert.AreEqual(putted.IdRole, TestGuids[2]);
            Assert.AreEqual(putted.IdPermission, TestGuids[3]);

            db.DeleteModel(posted.Id);

            models = db.GetModels().ToList();

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestStudentsGroups()
        {
            DBConnector.UseTestBase = true;
            StudentsGroupsModel model = new();
            model.IdStudent = TestGuids[0];
            model.IdGroup = TestGuids[1];

            StudentsGroups db = new();
            StudentsGroupsModel posted = db.PostModel(model);

            Assert.AreEqual(posted.IdStudent, TestGuids[0]);
            Assert.AreEqual(posted.IdGroup, TestGuids[1]);

            System.Collections.Generic.List<StudentsGroupsModel> models = db.GetModels().ToList();

            Assert.AreEqual(models.FirstOrDefault().IdStudent, TestGuids[0]);
            Assert.AreEqual(models.FirstOrDefault().IdGroup, TestGuids[1]);

            posted.IdStudent = TestGuids[2];
            posted.IdGroup = TestGuids[3];

            StudentsGroupsModel putted = db.PutModel(posted);

            Assert.AreEqual(putted.IdStudent, TestGuids[2]);
            Assert.AreEqual(putted.IdGroup, TestGuids[3]);

            db.DeleteModel(posted.Id);

            models = db.GetModels().ToList();

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestTimeLessons()
        {
            DBConnector.UseTestBase = true;
            TimeLessonsModel model = new();
            model.StartTime = TestTimes[0];
            model.EndTime = TestTimes[1];
            model.BreakTime = TestTimes[2];

            TimeLessons db = new();
            TimeLessonsModel posted = db.PostModel(model);

            Assert.AreEqual(posted.StartTime, TestTimes[0]);
            Assert.AreEqual(posted.EndTime, TestTimes[1]);
            Assert.AreEqual(posted.BreakTime, TestTimes[2]);

            System.Collections.Generic.List<TimeLessonsModel> models = db.GetModels().ToList();

            Assert.AreEqual(models.FirstOrDefault().StartTime, TestTimes[0]);
            Assert.AreEqual(models.FirstOrDefault().EndTime, TestTimes[1]);
            Assert.AreEqual(models.FirstOrDefault().BreakTime, TestTimes[2]);

            posted.StartTime = TestTimes[3];
            posted.EndTime = TestTimes[4];
            posted.BreakTime = TestTimes[5];

            TimeLessonsModel putted = db.PutModel(posted);

            Assert.AreEqual(putted.StartTime, TestTimes[3]);
            Assert.AreEqual(putted.EndTime, TestTimes[4]);
            Assert.AreEqual(putted.BreakTime, TestTimes[5]);

            db.DeleteModel(posted.Id);

            models = db.GetModels().ToList();

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestTypeClasses()
        {
            DBConnector.UseTestBase = true;
            TypeClassesModel model = new();
            model.Title = TestStrings[0];

            TypeClasses db = new();
            TypeClassesModel posted = db.PostModel(model);

            Assert.AreEqual(posted.Title, TestStrings[0]);

            System.Collections.Generic.List<TypeClassesModel> models = db.GetModels().ToList();

            Assert.AreEqual(models.FirstOrDefault().Title, posted.Title);

            posted.Title = TestStrings[1];

            TypeClassesModel putted = db.PutModel(posted);

            Assert.AreEqual(putted.Title, TestStrings[1]);

            db.DeleteModel(posted.Id);

            models = db.GetModels().ToList();

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestUsersDialogs()
        {
            DBConnector.UseTestBase = true;
            UsersDialogsModel model = new();
            model.IdUser = TestGuids[0];
            model.IdDialog = TestGuids[1];

            UsersDialogs db = new();
            UsersDialogsModel posted = db.PostModel(model);

            Assert.AreEqual(posted.IdUser, TestGuids[0]);
            Assert.AreEqual(posted.IdDialog, TestGuids[1]);

            System.Collections.Generic.List<UsersDialogsModel> models = db.GetModels().ToList();

            Assert.AreEqual(models.FirstOrDefault().IdUser, TestGuids[0]);
            Assert.AreEqual(models.FirstOrDefault().IdDialog, TestGuids[1]);

            posted.IdUser = TestGuids[2];
            posted.IdDialog = TestGuids[3];

            UsersDialogsModel putted = db.PutModel(posted);

            Assert.AreEqual(putted.IdUser, TestGuids[2]);
            Assert.AreEqual(putted.IdDialog, TestGuids[3]);

            db.DeleteModel(posted.Id);

            models = db.GetModels().ToList();

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestUsersRoles()
        {
            DBConnector.UseTestBase = true;
            UsersRolesModel model = new();
            model.IdUser = TestGuids[0];
            model.IdRole = TestGuids[1];

            UsersRoles db = new();
            UsersRolesModel posted = db.PostModel(model);

            Assert.AreEqual(posted.IdUser, TestGuids[0]);
            Assert.AreEqual(posted.IdRole, TestGuids[1]);

            System.Collections.Generic.List<UsersRolesModel> models = db.GetModels().ToList();

            Assert.AreEqual(models.FirstOrDefault().IdUser, TestGuids[0]);
            Assert.AreEqual(models.FirstOrDefault().IdRole, TestGuids[1]);

            posted.IdUser = TestGuids[2];
            posted.IdRole = TestGuids[3];

            UsersRolesModel putted = db.PutModel(posted);

            Assert.AreEqual(putted.IdUser, TestGuids[2]);
            Assert.AreEqual(putted.IdRole, TestGuids[3]);

            db.DeleteModel(posted.Id);

            models = db.GetModels().ToList();

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestTestHeaders()
        {
            DBConnector.UseTestBase = true;
            TestHeadersModel model = new();
            model.TestID = TestGuids[0];
            model.GroupID = TestGuids[1];
            model.TeacherID = TestGuids[2];
            model.TestName = TestStrings[0];
            model.TestType = TestStrings[1];
            model.TestTime = TestStrings[2];
            model.CountQuestions = TestIntegers[0];

            TestHeaders db = new();
            TestHeadersModel posted = db.PostModel(model);

            Assert.AreEqual(posted.TestID, TestGuids[0]);
            Assert.AreEqual(posted.GroupID, TestGuids[1]);
            Assert.AreEqual(posted.TeacherID, TestGuids[2]);
            Assert.AreEqual(posted.TestName, TestStrings[0]);
            Assert.AreEqual(posted.TestType, TestStrings[1]);
            Assert.AreEqual(posted.TestTime, TestStrings[2]);
            Assert.AreEqual(posted.CountQuestions, TestIntegers[0]);

            System.Collections.Generic.List<TestHeadersModel> models = db.GetModels().ToList();

            Assert.AreEqual(models.FirstOrDefault().TestID, TestGuids[0]);
            Assert.AreEqual(models.FirstOrDefault().GroupID, TestGuids[1]);
            Assert.AreEqual(models.FirstOrDefault().TeacherID, TestGuids[2]);
            Assert.AreEqual(models.FirstOrDefault().TestName, TestStrings[0]);
            Assert.AreEqual(models.FirstOrDefault().TestType, TestStrings[1]);
            Assert.AreEqual(models.FirstOrDefault().TestTime, TestStrings[2]);
            Assert.AreEqual(models.FirstOrDefault().CountQuestions, TestIntegers[0]);

            posted.TestID = TestGuids[3];
            posted.GroupID = TestGuids[4];
            posted.TeacherID = TestGuids[5];
            posted.TestName = TestStrings[3];
            posted.TestType = TestStrings[4];
            posted.TestTime = TestStrings[5];
            posted.CountQuestions = TestIntegers[1];

            TestHeadersModel putted = db.PutModel(posted);

            Assert.AreEqual(putted.TestID, TestGuids[3]);
            Assert.AreEqual(putted.GroupID, TestGuids[4]);
            Assert.AreEqual(putted.TeacherID, TestGuids[5]);
            Assert.AreEqual(putted.TestName, TestStrings[3]);
            Assert.AreEqual(putted.TestType, TestStrings[4]);
            Assert.AreEqual(putted.TestTime, TestStrings[5]);
            Assert.AreEqual(putted.CountQuestions, TestIntegers[1]);

            db.DeleteModel(posted.ID);

            models = db.GetModels().ToList();

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestTestResults()
        {
            DBConnector.UseTestBase = true;
            TestResultsModel model = new();
            model.TestID = TestGuids[0];
            model.UserID = TestGuids[1];
            model.StudentName = TestStrings[0];
            model.StudentSurname = TestStrings[1];
            model.CorrectAnswers = TestIntegers[0];
            model.TestCompleted = TestBooleans[0];

            TestResults db = new();
            TestResultsModel posted = db.PostModel(model);

            Assert.AreEqual(posted.TestID, TestGuids[0]);
            Assert.AreEqual(posted.UserID, TestGuids[1]);
            Assert.AreEqual(posted.StudentName, TestStrings[0]);
            Assert.AreEqual(posted.StudentSurname, TestStrings[1]);
            Assert.AreEqual(posted.CorrectAnswers, TestIntegers[0]);
            Assert.AreEqual(posted.TestCompleted, TestBooleans[0]);

            System.Collections.Generic.List<TestResultsModel> models = db.GetModels().ToList();

            Assert.AreEqual(models.FirstOrDefault().TestID, TestGuids[0]);
            Assert.AreEqual(models.FirstOrDefault().UserID, TestGuids[1]);
            Assert.AreEqual(models.FirstOrDefault().StudentName, TestStrings[0]);
            Assert.AreEqual(models.FirstOrDefault().StudentSurname, TestStrings[1]);
            Assert.AreEqual(models.FirstOrDefault().CorrectAnswers, TestIntegers[0]);
            Assert.AreEqual(models.FirstOrDefault().TestCompleted, TestBooleans[0]);

            posted.TestID = TestGuids[2];
            posted.UserID = TestGuids[3];
            posted.StudentName = TestStrings[2];
            posted.StudentSurname = TestStrings[3];
            posted.CorrectAnswers = TestIntegers[1];
            posted.TestCompleted = TestBooleans[1];

            TestResultsModel putted = db.PutModel(posted);

            Assert.AreEqual(putted.TestID, TestGuids[2]);
            Assert.AreEqual(putted.UserID, TestGuids[3]);
            Assert.AreEqual(putted.StudentName, TestStrings[2]);
            Assert.AreEqual(putted.StudentSurname, TestStrings[3]);
            Assert.AreEqual(putted.CorrectAnswers, TestIntegers[1]);
            Assert.AreEqual(putted.TestCompleted, TestBooleans[1]);

            db.DeleteModel(posted.ID);

            models = db.GetModels().ToList();

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestTests()
        {
            DBConnector.UseTestBase = true;
            TestsModel model = new();
            model.Questions = TestStrings[0];

            Tests db = new();
            TestsModel posted = db.PostModel(model);

            Assert.AreEqual(posted.Questions, TestStrings[0]);

            System.Collections.Generic.List<TestsModel> models = db.GetModels().ToList();

            Assert.AreEqual(models.FirstOrDefault().Questions, TestStrings[0]);

            posted.Questions = TestStrings[1];

            TestsModel putted = db.PutModel(posted);

            Assert.AreEqual(putted.Questions, TestStrings[1]);

            db.DeleteModel(posted.ID);

            models = db.GetModels().ToList();

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestComments()
        {
            DBConnector.UseTestBase = true;
            CommentModel model = new();
            model.Message = TestStrings[0];

            Comments db = new();
            CommentModel posted = db.PostModel(model);

            Assert.AreEqual(posted.Message, TestStrings[0]);

            System.Collections.Generic.List<CommentModel> models = db.GetModels().ToList();

            Assert.AreEqual(models.FirstOrDefault().Message, TestStrings[0]);

            posted.Message = TestStrings[1];

            CommentModel putted = db.PutModel(posted);

            Assert.AreEqual(putted.Message, TestStrings[1]);

            db.DeleteModel(posted.Id);

            models = db.GetModels().ToList();

            Assert.AreEqual(models.Count, 0);
        }
        [TestMethod]
        public void TestMaterials()
        {
            DBConnector.UseTestBase = true;
            MaterialsModel model = new();
            model.Title = TestStrings[0];

            Materials db = new();
            MaterialsModel posted = db.PostModel(model);

            Assert.AreEqual(posted.Title, TestStrings[0]);

            System.Collections.Generic.List<MaterialsModel> models = db.GetModels().ToList();

            Assert.AreEqual(models.FirstOrDefault().Title, TestStrings[0]);

            posted.Title = TestStrings[1];

            MaterialsModel putted = db.PutModel(posted);

            Assert.AreEqual(putted.Title, TestStrings[1]);

            db.DeleteModel(posted.Id);

            models = db.GetModels().ToList();

            Assert.AreEqual(models.Count, 0);
        }
        #endregion
    }
}
