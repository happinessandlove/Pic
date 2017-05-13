using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using Models;

namespace MvcHelper.Management
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            UnityConfig.Container.RegisterType<IDbContext, DbEntity>(new PerRequestLifetimeManager());
            
            HttpContext.Current.Application["SiteDirectories"] = SiteDirectory.Load();

            ClientDataTypeModelValidatorProvider.ResourceClassKey = "ErrorMessage_zh_CN";
            DefaultModelBinder.ResourceClassKey = "ErrorMessage_zh_CN";
        }
    }
}
