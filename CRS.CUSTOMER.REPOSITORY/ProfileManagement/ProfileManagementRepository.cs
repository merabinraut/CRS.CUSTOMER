using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.ProfileManagement;
using CRS.CUSTOMER.SHARED.SearchFilterManagement;
using System.Collections.Generic;
using System.Linq;

namespace CRS.CUSTOMER.REPOSITORY.ProfileManagement
{
    public class ProfileManagementRepository : IProfileManagementRepository
    {
        private readonly RepositoryDao _dao;

        public ProfileManagementRepository() => this._dao = new RepositoryDao();

        public CommonDbResponse ChangePassword(PasswordCommon passwordCommon)
        {
            string sql = "sproc_customer_profile_management";
            sql += " @Flag='cp'";
            sql += " ,@CurrentPassword= N" + _dao.FilterString(passwordCommon.OldPassword);
            sql += " ,@NewPassword=N" + _dao.FilterString(passwordCommon.ConfirmPassword);
            sql += " ,@UserId=" + _dao.FilterString(passwordCommon.ActionUserId);
            sql += " ,@AgentId=" + _dao.FilterString(passwordCommon.AgentId);
            sql += " ,@Session=" + _dao.FilterString(passwordCommon.Session);
            sql += " ,@ActionPlatform=" + _dao.FilterString(passwordCommon.BrowserInfo);
            sql += " ,@ActionIP=" + _dao.FilterString(passwordCommon.IPAddress);
            sql += " ,@ActionUser=" + _dao.FilterString(passwordCommon.ActionUser);
            return _dao.ParseCommonDbResponse(sql);
        }

        public CommonDbResponse ChangeProfileImage(UserProfileCommon common)
        {
            var sql = "sproc_customer_profile_management @flag='uimg'";
            sql += " ,@ProfileImage=" + _dao.FilterString(common.ProfileImage);
            sql += " ,@AgentId=" + _dao.FilterString(common.AgentId);
            sql += " ,@UserId=" + _dao.FilterString(common.ActionUserId);
            sql += " ,@ActionPlatform=" + _dao.FilterString(common.BrowserInfo);
            sql += " ,@ActionIP=" + _dao.FilterString(common.ActionIP);
            sql += " ,@ActionUser=" + _dao.FilterString(common.ActionUser);
            return _dao.ParseCommonDbResponse(sql);
        }

        public CommonDbResponse DeleteCustomerAccount(string customerId, string actionUser, string actionIp, string actionPlatform)
        {
            string sp_name = "EXEC sproc_customer_profile_management @Flag='dca'";
            sp_name += ",@AgentId=" + _dao.FilterString(customerId);
            sp_name += ",@ActionUser=" + _dao.FilterString(actionUser);
            sp_name += ",@ActionIp=" + _dao.FilterString(actionIp);
            sp_name += ",@ActionPlatform=" + _dao.FilterString(actionPlatform);
            return _dao.ParseCommonDbResponse(sp_name);

        }

        public UserProfileCommon GetUserProfileDetail(UserProfileCommon userProfileCommon)
        {
            var profile = new UserProfileCommon();
            string sql = "sproc_customer_profile_management @flag='s'";
            sql += ", @userId= " + _dao.FilterString(userProfileCommon.ActionUserId);
            sql += ", @AgentId= " + _dao.FilterString(userProfileCommon.AgentId);
            var dr = _dao.ExecuteDataRow(sql);
            if (dr != null)
            {
                profile.ActionUserId = userProfileCommon.ActionUserId;
                profile.NickName = dr["NickName"].ToString();
                profile.FirstName = dr["FirstName"].ToString();
                profile.LastName = dr["LastName"].ToString();
                profile.MobileNumber = dr["MobileNumber"].ToString();
                profile.DateOfBirth = dr["DOB"].ToString();
                profile.EmailAddress = dr["EmailAddress"].ToString();
                profile.Gender = dr["Gender"].ToString();
                profile.PreferredLocation = dr["PreferredLocation"].ToString();
                profile.PostalCode = dr["PostalCode"].ToString();
                profile.Prefecture = dr["Prefecture"].ToString();
                profile.City = dr["City"].ToString();
                profile.Street = dr["Street"].ToString();
                profile.ResidenceNumber = dr["ResidenceNumber"].ToString();
                profile.ProfileImage = dr["ProfileImage"].ToString();
            }
            return profile;
        }

        public CommonDbResponse UpdateUserProfileDetail(UserProfileCommon userProfileCommon)
        {
            string sql = "sproc_customer_profile_management";
            sql += " @flag= 'u'";
            sql += ",  @NickName=" + _dao.FilterString(userProfileCommon.NickName);

            sql += string.IsNullOrEmpty(userProfileCommon.FirstName)? ",  @FirstName=" + _dao.FilterString(userProfileCommon.FirstName) : ",  @FirstName=N" + _dao.FilterString(userProfileCommon.FirstName);
            sql += string.IsNullOrEmpty(userProfileCommon.LastName) ? ", @LastName=" + _dao.FilterString(userProfileCommon.LastName) : ", @LastName=N" + _dao.FilterString(userProfileCommon.LastName);

            sql += ", @MobileNumber=" + _dao.FilterString(userProfileCommon.MobileNumber);
            sql += ", @DOB=" + _dao.FilterString(userProfileCommon.DateOfBirth);
            sql += ", @EmailAddress=" + _dao.FilterString(userProfileCommon.EmailAddress);
            sql += ", @Gender=" + _dao.FilterString(userProfileCommon.Gender);
            sql += ", @PreferredLocation=" + _dao.FilterString(userProfileCommon.PreferredLocation);
            sql += ", @PostalCode=" + _dao.FilterString(userProfileCommon.PostalCode);
            sql += ", @Prefecture=" + _dao.FilterString(userProfileCommon.Prefecture);

            sql += string.IsNullOrEmpty(userProfileCommon.City) ? ", @City=" + _dao.FilterString(userProfileCommon.City) : ", @City=N" + _dao.FilterString(userProfileCommon.City);
            sql += string.IsNullOrEmpty(userProfileCommon.Street) ? ", @Street=" + _dao.FilterString(userProfileCommon.Street): ", @Street=N" + _dao.FilterString(userProfileCommon.Street);

            sql += string.IsNullOrEmpty(userProfileCommon.ResidenceNumber) ?  ", @ResidenceNumber=" + _dao.FilterString(userProfileCommon.ResidenceNumber): ", @ResidenceNumber=N" + _dao.FilterString(userProfileCommon.ResidenceNumber);
            sql += ", @ProfileImage=" + _dao.FilterString(userProfileCommon.ProfileImage);
            sql += ", @Session=" + _dao.FilterString(userProfileCommon.Session);
            sql += ", @UserId=" + _dao.FilterString(userProfileCommon.ActionUserId);
            sql += ", @AgentId=" + _dao.FilterString(userProfileCommon.AgentId);
            sql += ", @ActionUser=" + _dao.FilterString(userProfileCommon.ActionUser);
            return _dao.ParseCommonDbResponse(sql);
        }

        public List<PointReportCommon> GetCustomerPointsReport(string customerId,string TxnType)
        {
            string SQL = "EXEC sproc_customer_get_points_report ";
            SQL += !string.IsNullOrEmpty(customerId) ? " @customerid=" + _dao.FilterString(customerId) : "";
            SQL += !string.IsNullOrEmpty(TxnType) ? ",@TransactionMode=" + _dao.FilterString(TxnType) : "";
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0) return _dao.DataTableToListObject<PointReportCommon>(dbResponse).ToList();
            return new List<PointReportCommon>();
        }
    }
}