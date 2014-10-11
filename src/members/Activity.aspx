<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    CodeFile="Activity.aspx.cs" Inherits="members_Activity" %>

<%@ MasterType VirtualPath="~/main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/controls/ActivityLog.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
    <br />
    <div class="Title">
        Activity logs against
        <asp:Label ID="lblActLog" runat="server"></asp:Label></div>
    <br />
    <div style="margin-right: 150px;">
        <div class="divTitles Bold">
            Instructions & Info</div>
        Below is a list of the number of intelligence posts and ops performed for each of
        your kingdom mates against the kingdom of
        <asp:Label ID="lblActLog1" runat="server"></asp:Label>. The op numbers are determined
        by counting the actual number of ops done, even if several ops are pasted into a
        single post by a user. You can click on a province's name to see a detailed list
        of the ops they have performed.
    </div>
    <asp:Literal ID="ltActivityLog" runat="server"></asp:Literal>
    <br />
    <asp:Label ID="lblTimes" Style="margin-right: 150px;" runat="server"></asp:Label>
    <br />
    <div id="divOps">
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
</asp:Content>
