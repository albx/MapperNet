using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapperNet.Mapping;

namespace MapperNet.ConsoleApp
{
    [Table]
    public class Person
    {
        [Column(IsPrimaryKey=true)]
        public int Id { get; set; }

        [Column]
        public string FirstName { get; set; }

        [Column]
        public string LastName { get; set; }

        [Column]
        public DateTime DateOfBirth { get; set; }

        [Column]
        public int Age { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}] {1} {2}, nato il {3}, età {4} anni", Id, FirstName, LastName, DateOfBirth.ToString("dd/MM/yyyy"), Age);
        }
    }
}
