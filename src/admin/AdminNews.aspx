<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="AdminNews.aspx.cs"
    Inherits="admin_AdminNews" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <div>
            <asp:Label ID="lblTitle" runat="server" Text="Title:"></asp:Label><asp:TextBox ID="txtbxTitle"
                runat="server"></asp:TextBox></div>
        <div>
            <textarea id="taAddInfo" style="height:400px; width:600px;" runat="server" tabindex="101"></textarea>
            <div class="wmd-preview divAddGamePreview">
            </div>
        </div>
        <div>
            <asp:Button ID="btnSubmit" runat="server" Text="Submit Article" OnClick="btnSubmit_Click" /></div>
    </div>
    <div>
        <asp:DataList ID="dlAdminNews" runat="server" DataSourceID="sdsAdminNews">
            <ItemTemplate>
                Title:
                <asp:Label ID="TitleLabel" runat="server" Text='<%# Eval("Title") %>' />
                <br />
                Body:<asp:Literal ID="Literal1" Text='<%# Eval("Body") %>' runat="server"></asp:Literal>
                                <br />
                Time Stamp:
                <asp:Label ID="TimeStampLabel" runat="server" Text='<%# Eval("TimeStamp") %>' />
                <br />
                User Name:
                <asp:Label ID="User_ID_AddedLabel" runat="server" Text='<%# SupportFramework.Users.Memberships.getUserName(new Guid( Eval("User_ID_Added").ToString())) %>' />
                <hr />
                <br />
            </ItemTemplate>
        </asp:DataList>
        <asp:SqlDataSource ID="sdsAdminNews" runat="server" ConnectionString="<%$ ConnectionStrings:UPConnectionString %>"
            SelectCommand="SELECT TOP (5) uid, TimeStamp, Title, Body, User_ID_Added FROM Utopia_News ORDER BY uid DESC">
        </asp:SqlDataSource>
    </div>

    <script src="http://codingforcharity.org/controls/wmd/wmd.js" type="text/javascript"></script>

</asp:Content>
