using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PimpLibrary.Static.Enums;
using PimpLibrary.Utopia.Ce;
using Pimp.UData;


namespace Pimp.UParser
{
    /// <summary>
    /// Summary description for Sql
    /// </summary>
    public static class Sql
    {
        public static int GetCeTypeId(string ceType, Guid userID)
        {

            int uid = UtopiaHelper.Instance.CeTypes.Where(x => x.name == (CeTypeEnum)Enum.Parse(typeof(CeTypeEnum), ceType)).Select(x => x.uid).FirstOrDefault();
            if (uid == 0)
            {
                CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
                CS_Code.Utopia_Kingdom_CE_Type_Pull UKCETPInsert = new CS_Code.Utopia_Kingdom_CE_Type_Pull();
                UKCETPInsert.CE_Type = ceType.ToString();
                UKCETPInsert.Added_By_DateTime = DateTime.UtcNow;
                UKCETPInsert.Added_By_UserID = userID;
                db.Utopia_Kingdom_CE_Type_Pulls.InsertOnSubmit(UKCETPInsert);
                db.SubmitChanges();
                UtopiaHelper.Instance.CeTypes = null;
                return UKCETPInsert.uid;
            }
            return uid;
        }
        public static int GetCeTypeId(CeTypeEnum ceType, Guid userID)
        {
            int uid = UtopiaHelper.Instance.CeTypes.Where(x => x.name == ceType).Select(x => x.uid).FirstOrDefault();
            if (uid == 0)
            {
                CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
                CS_Code.Utopia_Kingdom_CE_Type_Pull UKCETPInsert = new CS_Code.Utopia_Kingdom_CE_Type_Pull();
                UKCETPInsert.CE_Type = ceType.ToString();
                UKCETPInsert.Added_By_DateTime = DateTime.UtcNow;
                UKCETPInsert.Added_By_UserID = userID;
                db.Utopia_Kingdom_CE_Type_Pulls.InsertOnSubmit(UKCETPInsert);
                db.SubmitChanges();

                UtopiaHelper.Instance.CeTypes = null;

                return UKCETPInsert.uid;
            }
            return uid;
        }


    }
}