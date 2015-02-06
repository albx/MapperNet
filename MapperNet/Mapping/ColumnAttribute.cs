using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapperNet.Mapping
{
    public class ColumnAttribute : Attribute
    {
        /// <summary>
        /// Get or set the column's name in the database
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or set wheter the field is a primary key for the table
        /// </summary>
        public bool IsPrimaryKey { get; set; }
    }
}
