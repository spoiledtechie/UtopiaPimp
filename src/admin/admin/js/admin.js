function Confirm_Truncate_Table(tableName, databaseName) {
    if (confirm("Truncate " + tableName + " on database " + databaseName + "?") == true) {
        admin.ConfirmTruncateTable(tableName, databaseName, onWSCompleteTruncate);
    }
    else
        return false;
}
function onWSCompleteTruncate() {
    window.location.href = 'Database.aspx';
}


function ResetUsersAccount(username, appID) {
    admin.ResetUserAccountSettings(username, appID, onWSSuccess);
}

//UAdmin Functions
//
//
//
//
function DisconnectProvFromUser(provinceID) {
    admin.DisconnectProvinceFromUser(provinceID, onWSSuccess);
}
function onWSSuccess(results) {
    if (results === true)
        alert("deleted");
}