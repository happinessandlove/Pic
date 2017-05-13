using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using System.Xml;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using Newtonsoft.Json;

namespace System
{
    public static class Ali
    {
        public static bool SendWorkerRegisterSms(string code, string mobile)
        {
            return SendSms( JsonConvert.SerializeObject(new { code = code }), "SMS_66655233", mobile);
        }
        public static bool SendCustomerRegisterSms(string code, string mobile)
        {
            return SendSms( JsonConvert.SerializeObject(new { code = code }), "SMS_66655233", mobile);
        }

        public static bool SendResetPasswordSms(string code, string mobile)
        {
            return SendSms(JsonConvert.SerializeObject(new { code = code }), "SMS_66655233", mobile);
        }

        public static bool SendChangePasswordSms(string code, string mobile)
        {
            return SendSms(JsonConvert.SerializeObject(new { code = code }), "SMS_66655233", mobile);
        }

        private static bool SendSms( string param, string template, string mobile)
        {
            try
            {
                ITopClient client = new DefaultTopClient("http://gw.api.taobao.com/router/rest", "23814669", "61461c5572f2ed1982cb2cc36c6d30d1");
                AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
                //req.Extend = "123456";
                req.SmsType = "normal";
                req.SmsFreeSignName = "刘明";
                req.SmsParam = param;
                req.RecNum = mobile;
                req.SmsTemplateCode = template;
                AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
                return rsp.Result.Success;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
