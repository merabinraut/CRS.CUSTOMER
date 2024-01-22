namespace CRS.CUSTOMER.SHARED.ProfileManagement
{
    public class PasswordCommon : Common
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string UserId { get; set; }
        public string Session { get; set; }
        public string IPAddress { get; set; }
    }
}