<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="CETypes.aspx.cs" Inherits="admin_CETypes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            
            <asp:GridView ID="gvViewOps" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                DataKeyNames="uid" DataSourceID="sdsViewOps">
                <Columns>
                    <asp:CommandField ShowEditButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="uid" HeaderText="uid" InsertVisible="False" ReadOnly="True"
                        SortExpression="uid" />
                    <asp:BoundField DataField="CE_Type" HeaderText="CE Type" SortExpression="CE_Type" />
                    <asp:CommandField ShowDeleteButton="True" />
                </Columns>
                <FooterStyle CssClass="GridviewFooterStyle" />
                <RowStyle CssClass="GridviewRowStyle" />
                <EditRowStyle CssClass="GridviewEditRowStyle" />
                <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                <PagerStyle CssClass="GridviewPagerStyle" />
                <HeaderStyle CssClass="GridviewHeaderStyle" />
                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
            </asp:GridView>
            <asp:SqlDataSource ID="sdsViewOps" runat="server" ConnectionString="<%$ ConnectionStrings:UPConnectionString %>"
                
                DeleteCommand="DELETE FROM [Utopia_Kingdom_CE_Type_Pull] WHERE [uid] = @uid" InsertCommand="INSERT INTO [Utopia_Kingdom_CE_Type_Pull] ([CE_Type]) VALUES (@CE_Type)"
                SelectCommand="SELECT [uid], [CE_Type] FROM [Utopia_Kingdom_CE_Type_Pull] ORDER BY [CE_Type]"
                
                UpdateCommand="UPDATE [Utopia_Kingdom_CE_Type_Pull] SET [CE_Type] = @CE_Type WHERE [uid] = @uid">
                <DeleteParameters>
                    <asp:Parameter Name="uid" Type="Int32" />
                </DeleteParameters>
                <UpdateParameters>
                    <asp:Parameter Name="CE_Type" Type="String" />
                    <asp:Parameter Name="uid" Type="Int32" />
                </UpdateParameters>
                <InsertParameters>
                    <asp:Parameter Name="CE_Type" Type="String" />
                </InsertParameters>
            </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

