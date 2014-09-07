<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ChangeKingdom.aspx.cs" Inherits="admin_ChangeKingdom" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server"><br /><br />
<b><asp:Label ID="message" runat="server"></asp:Label></b>
<br /><br />
Change my province starting kingdom to this <asp:TextBox ID="txtKingdomID" runat="server"></asp:TextBox> guid.
<br />
<asp:Button ID="save" runat="server" OnClick="btnSave_Click" Text="Update" />
<br /><br />
Recently used kingdoms:<br />
<asp:Literal ID="recentlyKingdomIds" runat="server"></asp:Literal>
</asp:Content>

