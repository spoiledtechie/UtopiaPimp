using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Boomers.Utilities.Guids
{
    /// <summary>
    /// Guid Extensions.
    /// </summary>
    public static class GuidExt
    {
        //private static Regex isGuid = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$");
        /// <summary>
        /// Checks if it is a valid GUID.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>true of false.</returns>
        public static Boolean IsValidGuid(this string guid)
        {
            if (guid != null)
            {
                try
                {
                    new Guid(guid);
                    return true;
                }
                catch { }
            }
            return false;
        }

        public static Guid NewGuidIfEmpty(this Guid uuid)
        {
            ///Original code:
            ///if (guid != Guid.Empty) return guid;
            ///else return Guid.NewGuid();

            ///New code:
            ///return guid.NewGuidIfEmpty();
            return (uuid != Guid.Empty ? uuid : Guid.NewGuid());
        }
        public static string RemoveDashes(this Guid guid)
        {
           return Boomers.Utilities.Text.StringExt.RemoveDashes(guid.ToString());
            }
    }
}
