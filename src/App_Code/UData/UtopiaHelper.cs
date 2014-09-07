using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PimpLibrary.Utopia.Ops;
using PimpLibrary.Utopia.Kingdom;
using PimpLibrary.Utopia.Players;
using PimpLibrary.UI;
using PimpLibrary.Utopia.Ce;
using PimpLibrary.Static.Enums;
using SupportFramework.Data;
using PimpLibrary.Utopia.Province;

namespace Pimp.UData
{

    /// <summary>
    /// Summary description for UtopiaHelper
    /// </summary>
    public class UtopiaHelper
    {
        static UtopiaHelper instance = new UtopiaHelper();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static UtopiaHelper()
        {
        }

        UtopiaHelper()
        {
        }

        public static UtopiaHelper Instance
        {
            get
            {
                return instance;
            }
        }

        private List<CeType> _ceTypes;
        public List<CeType> CeTypes
        {
            get
            {
                if (_ceTypes == null)
                {
                    _ceTypes = getCeTypes();
                }
                return _ceTypes;
            }
            set { _ceTypes = value; }
        }
        private List<CeType> getCeTypes()
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            List<CeType> ceTypeList = new List<CeType>();
            var ceTypes = (from UPRP in db.Utopia_Kingdom_CE_Type_Pulls
                           select new
                           {
                               name = UPRP.CE_Type,
                               uid = UPRP.uid,
                           }).ToList();

            for (int i = 0; i < ceTypes.Count; i++)
            {
                try
                {
                    CeType type = new CeType();
                    type.name = (CeTypeEnum)Enum.Parse(typeof(CeTypeEnum), ceTypes[i].name);
                    type.uid = ceTypes[i].uid;
                    ceTypeList.Add(type);
                }
                catch (Exception e)
                {
                    Errors.logError(e, ceTypes[i].name); ;
                }
            }

            return ceTypeList;
        }

        private List<AttackType> _attackType;
        public List<AttackType> AttackType
        {
            get
            {
                if (_attackType == null)
                {
                    _attackType = getAttackTypes();
                }
                return _attackType;
            }
            set { _attackType = value; }
        }
        private List<AttackType> getAttackTypes()
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            List<AttackType> attackType = (from UPNP in db.Utopia_Province_Data_Captured_Attack_Pulls
                                           select new AttackType
                                           {
                                               name = UPNP.Attack_Type_Name,
                                               uid = UPNP.uid
                                           }).ToList();
            return attackType;
        }
        private List<KingdomStance> getKingdomStances()
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            List<KingdomStance> kdStance = (from UPRP in db.Utopia_Kingdom_Stance_Pulls
                                            select new KingdomStance
                                            {
                                                name = UPRP.stance,
                                                uid = UPRP.uid,
                                            }).ToList();
            return kdStance;
        }
        private List<KingdomStance> _kingdomStances;
        public List<KingdomStance> KingdomStances
        {
            get
            {
                if (_kingdomStances == null)
                {
                    _kingdomStances = getKingdomStances();
                }
                return _kingdomStances;
            }
            set { _kingdomStances = value; }
        }
        private List<Rank> getRanks()
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            List<Rank> rank = (from UPNP in db.Utopia_Province_Nobility_Pulls
                               select new Rank
                               {
                                   name = UPNP.Nobility,
                                   uid = UPNP.uid,
                                   income = UPNP.Income_Multi,
                                   ome = UPNP.OME_Mulit,
                                   popMulti = UPNP.popMulti,
                                   tpaMulti = UPNP.TPA_Multi,
                                   wpaMulti = UPNP.WPA_Multi

                               }).ToList();
            return rank;
        }
        private List<Rank> _ranks;
        public List<Rank> Ranks
        {
            get
            {
                if (_ranks == null)
                {
                    _ranks = getRanks();
                }
                return _ranks;
            }
            set { _ranks = value; }
        }
        private List<Personality> getPersonalities()
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            List<Personality> personality = (from UPP in db.Utopia_Personality_Pulls
                                             select new Personality
                                             {
                                                 name = UPP.Personality_Name,
                                                 uid = UPP.uid
                                             }).ToList();
            return personality;
        }
        private List<Personality> _personalities;
        public List<Personality> Personalities
        {
            get
            {
                if (_personalities == null)
                {
                    _personalities = getPersonalities();
                }
                return _personalities;
            }
            set { _personalities = value; }
        }

        private List<Op> getOps()
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            List<Op> op = (from UPP in db.Utopia_Province_Ops_Pulls
                           select new Op
                           {
                               OP_Name = UPP.OP_Name,
                               uid = UPP.uid
                           }).ToList();

            return op;
        }
        private List<Op> _ops;
        public List<Op> Ops
        {
            get
            {
                if (_ops == null)
                {
                    _ops = getOps();
                }
                return _ops;
            }
            set { _ops = value; }
        }

        private List<Race> getRaces()
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            
            var raceList = (from UPRP in db.Utopia_Province_Race_Pulls
                               from xx in db.Utopia_Province_Race_Military_Names
                               where xx.Race_ID == UPRP.uid
                               select new Race
                               {
                                   name = UPRP.Race_Name,
                                   uid = UPRP.uid,
                                   eliteName = xx.Elite_Name,
                                   eliteOffMulitplier = (int)xx.Elite_Off_Multiplier,
                                   eliteDefMulitplier = (int)xx.Elite_Def_Multiplier,
                                   soldierOffName = xx.Soldier_Off_Name,
                                   soldierOffMultiplier = (int)xx.Soldier_Off_Multiplier,
                                   soldierDefName = xx.Soldier_Def_Name,
                                   soldierDefMultiplier = (int)xx.Soldier_Def_Multiplier
                               }).ToList();

            //for (int i = 0; i < races.Count; i++)
            //{
            //    try
            //    {
            //        Race race = new Race();
            //        race.Name = (RaceEnum)Enum.Parse(typeof(RaceEnum), races[i].name);
            //        race.eliteDefMulitplier = races[i].eliteDefMulitplier;
            //        race.eliteName = races[i].eliteName;
            //        race.eliteOffMulitplier = races[i].eliteOffMulitplier;
            //        race.soldierDefMultiplier = races[i].soldierDefMultiplier;
            //        race.soldierDefName = races[i].soldierDefName;
            //        race.soldierOffMultiplier = races[i].soldierOffMultiplier;
            //        race.soldierOffName = races[i].soldierOffName;
            //        race.uid = races[i].uid;
            //        raceList.Add(race);
            //    }
            //    catch (Exception e)
            //    {
            //        Errors.logError(e, races[i].name); ;
            //    }
            //}


            return raceList;
        }
        private List<Race> _races;
        public List<Race> Races
        {
            get
            {
                if (_races == null)
                {
                    _races = getRaces();
                }
                return _races;
            }
            set { _races = value; }
        }
        private List<ColumnName> getColumnNames()
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            List<ColumnName> columnNames = (from ucnp in db.Utopia_Column_Name_Pulls
                                            from uccnp in db.Utopia_Column_Catagory_Name_Pulls
                                            where ucnp.Category_ID == uccnp.Category_ID
                                            orderby ucnp.Column_Name ascending
                                            select new ColumnName
                                            {
                                                uid = ucnp.uid,
                                                columnName = ucnp.Column_Name,
                                                categoryID = ucnp.Category_ID,
                                                categoryName = uccnp.Category_Name,
                                                alt = ucnp.Alt
                                            }).ToList();
            return columnNames;
        }
        private List<ColumnName> _columnNames;
        public List<ColumnName> ColumnNames
        {
            get
            {
                if (_columnNames == null)
                {
                    _columnNames = getColumnNames();
                }
                return _columnNames;
            }
            set { _columnNames = value; }
        }
    }
}