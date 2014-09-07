<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="monarchs.aspx.cs"
    ValidateRequest="false" Inherits="members_monarchs" %>

<%@ MasterType VirtualPath="~/main.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/controls/MonarchHelper.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
    <br />
    <div id="usualMonarch" class="usual3">
        <ul class="mainTabs">
            <asp:Literal ID="ltTabsMonarch" runat="server"></asp:Literal>
        </ul>
        <div id="tab1Monarch" class="borders">
            <asp:Literal ID="ltProvinceCodes" runat="server"></asp:Literal>
        </div>
        <div id="tab2Monarch" class="borders">
        <asp:Literal ID="ltKingdomLists" runat="server"></asp:Literal>
        
        
      <%--      <asp:GridView ID="gvKingdomLists" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField HeaderText="Retired">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server" ToolTip='<%# Eval("Kingdom_ID") %>' Checked='<%# Eval("Retired") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Kingdom" HeaderText="Kingdom" SortExpression="Kingdom" />
                    <asp:BoundField DataField="KingdomName" HeaderText="Kingdom Name" SortExpression="KingdomName" />
                    <asp:TemplateField HeaderText="Status/Notes on Kingdom">
                        <ItemTemplate>
                            <asp:TextBox ID="tbStatus" Width="200px" runat="server" Text='<%# Eval("tbStatusInfo") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblKingdomID" runat="server" Text='<%# Eval("Kingdom_ID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="winLose" HeaderText="Wins/Losses" SortExpression="winLose" />
                </Columns>
            </asp:GridView>
            <br />
            <asp:Button ID="btnUpdateKingdomList" OnClick="btnUpdateKingdomList_Click" runat="server"
                Text="Submit Kingdom List" />--%>
             
        </div>
        <div id="tab3Monarch" class="borders">
            <div>
                As Monarch, you have the option to disconnect any province in your kingdom from
                the user who owns it. This will permentally remove the user until you give another
                user a province code.</div>
            <br />
            <asp:GridView ID="gvDeleteProvince" runat="server" DataKeyNames="Province_ID" AutoGenerateColumns="false"
                OnRowCommand="gvDeleteProvince_RowCommand">
                <Columns>
                    <%--<asp:BoundField DataField="userName" HeaderText="User" />--%>
                    <asp:BoundField DataField="Province_Name" HeaderText="Province" SortExpression="Province_Name" />
                    <asp:TemplateField HeaderText="Disconnect">
                        <ItemTemplate>
                            <asp:Button ID="Button1" Text="Disconnect Province/User" runat="server" OnClientClick="return confirm('Are you sure?')"
                                CommandName="cmdDelete" CommandArgument='<%# Eval("Province_ID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblKingdomID" runat="server" Text='<%# Eval("Province_ID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div id="tab4Monarch" class="borders">
            <ul class="ulList">
                <li>Click on user to change Monarch Status.</li>
                <li>
                    <img alt="on" src="http://codingforcharity.org/utopiapimp/img/icons/on.png" />
                    Owner of the Kingdom (O)</li>
                <li>
                    <img alt="on" src="http://codingforcharity.org/utopiapimp/img/icons/on.png" />
                    User is a Sub-Monarch (SM)</li>
                <li>
                    <img alt="off" src="http://codingforcharity.org/utopiapimp/img/icons/off.png" />
                    User is Not a Sub-Monarch</li>
                <li>No Underline - The user has not connected to their province yet.</li>
            </ul>
            <asp:Literal ID="ltSubMonarchs" runat="server"></asp:Literal></div>
        <div id="tab5Monarch" class="borders">
            <asp:Literal ID="ltAPIKeys" runat="server"></asp:Literal>
        </div>
        <div id="tab6Monarch" class="borders">
            <asp:Literal ID="ltKdLessMonarchs" runat="server"></asp:Literal>
        </div>
        <div id="tab7Monarch" class="borders">
            <asp:Literal ID="ltIRC" runat="server"></asp:Literal>
        </div>
    </div>
    <script type="text/javascript">
        $("#usualMonarch ul").idTabs();
    </script>
    <div class="divAdRight" id="divAdRight">
             
      <% if (Boomers.Utilities.Compare.CompareExt.getRandomTrueFalse())
         { %>
        <script type="text/javascript">
            getSideAd();
        </script>
        <%}
         else
         { %>
         <script type="text/javascript"><!--
             google_ad_client = "ca-pub-6494646249414123";
             /* PostSecretSkyScraper */
             google_ad_slot = "6985393558";
             google_ad_width = 160;
             google_ad_height = 600;
//-->
</script>
<script type="text/javascript"
src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
</script>

        <%} %>
       
    </div>
</asp:Content>
