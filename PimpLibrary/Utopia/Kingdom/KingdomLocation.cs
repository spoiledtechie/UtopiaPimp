using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PimpLibrary.Utopia.Kingdom
{
  public   class KingdomLocation
    {
          public KingdomLocation() { }
          public KingdomLocation(int island, int kingdom)
        {
            Island = island;
            Kingdom = kingdom;
        }
        public int Island { get; set; }
        public int Kingdom { get; set; }
    }
}
