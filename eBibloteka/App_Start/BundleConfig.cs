using System.Web;
using System.Web.Optimization;

namespace eBibloteka
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/select2/js").Include(
                "~/adminty/files/bower_components/select2/js/select2.full.min.js"));

            bundles.Add(new ScriptBundle("~/select2/js").Include(
                    "~/Scripts/select2.min.js"));

            bundles.Add(new StyleBundle("~/select2/css").Include(
                    "~/Content/Select2css/select2.min.css",
                    "~/Content/Select2css/select2-bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/bundles/select2/css").Include(
                   "~/adminty/files/bower_components/select2/css/select2.min.css"));
        }
    }
}
