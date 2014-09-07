<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="kingdom.aspx.cs"
    Inherits="members_kingdom" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <div>
        <div id="kingdomTabs" class="usual">
            <ul class="mainTabs">
                <li><a href="#tabkingdom1" runat="server" id="aJoin">Join Kingdom</a></li>
                <li><a href="#tabkingdom2" runat="server" id="aAdd">Add My Kingdom</a></li>
            </ul>
            <div id="tabkingdom1">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblProvinceCode" runat="server" Text="Enter In Province Code:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtbxProvinceCode" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblWarningProvinceCode" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnSubmit" runat="server" ValidationGroup="ProvinceGroup" Text="Join Kingdom"
                                OnClick="btnSubmit_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="tabkingdom2">
                <table>
                    <tr>
                        <td colspan="2">
                            Start a Kingdom Here
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Your <b>Province</b> Name (<b>WITHOUT</b> locations/symbols like * ^):
                        </td>
                        <td>
                            <asp:TextBox ID="txtbxAddKingdom" runat="server" CssClass="txtbx"></asp:TextBox><asp:RequiredFieldValidator
                                ValidationGroup="SubmitNewKingdom" ControlToValidate="txtbxAddKingdom" CssClass="RFAsterisk"
                                ID="RequiredFieldValidator1" runat="server" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblServerName" runat="server" Text="Server:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlServerName" runat="server" DataSourceID="LServerName" AppendDataBoundItems="true"
                                CssClass="AllDropDowns" DataTextField="Server_Name" DataValueField="uid">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="SubmitNewKingdom"
                                ControlToValidate="ddlServerName" CssClass="RFAsterisk" runat="server" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                            <asp:LinqDataSource ID="LServerName" runat="server" ContextTypeName="CS_Code.UtopiaDataContext"
                                OrderBy="Server_Name" Select="new (uid, Server_Name)" TableName="Utopia_Server_Pulls">
                            </asp:LinqDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblKingdomPage" runat="server" Text="Kingdom Page:"></asp:Label>
                        </td>
                        <td valign="top">
                            <asp:TextBox ID="txtbxKingdomPage" runat="server" CssClass="AllDescriptionFields"
                                TextMode="MultiLine" Rows="5"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator3"
                                    ControlToValidate="txtbxKingdomPage" CssClass="RFAsterisk" runat="server" ValidationGroup="SubmitNewKingdom"
                                    ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                            <br />
                            Please Paste a RAW unformatted Kingdom Page or an Utopia Angel Formatted Kingdom
                            Page.
                            <br />
                            PS. This will break if your province name cannot be found in the kingdom page. You
                            MUST input the province name EXACTLY how it is shown on the kingdom page. Correct
                            Case and all.
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnStartKingdom" runat="server" Text="Setup my Kingdom Account" ValidationGroup="SubmitNewKingdom"
                                CssClass="btnAllButtons" OnClick="btnStartKingdom_Click" /><br />
                            <div id="divWarning" runat="server">
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Literal ID="ltDiv" runat="server"></asp:Literal>
        </div>
        <script type="text/javascript">
            $("#kingdomTabs ul").idTabs();
        </script>
    </div>
</asp:Content>
