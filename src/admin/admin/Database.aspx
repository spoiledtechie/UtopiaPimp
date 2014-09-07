<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Admin.master" AutoEventWireup="true"
    CodeFile="Database.aspx.cs" Inherits="admin_admin_Database" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        $(document).ready(
        function() {
            $.tablesorter.addParser({
                id: "fancyNumber",
                is: function(s) {
                    return /^[0-9]?[0-9,\.]*$/.test(s);
                },
                format: function(s) {
                    return $.tablesorter.formatFloat(s.replace(/,/g, ''));
                },
                type: "numeric"
            });
            // call the tablesorter plugin, the magic happens in the markup
            $("#tableInfoSite").tablesorter({ widgets: ['zebra'], widgetZebra: { css: ['d0', 'd1']} });
            $("#tableInfo").tablesorter({ widgets: ['zebra'], widgetZebra: { css: ['d0', 'd1']} });
        });

    </script>

    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Scripts>
            <asp:ScriptReference Path="http://codingforcharity.org/libs/jquery/jquery-1.3.2.min.js" />
            <asp:ScriptReference Path="http://codingforcharity.org/libs/jquery/jquery.metadata.js" />
            <asp:ScriptReference Path="http://codingforcharity.org/libs/jquery/jquery.tablesorter.min.js" />
            <asp:ScriptReference Path="http://codingforcharity.org/admin/js/admin.js" />
        </Scripts>
        <Services>
            <asp:ServiceReference Path="~/admin/admin/controls/admin.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
    <div class="divRolesColumns">
        <div class="divContent">
            <div>
                <asp:HyperLink ID="HyperLink2" NavigateUrl="~/admin/admin/Database.aspx" runat="server">Membership Database</asp:HyperLink></div>
            <ul class="ulList">
                <li>DB Name:
                    <asp:Label ID="lblDBName" runat="server"></asp:Label></li>
                <li>Total Rows:
                    <asp:Label ID="lbltblRows" runat="server"></asp:Label></li>
                <li>DB Reserved:
                    <asp:Label ID="lblDBReserved" runat="server"></asp:Label></li>
                <li>DB Size:
                    <asp:Label ID="lblDBSize" runat="server"></asp:Label></li>
                <li>DB Data:
                    <asp:Label ID="lblDBData" runat="server"></asp:Label></li>
                <li>DB Index Size:
                    <asp:Label ID="lblDBIndexSize" runat="server"></asp:Label></li>
                <li>DB UnUsed Space:
                    <asp:Label ID="lblDBUnUsed" runat="server"></asp:Label></li>
                <li>Total Tables:
                    <asp:Label ID="lblTotalTables" runat="server"></asp:Label></li>
            </ul>
        </div>
        <asp:Literal ID="ltMemInformation" runat="server"></asp:Literal>
    </div>
    <div class="divRolesColumns">
        <div class="divContent">
            <div>
                <asp:HyperLink ID="HyperLink1" NavigateUrl="~/admin/admin/Database.aspx" runat="server">Site Database</asp:HyperLink></div>
            <ul class="ulList">
                <li>DB Name:
                    <asp:Label ID="lblSiteDBName" runat="server"></asp:Label></li>
                <li>Total Rows:
                    <asp:Label ID="lblSitetblRows" runat="server"></asp:Label></li>
                <li>DB Reserved:
                    <asp:Label ID="lblSiteDBReserved" runat="server"></asp:Label></li>
                <li>DB Size:
                    <asp:Label ID="lblSiteDBSize" runat="server"></asp:Label></li>
                <li>DB Data:
                    <asp:Label ID="lblSiteDBData" runat="server"></asp:Label></li>
                <li>DB Index Size:
                    <asp:Label ID="lblSiteDBIndexSize" runat="server"></asp:Label></li>
                <li>DB UnUsed Space:
                    <asp:Label ID="lblSiteDBUnUsed" runat="server"></asp:Label></li>
                <li>Total Tables:
                    <asp:Label ID="lblSiteTotalTables" runat="server"></asp:Label></li>
            </ul>
        </div>
        <asp:Literal ID="ltSiteInformation" runat="server"></asp:Literal>
    </div>
    <asp:Literal ID="ltGeneralInfo" runat="server"></asp:Literal>
</asp:Content>
