using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pimp.Chat
{
    /// <summary>
    /// A message to be sent over chat.
    /// </summary>    
    public class Message
    {
        public Guid FromGuid;
        public string messageFrom;
        public DateTime timeStamp;
        public string message;
        public int uid;
    }

}