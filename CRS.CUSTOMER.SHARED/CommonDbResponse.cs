using System;
using System.Data;

namespace CRS.CUSTOMER.SHARED
{
    public enum ResponseCode
    {
        Exception = 9,
        Success = 0,
        Failed = 1,
        Warning = 2
    }
    public class CommonDbResponse
    {
        public ResponseCode Code { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public string Id { get; set; }
        public string Extra1 { get; set; }
        public string Extra2 { get; set; }
        public string Extra3 { get; set; }
        public string Extra4 { get; set; }
        public string Extra5 { get; set; }
        public object Data { get; set; }
        public void SetMessage(int Code, string Message, string Extra1 = "", String Extra2 = "", string Extra3 = "", string Extra4 = "", string Extra5 = "", DataTable dt = null, DataRow row = null)
        {
            switch (Code)
            {
                case 1:
                    this.Code = ResponseCode.Failed;
                    break;
                case 2:
                    this.Code = ResponseCode.Warning;
                    break;
                case 0:
                    this.Code = ResponseCode.Success;
                    break;
                default:
                    this.Code = ResponseCode.Exception;
                    break;
            }

            this.ErrorCode = Code;
            this.Message = Message;
            this.Extra1 = Extra1 ?? "";
            this.Extra2 = Extra2 ?? "";
            this.Extra3 = Extra3 ?? "";
            this.Extra4 = Extra4 ?? "";
            this.Extra5 = Extra5 ?? "";
            if (dt != null)
                this.Data = dt;
            else
                this.Data = row;
        }
        public void SetMessage(string Code, string Message, string Extra1 = "", String Extra2 = "", string Extra3 = "", string Extra4 = "", string Extra5 = "", DataTable dt = null, DataRow row = null)
        {
            int _code = 0;
            if (int.TryParse(Code.Trim(), out _code))
            {
                SetMessage(_code, Message, Extra1, Extra2, Extra3, Extra4, Extra5);
            }
            else
                SetMessage(9, "Invalid Response Code", Extra1, Extra2, Extra3, Extra4, Extra5);
            if (dt != null)
                this.Data = dt;
            else
                this.Data = row;
        }
    }
}
