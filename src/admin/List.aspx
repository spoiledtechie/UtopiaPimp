<%@ Page Language="C#" AutoEventWireup="true" CodeFile="List.aspx.cs" Inherits="admin_List" EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>List</title>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4/jquery.min.js"></script>
    <script src="../js/jquery-ui-1.8.7.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/jquery.block.js"></script>
    <script type="text/javascript" src="../js/fancybox/jquery.mousewheel-3.0.2.pack.js"></script>
    <script type="text/javascript" src="../js/fancybox/jquery.fancybox-1.3.1.js"></script>
    <script language="javascript" type="text/javascript" src="../js/niceforms/niceforms.js"></script>
    <link rel="stylesheet" type="text/css" media="all" href="../js/niceforms/niceforms-default.css" />
    <link rel="stylesheet" type="text/css" href="../js/fancybox/jquery.fancybox-1.3.1.css"
        media="screen" />
    <link rel="stylesheet" type="text/css" href="../CSS/ui-lightness/jquery-ui-1.8.7.custom.css" />
    <script type="text/javascript" src="js/list.js" language="javascript"></script>
    <style type="text/css">
    .BigBox { width: 1400px; height: auto; }
    .TopBox { width: 1400px; margin-bottom: 15px; height: auto; }
    .LeftBox { float: left; width: 695px; height: auto; margin-right: 10px; }
    .RightBox { float: left; width: 695px; height: auto; }
    .ListSize { font-size: larger; }
    .MessageHeadline { font-size: large; }
    .MessageBody { font-size: normal; }  
    .ItemTools { font-size: x-small; }  
    .ItemFinished { text-decoration: line-through; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="BigBox">
        <div class="TopBox">
           <span class="ListSize"><a href="Default.aspx">Back to admin</a> - Lists: </span><span id="ListsBoxSpan" class="ListSize"></span><span class="ListSize"> - <a id="showEditBoxForLists" href="#editBoxDiv" onclick="EditDisplayMode(true, false, 'Create', CreateNewList);">New list</a></span></div>
        <div class="LeftBox">
            <p><a id="showEditBoxForItems" href="#editBoxDiv" onclick="EditDisplayMode(true, true, 'Create', CreateNewItem);">New item</a></p>
            <div id="ItemBoxDiv"></div>
        </div>
        <div class="RightBox">
            <p><a id="showEditBoxForMessages" href="#editBoxDiv" onclick="EditDisplayMode(false, true, 'Create', CreateNewMessage);">New message</a></p>  
            <div id="MessageBoxDiv"></div>
        </div>
    </div>
    <div style="display: none">
        <div id="editBoxDiv">
            <div id="editBoxTitleDiv">
                <span style="font-weight: bold;">Title:</span><br />
                <input type="text" id="editContentTitle" name="editContentTitle" style="width: 250px;" />
            </div>
            <div id="editBoxContentDiv">
                <span style="font-weight: bold;">Text:</span><br />
                <textarea id="editContentBox" name="editContentBox" rows="20" cols="80"></textarea><br />
            </div>
            <input type="submit" id="editBoxButton" value="" />
        </div>
    </div>    
    </form>
</body>
</html>
