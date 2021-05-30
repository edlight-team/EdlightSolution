using System;
using RandomFriendlyNameGenerator;

namespace NameGeneratorTest
{
    class Program
    {
        static void Main()
        {
            int minAge = 16, maxAge = 45;

            Random rnd = new();

            var names = NameGenerator.PersonNames.Get(150, NameGender.Male, NameComponents.FirstNameMiddleNameLastName, separator: ":");

            foreach (var item in names)
            {
                Console.WriteLine(item + ":" + rnd.Next(minAge, maxAge));
            }

            //PersonGenerator.GeneratorSettings settings = new()
            //{
            //    Age = true,
            //    Language = PersonGenerator.Languages.English,
            //    FirstName = true,
            //    MiddleName = true,
            //    LastName = true,
            //    MinAge = 16,
            //    MaxAge = 25
            //};
            //PersonGenerator.PersonGenerator generator = new(settings);

            //var generated = generator.Generate(150);

            //foreach (var item in generated)
            //{
            //    Console.WriteLine($"{item.FirstName} {item.MiddleName} {item.LastName} {item.Age}");
            //}

            Console.ReadKey();
        }
    }
}
