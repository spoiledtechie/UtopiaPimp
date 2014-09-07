using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

using Pimp.UParser;
using Pimp.Utopia;
using SupportFramework.Data;
using Pimp.Users;

namespace Pimp.UIBuilder
{

    /// <summary>
    /// Helps split up the work of the KDPage web site builder.
    /// </summary>
    public class KdPageHelper
    {
        public static string PLEASE_MAKE_ICON = " (SOMEONE MAKE ME AN ICON PLEASE!)";

        public static void displayPopAcreColumn(StringBuilder sb, bool cbCheck, ProvinceClass item, CS_Code.Utopia_Province_Data_Captured_CB cb)
        {
            try
            {
                switch (cbCheck)
                {
                    case true:
                        if (item.Land.GetValueOrDefault() == 0)
                            sb.Append("-");
                        else
                            sb.Append(((decimal)cb.Population.GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N1"));
                        break;
                    default:
                        sb.Append("-");
                        break;
                }
            }
            catch (Exception e)
            {

                Errors.logError(e);
                sb.Append("-");
            }
        }

        /// <summary>
        /// displays the NW/Acre Column
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="item"></param>
        public static void displayNwPerAcreColumn(StringBuilder sb, ProvinceClass item)
        {
            try
            {

                if (item.Networth.HasValue && item.Land.HasValue && item.Land > 0)
                    sb.Append(((decimal)item.Networth.GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N1"));
                else
                    sb.Append("-");
            }
            catch (Exception e)
            {
                HttpContext.Current.Session["SubmittedData"] = item.Land.GetValueOrDefault();
                Errors.logError(e);
                sb.Append("-");
            }
        }
    }
}