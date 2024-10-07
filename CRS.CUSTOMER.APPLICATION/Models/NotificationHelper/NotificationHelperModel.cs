using System;
using System.ComponentModel.DataAnnotations;

namespace CRS.CUSTOMER.APPLICATION.Models.NotificationHelper
{
    public class NotificationHelperCommonAPIResponseModel
    {
        public string code { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }

    public class NotificationManagementModel
    {
        public string agentId { get; set; }
        public string notificationType { get; set; }
        public string extraId1 { get; set; }
        public string actionPlatform { get; set; }
    }

    public class NotificationModel
    {
        public string notificationId { get; set; }
        public string notificationType { get; set; }
        public string notificationSubject { get; set; }
        public string notificationBody { get; set; }
        public string secondNotificationBody { get; set; }
        public string notificationImageURL { get; set; }
        public string notificationReadStatus { get; set; }
        public string dateCategory { get; set; }
        public string formattedDate { get; set; }
        public string notificationURL { get; set; }
        public string notificationImage { get; set; }
        public string createdDate { get; set; }
        public int unReadNotification { get; set; } = 0;
        public int totalRecords { get; set; } = 0;
    }

    public class NotificationReadRequestModel
    {
        public string notificationId { get; set; }
        public string agentId { get; set; }
        public string actionPlatform { get; set; }
    }

    public class NotificationReadResponseModel
    {
        public string[] notificationId { get; set; } = Array.Empty<string>();
        public int notificationUnReadCount { get; set; } = 0;
    }

    public class LoginRequestModel
    {
        public string userName { get; set; }
        public string password { get; set; }
        public string actionPlatform { get; set; }
    }

    public class LoginResponseModel
    {
        public string token { get; set; }
    }
}
