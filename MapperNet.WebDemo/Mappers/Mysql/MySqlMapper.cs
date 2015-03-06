using MapperNet.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MapperNet.WebDemo.Mappers.Mysql
{
    public class MySqlMapper<TModel> : EntityTable<TModel> where TModel : class
    {
        public MySqlMapper() : base("LocalMySqlServer") { }
    }
}