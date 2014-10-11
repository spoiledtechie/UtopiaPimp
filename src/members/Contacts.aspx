<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    CodeFile="Contacts.aspx.cs" Inherits="members_Contacts" %>

<%@ MasterType VirtualPath="~/main.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../controls/EditUserInfo.ascx" TagName="EditUserInfo" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/controls/ContactList.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
    <div id="jsTest" runat="server">
    </div>
    <br />
    <asp:Literal ID="ltInfo" runat="server"></asp:Literal>
    <br />
    <br />
    <div class="divKingSummary center" style="width: 600px;">
        <div class="borderBottom">
            Edit/Add Your User Info</div>
        <br />
        <uc1:EditUserInfo ID="EditUserInfo1" runat="server" />
    </div>
    <br />
    <br />
    <div>
        How to call internationally: <a target="_blank" href="http://www.timeanddate.com/worldclock/dialing.html">
            http://www.timeanddate.com/worldclock/dialing.html</a>
    </div>
    <div>
        Questions about your Privacy?:
        <asp:HyperLink ID="hlPrivacyLoggedIn" runat="server" NavigateUrl="~/anonymous/privacy.aspx">Privacy Policy</asp:HyperLink>
    </div>
</asp:Content>
