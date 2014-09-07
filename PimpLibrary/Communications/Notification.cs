using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PimpLibrary.Communications
{
    public class Notification
    {
        public Notification()
        {
            Details = new List<NotificationDetail>();
        }

        public Guid UserId { get; set; }
        public Guid ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public List<NotificationDetail> Details { get; set; }
    }
}
