<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="CreateItem.aspx.cs" Inherits="members_WorkItem_CreateItem"  %>

<%@ Register src="../controls/IssueCreateItem.ascx" tagname="IssueCreateItem" tagprefix="uc2" %>
<%@ Register src="../controls/IssueTopLinks.ascx" tagname="IssueTopLinks" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <uc1:IssueTopLinks ID="IssueTopLinks1" runat="server" />
    <uc2:IssueCreateItem ID="IssueCreateItem1" runat="server" />
</asp:Content>

