<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Mibbit.aspx.cs" Inherits="controls_IRC_Mibbit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>IRC for Utopiapimp.com</title>
    <asp:Literal runat="server" ID="ltCSS"></asp:Literal>
</head>
<body>
    <form id="form1" runat="server">
    <%-- <script src="../../js/Master.js" type="text/javascript"></script>--%>
    <script src="http://codingforcharity.org/utopiapimp/js/Master.js?v=<%= SupportFramework.StaticContent.JavaScript.JsVersion %>"
        type="text/javascript" />
       
    <script type="text/javascript">
        function changeHeight(iframe) {
            try {
                var innerDoc = (iframe.contentDocument) ? iframe.contentDocument : iframe.contentWindow.document;
                if (innerDoc.body.offsetHeight) //ns6 syntax
                {
                    iframe.height = innerDoc.body.offsetHeight + 32; //Extra height FireFox
                }
                else if (iframe.Document && iframe.Document.body.scrollHeight) //ie5+ syntax
                {
                    iframe.height = iframe.Document.body.scrollHeight;
                }
            }
            catch (err) {
                alert(err.message);
            }
        }
    </script>
     <asp:Literal ID="ltChat" runat="server"></asp:Literal>
     How To: <a href="http://en.wikipedia.org/wiki/List_of_Internet_Relay_Chat_commands" target="_blank">List of
            IRC Commands</a> - <a href="http://en.wikipedia.org/wiki/List_of_Internet_Relay_Chat_commands#JOIN" target="_blank">
                Join a Channel</a>
    <div class="center">
        <table class="center noFluff">
            <tr>
                <td ><div id="tdAdd1">
                    <script type="text/javascript">
                        getButtonAd("tdAdd1");
                    </script></div>
                </td>
                <td><div id="tdAdd2">
                    <script type="text/javascript">
                        getButtonAd("tdAdd2");
                    </script></div>
                </td>
                <td><div id="tdAdd3">
                    <script type="text/javascript">
                        getButtonAd("tdAdd3");
                    </script></div>
                </td>
                <td><div id="tdAdd4">
                    <script type="text/javascript">
                        getButtonAd("tdAdd4");
                    </script></div>
                </td>
                <td><div id="tdAdd5">
                    <script type="text/javascript">
                        getButtonAd("tdAdd5");
                    </script></div>
                </td>
            </tr>
        </table>
    </div>
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
    </form>
</body>
</html>
