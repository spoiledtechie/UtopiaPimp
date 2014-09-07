using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Pimp.UData;
using Pimp.Utopia;
using Pimp.Users;

using Boomers.UserUtil;

namespace Pimp.UCache
{

    /// <summary>
    /// Summary description for UsersCache
    /// </summary>
    public class UsersCache
    {

        /// <summary>
        /// changes the current active province for the user
        /// </summary>
        /// <param name="toProvinceID"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public static PimpUserWrapper  ChangeCurrentActiveProvinceInCache(Guid toProvinceID, PimpUserWrapper  currentUser)
        {
            currentUser.PimpUser.CurrentActiveProvince = toProvinceID;
            currentUser.PimpUser.CurrentActiveProvinceName = currentUser.PimpUser.ProvincesOwned.Where(x => x.Province_ID == toProvinceID).FirstOrDefault().Province_Name;
            PimpUserWrapper.updateListOfUsers(currentUser.PimpUser);
            return currentUser;
        }
        public static Contact GetContact(Guid ownerKingdomID, PimpUserWrapper  currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            var getContacts = cachedKingdom.Contacts;
            if (getContacts != null && getContacts.Count > 0)
            {
                var contact = getContacts.Where(x => x.user_ID == currentUser.PimpUser.UserID).FirstOrDefault();
                if (contact != null)
                    return contact;
            }
            Guid appGuid = SupportFramework.Applications.Instance.ApplicationId;

            updateContactForUser(ownerKingdomID, currentUser.PimpUser.UserID, appGuid, cachedKingdom);
            return SupportFramework.Users.Memberships.getUserContact(currentUser.PimpUser.UserID, appGuid);
        }
        /// <summary>
        /// adds a phone number of a contact to cache
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="userID"></param>
        /// <param name="ownerKingdomID"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static List<Contact> addPhoneNumberToCache(PhoneType phone, Guid userID, Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            //if contacts are null.
            if (cachedKingdom.Contacts == null)
            {
                cachedKingdom.Contacts = KingdomCache.getContactsForKingdom(ownerKingdomID, cachedKingdom);
                return cachedKingdom.Contacts;
            }

            if (cachedKingdom.Contacts.Count > 0)
            {
                Contact contact = cachedKingdom.Contacts.Where(x => x.user_ID == userID).FirstOrDefault();
                if (contact != null)
                {
                    contact.phoneNumbers.Add(phone);
                    cachedKingdom.Contacts.Remove(cachedKingdom.Contacts.Where(x => x.user_ID == userID).FirstOrDefault());
                }
                else
                {
                    contact = SupportFramework.Users.Memberships.getUserContact(userID, SupportFramework.Applications.Instance.ApplicationId);
                }

                if (contact != null)
                    cachedKingdom.Contacts.Add(contact);
                HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            }
            return cachedKingdom.Contacts;
        }

        public static List<Contact> updateContactForUser(Guid ownerKingdomID, Guid userID, Guid applicationID, OwnedKingdomProvinces cachedKingdom)
        {
            //if contacts are null.
            if (cachedKingdom.Contacts == null)
                KingdomCache.getContactsForKingdom(ownerKingdomID, cachedKingdom);

            HttpContext.Current.Session.Add("SubmittedData", "ownerKingdom:" + ownerKingdomID.ToString() + "userID:" + userID.ToString() + "AppID:" + applicationID + "cachedKingdom" + cachedKingdom.Contacts.Count);
            if (cachedKingdom.Contacts == null)
            {
                cachedKingdom.Contacts = Kingdom.getKingdomContacts(ownerKingdomID, cachedKingdom);
                HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            }
            else
            {
                if (cachedKingdom.Contacts.Count > 0)
                    if (cachedKingdom.Contacts.Where(x => x.user_ID == userID).FirstOrDefault() != null)
                        cachedKingdom.Contacts.Remove(cachedKingdom.Contacts.Where(x => x.user_ID == userID).FirstOrDefault());
                var contact = SupportFramework.Users.Memberships.getUserContact(userID, applicationID);
                if (contact != null)
                    cachedKingdom.Contacts.Add(contact);
                HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            }
            return cachedKingdom.Contacts;
        }

    }
}