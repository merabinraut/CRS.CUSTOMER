using CRS.CUSTOMER.REPOSITORY.NotificationManagement;
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.NotificationManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.BUSINESS.NotificationManagement
{
    public class NotificationManagementBusiness : INotificationManagementBusiness
    {
        private readonly INotificationManagementRepository _repo;
        public NotificationManagementBusiness() => _repo = new NotificationManagementRepository();

        public List<NotificationDetailCommon> GetAllNotification(ManageNotificationCommon Request)
        {
            return _repo.GetAllNotification(Request);
        }

        public List<NotificationDetailCommon> GetNotification(string AgentId)
        {
            return _repo.GetNotification(AgentId);
        }

        public bool HasUnReadNotification(string AgentId)
        {
            return _repo.HasUnReadNotification(AgentId);
        }

        public CommonDbResponse ManageNotificationReadStatus(Common Request)
        {
            return _repo.ManageNotificationReadStatus(Request);
        }
        public CommonDbResponse ManageReservationCancelRemark(Common Request, string NotificationId, string CustomerRemarks)
        {
            return _repo.ManageReservationCancelRemark(Request, NotificationId, CustomerRemarks);
        }

        public CommonDbResponse ManageSingleNotificationReadStatus(Common dbRequest, string NotificationId)
        {
            return _repo.ManageSingleNotificationReadStatus(dbRequest, NotificationId);
        }
    }
}
