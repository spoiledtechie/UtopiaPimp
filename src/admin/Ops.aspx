<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Ops.aspx.cs"
    Inherits="admin_Ops" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblOps" runat="server" Text="Add Mystic/Intel Operation:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtbxOp" runat="server"></asp:TextBox><asp:RequiredFieldValidator
                            ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtbxOp" ValidationGroup="SubmitOp"
                            ErrorMessage="RequiredFieldValidator" CssClass="RFAsterisk">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnAddOp" runat="server" Text="Add Op" ValidationGroup="SubmitOp"
                            OnClick="btnAddOp_Click" />
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gvViewOps" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                DataKeyNames="uid" DataSourceID="sdsViewOps">
                <Columns>
                    <asp:CommandField ShowEditButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="uid" HeaderText="uid" InsertVisible="False" ReadOnly="True"
                        SortExpression="uid" />
                    <asp:BoundField DataField="OP_Name" HeaderText="OP Name" SortExpression="OP_Name" />
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
                DeleteCommand="DELETE FROM [Utopia_Province_Ops_Pull] WHERE [uid] = @uid" InsertCommand="INSERT INTO [Utopia_Province_Ops_Pull] ([OP_Name]) VALUES (@OP_Name)"
                SelectCommand="SELECT [uid], [OP_Name] FROM [Utopia_Province_Ops_Pull] ORDER BY [OP_Name]"
                UpdateCommand="UPDATE [Utopia_Province_Ops_Pull] SET [OP_Name] = @OP_Name WHERE [uid] = @uid">
                <DeleteParameters>
                    <asp:Parameter Name="uid" Type="Int32" />
                </DeleteParameters>
                <UpdateParameters>
                    <asp:Parameter Name="OP_Name" Type="String" />
                    <asp:Parameter Name="uid" Type="Int32" />
                </UpdateParameters>
                <InsertParameters>
                    <asp:Parameter Name="OP_Name" Type="String" />
                </InsertParameters>
            </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
