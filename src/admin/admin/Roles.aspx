<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Admin.master" AutoEventWireup="true"
    CodeFile="Roles.aspx.cs" Inherits="admin_Roles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Scripts>
            <asp:ScriptReference Path="http://codingforcharity.org/libs/jquery/jquery-1.3.2.min.js" />
            <asp:ScriptReference Path="http://codingforcharity.org/libs/jquery/jquery.tableSorter.js" />
        </Scripts>
    </asp:ScriptManagerProxy>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
            <ContentTemplate>
                Create a new Role:
                <asp:TextBox ID="tbNewRole" runat="server"></asp:TextBox>
                <asp:Button ID="btnNewRole" runat="server" Text="Create Role" OnClick="btnNewRole_Click" />
                <asp:Label ID="lblWarning" runat="server"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="divRolesColumns">
        Roles
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView ID="gvRoles" OnRowDeleting="gvRoles_RowDeleting" CssClass="GridViewDefault"
                    OnSelectedIndexChanged="gvRoles_SelectedIndexChanged" AutoGenerateColumns="false"
                    DataKeyNames="Role" runat="server">
                    <Columns>
                        <asp:CommandField ShowSelectButton="true" />
                        <asp:BoundField DataField="Role" HeaderText="Role" />
                        <asp:BoundField DataField="Count" HeaderText="Count" />
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbDelete" runat="server" CausesValidation="false" CommandName="Delete"
                                    OnClientClick="return confirm('Are you sure?')" Text="Delete"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle CssClass="GridviewFooterStyle" />
                    <RowStyle CssClass="GridviewRowStyle" />
                    <EditRowStyle CssClass="GridviewEditRowStyle" />
                    <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                    <PagerStyle CssClass="GridviewPagerStyle" />
                    <HeaderStyle CssClass="GridviewHeaderStyle" />
                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnNewRole" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="divRolesColumns">
        Users In Role
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gvUsers" AutoGenerateColumns="false" CssClass="GridViewDefault"
                    OnRowDeleting="gvUsers_RowDeleting" runat="server">
                    <Columns>
                        <asp:BoundField DataField="Role" HeaderText="Users" />
                        <asp:TemplateField HeaderText="Email">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlemail" Text='<%# Bind("Email") %>' NavigateUrl='<%# Bind("Email","MAILTO:{0}")%>'
                                    runat="server"></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbDelete" runat="server" CausesValidation="false" CommandName="Delete"
                                    OnClientClick="return confirm('Are you sure?')" Text="Remove"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle CssClass="GridviewFooterStyle" />
                    <RowStyle CssClass="GridviewRowStyle" />
                    <EditRowStyle CssClass="GridviewEditRowStyle" />
                    <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                    <PagerStyle CssClass="GridviewPagerStyle" />
                    <HeaderStyle CssClass="GridviewHeaderStyle" />
                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="divRolesColumns">
        Users Not in Role
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gvUsersAdd" CssClass="GridViewDefault" OnPageIndexChanging="gvUsersAdd_PageIndexChanging"
                    AllowSorting="false" AllowPaging="true" PageSize="20" AutoGenerateColumns="false"
                    OnRowDeleting="gvUsersAdd_RowDeleting" runat="server">
                    <Columns>
                        <asp:CommandField ShowDeleteButton="true" DeleteText="Add" />
                        <asp:BoundField DataField="userName" HeaderText="Users" />
                        <asp:TemplateField HeaderText="Email">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlemail" Text='<%# Bind("email") %>' NavigateUrl='<%# Bind("email","MAILTO:{0}")%>'
                                    runat="server"></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle CssClass="GridviewFooterStyle" />
                    <RowStyle CssClass="GridviewRowStyle" />
                    <EditRowStyle CssClass="GridviewEditRowStyle" />
                    <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                    <PagerStyle CssClass="GridviewPagerStyle" />
                    <HeaderStyle CssClass="GridviewHeaderStyle" />
                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:Literal ID="ltJavascriptInject" runat="server"></asp:Literal>
</asp:Content>
