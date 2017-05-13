using System.Web;
using System.Web.Optimization;

namespace MvcHelper.Management
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region base
            bundles.Add(new ScriptBundle("~/scripts/site").Include(
                         "~/Scripts/jquery-{version}.js"
                        , "~/Scripts/d/d.js"
                        , "~/Scripts/d/d.masker.js"
                        , "~/Scripts/d/d.float.js"
                        , "~/Scripts/d/d.dialog.js"
                        , "~/Scripts/d/d.dropdown.js"
                        , "~/Scripts/d/d.spinner.js"
                        , "~/Scripts/d/d.tab.js"
            ));

            bundles.Add(new StyleBundle("~/styles/site")
                .Include("~/Content/d/d.css", new CssRewriteUrlTransform())
                .Include("~/Content/d/d.dialog.css", new CssRewriteUrlTransform())
                .Include("~/Content/d/d.dropdown.css", new CssRewriteUrlTransform())
                .Include("~/Content/d/d.spinner.css", new CssRewriteUrlTransform())
                .Include("~/Content/d/d.tab.css", new CssRewriteUrlTransform())
            );
            #endregion

            #region framework
            bundles.Add(new ScriptBundle("~/scripts/framework").Include("~/Scripts/d/d.framework-{version}.js"));
            bundles.Add(new StyleBundle("~/styles/framework").Include("~/Content/d/d.framework-{version}.css", new CssRewriteUrlTransform()));

            bundles.Add(new ScriptBundle("~/scripts/subpage").Include("~/Scripts/d/d.subpage-{version}.js"));
            bundles.Add(new StyleBundle("~/styles/subpage").Include("~/Content/d/d.subpage-{version}.css", new CssRewriteUrlTransform()));
            #endregion
                        
            #region 验证
            //脚本
            bundles.Add(new ScriptBundle("~/scripts/jquery-validate").Include(
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive*",
                        "~/Scripts/jquery.validate.methods.custom-{version}.js"));
            #endregion

            #region login
            bundles.Add(new StyleBundle("~/styles/login").Include("~/Content/login.css"));
            #endregion

        }
    }
}
