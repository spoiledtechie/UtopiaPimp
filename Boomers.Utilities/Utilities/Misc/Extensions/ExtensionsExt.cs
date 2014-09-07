using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.Misc.Extensions
{
     public static class ExtensionsExt
    {
        /// <summary>
        /// Mimics a coin toss.
        /// </summary>
        /// <param name="rng"></param>
        /// <returns></returns>
        public static bool CoinToss(this Random rng)
        {
            //Random rand;
           // bool luckyDay = rand.CoinToss();
            return rng.Next(2) == 0;
        }
        /// <summary>
        /// Gets a random of any item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rng"></param>
        /// <param name="things"></param>
        /// <returns></returns>
        public static T OneOf<T>(this Random rng, params T[] things)
        {
            //Random rand;
            //bool luckyDay = rand.CoinToss();
            //string babyName = rand.OneOf("John", "George", "Radio XBR74 ROCKS!");
            return things[rng.Next(things.Length)];
        }
    }
}
