//<a href="javascript:commonPopup('/news/pictures/rpSlideshows?articleId=USRTX8WVW20081105',920,585,4,'Chat');">Text</a>
///<summary>
///  creates a popup with the specified settings
///</summary>
///<param name="url" optional="false">url to pop up into.</param>
///<param name="width" optional="false">width of window</param>
///<param name="height" optional="false">height of window</param>
///<param name="toolsInd" optional="false">id of type of window</param>
///<param name="wname" optional="true">Window name.</param>
function commonPopup(url, width, height, toolsInd, wname) {
    var options = "width=" + width + ",height=" + height + ",top=" + ((screen.height - height) / 4).toString() + ",left=" + ((screen.width - width) / 2).toString();
    switch (toolsInd) {
        case 1:
            options += ",toolbar=no,status=no,resizable=no,scrollbars=yes";
            break;
        case 2:
            options += ",menubar=yes,toolbar=yes,status=yes,resizable=yes,location=yes,scrollbars=yes";
            break;
        case 3:
            options += ",top=50,left=50,resizable=yes,scrollbars=yes,status=no,menubar=no,toolbar=no,location=yes";
            break;
        case 4:
            options += ",top=50,left=50,resizable=yes,scrollbars=no,status=no,menubar=no,toolbar=no,location=yes";
            break;
        default:
            //do nothing
            break;
    }
    //if no name exists, crate one.
    if (!wname) {
        wname = "Chat";
    }
  var  popupWindow = window.open(url, wname, options);
    if (popupWindow) {
        popupWindow.focus();
    }
};

///<summary>
/// clears the list box.
///</summary>
///<param name="results" optional="false">the listbox to clear.</param>
function clearlistbox(lb) {
    for (var i = lb.options.length - 1; i >= 0; i--) {
        lb.options[i] = null;
    }
    lb.selectedIndex = -1;
};


