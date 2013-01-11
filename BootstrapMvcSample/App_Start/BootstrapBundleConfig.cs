using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BootstrapSupport
{
    public class BootstrapBundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js").Include(
                "~/Scripts/jquery-*",
                "~/Scripts/globalize.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/bootstrap-datepicker.js",
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/jquery.validate.unobtrusive-custom-for-bootstrap.js",
                "~/Scripts/jquery.validate.globalize.js"
                ));

            //Create culture specific bundles
            foreach (var culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                bundles.Add(new ScriptBundle("~/js-culture." + culture.Name).Include(
                    DetermineCultureFile(culture, "~/Scripts/globalize-cultures/globalize.culture.{0}.js"),
                    DetermineCultureFile(culture, "~/Scripts/bootstrap-datepicker-locales/bootstrap-datepicker.{0}.js")
                ));
            }

            bundles.Add(new StyleBundle("~/content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-datepicker.css"
                ));

            bundles.Add(new StyleBundle("~/content/css-responsive").Include(
                "~/Content/bootstrap-responsive.css"
                ));
        }

        /// <summary>
        /// Given the supplied culture, determine the most appropriate Globalize culture script file that should be served up
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="filePattern"></param>
        /// <param name="defaultCulture">Default localisation to use</param>
        /// <returns></returns>
        private static string DetermineCultureFile(CultureInfo culture, 
            string filePattern,
            string defaultCulture = "en-GB" // I'm a Brit and this is mine
            )
        {
            //Determine culture - GUI culture for preference, user selected culture as fallback
            var regionalisedFileToUse = string.Format(filePattern, defaultCulture);

            //Try to pick a more appropriate regionalisation if there is one
            if (File.Exists(HttpContext.Current.Server.MapPath(string.Format(filePattern, culture.Name)))) //First try for a globalize.culture.en-GB.js style file
                regionalisedFileToUse = string.Format(filePattern, culture.Name);
            else if (File.Exists(HttpContext.Current.Server.MapPath(string.Format(filePattern, culture.TwoLetterISOLanguageName)))) //That failed; now try for a globalize.culture.en.js style file
                regionalisedFileToUse = string.Format(filePattern, culture.TwoLetterISOLanguageName);

            return regionalisedFileToUse;
        }

    }
}