using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Linq;
using System.IO;
using Boomers.Political.SunlightFoundation.ViewModels;

namespace Boomers.Political.SunlightFoundation.Congress
{
public class Committees
    {
        private string _url = "http://services.sunlightlabs.com/api/";
        public string ApiKey { get; set; }
        public string Url { get { return _url; } }
        public List<CommitteeViewModel> GetCommitteesByChamber(Chamber chamber)
        {
            if (ApiKey == null)
                throw new NullReferenceException("No API Key Found");

            string url = Url + "committees.getList.xml?apikey=" + ApiKey;

            url += "&chamber=" + chamber.ToString();

            return GetComittees(url);
        }
        public List<CommitteeViewModel> GetComittees(string url)
        {
            WebClient wc = new WebClient();
            string legs = wc.DownloadString(url);
            byte[] byteArray = Encoding.ASCII.GetBytes(legs);
            MemoryStream stream = new MemoryStream(byteArray);
            XElement doc = XElement.Load(stream);

            return (from l in doc.Elements("committees").Elements("committee")
                    select new CommitteeViewModel
                    {
                        Chamber = l.Element("chamber").Value,
                        Id = l.Element("id").Value,
                        CommitteeName = l.Element("name").Value,
                    }).ToList();
        }
        public enum Chamber
        {
            Joint,
            Senate,
            House
        }
    }
}
