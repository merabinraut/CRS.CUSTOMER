function ManageBookmark(ClubId, HostId, AgentType, ClickedElement) {
    EnableLoaderFunction();
    $.ajax({
        url: '/BookmarkManagement/ManageBookmark',
        type: 'POST',
        async: true,
        dataType: 'json',
        data: { clubId: ClubId, hostId: HostId, agentType: AgentType },
        success: function (data) {
            CheckIfHasRedirectURL(data);
            if (ClickedElement !== null && ClickedElement !== undefined) {
                var type = data.type;
                var imageElement = ClickedElement.querySelector('img');
                var svgElement = ClickedElement.querySelector('svg');
                if (type) {
                    var ClubFilledURL = "/Content/assets/images/bookmark-filled.svg";
                    var ClubUnFilledURL = "/Content/assets/images/bookmark-notfilled.svg";
                    var HostFilledSVGURL = `<svg xmlns="http://www.w3.org/2000/svg" width="34" height="34" viewBox="0 0 34 34" fill="none">
                                <g filter="url(#filter0_d_4820_24085)">
                                    <circle cx="17" cy="17" r="10" fill="#D75A8B" />
                                </g>
                                <path d="M14.5018 13.1543C12.9108 13.1543 11.6172 14.4578 11.6172 16.0277C11.6172 16.5754 11.8666 17.0498 12.098 17.3925C12.3293 17.7353 12.5667 17.9553 12.5667 17.9553L16.7254 22.1097L17.0018 22.3851L17.2782 22.1097L21.4369 17.9553C21.4369 17.9553 22.3864 17.1217 22.3864 16.0277C22.3864 14.4578 21.0928 13.1543 19.5018 13.1543C18.1812 13.1543 17.3308 13.946 17.0018 14.2797C16.6728 13.946 15.8224 13.1543 14.5018 13.1543Z" fill="white" />
                                <defs>
                                    <filter id="filter0_d_4820_24085" x="0" y="0" width="34" height="34" filterUnits="userSpaceOnUse" color-interpolation-filters="sRGB">
                                        <feFlood flood-opacity="0" result="BackgroundImageFix" />
                                        <feColorMatrix in="SourceAlpha" type="matrix" values="0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 127 0" result="hardAlpha" />
                                        <feOffset />
                                        <feGaussianBlur stdDeviation="3.5" />
                                        <feComposite in2="hardAlpha" operator="out" />
                                        <feColorMatrix type="matrix" values="0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0.25 0" />
                                        <feBlend mode="normal" in2="BackgroundImageFix" result="effect1_dropShadow_4820_24085" />
                                        <feBlend mode="normal" in="SourceGraphic" in2="effect1_dropShadow_4820_24085" result="shape" />
                                    </filter>
                                </defs>
                            </svg>`;
                    var HostNotFilledSVGURL = ` <svg class="inActive-svg" xmlns="http://www.w3.org/2000/svg" width="31" height="34"
                                 viewBox="0 0 31 34" fill="none">
                                <g filter="url(#filter0_d_3058_46008)">
                                    <circle cx="17" cy="17" r="10" fill="white" />
                                </g>
                                <path d="M13.0968 17.4266L13.0868 17.4167L13.0784 17.4089L13.0783 17.4088L13.0763 17.4068C13.0729 17.4036 13.0665 17.3974 13.0575 17.3883C13.0393 17.3701 13.011 17.3408 12.976 17.302C12.9051 17.2235 12.8112 17.1105 12.7196 16.9748C12.5183 16.6767 12.3672 16.3544 12.3672 16.0296C12.3672 14.8742 13.3248 13.9062 14.5018 13.9062C15.5079 13.9062 16.1721 14.5083 16.4677 14.8082L17.0018 15.3499L17.5359 14.8082C17.8316 14.5083 18.4957 13.9062 19.5018 13.9062C20.6789 13.9062 21.6364 14.8742 21.6364 16.0296C21.6364 16.3638 21.4878 16.6965 21.2937 16.9787C21.2005 17.1141 21.1068 17.2235 21.0373 17.2977C21.0029 17.3345 20.9754 17.3617 20.9582 17.3783C20.9497 17.3865 20.9437 17.392 20.9409 17.3946C20.9404 17.3951 20.94 17.3954 20.9397 17.3957C20.9395 17.3959 20.9393 17.396 20.9392 17.3961L20.9239 17.4095L20.9068 17.4266L17.0018 21.3277L13.0968 17.4266Z"
                                      stroke="#E3A1A1" stroke-width="1.5" />
                                <defs>
                                    <filter id="filter0_d_3058_46008" x="0" y="0" width="34" height="34"
                                            filterUnits="userSpaceOnUse" color-interpolation-filters="sRGB">
                                        <feFlood flood-opacity="0" result="BackgroundImageFix" />
                                        <feColorMatrix in="SourceAlpha" type="matrix"
                                                       values="0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 127 0" result="hardAlpha" />
                                        <feOffset />
                                        <feGaussianBlur stdDeviation="3.5" />
                                        <feComposite in2="hardAlpha" operator="out" />
                                        <feColorMatrix type="matrix" values="0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0.25 0" />
                                        <feBlend mode="normal" in2="BackgroundImageFix"
                                                 result="effect1_dropShadow_3058_46008" />
                                        <feBlend mode="normal" in="SourceGraphic" in2="effect1_dropShadow_3058_46008"
                                                 result="shape" />
                                    </filter>
                                </defs>
                            </svg>
                            <svg class="active-svg" xmlns="http://www.w3.org/2000/svg" width="31" height="34"
                                 viewBox="0 0 31 34" fill="none">
                                <g filter="url(#filter0_d_3058_39409)">
                                    <circle cx="17" cy="17" r="10" fill="#D75A8B" />
                                </g>
                                <path d="M14.5018 13.1562C12.9108 13.1562 11.6172 14.4598 11.6172 16.0296C11.6172 16.5774 11.8666 17.0518 12.098 17.3945C12.3293 17.7372 12.5667 17.9572 12.5667 17.9572L16.7254 22.1117L17.0018 22.387L17.2782 22.1117L21.4369 17.9572C21.4369 17.9572 22.3864 17.1236 22.3864 16.0296C22.3864 14.4598 21.0928 13.1562 19.5018 13.1562C18.1812 13.1562 17.3308 13.9479 17.0018 14.2817C16.6728 13.9479 15.8224 13.1562 14.5018 13.1562Z"
                                      fill="white" />
                                <defs>
                                    <filter id="filter0_d_3058_39409" x="0" y="0" width="34" height="34"
                                            filterUnits="userSpaceOnUse" color-interpolation-filters="sRGB">
                                        <feFlood flood-opacity="0" result="BackgroundImageFix" />
                                        <feColorMatrix in="SourceAlpha" type="matrix"
                                                       values="0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 127 0" result="hardAlpha" />
                                        <feOffset />
                                        <feGaussianBlur stdDeviation="3.5" />
                                        <feComposite in2="hardAlpha" operator="out" />
                                        <feColorMatrix type="matrix" values="0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0.25 0" />
                                        <feBlend mode="normal" in2="BackgroundImageFix"
                                                 result="effect1_dropShadow_3058_39409" />
                                        <feBlend mode="normal" in="SourceGraphic" in2="effect1_dropShadow_3058_39409"
                                                 result="shape" />
                                    </filter>
                                </defs>
                            </svg>`;
                    var HostFilledImgURL = '/Content/assets/images/loveicon-filled.svg';
                    var HostNotFilledImgURL = '/Content/assets/images/loveicon-nf.svg';
                    if (imageElement) {
                        if (AgentType == 'host') {
                            if (type == 'A') {
                                imageElement.src = HostFilledImgURL;
                            }
                            else {
                                imageElement.src = HostNotFilledImgURL;
                            }
                        }
                        else if (AgentType == 'club') {
                            if (type == 'A') {
                                imageElement.src = ClubFilledURL;
                            }
                            else {
                                imageElement.src = ClubUnFilledURL;
                            }
                        }
                    } else if (svgElement) {
                        if (AgentType == 'host') {
                            if (type == 'A') {
                                svgElement.innerHTML = HostFilledSVGURL;
                            }
                            else {
                                svgElement.innerHTML = HostNotFilledSVGURL;
                            }
                        }
                    }
                }
            }
            else {
                location.reload();
            }
            DisableLoaderFunction();
        },
        error: function () {
            DisableLoaderFunction();
            //location.reload();
        }
    })
}