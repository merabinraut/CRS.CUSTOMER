namespace CRS.CUSTOMER.SHARED
{
    public enum NotificationMessage
    {
        WARNING = 3,
        SUCCESS = 0,
        ERROR = 1,
        INFORMATION = 2
    }

    public class NotificationModel
    {
        public string Message { get; set; }
        public string Title { get; set; }
        public NotificationMessage NotificationType { get; set; }
    }
}