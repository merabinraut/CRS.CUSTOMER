function ManageBookmark(ClubId, HostId, AgentType, ClickedElement) {
    EnableLoaderFunction();
    var imageElement = clickedElement.querySelector('img');
    var svgElement = clickedElement.querySelector('svg');
    $.ajax({
        url: '/BookmarkManagement/ManageBookmark',
        type: 'POST',
        async: true,
        dataType: 'json',
        data: { clubId: ClubId, hostId: HostId, agentType: AgentType },
        success: function (data) {
            CheckIfHasRedirectURL(data);
            var type = data.type;
            if (type) {
                var ClubFilledURL = "~/Content/assets/images/bookmark-filled.svg";
                var ClubUnFilledURL = "~/Content/assets/images/bookmark-notfilled.svg";
                if (imageElement) {
                    imageElement.src = newImageUrl;
                } else if (svgElement) {
                    svgElement.querySelector('circle').setAttribute('fill', 'red'); // For example, changing the circle color
                }
            }
            DisableLoaderFunction();
            //location.reload();
        },
        error: function () {
            DisableLoaderFunction();
            //location.reload();
        }
    })
}