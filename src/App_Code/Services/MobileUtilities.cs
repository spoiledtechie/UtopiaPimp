using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;

using System.Web.Security;

using Pimp.UParser;
using Pimp.UCache;
using Pimp.UData;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MobileUtilities" in code, svc and config file together.
public class MobileUtilities : IMobileUtilities
{
    //public string CheckUserNamePasswordProvinceName(string userName, string password, string provinceName)
    //{
    //    if (userName == null || userName == string.Empty)
    //        return "No Username filled out: " + userName;

    //    if (password == null || password == string.Empty)
    //        return "No password filled out";
    //    if (provinceName == null || provinceName == string.Empty)
    //        return "No province name filled out";

    //    if (!Membership.ValidateUser(userName, password))
    //        return "Bad Username/Password Combination.";

    //    PimpUserWrapper  pimpUser = new PimpUserWrapper ();

    //    var currentUser = pimpUser.PimpUser.getUserObject(SupportFramework.Users.Memberships.getUserID(userName));
    //    var province = currentUser.PimpUser.ProvincesOwned.Where(x => x.Province_Name == provinceName).FirstOrDefault();
    //    if (province != null && province.Province_ID != null && province.Province_ID != new Guid())
    //        return "true";

    //    return "Province Name Was Not Found.";
    //}
    //public string SubmitMobileUtopiaData(string userName, string password, string provinceName, string data)
    //{
    //    if (!Membership.ValidateUser(userName, password))
    //        return "UPDATEME: Bad Username/Password Combination.";

    //    PimpUserWrapper  pimpUser = new PimpUserWrapper ();
    //    var currentUser = pimpUser.PimpUser.getUserObject(SupportFramework.Users.Memberships.getUserID(userName));
    //    var province = currentUser.PimpUser.ProvincesOwned.Where(x => x.Province_Name == provinceName).FirstOrDefault();
    //    if (province != null && province.Province_ID != null && province.Province_ID != new Guid())
    //    {
    //        data = Regex.Replace(System.Web.HttpUtility.HtmlDecode(data), "<(.|\n)*?>", "");
    //        UtopiaParser.UtopiaParsing(data, "MobileApp", "2", provinceName, string.Empty, currentUser, KingdomCache.getKingdom(currentUser.PimpUser.StartingKingdom));
    //        return "true";
    //    }
    //    else
    //        return "UPDATEME: Province Name Was Not Found.";

    //}
}
