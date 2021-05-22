using ApplicationModels.Models;
using ApplicationServices.HashingService;
using ApplicationServices.PermissionService;
using ApplicationServices.WebApiService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DbConsoleFiller
{
    internal class Program
    {
        private static RolesModel studentRole;
        private static RolesModel teacherRole;
        private static RolesModel umoRole;
        private static UserModel student;
        private static UserModel teacher;
        private static UserModel umoadmin;
        private static List<UserModel> otherusers;
        private static List<UsersRolesModel> otherUserRoles;
        private static List<GroupsModel> groups;
        private static TestsModel test;
        private static readonly Dictionary<string, PermissionsModel> permissionNamesDictionary = new();
        private static IWebApiService api;
        private static IHashingService hashing;

        private static void Main() => RunFill().Wait();

        private static async Task RunFill()
        {
            Stopwatch all_watch = new();
            Console.WriteLine("Start filling");
            all_watch.Start();
            api = new WebApiServiceImplementation();
            hashing = new HashingImplementation();

            await FillAcademicDisciplines();
            await FillAudiencesModel();
            await FillTypeClassesModel();
            await FillRolesModel();
            await FillPermissions();
            await FillRolePermissions();
            await FillUsersModel();
            await FillUsersRolesModel();
            await FillGroupsModel();
            await FillStudentsGroupsModel();
            await FillTestsModel();
            await FillTestHeaderModel();
            await FillTestResult();

            Console.Write("Fill Complete, time ellapsed: ");
            all_watch.Stop();
            Console.WriteLine(all_watch.Elapsed.TotalSeconds + " sec.");
            Console.ReadKey();
        }

        private static async Task FillAcademicDisciplines()
        {
            Stopwatch watch = new();
            watch.Start();
            await api.DeleteAll(WebApiTableNames.AcademicDisciplines);

            AcademicDisciplinesModel model = new();
            model.Title = "Предмет 1";
            await api.PostModel(model, WebApiTableNames.AcademicDisciplines);
            model.Title = "Предмет 2";
            await api.PostModel(model, WebApiTableNames.AcademicDisciplines);
            model.Title = "Предмет 3";
            await api.PostModel(model, WebApiTableNames.AcademicDisciplines);
            model.Title = "Предмет 4";
            await api.PostModel(model, WebApiTableNames.AcademicDisciplines);
            model.Title = "Предмет 5";
            await api.PostModel(model, WebApiTableNames.AcademicDisciplines);

            int count = (await api.GetModels<AcademicDisciplinesModel>(WebApiTableNames.AcademicDisciplines)).Count;
            Console.Write("type " + model.GetType().Name + " in db count = " + count);
            watch.Stop();
            Console.WriteLine(" ellapsed time: " + watch.Elapsed.TotalSeconds + " sec., " + watch.ElapsedMilliseconds + " ms.");
        }

        private static async Task FillAudiencesModel()
        {
            Stopwatch watch = new();
            watch.Start();
            await api.DeleteAll(WebApiTableNames.Audiences);

            AudiencesModel model = new();
            model.NumberAudience = "Аудитория 208";
            await api.PostModel(model, WebApiTableNames.Audiences);
            model.NumberAudience = "Аудитория 306";
            await api.PostModel(model, WebApiTableNames.Audiences);
            model.NumberAudience = "Аудитория 214";
            await api.PostModel(model, WebApiTableNames.Audiences);
            model.NumberAudience = "Аудитория 410";
            await api.PostModel(model, WebApiTableNames.Audiences);
            model.NumberAudience = "Аудитория 415";
            await api.PostModel(model, WebApiTableNames.Audiences);

            int count = (await api.GetModels<AudiencesModel>(WebApiTableNames.Audiences)).Count;
            Console.Write("type " + model.GetType().Name + " in db count = " + count);
            watch.Stop();
            Console.WriteLine(" ellapsed time: " + watch.Elapsed.TotalSeconds + " sec., " + watch.ElapsedMilliseconds + " ms.");
        }

        private static async Task FillTypeClassesModel()
        {
            Stopwatch watch = new();
            watch.Start();
            await api.DeleteAll(WebApiTableNames.TypeClasses);

            TypeClassesModel model = new();
            model.Title = "Лекция";
            await api.PostModel(model, WebApiTableNames.TypeClasses);
            model.Title = "Лабораторная работа";
            await api.PostModel(model, WebApiTableNames.TypeClasses);
            model.Title = "Практика";
            await api.PostModel(model, WebApiTableNames.TypeClasses);
            model.Title = "Экзамен";
            await api.PostModel(model, WebApiTableNames.TypeClasses);

            int count = (await api.GetModels<TypeClassesModel>(WebApiTableNames.TypeClasses)).Count;
            Console.Write("type " + model.GetType().Name + " in db count = " + count);
            watch.Stop();
            Console.WriteLine(" ellapsed time: " + watch.Elapsed.TotalSeconds + " sec., " + watch.ElapsedMilliseconds + " ms.");
        }

        private static async Task FillRolesModel()
        {
            Stopwatch watch = new();
            watch.Start();
            await api.DeleteAll(WebApiTableNames.Roles);

            RolesModel model = new();
            model.RoleName = "student";
            model.RoleDescription = "Студент";
            studentRole = await api.PostModel(model, WebApiTableNames.Roles);
            model.RoleName = "teacher";
            model.RoleDescription = "Преподаватель";
            teacherRole = await api.PostModel(model, WebApiTableNames.Roles);
            model.RoleName = "umo";
            model.RoleDescription = "Учебно-методический отдел";
            umoRole = await api.PostModel(model, WebApiTableNames.Roles);

            int count = (await api.GetModels<RolesModel>(WebApiTableNames.Roles)).Count;
            Console.Write("type " + model.GetType().Name + " in db count = " + count);
            watch.Stop();
            Console.WriteLine(" ellapsed time: " + watch.Elapsed.TotalSeconds + " sec., " + watch.ElapsedMilliseconds + " ms.");
        }

        private static async Task FillPermissions()
        {
            Stopwatch watch = new();
            watch.Start();
            await api.DeleteAll(WebApiTableNames.Permissions);

            Type permission_class_type = typeof(PermissionNames);
            foreach (System.Reflection.MemberInfo member in permission_class_type.GetMembers())
            {
                object[] attr = member.GetCustomAttributes(typeof(PermissionDescription), true);
                if (attr.Length != 0)
                {
                    string description = (attr[0] as PermissionDescription).Description;
                    string name = member.Name;

                    PermissionsModel posted =
                        await api.PostModel(new PermissionsModel() { PermissionName = name, PermissionDescription = description }, WebApiTableNames.Permissions);

                    permissionNamesDictionary.Add(name, posted);
                }
            }

            int count = (await api.GetModels<PermissionsModel>(WebApiTableNames.Permissions)).Count;
            Console.Write("type " + nameof(PermissionsModel) + " in db count = " + count);
            watch.Stop();
            Console.WriteLine(" ellapsed time: " + watch.Elapsed.TotalSeconds + " sec., " + watch.ElapsedMilliseconds + " ms.");
        }

        private static async Task FillRolePermissions()
        {
            Stopwatch watch = new();
            watch.Start();
            await api.DeleteAll(WebApiTableNames.RolesPermissions);

            //Студент
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = studentRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.GetScheduleComments].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = studentRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.CreateScheduleComments].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = studentRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.EditScheduleComments].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = studentRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.DeleteScheduleComments].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = studentRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.GetFile].Id
            }, WebApiTableNames.RolesPermissions);

            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = studentRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.TakeTest].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = studentRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.ViewSelfTestResults].Id
            }, WebApiTableNames.RolesPermissions);

            //Преподаватель
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = teacherRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.ManageGroups].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = teacherRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.GetScheduleManaging].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = teacherRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.GetScheduleComments].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = teacherRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.CreateScheduleComments].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = teacherRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.EditScheduleComments].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = teacherRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.DeleteScheduleComments].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = teacherRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.SetScheduleStatus].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = teacherRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.GetFile].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = teacherRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.PushFile].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = teacherRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.DeleteFile].Id
            }, WebApiTableNames.RolesPermissions);

            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = teacherRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.CreateTestRecords].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = teacherRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.UpdateTestRecord].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = teacherRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.DeleteTestRecord].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = teacherRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.SetTestFilter].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = teacherRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.ViewStudentTestResults].Id
            }, WebApiTableNames.RolesPermissions);

            //УМО
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = umoRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.ManageGroups].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = umoRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.ManageDictionaries].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = umoRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.GetScheduleManaging].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = umoRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.GetScheduleComments].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = umoRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.CreateScheduleComments].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = umoRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.CreateScheduleRecords].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = umoRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.EditScheduleRecords].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = umoRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.SetScheduleStatus].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = umoRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.DeleteScheduleRecord].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = umoRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.GetFile].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = umoRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.PushFile].Id
            }, WebApiTableNames.RolesPermissions);
            await api.PostModel(new RolesPermissionsModel()
            {
                IdRole = umoRole.Id,
                IdPermission = permissionNamesDictionary[PermissionNames.DeleteFile].Id
            }, WebApiTableNames.RolesPermissions);

            int count = (await api.GetModels<RolesPermissionsModel>(WebApiTableNames.RolesPermissions)).Count;
            Console.Write("type " + nameof(RolesPermissionsModel) + " in db count = " + count);
            watch.Stop();
            Console.WriteLine(" ellapsed time: " + watch.Elapsed.TotalSeconds + " sec., " + watch.ElapsedMilliseconds + " ms.");
        }

        private static async Task FillUsersModel()
        {
            Stopwatch watch = new();
            watch.Start();
            await api.DeleteAll(WebApiTableNames.Users);

            UserModel model = new();
            model.Login = "admin";
            model.Password = hashing.EncodeString("admin");
            model.Name = "Админ";
            model.Surname = "Админов";
            model.Patrnymic = "Админович";
            model.Age = 18;
            model.Sex = 1;
            await api.PostModel(model, WebApiTableNames.Users);
            model.Login = "student";
            model.Password = hashing.EncodeString("student");
            model.Name = "Студент";
            model.Surname = "Студентов";
            model.Patrnymic = "Студентович";
            model.Age = 18;
            model.Sex = 1;
            student = await api.PostModel(model, WebApiTableNames.Users);
            model.Login = "teacher";
            model.Password = hashing.EncodeString("teacher");
            model.Name = "Преподаватель";
            model.Surname = "Преподавателев";
            model.Patrnymic = "Преподавателевич";
            model.Age = 35;
            model.Sex = 1;
            teacher = await api.PostModel(model, WebApiTableNames.Users);
            model.Login = "umoadmin";
            model.Password = hashing.EncodeString("umoadmin");
            model.Name = "Сотрудник";
            model.Surname = "Учебно-методического";
            model.Patrnymic = "Отдела";
            model.Age = 35;
            model.Sex = 2;
            umoadmin = await api.PostModel(model, WebApiTableNames.Users);

            otherusers = new();
            List<UserModel> users = GenerateRandomUsers(50);
            foreach (UserModel item in users)
                otherusers.Add(await api.PostModel(item, WebApiTableNames.Users));
            int count = (await api.GetModels<UserModel>(WebApiTableNames.Users)).Count;
            Console.Write("type " + model.GetType().Name + " in db count = " + count);
            watch.Stop();
            Console.WriteLine(" ellapsed time: " + watch.Elapsed.TotalSeconds + " sec., " + watch.ElapsedMilliseconds + " ms.");
        }

        private static async Task FillUsersRolesModel()
        {
            Stopwatch watch = new();
            watch.Start();
            await api.DeleteAll(WebApiTableNames.UsersRoles);

            UsersRolesModel model = new();
            otherUserRoles = new();
            model.IdRole = studentRole.Id;
            model.IdUser = student.ID;
            await api.PostModel(model, WebApiTableNames.UsersRoles);
            model.IdRole = teacherRole.Id;
            model.IdUser = teacher.ID;
            await api.PostModel(model, WebApiTableNames.UsersRoles);
            model.IdRole = umoRole.Id;
            model.IdUser = umoadmin.ID;
            await api.PostModel(model, WebApiTableNames.UsersRoles);

            foreach (UserModel item in otherusers)
                otherUserRoles.Add(await api.PostModel(new UsersRolesModel()
                {
                    IdRole = item.Age >= 22 ? teacherRole.Id : studentRole.Id,
                    IdUser = item.ID
                }, WebApiTableNames.UsersRoles));

            int count = (await api.GetModels<UsersRolesModel>(WebApiTableNames.UsersRoles)).Count;
            Console.Write("type " + model.GetType().Name + " in db count = " + count);
            watch.Stop();
            Console.WriteLine(" ellapsed time: " + watch.Elapsed.TotalSeconds + " sec., " + watch.ElapsedMilliseconds + " ms.");
        }

        private static async Task FillGroupsModel()
        {
            Stopwatch watch = new();
            watch.Start();
            await api.DeleteAll(WebApiTableNames.Groups);

            int groupNumder = 1;

            groups = new();
            while (groupNumder <= 5)
            {
                groups.Add(await api.PostModel(new GroupsModel() { Group = $"Группа {groupNumder++}" }, WebApiTableNames.Groups));
            };

            int count = (await api.GetModels<GroupsModel>(WebApiTableNames.Groups)).Count;
            Console.Write("type " + nameof(GroupsModel) + " in db count = " + count);
            watch.Stop();
            Console.WriteLine(" ellapsed time: " + watch.Elapsed.TotalSeconds + " sec., " + watch.ElapsedMilliseconds + " ms.");
        }

        private static async Task FillStudentsGroupsModel()
        {
            Stopwatch watch = new();
            watch.Start();
            await api.DeleteAll(WebApiTableNames.StudentsGroups);

            await api.PostModel(new StudentsGroupsModel() { IdGroup = groups[0].Id, IdStudent = student.ID }, WebApiTableNames.StudentsGroups);

            Random random = new();
            for (int i = 0; i < otherusers.Count; i++)
            {
                if (otherUserRoles[i].IdRole == studentRole.Id)
                {
                    StudentsGroupsModel model = new();
                    model.IdGroup = groups[random.Next(0, groups.Count - 1)].Id;
                    model.IdStudent = otherusers[i].ID;
                    await api.PostModel(model, WebApiTableNames.StudentsGroups);
                }
            }

            int count = (await api.GetModels<StudentsGroupsModel>(WebApiTableNames.StudentsGroups)).Count;
            Console.Write("type " + nameof(StudentsGroupsModel) + " in db count = " + count);
            watch.Stop();
            Console.WriteLine(" ellapsed time: " + watch.Elapsed.TotalSeconds + " sec., " + watch.ElapsedMilliseconds + " ms.");
        }

        private static async Task FillTestsModel()
        {
            Stopwatch watch = new();
            watch.Start();
            await api.DeleteAll(WebApiTableNames.Tests);

            List<QuestionsModel> list = new();
            QuestionsModel questions = new();
            questions.Question = "Вопрос1";
            questions.NumberQuestion = 1;
            questions.CorrectAnswerIndex = 1;
            questions.AnswerOptions = new System.Collections.ObjectModel.ObservableCollection<TestAnswer>()
            {
                new TestAnswer(){ Answer= "ответ1"},
                new TestAnswer(){ Answer= "ответ2"},
                new TestAnswer(){ Answer= "ответ3"},
                new TestAnswer(){ Answer= "ответ4"}
            };
            list.Add(questions);
            questions.Question = "Вопрос2";
            questions.NumberQuestion = 2;
            list.Add(questions);
            questions.Question = "Вопрос3";
            questions.NumberQuestion = 3;
            list.Add(questions);
            TestsModel model = new();
            model.Questions = JsonConvert.SerializeObject(list);
            test = await api.PostModel(model, WebApiTableNames.Tests);

            int count = (await api.GetModels<TestsModel>(WebApiTableNames.Tests)).Count;
            Console.Write("type " + model.GetType().Name + " in db count = " + count);
            watch.Stop();
            Console.WriteLine(" ellapsed time: " + watch.Elapsed.TotalSeconds + " sec., " + watch.ElapsedMilliseconds + " ms.");
        }

        private static async Task FillTestHeaderModel()
        {
            Stopwatch watch = new();
            watch.Start();
            await api.DeleteAll(WebApiTableNames.TestHeaders);

            TestHeadersModel model = new();
            model.GroupID = groups[0].Id;
            model.TeacherID = teacher.ID;
            model.CountQuestions = 3;
            model.TestID = test.ID;
            model.TestName = "Тест1";
            model.TestType = "Контрольная работа";
            model.TestTime = new DateTime(10, 10, 10, 1, 0, 0).ToLongTimeString();
            model.TestStartDate = new DateTime(10, 10, 10, 1, 0, 0).ToString();
            model.TestEndDate = new DateTime(10, 10, 10, 1, 0, 0).ToString();
            await api.PostModel(model, WebApiTableNames.TestHeaders);

            int count = (await api.GetModels<TestHeadersModel>(WebApiTableNames.TestHeaders)).Count;
            Console.Write("type " + model.GetType().Name + " in db count = " + count);
            watch.Stop();
            Console.WriteLine(" ellapsed time: " + watch.Elapsed.TotalSeconds + " sec., " + watch.ElapsedMilliseconds + " ms.");
        }

        private static async Task FillTestResult()
        {
            Stopwatch watch = new();
            watch.Start();
            await api.DeleteAll(WebApiTableNames.TestResults);

            TestResultsModel model = new();
            model.TestID = test.ID;
            model.UserID = student.ID;
            model.StudentName = student.Name;
            model.StudentSurname = student.Surname;
            model.TestCompleted = false;
            model.CorrectAnswers = 0;
            await api.PostModel(model, WebApiTableNames.TestResults);

            int count = (await api.GetModels<TestResultsModel>(WebApiTableNames.TestResults)).Count;
            Console.Write("type " + model.GetType().Name + " in db count = " + count);
            watch.Stop();
            Console.WriteLine(" ellapsed time: " + watch.Elapsed.TotalSeconds + " sec., " + watch.ElapsedMilliseconds + " ms.");
        }

        public static List<UserModel> GenerateRandomUsers(int countUsers)
        {
            PersonGenerator.GeneratorSettings settings = new()
            {
                Age = true,
                Language = PersonGenerator.Languages.English,
                FirstName = true,
                MiddleName = true,
                LastName = true,
                MinAge = 16,
                MaxAge = 25
            };
            PersonGenerator.PersonGenerator generator = new(settings);

            List<PersonGenerator.Person> generated = generator.Generate(countUsers);

            List<UserModel> users = new();

            foreach (PersonGenerator.Person item in generated)
            {
                users.Add(new UserModel()
                {
                    Name = item.FirstName,
                    Surname = item.MiddleName,
                    Patrnymic = item.LastName,
                    Age = item.Age,
                    Sex = item.FirstName.Length > 6 ? 2 : 1,
                    Login = new Regex(@"@+\w*").Replace(item.FirstName, ""),
                    Password = hashing.EncodeString(new Regex(@"@+\w*").Replace(item.FirstName, ""))
                });
            }
            return users;
        }
    }
}
