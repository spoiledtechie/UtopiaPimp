<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    CodeFile="CE.aspx.cs" Inherits="members_CE" %>

<%@ MasterType VirtualPath="~/main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
           <Services>
            <asp:ServiceReference Path="~/controls/CEChooser.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
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
                width: 500,
                buttons: {
                    "Ok": function () {
                        $(this).dialog("close");
                    }
                }
            });
        });

    </script>
    <div id="dialog" title="Uniques">
    </div>
    <div class="divAdRight2" id="divAdRight">
                   
   
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
</asp:Content>
