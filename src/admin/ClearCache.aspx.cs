using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ClearCache : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        foreach (System.Collections.DictionaryEntry entry in Cache)
        {
            Cache.Remove(entry.Key.ToString());
        }
        Session.Clear();
        Session.Abandon();
        Session.RemoveAll();
    }
}