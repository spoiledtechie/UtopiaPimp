<%@ Page Language="C#" MasterPageFile="~/LoggedOut.master" AutoEventWireup="true"
    CodeFile="password.aspx.cs" Inherits="anonymous_password" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="divUserName" runat="server">
        Username: <asp:TextBox ID="tbUserName" runat="server"></asp:TextBox><br />
        <asp:Button ID="btnUserName" runat="server" Text="Check Username" 
            onclick="btnUserName_Click" />
    </div>
    <div id="divQuestion" runat="server">
        <ul class="ulLists">
            <li>Question:
                <asp:Label ID="lblQuestion" runat="server"></asp:Label></li>
            <li>Answer:
                <asp:TextBox ID="tbAnswer" runat="server"></asp:TextBox> Please Fill out the answer to the question.</li>
        </ul>
        <asp:Button ID="btnQuestion" runat="server" Text="Get Password" 
            onclick="btnQuestion_Click" />
    </div>
    <div>
        <asp:Label ID="lblWarning" runat="server"></asp:Label></div>
    <asp:HiddenField ID="hfUserName" runat="server" />
</asp:Content>
