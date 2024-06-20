using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.Home;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CRS.CUSTOMER.REPOSITORY.Home
{
    public class HomeRepository : IHomeRepository
    {
        readonly RepositoryDao _dao;
        public HomeRepository()
        {
            _dao = new RepositoryDao();
        }
        #region Registration Management
        public CommonDbResponse RegistrationHold(RegistrationHoldCommon Request)
        {
            string SQL = "EXEC sproc_customer_registration_management @Flag='rhc'";
            SQL += ",@MobileNumber=" + _dao.FilterString(Request.MobileNumber);
            SQL += ",@NickName=N" + _dao.FilterString(Request.NickName);
            SQL += ",@ActionUser=" + _dao.FilterString(Request.ActionUser);
            SQL += ",@ActionPlatform=" + _dao.FilterString(Request.ActionPlatform);
            SQL += ",@ActionIP=" + _dao.FilterString(Request.ActionIP);
            return _dao.ParseCommonDbResponse(SQL);
        }

        public CommonDbResponse ResendRegistrationOTP(ResendRegistrationOTPCommon Request)
        {
            string SQL = "EXEC sproc_customer_registration_management @Flag='rrotp'";
            SQL += ",@MobileNumber=" + _dao.FilterString(Request.MobileNumber);
            SQL += ",@AgentId=" + _dao.FilterString(Request.AgentId);
            SQL += ",@ActionUser=" + _dao.FilterString(Request.ActionUser);
            SQL += ",@ActionPlatform=" + _dao.FilterString(Request.ActionPlatform);
            SQL += ",@ActionIP=" + _dao.FilterString(Request.ActionIP);
            return _dao.ParseCommonDbResponse(SQL);
        }

        public CommonDbResponse Register(RegistrationCommon Request)
        {
            string SQL = "EXEC sproc_customer_registration_management @Flag='vrotp'";
            SQL += ",@MobileNumber=" + _dao.FilterString(Request.MobileNumber);
            SQL += ",@AgentId=" + _dao.FilterString(Request.AgentId);
            SQL += ",@VerificationCode=" + _dao.FilterString(Request.VerificationCode);
            SQL += ",@ActionUser=" + _dao.FilterString(Request.ActionUser);
            SQL += ",@ActionPlatform=" + _dao.FilterString(Request.ActionPlatform);
            SQL += ",@ActionIP=" + _dao.FilterString(Request.ActionIP);
            SQL += !string.IsNullOrEmpty(Request.ReferCode) ? ",@ReferCode=" + _dao.FilterString(Request.ReferCode) : null;
            return _dao.ParseCommonDbResponse(SQL);
        }

        public CommonDbResponse SetRegistrationPassword(SetRegistrationPasswordCommon Request)
        {
            string SQL = "EXEC sproc_customer_registration_management @Flag='srp'";
            SQL += ",@UserId=" + _dao.FilterString(Request.UserId);
            SQL += ",@AgentId=" + _dao.FilterString(Request.AgentId);
            SQL += ",@Password=" + _dao.FilterString(Request.Password);
            SQL += ",@ActionUser=" + _dao.FilterString(Request.ActionUser);
            SQL += ",@ActionPlatform=" + _dao.FilterString(Request.ActionPlatform);
            SQL += ",@ActionIP=" + _dao.FilterString(Request.ActionIP);
            return _dao.ParseCommonDbResponse(SQL);
        }

        public CommonDbResponse VerifyAffiliateOtp(RegistrationCommon common)
        {
            return new CommonDbResponse();
            //string sp_name = "EXEC sproc_customer_registration_management @Flag = 'vrotp'";
            //sp_name += ",@MobileNumber=" + _dao.FilterString(common.MobileNumber);
            //sp_name += ",@AgentId=" + _dao.FilterString(common.AgentId);
            //sp_name += ",@VerificationCode=" + _dao.FilterString(common.VerificationCode);
            //sp_name += ",@ReferCode=" + _dao.FilterString(common.ReferCode);
            //sp_name += ",@ActionUser=" + _dao.FilterString(common.ActionUser);
            //sp_name += ",@ActionPlatform=" + _dao.FilterString(common.ActionPlatform);
            //sp_name += ",@ActionIP=" + _dao.FilterString(common.ActionIP);
            //return _dao.ParseCommonDbResponse(sp_name);
        }
        #endregion
        #region Login
        public CommonDbResponse Login(LoginRequestCommon Request)
        {
            string SQL = "EXEC sproc_customer_login_management @flag='login'";
            SQL += ",@LoginId=" + _dao.FilterString(Request.LoginId);
            SQL += ",@Password=" + _dao.FilterString(Request.Password);
            SQL += ",@ActionIP=" + _dao.FilterString(Request.ActionIP);
            SQL += ",@ActionPlatform=" + _dao.FilterString(Request.ActionPlatform);
            var dbResponse = _dao.ExecuteDataRow(SQL);
            if (dbResponse != null)
            {
                string code = _dao.ParseColumnValue(dbResponse, "Code").ToString();
                string message = _dao.ParseColumnValue(dbResponse, "Message").ToString();
                if (string.IsNullOrEmpty(code) || code.Trim() != "0")
                {
                    return new CommonDbResponse()
                    {
                        Code = ResponseCode.Failed,
                        Message = string.IsNullOrEmpty(message) ? "Failed" : message
                    };
                }
                else
                {
                    var loginResponse = new LoginResponseCommon()
                    {
                        NickName = _dao.ParseColumnValue(dbResponse, "NickName").ToString(),
                        IsPasswordForceful = _dao.ParseColumnValue(dbResponse, "IsPasswordForceful").ToString(),
                        MobileNumber = _dao.ParseColumnValue(dbResponse, "MobileNumber").ToString(),
                        AgentId = _dao.ParseColumnValue(dbResponse, "AgentId").ToString(),
                        UserId = _dao.ParseColumnValue(dbResponse, "UserId").ToString(),
                        EmailAddress = _dao.ParseColumnValue(dbResponse, "EmailAddress").ToString(),
                        ProfileImage = _dao.ParseColumnValue(dbResponse, "ProfileImage").ToString(),
                        SessionId = _dao.ParseColumnValue(dbResponse, "SessionId").ToString(),
                        ActionDate = _dao.ParseColumnValue(dbResponse, "ActionDate").ToString(),
                        Amount =  Convert.ToInt32(_dao.ParseColumnValue(dbResponse, "Amount").ToString())
                    };

                    string SQL2 = "EXEC sproc_system_links_management";
                    var dbResponse2 = _dao.ExecuteDataTable(SQL2);
                    if (dbResponse2 != null && dbResponse2.Rows.Count > 0) loginResponse.SystemLink = _dao.DataTableToListObject<SystemLinkCommon>(dbResponse2).ToList();
                    return new CommonDbResponse()
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = loginResponse
                    };
                }
            }
            return new CommonDbResponse()
            {
                Code = ResponseCode.Failed,
                Message = "Login failed"
            };
        }
        #endregion

        #region Forgot Password
        public CommonDbResponse ForgotPassword(ForgotPasswordCommon common)
        {
            string sql = "sproc_customer_registration_management ";
            sql += "@flag='fp_otp'";
            sql += ",@MobileNumber=" + _dao.FilterString(common.MobileNo);
            //sql += ",@NickName=" + _dao.FilterString(common.Username);
            sql += ",@ActionPlatform=" + _dao.FilterString(common.CreatedPlatform);
            return _dao.ParseCommonDbResponse(sql);
        }

        public CommonDbResponse ForgotPasswordOTP(RegistrationCommon common)
        {
            string sql = "sproc_customer_registration_management ";
            sql += "@flag='vfp_otp'";
            sql += ",@VerificationCode=" + _dao.FilterString(common.VerificationCode);
            sql += ",@MobileNumber=" + _dao.FilterString(common.MobileNumber);
            return _dao.ParseCommonDbResponse(sql);
        }

        public CommonDbResponse SetNewPassword(SetRegistrationPasswordCommon common)
        {
            string SQL = "EXEC sproc_customer_registration_management @Flag='snp'";
            SQL += ",@UserId=" + _dao.FilterString(common.UserId);
            SQL += ",@AgentId=" + _dao.FilterString(common.AgentId);
            SQL += ",@Password=" + _dao.FilterString(common.Password);
            SQL += ",@ActionUser=" + _dao.FilterString(common.ActionUser);
            SQL += ",@ActionPlatform=" + _dao.FilterString(common.ActionPlatform);
            SQL += ",@ActionIP=" + _dao.FilterString(common.ActionIP);
            SQL += ",@IsPasswordForceful="+ _dao.FilterString(common.IsPasswordForceful); 
            return _dao.ParseCommonDbResponse(SQL);
        }

        public CommonDbResponse ResendForgotPasswordOTP(ResendRegistrationOTPCommon Request)
        {
            string SQL = "EXEC sproc_customer_registration_management @Flag='rfpotp'";
            SQL += ",@MobileNumber=" + _dao.FilterString(Request.MobileNumber);
            SQL += ",@AgentId=" + _dao.FilterString(Request.AgentId);
            SQL += ",@ActionUser=" + _dao.FilterString(Request.ActionUser);
            SQL += ",@ActionPlatform=" + _dao.FilterString(Request.ActionPlatform);
            SQL += ",@ActionIP=" + _dao.FilterString(Request.ActionIP);
            return _dao.ParseCommonDbResponse(SQL);
        }
        #endregion

        #region "Referral"
        public ReferralModelCommon ValidateReferralCode(ReferralModelCommon referCommon)
        {
            var responsInfo = new ReferralModelCommon();
            string sp_name = "EXEC sproc_customer_registration_management @Flag = 'vrc'";
            sp_name += ",@ReferCode=" + _dao.FilterString(referCommon.ReferCode);
            sp_name += ",@ActionIp=" + _dao.FilterString(referCommon.ActionIP);
            sp_name += ",@ActionPlatform=" + _dao.FilterString(referCommon.ActionPlatform);
            var dbResponseInfo = _dao.ExecuteDataRow(sp_name);
            if (dbResponseInfo != null)
            {
                responsInfo.Code = dbResponseInfo["Code"].ToString();
                responsInfo.Message = dbResponseInfo["Message"].ToString();
            }
            return responsInfo;
        }
        #endregion
    }
}
