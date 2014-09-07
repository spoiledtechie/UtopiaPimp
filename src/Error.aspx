<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Error.aspx.cs"
    Inherits="Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

  

   You have been redirected here because of an error.
    <br />
    The error has been sent to the UtopiaPimp for review.<br />
    You may continue your regular operations.<br />
    <br />
    Thank You.
    <br />
    <br />
    <b>If you got here after pasting something, try copying the ENTIRE page you just selected....</b><br />
    <br />
    <asp:HyperLink ID="hlErrors" Visible="false" NavigateUrl="~/admin/admin/Errors.aspx"
        runat="server">Go to Errors</asp:HyperLink>

</asp:Content>
