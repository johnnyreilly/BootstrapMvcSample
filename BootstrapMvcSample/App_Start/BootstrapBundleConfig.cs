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
                "~/Scripts/globalize.js", //The Globalize library
                "~/Scripts/bootstrap.js",
                "~/Scripts/bootstrap-datepicker.js", //This is the brand new internationalised Bootstrap Datepicker 
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/jquery.validate.unobtrusive-custom-for-bootstrap.js",
                "~/Scripts/jquery.validate.globalize.js" //My jQuery Validate extension which depends on Globalize
                ));

            //Create culture specific bundles which contain the JavaScript files that should be served for each culture
            foreach (var culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                bundles.Add(new ScriptBundle("~/js-culture." + culture.Name).Include( //example bundle name would be "~/js-culture.en-GB"
                    DetermineCultureFile(culture, "~/Scripts/globalize-cultures/globalize.culture.{0}.js"),             //The Globalize locale-specific JavaScript file
                    DetermineCultureFile(culture, "~/Scripts/bootstrap-datepicker-locales/bootstrap-datepicker.{0}.js") //The Bootstrap Datepicker locale-specific JavaScript file
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
        /// <param name="filePattern">a file pattern, eg "~/Scripts/globalize-cultures/globalize.culture.{0}.js"</param>
        /// <param name="defaultCulture">Default culture string to use (eg "en-GB") if one cannot be found for the supplied culture</param>
        /// <returns></returns>
        private static string DetermineCultureFile(CultureInfo culture,
            string filePattern,
            string defaultCulture = "en-GB" // I'm a Brit and this is my default
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