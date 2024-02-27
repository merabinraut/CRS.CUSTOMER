using CRS.CUSTOMER.BUSINESS.BookmarkManagement;
using CRS.CUSTOMER.BUSINESS.CommonManagement;
using CRS.CUSTOMER.BUSINESS.Dashboard;
using CRS.CUSTOMER.BUSINESS.DashboardV2;
using CRS.CUSTOMER.BUSINESS.Home;
using CRS.CUSTOMER.BUSINESS.LocationManagement;
using CRS.CUSTOMER.BUSINESS.LogManagement.APILogManagement;
using CRS.CUSTOMER.BUSINESS.LogManagement.EmailLogManagement;
using CRS.CUSTOMER.BUSINESS.LogManagement.ErrorLogManagement;
using CRS.CUSTOMER.BUSINESS.NotificationManagement;
using CRS.CUSTOMER.BUSINESS.ProfileManagement;
using CRS.CUSTOMER.BUSINESS.RecommendedClubHost;
using CRS.CUSTOMER.BUSINESS.ReservationHistoryManagementV2;
using CRS.CUSTOMER.BUSINESS.ReservationManagement;
using CRS.CUSTOMER.BUSINESS.ReservationManagementV2;
using CRS.CUSTOMER.BUSINESS.ReviewManagement;
using CRS.CUSTOMER.BUSINESS.Search;
using CRS.CUSTOMER.BUSINESS.SearchFilterManagement;
using CRS.CUSTOMER.BUSINESS.UserManagement;
using System.Web.Mvc;
using Unity;
using Unity.AspNet.Mvc;

namespace CRS.CUSTOMER.APPLICATION
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<IHomeBusiness, HomeBusiness>();
            container.RegisterType<IUserManagementBusiness, UserManagementBusiness>();
            container.RegisterType<IAPILogManagementBusiness, APILogManagementBusiness>();
            container.RegisterType<IEmailLogManagementBusiness, EmailLogManagementBusiness>();
            container.RegisterType<IErrorLogManagementBusiness, ErrorLogManagementBusiness>();
            container.RegisterType<IProfileManagementBusiness, ProfileManagementBusiness>();
            container.RegisterType<ICommonManagementBusiness, CommonManagementBusiness>();
            container.RegisterType<INotificationManagementBusiness, NotificationManagementBusiness>();
            container.RegisterType<IDashboardBusiness, DashboardBusiness>();
            container.RegisterType<ILocationManagementBusiness, LocationManagementBusiness>();
            container.RegisterType<IReservationManagementBusiness, ReservationManagementBusiness>();
            container.RegisterType<ISearchFilterManagementBusiness, SearchFilterManagementBusiness>();
            container.RegisterType<IReviewManagementBusiness, ReviewManagementBusiness>();
            container.RegisterType<IBookmarkManagementBusiness, BookmarkManagementBusiness>();
            container.RegisterType<IRecommendedClubHostBusiness, RecommendedClubHostBusiness>();
            container.RegisterType<IReservationHistoryManagementV2Business, ReservationHistoryManagementV2Business>();
            container.RegisterType<IReservationManagementV2Business, ReservationManagementV2Business>();
            container.RegisterType<IDashboardV2Business, DashboardV2Business>();
            container.RegisterType<ISearchBusiness, SearchBusiness>();
            return container;
        }
    }
}