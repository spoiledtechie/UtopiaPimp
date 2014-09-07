using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Pimp;
using Boomers.UserUtil;

namespace Pimp.Users
{
    public class UserPreferences
    {
        private readonly NotificationSettings notifierSettings;
        public NotificationSettings Email { get { return notifierSettings; } }

        private readonly ContactSettings contactSettings;
        public ContactSettings Contact { get { return contactSettings; } }

        public Guid UserID { get; set; }
        public IEnumerable<Guid> UserKingdoms { get; set; }

        public UserPreferences()
        {
            notifierSettings = new NotificationSettings();
            contactSettings = new ContactSettings();
        }

        public class NotificationSettings
        {
            public NotificationSettings() { }
 
            public bool AttackAgainstSelf { get; set; } // An attack was commited against the player
            public bool DragonProjectStarted { get; set; } // Dragon prject launched
            public bool DragonRavagingLands { get; set; } // Dragon has begun ravaging the lands                        
            public bool ArmyReturns { get; set; } // The users are has returned from battle
            public bool WarAction { get; set; } // Kingdom enters/exits a war
            public bool OpsEnded { get; set; }
            public bool TrackedProvinceUpdated { get; set; } // The users tracked target changed
            public DateTime LastEmailSent { get; set; }
            public TimeSpan SendFrequency { get; set; }
            public NotificationType DeliveryMethod { get; set; }

            public enum NotificationType : byte
            {
                Email = 1,
                Sms = 2
            }
        }

        public class ContactSettings
        {
            internal ContactSettings() { }

            public string State { get; set; }
            public string Nick_Name { get; set; }
            public string GMT_Offset { get; set; }
            public string Country { get; set; }
            public string City { get; set; }
            public Guid user_ID { get; set; }
            public string Notes { get; set; }
            public List<PhoneType> phoneNumbers { get; set; }
            public List<IMType> imNames { get; set; }
            public DateTime? DOB { get; set; }
        }
    }
}