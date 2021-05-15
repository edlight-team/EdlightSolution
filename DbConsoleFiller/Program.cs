using ApplicationModels.Models;
using ApplicationServices.HashingService;
using ApplicationServices.WebApiService;
using System;
using System.Threading.Tasks;

namespace DbConsoleFiller
{
    class Program
    {
        static RolesModel studentRole;
        static RolesModel teacherRole;

        static UserModel student;
        static UserModel teacher;

        static void Main() => RunFill().Wait();
        static async Task RunFill()
        {
            Console.WriteLine("Start filling");
            IWebApiService api = new WebApiServiceImplementation();
            IHashingService hashing = new HashingImplementation();

            await FillAcademicDisciplines(api);
            await FillAudiencesModel(api);
            await FillTypeClassesModel(api);
            await FillRolesModel(api);
            await FillUsersModel(api, hashing);
            await FillUsersRolesModel(api);

            Console.WriteLine("Fill Complete");
            Console.ReadKey();
        }
        static async Task FillAcademicDisciplines(IWebApiService api)
        {
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
            Console.WriteLine("type " + model.GetType().Name +  " in db count = " + count);
        }
        static async Task FillAudiencesModel(IWebApiService api)
        {
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
            Console.WriteLine("type " + model.GetType().Name +  " in db count = " + count);
        }
        static async Task FillTypeClassesModel(IWebApiService api)
        {
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
            Console.WriteLine("type " + model.GetType().Name +  " in db count = " + count);
        }
        static async Task FillRolesModel(IWebApiService api)
        {
            await api.DeleteAll(WebApiTableNames.Roles);

            RolesModel model = new();
            model.RoleName = "student";
            model.RoleDescription = "Студент";
            studentRole = await api.PostModel(model, WebApiTableNames.Roles);
            model.RoleName = "teacher";
            model.RoleDescription = "Преподаватель";
            teacherRole = await api.PostModel(model, WebApiTableNames.Roles);

            int count = (await api.GetModels<RolesModel>(WebApiTableNames.Roles)).Count;
            Console.WriteLine("type " + model.GetType().Name +  " in db count = " + count);
        }
        static async Task FillUsersModel(IWebApiService api, IHashingService hashing)
        {
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

            int count = (await api.GetModels<UserModel>(WebApiTableNames.Roles)).Count;
            Console.WriteLine("type " + model.GetType().Name +  " in db count = " + count);
        }
        static async Task FillUsersRolesModel(IWebApiService api)
        {
            await api.DeleteAll(WebApiTableNames.UsersRoles);

            UsersRolesModel model = new();
            model.IdRole = studentRole.Id;
            model.IdUser = student.ID;
            await api.PostModel(model, WebApiTableNames.UsersRoles);
            model.IdRole = teacherRole.Id;
            model.IdUser = teacher.ID;
            await api.PostModel(model, WebApiTableNames.UsersRoles);

            int count = (await api.GetModels<UsersRolesModel>(WebApiTableNames.UsersRoles)).Count;
            Console.WriteLine("type " + model.GetType().Name + " in db count = " + count);
        }
    }
}
