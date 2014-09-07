using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Pimp.Utopia;
using Pimp.Users;
using Pimp.UCache;
using Pimp.UData;
using Pimp.UIBuilder;
using Pimp.UParser;
using PimpLibrary.UI;
using System.Text;
using PimpLibrary.Static.Enums;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for Column
/// </summary>
public class Column
{
    /// <summary>
    /// gets the column sets for the kingdom or user
    /// </summary>
    /// <param name="userKingdomID"></param>
    /// <param name="db"></param>
    /// <returns></returns>
    public static List<ColumnSet> getColumnSets(Guid userKingdomID, CS_Code.UtopiaDataContext db)
    {
        return (from xx in db.Utopia_Column_Names
                from yy in db.Utopia_Column_Data_Types
                where xx.Data_Type_ID == yy.uid
                where xx.User_ID == userKingdomID
                select new ColumnSet
                {
                    setUid = yy.uid,
                    setName = yy.Column_Data_Type_Name,
                    columnIDs = xx.Column_IDs,
                    columnNameUid = xx.uid,
                    columnTypeID = (int)xx.Data_Type_ID.GetValueOrDefault(0)
                }).ToList();
    }

    public static string ColumnListForKingdom(Guid userKingdomID, int setUid, PimpUserWrapper  currentUser)
    {
                List<ColumnSet> getMyColumnsSets = currentUser.PimpUser.UserColumns;
        if (setUid != 0)
        {
            var query = getMyColumnsSets.Where(x => x.setUid == setUid).FirstOrDefault().columnIDs;

            var getCatagoryTypes = (from xx in UtopiaHelper.Instance.ColumnNames
                                    select new { xx.categoryID, xx.categoryName }).Distinct();

            StringBuilder stb = new StringBuilder();
            stb.Append("<ul class=\"ulColumnChooser\">");
            foreach (var item in getCatagoryTypes)
            {
                stb.Append("<li class=\"liColumnChooserHeader\">");
                stb.Append(item.categoryName);
                stb.Append("</li>");
            }
            stb.Append("<li class=\"liColumnChooserHeader\">Column Name</li>");
            stb.Append("</ul>");

            stb.Append("<ul class=\"ulColumnChooser\">");
            foreach (var item in getCatagoryTypes)
            {
                stb.Append("<li class=\"liColumnChooser\">");
                var getColumnNames =UtopiaHelper.Instance.ColumnNames.Where(x => x.categoryID == item.categoryID);

                stb.Append("<ul>");
                foreach (var column in getColumnNames)
                {
                    stb.Append("<li>");
                    int dirtyBit = 0;
                    try
                    {
                        for (int i = 0; i < URegEx.rgxNumber.Matches(query).Count; i++)
                            if (URegEx.rgxNumber.Matches(query)[i].Value == column.uid.ToString())
                                dirtyBit = 1;
                    }
                    catch { }
                    switch (dirtyBit)
                    {
                        case 0:
                            stb.Append("<a href='#' onclick=\"SetColumn(this);return false;\" name=\"" + column.uid + "\" title=\"" + column.alt + "\">" + column.columnName + "</a>");
                            break;
                        case 1:
                            stb.Append("<a href='#' class=\"ColumnOn\" onclick=\"SetColumn(this);return false;\" name=\"" + column.uid + "\" title=\"" + column.alt + "\">" + column.columnName + "</a>");
                            break;
                    }
                    stb.Append("</li>");
                }

                stb.Append("</ul>");
                stb.Append("</li>");
            }
            stb.Append("<li class=\"liColumnChooser\" id=\"RealColumns\">");
            stb.Append(Pimp.UIBuilder.Columns.CreateDT(userKingdomID, setUid, query));
            stb.Append("</li>");

            stb.Append("</ul>");
            return stb.ToString();
        }
        return "";
    }
    public static string ColumnList(Guid userKingdomID, int setUid, PimpUserWrapper  currentUser)
    {
                List<ColumnSet> getMyColumnsSets = currentUser.PimpUser.UserColumns;
        if (setUid != 0)
        {
            var query = getMyColumnsSets.Where(x => x.setUid == setUid).FirstOrDefault().columnIDs;

            var getCatagoryTypes = (from xx in UtopiaHelper.Instance.ColumnNames
                                    select new { xx.categoryID, xx.categoryName }).Distinct();

            StringBuilder stb = new StringBuilder();
            stb.Append("<ul class=\"ulColumnChooser\">");
            foreach (var item in getCatagoryTypes)
            {
                stb.Append("<li class=\"liColumnChooserHeader\">");
                stb.Append(item.categoryName);
                stb.Append("</li>");
            }
            stb.Append("<li class=\"liColumnChooserHeader\">Column Name</li>");
            stb.Append("</ul>");

            stb.Append("<ul class=\"ulColumnChooser\">");
            foreach (var item in getCatagoryTypes)
            {
                stb.Append("<li class=\"liColumnChooser\">");
                var getColumnNames =UtopiaHelper.Instance.ColumnNames.Where(x => x.categoryID == item.categoryID);

                stb.Append("<ul>");
                foreach (var column in getColumnNames)
                {
                    stb.Append("<li>");
                    int dirtyBit = 0;
                    try
                    {
                        for (int i = 0; i < URegEx.rgxNumber.Matches(query).Count; i++)
                            if (URegEx.rgxNumber.Matches(query)[i].Value == column.uid.ToString())
                                dirtyBit = 1;
                    }
                    catch { }
                    switch (dirtyBit)
                    {
                        case 0:
                            stb.Append("<a href='#' onclick=\"SetColumn(this);return false;\" name=\"" + column.uid + "\" title=\"" + column.alt + "\">" + column.columnName + "</a>");
                            break;
                        case 1:
                            stb.Append("<a href='#' class=\"ColumnOn\" onclick=\"SetColumn(this);return false;\" name=\"" + column.uid + "\" title=\"" + column.alt + "\">" + column.columnName + "</a>");
                            break;
                    }
                    stb.Append("</li>");
                }

                stb.Append("</ul>");
                stb.Append("</li>");
            }
            stb.Append("<li class=\"liColumnChooser\" id=\"RealColumns\">");
            stb.Append(Pimp.UIBuilder.Columns.CreateDT(userKingdomID, setUid, query));
            stb.Append("</li>");

            stb.Append("</ul>");
            return stb.ToString();
        }
        return "";
    }
    public static string[] CreateSetsListForKingdom(bool ownerKingdom, PimpUserWrapper  currentUser, OwnedKingdomProvinces cachedKingdom)
    {
        var getMyColumnsSets = cachedKingdom.KingdomColumnSets;
        StringBuilder sb = new StringBuilder();
        sb.Append("<ul class=\"ulMyColumnSets\"><li>Your Kingdom's Sets:</li>");
        switch (ownerKingdom)
        {
            case true:
                for (int i = 0; i < getMyColumnsSets.Count; i++)
                {
                    sb.Append("<li>");
                    switch (i)
                    {
                        case 0:
                            sb.Append("<a class=\"ColumnOn\" id=\"aColumnKingdomSet\" onclick=\"ChangeKingdomSet(this);return false;\" name=\"" + getMyColumnsSets[i].setUid + "\" href=\"#\">" + getMyColumnsSets[i].setName + "</a>");
                            break;
                        default:
                            sb.Append("<a onclick=\"ChangeKingdomSet(this);return false;\" name=\"" + getMyColumnsSets[i].setUid + "\" href=\"#\">" + getMyColumnsSets[i].setName + "</a>");
                            break;
                    }
                    sb.Append("</li>");
                }
                break;
            default:
                for (int i = 0; i < getMyColumnsSets.Count; i++)
                {
                    sb.Append("<li>");
                    sb.Append("<a onclick=\"ChangeKingdomSet(this);return false;\" name=\"" + getMyColumnsSets[i].setUid + "\" href=\"#\">" + getMyColumnsSets[i].setName + "</a>");
                    sb.Append("</li>");
                }
                break;
        }
        if (currentUser.PimpUser.MonarchType != MonarchType.none && currentUser.PimpUser.MonarchType != MonarchType.kdMonarch)
        {
            sb.Append("<li id=\"AddKingdomSet\"><input id=\"btnAddKingdomSet\" type=\"button\" value=\"Add Set\" onclick=\"AddKingdomSet(this);return false;\" /></li>");
            sb.Append("<li id=\"DeleteKingdomSet\"><input id=\"btnDeleteKingdomSet\" type=\"button\" value=\"Delete Set\" onclick=\"DeleteKingdomSet(this);return false;\" /></li>");

            switch (getMyColumnsSets.Count)
            {
                case 0:
                    sb.Append("<li> As 'Kingdom Owner', you are allowed to decide default column sets for your kingdom.</li>");
                    break;
            }
        }
        sb.Append("<li id=\"AddMyKingdomSet\"><input id=\"btnMyKingdomSet\" type=\"button\" value=\"Add to My Sets\" onclick=\"AddToMySet(this);return false;\" /></li>");

        sb.Append("</ul>");

        string[] list = new string[2];
        list[0] = sb.ToString();
        switch (getMyColumnsSets.Count)
        {
            case 0:
                list[1] = "0";
                break;
            default:
                list[1] = getMyColumnsSets.FirstOrDefault().setUid.ToString();
                break;
        }

        return list;
    }
    public static string[] CreateSetsList(bool ownerKingdom, PimpUserWrapper  currentUser, OwnedKingdomProvinces cachedKingdom)
    {
        List<ColumnSet> getMyColumnsSets = currentUser.PimpUser.UserColumns;
        StringBuilder sb = new StringBuilder();
        sb.Append("<ul class=\"ulMyColumnSets\"><li>My Sets:</li>");
        switch (ownerKingdom)
        {
            case false:
                for (int i = 0; i < getMyColumnsSets.Count; i++)
                {
                    sb.Append("<li>");
                    switch (i)
                    {
                        case 0:
                            sb.Append("<a class=\"ColumnSetOn\" id=\"aColumnSet\" onclick=\"ChangeSet(this);return false;\" name=\"" + getMyColumnsSets[i].setUid + "\" href=\"#\">" + getMyColumnsSets[i].setName + "</a>");
                            break;
                        default:
                            sb.Append("<a onclick=\"ChangeSet(this);return false;\" name=\"" + getMyColumnsSets[i].setUid + "\" href=\"#\">" + getMyColumnsSets[i].setName + "</a>");
                            break;
                    }
                    sb.Append("</li>");
                }
                break;
            default:
                for (int i = 0; i < getMyColumnsSets.Count; i++)
                {
                    sb.Append("<li>");
                    sb.Append("<a onclick=\"ChangeSet(this);return false;\" name=\"" + getMyColumnsSets[i].setUid + "\" href=\"#\">" + getMyColumnsSets[i].setName + "</a>");
                    sb.Append("</li>");
                }
                break;
        }
        sb.Append("<li id=\"AddSet\"><input id=\"btnAddSet\" type=\"button\" value=\"Add Set\" onclick=\"AddSet(this);return false;\" /></li>");
        sb.Append("<li id=\"DeleteSet\"><input id=\"btnDeleteSet\" type=\"button\" value=\"Delete Set\" onclick=\"DeleteSet(this);return false;\" /></li>");
        if (currentUser.PimpUser.MonarchType != MonarchType.none && currentUser.PimpUser.MonarchType != MonarchType.kdMonarch)
            sb.Append("<li id=\"AddToKingdomSet\"><input id=\"btnToKingdomSet\" type=\"button\" value=\"Add to Kingdoms Sets\" onclick=\"AddToTheKingdomSets(this);return false;\" />As Monarch/Sub you are allowed to add your sets to your kingdoms sets.</li>");

        sb.Append("</ul>");

        switch (getMyColumnsSets.Count)
        {
            case 0:
                sb.Append("<div><ol>");
                sb.Append("<b>Before You Begin: PLEASE READ</b><br />");
                sb.Append("<li>UP 2.1 has upgraded the way to choose column sets.  In order to do it correctly, please follow the instructions below. UP Columns are Dynamic.</li>");
                sb.Append("<li>Add a ‘Column Set’ by going to ‘My Sets’.  Click on ‘Add Set’ and name the set. Once named and added.</li>");
                sb.Append("<li>Select from the boxes under General, Planner, Resources, Military, Buildings or Science.</li>");
                sb.Append("<li>Click on the boxes to select a column.</li>");
                sb.Append("<li>Selected column values will appear colored and in a list under Colum name.</li>");
                sb.Append("<li>Once done, you will need to REFRESH your browser. You will then have Columns.</li>");
                sb.Append("</ol></div>");
                break;
        }
        string[] list = new string[2];
        list[0] = sb.ToString();
        switch (getMyColumnsSets.Count)
        {
            case 0:
                list[1] = "0";
                break;
            default:
                list[1] = getMyColumnsSets.FirstOrDefault().setUid.ToString();
                break;
        }
        list[0] = CreateSetsListForKingdom(ownerKingdom, currentUser, cachedKingdom)[0] + list[0];
        return list;
    }
    public static string DeleteSetForUser(Guid kingdomID, int setID, Guid userId, OwnedKingdomProvinces cachedKingdom)
    {
       
           CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
           var getDataType = (from xx in db.Utopia_Column_Data_Types
                              where xx.uid == setID
                              where xx.User_ID == userId
                              select xx);
           db.Utopia_Column_Data_Types.DeleteAllOnSubmit(getDataType);

           var getUserInfo = (from xx in db.Utopia_Column_Names
                              where xx.User_ID == userId
                              where xx.Data_Type_ID == setID
                              select xx);
           db.Utopia_Column_Names.DeleteAllOnSubmit(getUserInfo);
           db.SubmitChanges();
    

        PimpUserWrapper  pimpUser = new PimpUserWrapper ();
        pimpUser.deleteColumnSetsForUser(setID);
        return Columns.CreateColumnSetsForDisplay(kingdomID, null, null, pimpUser, cachedKingdom);
    }
    public static string DeleteSetForKingdom(Guid kingdomID, int setID, Guid ownerKingdomID, PimpUserWrapper  currentUser, OwnedKingdomProvinces cachedKingdom)
    {
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        var checkCount = (from xx in db.Utopia_Column_Names
                          where xx.Data_Type_ID == setID
                          select xx.uid).Count();
        if (checkCount == 1)
        {
            var getDataType = (from xx in db.Utopia_Column_Data_Types
                               where xx.uid == setID
                               where xx.User_ID == kingdomID
                               select xx);
            db.Utopia_Column_Data_Types.DeleteAllOnSubmit(getDataType);
        }
        var getUserInfo = (from xx in db.Utopia_Column_Names
                           where xx.User_ID == kingdomID
                           where xx.Data_Type_ID == setID
                           select xx);
        db.Utopia_Column_Names.DeleteAllOnSubmit(getUserInfo);
        db.SubmitChanges();
        KingdomCache.updateColumnSetsForKingdom(kingdomID, cachedKingdom);
        return Columns.CreateColumnSetsForDisplay(kingdomID, null, null, currentUser, cachedKingdom);
    }
    public static string AddToKingdomColumnSets(Guid kingdomID, int setID, Guid ownerKingdomID, PimpUserWrapper  currentUser, OwnedKingdomProvinces cachedKingdom)
    {
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        var userSetColumns = currentUser.PimpUser.UserColumns.Where(x => x.setUid == setID).FirstOrDefault().columnIDs;

        CS_Code.Utopia_Column_Name ucdt = new CS_Code.Utopia_Column_Name();
        ucdt.Column_IDs = userSetColumns;
        ucdt.Data_Type_ID = setID;
        ucdt.DateTime_Added = DateTime.UtcNow;
        ucdt.User_ID = ownerKingdomID;
        db.Utopia_Column_Names.InsertOnSubmit(ucdt);
        db.SubmitChanges();
        KingdomCache.updateColumnSetsForKingdom(currentUser.PimpUser.StartingKingdom, cachedKingdom);
        return Columns.CreateColumnSetsForDisplay(kingdomID, setID, 1, currentUser, cachedKingdom);
    }
    public static string AddToMyColumnSets(Guid kingdomID, int setID, PimpUserWrapper  currentUser, OwnedKingdomProvinces cachedKingdom)
    {
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        var kingdomSetColumns = KingdomCache.getColumnSetsForKingdom(kingdomID, cachedKingdom).KingdomColumnSets.Where(x => x.setUid == setID).FirstOrDefault();
        if (kingdomSetColumns != null)
        {
          
                CS_Code.Utopia_Column_Name ucdt = new CS_Code.Utopia_Column_Name();
                ucdt.Column_IDs = kingdomSetColumns.columnIDs;
                ucdt.Data_Type_ID = setID;
                ucdt.DateTime_Added = DateTime.UtcNow;
                ucdt.User_ID = currentUser.PimpUser.UserID;
                db.Utopia_Column_Names.InsertOnSubmit(ucdt);
                db.SubmitChanges();
         

            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            pimpUser.updateColumnSetsForUser(kingdomSetColumns);
        }
        return Columns.CreateColumnSetsForDisplay(kingdomID, setID, 0, currentUser, cachedKingdom);
    }
    public static string AddSetNameToKingdom(Guid kingdomID, string setName, PimpUserWrapper  currentUser, OwnedKingdomProvinces cachedKingdom)
    {
        HttpContext.Current.Session["SubmittedData"] = setName;

        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        CS_Code.Utopia_Column_Data_Type ucdt = new CS_Code.Utopia_Column_Data_Type();
        ucdt.Column_Data_Type_Name = setName;
        ucdt.DateTime_Added = DateTime.UtcNow;
        ucdt.User_ID = currentUser.PimpUser.StartingKingdom;
        db.Utopia_Column_Data_Types.InsertOnSubmit(ucdt);
        db.SubmitChanges();

        CS_Code.Utopia_Column_Name tec = new CS_Code.Utopia_Column_Name();
        tec.DateTime_Added = DateTime.UtcNow;
        tec.User_ID = currentUser.PimpUser.StartingKingdom;
        tec.Column_IDs = string.Empty;
        tec.Data_Type_ID = ucdt.uid;
        db.Utopia_Column_Names.InsertOnSubmit(tec);
        db.SubmitChanges();
        KingdomCache.updateColumnSetsForKingdom(currentUser.PimpUser.StartingKingdom, cachedKingdom);

        return Columns.CreateColumnSetsForDisplay(kingdomID, ucdt.uid, 1, currentUser, cachedKingdom);
    }
    public static string AddSetNameToUser(Guid kingdomID, string setName, Guid ownerKingdomID, Guid userId, OwnedKingdomProvinces cachedKingdom)
    {
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        CS_Code.Utopia_Column_Data_Type ucdt = new CS_Code.Utopia_Column_Data_Type();
        ucdt.Column_Data_Type_Name = setName;
        ucdt.DateTime_Added = DateTime.UtcNow;
        ucdt.User_ID = userId;
        db.Utopia_Column_Data_Types.InsertOnSubmit(ucdt);
        db.SubmitChanges();

        CS_Code.Utopia_Column_Name tec = new CS_Code.Utopia_Column_Name();
        tec.DateTime_Added = DateTime.UtcNow;
        tec.User_ID = userId;
        tec.Column_IDs = string.Empty;
        tec.Data_Type_ID = ucdt.uid;
        db.Utopia_Column_Names.InsertOnSubmit(tec);
        db.SubmitChanges();

        ColumnSet set = new ColumnSet();
        set.setName = setName;
        set.setUid = (int)tec.Data_Type_ID;
        set.columnNameUid = tec.uid;
        set.columnTypeID =(int) tec.Data_Type_ID;
        set.columnIDs = tec.Column_IDs;

        PimpUserWrapper  pimpUser = new PimpUserWrapper ();
        pimpUser.updateColumnSetsForUser(set);

        return Columns.CreateColumnSetsForDisplay(kingdomID, ucdt.uid, 0, pimpUser, cachedKingdom);
    }

    public static string UpColumnForUser(string columnNumber, int setID, Guid userID)
    {
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        var query = (from tec in db.Utopia_Column_Names
                     where tec.User_ID == userID
                     where tec.Data_Type_ID == setID
                     select tec).FirstOrDefault();
        MatchCollection mc = URegEx.rgxNumber.Matches(query.Column_IDs);
        List<string> items = new List<string>();

        for (int i = 0; i < mc.Count; i++)
        {
            items.Add(mc[i].Value);

            if (items[i] == columnNumber)//identify the selected item
            {
                //swap with the top item(move up)
                if (i > 0)
                {
                    string bottom = items[i];
                    string top = items[i - 1];
                    items[i - 1] = bottom;
                    items[i] = top;
                }
            }
        }
        query.Column_IDs = string.Empty;
        for (int i = 0; i < items.Count; i++)
            query.Column_IDs += ":" + items[i];
        db.SubmitChanges();
        //updates the set
        PimpUserWrapper  pimpUser = new PimpUserWrapper ();
        var set = pimpUser.PimpUser.UserColumns.Where(x => x.setUid == setID).FirstOrDefault();
        set.columnIDs = query.Column_IDs;
        pimpUser.updateColumnSetsForUser( set);

        return Columns.GetUserDefinedColumns(set.columnIDs);
    }

    public static string UpColumnForKingdom(string columnNumber, int setID, Guid kingdomID, OwnedKingdomProvinces cachedKingdom)
    {
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        var query = (from tec in db.Utopia_Column_Names
                     where tec.User_ID == kingdomID
                     where tec.Data_Type_ID == setID
                     select tec).FirstOrDefault();
        MatchCollection mc = URegEx.rgxNumber.Matches(query.Column_IDs);
        List<string> items = new List<string>();

        for (int i = 0; i < mc.Count; i++)
        {
            items.Add(mc[i].Value);

            if (items[i] == columnNumber)//identify the selected item
            {
                //swap with the top item(move up)
                if (i > 0)
                {
                    string bottom = items[i];
                    string top = items[i - 1];
                    items[i - 1] = bottom;
                    items[i] = top;
                }
            }
        }
        query.Column_IDs = string.Empty;
        for (int i = 0; i < items.Count; i++)
            query.Column_IDs += ":" + items[i];
        db.SubmitChanges();
        var ids = KingdomCache.updateColumnSetsForKingdom(kingdomID, cachedKingdom).KingdomColumnSets.Where(x => x.setUid == setID).FirstOrDefault().columnIDs;
        return Columns.GetUserDefinedColumns(ids);
    }


    public static string DownColumnForUser(string columnNumber, int setID, Guid userID)
    {
        int dirtyBit = 0;
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        var query = (from tec in db.Utopia_Column_Names
                     where tec.User_ID == userID
                     where tec.Data_Type_ID == setID
                     select tec).FirstOrDefault();
        MatchCollection mc = URegEx.rgxNumber.Matches(query.Column_IDs);
        List<string> items = new List<string>();
        query.Column_IDs = string.Empty;
        string top = string.Empty;
        string bottom = string.Empty;

        for (int i = 0; i < mc.Count; i++)
        {
            if (mc[i].Value == columnNumber && bottom != mc[i].Value && top != mc[i].Value)//identify the selected item
            {
                //swap with the top item(move up)
                if (i < mc.Count && dirtyBit == 0)
                {
                    bottom = mc[i].Value;
                    top = mc[i + 1].Value;
                    items.Add(top);
                    items.Add(bottom);
                    dirtyBit = 1;
                }
            }
            else if (bottom != mc[i].Value && top != mc[i].Value)
            {
                items.Add(mc[i].Value);
            }
            query.Column_IDs += ":" + items[i];
        }
        db.SubmitChanges();

        //updates the set
        PimpUserWrapper  pimpUser = new PimpUserWrapper ();
        var set = pimpUser.PimpUser.UserColumns.Where(x => x.setUid == setID).FirstOrDefault();
        set.columnIDs = query.Column_IDs;
        pimpUser.updateColumnSetsForUser( set);

        return Columns.GetUserDefinedColumns(set.columnIDs);
    }

    public static string DownColumnForKingdom(string columnNumber, int setID, Guid kingdomID, OwnedKingdomProvinces cachedKingdom)
    {
        int dirtyBit = 0;
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        var query = (from tec in db.Utopia_Column_Names
                     where tec.User_ID == kingdomID
                     where tec.Data_Type_ID == setID
                     select tec).FirstOrDefault();
        MatchCollection mc = URegEx.rgxNumber.Matches(query.Column_IDs);
        List<string> items = new List<string>();
        query.Column_IDs = string.Empty;
        string top = string.Empty;
        string bottom = string.Empty;

        for (int i = 0; i < mc.Count; i++)
        {
            if (mc[i].Value == columnNumber && bottom != mc[i].Value && top != mc[i].Value)//identify the selected item
            {
                //swap with the top item(move up)
                if (i < mc.Count && dirtyBit == 0)
                {
                    bottom = mc[i].Value;
                    top = mc[i + 1].Value;
                    items.Add(top);
                    items.Add(bottom);
                    dirtyBit = 1;
                }
            }
            else if (bottom != mc[i].Value && top != mc[i].Value)
            {
                items.Add(mc[i].Value);
            }
            query.Column_IDs += ":" + items[i];
        }
        db.SubmitChanges();
        var ids = KingdomCache.updateColumnSetsForKingdom(kingdomID, cachedKingdom).KingdomColumnSets.Where(x => x.setUid == setID).FirstOrDefault().columnIDs;
        return Columns.GetUserDefinedColumns(ids);
    }
    public static string ToggleColumnForUser(int columnNumber, int setID, Guid userID, Guid kingdomID)
    {
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        var query = (from tec in db.Utopia_Column_Names
                     where tec.User_ID == userID
                     where tec.Data_Type_ID == setID
                     select tec).FirstOrDefault();

        PimpUserWrapper  pimpUser = new PimpUserWrapper ();
        var set = pimpUser.PimpUser.UserColumns.Where(x => x.setUid == setID).FirstOrDefault();

        if (query != null)
        {
            int dirtyBit = 0;
            string columns = query.Column_IDs;
            query.Column_IDs = string.Empty;
            for (int i = 0; i < URegEx.rgxNumber.Matches(columns).Count; i++)
            {
                if (URegEx.rgxNumber.Matches(columns)[i].Value == columnNumber.ToString())
                    dirtyBit = 1;
                else
                    query.Column_IDs += ":" + URegEx.rgxNumber.Matches(columns)[i].Value;
            }
            switch (dirtyBit)
            {
                case 0:
                    query.Column_IDs += ":" + columnNumber;
                    break;
            }
            set.columnIDs = query.Column_IDs;
        }
        else
        {
            CS_Code.Utopia_Column_Name tec = new CS_Code.Utopia_Column_Name();
            tec.DateTime_Added = DateTime.UtcNow;
            tec.User_ID = userID;
            tec.Column_IDs = columnNumber.ToString();
            tec.Data_Type_ID = setID;
            db.Utopia_Column_Names.InsertOnSubmit(tec);
            set.columnIDs = tec.Column_IDs;
        }
        db.SubmitChanges();


        pimpUser.updateColumnSetsForUser( set);
        return Columns.GetUserDefinedColumns(set.columnIDs);
    }
    public static string ToggleColumnForKingdom(int columnNumber, int setID, Guid userID, Guid kingdomID, OwnedKingdomProvinces cachedKingdom)
    {
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        var query = (from tec in db.Utopia_Column_Names
                     where tec.User_ID == kingdomID
                     where tec.Data_Type_ID == setID
                     select tec).FirstOrDefault();

        if (query != null)
        {
            int dirtyBit = 0;
            string columns = query.Column_IDs;
            query.Column_IDs = string.Empty;
            for (int i = 0; i < URegEx.rgxNumber.Matches(columns).Count; i++)
            {
                if (URegEx.rgxNumber.Matches(columns)[i].Value == columnNumber.ToString())
                    dirtyBit = 1;
                else
                    query.Column_IDs += ":" + URegEx.rgxNumber.Matches(columns)[i].Value;
            }
            switch (dirtyBit)
            {
                case 0:
                    query.Column_IDs += ":" + columnNumber;
                    break;
            }
        }
        else
        {
            CS_Code.Utopia_Column_Name tec = new CS_Code.Utopia_Column_Name();
            tec.DateTime_Added = DateTime.UtcNow;
            tec.User_ID = kingdomID;
            tec.Column_IDs = columnNumber.ToString();
            tec.Data_Type_ID = setID;
            db.Utopia_Column_Names.InsertOnSubmit(tec);
        }
        db.SubmitChanges();
        var ids = KingdomCache.updateColumnSetsForKingdom(kingdomID, cachedKingdom).KingdomColumnSets.Where(x => x.setUid == setID).FirstOrDefault().columnIDs;
        return Columns.GetUserDefinedColumns(ids);
    }
    /// <summary>
    /// inserts a column for the user.
    /// </summary>
    /// <param name="ColumnID"></param>
    /// <param name="currentUserID"></param>
    public static void InsertColumnChoice(int ColumnID, Guid currentUserID)
    {
       
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            CS_Code.Utopia_Column_Name UCN = new CS_Code.Utopia_Column_Name();
            UCN.User_ID = currentUserID;
            UCN.Column_IDs = ColumnID.ToString();
            UCN.DateTime_Added = DateTime.UtcNow;
            db.Utopia_Column_Names.InsertOnSubmit(UCN);
            db.SubmitChanges();
     
    }
}