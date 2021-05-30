using ApplicationModels.Models;
using ApplicationServices.HashingService;
using ApplicationServices.PermissionService;
using ApplicationServices.WebApiService;
using Newtonsoft.Json;
using RandomFriendlyNameGenerator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DbFiller
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
        private static StoragesHeadersModel storage;
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
            await FillStorageHeader();
            await FillStorageFile();

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

            string aud_path = Environment.CurrentDirectory + "\\audiences.txt";
            string buffer = System.IO.File.ReadAllText(aud_path);
            string[] audiences_text = buffer.Split(Environment.NewLine.ToCharArray());
            List<string> audiences_to_DB = new();
            foreach (var item in audiences_text)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    audiences_to_DB.Add(item);
                }
            }
            audiences_to_DB = audiences_to_DB.Distinct().OrderBy(a => a).ToList();

            foreach (var item in audiences_to_DB)
            {
                AudiencesModel model = new();
                model.NumberAudience = item;
                await api.PostModel(model, WebApiTableNames.Audiences);
            }

            int count = (await api.GetModels<AudiencesModel>(WebApiTableNames.Audiences)).Count;
            Console.Write("type " + nameof(AudiencesModel) + " in db count = " + count);
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
            model.ShortTitle = "Лек";
            model.ColorHex = "#9966CC";
            await api.PostModel(model, WebApiTableNames.TypeClasses);

            model.Title = "Практика";
            model.ShortTitle = "Прак";
            model.ColorHex = "#44944A";
            await api.PostModel(model, WebApiTableNames.TypeClasses);

            model.Title = "Практическая работа";
            model.ShortTitle = "Пр";
            model.ColorHex = "#C1876B";
            await api.PostModel(model, WebApiTableNames.TypeClasses);

            model.Title = "Лабораторная работа";
            model.ShortTitle = "Лаб";
            model.ColorHex = "#FFDEAD";
            await api.PostModel(model, WebApiTableNames.TypeClasses);

            model.Title = "Курсовая работа";
            model.ShortTitle = "КР";
            model.ColorHex = "#FFCF40";
            await api.PostModel(model, WebApiTableNames.TypeClasses);

            model.Title = "Государственная Аттестационная Комиссия";
            model.ShortTitle = "ГАК";
            model.ColorHex = "#755D9A";
            await api.PostModel(model, WebApiTableNames.TypeClasses);

            model.Title = "Выпускная Квалификационная Работа";
            model.ShortTitle = "ВКР";
            model.ColorHex = "#E32636";
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
            List<UserModel> users = GenerateRandomUsers();
            foreach (UserModel item in users)
            {
                try
                {
                    otherusers.Add(await api.PostModel(item, WebApiTableNames.Users));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
            }
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
                    //IdRole = item.Age >= 22 ? teacherRole.Id : studentRole.Id,
                    IdRole = studentRole.Id,
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

            string gr_path = Environment.CurrentDirectory + "\\groups.txt";
            string buffer = System.IO.File.ReadAllText(gr_path);
            string[] groups_text = buffer.Split(Environment.NewLine.ToCharArray());
            List<string> groups_to_DB = new();
            foreach (var item in groups_text)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    groups_to_DB.Add(item);
                }
            }
            groups_to_DB = groups_to_DB.Distinct().OrderBy(a => a).ToList();

            groups = new();
            foreach (var item in groups_to_DB)
            {
                GroupsModel model = new();
                model.Group = item;
                groups.Add(await api.PostModel(model, WebApiTableNames.Groups));
            }

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

        private static async Task FillStorageHeader()
        {
            Stopwatch watch = new();
            watch.Start();
            await api.DeleteAll(WebApiTableNames.StoragesHeaders);

            StoragesHeadersModel model = new();
            model.CreatorID = teacher.ID;
            model.GroupID = groups[0].Id;
            model.StorageName = "Хранилище";
            model.DateCloseStorage = "00";
            storage = await api.PostModel(model, WebApiTableNames.StoragesHeaders);

            int count = (await api.GetModels<TestResultsModel>(WebApiTableNames.StoragesHeaders)).Count;
            Console.Write("type " + model.GetType().Name + " in db count = " + count);
            watch.Stop();
            Console.WriteLine(" ellapsed time: " + watch.Elapsed.TotalSeconds + " sec., " + watch.ElapsedMilliseconds + " ms.");
        }

        private static async Task FillStorageFile()
        {
            Stopwatch watch = new();
            watch.Start();
            await api.DeleteAll(WebApiTableNames.StorageFiles);

            StorageFilesModel model = new();
            model.StorageID = storage.ID;
            model.StudentID = student.ID;
            model.FileName = "file";
            await api.PostModel(model, WebApiTableNames.StorageFiles);

            int count = (await api.GetModels<TestResultsModel>(WebApiTableNames.StorageFiles)).Count;
            Console.Write("type " + model.GetType().Name + " in db count = " + count);
            watch.Stop();
            Console.WriteLine(" ellapsed time: " + watch.Elapsed.TotalSeconds + " sec., " + watch.ElapsedMilliseconds + " ms.");
        }

        public static List<UserModel> GenerateRandomUsers()
        {
            int minAge = 16, maxAge = 45;

            Random rnd = new();

            var male_names = NameGenerator.PersonNames.Get(150, NameGender.Male, NameComponents.FirstNameMiddleNameLastName, separator: ":");
            var female_names = NameGenerator.PersonNames.Get(150, NameGender.Female, NameComponents.FirstNameMiddleNameLastName, separator: ":");

            List<UserModel> users = new();
            foreach (var item in male_names)
            {
                string[] parts = item.Split(':');
                UserModel user = new()
                {
                    Name = parts[0],
                    Surname = parts[1],
                    Patrnymic = parts[2],
                    Age = rnd.Next(minAge, maxAge),
                    Sex = 1,
                };
                user.Login = new Regex(@"@+\w*").Replace(user.Name, "").ToLower() + $"{user.Surname[0]}{user.Patrnymic[0]}";
                user.Password = hashing.EncodeString(user.Login);
                users.Add(user);
            }
            foreach (var item in female_names)
            {
                string[] parts = item.Split(':');
                UserModel user = new()
                {
                    Name = parts[0],
                    Surname = parts[1],
                    Patrnymic = parts[2],
                    Age = rnd.Next(minAge, maxAge),
                    Sex = 2,
                };
                user.Login = new Regex(@"@+\w*").Replace(user.Name, "").ToLower() + $"{user.Surname[0]}{user.Patrnymic[0]}";
                user.Password = hashing.EncodeString(user.Login);
                users.Add(user);
            }

            return users;
        }
    }
}
