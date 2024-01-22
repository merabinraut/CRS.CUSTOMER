using CRS.CUSTOMER.SHARED.BookmarkManagement;
using CRS.CUSTOMER.SHARED;
using System.Collections.Generic;

namespace CRS.CUSTOMER.BUSINESS.BookmarkManagement
{
    public interface IBookmarkManagementBusiness
    {
        List<BookmarkedClubCommon> GetBookmarkedClubList(string agentId);
        List<BookmarkedHostCommon> GetBookmarkedHostList(string agentId);
        List<GalleryImageCommon> GetGalleryImage(ClubHostManagementCommon galleryImage, string imageFor);
        CommonDbResponse ManageBoookmark(ClubHostManagementCommon manageClubBookmark, string manageFor);
    }
}