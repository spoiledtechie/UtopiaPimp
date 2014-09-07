<%@ Control Language="C#" AutoEventWireup="true" CodeFile="IssueVotedBox.ascx.cs"
    Inherits="controls_IssueVotedBox" %>
<asp:UpdatePanel ID="upUpdateVoteBox" ChildrenAsTriggers="true" UpdateMode="Conditional"
    runat="server">
    <ContentTemplate>
        <div class="divVotesBoxContainer centertext">
            <div class="divVoteCountContainer VotesBoxPadding">
                <div>
                    <asp:Label ID="lblVoteCount" CssClass="Bold" runat="server"></asp:Label></div>
                <div>
                    <asp:Label ID="lblVountCountText" CssClass="voteCountText" runat="server"></asp:Label>
                </div>
            </div>
            <div class="divVoteTypeContainer VotesBoxPadding">
                <asp:Label ID="lblCheckVote" runat="server" Visible="false"></asp:Label></div>
            <asp:LinkButton ID="lbVote" runat="server" Visible="false" Text="Vote" OnClick="lbVote_Click"></asp:LinkButton>
            <div class="VotesBoxPadding">
                <asp:Label ID="lblStatus" runat="server"></asp:Label><asp:DropDownList ID="ddlStatus"
                    runat="server" Visible="False" DataSourceID="sdsStatus" 
                    DataTextField="Status" DataValueField="uid" AutoPostBack="true"
                    onselectedindexchanged="ddlStatus_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:SqlDataSource ID="sdsStatus" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:UPConnectionString %>" 
                    SelectCommand="SELECT [uid], [Status] FROM [Issues_Status_Pull]">
                </asp:SqlDataSource>
            </div>
            <asp:HiddenField ID="hfIssueID" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
