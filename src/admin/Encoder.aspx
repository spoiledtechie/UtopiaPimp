<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Encoder.aspx.cs" Inherits="admin_Encoder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox><br />
    <asp:Button ID="btnEncode" runat="server" Text="Encode" 
        onclick="btnEncode_Click" />
    <asp:Button ID="btnDecode"
        runat="server" Text="Decode" onclick="btnDecode_Click" /><br />
    <br />
    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
</asp:Content>

