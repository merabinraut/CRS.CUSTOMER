using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.NotificationManagement;
using System.Collections.Generic;
using System.Linq;

namespace CRS.CUSTOMER.REPOSITORY.NotificationManagement
{
    public class NotificationManagementRepository : INotificationManagementRepository
    {
        private readonly RepositoryDao _dao;
        public NotificationManagementRepository() => _dao = new RepositoryDao();

        public List<NotificationDetailCommon> GetNotification(string AgentId)
        {
            string SQL = "sproc_customer_notification_management @Flag='s'";
            SQL += ",@AgentId=" + _dao.FilterString(AgentId);
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0) return _dao.DataTableToListObject<NotificationDetailCommon>(dbResponse).ToList();
            return new List<NotificationDetailCommon>();
        }

        public List<NotificationDetailCommon> GetAllNotification(ManageNotificationCommon Request)
        {
            string SQL = "sproc_customer_notification_management @Flag='s'";
            SQL += ",@AgentId=" + _dao.FilterString(Request.AgentId);
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0) return _dao.DataTableToListObject<NotificationDetailCommon>(dbResponse).ToList();
            return new List<NotificationDetailCommon>();
        }

        public bool HasUnReadNotification(string AgentId)
        {
            string SQL = "sproc_customer_notification_management @Flag='cichurn'";
            SQL += ",@AgentId=" + _dao.FilterString(AgentId);
            var dbResponse = _dao.ExecuteDataRow(SQL);
            if (dbResponse != null)
            {
                var code = _dao.ParseColumnValue(dbResponse, "Code").ToString();
                if (!string.IsNullOrEmpty(code) && code.Trim() == "0") return true;
            }
            return false;
        }

        public CommonDbResponse ManageNotificationReadStatus(Common Request)
        {
            string SQL = "sproc_customer_notification_management @Flag='unrs'";
            SQL += ",@AgentId=" + _dao.FilterString(Request.AgentId);
            SQL += ",@ActionUser=" + _dao.FilterString(Request.ActionUser);
            return _dao.ParseCommonDbResponse(SQL);
        }
        public CommonDbResponse ManageReservationCancelRemark(Common Request, string NotificationId, string CustomerRemarks)
        {
            string SQL = "sproc_customer_notification_management @Flag='urc'";
            SQL += ",@AgentId=" + _dao.FilterString(Request.AgentId);
            SQL += ",@ActionUser=" + _dao.FilterString(Request.ActionUser);
            SQL += ",@NotificationId=" + _dao.FilterString(NotificationId);
            SQL += ",@CustomerRemarks=" + _dao.FilterString(CustomerRemarks);
            return _dao.ParseCommonDbResponse(SQL);
        }

        public CommonDbResponse ManageSingleNotificationReadStatus(Common dbRequest, string NotificationId)
        {
            string sp_name = "sproc_customer_notification_management @Flag='msnrs'";
            sp_name += ",@notificationId=" + _dao.FilterString(NotificationId);
            sp_name += ",@AgentId=" + _dao.FilterString(dbRequest.AgentId);
            sp_name += ",@ActionUser=" + _dao.FilterString(dbRequest.ActionUser);
            sp_name += ",@ActionIP=" + _dao.FilterString(dbRequest.ActionIP);
            sp_name += ",@ActionPlatform=" + _dao.FilterString(dbRequest.ActionPlatform);
            return _dao.ParseCommonDbResponse(sp_name);
        }
    }
}