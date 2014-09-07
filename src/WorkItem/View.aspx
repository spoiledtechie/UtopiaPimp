<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="View.aspx.cs"
    Inherits="members_WorkItem_View" %>

<%@ Register Src="~/controls/IssueTopLinks.ascx" TagName="IssueTopLinks" TagPrefix="uc1" %>
<%@ Register Src="~/controls/IssueVotedBox.ascx" TagName="IssueVotedBox" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:IssueTopLinks ID="IssueTopLinks1" runat="server" />
    <div>
        <div class="divItemContainer">
            <div class="divVotesBox VotesBoxMargin">
                <uc2:IssueVotedBox ID="IssueVotedBox" runat="server" />
            </div>
            <div class="divContainerSingleItem">
                <div>
                    <asp:Label ID="lblTitle" CssClass="hlTitleOfItem" runat="server"></asp:Label></div>
                <div class="litDescriptionItem">
                    <asp:Literal ID="litDescriptionInsert" runat="server"></asp:Literal></div>
                <div class="divCommentContainer">
                    <asp:DataList ID="dlComments" runat="server">
                        <ItemTemplate>
                            <div class="divUserNameComment">
                                <asp:Label ID="lblUserName" Text='<%# Eval("UserName") %>' runat="server"></asp:Label></div>
                            <asp:Literal ID="litComment" runat="server" Text='<%# Eval("Comment") %>'></asp:Literal>
                        </ItemTemplate>
                    </asp:DataList>
                </div>
                <div class="divCommentTextboxContainer">
                    <div>
                        <asp:Label ID="lblAddComments" runat="server" Text="Add Comment"></asp:Label>
                    </div>
                    <div>
                        <asp:TextBox ID="txtbxAddComment" TextMode="MultiLine" CssClass="txtbxAddComments"
                            Rows="5" runat="server"></asp:TextBox>
                    </div>
                    <div>
                        <asp:Button ID="btnSubmitComment" runat="server" Text="Post" OnClick="btnSubmitComment_Click" /></div>
                </div>
            </div>
        </div>
        <div class="divItemDetails">
            <ul class="ulWorkList">
                <li class="liWorkList">
                   <asp:Label ID="lblRightColumnTitle" CssClass="Bold" runat="server" Text="Work Item Details"></asp:Label></li>
                <li class="liWorkList">
                    <asp:Label ID="lblReportedOn" runat="server" Text="Reported On "></asp:Label><asp:Label
                        ID="lblReportedOnInsert" runat="server"></asp:Label></li>
                <li class="liWorkList">
                    <asp:Label ID="lblReportedBy" runat="server" Text="Reported By "></asp:Label><asp:Label
                        ID="lblReportedByInsert" runat="server"></asp:Label></li>
                <li class="liWorkList">
                    <asp:Label ID="lblUpdatedOn" runat="server" Text="Updated On "></asp:Label><asp:Label
                        ID="lblUpdatedOnInsert" runat="server"></asp:Label></li>
                <li class="liWorkList">
                    <asp:Label ID="lblUpdatedBy" runat="server" Text="Updated By "></asp:Label><asp:Label
                        ID="lblUpdatedByInsert" runat="server"></asp:Label></li>
                <li class="liWorkList">
                    <asp:Label ID="lblClosedOn" runat="server" Text="Closed By "></asp:Label><asp:Label
                        ID="lblClosedOnInsert" runat="server"></asp:Label></li>
                <li class="liWorkList">
                    <asp:Label ID="lblClosedBy" runat="server" Text="Closed By "></asp:Label><asp:Label
                        ID="lblClosedByInsert" runat="server"></asp:Label></li>
            </ul>
            
        </div>
    </div>
    <asp:HiddenField ID="hfItemID" runat="server" />
</asp:Content>
