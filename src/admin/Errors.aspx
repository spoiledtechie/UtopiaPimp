<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Errors.aspx.cs"
    Inherits="admin_Errors" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table>
        <tr>
            <td>
                <div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            Failed At Code: <asp:Label ID="lblCount" runat="server"></asp:Label> left
                            <asp:Button ID="btnReviewedFailers" runat="server" Text="Review All" OnClick="btnReviewedFailers_Click" />
                            <asp:GridView ID="gvFailedAts" runat="server" AllowPaging="True" AllowSorting="True"
                                OnRowCommand="gvFailedAts_RowCommand" AutoGenerateColumns="False" DataKeyNames="uid"
                                DataSourceID="sdsGetFailedPoints">
                                <Columns>
                                    <asp:CommandField ShowSelectButton="True" />
                                    <asp:ButtonField ButtonType="Button" CommandName="cmdReviewed" Text="Reviewed" />
                                   <asp:ButtonField ButtonType="Button" CommandName="cmdReviewedAll" Text="Reviewed All for User" />
                                    <asp:BoundField DataField="uid" HeaderText="ID" ReadOnly="True" SortExpression="uid" />
                                   
                                    <asp:TemplateField HeaderText="UserName">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlemail" Text='<%# Bind("UserName") %>' NavigateUrl='<%# Bind("UserName","admin/Email.aspx?title=Please Read:&userName={0}")%>'
                                                runat="server" Target="_blank"></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="date_time" HeaderText="Date Time" ReadOnly="True" SortExpression="date_time" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkRows" runat="server" />
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
                            <asp:Button ID="btnMultipleRowDelete" OnClick="btnMultipleRowDelete_Click" runat="server"
                                Text="Delete Rows" />
                            <asp:SqlDataSource ID="sdsGetFailedPoints" runat="server" ConnectionString="<%$ ConnectionStrings:UPConnectionString %>"
                                SelectCommand="SELECT aspnet_Users.UserName, Utopia_Distorted_Data.uid, Utopia_Distorted_Data.date_time FROM aspnet_Users RIGHT OUTER JOIN Utopia_Distorted_Data ON aspnet_Users.UserId = Utopia_Distorted_Data.aspnet_ID WHERE (Utopia_Distorted_Data.Reviewed = '0')">
                            </asp:SqlDataSource>
                            <asp:LinqDataSource ID="ldsGetFailedPoints" runat="server" ContextTypeName="CS_Code.UtopiaDataContext"
                                OrderBy="date_time" Select="new (uid, aspnet_ID, date_time)" TableName="Utopia_Distorted_Datas"
                                Where="Reviewed == @Reviewed">
                                <WhereParameters>
                                    <asp:Parameter DefaultValue="0" Name="Reviewed" Type="Int32" />
                                </WhereParameters>
                            </asp:LinqDataSource>
                            <asp:DataList ID="dlFaildAts" runat="server" DataKeyField="uid" DataSourceID="sdsFailedAts">
                                <ItemTemplate>
                                    uid:
                                    <asp:Label ID="uidLabel" runat="server" Text='<%# Eval("uid") %>' />
                                    <br />
                                    UserName:
                                    <asp:Label ID="aspnet_IDLabel" runat="server" Text='<%# SupportFramework.Users.Memberships.getUserName(new Guid(Eval("aspnet_ID").ToString())) %>' />
                                    <br />
                                    date_time:
                                    <asp:Label ID="date_timeLabel" runat="server" Text='<%# Eval("date_time") %>' />
                                    <br />
                                    Failed_At:
                                    <asp:Label ID="Failed_AtLabel" runat="server" Text='<%# Eval("Failed_At") %>' />
                                    <br />
                                    Raw_Data:
                                    <asp:Label ID="Raw_DataLabel" runat="server" Text='<%# Eval("Raw_Data") %>' />
                                    <br />
                                    Reviewed_By_DateTime:
                                    <asp:Label ID="Reviewed_By_DateTimeLabel" runat="server" Text='<%# Eval("Reviewed_By_DateTime") %>' />
                                    <br />
                                    Reviewed_By_UserID:
                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("Reviewed_By_UserID") %>' />
                                    <br />
                                    <br />
                                    Version:
                                    <asp:Label ID="Reviewed_By_UserIDLabel" runat="server" Text='<%# Eval("Version") %>' />
                                    <br />
                                    <br />
                                    Submitted Data:
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("rawData") %>' />
                                    <br />
                                </ItemTemplate>
                            </asp:DataList>
                            <asp:SqlDataSource ID="sdsFailedAts" runat="server" ConnectionString="<%$ ConnectionStrings:UPConnectionString %>"
                                SelectCommand="SELECT [uid], [aspnet_ID], [date_time], [Failed_At], [Raw_Data], [rawData], [Reviewed_By_DateTime], [Reviewed_By_UserID], [Version] FROM [Utopia_Distorted_Data] WHERE ([uid] = @uid)">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="gvFailedAts" Name="uid" PropertyName="SelectedValue"
                                        Type="Int32" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
    </table>
    <div>
        Test Pimp Agent Queries.
        <div>
            <div>
                Decode</div>
            <asp:TextBox ID="tbTesting" TextMode="MultiLine" Rows="5" Columns="50" runat="server"></asp:TextBox>
            <asp:Button ID="btnTest" runat="server" Text="Decode" OnClick="btnTest_Click" />
            <asp:Label ID="lblTesting" runat="server"></asp:Label>
        </div>
        <div>
            <div>
                Encode</div>
            <asp:TextBox ID="tbEncode" TextMode="MultiLine" Rows="5" Columns="50" runat="server"></asp:TextBox>
            <asp:Button ID="btnEncode" runat="server" Text="Encode" OnClick="btnEncode_Click" />
            <asp:Label ID="lblEncode" runat="server"></asp:Label>
        </div>
    </div>
    <div>
        Javascript Decoding
        <div>
            <textarea name="theText" cols="40" rows="6" id="taDecode"></textarea><br>
            <input type="button" name="encode" value="Encode to base64" onclick="document.getElementById('taDecode').value=encode64(document.getElementById('taDecode').value);">
            <input type="button" name="decode" value="Decode from base64" onclick="document.getElementById('taDecode').value=decode64(document.getElementById('taDecode').value);">
        </div>
    </div>

    <script type="text/javascript"><!--

        // This code was written by Tyler Akins and has been placed in the
        // public domain.  It would be nice if you left this header intact.
        // Base64 code from Tyler Akins -- http://rumkin.com

        var keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

        function encode64(input) {
            var output = "";
            var chr1, chr2, chr3;
            var enc1, enc2, enc3, enc4;
            var i = 0;

            do {
                chr1 = input.charCodeAt(i++);
                chr2 = input.charCodeAt(i++);
                chr3 = input.charCodeAt(i++);

                enc1 = chr1 >> 2;
                enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
                enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
                enc4 = chr3 & 63;

                if (isNaN(chr2)) {
                    enc3 = enc4 = 64;
                } else if (isNaN(chr3)) {
                    enc4 = 64;
                }

                output = output + keyStr.charAt(enc1) + keyStr.charAt(enc2) +
         keyStr.charAt(enc3) + keyStr.charAt(enc4);
            } while (i < input.length);

            return output;
        }

        function decode64(input) {
            input = decodeURIComponent(input);
            var output = "";
            var chr1, chr2, chr3;
            var enc1, enc2, enc3, enc4;
            var i = 0;

            // remove all characters that are not A-Z, a-z, 0-9, +, /, or =
            input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");

            do {
                enc1 = keyStr.indexOf(input.charAt(i++));
                enc2 = keyStr.indexOf(input.charAt(i++));
                enc3 = keyStr.indexOf(input.charAt(i++));
                enc4 = keyStr.indexOf(input.charAt(i++));

                chr1 = (enc1 << 2) | (enc2 >> 4);
                chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
                chr3 = ((enc3 & 3) << 6) | enc4;

                output = output + String.fromCharCode(chr1);

                if (enc3 != 64) {
                    output = output + String.fromCharCode(chr2);
                }
                if (enc4 != 64) {
                    output = output + String.fromCharCode(chr3);
                }
            } while (i < input.length);

            return output;
        }
        function urlDecode(str) {
            str = str.replace(new RegExp('\\+', 'g'), ' ');
            return unescape(str);
        }
        function urlEncode(str) {
            str = escape(str);
            str = str.replace(new RegExp('\\+', 'g'), '%2B');
            return str.replace(new RegExp('%20', 'g'), '+');
        }
//--></script>

</asp:Content>
