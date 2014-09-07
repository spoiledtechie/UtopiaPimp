<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    CodeFile="Columns2.aspx.cs" Inherits="members_Columns2" %>

<%@ MasterType VirtualPath="~/main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManagerProxy ID="sm" runat="server">
        <Services>
            <asp:ServiceReference Path="~/controls/ColumnChooser.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
    <div id="divColumnSets">
        <center>
            <img src="http://codingforcharity.org/utopiapimp/img/Loading.gif" alt="Loading..."
                width="50px" /></center>
    </div>
    <div id="divChooseColumns">
    </div>
    <div id="divColumnsChosen">
    </div>
    <div id="divExtra" runat="server">
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            LoadFirstColumnSets();
        });
    </script>
    <div class="divAdRight" id="divAdRight">
               
        <% if (Boomers.Utilities.Compare.CompareExt.getRandomTrueFalse())
         { %>
        <script type="text/javascript">
            getSideAd();
        </script>
        <%}
         else
         { %>
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

        <%} %>
        
    </div>
</asp:Content>
