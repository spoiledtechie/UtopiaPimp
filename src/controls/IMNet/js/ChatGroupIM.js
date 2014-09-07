/// <reference path="jquery-1.2.6.min.js" />

$(document).ready(function () {
    var objDiv = document.getElementById('divChatHistory');
    objDiv.scrollTop = objDiv.scrollHeight;

    var recipientName = document.getElementById('hfRecipient').value; //Contains the GUID of the Chat.
    var senderName = document.getElementById('hfSender').value;
    //sends a request to get messages from the database.
    $.timer(3000, function (timer) {
        //gets messages every 3 seconds.
        getGroupMessages(senderName, recipientName);
    });
    $.timer(60000, function (timer) {
        Chat.GetUsersOnline(document.getElementById('hfRecipient').value, OnWSReturnSetList);
        Chat.keepMeAlive();
    });
    $.timer(180000, function (timer) {
        Chat.GetServerTime(OnWSServerTimeComplete);
        Chat.ClearOldMessages();
    });
});

///<summary>
/// sends the message to be inserted into the databse.
///</summary>
///<param name="message" optional="false">this is the actual message.</param>
function SetGroupMessages(message) {
    var senderName = document.getElementById('hfRecipient').value;
    var recipientName = document.getElementById('hfSender').value;
    var objDiv = document.getElementById('divChatHistory');
    objDiv.innerHTML += "<div class=\"spanReceiver\">" + document.getElementById('hfNickName').value + ": " + message.replace("<p>", "").replace("</p>", "") + "</div>";
    objDiv.scrollTop = objDiv.scrollHeight;
    Chat.setGroupMessage(message, recipientName, senderName);
}

function OnWSServerTimeComplete(results) {
    document.getElementById('divChatHistory').innerHTML += "<div class=\"spanServerTime\"> Server time is now " + results + "</div>";
}

///<summary>
/// gets the messages from the database.
///</summary>
///<param name="sender" optional="false">the sender user name</param>
///<param name="recipient" optional="false">the recipient user name</param>
function getGroupMessages(sender, recipient) {
    //    alert(document.getElementById('hfLastCheckedUID').value);
    var lastChecked = document.getElementById('hfLastCheckedUID').value;
    Chat.getGroupMessagesChatting(sender, recipient, lastChecked, OnWSRequestCompleteGroup);
};

///<summary>
/// returns the messages from the database
///</summary>
///<param name="results" optional="false">the messages in a string[] value</param>
function OnWSRequestCompleteGroup(results) {
    var objDiv = document.getElementById('divChatHistory');
    if (results[1].length > 0)
        document.getElementById('hfLastCheckedUID').value = results[1];
    if (results[0] != '') {
        objDiv.innerHTML += results[0];
        objDiv.scrollTop = objDiv.scrollHeight;
    }
};
function OnWSRequestCompleteGroupSendingMessage(results) {
    var objDiv = document.getElementById('divChatHistory');
    objDiv.innerHTML += results;
    objDiv.scrollTop = objDiv.scrollHeight;
};

///<summary>
/// sets the lists content
///</summary>
///<param name="results" optional="false">sets the results of the list.</param>
function OnWSReturnSetList(results) {
    document.getElementById("divGroupChatList").innerHTML = results;
}
