namespace CRS.CUSTOMER.SHARED
{
    public class Common
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Action { get; set; }
        public string IpAddress { get; set; }
        public string BrowserInfo { get; set; }
        public string ActionUser { get; set; }
        public string ActionUserId { get; set; }
        public string AgentId { get; set; }
        public string ActionDate { get; set; }
        public string ActionPlatform { get; set; } = "CUSTOMER";
        public string ActionIP { get; set; }
    }
    public class StaticDataCommon
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public string ExtraData1 { get; set; }
    }
}