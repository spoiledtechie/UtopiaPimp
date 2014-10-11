<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Profile.aspx.cs"
    Inherits="members_Profile" %>
    <%@ MasterType VirtualPath="~/main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="pnlContent">
        <div class="pnls">
            <div class="pnldivHeaders">
                ID numbers</div>
            <asp:Literal ID="idNumbers" runat="server"></asp:Literal>
        </div>
        <br />
        <br />
        <br />
        <div class="pnls">
            <div class="pnldivHeaders">
                Current Theme:
                <asp:Label ID="lblCurrentTheme" runat="server" Text="Default"></asp:Label>
            </div>
            <asp:DropDownList ID="ddlChangeTheme" EnableViewState="true" runat="server" CssClass="AllDropDowns"
                AutoPostBack="True" OnSelectedIndexChanged="ddlChangeTheme_SelectedIndexChanged">
                <asp:ListItem>Default</asp:ListItem>
                <asp:ListItem>Black</asp:ListItem>
                <asp:ListItem>Classic</asp:ListItem>
                <asp:ListItem>Desert Sands</asp:ListItem>
                <asp:ListItem>Emerald</asp:ListItem>
                <asp:ListItem>Ghetto Sunset</asp:ListItem>
                <asp:ListItem>Tropic Breeze</asp:ListItem>
            </asp:DropDownList>
            <a href="http://getsatisfaction.com/utopiapimp/topics/theme30" target="_blank">Suggest
                a Theme</a>
        </div>
        <br />
        <br />
        <br />
        <div class="pnls">
            <div class="pnldivHeaders">
                Change Password</div>
            <asp:ChangePassword ID="ChangePassword1" runat="server">
            </asp:ChangePassword>
        </div>
        <br />
        <br />
        <div class="pnls">
            <div class="pnldivHeaders">
                Change Your UserName</div>
            <div class="pnldivContent">
                <asp:TextBox ID="tbChangeUserName" runat="server"></asp:TextBox><br />
                <asp:Button ID="btnChangeUserName" runat="server" Text="Change your User Name" OnClick="btnChangeUserName_Click" /><br />
                <asp:Label ID="lblChangeUserName" runat="server"></asp:Label>
            </div>
        </div>
        <br />
        <br />
        <div class="pnls">
            <div class="pnldivHeaders">
                Change Your Email</div>
            <div>
                <asp:TextBox ID="tbChangeEmail" runat="server"></asp:TextBox><br />
                <asp:Button ID="btnChangeEmail" runat="server" Text="Change your Email" OnClick="btnChangeEmail_Click" /><br />
                <asp:Label ID="lblChangeEmail" runat="server"></asp:Label>
            </div>
        </div>
        <br />
        <br />
        <div class="pnls">
            <div class="pnldivHeaders">
                Change Your Question and Answer</div>
            <div>
                <ul>
                    <li>Password:
                        <asp:TextBox ID="tbPassword" runat="server" TextMode="Password"></asp:TextBox></li>
                    <li>Question:
                        <asp:TextBox ID="tbQuestion" runat="server"></asp:TextBox></li>
                    <li>Answer:
                        <asp:TextBox ID="tbAnswer" runat="server"></asp:TextBox></li></ul>
                <br />
                <asp:Button ID="btnQuestionAnswer" runat="server" Text="Change your Question and Answer"
                    OnClick="btnChangeQuestion_Click" /><br />
                <asp:Label ID="lblQuestion" runat="server"></asp:Label>
            </div>
        </div>
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
