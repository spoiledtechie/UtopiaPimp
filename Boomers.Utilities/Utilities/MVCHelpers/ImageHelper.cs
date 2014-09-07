using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace Boomers.Utilities.MVCHelpers
{
    public static class ImageHtmlHelper
    {

        //       <%= Html.Image( Url.Content( "~/Content/images/img.png" ),
        //              "alt text",
        //              new { id = "myImage", border = "0" } )
        //%>

        public static string Image(this HtmlHelper helper,
                                    string url,
                                    string altText,
                                    object htmlAttributes)
        {
            //TagBuilder builder = new TagBuilder("img");
            //builder.Attributes.Add("src", url);
            //builder.Attributes.Add("alt", altText);
            //builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            //return builder.ToString(TagRenderMode.SelfClosing);
            return string.Empty;
        }
    }
}
