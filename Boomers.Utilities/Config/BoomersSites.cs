using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;

namespace Boomers.Config
{
    public class BoomersSites
    {
        public List<Site> Sites;

        public BoomersSites()
        {
            Sites = new List<Site>();
            Sites.Add(new Site("Roller Derby Penalty Timer", "https://market.android.com/details?id=com.rdnation.pbtimerpaid", "An Android Application for the Famous Sport of Roller Derby"));
            Sites.Add(new Site("DrinkingFor", "http://www.DrinkingFor.com", "Games created so that you may drink to them!"));
            Sites.Add(new Site("DeMotivatedPosters", "http://www.DeMotivatedPosters.com", "Motivational Posters Turned Bad. Sexy, Hott, Funny, Crazy, Different."));
            Sites.Add(new Site("PostSecretCollection", "http://www.PostSecretCollection.com", "The Secrets of the Entire World Written on Postcards. From PostSecret.com"));
            Sites.Add(new Site("DolphinWords", "http://www.DolphinWords.com", "Word Dictionary for Scrabble."));
            Sites.Add(new Site("ItFeelsLike", "http://www.ItFeelsLike.com", "Ever Wonder what something Feels Like in this World, but too Scared to try?  Drugs, Shooting, Pregnancy."));
            Sites.Add(new Site("UtopiaPimp", "http://www.UtopiaPimp.com", "An Engine to Help support one of the Internets Oldest and Most Popular online Role Playing Games."));
            Sites.Add(new Site("SpoiledTechie", "http://www.SpoiledTechie.com", "The Blog of a Internet Giant."));

        }
        /// <summary>
        /// gets a random site
        /// </summary>
        /// <returns></returns>
        public Site getRandomSite()
        {
            Random r = new Random();
            int rand=r.Next(0, Sites.Count);
         return   Sites[rand];
        }


        public static readonly string Name;
        public string Url;
        public string Description;
    }

    public class Site
    {
        public string Name;
        public string Url;
        public string Description;

        public Site(string name, string url, string description)
        {
            this.Name = name;
            this.Url = url;
            this.Description = description;
        }
    }
}
