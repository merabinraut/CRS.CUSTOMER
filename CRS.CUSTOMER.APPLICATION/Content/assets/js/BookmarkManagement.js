function ManageBookmark(ClubId, HostId, AgentType) {
    EnableLoaderFunction();
    $.ajax({
        url: '/BookmarkManagement/ManageBookmark',
        type: 'POST',
        data: { __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(), ClubId, HostId, AgentType },
        success: function (data) {
            DisableLoaderFunction();
            CheckIfHasRedirectURL(data);
            location.reload();
        },
        error: function () {
            DisableLoaderFunction();
            location.reload();
        }
    })
}