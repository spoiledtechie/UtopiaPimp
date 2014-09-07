using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Mvc.ViewModels
{
   public  class SysObjectViewModel
    {
       public static string SELECT_USER_TABLES_QUERY = "SELECT * FROM sysobjects WHERE type = 'U'";

        public string name;
        public int Id;
        public DateTime crdate;
        public int schema_ver;
        public string type;

        
    }
}
