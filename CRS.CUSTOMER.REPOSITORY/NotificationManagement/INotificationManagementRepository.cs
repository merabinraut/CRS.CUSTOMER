using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.NotificationManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.REPOSITORY.NotificationManagement
{
    public interface INotificationManagementRepository
    {
        List<NotificationDetailCommon> GetNotification(string AgentId);
        List<NotificationDetailCommon> GetAllNotification(ManageNotificationCommon Request);
        bool HasUnReadNotification(string AgentId);
        CommonDbResponse ManageNotificationReadStatus(Common Request, string NotificationId);
        CommonDbResponse ManageReservationCancelRemark(Common Request, string NotificationId, string CustomerRemarks);
        CommonDbResponse ManageSingleNotificationReadStatus(Common dbRequest, string NotificationId);
    }
}
