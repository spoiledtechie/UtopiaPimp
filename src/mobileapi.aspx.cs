using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Text.RegularExpressions;

using Pimp.UParser;
using Pimp.UCache;
using SupportFramework.Data;
using Pimp.UData;

public partial class mobileapi : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            switch (!IsPostBack)
            {
                case true:
                    int API_ENGINE_VERSION = 1;
                    int MINIMUM_AGENT_VERSION = 19;
                    string BULK_MODE = "yes";
                    string FORUM_AGENT_HEADER = "[FORUM AGENT API]";
                    //string INSTRUCTION_PREFIX = "FORUMAGENT:";
                    string RECORD_SEP = new string((char)30, 1);
                    string GROUP_SEP = new string((char)29, 1);
                    string data = Request.Form["bulk_data"];

                    string username = Request.Form["username"];
                    string password = Request.Form["password"];
                    string forumname = Request.Form["forum_name"];
                    string forumpass = Request.Form["forum_password"];
                    

                    if (username == null && forumname == null)
                    {
                        HttpContext.Current.Response.Write(FORUM_AGENT_HEADER + (char)13 + (char)10);
                        HttpContext.Current.Response.Write("FORUMAGENT:api_engine_version=\"" + API_ENGINE_VERSION + "\"" + (char)13 + (char)10);
                        HttpContext.Current.Response.Write("FORUMAGENT:minimum_forum_agent_version=\"" + MINIMUM_AGENT_VERSION + "\"" + (char)13 + (char)10);
                        HttpContext.Current.Response.Write("FORUMAGENT:bulk_mode=\"" + BULK_MODE + "\"" + (char)13 + (char)10);
                        HttpContext.Current.Response.Write("IF_YOUR_READING_THIS: each piece of data must have a header line Exactly matching: \"The following data was posted by\"" + (char)13 + (char)10);
                        HttpContext.Current.Response.Write("IF_YOUR_READING_THIS: Bulk Data ONLY can be sent to this API. Matching this standard for bulk data: http://utopiapimp.com/data/forum-agent-api.txt" + (char)13 + (char)10);
                        //HttpContext.Current.Response.Write("FORUMAGENT:debug_mode=\"" + BULK_MODE + "\"" + (char)13 + (char)10);
                    }
                    else if (username != null && forumname != null && password != null)
                    {
                        if (Membership.ValidateUser(username, password))
                        {
                            PimpUserWrapper  pimpUser = new PimpUserWrapper (username);

                            var province = pimpUser.PimpUser.ProvincesOwned.Where(x => x.Province_Name == forumname).FirstOrDefault();
                            if (province != null && province.Province_ID != null && province.Province_ID != new Guid())
                            {
                                Response.Write("+LOGIN" + (char)10 + (char)13); //"+LOGIN\r\n"
                                List<string> bulk = new List<string>();
                                int i = 0;
                                foreach (var group in data.Split((char)29))
                                {
                                    foreach (var row in group.Split((char)30))
                                    {
                                        if (row.Contains("The following data was posted by"))
                                        {
                                            string te = Regex.Replace(Server.HtmlDecode(row.Replace("~", "&")), "<(.|\n)*?>", "");
                                            Session.Add("SubmittedData", username + ":" + password + ":" + forumname + ":" + forumpass + ":" + te + ":");
                                            //Session.Add("SubmittedData", username + ":" + password + ":" + forumname + ":" + forumpass + ":" + te + ":");
                                         Errors.failedAt("test", te, Guid.NewGuid());
                                         UtopiaParser.UtopiaParsing(te, "angel", "2", forumname, string.Empty, pimpUser, KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom));
                                            Session.Remove("SubmittedData");
                                            Response.Write("+" + i + (char)10 + (char)13);
                                            i += 1;
                                        }
                                    }
                                }
                            }
                            else
                                HttpContext.Current.Response.Write("-LOGIN Forum Name required. UserName Correct, ForumName == Province Name, Not Found");
                        }
                        else
                            HttpContext.Current.Response.Write("-LOGIN Username & Password Are Incorrect");
                    }
                    else
                        HttpContext.Current.Response.Write("-LOGIN Account Username/Forum Name/Password Required.");
                    break;
            }
        }
        catch (Exception excp)
        {
            Errors.logError(excp);
            HttpContext.Current.Response.Write("-LOGIN Your Forum Agent API is incompatible. BULK_MODE only.");
        }
    }
}