<%@ Page Title="" Language="C#" MasterPageFile="~/admin/UAdmin/UAdmin.master" AutoEventWireup="true" CodeFile="Pulls.aspx.cs" Inherits="admin_UAdmin_Pulls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="uid" DataSourceID="sdsPullStance">
        <Columns>
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
            <asp:BoundField DataField="uid" HeaderText="uid" InsertVisible="False" 
                ReadOnly="True" SortExpression="uid" />
            <asp:BoundField DataField="stance" HeaderText="stance" 
                SortExpression="stance" />
            <asp:BoundField DataField="alt" HeaderText="alt" SortExpression="alt" />
        </Columns>
    </asp:GridView>

    <asp:SqlDataSource ID="sdsPullStance" runat="server" 
        ConnectionString="<%$ ConnectionStrings:UPConnectionString %>" 
        DeleteCommand="DELETE FROM [Utopia_Kingdom_Stance_Pull] WHERE [uid] = @uid" 
        InsertCommand="INSERT INTO [Utopia_Kingdom_Stance_Pull] ([stance], [alt]) VALUES (@stance, @alt)" 
        SelectCommand="SELECT [uid], [stance], [alt] FROM [Utopia_Kingdom_Stance_Pull] ORDER BY [stance]" 
        UpdateCommand="UPDATE [Utopia_Kingdom_Stance_Pull] SET [stance] = @stance, [alt] = @alt WHERE [uid] = @uid">
        <DeleteParameters>
            <asp:Parameter Name="uid" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="stance" Type="String" />
            <asp:Parameter Name="alt" Type="String" />
            <asp:Parameter Name="uid" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="stance" Type="String" />
            <asp:Parameter Name="alt" Type="String" />
        </InsertParameters>
    </asp:SqlDataSource>

</asp:Content>

