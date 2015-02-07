using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapperNet.ConsoleApp
{
    class Program
    {
        private static void AddCars()
        {
            var mapper = new CarMapper();

            var car1 = new Car
            {
                Cilindrata = "1.4",
                ConsumoMedio = "20",
                Modello = "Ford Fiesta",
                Porte = 5
            };
            mapper.Insert(car1);

            var car2 = new Car
            {
                Cilindrata = "1.6",
                ConsumoMedio = "14",
                Modello = "Mini",
                Porte = 3
            };
            mapper.Insert(car2);

            var car3 = new Car
            {
                Cilindrata = "3.0",
                ConsumoMedio = "3",
                Modello = "Ferrari",
                Porte = 3
            };
            mapper.Insert(car3);
        }

        private static void AddPeople()
        {
            var mapper = new PersonMapper();

            for (var i = 0; i < 5; i++)
            {
                var birthYear = DateTime.Now.Year - ((i + 1) * 10);

                var person = new Person
                {
                    FirstName = string.Format("Nome {0}", i),
                    LastName = string.Format("Cognome {0}", i),
                    Age = (i + 1)*10,
                    DateOfBirth = new DateTime(birthYear, DateTime.Now.Month, DateTime.Now.Day)
                };
                mapper.Insert(person);
            }
        }

        static void Main(string[] args)
        {
            try
            {
                System.Console.WriteLine("1...");

                var personMapper = new PersonMapper();

                //AddPeople();

                var people = personMapper.Query();
                foreach (var person in people)
                {
                    System.Console.WriteLine(person.ToString());
                }

                System.Console.WriteLine("2...");

                var carMapper = new CarMapper();

                //AddCars();

                foreach (var car in carMapper.Query())
                {
                    Console.WriteLine(car.ToString());
                }

                System.Console.WriteLine("Fine...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("{0} - {1}", ex.Message, ex.StackTrace));
            }

            System.Console.ReadLine();
        }
    }
}
