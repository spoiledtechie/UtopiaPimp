<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Races.aspx.cs" Inherits="admin_Races" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            
            <asp:GridView ID="gvViewOps" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                DataKeyNames="uid" DataSourceID="sdsViewOps">
                <Columns>
                    <asp:CommandField ShowEditButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="uid" HeaderText="uid" InsertVisible="False" ReadOnly="True"
                        SortExpression="uid" />
                    <asp:BoundField DataField="Race_Name" HeaderText="Race Name" SortExpression="Race_Name" />
                    <asp:CommandField ShowDeleteButton="True" />
                <asp:TemplateField>
       <ItemTemplate>
             <asp:CheckBox ID="chkRows" runat="server"/>
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
            <asp:Button
   ID="btnMultipleRowDelete"
   OnClick="btnMultipleRowDelete_Click"
   runat="server"
   Text="Delete Rows" />
 
            <asp:SqlDataSource ID="sdsViewOps" runat="server" ConnectionString="<%$ ConnectionStrings:UPConnectionString %>"
                
                DeleteCommand="DELETE FROM [Utopia_Province_Race_Pull] WHERE [uid] = @uid" InsertCommand="INSERT INTO [Utopia_Province_Race_Pull] ([Race_Name]) VALUES (@Race_Name)"
                SelectCommand="SELECT [uid], [Race_Name] FROM [Utopia_Province_Race_Pull] ORDER BY [Race_Name]"
                
                
                UpdateCommand="UPDATE [Utopia_Province_Race_Pull] SET [Race_Name] = @Race_Name WHERE [uid] = @uid">
                <DeleteParameters>
                    <asp:Parameter Name="uid" Type="Int32" />
                </DeleteParameters>
                <UpdateParameters>
                    <asp:Parameter Name="Race_Name" Type="String" />
                    <asp:Parameter Name="uid" Type="Int32" />
                </UpdateParameters>
                <InsertParameters>
                    <asp:Parameter Name="Race_Name" Type="String" />
                </InsertParameters>
            </asp:SqlDataSource>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="uid" DataSourceID="sdsRaceMilitary">
                <Columns>
                 <asp:CommandField ShowEditButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="uid" HeaderText="uid" InsertVisible="False" 
                        ReadOnly="True" SortExpression="uid" />
                    <asp:BoundField DataField="Race_ID" HeaderText="Race_ID" 
                        SortExpression="Race_ID" />
                    <asp:BoundField DataField="Elite_Name" HeaderText="Elite_Name" 
                        SortExpression="Elite_Name" />
                    <asp:BoundField DataField="Soldier_Off_Name" HeaderText="Soldier_Off_Name" 
                        SortExpression="Soldier_Off_Name" />
                    <asp:BoundField DataField="Soldier_Def_Name" HeaderText="Soldier_Def_Name" 
                        SortExpression="Soldier_Def_Name" />
                    <asp:BoundField DataField="Elite_Off_Multiplier" 
                        HeaderText="Elite_Off_Multiplier" SortExpression="Elite_Off_Multiplier" />
                    <asp:BoundField DataField="Elite_Def_Multiplier" 
                        HeaderText="Elite_Def_Multiplier" SortExpression="Elite_Def_Multiplier" />
                    <asp:BoundField DataField="Soldier_Off_Multiplier" 
                        HeaderText="Soldier_Off_Multiplier" SortExpression="Soldier_Off_Multiplier" />
                    <asp:BoundField DataField="Soldier_Def_Multiplier" 
                        HeaderText="Soldier_Def_Multiplier" SortExpression="Soldier_Def_Multiplier" />
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
            <asp:SqlDataSource ID="sdsRaceMilitary" runat="server" 
                ConnectionString="<%$ ConnectionStrings:UPConnectionString %>" 
                DeleteCommand="DELETE FROM [Utopia_Province_Race_Military_Names] WHERE [uid] = @uid" 
                InsertCommand="INSERT INTO [Utopia_Province_Race_Military_Names] ([Race_ID], [Elite_Name], [Soldier_Off_Name], [Soldier_Def_Name], [Elite_Off_Multiplier], [Elite_Def_Multiplier], [Soldier_Off_Multiplier], [Soldier_Def_Multiplier], [Added_By_User_Id], [Added_DateTime]) VALUES (@Race_ID, @Elite_Name, @Soldier_Off_Name, @Soldier_Def_Name, @Elite_Off_Multiplier, @Elite_Def_Multiplier, @Soldier_Off_Multiplier, @Soldier_Def_Multiplier, @Added_By_User_Id, @Added_DateTime)" 
                SelectCommand="SELECT [uid], [Race_ID], [Elite_Name], [Soldier_Off_Name], [Soldier_Def_Name], [Elite_Off_Multiplier], [Elite_Def_Multiplier], [Soldier_Off_Multiplier], [Soldier_Def_Multiplier], [Added_By_User_Id], [Added_DateTime] FROM [Utopia_Province_Race_Military_Names] ORDER BY [uid]" 
                UpdateCommand="UPDATE [Utopia_Province_Race_Military_Names] SET [Race_ID] = @Race_ID, [Elite_Name] = @Elite_Name, [Soldier_Off_Name] = @Soldier_Off_Name, [Soldier_Def_Name] = @Soldier_Def_Name, [Elite_Off_Multiplier] = @Elite_Off_Multiplier, [Elite_Def_Multiplier] = @Elite_Def_Multiplier, [Soldier_Off_Multiplier] = @Soldier_Off_Multiplier, [Soldier_Def_Multiplier] = @Soldier_Def_Multiplier WHERE [uid] = @uid">
                <DeleteParameters>
                    <asp:Parameter Name="uid" Type="Int32" />
                </DeleteParameters>
                <UpdateParameters>
                    <asp:Parameter Name="Race_ID" Type="Int32" />
                    <asp:Parameter Name="Elite_Name" Type="String" />
                    <asp:Parameter Name="Soldier_Off_Name" Type="String" />
                    <asp:Parameter Name="Soldier_Def_Name" Type="String" />
                    <asp:Parameter Name="Elite_Off_Multiplier" Type="Int32" />
                    <asp:Parameter Name="Elite_Def_Multiplier" Type="Int32" />
                    <asp:Parameter Name="Soldier_Off_Multiplier" Type="Int32" />
                    <asp:Parameter Name="Soldier_Def_Multiplier" Type="Int32" />
                    <asp:Parameter Name="Added_By_User_Id" Type="Object" />
                    <asp:Parameter Name="Added_DateTime" Type="DateTime" />
                    <asp:Parameter Name="uid" Type="Int32" />
                </UpdateParameters>
                <InsertParameters>
                    <asp:Parameter Name="Race_ID" Type="Int32" />
                    <asp:Parameter Name="Elite_Name" Type="String" />
                    <asp:Parameter Name="Soldier_Off_Name" Type="String" />
                    <asp:Parameter Name="Soldier_Def_Name" Type="String" />
                    <asp:Parameter Name="Elite_Off_Multiplier" Type="Int32" />
                    <asp:Parameter Name="Elite_Def_Multiplier" Type="Int32" />
                    <asp:Parameter Name="Soldier_Off_Multiplier" Type="Int32" />
                    <asp:Parameter Name="Soldier_Def_Multiplier" Type="Int32" />
                    <asp:Parameter Name="Added_By_User_Id" Type="Object" />
                    <asp:Parameter Name="Added_DateTime" Type="DateTime" />
                </InsertParameters>
            </asp:SqlDataSource>
        </ContentTemplate>
        
    </asp:UpdatePanel>
</asp:Content>

