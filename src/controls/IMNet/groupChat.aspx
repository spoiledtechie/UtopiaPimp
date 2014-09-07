<%@ Page Language="C#" AutoEventWireup="true" CodeFile="groupChat.aspx.cs" Inherits="IM_groupChat" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <asp:Literal runat="server" ID="lblTitle"></asp:Literal></title>
    <link href="http://codingforcharity.org/controls/imnet/css/IM.css" rel="stylesheet"
        type="text/css" />
    <link href="http://codingforcharity.org/controls/imnet/wmd.css" rel="stylesheet"
        type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <script src="http://codingforcharity.org/utopiapimp/js/master.js?v=<%= SupportFramework.StaticContent.JavaScript.JsVersion %>"
        type="text/javascript" ></script>
    <script src="http://codingforcharity.org/controls/imnet/js/jquery-1.2.6.min.js" type="text/javascript"></script>
    <script src="http://codingforcharity.org/controls/imnet/js/jquery-timer.js" type="text/javascript" ></script>
    <script src="http://codingforcharity.org/controls/imnet/js/ChatGroupIM.js?v=<%= SupportFramework.StaticContent.JavaScript.JsVersion %>"
        type="text/javascript" ></script>
    <script src="http://codingforcharity.org/controls/imnet/js/utilities.js?v=<%= SupportFramework.StaticContent.JavaScript.JsVersion %>"
        type="text/javascript" ></script>
    <script src="http://codingforcharity.org/controls/imnet/showdown.js" type="text/javascript" ></script>
    <script src="http://codingforcharity.org/controls/imnet/wmd.js" type="text/javascript" ></script>
    <asp:ScriptManager ID="ScriptManager2" runat="server">
        <Services>
            <asp:ServiceReference Path="~/controls/IMNet/controls/Chat.asmx" />
        </Services>
    </asp:ScriptManager>
    <div class="groupChatBox">
        <div class="divChatBox">
            <div class="divChatBoxHistory" id="divChatHistory" runat="server">
            </div>
        </div>
        <div>
            <div id="wmd-editor" class="wmd-panel">
                <div id="wmd-button-bar" style="display: none;">
                </div>
                <textarea id="wmd-input"></textarea>
            </div>
            <div id="wmd-preview" class="wmd-panel">
            </div>
        </div>
    </div>
    <div class="groupChatList" id="divGroupChatList" runat="server">
    </div>
    <div id="divAddTop" class="divChatAd">
         
        <% if (Boomers.Utilities.Compare.CompareExt.getRandomTrueFalse())
         { %>
        <script type="text/javascript">
            getTopAd("true");
        </script>
        <%}
         else
         { %>
         <script type="text/javascript"><!--
             google_ad_client = "ca-pub-6494646249414123";
             /* PostSecretLeaderBoard */
             google_ad_slot = "1867020116";
             google_ad_width = 728;
             google_ad_height = 90;
//-->
</script>
<script type="text/javascript"
src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
</script>

        <%} %>
     
    </div>
    <asp:HiddenField ID="hfRecipient" runat="server" />
    <asp:HiddenField ID="hfSender" runat="server" />
    <asp:HiddenField ID="hfLastCheckedUID" runat="server" />
    <asp:HiddenField ID="hfNickName" runat="server" />
    </form>
    <script type="text/javascript">

        var _gaq = _gaq || [];
        _gaq.push(['_setAccount', 'UA-6812912-2']);
        _gaq.push(['_trackPageview']);

        (function () {
            var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
            ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
        })();

</script>
</body>
</html>
