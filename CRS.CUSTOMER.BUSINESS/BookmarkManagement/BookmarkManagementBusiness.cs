using CRS.CUSTOMER.REPOSITORY.BookmarkManagement;
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.BookmarkManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.BUSINESS.BookmarkManagement
{
    public class BookmarkManagementBusiness : IBookmarkManagementBusiness
    {
        private readonly IBookmarkManagementRepository _repository;
        public BookmarkManagementBusiness(BookmarkManagementRepository repository) => _repository = repository;

        public List<BookmarkedClubCommon> GetBookmarkedClubList(string agentId)
        {
            return _repository.GetBookmarkedClubList(agentId);
        }

        public List<BookmarkedHostCommon> GetBookmarkedHostList(string agentId)
        {
            return _repository.GetBookmarkedHostList(agentId);
        }

        public List<GalleryImageCommon> GetGalleryImage(ClubHostManagementCommon galleryImage, string imageFor)
        {
            return _repository.GetGalleryImage(galleryImage, imageFor);
        }

        public CommonDbResponse ManageBoookmark(ClubHostManagementCommon manageClubBookmark, string manageFor)
        {
            return _repository.ManageBoookmark(manageClubBookmark, manageFor);
        }
    }
}
