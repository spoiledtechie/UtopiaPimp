<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Admin.master" AutoEventWireup="true"
    CodeFile="Email.aspx.cs" ValidateRequest="false" Inherits="admin_admin_Email" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="divEmailItems">
        <div>
            Roles:</div>
        <asp:ListBox ID="lbRoles" runat="server" AppendDataBoundItems="true" SelectionMode="Multiple"
            Height="100px">
            <asp:ListItem Value="All" Text="All"></asp:ListItem>
        </asp:ListBox>
        <br />
        OR
        <br />
        <div>
            Email Username:</div>
        <asp:TextBox runat="server" ID="tbEmailUser"></asp:TextBox>
        <br />
        OR
        <br />
        <div>
            Email Email Address:</div>
        <asp:TextBox runat="server" ID="tbEmailAddress"></asp:TextBox>
    </div>
    <div class="divEmailItems">
        <div>
            Email Users:</div>
        <div>
            <asp:Label ID="lblTitle" runat="server" Text="Title:"></asp:Label><asp:TextBox ID="tbTitle"
                TabIndex="100" runat="server"></asp:TextBox></div>
        <div>
            <textarea id="taAddInfo" style="height: 300px; width: 600px;" runat="server" tabindex="101"></textarea>
            <div class="wmd-preview divAddGamePreview">
            </div>
        </div>
        <div>
            <asp:Button ID="btnSubmit" runat="server" Text="Submit Article" OnClick="btnSubmit_Click"
                TabIndex="102" /></div>
    </div>
    <div>
        <asp:Label runat="server" ID="lblWarning"></asp:Label></div>

    <script src="http://codingforcharity.org/controls/wmd/wmd.js" type="text/javascript"></script>

</asp:Content>
