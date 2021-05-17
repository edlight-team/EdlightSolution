using System;

namespace NameGeneratorTest
{
    class Program
    {
        static void Main()
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

            var generated = generator.Generate(150);

            foreach (var item in generated)
            {
                Console.WriteLine($"{item.FirstName} {item.MiddleName} {item.LastName} {item.Age}");
            }

            Console.ReadKey();
        }
    }
}
