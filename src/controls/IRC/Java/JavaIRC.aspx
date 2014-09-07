<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JavaIRC.aspx.cs" Inherits="controls_IRC_Java_JavaIRC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>UtopiaPimp's Java IRC</title>
    <asp:Literal runat="server" ID="ltCSS"></asp:Literal>
</head>
<body>
    <script src="http://codingforcharity.org/utopiapimp/js/Master.js?v=<%= SupportFramework.StaticContent.JavaScript.JsVersion %>"
        type="text/javascript" />
    <applet name="applet" code="IRCApplet.class" archive="irc.jar,pixx.jar" width="640"
        height="450">
        <param name="CABINETS" value="irc.cab,securedirc.cab,pixx.cab">
        <asp:Literal ID="ltApplet" runat="server"></asp:Literal>
        <param name="fullname" value="UtopiaPimp.com User">
        <param name="host" value="irc.utonet.org">
        <param name="gui" value="pixx">

        <param name="style:bitmapsmileys" value="true">
        <param name="style:smiley1" value=":) img/sourire.gif">
        <param name="style:smiley2" value=":-) img/sourire.gif">
        <param name="style:smiley3" value=":-D img/content.gif">
        <param name="style:smiley4" value=":d img/content.gif">
        <param name="style:smiley5" value=":-O img/OH-2.gif">
        <param name="style:smiley6" value=":o img/OH-1.gif">
        <param name="style:smiley7" value=":-P img/langue.gif">
        <param name="style:smiley8" value=":p img/langue.gif">
        <param name="style:smiley9" value=";-) img/clin-oeuil.gif">
        <param name="style:smiley10" value=";) img/clin-oeuil.gif">
        <param name="style:smiley11" value=":-( img/triste.gif">
        <param name="style:smiley12" value=":( img/triste.gif">
        <param name="style:smiley13" value=":-| img/OH-3.gif">
        <param name="style:smiley14" value=":| img/OH-3.gif">
        <param name="style:smiley15" value=":'( img/pleure.gif">
        <param name="style:smiley16" value=":$ img/rouge.gif">
        <param name="style:smiley17" value=":-$ img/rouge.gif">
        <param name="style:smiley18" value="(H) img/cool.gif">
        <param name="style:smiley19" value="(h) img/cool.gif">
        <param name="style:smiley20" value=":-@ img/enerve1.gif">
        <param name="style:smiley21" value=":@ img/enerve2.gif">
        <param name="style:smiley22" value=":-S img/roll-eyes.gif">
        <param name="style:smiley23" value=":s img/roll-eyes.gif">
    </applet>
   
    <form id="form1" runat="server">
    <div>
        <input type="BUTTON" value="Identify Nick" onclick="document.applet.setFieldText(document.applet.getFieldText()+'/nickserv identify ');document.applet.requestSourceFocus()">
        <input type="BUTTON" value="Join Channel" onclick="document.applet.setFieldText(document.applet.getFieldText()+'/j #');document.applet.requestSourceFocus()">
 How To: <a href="http://en.wikipedia.org/wiki/List_of_Internet_Relay_Chat_commands" target="_blank">List of
            IRC Commands</a> - <a href="http://en.wikipedia.org/wiki/List_of_Internet_Relay_Chat_commands#JOIN" target="_blank">
                Join a Channel</a>
    </div>
    <div class="center">
        <table class="center noFluff">
            <tr>
                <td>
                    <div id="tdAdd1">
                        <script type="text/javascript">
                            getButtonAd("tdAdd1");
                        </script>
                    </div>
                </td>
                <td>
                    <div id="tdAdd2">
                        <script type="text/javascript">
                            getButtonAd("tdAdd2");
                        </script>
                    </div>
                </td>
                <td>
                    <div id="tdAdd3">
                        <script type="text/javascript">
                            getButtonAd("tdAdd3");
                        </script>
                    </div>
                </td>
                <td>
                    <div id="tdAdd4">
                        <script type="text/javascript">
                            getButtonAd("tdAdd4");
                        </script>
                    </div>
                </td>
                <td>
                    <div id="tdAdd5">
                        <script type="text/javascript">
                            getButtonAd("tdAdd5");
                        </script>
                    </div>
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
