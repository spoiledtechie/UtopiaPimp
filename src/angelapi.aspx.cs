using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;

using Pimp.UParser;
using SupportFramework.Data;
using Pimp.UCache;
using Pimp.UData;
using Pimp.Utopia;


public partial class angel : System.Web.UI.Page
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
                    else if (data != null && username != null && forumname != null && password != null)
                    {
                        //ClearAngelPostsAfterBackedUp(data);


                        bool isValidated = false;
                        var users = PimpUserWrapper.getListOfUsers();
                        var user = users.Where(x => x.UserName == username.ToLower()).FirstOrDefault();
                        PimpUserWrapper pimpUser = null;                        

                        if (user != null && user.Password == password)
                        {
                            isValidated = true;
                            pimpUser = new PimpUserWrapper(username);
                        }
                        else if (Membership.ValidateUser(username, password))
                        {
                            isValidated = true;
                             pimpUser = new PimpUserWrapper(username);
                        pimpUser.PimpUser.Password = password;
                            PimpUserWrapper.updateListOfUsers(pimpUser.PimpUser);
                        }

                        if (isValidated)
                        {
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
                                            Session.Add("SubmittedData", username + ":" + password + ":" + forumname + ":" + forumpass + ":" + row + ":");
                                            UtopiaParser.UtopiaParsing(row, "angel", "2", forumname, string.Empty, pimpUser, KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom));
                                            Response.Write("+" + i + (char)10 + (char)13);
                                            i += 1;
                                            Session.Remove("SubmittedData");
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
                        HttpContext.Current.Response.Write("-LOGIN Account Username/Forum Name/Password Required/Bulk Data Required.");
                    break;
            }
        }
        catch (Exception excp)
        {
            Errors.logError(excp);
            HttpContext.Current.Response.Write("-LOGIN Your Forum Agent API is incompatible Or Identity Information is Old. BULK_MODE only.");
        }
    }

    private void ClearAngelPostsAfterBackedUp(string data)
    {
        Response.Write("+LOGIN" + (char)10 + (char)13); //"+LOGIN\r\n"
        List<string> bulk = new List<string>();
        int i = 0;
        foreach (var group in data.Split((char)29))
        {
            foreach (var row in group.Split((char)30))
            {
                Response.Write("+" + i + (char)10 + (char)13);
                i += 1;
            }
        }
    }
}