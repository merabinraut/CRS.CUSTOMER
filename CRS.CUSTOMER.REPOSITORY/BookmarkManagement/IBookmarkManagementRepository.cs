using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.BookmarkManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.REPOSITORY.BookmarkManagement
{
    public interface IBookmarkManagementRepository
    {
        List<BookmarkedClubCommon> GetBookmarkedClubList(string agentId);
        List<BookmarkedHostCommon> GetBookmarkedHostList(string agentId);
        List<GalleryImageCommon> GetGalleryImage(ClubHostManagementCommon galleryImage, string imageFor);
        CommonDbResponse ManageBoookmark(ClubHostManagementCommon manageClubBookmark, string manageFor);
    }
}