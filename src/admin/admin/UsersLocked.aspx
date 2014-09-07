<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Admin.master" AutoEventWireup="true"
    CodeFile="UsersLocked.aspx.cs" Inherits="admin_admin_UsersLocked" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="divUsersTable">
                <div class="divUserColumnsTitle">
                    <asp:Label ID="lblUserCount" runat="server"></asp:Label>
                    Locked Users</div>
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
        <div class="divUserColumns">
            <div class="divUserColumnsTitle">
                Unlock All Users</div>
            <asp:Button ID="btnUnlockUsers" runat="server" Text="Unlock Users" 
                onclick="btnUnlockUsers_Click" />
        </div>
    </div>
</asp:Content>
