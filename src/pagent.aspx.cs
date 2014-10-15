using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class pagent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string _currverr = "2010.05.14";
        //if (!IsPostBack)
        //{
        //    if (Session["FormInfo"] == null)
        //    {
        //        Session["FormInfo"] = Request.Form;
        //        Session["SubmittedData"] = Request.Form;
        //    }
        //    NameValueCollection form = (NameValueCollection)Session["FormInfo"];

        //    string userName = form.Get("username");
        //    string password = form.Get("password");
        //    string provincename = form.Get("provincename");
        //    string action = form.Get("action");

        //    if (userName != null & password != null & provincename != null & action != null | userName != string.Empty & password != string.Empty & provincename != string.Empty & action != string.Empty | Page.User.Identity.IsAuthenticated)
        //    {
        //        if (Membership.ValidateUser(userName, password))
        //        {
        //            if (action == "checkuser")
        //            {
        //                Guid userID;
        //                if (Page.User.Identity.IsAuthenticated)
        //                                                           userID = SupportFramework.Users.Memberships.getUserID();
        //                else
        //                    userID = SupportFramework.Users.Memberships.getUserID(userName);

        //                CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        //                var ValidateProvinceName = (from  xx in db.Utopia_Province_Data_Captured_Gens
        //                                            from zz in db.Utopia_Kingdom_Infos
        //                                            where zz.Owner_Kingdom_ID == xx.Owner_Kingdom_ID
        //                                            where xx.Owner_User_ID == userID
        //                                            select new { xx.Province_Name, zz.Server_ID });
        //                bool check = false;
        //                int serverID = 0;
        //                foreach (var item in ValidateProvinceName)
        //                    if (item.Province_Name == provincename)
        //                    {
        //                        check = true;
        //                        serverID = item.Server_ID;
        //                    }
        //                if (check == false)
        //                    HttpContext.Current.Response.Write("ERROR: Province Name was not found, Check the spelling. \n");
        //                else
        //                    HttpContext.Current.Response.Write("wol: " + provincename);

        //                Session.Remove("FormInfo");
        //                Session.Remove("SubmittedData");
        //            }
        //            else
        //            {
        //                string currver = form.Get("currver");
        //                if (currver == _currverr)
        //                {
        //                    if (action == "postdata")
        //                    {

        //                        var currentUser = CachedItems.GetUserAngelCached(userName);
        //                                var province = currentUser.PimpUser.ProvincesOwned.Where(x => x.Province_Name == provincename).FirstOrDefault();

        //                                if (province != null && province.Province_ID != null)
        //                                {
        //                                    if (province.Province_ID != new Guid())
        //                                    {
        //                                                                                        StringBuilder sb = new StringBuilder();
        //                                        MatchCollection mc = Static.rgxNumber.Matches(form.Get("ids"));
        //                                        for (int i = 0; i < mc.Count; i++)
        //                                        {
        //                                            string data = form.Get("data" + Convert.ToInt32(mc[i].Value));
        //                                            data = Server.UrlDecode(data);
        //                                            try
        //                                            {
        //                                                byte[] encbuff = Convert.FromBase64String(data);
        //                                                data = System.Text.Encoding.UTF8.GetString(encbuff);
        //                                                Session["SubmittedData"] += data;
        //                                            }
        //                                            catch
        //                                            {
        //                                                Session.Remove("FormInfo");
        //                                                Session.Remove("SubmittedData");
        //                                            }
        //                                            UtopiaParser.UtopiaParsing(data, "pagent", "2", form.Get("provincename"), string.Empty, currentUser, KingdomCache.getKingdom(currentUser.PimpUser.StartingKingdom));
        //                                            if (HttpContext.Current.Session["Failed"] != null)
        //                                            {
        //                                                if ((int)HttpContext.Current.Session["Failed"] == 0) //If Item doesn't fail
        //                                                    sb.Append("RESULT_" + Convert.ToInt32(mc[i].Value) + "_1: Intel Uploaded.\n");
        //                                            }
        //                                            else
        //                                                sb.Append("RESULT_" + Convert.ToInt32(mc[i].Value) + "_1: Intel Uploaded.\n");

        //                                            Session.Remove("Failed");
        //                                        }
        //                                        HttpContext.Current.Response.Write(sb.ToString());
        //                                        Session.Remove("FormInfo");
        //                                        Session.Remove("SubmittedData");
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    HttpContext.Current.Response.Write("ERROR: Province Name Could Not Be Found. \n");
        //                                }
                               
        //                    }
        //                    else
        //                    {
        //                        Session.Remove("FormInfo"); Session.Remove("SubmittedData");
        //                        HttpContext.Current.Response.Write("ERROR: No Action to Commit. \n");
        //                    }
        //                }
        //                else
        //                    HttpContext.Current.Response.Write("UPGRADE: Version " + _currverr + " is ready!, you currently have " + currver + " \n");
        //            }
        //        }
        //        else
        //            HttpContext.Current.Response.Write("ERROR: Couldn\'t validate username/password combination. \n");
        //    }
        //    else
        //    {
        //        if (userName == string.Empty)
        //            HttpContext.Current.Response.Write("ERROR: No username. \n");
        //        if (password == string.Empty)
        //            HttpContext.Current.Response.Write("ERROR: No password. \n");
        //        if (action == string.Empty)
        //            HttpContext.Current.Response.Write("ERROR: No action item. \n");
        //        if (provincename == string.Empty)
        //            HttpContext.Current.Response.Write("ERROR: No province name. \n");
        //    }
        //    Session.Remove("FormInfo");
        //    Session.Remove("SubmittedData");
        //}
        //Session.Remove("FormInfo");
        //Session.Remove("SubmittedData");
        HttpContext.Current.Response.Write("ERROR: Pimp Agent For Utopiapimp.com is currently down. \n");
    }

}
