﻿using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.NotificationManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.BUSINESS.NotificationManagement
{
    public interface INotificationManagementBusiness
    {
        List<NotificationDetailCommon> GetNotification(string AgentId);
        List<NotificationDetailCommon> GetAllNotification(ManageNotificationCommon Request);
        CommonDbResponse ManageNotificationReadStatus(Common Request, string NotificationId);
        bool HasUnReadNotification(string AgentId);
        CommonDbResponse ManageReservationCancelRemark(Common Request, string NotificationId, string CustomerRemarks);
        CommonDbResponse ManageSingleNotificationReadStatus(Common dbRequest, string NotificationId);
    }
}
