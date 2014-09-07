<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    CodeFile="kd.aspx.cs" Inherits="members_kd" %>

<%@ MasterType VirtualPath="~/main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="jsTest" runat="server">
    </div>
    <%--  
  This Script is being injected via the CS page.--%>
    <script type="text/javascript">
        function IdentifyProvince(trID) {
            if (trID != '') {
                if (keycodeMain != 17) {
                    if (document.getElementById('divKingdomID') != null) {
                        document.getElementById('divGuids').innerHTML = trID;
                        KDPage.GetProvinces(document.getElementById('divKingdomID').innerHTML, trID, document.getElementById('divOwnerID').innerHTML, OnWSUpdateTargetedNames)
                    }
                }
                else {
                    if (document.getElementById('divGuids').innerHTML.indexOf(trID) > -1) { }
                    else {
                        if (document.getElementById('divKingdomID') != null) {
                            document.getElementById('divGuids').innerHTML += ',' + trID;
                            KDPage.GetProvinces(document.getElementById('divKingdomID').innerHTML, document.getElementById('divGuids').innerHTML, document.getElementById('divOwnerID').innerHTML, OnWSUpdateTargetedNames)
                        }
                    }
                }
            }
            else { OnWSUpdateTargetedNames(''); }
        }
        function OnWSUpdateTargetedNames(results) {
            document.getElementById("divNames").innerHTML = results;
            if (results === '')
                document.getElementById('divGuids').innerHTML = '';
        }
    </script>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/controls/KDPage.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
    <div id="divSummary" class="divKingSummary" runat="server" visible="false">
    </div>
    <div id="divFilter" runat="server" visible="false">
        <br />
        <br />
        <div id="filterTabs" class="usualList">
            <ul class="mainTabs">
                <li><a href="#tabFilter1">Standard</a></li>
                <li><a href="#tabFilter2">Input Values</a></li>
            </ul>
            <div id="tabFilter1">
                <span style="font-weight: bold;">Kingdom-less Provinces</span><br />
                You can use the below settings to filter the kingdomless provinces.<br />
                <table>
                    <tr>
                        <td>
                            Networth:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStaNetworth" runat="server">
                                <asp:ListItem Text="(no filter)" Value="0-10000"></asp:ListItem>
                                <asp:ListItem Text="0-50k" Value="0-50"></asp:ListItem>
                                <asp:ListItem Text="50k-75k" Value="50-75"></asp:ListItem>
                                <asp:ListItem Text="75k-110k" Value="75-110"></asp:ListItem>
                                <asp:ListItem Text="110k-175k" Value="110-175"></asp:ListItem>
                                <asp:ListItem Text="175k-250k" Value="175-250"></asp:ListItem>
                                <asp:ListItem Text="250k-350k" Value="250-350"></asp:ListItem>
                                <asp:ListItem Text="350k-500k" Value="350-500"></asp:ListItem>
                                <asp:ListItem Text="500k-650k" Value="500-650"></asp:ListItem>
                                <asp:ListItem Text="650k+" Value="651-10000"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            Acres:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStaAcres" runat="server">
                                <asp:ListItem Text="(no filter)" Value="0-100000"></asp:ListItem>
                                <asp:ListItem Text="0-500" Value="0-500"></asp:ListItem>
                                <asp:ListItem Text="501-800" Value="501-800"></asp:ListItem>
                                <asp:ListItem Text="801-1200" Value="801-1200"></asp:ListItem>
                                <asp:ListItem Text="1201-1700" Value="1201-1700"></asp:ListItem>
                                <asp:ListItem Text="1701-2500" Value="1701-2500"></asp:ListItem>
                                <asp:ListItem Text="2501-3500" Value="2501-3500"></asp:ListItem>
                                <asp:ListItem Text="3501-4500" Value="3501-4500"></asp:ListItem>
                                <asp:ListItem Text="4501+" Value="4501-100000"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Last Updated:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStaLastUpdated" runat="server">
                                <asp:ListItem Text="(no filter)" Value="0"></asp:ListItem>
                                <asp:ListItem Text="0-1 days" Value="1"></asp:ListItem>
                                <asp:ListItem Text="0-2 days" Value="2"></asp:ListItem>
                                <asp:ListItem Text="0-3 days" Value="3"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <input id="btnStaFilter" runat="server" type="button" value="Filter" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="tabFilter2">
                <span style="font-weight: bold;">Kingdom-less Provinces</span><br />
                You can use the below settings to filter the kingdomless provinces.<br />
                <table>
                    <tr>
                        <td>
                            Networth:
                        </td>
                        <td>
                            <input id="tbInpNetworthStart" type="text" class="tbInput" />-<input id="tbInpNetworthEnd"
                                type="text" class="tbInput" />
                        </td>
                        <td>
                            Acres:
                        </td>
                        <td>
                            <input id="tbInpAcresStart" type="text" class="tbInput" />-<input id="tbInpAcresEnd"
                                type="text" class="tbInput" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Last Updated:
                        </td>
                        <td>
                            <input id="tbInpLUStart" type="text" class="tbInput" />-<input id="tbInpLUEnd" class="tbInput"
                                type="text" />
                        </td>
                        <td>
                            <input id="btnInpFilter" type="button" runat="server" value="Filter" />
                        </td>
                    </tr>
                </table>
                No need for comma's. Just enter the numbers and thats all we need to filter. You
                can also leave the boxes blank.
            </div>
        </div>
        <script type="text/javascript">
            $("#filterTabs ul").idTabs();
        </script>
    </div>
    <div>
        <asp:Label ID="lblWarning" runat="server"></asp:Label></div>
    <asp:SqlDataSource ID="sdsColumns" runat="server" EnableCaching="true" CacheDuration="180"
        ConnectionString="<%$ ConnectionStrings:UPConnectionString %>" SelectCommand="SELECT [Column_Name], [uid] FROM [Utopia_Column_Name_Pull] ORDER BY [Column_Name]">
    </asp:SqlDataSource>
    Sort by:
    <asp:DropDownList ID="ddlColumnList" AppendDataBoundItems="True" runat="server" AutoPostBack="True"
        DataTextField="Column_Name" DataValueField="uid" OnSelectedIndexChanged="ddlColumnList_SelectedIndexChanged"
        DataSourceID="sdsColumns" CssClass="ddlSortColumns">
        <asp:ListItem Text="[Clear Items]" Value="CLEAR"></asp:ListItem>
    </asp:DropDownList>
    <div id="pnlKDData" class="divKingdomInfo">
        <script type="text/javascript">
            $(document).ready(function () {
                LoadKingdomGrid();
            });
        </script>
        <center>
            <img src="http://codingforcharity.org/utopiapimp/img/Loading.gif" alt="Loading..."
                width="50px" /></center>
    </div>
    <asp:Button ID="btnExport" runat="server" Text="Export to Excel" OnClick="btnExport_Click" />
    <asp:Label ID="lblTimer" runat="server"></asp:Label>
    <br />
    <br />
    <div id="divOpTimeLimit" runat="server">
    </div>
    <br />
    <div id="pnlOpHistory">
        <script type="text/javascript">
            $(document).ready(function () {
                LoadOpHistroySummary();
            });
        </script>
        <center>
            <img src="http://codingforcharity.org/utopiapimp/img/Loading.gif" alt="Loading..."
                width="50px" /></center>
    </div>
    <div class="footer">
        <asp:Label ID="lblLoad" runat="server"></asp:Label>
        <ul>
            <li>A submitted CE shows attack times only. It does not show return times, therefore
                any army marked as away without an SOM is estimated at a 12 hour return time.</li>
            <li>Any Op without an Expiration Date will be archived after 24 hours of submitting
                it.</li>
            <li>You can request ops from your kingdom mates by clicking on the square box under
                the cb/som/sos/survey columns. This will alert you kingdom to help you get a op
                for that particular province.</li>
            <li>Why hasn't the Province been removed? Read this:<a href="http://blog.utopiapimp.com/post/1116040166/provinces-need-to-cool-down">
                http://blog.utopiapimp.com/post/1116040166/provinces-need-to-cool-down</a></li>
        </ul>
    </div>
    <asp:Literal ID="ltJavascript" runat="server"></asp:Literal>
    <asp:Literal ID="ltKI" Visible="false" runat="server"></asp:Literal>
    <div id="divNames" class="divTargetProv">
    </div>
    <div id="divGuids" style="display: none;">
    </div>
    <div id="toolTipLayer" style="position: absolute; visibility: hidden; left: 0; right: 0">
    </div>
    <div id="toolTipAdd" style="position: absolute; visibility: hidden; left: 0; right: 0">
    </div>
    <div class="divAdRightHide" id="divAdRight">
                <%--Don't want to use google ads here because this one is just hidden at all times.--%>
        <script type="text/javascript">
            getSideAd();
        </script>
        
    </div>
    <script src="http://codingforcharity.org/libs/jquery/jquery.metadata.js" type="text/javascript"></script>
    <script src="http://codingforcharity.org/libs/jquery/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="http://codingforcharity.org/libs/jquery/jquery.tablehover.js" type="text/javascript"></script>
    <script src="http://codingforcharity.org/utopiapimp/js/jquery-ui-1.7.2.custom.min.js"
        type="text/javascript"></script>
</asp:Content>
