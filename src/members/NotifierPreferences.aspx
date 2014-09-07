<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    CodeFile="NotifierPreferences.aspx.cs" Inherits="members_NotifierPreferences" %>

<%@ MasterType VirtualPath="~/main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    The pimp staff are proud to present the brand new notification service. You can
    now recieve game updates as soon as someone posts a CE or when your posted spells/armies are about to expire/return.<br />
    <br />
    <asp:UpdatePanel ID="updatePanel1" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
        <ContentTemplate>
            <fieldset>
                <legend>Preferences</legend>
                <asp:CheckBox ID="cbxAttacked" runat="server" />
                recieve a notification when you have been attacked.<br />
                <asp:CheckBox ID="cbxWarAction" runat="server" />
                recieves a notification when the kingdoms enter a new war/surrenders or wins an
                active war.<br />
                <asp:CheckBox ID="cbxDragonRavaging" runat="server" />
                recieves a notification when a dragon is ravaging your lands.<br />
                <asp:CheckBox ID="cbxDragonInitiated" runat="server" />
                recieves a notification when a dragon project has been initiated by the monarch.<br />
                <asp:CheckBox ID="cbxArmyReturns" runat="server" />
                recieves a notification when your army is about to return.<br />
                <asp:CheckBox ID="cbxObsExpired" runat="server" />
                recieves a notification when your ops are about to expire.<br />
                <asp:CheckBox ID="cbxTrackedProvinceUpdated" runat="server" />
                recieves a notification when your tracked province gets updated. (not available right now)<br />
                <br />
            </fieldset>
            <br />
            <fieldset>
                <legend>Options</legend>Frequency:
                <asp:DropDownList ID="ddlFrequency" runat="server">
                    <asp:ListItem Value="1" Text="Always"></asp:ListItem>
                    <asp:ListItem Value="15" Text="15 minutes"></asp:ListItem>
                    <asp:ListItem Value="20" Text="20 minutes"></asp:ListItem>
                    <asp:ListItem Value="30" Text="30 minutes"></asp:ListItem>
                    <asp:ListItem Value="45" Text="45 minutes"></asp:ListItem>
                    <asp:ListItem Value="60" Text="1 hour" Selected="True"></asp:ListItem>
                </asp:DropDownList>
                <br />
                (How often you will receive a notification from UtopiaPimp).<br />
                Delivery method:
                <asp:DropDownList ID="ddlDeliveryMethod" runat="server">
                    <asp:ListItem Text="Email" Value="1" Selected="True" />
                    <asp:ListItem Text="SMS (under construction)" Enabled="false" Value="2" />
                </asp:DropDownList>
            </fieldset>
            <asp:Button ID="btnSave" runat="server" Text="Save preferences" OnClick="btnSave_Click" />
            <br />
            <asp:Label ID="lblStatus" runat="server"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
