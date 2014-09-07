<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Comments.aspx.cs"
    Inherits="admin_Comments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gvViewComments" runat="server" AutoGenerateColumns="False" DataKeyNames="uid"
                DataSourceID="SQLCommentView" OnRowCommand="gvViewComments_RowCommand"
                >
                <Columns>
                    <asp:BoundField DataField="uid" HeaderText="uid" InsertVisible="False" Visible="false"
                        ReadOnly="True" SortExpression="uid" />
                    <asp:ButtonField ButtonType="Button" CommandName="cmdReviewed" Text="Reviewed" />
                    <asp:BoundField DataField="User_ID" HeaderText="User_ID" SortExpression="User_ID" />
                    <asp:BoundField DataField="Date_Time" HeaderText="Date_Time" SortExpression="Date_Time" />
                    <asp:BoundField DataField="Comment" HeaderText="Comment" SortExpression="Comment" />
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
            <asp:AsyncPostBackTrigger ControlID="gvViewComments" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:SqlDataSource ID="SQLCommentView" runat="server" ConnectionString="<%$ ConnectionStrings:UPConnectionString %>"
        SelectCommand="SELECT [uid], [User_ID], [Comment], [Date_Time] FROM [Utopia_Comments] WHERE ([Reviewed] = @Reviewed)"
        DeleteCommand="DELETE FROM [Utopia_Comments] WHERE [uid] = @uid" InsertCommand="INSERT INTO [Utopia_Comments] ([User_ID], [Comment], [Date_Time]) VALUES (@User_ID, @Comment, @Date_Time)"
        UpdateCommand="UPDATE [Utopia_Comments] SET [User_ID] = @User_ID, [Comment] = @Comment, [Date_Time] = @Date_Time WHERE [uid] = @uid">
        <SelectParameters>
            <asp:Parameter DefaultValue="0" Name="Reviewed" Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="uid" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="User_ID" Type="Object" />
            <asp:Parameter Name="Comment" Type="String" />
            <asp:Parameter Name="Date_Time" Type="DateTime" />
            <asp:Parameter Name="uid" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="User_ID" Type="Object" />
            <asp:Parameter Name="Comment" Type="String" />
            <asp:Parameter Name="Date_Time" Type="DateTime" />
        </InsertParameters>
    </asp:SqlDataSource>
</asp:Content>
