using MapperNet.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapperNet.ConsoleApp
{
    public class PersonMapper : EntityTable<Person>
    {
        public PersonMapper() : base() { }
    }
}
