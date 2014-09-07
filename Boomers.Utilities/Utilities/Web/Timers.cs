using System;
using System.Timers;

namespace Boomers.Utilities.Web
{
   public  class Timers
    {
       private static System.Timers.Timer aTimer;
       public static void RestartApplication(int timeInMilliseconds)
       {
           aTimer = new System.Timers.Timer(timeInMilliseconds);
                   aTimer.Elapsed += new ElapsedEventHandler(RestartApplication);
           aTimer.Enabled = true;
       }
       private static void RestartApplication(object sender, ElapsedEventArgs e)
       {
           System.Web.HttpRuntime.UnloadAppDomain();
       }

    }
}
