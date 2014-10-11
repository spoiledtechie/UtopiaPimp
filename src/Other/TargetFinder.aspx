<%@ Page Title="Utopiapimp.com Target Finder" Language="C#" MasterPageFile="~/Other/Other.master"
    AutoEventWireup="true" CodeFile="TargetFinder.aspx.cs" Inherits="Other_TargetFinder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="center">
        <a href="http://utopiapimp.com">UtopiaPimp's</a> Target Finder</h2>
    <div class="center">
        Now Storing
        <asp:Label ID="lblProvinces" CssClass="Bold" runat="server"></asp:Label>
        Provinces with
        <asp:Label ID="lblProvincesUpdates" CssClass="Bold" runat="server"></asp:Label>
        updated within the past hour.<br />
        <asp:Label ID="lblUsersUsed" runat="server"></asp:Label>
        users have used this target finder within the past hour.</div>
    <br />
    <ul class="ulLoggedInLinks center">
        <li>Username:</li>
        <li>
            <asp:Label runat="server" CssClass="Bold" ID="lbUserName"></asp:Label></li>
        <li>You can search for
            <asp:Label ID="spanSearchCount" CssClass="Bold" runat="server"></asp:Label>
            provinces</li>
    </ul>
    <br />
    <div class="divCenter">
        <div style="float: left; width: 49%; position: relative;">
            <div class="divAddTargetDataBox" id="divAddTargetData" runat="server">
                Submit Kingdom Pages Here:<br />
                <textarea id="tbAddTargetInfo" rows="5" cols="20" class="AllDescriptionFields"></textarea><br />
                <input id="btnAddTargetInfo" type="button" onclick="javascript:AddTargetDatajs();"
                    value="Add Data" /><input id="btnClearTargetInfo" type="button" value="Clear" onclick="javascript:ClearTargetBox();" />
                <div id="divTargetWarning" class="divWarning">
                </div>
            </div>
        </div>
        <div style="width: 49%; float: left; position: relative;">
            <ul class="ulList">
                <li class="Title">Instructions:</li><li>- You may ONLY submit kingdom pages to this
                    target finder. Due to the way kingdom pages look in utopia, only one page can be
                    submitted at a time.</li>
                <li>- For each Province that gets submitted, you will recieve one province result in
                    your search. PLUS another 15 provinces just for submitting a kingdom page.</li>
                <li>- <b>For Example:</b> If you submit a kingdom page with 12 provinces on it, you
                    will be allowed to search for 12 + 15 = 27 more provinces.</li>
                <li>- The more kingdom pages you submit at a time, means the more provinces you will
                    be allowed to search with.</li>
                <li></li>
            </ul>
        </div>
        <div id="divFilter" style="clear: both;" runat="server">
            Searches the Latest
            <input id="tbProvinceCount" value="25" style="width: 40px;" type="text" />
            Provinces (set how many provinces you want to return)
            <br />
            <div style="font-weight: bold;">
                Target Finder Search</div>
            <div id="usual1" class="usual3">
                <ul class="mainTabs">
                    <li><a href="#tab1">Standard</a></li>
                    <li><a href="#tab2">Input Values</a></li>
                </ul>
                <div id="tab1" class="borders">
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
                                Last Update:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStaLastUpdated" runat="server">
                                    <asp:ListItem Text="(no filter)" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="0-1 days" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="0-2 days" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="0-3 days" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="0-4 days" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="0-5 days" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="0-6 days" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="0-7 days" Value="7"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                Race:
                            </td>
                            <td valign="top">
                                <div id="divRaces" >
                                    <input id="cbAvian" type="checkbox" checked="checked" />Avian<br />
                                    <input id="cbDarkElf" type="checkbox" checked="checked" />Dark Elf<br />
                                    <input id="cbDwarf" type="checkbox" checked="checked" />Dwarf<br />
                                    <input id="cbElf" type="checkbox" checked="checked" />Elf<br />
                                    <input id="cbFaery" type="checkbox" checked="checked" />Faery<br />
                                    <input id="cbGnome" type="checkbox" checked="checked" />Gnome<br />
                                    <input id="cbHalfling" type="checkbox" checked="checked" />Halfling<br />
                                    <input id="cbHuman" type="checkbox" checked="checked" />Human<br />
                                    <input id="cbOrc" type="checkbox" checked="checked" />Orc<br />
                                    <input id="cbUndead" type="checkbox" checked="checked" />Undead<br />
                                </div>
                            </td>
                            <td valign="top">
                                Honor:
                            </td>
                            <td valign="top">
                                <div id="divHonor">
                                    <input id="cbPeasant" type="checkbox" checked="checked" />Peasant<br />
                                    <input id="cbLord-NobleLady" type="checkbox" checked="checked" />Lord/Noble Lady
                                    <br />
                                    <input id="cbBaron-Baroness" type="checkbox" checked="checked" />Baron/Baroness
                                    <br />
                                    <input id="cbViscount-Viscountess" type="checkbox" checked="checked" />Viscount/Viscountess
                                    <br />
                                    <input id="cbCount-Countess" type="checkbox" checked="checked" />Count/Countess
                                    <br />
                                    <input id="cbMarquis-Marchioness" type="checkbox" checked="checked" />Marquis/Marchioness
                                    <br />
                                    <input id="cbDuke-Duchess" type="checkbox" checked="checked" />Duke/Duchess
                                    <br />
                                    <input id="Prince-Princess" type="checkbox" checked="checked" />Prince/Princess
                                    <br />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <input id="btnStaFilter" runat="server" type="button" value="Search" /><div id="divSearchingAuto"
                                    class="divWarning">
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="tab2"  class="borders">
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
                        </tr>
                        <tr>
                            <td valign="top">
                                Race:
                            </td>
                            <td valign="top">
                                <div id="divRacesInput">
                                    <input id="cb1Avian" type="checkbox" checked="checked" />Avian<br />
                                    <input id="cb1DarkElf" type="checkbox" checked="checked" />Dark Elf<br />
                                    <input id="cb1Dwarf" type="checkbox" checked="checked" />Dwarf<br />
                                    <input id="cb1Elf" type="checkbox" checked="checked" />Elf<br />
                                    <input id="cb1Faery" type="checkbox" checked="checked" />Faery<br />
                                    <input id="cb1Gnome" type="checkbox" checked="checked" />Gnome<br />
                                    <input id="cb1Halfling" type="checkbox" checked="checked" />Halfling<br />
                                    <input id="cb1Human" type="checkbox" checked="checked" />Human<br />
                                    <input id="cb1Orc" type="checkbox" checked="checked" />Orc<br />
                                    <input id="cb1Undead" type="checkbox" checked="checked" />Undead<br />
                                </div>
                            </td>
                            <td valign="top">
                                Honor:
                            </td>
                            <td valign="top">
                                <div id="divHonorInput">
                                    <input id="cb1Peasant" type="checkbox" checked="checked" />Peasant<br />
                                    <input id="cb1Lord-NobleLady" type="checkbox" checked="checked" />Lord/Noble Lady
                                    <br />
                                    <input id="cb1Baron-Baroness" type="checkbox" checked="checked" />Baron/Baroness
                                    <br />
                                    <input id="cb1Viscount-Viscountess" type="checkbox" checked="checked" />Viscount/Viscountess
                                    <br />
                                    <input id="cb1Count-Countess" type="checkbox" checked="checked" />Count/Countess
                                    <br />
                                    <input id="cb1Marquis-Marchioness" type="checkbox" checked="checked" />Marquis/Marchioness
                                    <br />
                                    <input id="cb1Duke-Duchess" type="checkbox" checked="checked" />Duke/Duchess
                                    <br />
                                    <input id="cb1Prince-Princess" type="checkbox" checked="checked" />Prince/Princess
                                    <br />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <input id="btnInpFilter" type="button" runat="server" value="Search" /><div id="divSearchingInput"
                                    class="divWarning">
                                </div>
                            </td>
                        </tr>
                    </table>
                    No need for comma's. Just enter the numbers and thats all we need to filter. You
                    can also leave the boxes blank.</div>
            </div>
            <script type="text/javascript">
                $("#usual1 ul").idTabs();
            </script>
        </div>
    </div>
    <div class="divAdRight2" id="divAdRight">
   
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

       
    </div>
    <br />
    <div class="center">
        If you have ANY problems/Questions/Comments/Concerns with the Target Finder, please
        post them here: <a href="http://getsatisfaction.com/utopiapimp/topics/target_finder_problems">
            http://getsatisfaction.com/utopiapimp/topics/target_finder_problems</a></div>
    <br />
    <br />
    <div id="divSearchResults">
    </div>
    <br />
    <div class="center footer">
        <b>Disclaimer:</b> By no way, shape, or form does information displayed on this
        page get pulled from any other part of Utopiapimp.com, Utopiashrimp.com or Utopia-Game.com.
        This Target finder will NEVER talk to any other part of UtopiaPimp.com.</div>
        <asp:Label ID="lblHidden" runat="server"></asp:Label>
</asp:Content>
