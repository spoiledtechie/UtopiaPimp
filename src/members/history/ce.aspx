<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ce.aspx.cs" Inherits="members_history_ce" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <asp:Literal runat="server" ID="ltCSS"></asp:Literal>
</head>
<body>
    <form id="form1" runat="server">
    <script src="http://codingforcharity.org/libs/jquery/jquery-1.3.2.min.js" type="text/javascript" ></script>
    <script src="http://codingforcharity.org/utopiapimp/js/jquery-ui-1.7.2.custom.min.js"
        type="text/javascript" ></script>
    <script src="http://codingforcharity.org/utopiapimp/js/getElementsByClassName-1.0.1.js"
        type="text/javascript" ></script>
    <script src="http://codingforcharity.org/utopiapimp/js/master.js?v=<%= SupportFramework.StaticContent.JavaScript.JsVersion %>"
        type="text/javascript" ></script>
    <asp:ScriptManager ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/controls/CEChooser.asmx" />
        </Services>
    </asp:ScriptManager>
    <br />
    <div>
        <asp:Literal ID="ltYears" runat="server"></asp:Literal>
        <asp:Literal ID="ltMonths" runat="server"></asp:Literal>
    </div>
    <asp:Literal ID="ltText" runat="server"></asp:Literal>
    <script type="text/javascript">
        $(function () {
            // Dialog			
            $('#dialog').dialog({
                autoOpen: false,
                width: 600,
                buttons: {
                    "Ok": function () {
                        $(this).dialog("close");
                    }
                }
            });
        });
    </script>
    <div id="dialog" title="Dialog Title">
    </div>
    <div class="divAdRight" id="divAdRight">
       
         <script type="text/javascript"><!--
             google_ad_client = "ca-pub-6494646249414123";
             /* PostSecretSkyScraper */
             google_ad_slot = "6985393558";
             google_ad_width = 160;
             google_ad_height = 600;
//-->
</script>
<script type="text/javascript"
src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
</script>

       
    </div>
    <div class="divFooter" id="divAddTop">
        <br />
        <br />
        <br />
      
      
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

     
    </div>
    </form><script type="text/javascript">

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
