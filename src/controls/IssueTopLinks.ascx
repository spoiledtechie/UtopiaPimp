<%@ Control Language="C#" AutoEventWireup="true" CodeFile="IssueTopLinks.ascx.cs"
    Inherits="controls_IssueTopLinks" %>
<ul class="ulWorkItemLinks">
    <li class="liWorkItemLinks">
        <asp:HyperLink ID="hlBasicView" runat="server" Text="Basic View" NavigateUrl="~/WorkItem/List.aspx"></asp:HyperLink>
        <asp:Label ID="lblBasicView" runat="server" CssClass="Bold" Text="Basic View"></asp:Label>
    </li>
    <li class="liWorkItemLinks" visible="false" runat="server">|</li>
    <li class="liWorkItemLinks" visible="false" runat="server">
        <asp:HyperLink ID="hlAdvancedView" runat="server" Text="Advanced View" NavigateUrl="~/WorkItem/AdvancedList.aspx"></asp:HyperLink><asp:Label
            ID="lblAdvancedView" runat="server" CssClass="Bold" Text="Advanced View"></asp:Label>
    </li>
    <li class="liWorkItemLinks">|</li>
    <li class="liWorkItemLinks">
        <asp:HyperLink ID="hlCreateItem" runat="server" Text="Create Item" NavigateUrl="~/WorkItem/CreateItem.aspx"></asp:HyperLink><asp:Label
            ID="lblCreateItem" runat="server" CssClass="Bold" Text="Create Item"></asp:Label>
    </li>
</ul>
