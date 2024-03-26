function AddBookmark(ClubId, HostId, AgentType) {
    EnableLoaderFunction();
    $.ajax({
        url: '/BookmarkManagement/AddBookmarks',
        type: 'POST',
        data: { ClubId, HostId, AgentType },
        success: function (data) {
            DisableLoaderFunction();
            location.reload();
        },
        error: function () {
            DisableLoaderFunction();
            location.reload();
        }
    })
}

function RemoveBookmark(ClubId, HostId, AgentType) {
    EnableLoaderFunction();
    $.ajax({
        url: '/BookmarkManagement/RemoveBookmarks',
        type: 'POST',
        data: { ClubId, HostId, AgentType },
        success: function (data) {
            DisableLoaderFunction();
            location.reload();
        },
        error: function () {
            DisableLoaderFunction();
            location.reload();
        }
    })
}