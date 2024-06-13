namespace CRS.CUSTOMER.SHARED.NotificationManagement
{
    public class ManageNotificationCommon : Common
    {
        public string NotificationId { get; set; }
    }
    public class NotificationDetailCommon
    {
        public string NotificationId { get; set; }
        public string NotificationTo { get; set; }
        public string NotificationType { get; set; }
        public string NotificationSubject { get; set; }
        public string NotificationBody { get; set; }
        public string NotificationImageURL { get; set; }
        public string NotificationStatus { get; set; }
        public string NotificationReadStatus { get; set; }
        public string NotificationCount { get; set; }
        public string DateCategory { get; set; }
        public string FormattedDate { get; set; }
        public string NotificationURL { get; set; }
        public string NotificationImage { get; set; }
        public string UnReadNotification { get; set; }
        public string IsRemarksDone { get; set; }
    }
}
