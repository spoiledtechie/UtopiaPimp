<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Admin.master" AutoEventWireup="true"
    CodeFile="Users.aspx.cs" Inherits="admin_Users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <ul class="ulList ulListSide">
                      <li>Username:
                <asp:TextBox ID="tbUsernameSearch" runat="server"></asp:TextBox>
                <asp:Button ID="btnSearchUserName" runat="server" Text=">" 
                    onclick="btnSearchUserName_Click" /></li>
            <li>Email:
                <asp:TextBox ID="tbEmailSearch" runat="server"></asp:TextBox>
                <asp:Button ID="btnEmailSearch" runat="server" Text=">" 
                    onclick="btnEmailSearch_Click" /></li>
                    <li>WildCard: %</li>
                   
                    </ul>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="divUsersTable">
                <div class="divUserColumnsTitle">
                    <asp:Label ID="lblUserCount" runat="server"></asp:Label>
                    Registered Users</div>
                <asp:GridView ID="gvUsers" CssClass="GridviewFull" runat="server" PageSize="25" AllowPaging="true"
                    DataKeyNames="userName" OnRowCancelingEdit="gvUsers_RowCancelingEdit" OnRowEditing="gvUsers_RowEditing"
                    OnRowUpdating="gvUsers_RowUpdating" OnRowCommand="gvUsers_RowCommand" OnSelectedIndexChanged="gvUsers_SelectedIndexChanged"
                    OnPageIndexChanging="gvUsers_PageIndexChanging" AutoGenerateColumns="false">
                    <Columns>
                        <asp:CommandField ShowSelectButton="true" ShowEditButton="true" />
                        <asp:BoundField DataField="userName" HeaderText="User" ReadOnly="true" />
                        <asp:TemplateField HeaderText="Email">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlemail" Text='<%# Bind("email") %>' NavigateUrl='<%# Bind("email","MAILTO:{0}")%>'
                                    runat="server"></asp:HyperLink>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbEmail" runat="server" Text='<%# Bind("email") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="passwordQuestion" HeaderText="Pass Quest" ReadOnly="true" />
                        <asp:TemplateField HeaderText="Comments">
                            <ItemTemplate>
                                <%# Eval("comments") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbComments" runat="server" Text='<%# Bind("comments") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:CheckBoxField DataField="userOnline" HeaderText="Online" ReadOnly="true" />
                        <asp:BoundField DataField="createDate" HeaderText="Created" ReadOnly="true" />
                        <asp:BoundField DataField="lastLogin" HeaderText="Last Login" ReadOnly="true" />
                        <asp:ButtonField DataTextField="approved" HeaderText="Approve" CommandName="ApproveUser" />
                        <asp:ButtonField DataTextField="locked" HeaderText="Locked" CommandName="UnLockUser" />
                        <asp:BoundField DataField="userErrors" HeaderText="Errors" ReadOnly="true" />
                        <asp:BoundField DataField="userPageViews" HeaderText="Page Views" ReadOnly="true" />
                        <asp:BoundField DataField="userName" HeaderText="User" ReadOnly="true" />
                    </Columns>
                    <FooterStyle CssClass="GridviewFooterStyle" />
                    <RowStyle CssClass="GridviewRowStyle" />
                    <EditRowStyle CssClass="GridviewEditRowStyle" />
                    <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                    <PagerStyle CssClass="GridviewPagerStyle" />
                    <HeaderStyle CssClass="GridviewHeaderStyle" />
                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                </asp:GridView>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSearchUserName" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnEmailSearch" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <div>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="divUserColumns">
                    <div class="divUserColumnsTitle">
                        Reset User Password</div>
                    <asp:Label ID="lblResetPassUser" runat="server"></asp:Label><br />
                    <asp:Label ID="lblResetPassword" runat="server"></asp:Label><br />
                    <asp:Button ID="btnResetPassword" runat="server" Text="Reset Password" OnClick="btnResetPassword_Click" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvUsers" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="divUserColumns">
                    <div class="divUserColumnsTitle">
                        Roles for User
                    </div>
                    <asp:GridView ID="gvRolesIn" runat="server" CssClass="GridViewDefault" DataKeyNames="role"
                        AutoGenerateColumns="false" OnRowDeleting="gvRolesIn_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="role" HeaderText="Role" />
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
                </div>
                <div class="divUserColumns">
                    <div class="divUserColumnsTitle">
                        Roles User is not in
                    </div>
                    <asp:GridView ID="gvRolesNotIn" runat="server" CssClass="GridViewDefault" DataKeyNames="role"
                        AutoGenerateColumns="false" OnRowCommand="gvRolesNotIn_RowCommand">
                        <Columns>
                            <asp:ButtonField CommandName="AddRole" Text="Add" />
                            <asp:BoundField DataField="role" HeaderText="Role" />
                        </Columns>
                        <FooterStyle CssClass="GridviewFooterStyle" />
                        <RowStyle CssClass="GridviewRowStyle" />
                        <EditRowStyle CssClass="GridviewEditRowStyle" />
                        <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                        <PagerStyle CssClass="GridviewPagerStyle" />
                        <HeaderStyle CssClass="GridviewHeaderStyle" />
                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvUsers" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
