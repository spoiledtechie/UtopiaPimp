using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Boomers.Utilities.Web
{
  public  static  class WebExt
    {
      public static string IPAddress(this System.Web.HttpRequestBase request)
      {
          return request.ServerVariables["REMOTE_ADDR"].ToString();
      }
      public static string IPAddress(this System.Web.HttpRequest request)
      {
          return request.ServerVariables["REMOTE_ADDR"].ToString();
      }
      public static string BaseUrl(this System.Web.HttpRequest request)
      {
          return  request.Url.Scheme + "://" + request.Url.Authority + request.ApplicationPath.TrimEnd('/') + '/';
      }
    }
}
