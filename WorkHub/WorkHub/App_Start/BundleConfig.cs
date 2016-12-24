﻿using System.Web;
using System.Web.Optimization;

namespace WorkHub
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            /* Materialize Framework JS */
            bundles.Add(new ScriptBundle("~/bundles/materialize").Include(
                     "~/Scripts/Materialize/materialize.js",
                     "~/Scripts/Materialize/materialize.min.js",
                     "~/Scripts/jquery-{version}.js",
                      "~/Scripts/jquery.validate*"
                     ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/Index/css").Include(
                "~/Content/Index/index-style.css"));

            bundles.Add(new StyleBundle("~/Content/Register/css").Include(
              "~/Content/Register/register-style.css"));

            /* Materialize Framework Stylesheets */
            bundles.Add(new StyleBundle("~/Content/Materialize/css").Include(
             "~/Content/Materialize/materialize.css",
             "~/Content/Materialize/materialize.min.css"));
        }
    }
}
