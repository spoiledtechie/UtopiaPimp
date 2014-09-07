using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Pimp.UData;
using Pimp.UParser;
using PimpLibrary.Utopia.Ops;
using Pimp.Utopia;
using Pimp.Users;



namespace Pimp.UCache
{
    /// <summary>
    /// Summary description for OpCache
    /// </summary>
    public class OpCache
    {
        public static Attack UpdateAttackToCache(CS_Code.Utopia_Province_Data_Captured_Attack attack, OwnedKingdomProvinces cachedKingdom)
        {
            Attack at = new Attack();
            at.Time_To_Return = attack.Time_To_Return;
            at.Province_ID_Attacked = attack.Province_ID_Attacked;
            at.Province_ID_Added = attack.Province_ID_Added;
            at.Mod_Off_Sent = attack.Mod_Off_Sent;
            at.DateTime_Added = attack.DateTime_Added;
            at.Captured_Type_Number = attack.Captured_Type_Number;
            at.Attack_Type_Name = UtopiaHelper.Instance.AttackType.Where(x => x.uid == attack.Attack_Type).Select(x => x.name).FirstOrDefault();

            if (cachedKingdom.Attacks != null)
                cachedKingdom.Attacks.Add(at);
            else
                cachedKingdom.Attacks =Ops.getAttacks(attack.Owner_Kingdom_ID, CS_Code.UtopiaDataContext.Get(), cachedKingdom.KdOpsAttacksTimeLimit);

            HttpRuntime.Cache.Add("KingdomCache" + ((Guid)attack.Owner_Kingdom_ID).ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            return at;
        }
        public static Op UpdateOpToCache(CS_Code.Utopia_Province_Op ops, OwnedKingdomProvinces cachedKingdom)
        {
            Op op = new Op();
            op.Directed_To_Province_ID = ops.Directed_To_Province_ID;
            op.Added_By_Province_ID = ops.Added_By_Province_ID;
            op.OP_Text = ops.OP_Text;
            op.Op_ID = ops.Op_ID;
            op.TimeStamp = ops.TimeStamp;
            op.Expiration_Date = ops.Expiration_Date;
            op.Duration = ops.Duration;
            op.OP_Name = UtopiaHelper.Instance.Ops.Where(x => x.uid == ops.Op_ID).Select(x => x.OP_Name).FirstOrDefault();

            if (cachedKingdom.Effects != null)
                cachedKingdom.Effects.Add(op);
            else
                cachedKingdom.Effects =Ops.getEffects(ops.Owner_Kingdom_ID, CS_Code.UtopiaDataContext.Get(), cachedKingdom.KdOpsAttacksTimeLimit);

            HttpRuntime.Cache.Add("KingdomCache" + ((Guid)ops.Owner_Kingdom_ID).ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            return op;
        }
    }
}