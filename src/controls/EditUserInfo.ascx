<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditUserInfo.ascx.cs"
    Inherits="controls_EditUserInfo" %>
<table>
    <tr>
        <td class="tdRC">
            User Name:
        </td>
        <td class="tdLC">
            <asp:Label ID="lblUserName" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="tdRC">
            Nick Name:
        </td>
        <td class="tdLC">
            <input id="tbNickName" runat="server" type="text" />
            (Will be used as a display name later)
        </td>
    </tr>
    <tr>
        <td class="tdRC">
            Date of Birth:
        </td>
        <td class="tdLC">
            M:<asp:DropDownList ID="ddlMonth" runat="server">
                <asp:ListItem Value="0" Text=""></asp:ListItem>
                <asp:ListItem Value="1" Text="Jan."></asp:ListItem>
                <asp:ListItem Value="2" Text="Feb."></asp:ListItem>
                <asp:ListItem Value="3" Text="Mar."></asp:ListItem>
                <asp:ListItem Value="4" Text="Apr."></asp:ListItem>
                <asp:ListItem Value="5" Text="May."></asp:ListItem>
                <asp:ListItem Value="6" Text="Jun."></asp:ListItem>
                <asp:ListItem Value="7" Text="Jul."></asp:ListItem>
                <asp:ListItem Value="8" Text="Aug."></asp:ListItem>
                <asp:ListItem Value="9" Text="Sep."></asp:ListItem>
                <asp:ListItem Value="10" Text="Oct."></asp:ListItem>
                <asp:ListItem Value="11" Text="Nov."></asp:ListItem>
                <asp:ListItem Value="12" Text="Dec."></asp:ListItem>
            </asp:DropDownList>
            D:<asp:DropDownList ID="ddlDay" runat="server">
                <asp:ListItem Value="0" Text=""></asp:ListItem>
                <asp:ListItem Value="1"></asp:ListItem>
                <asp:ListItem Value="2"></asp:ListItem>
                <asp:ListItem Value="3"></asp:ListItem>
                <asp:ListItem Value="4"></asp:ListItem>
                <asp:ListItem Value="5"></asp:ListItem>
                <asp:ListItem Value="6"></asp:ListItem>
                <asp:ListItem Value="7"></asp:ListItem>
                <asp:ListItem Value="8"></asp:ListItem>
                <asp:ListItem Value="9"></asp:ListItem>
                <asp:ListItem Value="10"></asp:ListItem>
                <asp:ListItem Value="11"></asp:ListItem>
                <asp:ListItem Value="12"></asp:ListItem>
                <asp:ListItem Value="13"></asp:ListItem>
                <asp:ListItem Value="14"></asp:ListItem>
                <asp:ListItem Value="15"></asp:ListItem>
                <asp:ListItem Value="16"></asp:ListItem>
                <asp:ListItem Value="17"></asp:ListItem>
                <asp:ListItem Value="18"></asp:ListItem>
                <asp:ListItem Value="19"></asp:ListItem>
                <asp:ListItem Value="20"></asp:ListItem>
                <asp:ListItem Value="21"></asp:ListItem>
                <asp:ListItem Value="22"></asp:ListItem>
                <asp:ListItem Value="23"></asp:ListItem>
                <asp:ListItem Value="24"></asp:ListItem>
                <asp:ListItem Value="25"></asp:ListItem>
                <asp:ListItem Value="26"></asp:ListItem>
                <asp:ListItem Value="27"></asp:ListItem>
                <asp:ListItem Value="28"></asp:ListItem>
                <asp:ListItem Value="29"></asp:ListItem>
                <asp:ListItem Value="30"></asp:ListItem>
                <asp:ListItem Value="31"></asp:ListItem>
            </asp:DropDownList>
            Y:<asp:DropDownList AppendDataBoundItems="true" ID="ddlYear" runat="server">
                <asp:ListItem Value="0" Text=""></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="tdRC">
            Country:
        </td>
        <td class="tdLC">
            <input id="tbCountry" runat="server" type="text" />
        </td>
    </tr>
    <tr>
        <td class="tdRC">
            State:
        </td>
        <td class="tdLC">
            <input id="tbState" runat="server" type="text" />
        </td>
    </tr>
    <tr>
        <td class="tdRC">
            City:
        </td>
        <td class="tdLC">
            <input id="tbCity" runat="server" type="text" />
        </td>
    </tr>
    <tr>
        <td class="tdRC">
            GMT:
        </td>
        <td class="tdLC">
            <asp:DropDownList ID="ddlGMT" runat="server">
                <asp:ListItem Value="0"></asp:ListItem>
                <asp:ListItem Value="+1"></asp:ListItem>
                <asp:ListItem Value="+2"></asp:ListItem>
                <asp:ListItem Value="+3"></asp:ListItem>
                <asp:ListItem Value="+4"></asp:ListItem>
                <asp:ListItem Value="+5"></asp:ListItem>
                <asp:ListItem Value="+6"></asp:ListItem>
                <asp:ListItem Value="+7"></asp:ListItem>
                <asp:ListItem Value="+8"></asp:ListItem>
                <asp:ListItem Value="+9"></asp:ListItem>
                <asp:ListItem Value="+10"></asp:ListItem>
                <asp:ListItem Value="+11"></asp:ListItem>
                <asp:ListItem Value="+12"></asp:ListItem>
                <asp:ListItem Value="-1"></asp:ListItem>
                <asp:ListItem Value="-2"></asp:ListItem>
                <asp:ListItem Value="-3"></asp:ListItem>
                <asp:ListItem Value="-4"></asp:ListItem>
                <asp:ListItem Value="-5"></asp:ListItem>
                <asp:ListItem Value="-6"></asp:ListItem>
                <asp:ListItem Value="-7"></asp:ListItem>
                <asp:ListItem Value="-8"></asp:ListItem>
                <asp:ListItem Value="-9"></asp:ListItem>
                <asp:ListItem Value="-10"></asp:ListItem>
                <asp:ListItem Value="-11"></asp:ListItem>
                <asp:ListItem Value="-12"></asp:ListItem>
            </asp:DropDownList>
            <a target="_blank" href="http://www.worldtimezone.com/wtz.php">Find your GMT Offset</a>
        </td>
    </tr>
    <tr>
        <td class="tdRC">
            Phone Numbers:
        </td>
        <td class="tdLC">
            <asp:Literal ID="ltPH" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr>
        <td class="tdRC">
            IM/IRC Contact Names:
        </td>
        <td class="tdLC">
            <asp:Literal ID="ltCN" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr>
        <td class="tdRC">
            Notes:
        </td>
        <td class="tdLC">
            <asp:TextBox ID="tbNotes" runat="server" Rows="5" TextMode="MultiLine"></asp:TextBox>
        </td>
    </tr>
</table>
<div id="divItemUpdated">
</div>
<input id="btnSave" runat="server" type="button" value="Save/Update Contact Info" />
<%--<br />
Passwords will not show up on Kingdom Contact List. They are there just for automated
connections to various pimp tools.--%> 