using System.Configuration;

namespace System
{
    /// <summary>
    /// 自定义，相关配置信息
    /// </summary>
    public static partial class ConfigHelper
    {
#if DEBUG
        public static bool Debug = true;
#endif
#if !DEBUG
        public static bool Debug = false;
#endif
        public static string SiteName = "巧匠帮";
#if DEBUG
        public static string ApiDomainName = "http://localhost:64579";
       // public static string PortalsDomainName = "http://210.29.65.96:5054";
       // public static string WapDomainName = "http://210.29.65.96:219";
        public static string FileServerDomainName = "http://localhost:52639";
        public static string Host = "localhost";
#endif
#if !DEBUG
        public static string WorkerApiDomainName = "http://210.29.65.96:1031";
        public static string PortalsDomainName = "http://210.29.65.96:5054";
        public static string WapDomainName = "http://210.29.65.96:219";
        public static string FileServerDomainName = "http://210.29.65.96:1030";
        public static string Host = "210.29.65.96";
#endif
        public static int SocketPort = 6000;
        public static string BaiduPushApiKey = "GIwgK30FoBARL9wq1MAmjxh6";
        public static string BaiduPushSecretKey = "DURjRpfFAhKtvOCOoCGwjl1OGbNYZEfT";

        public static string WxAppId = "";
        public static string WxAppSecret = "";
        public static string WxPartner = "";
        public static string WxPartnerKey = "";
        public static string WxSslCertPath = "";
        public static string WxSslCertPassword = "";

        public static string WxOpenAppId = "";
        public static string WxOpenAppSecret = "";
        public static string WxOpenPartner = "";
        public static string WxOpenPartnerKey = "";
        public static string WxOpenSslCertPath = "";
        public static string WxOpenSslCertPassword = "";
    }
}
