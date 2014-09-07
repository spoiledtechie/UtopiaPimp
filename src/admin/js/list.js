var workingItem = -1;
var lastMessageID = -1;
var currentList = -1;
var author = '';
var messageBgColor = '#fff';

$(function () {
    $("a#showEditBoxForLists").fancybox({
        'showCloseButton': true,
        'hideOnOverlayClick': false,
        'hideOnContentClick': false
    });
    $("a#showEditBoxForItems").fancybox({
        'showCloseButton': true,
        'hideOnOverlayClick': false,
        'hideOnContentClick': false
    });
    $("a#showEditBoxForMessages").fancybox({
        'showCloseButton': true,
        'hideOnOverlayClick': false,
        'hideOnContentClick': false
    });
        
    $.ajax({
        url: 'List.aspx',
        data: ({ Callback: "LoadStartup" }),
        type: "POST",
        dataType: "json",
        success: function (data) {
            var listsData = '';
            if (data.LISTS.length > 0)
                currentList = data.LISTS[0].listID; // When this is loaded for the first time, then the first list will has it items displayed automatically. Save that id.

            for (i = 0; i < data.LISTS.length; i++) {
                if (i > 0)
                    listsData += " | ";

                listsData += GetListItemHTML(data.LISTS[i].listID, data.LISTS[i].listName);
            }
            $('#ListsBoxSpan').html(listsData);

            var itemsData = '';
            if (data.ITEMS.length > 0) {
                for (i = 0; i < data.ITEMS.length; i++)
                    itemsData += GetItemHTML(data.ITEMS[i].itemID, decodeURIComponent(data.ITEMS[i].itemSubject), decodeURIComponent(data.ITEMS[i].itemText), data.ITEMS[i].author, data.ITEMS[i].itemPosted, data.ITEMS[i].itemCompleted);
            }
            else
                itemsData = "<div>No items found</div>";
            $('#ItemBoxDiv').html(itemsData);

            var messageData = '';
            if (data.MESSAGES.length > 0) {
                for (i = 0; i < data.MESSAGES.length; i++) {
                    messageData += GetMessageItemHTML(data.MESSAGES[i].messageID, decodeURIComponent(data.MESSAGES[i].messageText), data.MESSAGES[i].author, data.MESSAGES[i].messagePosted);
                    lastMessageID = data.MESSAGES[i].messageID;
                }
            }
            else
                messageData = "<div>No messages found</div>";
            $('#MessageBoxDiv').html(messageData);

            RestartAccordion();
        },
        failed: function (data) {
            ErrorOccured('The startup data could not be loaded, an unknown error occured and the ajax request/response was incorrect');
        }
    });

    NFInit();
    window.setInterval(ListNewMessages, 60000);
});

$(document).ajaxStart(function () {
    $.blockUI({
        fadeIn: 700,
        fadeOut: 700,
        timeout: 2000,
        showOverlay: false,
        css: {
            border: 'none',
            padding: '15px',
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: .5,
            color: '#fff'
        }
    });
}).ajaxStop(function () {
    $.unblockUI();
}).ajaxError(function () {
    $.unblockUI();
});

function EditDisplayMode(showTitle, showContent, buttonTitle, eventHandler) {
    $('#editContentTitle').val("");
    $('#editContentBox').val("");

    if (showTitle)
        $('#editBoxTitleDiv').show();
    else
        $('#editBoxTitleDiv').hide();
    if (showContent)
        $('#editBoxContentDiv').show();
    else
        $('#editBoxContentDiv').hide();
    $('#editBoxButton').attr('value', buttonTitle);
    $("#editBoxButton").unbind("click");
    $('#editBoxButton').click(function () {
        eventHandler();
    });
}

function EditDisplayModeWithDefaultText(showTitle, showContent, buttonTitle, eventHandler, defaultTitle, defaultContent) {
    EditDisplayMode(showTitle, showContent, buttonTitle, eventHandler);
    $('#editContentTitle').val(defaultTitle);
    $('#editContentBox').val(defaultContent);
}

function CreateNewList() {
    $.fancybox.close();
    $.ajax({
        url: 'List.aspx',
        data: ({ Callback: "CreateNewList", ListName: $('#editContentTitle').val() }),
        type: "POST",
        dataType: "json",
        success: function (newList) {
            if (newList.status == "success") {
                $('#ListsBoxSpan').append(' | ' + GetListItemHTML(newList.listID, newList.listName));
                
            }
            else
                ErrorOccured('The list could not be created due to the following reason\\n' + newList.errorMessage);
        },
        failed: function (data) {
            ErrorOccured('The list could not be created, an unknown error occured and the ajax request/response was incorrect');
        }
    });
}

function GetListItemHTML(id, name) {
    return '<a href=\"javascript:void(0);\" onclick=\"ShowList(' + id + ');\">' + name + '</a>';
}


function CreateNewMessage() {    
    $.fancybox.close();
    $.ajax({
        url: 'List.aspx',
        data: ({
            Callback: "CreateNewMessage",
            MessageText: encodeURIComponent($('#editContentBox').val()), 
            Author: author 
        }),
        type: "POST",
        dataType: "json",
        success: function (newMessage) {
            if (newMessage.status == "success") {
                $('#MessageBoxDiv').prepend(GetMessageItemHTML(newMessage.messageID, decodeURIComponent(newMessage.messageText), newMessage.author, newMessage.messagePosted));
            }
            else
                ErrorOccured('The message could not be created due to the following reason\\n' + newMessage.errorMessage);
        },
        failed: function (data) {
            ErrorOccured('The new message could not be posted, an unknown error occured and the ajax request/response was incorrect');
        }
    });
}

function ListNewMessages() {
    $.ajax({
        url: 'List.aspx',
        data: ({ Callback: "ListNewMessages", LastMessageID: lastMessageID }),
        type: "POST",
        dataType: "json",
        success: function (data) {
            var messageData = '';
            if (data.MESSAGES.length > 0) {
                for (i = 0; i < data.MESSAGES.length; i++) {
                    messageData += GetMessageItemHTML(data.MESSAGES[i].messageID, decodeURIComponent(data.MESSAGES[i].messageText), data.MESSAGES[i].author, data.MESSAGES[i].messagePosted);
                    lastMessageID = data.MESSAGES[i].messageID;
                }
            }
            $('#MessageBoxDiv').prepend(messageData);
        },
        failed: function (data) {
            ErrorOccured('The new messages could not be fetched, an unknown error occured and the ajax request/response was incorrect');
        }
    });
}

function GetMessageItemHTML(id, message, author, dt) {
    ChangeTheMessageBackgroundColor();
    return '<div id="message_' + id + '" style=\"width: 695px; background-color: ' + messageBgColor + ';\"><div class="MessageHeadline">Posted by ' + author + ' - ' + dt + ' | <a href="javascript:void(0);" onclick="DeleteMessage(' + id + ');">Delete</a></div><p class="MessageBody">' + message + '</p></div>';
}

function DeleteMessage(id) {
    $.ajax({
        url: 'List.aspx',
        data: ({ Callback: "DeleteMessage", MessageID: id }),
        type: "POST",
        dataType: "json",
        success: function (data) {
            $('#message_' + data.messageID).remove();
        },
        failed: function (data) {
            ErrorOccured('The message could not be deleted, an unknown error occured and the ajax request/response was incorrect');
        }
    });
}

function ChangeTheMessageBackgroundColor() {
    if (messageBgColor == '#efefee')
        messageBgColor = '#fff';
    else
        messageBgColor = '#efefee';
}

function CreateNewItem() {
    $.fancybox.close();
    
    $.ajax({
        url: 'List.aspx',
        data: ({
            Callback: "CreateNewItem",
            ListID: currentList,
            ItemSubject: encodeURIComponent($('#editContentTitle').val()),
            ItemText: encodeURIComponent($('#editContentBox').val()),
            Author: author
        }),
        type: "POST",
        dataType: "json",
        success: function (newItem) {
            if (newItem.status == "success") {
                $('#ItemBoxDiv').append(GetItemHTML(newItem.itemID, decodeURIComponent(newItem.itemSubject), decodeURIComponent(newItem.itemText), author, newItem.itemPosted, newItem.itemCompleted));
                RestartAccordion();
            }
            else
                ErrorOccured('The message could not be created due to the following reason\\n' + newItem.errorMessage);
        },
        failed: function (data) {
            ErrorOccured('The item could not be created, an unknown error occured and the ajax request/response was incorrect');
        }
    });
}

function UpdateItems() {
    ShowList(currentList);
}

function ShowList(listID) {
    currentList = listID;
    $.ajax({
        url: 'List.aspx',
        data: ({ Callback: "ListItems", ListID: listID }),
        type: "POST",
        dataType: "json",
        success: function (data) {
            var itemsData = '';
            if (data.ITEMS.length > 0) {
                for (i = 0; i < data.ITEMS.length; i++)
                    itemsData += GetItemHTML(data.ITEMS[i].itemID, decodeURIComponent(data.ITEMS[i].itemSubject), decodeURIComponent(data.ITEMS[i].itemText), data.ITEMS[i].author, data.ITEMS[i].itemPosted, data.ITEMS[i].itemCompleted);
            }
            else
                itemsData = "<div>No items found</div>";
            $('#ItemBoxDiv').html(itemsData);
            RestartAccordion();
        },
        failed: function (data) {
            ErrorOccured('The list items data could not be loaded, an unknown error occured and the ajax request/response was incorrect');
        }
    });
}

function GetItemHTML(id, title, text, author, dt, completed) {
    if (completed == "false")
        return '<div id="item_' + id + '" style="width: 695px;"><h3 id="itemHead_' + id + '" class="">' + title + ', ' + dt + ', by ' + author + '</h3><div><div class="ItemTools"><a href="javascript:void(0);" onclick="GetItemDetails(' + id + ');">Edit</a> | <a href="javascript:void(0);" onclick="UpdateItemStatus(' + id + ');">Mark/Unmark completed</a> | <a href="javascript:void(0);" onclick="DeleteItem(' + id + ');">Delete</a></div><p>' + text + '</p></div></div>';
    else
        return '<div id="item_' + id + '" style="width: 695px;"><h3 id="itemHead_' + id + '" class="ItemFinished">' + title + ', ' + dt + ', by ' + author + '</h3><div><div class="ItemTools"><a href="javascript:void(0);" onclick="GetItemDetails(' + id + ');">Edit</a> | <a href="javascript:void(0);" onclick="UpdateItemStatus(' + id + ');">Mark/Unmark completed</a> | <a href="javascript:void(0);" onclick="DeleteItem(' + id + ');">Delete</a></div><p>' + text + '</p></div></div>';
}

function DeleteItem(id) {
    if (!confirm('Do you want to delete this item?'))
        return;

    $.ajax({
        url: 'List.aspx',
        data: ({ Callback: "DeleteItem", ItemID: id }),
        type: "POST",
        dataType: "json",
        success: function (data) {
            $('#item_' + data.itemID).remove();
            RestartAccordion();
        },
        failed: function (data) {
            ErrorOccured('The item could not be deleted, an unknown error occured and the ajax request/response was incorrect');
        }
    });
}

function UpdateItemStatus(id) {
    $.ajax({
        url: 'List.aspx',
        data: ({ Callback: "UpdateItemStatus", ItemID: id }),
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.status == "success") {

                if (data.textStatus == "true") {
                    $('#itemHead_' + data.itemID).addClass('ItemFinished');
                }
                else {
                    $('#itemHead_' + data.itemID).removeClass('ItemFinished');
                }
            }
        },
        failed: function (data) {
            ErrorOccured('The item could not be deleted, an unknown error occured and the ajax request/response was incorrect');
        }
    });
}

function GetItemDetails(id) {
    $.ajax({
        url: 'List.aspx',
        data: ({ Callback: "GetItemDetails", ItemID: id }),
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.status == "success") {
                workingItem = data.ITEM[0].itemID;
                EditDisplayModeWithDefaultText(true, true, "Update", UpdateItemDetails, decodeURIComponent(data.ITEM[0].itemSubject), decodeURIComponent(data.ITEM[0].itemText));
                $.fancybox({ 'showCloseButton': true, 'hideOnOverlayClick': false, 'hideOnContentClick': false, 'href': '#editBoxDiv' });
            }
            else
                ErrorOccured('The message could not be created due to the following reason\\n' + newItem.errorMessage);            
        },
        failed: function (data) {
            ErrorOccured('The item could not be deleted, an unknown error occured and the ajax request/response was incorrect');
        }
    });
}

function UpdateItemDetails() {
    $.fancybox.close();
    $.ajax({
        url: 'List.aspx',
        data: ({
            Callback: "UpdateItemDetails",
            ItemID: workingItem,
            ItemSubject: encodeURIComponent($('#editContentTitle').val()),
            ItemText: encodeURIComponent($('#editContentBox').val()) 
        }),
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.status == "success") {
                workingItem = -1;
                var itemData = GetItemHTML(data.ITEM[0].itemID, data.ITEM[0].itemSubject, data.ITEM[0].itemText, data.ITEM[0].author, data.ITEM[0].itemPosted, data.ITEM[0].itemCompleted);
                $('#item_' + data.ITEM[0].itemID).html("").html(itemData);
                RestartAccordion();
            }
            else
                ErrorOccured('The message could not be created due to the following reason\\n' + newItem.errorMessage);
        },
        failed: function (data) {
            ErrorOccured('The item could not be deleted, an unknown error occured and the ajax request/response was incorrect');
        }
    });
}

function ErrorOccured(msg) {
    alert(msg);
}

function RestartAccordion() {
    $("#ItemBoxDiv").accordion('destroy').accordion({
        header: "h3",
        collapsible: true,
        autoHeight: false,
        navigation: true
    });
}
