using MapperNet.Mapping;
using MapperNet.WebDemo.Models.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MapperNet.WebDemo.Mappers.SqlServer
{
    public class PersonMapper : EntityTable<Person>
    {
        public PersonMapper() : base() { }
    }
}