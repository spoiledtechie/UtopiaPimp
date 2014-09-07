<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestAddData.aspx.cs" Inherits="Sandbox_TestAddData" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Data:<br />
        <asp:TextBox ID="dataToAdd" runat="server" Rows="40" Columns="120" TextMode="MultiLine">
        </asp:TextBox>
        <br />
        <asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="btnSubmit_Click" />
    </div>
    </form>
</body>
</html>
