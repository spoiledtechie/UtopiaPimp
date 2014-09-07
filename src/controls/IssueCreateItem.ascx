<%@ Control Language="C#" AutoEventWireup="true" CodeFile="IssueCreateItem.ascx.cs"
    Inherits="controls_CreateItem" %>
<div><div class="divVotesBox VotesBoxMargin">
    <div class="divVotesBoxContainer centertext">
        <div class="divVoteCountContainer VotesBoxPadding">
            <div>
                <asp:Label ID="lblVoteCount" CssClass="Bold" runat="server" Text="1"></asp:Label></div>
            <div>
                <asp:Label ID="lblVountCountText" runat="server" Text="vote"></asp:Label>
            </div>
        </div>
        <div>
            <div class="divVoteTypeContainer VotesBoxPadding">
                <asp:Label ID="lblCheckVote" runat="server" Text="voted"></asp:Label></div>
            <asp:LinkButton ID="lbVote" runat="server" Visible="false" Text="Vote"></asp:LinkButton>
            <div class="VotesBoxPadding">
                <asp:Label ID="lblStatus" runat="server" Text="Open"></asp:Label>
            </div>
        </div>
        <asp:HiddenField ID="hfIssueID" runat="server" />
    </div>
    </div>
    Title:<asp:TextBox ID="txtbxTitle" runat="server"></asp:TextBox></div>
<div>
    Description:<asp:TextBox ID="txtbxDescription" Rows="10" TextMode="MultiLine" runat="server"></asp:TextBox></div>
<div>
    <asp:Button ID="btnSubmit" runat="server" Text="Submit Change" OnClick="btnSubmit_Click" /></div>
