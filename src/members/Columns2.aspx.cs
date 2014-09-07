using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PimpLibrary.Static.Enums;
using Pimp.UParser;
using Pimp.UCache;
using Pimp.UData;

public partial class members_Columns2 : MyBasePageCS
{    
    

    protected void Page_Load(object sender, EventArgs e)
    {
    
        if (!IsPostBack)
        {
            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            
    
            //If he is monarch he can modify kingdomsets.
            if (pimpUser.PimpUser.MonarchType != MonarchType.none && pimpUser.PimpUser.MonarchType != MonarchType.kdMonarch)
                divExtra.InnerHtml += "<div style=\"Display:none;\" id=\"divMonarch\">true</div>";
            else
                divExtra.InnerHtml += "<div style=\"Display:none;\" id=\"divMonarch\">false</div>";
        }
    }
}