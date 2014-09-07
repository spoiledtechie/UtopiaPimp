using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PimpLibrary.Utopia.Ops;

namespace Pimp.UData
{
    /// <summary>
    /// Summary description for Ops
    /// </summary>
    public class Ops
    {
        /// <summary>
        /// gets effects on the provinces within time given
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="db"></param>
        /// <param name="KdOpsAttacksTimeLimit"></param>
        /// <returns></returns>
        public static List<Op> getEffects(Guid ownerKingdomID, CS_Code.UtopiaDataContext db, int? KdOpsAttacksTimeLimit)
        {
            return (from xx in db.Utopia_Province_Ops
                    from yy in db.Utopia_Province_Ops_Pulls
                    where xx.Op_ID == yy.uid
                    where xx.Owner_Kingdom_ID == ownerKingdomID
                    where xx.TimeStamp >= DateTime.Now.AddHours(KdOpsAttacksTimeLimit.GetValueOrDefault(-48))
                    where xx.negated == null | xx.negated == 0
                    select new Op
                    {
                        Op_ID = xx.Op_ID,
                        uid = xx.uid,
                        Added_By_Province_ID = xx.Added_By_Province_ID,
                        Expiration_Date = xx.Expiration_Date,
                        OP_Text = xx.OP_Text,
                        Directed_To_Province_ID = xx.Directed_To_Province_ID,
                        OP_Name = yy.OP_Name,
                        TimeStamp = xx.TimeStamp
                    }).ToList();
        }

        /// <summary>
        /// gets all attacks within period of time given
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="db"></param>
        /// <param name="KdOpsAttacksTimeLimit"></param>
        /// <returns></returns>
        public  static List<Attack> getAttacks(Guid ownerKingdomID, CS_Code.UtopiaDataContext db, int? KdOpsAttacksTimeLimit)
        {
            return (from xx in db.Utopia_Province_Data_Captured_Attacks
                    from yy in db.Utopia_Province_Data_Captured_Attack_Pulls
                    where xx.Attack_Type == yy.uid
                    where xx.Owner_Kingdom_ID == ownerKingdomID
                    where xx.DateTime_Added >= DateTime.UtcNow.AddHours(KdOpsAttacksTimeLimit.GetValueOrDefault(-24))
                    select new Attack
                    {
                        Attack_Type_Name = yy.Attack_Type_Name,
                        Mod_Off_Sent = xx.Mod_Off_Sent,
                        Captured_Type_Number = xx.Captured_Type_Number,
                        Province_ID_Added = xx.Province_ID_Added,
                        Province_ID_Attacked = xx.Province_ID_Attacked,
                        Time_To_Return = xx.Time_To_Return,
                        DateTime_Added = xx.DateTime_Added
                    }).ToList();
        }
    }
}