<%@ WebHandler Language="C#" Class="PullData" %>

using System;
using System.Web;

public class PullData : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        if (context.Request["apikey"] == null | context.Request["secretkey"] == null)
        {
            context.Response.ContentType = "text/html";
            context.Response.Write("THE API IS STILL BEING DEVELOPED...  IT CURRENTLY DOES NOT WORK.");
            context.Response.Write("<br/>");
            context.Response.Write("Welcome to UtopiaPimp's API.");
            context.Response.Write("<br/>");
            context.Response.Write("In order to use this API, you must first obtain 2 keys from your monarch. This can be found on the Monarchs main page.");
            context.Response.Write("<div>Required for each request</div>");
            context.Response.Write("<ul>");
            context.Response.Write("<li>apikey: Kingdoms API Key can be found on the monarchs web page</li>");
            context.Response.Write("<li>secretkey: Kingdoms Secret Key can be found on the monarchs web page</li>");
            context.Response.Write("</ul>");

            context.Response.Write("<div>Optional items for each request</div>");
            context.Response.Write("<ul>");
            context.Response.Write("<li>kIL: Kingdoms Island and Location to pull from.</li>");
            context.Response.Write("<li>type: (ce|kingpage)</li>");
            context.Response.Write("<li>time: In integer format, Optional item for pulling back a CE which tells how far to look back in RL hours. If no time is set, pulls back the last 24 hours of a CE.</li>");
            context.Response.Write("</ul>");

            context.Response.Write("<ul>");
            context.Response.Write("<li>provname: Searches for the best matched Owned Province.</li>");
            context.Response.Write("<li>type: (all|cb|som|survey|sos|op) Pulls back the most recent data entered for the province.</li>");
            context.Response.Write("<li>time: In integer format, Optional item for pulling back Ops completed on the province in RL hours. If no time is set, pulls back the last 24 hours.</li>");
            context.Response.Write("</ul>");
            context.Response.End();
        }
        
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}