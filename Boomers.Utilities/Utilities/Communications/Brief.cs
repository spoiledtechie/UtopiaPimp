using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.Communications
{
    public static class Brief
    {
        /// <summary>
        /// Creates an email body to be sent to a user.
        /// </summary>
        /// <param name="bodyText">The main content of the email</param>        
        /// <returns>The email body</returns>
        public static string CreateBriefBodyWithAdvertisement(string bodyText)
        {
            return CreateBriefBodyWithAdvertisement(bodyText, string.Empty, string.Empty);
        }

        /// <summary>
        /// Creates an email body to be sent to a user.
        /// </summary>
        /// <param name="bodyText">The main content of the email</param>
        /// <param name="urlToChangeEmail">Url to a site were the users can change their mail settings or turn of the mails.</param>
        /// <param name="textToChangeEmail">Text to be displayed in conjunction with urlToChangeEmail. The text is placed first then a click here link.</param>
        /// <returns>The email body</returns>
        public static string CreateBriefBodyWithAdvertisement(string bodyText, string urlToChangeEmail, string textToChangeEmail)
        {
            StringBuilder output = new StringBuilder();
         
            output.AppendLine(bodyText);
            output.AppendLine("<br /><br /><br />");
            output.Append("This email was sponsered by:<br/>");
            output.Append("<b>");
            switch (new Random().Next(3))
            {
                case 0:
                    output.Append("<a href=\"http://www.DrinkingFor.com\">DrinkingFor.com</a> - Drinking games for any type of event.");
                    break;
                case 1:
                    output.Append("<a href=\"http://www.ItFeelsLike.com\">ItFeelsLike.com</a> - Stories of People feeling Pain, Extacy, Happiness and Sadnness.");
                    break;
                case 2:
                    output.Append("<a href=\"http://www.DeMotivatedPosters.com\">DeMotivatedPosters.com</a> - DeMotivational Posters of crazy things happening.");
                    break;
                case 3:
                    output.Append("<a href=\"http://www.PostSecretCollection.com\">PostSecretCollection.com</a> - PostSecret's Biggest and Fullest Postcard Collection. It containts people's deepest and most hidden secrets for all to see.");
                    break;
            }
            output.Append("</b>");
                       
            return output.ToString();
        }
    }
}
