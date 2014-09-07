using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using Boomers.Utilities.DatesTimes;
using System.IO;
using Boomers.Political.SunlightFoundation.ViewModels;

namespace Boomers.Political.SunlightFoundation.Congress
{
    public class StaffDirectory
    {
        private string _url = "http://staffers.sunlightfoundation.com";
        public string Url { get { return _url; } }
        Regex rgxOfficeUrl = new Regex(@"/office/[a-z-0-9]*", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        //Regex rgxTest = new Regex(@"<tr[a-z\s=\">]+<td>[\s<a-z=\"/\->A-Z&\.(),]+</td>\s+<td>[\s()\d\-]+</td>\s+<td>[\sA-Za-z0-9\-]+</td>\s+</tr>");
        Regex rgxGetCSV = new Regex("href=\"[/a-zA-Z0-9?=]+csv", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        string docLocation = @"C:\HostingSpaces\pio.scott@gmail.com\docs.codingforcharity.org\wwwroot\bt\" + DateTime.UtcNow.ToyyyyMMdd() + @"\";
        public List<OfficeViewModel> PullStaffOffices()
        {
            WebClient wc = new WebClient();
            string data = wc.DownloadString(Url + "/offices");

            if (!Directory.Exists(docLocation))
                Directory.CreateDirectory(docLocation);

            string findCsv = rgxGetCSV.Match(data).Value.Replace("href=\"", "").Replace("\"", "");
            wc.DownloadFile(Url + findCsv, docLocation + findCsv.Replace("?format=csv", "").Replace("/", "") + ".csv");

            string doc = docLocation + findCsv.Replace("?format=csv", "").Replace("/", "") + ".csv";

            List<OfficeViewModel> offices = new List<OfficeViewModel>();

            StreamReader sr = new StreamReader(doc);
            string line = sr.ReadLine();

            int i = 0;
            while (sr.EndOfStream == false)
            {
                i += 1;
                string line1 = sr.ReadLine();
                OfficeViewModel office = new OfficeViewModel();
                office.Name = line1.Split(',')[0];
                office.PhoneNumber = line1.Split(',')[1];
                office.BuildingName = line1.Split(',')[2];
                office.RoomNumber = line1.Split(',')[3];
                offices.Add(office);
            }

            return offices;
        }
    }
}
