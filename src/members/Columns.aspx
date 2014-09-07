<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    CodeFile="Columns.aspx.cs" Inherits="members_Columns" %>

<%@ MasterType VirtualPath="~/main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/controls/ColumnChooser.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
    <% if (new Pimp.UData.PimpUserWrapper ().PimpUser.IsUserAdmin)
       { %>
    <div>
        Add Column
        <asp:TextBox ID="txtbxAddColumn" runat="server" CssClass="txtbx"></asp:TextBox>
        Tool Tip
        <asp:TextBox ID="txtbxToolTip" runat="server" CssClass="txtbx"></asp:TextBox>
        <asp:DropDownList ID="ddlSelectCatagory" runat="server" CssClass="AllDropDowns" DataSourceID="ldsAddColumn"
            DataTextField="Category_Name" AppendDataBoundItems="true" DataValueField="Category_ID">
            <asp:ListItem Text="" Value=""></asp:ListItem>
        </asp:DropDownList>
        <asp:LinqDataSource ID="ldsAddColumn" runat="server" ContextTypeName="CS_Code.UtopiaDataContext"
            OrderBy="Category_Name" Select="new (Category_ID, Category_Name)" TableName="Utopia_Column_Catagory_Name_Pulls">
        </asp:LinqDataSource>
        <asp:Button ID="btnAddColumn" runat="server" Text="Submit Catagory" CssClass="btnAllButtons"
            OnClick="btnAddColumn_Click" />
        <asp:DropDownList ID="ddlDelete" runat="server" CssClass="AllDropDowns" DataSourceID="ldsDeleteColumn"
            DataTextField="Column_Name" DataValueField="uid" AppendDataBoundItems="true">
            <asp:ListItem Text="" Value=""></asp:ListItem>
        </asp:DropDownList>
        <asp:LinqDataSource ID="ldsDeleteColumn" runat="server" ContextTypeName="CS_Code.UtopiaDataContext"
            OrderBy="Column_Name" Select="new (Column_Name, uid)" TableName="Utopia_Column_Name_Pulls">
        </asp:LinqDataSource>
        <asp:Button ID="btnDelete" runat="server" Text="Delete Column" CssClass="btnAllButtons"
            OnClick="btnDelete_Click" />
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView runat="server" ID="gvColumnLists" AutoGenerateColumns="False" DataKeyNames="uid"
                    DataSourceID="sdsColumns">
                    <Columns>
                        <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                        <asp:BoundField DataField="uid" HeaderText="uid" InsertVisible="False" ReadOnly="True"
                            SortExpression="uid" />
                        <asp:BoundField DataField="Column_Name" HeaderText="Column_Name" SortExpression="Column_Name" />
                        <asp:BoundField DataField="DateTime_Added" HeaderText="DateTime_Added" SortExpression="DateTime_Added" />
                        <asp:CheckBoxField DataField="Enabled" HeaderText="Enabled" SortExpression="Enabled" />
                        <asp:BoundField DataField="Category_ID" HeaderText="Category_ID" SortExpression="Category_ID" />
                        <asp:BoundField DataField="Alt" HeaderText="Alt" SortExpression="Alt" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="sdsColumns" runat="server" ConnectionString="<%$ ConnectionStrings:UPConnectionString %>"
                    DeleteCommand="DELETE FROM [Utopia_Column_Name_Pull] WHERE [uid] = @uid" InsertCommand="INSERT INTO [Utopia_Column_Name_Pull] ([Column_Name], [DateTime_Added], [Enabled], [Category_ID], [Alt]) VALUES (@Column_Name, @DateTime_Added, @Enabled, @Category_ID, @Alt)"
                    SelectCommand="SELECT [uid], [Column_Name], [DateTime_Added], [Enabled], [Category_ID], [Alt] FROM [Utopia_Column_Name_Pull] ORDER BY [Category_ID], [Column_Name]"
                    UpdateCommand="UPDATE [Utopia_Column_Name_Pull] SET [Column_Name] = @Column_Name, [DateTime_Added] = @DateTime_Added, [Enabled] = @Enabled, [Category_ID] = @Category_ID, [Alt] = @Alt WHERE [uid] = @uid">
                    <DeleteParameters>
                        <asp:Parameter Name="uid" Type="Int32" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="Column_Name" Type="String" />
                        <asp:Parameter Name="DateTime_Added" Type="DateTime" />
                        <asp:Parameter Name="Enabled" Type="Boolean" />
                        <asp:Parameter Name="Category_ID" Type="Int32" />
                        <asp:Parameter Name="Alt" Type="String" />
                        <asp:Parameter Name="uid" Type="Int32" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="Column_Name" Type="String" />
                        <asp:Parameter Name="DateTime_Added" Type="DateTime" />
                        <asp:Parameter Name="Enabled" Type="Boolean" />
                        <asp:Parameter Name="Category_ID" Type="Int32" />
                        <asp:Parameter Name="Alt" Type="String" />
                    </InsertParameters>
                </asp:SqlDataSource>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <% } %>
    <asp:Panel ID="pnlMonarch" runat="server" Visible="false">
        <div>
            <h3>
                Set Default Kingdom Columns:</h3>
        </div>
        <div>
            As Monarch of your kingdom, you will be allowed to choose the Default Column SETS
            for your kingdom.</div>
    </asp:Panel>
    <%--    <div>
        We wanted to create a way where all Utopians can help each other, so while selecting
        columns you are also allowed to view other users columns and use their setup if
        you wish. Its sort of like group think, so you can see what they use and they can
        see what you use.
    </div>--%>
    <div id="divMySetsContainer">
        <div id="divMySets" runat="server">
        </div>
    </div>
    <div id="divItemContainer">
        <div id="divItem" runat="server">
        </div>
    </div>
</asp:Content>
