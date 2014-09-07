using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Pimp.UParser;
using PimpLibrary.Utopia.Ce;

namespace Pimp.UData
{
    /// <summary>
    /// Summary description for Ce
    /// </summary>
    public class Ce
    {
        public static List<BuildCe> BuildCE(int month, int year, string kingdomIL, string CEID, Guid kingdomID)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            switch (kingdomIL)
            {
                case "All":
                    return (from xx in db.Utopia_Kingdom_CEs
                            from yy in db.Utopia_Kingdom_CE_Type_Pulls
                            from zz in db.Utopia_Kingdom_Infos
                            from aa in db.Utopia_Kingdom_Infos
                            where aa.Kingdom_ID == kingdomID
                            where xx.Kingdom_ID == zz.Kingdom_ID
                            where xx.CE_Type == yy.uid
                            where xx.Owner_Kingdom_ID == kingdomID
                            where xx.Kingdom_ID == new Guid(CEID)
                            where xx.Utopia_Month == month
                            where xx.Utopia_Year == year
                            orderby xx.Utopia_Date_Day
                            orderby xx.Utopia_Month
                            orderby xx.Utopia_Year
                            select new BuildCe
                            {
                                RawLine = xx.Raw_Line,
                                ceType = yy.CE_Type,
                                sourIL = xx.Source_Kingdom_Island + ":" + xx.Source_Kingdom_Location,
                                kingIL = zz.Kingdom_Island + ":" + zz.Kingdom_Location,
                                targIL = xx.Target_Kingdom_Island + ":" + xx.Target_Kingdom_Location,
                                size = xx.value,
                                ownerKingdom = aa.Kingdom_Island + ":" + aa.Kingdom_Location,
                                sProvinceName = xx.Source_Province_Name,
                                tProvinceName = xx.Target_Province_Name
                            }).ToList();
                default:
                    return (from xx in db.Utopia_Kingdom_CEs
                            from yy in db.Utopia_Kingdom_CE_Type_Pulls
                            from zz in db.Utopia_Kingdom_Infos
                            from aa in db.Utopia_Kingdom_Infos
                            where aa.Kingdom_ID == kingdomID
                            where xx.Kingdom_ID == zz.Kingdom_ID
                            where xx.CE_Type == yy.uid
                            where xx.Owner_Kingdom_ID == kingdomID
                            where xx.Kingdom_ID == new Guid(CEID)
                            where xx.Utopia_Month == month
                            where xx.Utopia_Year == year
                            where xx.Source_Kingdom_Island == Convert.ToInt32(URegEx.rgxNumber.Matches(kingdomIL)[0].Value)
                            where xx.Source_Kingdom_Location == Convert.ToInt32(URegEx.rgxNumber.Matches(kingdomIL)[1].Value)
                            orderby xx.Utopia_Date_Day
                            orderby xx.Utopia_Month
                            orderby xx.Utopia_Year
                            select new BuildCe
                            {
                                RawLine = xx.Raw_Line,
                                ceType = yy.CE_Type,
                                sourIL = xx.Source_Kingdom_Island + ":" + xx.Source_Kingdom_Location,
                                kingIL = zz.Kingdom_Island + ":" + zz.Kingdom_Location,
                                targIL = xx.Target_Kingdom_Island + ":" + xx.Target_Kingdom_Location,
                                size = xx.value,
                                ownerKingdom = aa.Kingdom_Island + ":" + aa.Kingdom_Location,
                                sProvinceName = xx.Source_Province_Name,
                                tProvinceName = xx.Target_Province_Name
                            }).ToList();
            }
        }


        /// <summary>
        /// get ce info.
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="island"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public static List<BuildCe> GetCEInfo(Guid ownerKingdomID, int island, int location)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            return (from xx in db.Utopia_Kingdom_CEs
                    from yy in db.Utopia_Kingdom_CE_Type_Pulls
                    from zz in db.Utopia_Kingdom_Infos
                    from aa in db.Utopia_Province_Data_Captured_Gens
                    from bb in db.Utopia_Province_Data_Captured_Gens
                    where aa.Owner_Kingdom_ID == ownerKingdomID
                    where bb.Owner_Kingdom_ID == ownerKingdomID
                    where xx.Source_Province_Name == aa.Province_Name
                    where xx.Target_Province_Name == bb.Province_Name
                    where xx.CE_Type == yy.uid
                    where xx.Owner_Kingdom_ID == ownerKingdomID
                    where xx.Kingdom_ID == zz.Kingdom_ID
                    where (xx.Source_Kingdom_Island == island & xx.Source_Kingdom_Location == location) | (xx.Target_Kingdom_Island == island & xx.Target_Kingdom_Location == location)
                    select new BuildCe
                    {
                        RawLine = xx.Raw_Line,
                        ceType = yy.CE_Type,
                        sourIL = xx.Source_Kingdom_Island + ":" + xx.Source_Kingdom_Location,
                        targIL = xx.Target_Kingdom_Island + ":" + xx.Target_Kingdom_Location,
                        kingIL = zz.Kingdom_Island + ":" + zz.Kingdom_Location,
                        size = xx.value,
                        sProvinceName = xx.Source_Province_Name,
                        tProvinceName = xx.Target_Province_Name,
                        tProvinceNameGuid = bb.Province_ID,
                        sProvinceNameGuid = aa.Province_ID
                    }).ToList();
        }

        /// <summary>
        /// Gets the non cached CE for the kingdom of the past many hours
        /// </summary>
        /// <param name="historyInHours"></param>
        /// <param name="kingdomId"></param>
        /// <param name="ownerKingdomId"></param>
        /// <returns></returns>
        public static List<CS_Code.Utopia_Kingdom_CE> getCeForKingdom(Guid kingdomId, Guid ownerKingdomId)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            return (from xx in db.Utopia_Kingdom_CEs
                    where xx.Kingdom_ID == kingdomId
                    where xx.Owner_Kingdom_ID == ownerKingdomId
                    select xx).ToList();
        }
        /// <summary>
        /// gets the CE for kingdom depending on month and year.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="kingdomId"></param>
        /// <param name="ownerKingdomId"></param>
        /// <returns></returns>
        public static List<CS_Code.Utopia_Kingdom_CE> getCeForKingdom(int year, int month, Guid kingdomId, Guid ownerKingdomId)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            return (from xx in db.Utopia_Kingdom_CEs
                    where xx.Kingdom_ID == kingdomId
                    where xx.Owner_Kingdom_ID == ownerKingdomId
                    where xx.Utopia_Year == year
                    where xx.Utopia_Month == month
                    select xx).ToList();
        }
    }
}