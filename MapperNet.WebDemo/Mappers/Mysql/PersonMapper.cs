using MapperNet.WebDemo.Models.Mysql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MapperNet.WebDemo.Mappers.Mysql
{
    public class PersonMapper : MySqlMapper<Person>
    {
        public PersonMapper() : base() { }
    }
}