using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Web;

namespace System.Web.Mvc
{
    public static class GlobalizationHelpers
    {
        /// <summary>
        /// Adapted from Scott Hanselman's blog post: http://www.hanselman.com/blog/GlobalizationInternationalizationAndLocalizationInASPNETMVC3JavaScriptAndJQueryPart1.aspx
        /// </summary>
        /// <typeparam name="t"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static IHtmlString MetaAcceptLanguage<t>(this HtmlHelper<t> htmlHelper)
        {
            var acceptLanguage = HttpUtility.HtmlAttributeEncode(CultureInfo.CurrentCulture.ToString()); //Using CurrentCulture rather than CurrentUICulture as I'm interested in Date / Number localisation rather than language in particular: http://stackoverflow.com/a/329041/761388
            return new HtmlString(string.Format("<meta name=\"accept-language\" content=\"{0}\" />", acceptLanguage));
        }

        /// <summary>
        /// Return the JavaScript bundle for this users culture
        /// </summary>
        /// <typeparam name="t"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <returns>a culture bundle that looks something like this: "~/js-culture.en-GB"</returns>
        public static string JsCultureBundle<t>(this HtmlHelper<t> htmlHelper)
        {
            return "~/js-culture." + CultureInfo.CurrentCulture.ToString();
        }
    }
}