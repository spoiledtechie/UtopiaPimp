<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    CodeFile="Voting.aspx.cs" Inherits="members_Voting" %>

<%@ MasterType VirtualPath="~/main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="center">
        <br />
        <div class="divKingSummary center">
            Here you may change the Owner of the kingdom in Pimp by voting them in or out. The
            Owners of a kingdom control all monarch abilities and is allowed appoint new monarchs
            etc...
        </div>
        <br />
        <div>
            Vote For:
            <asp:DropDownList ID="ddlVotedFor" AppendDataBoundItems="true" runat="server" DataValueField="Province_ID"
                DataTextField="Province_Name">
                <asp:ListItem Value="" Text=""></asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnVote" runat="server" Text="SubmitVote" OnClick="btnVote_Click" /></div>
        <br />
        <div>
            Votes Needed to Change Owner of Kingdom:
            <asp:Label runat="server" ID="lblVotesNeeded"></asp:Label><br />
            Current Owner:
            <asp:Label runat="server" ID="lblOwner"></asp:Label></div>
        <br />
        <div id="divVotes" runat="server">
        </div>
    </div>
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
