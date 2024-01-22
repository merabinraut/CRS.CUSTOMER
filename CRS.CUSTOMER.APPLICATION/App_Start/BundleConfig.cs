using System.Web.Optimization;

namespace CRS.CUSTOMER.APPLICATION
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/UI/DesignCss")
                .Include("~/Content/assets/css/icons/icomoon/styles.min.css", new CssRewriteUrlTransform())
                .Include(
                "~/Content/assets/css/bootstrap.min.css",
                "~/Content/assets/css/bootstrap_limitless.min.css",
                "~/Content/assets/css/layout.min.css",
                "~/Content/assets/css/components.min.css"
                ).Include("~/Content/assets/css/colors.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/nepali.datepicker.v3/css/nepali.datepicker.v3.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/Site.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/UI/Style")
                .Include("~/Content/font/all.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/css/tempusdominus-bootstrap-4.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/css/daterangepicker.css", new CssRewriteUrlTransform())
                .Include("~/Content/css/jquery-ui.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/css/select2.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/css/jqvmap.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/css/adminlte.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/css/icheck-bootstrap.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/css/OverlayScrollbars.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/nepali.datepicker.v3/css/nepali.datepicker.v3.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/css/summernote-bs4.min.css", new CssRewriteUrlTransform())

                .Include("~/Content/css/dataTables.bootstrap4.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/css/responsive.bootstrap4.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/css/buttons.bootstrap4.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/css/style.css", new CssRewriteUrlTransform())
                );
            bundles.Add(new StyleBundle("~/UI/LoginStyle")
              .Include("~/Content/font/all.min.css", new CssRewriteUrlTransform())
              .Include("~/Content/css/icheck-bootstrap.min.css", new CssRewriteUrlTransform())
              .Include("~/Content/css/adminlte.min.css", new CssRewriteUrlTransform())
              );

            bundles.Add(new ScriptBundle("~/UI/JS").Include(
                "~/Content/js/jquery.min.js",

                "~/Content/js/jquery-ui.min.js"
                ));

            bundles.Add(new ScriptBundle("~/UI/JSplugin").Include(
                "~/Content/js/bootstrap.bundle.min.js",
                "~/Content/js/select2.full.min.js",
                "~/Content/js/Chart.min.js",
                "~/Content/js/sparkline.js",
                "~/Content/js/jquery.vmap.min.js",
                "~/Content/js/jquery.knob.min.js",
                "~/Content/js/moment.min.js",
                "~/Content/js/daterangepicker.js",
                "~/Content/js/tempusdominus-bootstrap-4.min.js",
                "~/Content/js/summernote-bs4.min.js",
                "~/Content/js/jquery.overlayScrollbars.min.js",
                "~/Content/nepali.datepicker.v3/js/nepali.datepicker.v3.min.js",
                "~/Content/js/bootbox.min.js",
                "~/Content/js/jquery.validate.min.js",
                "~/Content/js/jquery.validate.unobtrusive.min.js",
                "~/Content/js/uniform.min.js",



                "~/Content/js/jquery.dataTables.min.js",
                "~/Content/js/dataTables.bootstrap4.min.js",
                "~/Content/js/dataTables.responsive.min.js",
                "~/Content/js/responsive.bootstrap4.min.js",
                "~/Content/js/dataTables.buttons.min.js",
                "~/Content/js/buttons.bootstrap4.min.js",
                "~/Content/js/jszip.min.js",
                "~/Content/js/pdfmake.min.js",
                "~/Content/js/vfs_fonts.js",
                "~/Content/js/buttons.html5.min.js",
                "~/Content/js/buttons.print.min.js",
                "~/Content/js/buttons.colVis.min.js",
                "~/Content/js/adminlte.js",
                "~/Content/js/demo.js",
                 "~/Content/assets/js/xlsx.full.min.js"

                ));


            bundles.Add(new ScriptBundle("~/UI/CoreJs").Include(
                        "~/Content/assets/js/main/jquery.min.js",
                        "~/Content/assets/js/main/bootstrap.bundle.min.js",
                        "~/Content/assets/js/plugins/loaders/blockui.min.js"));

            bundles.Add(new ScriptBundle("~/UI/ThemeJs").Include(
                "~/Content/assets/js/main/jquery.validate.min.js",
                "~/Content/assets/js/main/jquery.validate.unobtrusive.min.js",
                "~/Content/assets/js/plugins/notifications/bootbox.min.js",
                "~/Content/assets/js/plugins/visualization/d3/d3.min.js",
                "~/Content/assets/js/plugins/visualization/d3/d3_tooltip.js",
                "~/Content/assets/js/plugins/forms/styling/uniform.min.js",
                "~/Content/assets/js/plugins/forms/styling/switchery.min.js",
                "~/Content/assets/js/plugins/forms/styling/switch.min.js",
                "~/Content/assets/js/plugins/forms/selects/bootstrap_multiselect.js",
                "~/Content/assets/js/plugins/ui/moment/moment.min.js",
                "~/Content/assets/js/plugins/pickers/daterangepicker.js",
                "~/Content/assets/js/plugins/pickers/pickadate/picker.js",
                "~/Content/assets/js/plugins/pickers/pickadate/picker.date.js",
                "~/Content/nepali.datepicker.v3/jquery-ui.min.js",
                "~/Content/nepali.datepicker.v3/js/nepali.datepicker.v3.min.js",
                "~/Content/assets/js/plugins/pickers/pickadate/picker.time.js",
                "~/Content/assets/js/plugins/ui/perfect_scrollbar.min.js",
                //"~/Content/assets/js/plugins/buttons/hover_dropdown.min.js",
                //"~/Content/assets/js/plugins/ui/prism.min.js",
                "~/Content/assets/js/plugins/tables/datatables/datatables.min.js",
                "~/Content/assets/js/plugins/tables/datatables/extensions/scroller.min.js",
                "~/Content/assets/js/plugins/tables/datatables/extensions/buttons.min.js",
                "~/Content/assets/js/plugins/forms/selects/select2.min.js",
                "~/Content/assets/js/plugins/notifications/jgrowl.min.js",
                "~/Content/assets/js/plugins/notifications/noty.min.js",
                // "~/Content/assets/js/demo_pages/datatables_extension_scroller.js",
                "~/Content/assets/js/app.js",
                "~/Content/assets/js/demo_pages/dashboard.js",
                "~/Content/assets/js/currencyFormatter.min.js",
                "~/Content/assets/js/chart.min.js",
                "~/Content/assets/js/xlsx.full.min.js"

                ));
            bundles.Add(new ScriptBundle("~/LoginJS").Include(
               "~/Content/js/jquery.min.js",
               "~/Content/js/bootstrap.bundle.min.js",
               "~/Content/js/adminlte.js"

               ));

            bundles.Add(new ScriptBundle("~/UI/AppJs").Include("~/Content/assets/js/app.js"));

            bundles.Add(new ScriptBundle("~/UI/Custom").Include(
                "~/Content/Custom-Scripts.js"));

            bundles.Add(new ScriptBundle("~/UI/FixedSidebar").Include(
                "~/Content/assets/js/demo_pages/layout_fixed_sidebar_custom.js"
                ));

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

        }
    }
}
