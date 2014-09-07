<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Admin.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="admin_Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="divContent">
        <div>
            <asp:HyperLink ID="hlUsers" runat="server" NavigateUrl="~/admin/admin/Users.aspx">Users</asp:HyperLink></div>
        <ul class="ulList">
            <li>
                <asp:Label ID="lblUserCount" runat="server"></asp:Label>
                Registered Users</li>
            <li>
                <asp:Label ID="lblUsersOnline" runat="server"></asp:Label>
            </li>
            <li class="links">
               <a href="UsersLocked.aspx" ><asp:Label ID="lblLockedUsers" runat="server"></asp:Label>
                Locked Users</a></li>
            <li>
                <asp:Label ID="lblApprovedUsers" runat="server"></asp:Label>
                Approved Users</li>
            <li>
                <asp:Label ID="lblUnapprovedUsers" runat="server"></asp:Label>
                Un-Approved Users</li>
            <li>
                <br />
            </li>
            <li>
                <asp:Label ID="lblUsersToday" runat="server"></asp:Label>
                Users Today</li>
            <li>
                <asp:Label ID="lblLoggedInLately" runat="server"></asp:Label>
                logged in within 5 days</li>
            <li>
                <asp:Label ID="lblLoggedInWhile" runat="server"></asp:Label>
                logged in within 10 days</li>
            <li>
                <asp:Label ID="lblLoggedInMonth" runat="server"></asp:Label>
                logged in within 30 days</li>
        </ul>
    </div>
    <div class="divContent">
        <div>
            <asp:HyperLink ID="hlRoles" NavigateUrl="~/admin/admin/Roles.aspx" runat="server">Roles</asp:HyperLink></div>
        <ul class="ulList">
            <li>
                <asp:Label ID="lblRolesCount" runat="server"></asp:Label>
                Roles in the Application</li>
        </ul>
    </div>
   
    <div class="divContent">
        <div>
            <asp:HyperLink ID="hlErrors" NavigateUrl="~/admin/admin/Errors.aspx" runat="server">Errors</asp:HyperLink></div>
        <ul class="ulList">
            <li>
                <asp:Label ID="lblUnsolvedErrors" runat="server"></asp:Label>
                Unsolved Errors</li>
            <li>
                <asp:Label ID="lblTotalErrors" runat="server"></asp:Label>
                Total Errors </li>
            <li>
                <br />
            </li>
            <li>
                <asp:Label ID="lblSevenDayErrors" runat="server"></asp:Label>
                Errors in the last 7 days</li>
            <li>
                <asp:Label ID="lblYearErrors" runat="server"></asp:Label>
                Errors in the last 365 Days</li>
        </ul>
    </div>
    <div class="divContent">
        <div>
            <asp:HyperLink ID="HyperLink1" NavigateUrl="~/admin/admin/Views.aspx" runat="server">Views</asp:HyperLink></div>
        <ul class="ulList">
            <li>
                <asp:Label ID="lblPageViewsToday" runat="server"></asp:Label>
                Page Views Today</li>
            <li>
                <asp:Label ID="lblPageViewsFive" runat="server"></asp:Label>
                Page Views in 5 days</li>
            <li>
                <asp:Label ID="lblPageViewsTen" runat="server"></asp:Label>
                Page Views in 10 days</li>
            <li>
                <asp:Label ID="lblPageViewsMonth" runat="server"></asp:Label>
                Page Views in 30 days</li>
            <li>
                <asp:Label ID="lblPageViewsTotal" runat="server"></asp:Label>
                Total Page Views</li>
            <li>
                <br />
            </li>
            <li>
                <asp:Label ID="lblBrowserTypes" runat="server"></asp:Label>
                Browser Types</li>
            <li>
                <asp:Label ID="lblOSTypes" runat="server"></asp:Label>
                OS Types</li>
            <li>
                <asp:Label ID="lblJavaMonth" runat="server"></asp:Label></li>
        </ul>
    </div>
    <div class="divContent">
        <div>
            <asp:HyperLink ID="HyperLink2" NavigateUrl="~/admin/admin/Database.aspx" runat="server">Database</asp:HyperLink></div>
        <ul class="ulList">
            <li>DB Name:
                <asp:Label ID="lblDBName" runat="server"></asp:Label></li>
           <li>DB Size:
                <asp:Label ID="lblDBSize" runat="server"></asp:Label></li>
                 <li>DB Reserved:
                <asp:Label ID="lblDBReserved" runat="server"></asp:Label></li>
                 <li>DB Data:
                <asp:Label ID="lblDBData" runat="server"></asp:Label></li>
                 <li>DB Index Size:
                <asp:Label ID="lblDBIndexSize" runat="server"></asp:Label></li>
                 <li>DB UnUsed Space:
                <asp:Label ID="lblDBUnUsed" runat="server"></asp:Label></li>
        </ul>
        <ul class="ulList">
            <li>DB Name:
                <asp:Label ID="lblSiteDBName" runat="server"></asp:Label></li>
           <li>DB Size:
                <asp:Label ID="lblSiteDBSize" runat="server"></asp:Label></li>
                 <li>DB Reserved:
                <asp:Label ID="lblSiteDBReserved" runat="server"></asp:Label></li>
                 <li>DB Data:
                <asp:Label ID="lblSiteDBData" runat="server"></asp:Label></li>
                 <li>DB Index Size:
                <asp:Label ID="lblSiteDBIndexSize" runat="server"></asp:Label></li>
                 <li>DB UnUsed Space:
                <asp:Label ID="lblSiteDBUnUsed" runat="server"></asp:Label></li>
        </ul>
    </div> 
    <div class="divContent">
        <div>
           Application Commands</div>
        <ul class="ulList">
            <li>
                <asp:Button ID="btnRestartTheApp" runat="server" Text="Restart the Application" 
                    onclick="btnRestartTheApp_Click" />
            </li>
        </ul>
    </div>
    <div>
        <p>
            Last generated on:
            <asp:Label ID="TimeMsg" runat="server" />
    </div>
</asp:Content>
