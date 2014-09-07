using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.Documents
{
    public static class MWord
    {
                public static string Header(string title)
        {
            StringBuilder strBody = new StringBuilder();
            strBody.Append("<html " + 
                "xmlns:o='urn:schemas-microsoft-com:office:office' " +
                "xmlns:w='urn:schemas-microsoft-com:office:word'" +
                "xmlns='http://www.w3.org/TR/REC-html40'>" +
                "<head><title>"+ title +"</title>");
            //'The setting specifies document's view after it is downloaded as Print Layout
            //'instead of the default Web Layout. For the Header & Footer to be visible,
            //'this mode is required.
            strBody.Append("<!--[if gte mso 9]>" +
                            "<xml>" +
                            "<w:WordDocument>" +
                            "<w:View>Print</w:View>" +
                            "<w:Zoom>100</w:Zoom>" +
                            "<w:DoNotOptimizeForBrowser/>" +
                            "</w:WordDocument>" +
                            "</xml>" +
                            "<![endif]-->");

            //'we can tweak the MsoFooter class that is referenced by the footer, as required
            strBody.Append("<style>" +
                            "<!-- /* Style Definitions */" +
                            "p.MsoFooter, li.MsoFooter, div.MsoFooter" +
                            "{margin:0in;" +
                            "margin-bottom:.0001pt;" +
                            "mso-pagination:widow-orphan;" +
                            "tab-stops:center 3.0in right 6.0in;" +
                            "font-size:12.0pt;}");

            //'Word uses the @page definition to store document layout settings for the entire document.
            //'Using @page SectionN, Word applies page formatting to individual HTML elements referenced
            //'through the class attribute.
            //mso-footer is the style attribute related to the footer 
            //Refer to the topic "Page Layout and Section Breaks" & "Headers and Footers" in the 
            //Office XML & HTML Reference for detailed info.
            strBody.Append("@page Section1 " +
                            "{size:8.5in 11.0in; " +
                            "margin:1.0in 1.25in 1.0in 1.25in ; " +
                            "mso-header-margin:.5in; " +
                            "mso-footer: f1;" +
                            "mso-footer-margin:.5in; mso-paper-source:0;} " +
                            "div.Section1 " +
                            "{page:Section1;}" +
                            "-->" +
                            "</style></head>");
            return strBody.ToString();
        }
        public static string Footer()
        {
            StringBuilder strBody = new StringBuilder();
            //Word marks and stores information for simple fields by means of the Span element with the 
            //mso-field-code style. Refer to the topic "Fields" in the Office XML & HTML Reference
            strBody.Append("<div style='mso-element:footer' id=f1>" +
                                    " <p class=MsoFooter>" +
                                    " <span style='mso-tab-count:2'></span><span style='mso-field-code:" + " PAGE " + "'></span>" +
                                    " </p></div>" +
                                    "</body></html>");
            return strBody.ToString();
        }
        public static string BodyStart()
        {
            StringBuilder strBody = new StringBuilder();
            strBody.Append("<body lang=EN-US style='tab-interval:.5in'>");
            return strBody.ToString();
        }
    }
}
