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
        CommonDbResponse ManageNotificationReadStatus(Common Request);
    }
}
