using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

using Pimp;
using Pimp.UParser;
using PimpLibrary.Static.Enums;
using Pimp.Utopia;
using Pimp.Users;
using Pimp.UData;

namespace Pimp.UIBuilder
{

    /// <summary>
    /// Summary description for Columns
    /// </summary>
    public class Columns
    {
        public static string CreateDT(Guid userKingdomID, int dateTypeID, string query)
        {
                        StringBuilder sb = new StringBuilder();
            sb.Append("<ul>");
            if (query != null)
            {
                int queryMatches = URegEx.rgxNumber.Matches(query).Count;
                for (int i = 0; i < queryMatches; i++)
                {
                    var getitem = (from tec in UtopiaHelper.Instance.ColumnNames
                                   where tec.uid.ToString() == URegEx.rgxNumber.Matches(query)[i].Value
                                   select new
                                   {
                                       Item = tec.columnName,
                                       ID = tec.uid
                                   }).FirstOrDefault();

                    if (getitem != null)
                    {
                        sb.Append("<li class=\"liActiveColumns\">");
                        switch (i)
                        {
                            case 0:
                                break;
                            default:
                                sb.Append("<img src=\"http://codingforcharity.org/utopiapimp/img/rise_dark.gif\" class=\"imgUp\" name=\"" + getitem.ID + "\" onclick=\"UpColumn(this); return false;\" /> ");
                                break;
                        }
                        sb.Append(getitem.Item);
                        if (i != URegEx.rgxNumber.Matches(query).Count - 1)
                            sb.Append(" <img src=\"http://codingforcharity.org/utopiapimp/img/fall_dark.gif\" class=\"imgDown\" name=\"" + getitem.ID + "\" onclick=\"DownColumn(this); return false;\" />");
                        sb.Append("</li>");
                    }
                }
            }
            sb.Append("<li>(click <img src=\"http://codingforcharity.org/utopiapimp/img/rise_dark.gif\" />/<img src=\"http://codingforcharity.org/utopiapimp/img/fall_dark.gif\" /> to change order)</li>");
            sb.Append("</ul>");

            return sb.ToString();
        }

        public static string CreateColumnSetsForDisplay(Guid kingdomID, int? selectedSet, int? userOrKingdom, PimpUserWrapper  currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            StringBuilder sb = new StringBuilder();
            var kingdomSets = cachedKingdom.KingdomColumnSets;
            var userColumns = currentUser.PimpUser.UserColumns;
            if (userColumns.Count < 2)
            {
                sb.Append("<div><ol>");
                sb.Append("<b>Before You Begin: PLEASE READ</b><br />");
                sb.Append("<li>UP 2.1 has upgraded the way to choose column sets.  In order to do it correctly, please follow the instructions below. UP Columns are Dynamic.</li>");
                sb.Append("<li>Add a ‘Column Set’ by going to ‘My Sets’.  Click on ‘Add Set’ and name the set. Once named and added.</li>");
                sb.Append("<li>Select from the boxes under General, Planner, Resources, Military, Buildings or Science.</li>");
                sb.Append("<li>Click on the boxes to select a column.</li>");
                sb.Append("<li>Selected column values will appear colored and in a list under Colum name.</li>");
                sb.Append("<li>Once done, you will need to REFRESH your browser. You will then have Columns.</li>");
                sb.Append("</ol></div>");
            }

            sb.Append("<ul class=\"ulMyColumnSets\"><li>Your Kingdom's Sets:</li>");
            if (selectedSet == null) // if a setnameuid isn't selected...
            {
                for (int i = 0; i < kingdomSets.Count; i++)
                    sb.Append("<li><span ty=\"k\" onclick=\"ChangeSet(this);return false;\" name=\"" + kingdomSets[i].setUid + "\" >" + kingdomSets[i].setName + "</span></li>");
            }
            else
            { //uid is selected so I run through the sets and see if it belongs to the kingdom set.
                for (int i = 0; i < kingdomSets.Count; i++)
                    if (kingdomSets[i].setUid == selectedSet)
                        if (userOrKingdom != null && userOrKingdom == 1)
                            sb.Append("<li><span class=\"ColumnOn\" ty=\"k\" id=\"activeColumnSet\" name=\"" + kingdomSets[i].setUid + "\" >" + kingdomSets[i].setName + "</span></li>");
                        else
                            sb.Append("<li><span ty=\"k\" onclick=\"ChangeSet(this);return false;\" name=\"" + kingdomSets[i].setUid + "\" >" + kingdomSets[i].setName + "</span></li>");
                    else
                        sb.Append("<li><span ty=\"k\" onclick=\"ChangeSet(this);return false;\" name=\"" + kingdomSets[i].setUid + "\" >" + kingdomSets[i].setName + "</span></li>");
            }
            if (currentUser.PimpUser.MonarchType != MonarchType.none && currentUser.PimpUser.MonarchType != MonarchType.kdMonarch)
            {
                sb.Append("<li id=\"AddKingdomSet\"><input id=\"btnAddKingdomSet\" type=\"button\" value=\"Add Set\" onclick=\"AddKingdomSet(this);return false;\" /></li>");
                sb.Append("<li id=\"DeleteKingdomSet\"><input id=\"btnDeleteKingdomSet\" type=\"button\" value=\"Delete Set\" onclick=\"DeleteKingdomSet(this);return false;\" /></li>");
                switch (kingdomSets.Count)
                {
                    case 0:
                        sb.Append("<li> As 'Kingdom Owner', you are allowed to decide default column sets for your kingdom.</li>");
                        break;
                }
            }
            else
                sb.Append("<li>Only Kingdom Owners Or Monarchs can modify kingdom sets.</li>");

            sb.Append("<li id=\"AddMyKingdomSet\"><input id=\"btnMyKingdomSet\" type=\"button\" value=\"Add to My Sets\" onclick=\"AddToMySet(this);return false;\" /></li>");
            sb.Append("</ul>");

            sb.Append("<ul class=\"ulMyColumnSets\"><li>My Sets:</li>");

            if (selectedSet == null) // if a setnameuid isn't selected...
            {
                for (int i = 0; i < userColumns.Count; i++)
                {
                    switch (i)
                    {
                        case 0: //first selected set
                            sb.Append("<li><span class=\"ColumnOn\" ty=\"u\" onclick=\"ChangeSet(this);return false;\" id=\"activeColumnSet\" name=\"" + userColumns[i].setUid + "\">" + userColumns[i].setName + "</span></li>");
                            selectedSet = userColumns[i].setUid;
                            break;
                        default:
                            sb.Append("<li><span ty=\"u\" onclick=\"ChangeSet(this);return false;\" name=\"" + userColumns[i].setUid + "\">" + userColumns[i].setName + "</span></li>");
                            break;
                    }
                }
            }
            else
            { //uid is selected so I run through the sets and see if it belongs to the kingdom set.
                for (int i = 0; i < userColumns.Count; i++)
                    if (userColumns[i].setUid == selectedSet)
                        if (userOrKingdom != null && userOrKingdom == 0)
                            sb.Append("<li><span class=\"ColumnOn\" ty=\"u\" onclick=\"ChangeSet(this);return false;\" id=\"activeColumnSet\" name=\"" + userColumns[i].setUid + "\">" + userColumns[i].setName + "</span></li>");
                        else
                            sb.Append("<li><span ty=\"u\" onclick=\"ChangeSet(this);return false;\" name=\"" + userColumns[i].setUid + "\">" + userColumns[i].setName + "</span></li>");
                    else
                        sb.Append("<li><span ty=\"u\" onclick=\"ChangeSet(this);return false;\" name=\"" + userColumns[i].setUid + "\">" + userColumns[i].setName + "</span></li>");
            }
            sb.Append("<li id=\"AddSet\"><input id=\"btnAddSet\" type=\"button\" value=\"Add Set\" onclick=\"AddSet(this);return false;\" /></li>");
            sb.Append("<li id=\"DeleteSet\"><input id=\"btnDeleteSet\" type=\"button\" value=\"Delete Set\" onclick=\"DeleteSet(this);return false;\" /></li>");
            sb.Append("</ul>");
            sb.Append("<div style=\"Display:none;\" id=\"selectedSetID\">" + selectedSet + "</div>");
            return sb.ToString();
        }

        public static string DisplayColumnsForDisplay(int setIDSelected, Guid kingdomID, Guid userID, PimpUserWrapper  currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            StringBuilder sb = new StringBuilder();
            var kingdomSets = cachedKingdom.KingdomColumnSets;
            var userSets = currentUser.PimpUser.UserColumns;

            var selectedSet = userSets.Where(x => x.setUid == setIDSelected).FirstOrDefault();
            if (selectedSet == null)
                selectedSet = kingdomSets.Where(x => x.setUid == setIDSelected).FirstOrDefault();

            var getCatagoryTypes = (from xx in UtopiaHelper.Instance.ColumnNames select new { xx.categoryID, xx.categoryName }).Distinct();

            sb.Append("<ul class=\"ulColumnChooser\">");
            foreach (var item in getCatagoryTypes)
            {
                sb.Append("<li class=\"liColumnChooserHeader\">");
                sb.Append(item.categoryName);
                sb.Append("</li>");
            }
            sb.Append("<li class=\"liColumnChooserHeader\">Column Name</li>");
            sb.Append("</ul>");


            sb.Append("<ul class=\"ulColumnChooser\">");
            foreach (var item in getCatagoryTypes)
            {
                sb.Append("<li class=\"liColumnChooser\">");
                var getColumnNames =UtopiaHelper.Instance.ColumnNames.Where(x => x.categoryID == item.categoryID).OrderBy(x => x.columnName);

                sb.Append("<ul>");
                foreach (var column in getColumnNames)
                {
                    sb.Append("<li>");
                    int dirtyBit = 0;
                    try
                    {
                        var columnCount = URegEx.rgxNumber.Matches(selectedSet.columnIDs).Count;
                        for (int i = 0; i < columnCount; i++)
                            if (URegEx.rgxNumber.Matches(selectedSet.columnIDs)[i].Value == column.uid.ToString())
                                dirtyBit = 1;
                    }
                    catch { }
                    switch (dirtyBit)
                    {
                        case 0:
                            sb.Append("<span onclick=\"SetColumn(this);return false;\" name=\"" + column.uid + "\" title=\"" + column.alt + "\">" + column.columnName + "</span>");
                            break;
                        case 1:
                            sb.Append("<span class=\"ColumnOn\" onclick=\"SetColumn(this);return false;\" name=\"" + column.uid + "\" title=\"" + column.alt + "\">" + column.columnName + "</span>");
                            break;
                    }
                    sb.Append("</li>");
                }

                sb.Append("</ul>");
                sb.Append("</li>");
            }

        
                sb.Append("<li class=\"liColumnChooser\" id=\"UserDefinedColumns\">");
                sb.Append(GetUserDefinedColumns(selectedSet.columnIDs));
                sb.Append("</li>");
         

            sb.Append("</ul>");
            return sb.ToString();
        }

        public static string GetUserDefinedColumns(string columnIDs)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul>");
            if (columnIDs != null)
            {
                for (int i = 0; i < URegEx.rgxNumber.Matches(columnIDs).Count; i++)
                {
                    var getitem = (from tec in UtopiaHelper.Instance.ColumnNames
                                   where tec.uid.ToString() == URegEx.rgxNumber.Matches(columnIDs)[i].Value
                                   select new
                                   {
                                       Item = tec.columnName,
                                       ID = tec.uid
                                   }).FirstOrDefault();

                    if (getitem != null)
                    {
                        sb.Append("<li class=\"liActiveColumns\">");
                        switch (i)
                        {
                            case 0: //if its at the top of its list
                                break;
                            default:
                                sb.Append("<img src=\"../images/rise_dark.gif\" class=\"imgUp\" name=\"" + getitem.ID + "\" onclick=\"UpColumn(this); return false;\" /> ");
                                break;
                        }
                        sb.Append(getitem.Item);
                        if (i != URegEx.rgxNumber.Matches(columnIDs).Count - 1) //if its not at the bottom of its list
                            sb.Append(" <img src=\"../images/fall_dark.gif\" class=\"imgDown\" name=\"" + getitem.ID + "\" onclick=\"DownColumn(this); return false;\" />");
                        sb.Append("</li>");
                    }
                }
            }
            sb.Append("<li>(click <img src=\"../images/rise_dark.gif\" />/<img src=\"../images/fall_dark.gif\" /> to change order)</li>");
            sb.Append("</ul>");
            return sb.ToString();
        }
    }
}