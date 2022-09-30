using System.Web;
using System.Web.Optimization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuanLyGiangDay
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/vendor/jquery/jquery.min.js"));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/theme/css").Include(
                      "~/Content/css/sb-admin-2.min.css",
                      "~/Content/vendor/datatables/dataTables.bootstrap4.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/theme/js").Include(
                      "~/Content/vendor/bootstrap/js/bootstrap.bundle.min.js",
                      "~/Content/vendor/vendor/jquery-easing/jquery.easing.min.js",
                      "~/Content/vendor/js/sb-admin-2.min.js",
                      "~/Content/vendor/datatables/jquery.dataTables.min.js",
                      "~/Content/vendor/datatables/dataTables.bootstrap4.js"));

        }
    }
}
