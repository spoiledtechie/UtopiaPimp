<%@ Page Title="" Language="C#" MasterPageFile="~/admin/UAdmin/UAdmin.master" AutoEventWireup="true"
    CodeFile="Provinces.aspx.cs" Inherits="admin_UAdmin_Provinces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <ul>
            <li>UserName:
                <asp:TextBox ID="tbUsername" runat="server"></asp:TextBox>
                <asp:Button ID="btnUserName"
                    runat="server" Text="Get Provinces" onclick="btnUserName_Click" /></li></ul>
    </div>
    <div id="divProvinces" runat="server">
    </div>
</asp:Content>
