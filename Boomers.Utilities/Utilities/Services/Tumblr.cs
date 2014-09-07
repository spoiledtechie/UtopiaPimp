using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Boomers.Utilities.Services
{
    public class Tumblr
    {
        public TumblrRssFeed Feed = new TumblrRssFeed();
        public TumblrPosts Posts = new TumblrPosts();
        public static Tumblr GetTumblr(string xmlDoc)
        {
            XDocument xmlDocs = XDocument.Load(xmlDoc);
            
            Tumblr tm = new Tumblr();
            tm.Feed = (from t in xmlDocs.Root.Elements("tumblelog")
                       select new TumblrRssFeed()
                       {
                           Name = (string)t.Attribute("name"),
                           CName = (string)t.Attribute("cname"),
                           TimeZone = (string)t.Attribute("timezone"),
                           Title = (string)t.Attribute("title"),
                           Description = (string)t.Value
                       }).FirstOrDefault();

            tm.Posts = (from ps in xmlDocs.Root.Elements("posts")
                        select new TumblrPosts()
                        {
                            Count = (string)ps.Attribute("total"),
                            Start = (string)ps.Attribute("start"),
                            Post = (from pst in ps.Elements("post")
                                    select new TumblrPost()
                                    {
                                        Id = (string)pst.Attribute("id"),
                                        Url = (string)pst.Attribute("url"),
                                        UrlSlug = (string)pst.Attribute("url-with-slug"),
                                        Type = (string)pst.Attribute("type"),
                                        Date = (string)pst.Attribute("date"),
                                        DateGMT = (string)pst.Attribute("date-gmt"),
                                        UnixTime = (string)pst.Attribute("unix-timestamp"),
                                        Format = (string)pst.Attribute("format"),
                                        Title = (string)pst.Element("regular-title").Value,
                                        Body = (string)pst.Element("regular-body").Value
                                    }).ToList()
                        }).FirstOrDefault();
            return tm;
        }

    }
    public class TumblrRssFeed
    {
        public string Name;
        public string TimeZone;
        public string CName;
        public string Title;
        public string Description;
    }
    public class TumblrPosts
    {
        public string Start;
        public string Count;
        public List<TumblrPost> Post;
    }
    public class TumblrPost
    {
        public string Id;
        public string Url;
        public string UrlSlug;
        public string Title;
        public string Body;
        public string Type;
        public string DateGMT;
        public string Date;
        public string UnixTime;
        public string Format;

    }

}
