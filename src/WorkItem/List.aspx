<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="List.aspx.cs"
    Inherits="members_WorkItem_List" %>

<%@ Register Src="~/controls/IssueTopLinks.ascx" TagName="IssueTopLinks" TagPrefix="uc1" %>
<%@ Register Src="~/controls/IssueVotedBox.ascx" TagName="IssueVotedBox" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:IssueTopLinks ID="IssueTopLinks1" runat="server" />
    <%--  <asp:Repeater ID="rptrItems" runat="server" >
        <ItemTemplate>
            <uc2:IssueVotedBox ID="IssueVotedBox" runat="server" />
            <asp:HyperLink ID="hlTitle" runat="server"></asp:HyperLink>
            <asp:Label ID="lblDescription" runat="server" Text="Label"></asp:Label>
            <asp:Label ID="lblBottomInfo" runat="server" Text="Label"></asp:Label>
            <asp:Label ID="lbluid" Visible="false" runat="server" Text="Label"></asp:Label>
        </ItemTemplate>
    </asp:Repeater>--%>
    <div>
        Hey guys, as I continue to build the site. I would love to hear user feedback, issues,
        problems and any suggested enhancements. What comes next in this site will rely
        HIGHLY on you and wht you vote for or propose. I made it as easy as possible to
        vote, suggest and GIVE IDEAS to a site that is still a baby in many ways. So please
        explore and suggest and vote so that I can see what to build NEXT!!</div>
    <br />
    <div>
        <ul>
            <li>Let me know the following:</li>
            <li>Wrong Calculations? Mis-Calculations?</li>
            <li>Items Never showing up?</li>
            <li>Ideas you have?</li>
            <li>New things you want to see?</li>
        </ul>
    </div>
    <br />
    <div>
        I will Work on the MOST voted items first!</div>
    <br />
    
    <asp:DataList ID="dlViewItems" runat="server" CssClass="divListContainer">
        <ItemTemplate>
            <div class="divVotesBox VotesBoxMargin">
                <uc2:IssueVotedBox ID="IssueVotedBox" IssueID='<%# Eval("id") %>' Status='<%# Eval("Status") %>'
                    VoteCount='<%# Eval("VoteCount") %>' runat="server" />
            </div>
            <div class="divBasicInfo">
                <asp:HyperLink ID="hlTitle" runat="server" CssClass="hlTitleOfItem" Text='<%# Eval("Title") %>' NavigateUrl='<%# Eval("NavigateURL") %>'></asp:HyperLink>
                <div>
                    <p class="workItemDescription">
                        <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label></p>
                    <asp:HyperLink ID="hlCommentCount" runat="server" Text='<%# Eval("CommentCount") %>'
                        NavigateUrl='<%# Eval("NavigateURL") %>'></asp:HyperLink><asp:Label ID="lblBottomInfo"
                            runat="server" Text='<%# Eval("BottomLine") %>'></asp:Label><asp:Label ID="Label1"
                                runat="server" Text='<%#  SupportFramework.Users.Memberships.getUserName(new Guid( Eval("ReportedBy").ToString())) %>'></asp:Label>
                </div>
                <asp:Label ID="lbluid" Visible="false" runat="server" Text='<%# Eval("id") %>'></asp:Label>
            </div>
        </ItemTemplate>
    </asp:DataList>
</asp:Content>
