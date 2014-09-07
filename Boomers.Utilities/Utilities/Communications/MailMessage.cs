using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace Boomers.Utilities.Communications
{
    public class MailMessage
    {
        public string ToName { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string CompanyName { get; set; }

        //public List<string> toList { get; set; }
        //public List<string> ccList { get; set; }
        //public List<string> bccList { get; set; }
        //public string subject { get; set; }
        //public string body { get; set; }
        //public string from { get; set; }
        //public bool isHTML { get; set; }

        //public MailMessage()
        //{
        //}
        //public MailMessage(List<string> _toList,List<string> _ccList, List<string> _bccList,string _subject,string _body,string _from,bool _isHTML)
        //{
        //    this.toList = _toList;
        //    this.ccList = _ccList;
        //    this.bccList = _bccList;
        //    this.subject = _subject;
        //    this.body = _body;
        //    this.from = _from;
        //    this.isHTML = _isHTML;
        //}
        //public MailMessage(List<string> _toList, string _subject, string _body, string _from, bool _isHTML)
        //{
        //    this.toList = _toList;
        //    this.subject = _subject;
        //    this.body = _body;
        //    this.from = _from;
        //    this.isHTML = _isHTML;
        //}

        //public bool SendMessage()
        //{
        //    try
        //    {
        //        return Email.SendEmail(this);
        //    }
        //    catch (Exception)
        //    {  
        //        throw;
        //    }
        //}
    }
}
