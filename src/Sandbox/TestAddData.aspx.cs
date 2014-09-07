using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Pimp.UCache;
using Pimp.UParser;
using Pimp;
using Pimp.Users;
using Pimp.Utopia;
using Pimp.UData;

public partial class Sandbox_TestAddData : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {


    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        PimpUserWrapper  pimpUser = new PimpUserWrapper ();
        string dataAdded = UtopiaParser.UtopiaParsing(dataToAdd.Text, "AddData", "1", "", "", pimpUser, KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom));
        Response.Write(dataAdded);
    }
}