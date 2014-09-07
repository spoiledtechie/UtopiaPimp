<%@ Page Language="C#" MasterPageFile="~/LoggedOut.master" AutoEventWireup="true" CodeFile="donate.aspx.cs"
    Inherits="anonymous_donate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <% if (Page.User.IsInRole("admin"))
           { %>
        <div>
            Name:<asp:TextBox ID="txtbxDonatorName" runat="server" CssClass="txtbx"></asp:TextBox><asp:RequiredFieldValidator
                ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtbxDonatorName"
                CssClass="RFAsterisk" ErrorMessage="RequiredFieldValidator" ValidationGroup="Donations">*</asp:RequiredFieldValidator>
            Amount:<asp:TextBox ID="txtbxDonatorAmount" runat="server" CssClass="txtbx"></asp:TextBox><asp:RequiredFieldValidator
                ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtbxDonatorAmount"
                CssClass="RFAsterisk" ErrorMessage="RequiredFieldValidator" ValidationGroup="Donations">*</asp:RequiredFieldValidator>
            Date:<asp:TextBox ID="txtbxDateDonated" runat="server" CssClass="txtbx"></asp:TextBox><asp:ImageButton
                ID="ibCalendar" runat="server" ImageUrl="http://codingforcharity.org/utopiapimp/img/Calendar_scheduleHS.png" /><asp:RequiredFieldValidator
                    ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtbxDateDonated"
                    CssClass="RFAsterisk" ErrorMessage="RequiredFieldValidator" ValidationGroup="Donations">*</asp:RequiredFieldValidator><cc1:CalendarExtender
                        ID="CalendarExtendertxtbxDate" runat="server" CssClass="ajax__calendar" Format="MM/dd/yyyy"
                        PopupButtonID="ibCalendar" TargetControlID="txtbxDateDonated">
                    </cc1:CalendarExtender>
            <asp:Button ID="btnAddDonator" runat="server" Text="Submit Name" ValidationGroup="Donations"
                OnClick="btnAddDonator_Click" />
        </div>
        <% } %>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:GridView ID="gvDonations" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                    AllowSorting="True" DataKeyNames="uid" DataSourceID="SqlDonations" PageSize="20">
                    <Columns>
                        <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                        <asp:BoundField DataField="uid" HeaderText="uid" SortExpression="uid" InsertVisible="False"
                            ReadOnly="True" Visible="false" />
                        <asp:BoundField DataField="Donator_Name" HeaderText="Name" SortExpression="Donator_Name" />
                        <asp:BoundField DataField="Donated_Amount" HeaderText="Amount" SortExpression="Donated_Amount" />
                        <asp:BoundField DataField="Date_Donated" HeaderText="Date" SortExpression="Date_Donated"
                            DataFormatString="{0:MM/dd/yy}" HtmlEncode="false" />
                    </Columns>
                    <FooterStyle CssClass="GridviewFooterStyle" />
                    <RowStyle CssClass="GridviewRowStyle" />
                    <EditRowStyle CssClass="GridviewEditRowStyle" />
                    <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                    <PagerStyle CssClass="GridviewPagerStyle" />
                    <HeaderStyle CssClass="GridviewHeaderStyle" />
                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDonations" runat="server" ConnectionString="<%$ ConnectionStrings:UPConnectionString1 %>"
                    DeleteCommand="DELETE FROM [Utopia_Donations] WHERE [uid] = @uid" InsertCommand="INSERT INTO [Utopia_Donations] ([Donator_Name], [Donated_Amount], [Date_Donated]) VALUES (@Donator_Name, @Donated_Amount, @Date_Donated)"
                    SelectCommand="SELECT [uid], [Donator_Name], [Donated_Amount], [Date_Donated] FROM [Utopia_Donations] ORDER BY [Date_Donated] DESC"
                    UpdateCommand="UPDATE [Utopia_Donations] SET [Donator_Name] = @Donator_Name, [Donated_Amount] = @Donated_Amount, [Date_Donated] = @Date_Donated WHERE [uid] = @uid">
                    <DeleteParameters>
                        <asp:Parameter Name="uid" Type="Int32" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="Donator_Name" Type="String" />
                        <asp:Parameter Name="Donated_Amount" Type="Decimal" />
                        <asp:Parameter Name="Date_Donated" Type="DateTime" />
                        <asp:Parameter Name="uid" Type="Int32" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="Donator_Name" Type="String" />
                        <asp:Parameter Name="Donated_Amount" Type="Decimal" />
                        <asp:Parameter Name="Date_Donated" Type="DateTime" />
                    </InsertParameters>
                </asp:SqlDataSource>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAddDonator" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
