using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Admin.MsSql
{
    /// <summary>
    /// pulling from the sys object tables, these are the values they return.
    /// </summary>
    public class DatabaseInfo
    {
        public string database_name;
        public string database_size;
        public string unallocated_space;
        public string reserved;
        public string data;
        public string index_size;
        public string unused;

    }
}
