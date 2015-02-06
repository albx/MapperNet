using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapperNet.Mapping
{
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// Get or set the table's name
        /// </summary>
        public string Name { get; set; }
    }
}
