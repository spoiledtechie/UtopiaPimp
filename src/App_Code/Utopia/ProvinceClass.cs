using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Pimp.Utopia
{
    /// <summary>
    /// Summary description for ProvinceClass
    /// </summary>
    public class ProvinceClass
    {
        public int NoteCount;
        public Guid? Added_By_User_ID;
        public int? Army_Out;
        public DateTime? Army_Out_Expires;
        public int? Building_Effectiveness;
        public string CB_Export_Line;
        public DateTime? CB_Requested;
        public Guid? CB_Requested_Province_ID;
        public DateTime? CB_Updated_By_DateTime;
        public Guid? CB_Updated_By_Province_ID;
        public long? Daily_Income;
        public DateTime? Date_Time_User_ID_Linked;
        public decimal? Draft;
        public long? Food;
        public string Formatted_By;
        public string Hit;
        public int? Honor;
        public Guid? Kingdom_ID;
        public int? Kingdom_Island;
        public int? Kingdom_Location;
        public long? Land;
        public DateTime? Last_Login_For_Province;
        public decimal? Mil_Overall_Efficiency;
        public int? Mil_Total_Generals;
        public string Mil_Training;
        public decimal? Mil_Wage;
        public decimal? Military_Current_Def;
        public decimal? Military_Current_Off;
        public decimal? Military_Efficiency_Def;
        public decimal? Military_Efficiency_Off;
        public decimal? Military_Net_Def;
        public decimal? Military_Net_Off;
        public int Monarch_Display;
        public Guid? Monarch_Vote_Province_ID;
        public int? Owner;
        public int? Sub_Monarch;
        public long? Money;
        public long? Networth;
        public int? Nobility_ID;
        public Guid? Owner_Kingdom_ID;
        public Guid? Owner_User_ID;
        public long? Peasents;
        public decimal? Peasents_Non_Percentage;
        public int? Personality_ID;
        public int? Plague;
        public long? Population;
        public int? Prisoners;
        public int? Protected;
        public Guid Province_ID { get; set; }
        public string Province_Name { get; set; }
        public string Province_Notes;
        public int? Race_ID;
        public string Ruler_Name;
        public long? Runes;
        public int? Soldiers;
        public int? Soldiers_Elites;
        public int? Soldiers_Regs_Def;
        public int? Soldiers_Regs_Off;
        public DateTime? SOM_Requested;
        public Guid? SOM_Requested_Province_ID;
        public DateTime? SOM_Updated_By_DateTime;
        public Guid? SOM_Updated_By_Province_ID;
        public DateTime? SOS_Requested;
        public Guid? SOS_Requested_Province_ID;
        public DateTime? Survey_Requested;
        public Guid? Survey_Requested_Province_ID;
        public int? Thieves;
        public int? Thieves_Value_Type;
        public int? Trade_Balance;
        public int uid;
        public DateTime? Updated_By_DateTime;
        public Guid? Updated_By_Province_ID;
        public DateTime? Utopian_Day_Month;
        public int? Utopian_Year;
        public int? War_Horses;
        public int? Wizards;
        public int? Wizards_Value_Type;
        //Used for Target Finder
        public int OnlineCurrently;
        public DateTime FirstListed;
        public int? Kingdom_Acres;
        public string Kingdom_Name;
        public int? Kingdom_Networth;
        public int? Kingdom_Province_Count;
        public int? Stance;
        public int? War;
        public List<CS_Code.Utopia_Province_Data_Captured_CB> CB;
        public List<CS_Code.Utopia_Province_Data_Captured_Type_Military> SOM;
        public List<CS_Code.Utopia_Province_Data_Captured_Science> SOS;
        public List<CS_Code.Utopia_Province_Data_Captured_Survey> Survey;
    }
}