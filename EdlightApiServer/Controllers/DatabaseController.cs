using Microsoft.AspNetCore.Mvc;
using SqliteDataExecuter.Entities;
using System;

namespace EdlightApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        [HttpGet]
        public object Get([FromHeader] string Target)
        {
            try
            {
                int count = 0;
                if (string.IsNullOrEmpty(Target))
                {
                    count += new AcademicDisciplines().DeleteAll();
                    count += new Audiences().DeleteAll();
                    count += new Comments().DeleteAll();
                    count += new Dialogs().DeleteAll();
                    count += new Groups().DeleteAll();
                    count += new Lessons().DeleteAll();
                    count += new Materials().DeleteAll();
                    count += new Messages().DeleteAll();
                    count += new Permissions().DeleteAll();
                    count += new Roles().DeleteAll();
                    count += new RolesPermissions().DeleteAll();
                    count += new StudentsGroups().DeleteAll();
                    count += new TimeLessons().DeleteAll();
                    count += new TypeClasses().DeleteAll();
                    count += new Users().DeleteAll();
                    count += new UsersDialogs().DeleteAll();
                    count += new UsersRoles().DeleteAll();
                    count += new Tests().DeleteAll();
                    count += new TestHeaders().DeleteAll();
                    count += new TestResults().DeleteAll();
                    count += new StoragesHeaders().DeleteAll();
                    count += new StorageFiles().DeleteAll();
                    count += new ManualsFiles().DeleteAll();
                }
                else
                {
                    count += Target switch
                    {
                        nameof(AcademicDisciplines) => new AcademicDisciplines().DeleteAll(),
                        nameof(Audiences) => new Audiences().DeleteAll(),
                        nameof(Comments) => new Comments().DeleteAll(),
                        nameof(Dialogs) => new Dialogs().DeleteAll(),
                        nameof(Groups) => new Groups().DeleteAll(),
                        nameof(Lessons) => new Lessons().DeleteAll(),
                        nameof(Materials) => new Materials().DeleteAll(),
                        nameof(Messages) => new Messages().DeleteAll(),
                        nameof(Permissions) => new Permissions().DeleteAll(),
                        nameof(Roles) => new Roles().DeleteAll(),
                        nameof(RolesPermissions) => new RolesPermissions().DeleteAll(),
                        nameof(StudentsGroups) => new StudentsGroups().DeleteAll(),
                        nameof(TimeLessons) => new TimeLessons().DeleteAll(),
                        nameof(TypeClasses) => new TypeClasses().DeleteAll(),
                        nameof(Users) => new Users().DeleteAll(),
                        nameof(UsersDialogs) => new UsersDialogs().DeleteAll(),
                        nameof(UsersRoles) => new UsersRoles().DeleteAll(),
                        nameof(Tests) => new Tests().DeleteAll(),
                        nameof(TestHeaders) => new TestHeaders().DeleteAll(),
                        nameof(TestResults) => new TestResults().DeleteAll(),
                        nameof(StoragesHeaders) => new StoragesHeaders().DeleteAll(),
                        nameof(StorageFiles) => new StorageFiles().DeleteAll(),
                        nameof(ManualsFiles) => new ManualsFiles().DeleteAll(),
                        _ => throw new Exception("Не верно указана сущность для удаления"),
                    };
                }
                return Ok("Успешно удалено " + count + " сущностей");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Произошла ошибка " + ex.Message);
            }
        }
    }
}
