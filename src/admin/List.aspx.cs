using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SupportFramework.Helpers.Extensions;
using System.Xml.Linq;


public partial class admin_List : System.Web.UI.Page
{
    //readonly string DataPath = @"C:\Users\Patrik\Documents\Visual Studio 2010\WebSites\UtopiaPimp\App_Data\Lists.xml";
    readonly string DataPath = @"C:\HostingSpaces\pio.scott@gmail.com\utopiashrimp.com\data\Lists.xml";    

    protected void Page_Load(object sender, EventArgs e)
    {        
        ProcessRequest();        
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "userName", "author = '" + User.Identity.Name + "';", true);
    }

    public bool IsAjaxRequest(HttpRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException("request");
        }

        return (request["X-Requested-With"] == "XMLHttpRequest") || ((request.Headers != null) && (request.Headers["X-Requested-With"] == "XMLHttpRequest"));
    }

    private void ProcessRequest()
    {        
        string callback = Request.Params["Callback"];
        if (string.IsNullOrEmpty(callback))
            return;

        

        string submittedItemID = string.Empty;
        if (callback == "LoadStartup")
        {
            LoadStartup();
        }
        else if (callback == "CreateNewList")
        {
            string listName = Request.Params["ListName"];
            if (string.IsNullOrEmpty(listName))
            {
                PrintFailed("No name supplied");                
            }
            else
            {
                int listID = CreateList(HttpUtility.HtmlEncode(listName));
                Response.Write((new { status = "success", listID = listID, listName = listName }).ToJSON());
                Response.End();
            }            
        }
        else if (callback == "CreateNewItem")
        {            
            string author = Request.Params["Author"];
            string listID = Request.Params["ListID"];
            string itemSubject = Request.Params["ItemSubject"];
            string itemText = Request.Params["ItemText"];
            DateTime posted = DateTime.Now;

            if (string.IsNullOrEmpty(author) || string.IsNullOrEmpty(listID) || string.IsNullOrEmpty(itemSubject) || string.IsNullOrEmpty(itemText))
            {
                PrintFailed("No author, list id, item subject or text supplied");                
            }
            else
            {
                
                int itemID = CreateItem(listID, author, itemSubject, itemText.Replace("%0A", "<br />"), posted);
                Response.Write((new { status = "success", itemID = itemID, itemSubject = itemSubject, itemText = itemText.Replace("%0A", "<br />"), itemPosted = posted.ToString("MMM dd yyyy"), itemCompleted = "false" }).ToJSON());
                Response.End();
            } 
        }
        else if (callback == "UpdateItemDetails")
        {            
            submittedItemID = Request.Params["ItemID"];
            string itemUpdateSubject = Request.Params["ItemSubject"];
            string itemUpdateText = Request.Params["ItemText"];
            if (string.IsNullOrEmpty(submittedItemID) || string.IsNullOrEmpty(itemUpdateSubject) || string.IsNullOrEmpty(itemUpdateText))
            {
                PrintFailed("No list id, item subject or text supplied");
            }
            else
            {
                UpdateItemDetails(submittedItemID, itemUpdateSubject.Replace("%0A", "<br />"), itemUpdateText.Replace("%0A", "<br />"));
                Response.Write("{");
                PrintSuccessStatus();
                PrintItemDetails(true, submittedItemID);
                Response.Write("}");
                Response.End(); 
            }
        }
        else if (callback == "CreateNewMessage")
        {
            string author = Request.Params["Author"];            
            string messageText = Request.Params["MessageText"];
            if (string.IsNullOrEmpty(author) || string.IsNullOrEmpty(messageText))
            {
                PrintFailed("No author or text supplied");
            }
            else
            {
                DateTime posted = DateTime.Now;
                int messageID = CreateMessage(author, messageText.Replace("%0A", "<br />"), posted);
                Response.Write((new { status = "success", messageID = messageID, messageText = messageText.Replace("%0A", "<br />"), messagePosted = posted.ToString("MMM dd yyyy") }).ToJSON());
                Response.End();
            }
        }
        else if (callback == "ListItems")
        {
            string listID = Request.Params["ListID"];
            if (string.IsNullOrEmpty(listID))
            {
                PrintFailed("No id supplied");
                return;
            }
            else
            {
                Response.Write("{");
                PrintSuccessStatus();
                PrintListItems(listID);
                Response.Write("}");
                Response.End();
            }
        }
        else if (callback == "ListNewMessages")
        {
            string lastMessageID = Request.Params["LastMessageID"];
            if (string.IsNullOrEmpty(lastMessageID))
            {
                PrintFailed("No id supplied");
                return;
            }
            else
            {
                Response.Write("{");
                PrintSuccessStatus();
                PrintNewMessages(Int32.Parse(lastMessageID));
                Response.Write("}");
                Response.End();
            }
        }
        else if (callback == "DeleteMessage")
        {
            string messageID = Request.Params["MessageID"];
            if (string.IsNullOrEmpty(messageID))
            {
                PrintFailed("No id supplied");
                return;
            }
            else
            {
                DeleteMessage(messageID);
                Response.Write("{");
                PrintSuccessStatus();
                Response.Write((new { messageID = messageID }).ToJSONWidthoutBrackets());
                Response.Write("}");
                Response.End();
            }
        }
        else if (callback == "DeleteItem")
        {
            submittedItemID = Request.Params["ItemID"];
            if (string.IsNullOrEmpty(submittedItemID))
            {
                PrintFailed("No id supplied");
                return;
            }
            else
            {
                DeleteItem(submittedItemID);
                Response.Write("{");
                PrintSuccessStatus();
                Response.Write((new { itemID = submittedItemID }).ToJSONWidthoutBrackets());
                Response.Write("}");
                Response.End();
            }
        }
        else if (callback == "UpdateItemStatus")
        {
            submittedItemID = Request.Params["ItemID"];
            if (string.IsNullOrEmpty(submittedItemID))
            {
                PrintFailed("No id supplied");
                return;
            }
            else
            {
                string textStatus = UpdateItemStatus(submittedItemID);
                Response.Write("{");
                PrintSuccessStatus();
                Response.Write((new { itemID = submittedItemID, textStatus = textStatus }).ToJSONWidthoutBrackets());
                Response.Write("}");
                Response.End();
            }
        }
        else if (callback == "GetItemDetails")
        {
            submittedItemID = Request.Params["ItemID"];
            if (string.IsNullOrEmpty(submittedItemID))
            {
                PrintFailed("No id supplied");
                return;
            }
            else
            {                
                Response.Write("{");
                PrintSuccessStatus();
                PrintItemDetails(false, submittedItemID);
                Response.Write("}");
                Response.End();
            }
        }

        PrintFailed("No idea why this is hit");
    }
  
    private void PrintFailed(string errorMessage)
    {
        Response.Write((new { status = "failed", errorMessage = errorMessage }).ToJSON());
        Response.End();
    }

    private void PrintSuccessStatus()
    {
        Response.Write((new { status = "success" }).ToJSONWidthoutBrackets());
        Response.Write(",");
    }

    private void LoadStartup()
    {
        XDocument document = XDocument.Load(DataPath);
        
        Response.Write("{ \"LISTS\":[");
        var lists = (from data in document.Descendants("list") select data).ToList();        
        for (int i = 0; i < lists.Count; i++)
        {
            Response.Write((new { listID = lists[i].Attribute("id").Value, listName = lists[i].Attribute("name").Value }).ToJSON());            
            if (i < lists.Count-1)
                Response.Write(",");
        }                    
        Response.Write("],");

        
        Response.Write("\"ITEMS\":[");
        string firstItem = lists[0].Attribute("id").Value;
        var items = (from data in lists.Descendants("item") where data.Parent.Attribute("id").Value == firstItem select data).ToList();
        for (int i = 0; i < items.Count; i++)
        {
            DateTime posted = DateTime.Parse(items[i].Attribute("posted").Value);
            Response.Write((new { itemID = items[i].Attribute("id").Value, itemSubject = items[i].Attribute("subject").Value, itemText = items[i].Value, author = items[i].Attribute("postedBy").Value, itemPosted = posted.ToString("MMM dd yyyy"), itemCompleted = items[i].Attribute("completed").Value }).ToJSON());            
            if (i < items.Count - 1)
                Response.Write(",");
        }
        Response.Write("],");
        

        Response.Write("\"MESSAGES\":[");
        var messages = (from data in document.Descendants("message") orderby Int32.Parse(data.Attribute("id").Value) descending select data).ToList();
        for (int i = 0; i < messages.Count; i++)
        {
            DateTime posted = DateTime.Parse(messages[i].Attribute("posted").Value);
            Response.Write((new { messageID = messages[i].Attribute("id").Value, messageText = messages[i].Value, author = messages[i].Attribute("postedBy").Value, messagePosted = posted.ToString("MMM dd yyyy") }).ToJSON());            
            if (i < messages.Count - 1)
                Response.Write(",");
        }
        Response.Write("]}");
        Response.End();
    }

    private int CreateList(string listName)
    {
        XDocument document = XDocument.Load(DataPath);
        XElement listsRoot = (from data in document.Descendants("lists") select data).Single();
        int nextListID = Int32.Parse(listsRoot.Attribute("nextListID").Value);
        XElement newNode = new XElement("list", new XAttribute("id", nextListID.ToString()), new XAttribute("name", listName));
        nextListID++;
        listsRoot.Attribute("nextListID").Value = nextListID.ToString();
        listsRoot.Add(newNode);
        try
        {
            document.Save(DataPath);            
            return --nextListID;
        }
        catch
        {
            return -1;
        }
    }

    private int CreateMessage(string author, string text, DateTime posted)
    {        
        XDocument document = XDocument.Load(DataPath);
        XElement messagesRoot = (from data in document.Descendants("messages") select data).Single();
        int nextMessageID = Int32.Parse(messagesRoot.Attribute("nextMessageID").Value);
        XElement newNode = new XElement("message", new XAttribute("id", nextMessageID.ToString()), new XAttribute("postedBy", author), new XAttribute("posted", posted.ToString("yyyy-MM-dd HH:mm:ss")));
        newNode.Value = text;
        nextMessageID++;
        messagesRoot.Attribute("nextMessageID").Value = nextMessageID.ToString();
        messagesRoot.AddFirst(newNode);
        try
        {
            document.Save(DataPath);
            return --nextMessageID;
        }
        catch
        {
            return -1;
        }
    }

    private int CreateItem(string listID, string author, string subject, string text, DateTime posted)
    {
        XDocument document = XDocument.Load(DataPath);
        XElement listsRoot = (from data in document.Descendants("lists") select data).Single();
        int nextItemID = Int32.Parse(listsRoot.Attribute("nextItemID").Value);
        XElement newNode = new XElement("item", new XAttribute("id", nextItemID.ToString()), new XAttribute("subject", subject), new XAttribute("postedBy", author), new XAttribute("posted", posted.ToString("yyyy-MM-dd HH:mm:ss")), new XAttribute("completed", "false"));
        newNode.Value = text;
        nextItemID++;
        listsRoot.Attribute("nextItemID").Value = nextItemID.ToString();
        XElement listRoot = (from data in document.Descendants("list") where data.Attribute("id").Value == listID select data).SingleOrDefault();
        if (listRoot == null)
            return -1;
        listRoot.Add(newNode);
        try
        {
            document.Save(DataPath);
            return --nextItemID;
        }
        catch
        {
            return -1;
        }
    }

    private void PrintListItems(string listID)
    {
        XDocument document = XDocument.Load(DataPath);
        var items = (from data in document.Descendants("item") where data.Parent.Attribute("id").Value == listID select data).ToList();
        Response.Write("\"ITEMS\":[");
        for (int i = 0; i < items.Count; i++)
        {
            DateTime posted = DateTime.Parse(items[i].Attribute("posted").Value);
            Response.Write((new { itemID = items[i].Attribute("id").Value, itemSubject = items[i].Attribute("subject").Value, itemText = items[i].Value, author = items[i].Attribute("postedBy").Value, itemPosted = posted.ToString("MMM dd yyyy"), itemCompleted = items[i].Attribute("completed").Value }).ToJSON());            
            if (i < items.Count - 1)
                Response.Write(",");
        }
        Response.Write("]");
    }

    private void PrintNewMessages(int lastMessageID)
    {
        XDocument document = XDocument.Load(DataPath);
        var messages = (from data in document.Descendants("message") let id = Int32.Parse(data.Attribute("id").Value) where id > lastMessageID orderby id descending select data).ToList();
        Response.Write("\"MESSAGES\":[");
        for (int i = 0; i < messages.Count; i++)
        {
            DateTime posted = DateTime.Parse(messages[i].Attribute("posted").Value);
            Response.Write((new { messageID = messages[i].Attribute("id").Value, messageText = messages[i].Value, author = messages[i].Attribute("postedBy").Value, messagePosted = posted.ToString("MMM dd yyyy") }).ToJSON());
            if (i < messages.Count - 1)
                Response.Write(",");
        }
        Response.Write("]");
    }

    private string UpdateItemStatus(string itemID)
    {
        XDocument document = XDocument.Load(DataPath);
        XElement message = (from data in document.Descendants("item") where data.Attribute("id").Value == itemID select data).SingleOrDefault();
        if (message == null)
            return "";
        string status = message.Attribute("completed").Value;
        if (status == "false")
            status = message.Attribute("completed").Value = "true";
        else
            status = message.Attribute("completed").Value = "false";
        document.Save(DataPath);
        return status;
    }

    private void DeleteMessage(string messageID)
    {
        XDocument document = XDocument.Load(DataPath);
        XElement message = (from data in document.Descendants("message") where data.Attribute("id").Value == messageID select data).SingleOrDefault();
        if (message == null)
            return;
        message.Remove();
        document.Save(DataPath);
    }

    private void DeleteItem(string itemID)
    {
        XDocument document = XDocument.Load(DataPath);
        XElement message = (from data in document.Descendants("item") where data.Attribute("id").Value == itemID select data).SingleOrDefault();
        if (message == null)
            return;
        message.Remove();
        document.Save(DataPath);
    }

    private void PrintItemDetails(bool allDetails, string itemID)
    {
        XDocument document = XDocument.Load(DataPath);
        XElement item = (from data in document.Descendants("item") where data.Attribute("id").Value == itemID select data).SingleOrDefault();
        Response.Write("\"ITEM\":[");
        if (!allDetails)
            Response.Write((new { itemID = item.Attribute("id").Value, itemSubject = HttpUtility.HtmlDecode(item.Attribute("subject").Value.Replace("<br />", "\n")), itemText = HttpUtility.HtmlDecode(item.Value.Replace("<br />", "\n")) }).ToJSON());            
        else
            Response.Write((new { itemID = item.Attribute("id").Value, itemSubject = item.Attribute("subject").Value, itemText = item.Value, author = item.Attribute("postedBy").Value, itemPosted = DateTime.Parse(item.Attribute("posted").Value).ToString("MMM dd yyyy"), itemCompleted = item.Attribute("completed").Value }).ToJSON());            
        Response.Write("]");
    }

    private void UpdateItemDetails(string itemID, string subject, string text)
    {
        XDocument document = XDocument.Load(DataPath);
        XElement item = (from data in document.Descendants("item") where data.Attribute("id").Value == itemID select data).SingleOrDefault();
        item.Attribute("subject").Value = subject;
        item.Value = text;
        document.Save(DataPath);
    }
}