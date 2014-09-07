using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.UserUtil
{
    public class Contact
    {
        public string State;
        public string Nick_Name;
        public string GMT_Offset;
        public string Country;
        public string City;
        public Guid user_ID;
        public string Notes;
        public List<PhoneType> phoneNumbers;
        public List<IMType> imNames;
        public DateTime? DOB;
    }
}
