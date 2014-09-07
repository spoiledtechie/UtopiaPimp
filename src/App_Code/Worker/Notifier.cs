using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

using PimpLibrary.Communications;

namespace App_Code.CS_Code.Worker
{
    /// <summary>
    /// Summary description for Notifier
    /// </summary>
    public class Notifier
    {
        List<Notification> _output = new List<Notification>();
        /// <summary>
        /// sends the notification
        /// </summary>
        /// <param name="input"></param>
        public void SendNotification(Notification input)
        {
            _output.Add(input);
        }
       /// <summary>
       /// 
       /// </summary>
        public void Commit()
        {
            foreach (var item in _output)
                if (item.UserId == new Guid())
                    _output.Remove(item);
            NotifierWorker.Submit(_output);
            _output.Clear();
        }
        /// <summary>
        /// Creates the string notifier
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            foreach (var item in _output)
            {
                output.AppendLine(string.Format("UserID: {0}<br />ProvinceID: {1}<br />Province Name: {2}<br />Details:", item.UserId.ToString(), item.ProvinceId.ToString(), item.ProvinceName));
                foreach (var detail in item.Details)
                                    output.AppendLine(string.Format("Attacker: {0}({1}:{2}) --- Date: {3} --- Event type: {4} --- Event text: {5} --- Location: {6}", detail.Attacker.Name, detail.Attacker.Location.Island.ToString(), detail.Attacker.Location.Kingdom.ToString(), Convert.ToString("YR" + detail.Date.Year.ToString() + ", M: " + detail.Date.Month.ToString() + ", D: " + detail.Date.Day.ToString()), detail.EventType.ToString(), detail.EventText, Convert.ToString(detail.Location.Island.ToString() + ":" + detail.Location.Kingdom.ToString())));
                
                output.AppendLine("<br />");
            }
            return output.ToString();
        }
    }
}