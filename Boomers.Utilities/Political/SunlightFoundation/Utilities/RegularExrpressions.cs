using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Boomers.Political.SunlightFoundation.Utilities
{
   public static  class RegularExrpressions
    {
       //public static Regex MiddleNameIncluded = new Regex(@"(REP\.|HON\.|DEL\.|COM\.|SEN\.)(\s+)?[A-Za-z]+\s+[a-zA-Z]{1,}(\.)?\s+[a-zA-Z]{1,}(\s(JR(\.)?|SR(\.)?))?(\s+[I]+)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
       //public static Regex NoMiddleName = new Regex(@"(REP\.|HON\.|DEL\.|COM\.|SEN\.)(\s+)?[A-Za-z]+\s+[a-zA-Z]{3,}(\s(JR(\.)?|SR(\.)?))?(\s+[I]+)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
       //public static Regex NickName = new Regex(@"(REP\.|HON\.|DEL\.|COM\.|SEN\.)?(\s+)?[A-Za-z]+\s+'[a-zA-Z\.]+'\s+[a-zA-Z\-]+(\s(JR(\.)?|SR(\.)?))?(\s+[I]+)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
       //public static Regex FirstInitialName = new Regex(@"(REP\.|HON\.|DEL\.|COM\.|SEN\.)(\s+)?[A-Za-z]+\s+[a-zA-Z]{1,2}(\.)?\s+[a-zA-Z]+(\s(JR(\.)?|SR(\.)?))?(\s+[I]+)?", RegexOptions.IgnoreCase | RegexOptions.Compiled); 
       public static Regex rgxSuffix = new Regex(@"((JR(\.)?|SR(\.)?))", RegexOptions.IgnoreCase | RegexOptions.Compiled); 
   }
}
