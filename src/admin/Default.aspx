<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Default.aspx.cs"
    Inherits="admin_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
    </div>
    <ul>
        <li>
            <asp:HyperLink ID="hlUserAdmin" runat="server" NavigateUrl="~/admin/admin">General Info</asp:HyperLink></li>
        <li>
            <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/admin/UAdmin">Site Info</asp:HyperLink></li>
        <li>
            <asp:HyperLink ID="hlErrors" runat="server" NavigateUrl="~/admin/admin/Errors.aspx">Errors</asp:HyperLink></li>
            <li>
            <asp:HyperLink ID="HyperLink9" runat="server" NavigateUrl="~/admin/List.aspx">To-Do list</asp:HyperLink></li>
    </ul>
    <ul>
        <li>
        <asp:HyperLink ID="HyperLink10" runat="server" NavigateUrl="~/admin/ChangeKingdom.aspx">Change viewing kingdom (does not alter your own kingdom). Relog required to return to your own kingdom.</asp:HyperLink>        
        </li>
        <li>
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/admin/Errors.aspx">Code</asp:HyperLink></li>
            <li>
            <asp:HyperLink ID="HyperLink8" runat="server" NavigateUrl="~/admin/Encoder.aspx">Encoder</asp:HyperLink></li>
        <li>
            <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/admin/CETypes.aspx">CE Types</asp:HyperLink></li>
              <li>
            <asp:HyperLink ID="HyperLink7" runat="server" NavigateUrl="~/admin/Races.aspx">Races</asp:HyperLink></li>
        <li>
            <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="~/admin/Ops.aspx">Add Ops</asp:HyperLink></li>
        <li>
            <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/admin/Comments.aspx">View Comments</asp:HyperLink></li>
        <li>
            <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/admin/AdminNews.aspx">Admin News</asp:HyperLink></li>            
    </ul>
</asp:Content>
