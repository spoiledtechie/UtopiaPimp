using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using Boomers.Utilities.Text;

namespace Boomers.Political.SunlightFoundation.Money
{
    public class ContributionsModel
    {
        private static string TRANSPARENCY_DATA_JSON_API_URL = "http://transparencydata.com/api/1.0/contributions.json";


        private string _committee_ext_id;

        public string Committee_ext_id
        {
            get { return _committee_ext_id; }
            set { _committee_ext_id = value; }
        }
        private string _seat_held;

        public string Seat_held
        {
            get { return _seat_held; }
            set { _seat_held = value; }
        }
        private string _recipient_party;

        public string Recipient_party
        {
            get { return _recipient_party; }
            set { _recipient_party = value; }
        }
        private string _recipient_type;

        public string Recipient_type
        {
            get { return _recipient_type; }
            set { _recipient_type = value; }
        }
        private string _seat_status;

        public string Seat_status
        {
            get { return _seat_status; }
            set { _seat_status = value; }
        }
        private string _recipient_state;

        public string Recipient_state
        {
            get { return _recipient_state; }
            set { _recipient_state = value; }
        }
        private string _contributor_category;

        public string Contributor_category
        {
            get { return _contributor_category; }
            set { _contributor_category = value; }
        }
        private string _contributor_gender;

        public string Contributor_gender
        {
            get { return _contributor_gender; }
            set { _contributor_gender = value; }
        }
        private string _contributor_state;

        public string Contributor_state
        {
            get { return _contributor_state; }
            set { _contributor_state = value; }
        }
        private string _recipient_category;

        public string Recipient_category
        {
            get { return _recipient_category; }
            set { _recipient_category = value; }
        }
        private string _is_amendment;

        public string Is_amendment
        {
            get { return _is_amendment; }
            set { _is_amendment = value; }
        }
        private string _district;

        public string District
        {
            get { return _district; }
            set { _district = value; }
        }
        private string _organization_name;

        public string Organization_name
        {
            get { return _organization_name; }
            set { _organization_name = value; }
        }
        private string _recipient_ext_id;

        public string Recipient_ext_id
        {
            get { return _recipient_ext_id; }
            set { _recipient_ext_id = value; }
        }
        private string _parent_organization_name;

        public string Parent_organization_name
        {
            get { return _parent_organization_name; }
            set { _parent_organization_name = value; }
        }
        private string _contributor_address;

        public string Contributor_address
        {
            get { return _contributor_address; }
            set { _contributor_address = value.ToPascelCase(); }
        }
        private string _transaction_id;

        public string Transaction_Id
        {
            get { return _transaction_id; }
            set { _transaction_id = value; }
        }
        private string _contributor_occupation;

        public string Contributor_occupation
        {
            get { return _contributor_occupation; }
            set { _contributor_occupation = value; }
        }
        private string _filing_id;

        public string Filing_id
        {
            get { return _filing_id; }
            set { _filing_id = value; }
        }
        private string _contributor_city;

        public string Contributor_city
        {
            get { return _contributor_city; }
            set { _contributor_city = value; }
        }
        private string _recipient_state_held;

        public string Recipient_state_held
        {
            get { return _recipient_state_held; }
            set { _recipient_state_held = value; }
        }
        private string _district_held;

        public string District_held
        {
            get { return _district_held; }
            set { _district_held = value; }
        }
        private string _recipient_name;

        public string Recipient_name
        {
            get { return _recipient_name; }
            set { _recipient_name = value; }
        }
        private string _organization_ext_id;

        public string Organization_ext_id
        {
            get { return _organization_ext_id; }
            set { _organization_ext_id = value; }
        }
        private string _contributor_zipcode;

        public string Contributor_zipcode
        {
            get { return _contributor_zipcode; }
            set { _contributor_zipcode = value; }
        }
        private string _transaction_namespace;
        /// <summary>
        /// filters on federal or state contributions
        ///Options	Description
        ///urn:fec:transaction	federal contributions
        ///urn:nimsp:transaction	state contributions
        /// </summary>
        public string Transaction_namespace
        {
            get { return _transaction_namespace; }
            set { _transaction_namespace = value; }
        }
        private string _committee_name;

        public string Committee_name
        {
            get { return _committee_name; }
            set { _committee_name = value; }
        }
        private string _candidacy_status;

        public string Candidacy_status
        {
            get { return _candidacy_status; }
            set { _candidacy_status = value; }
        }
        private string _parent_organization_ext_id;

        public string Parent_organization_ext_id
        {
            get { return _parent_organization_ext_id; }
            set { _parent_organization_ext_id = value; }
        }
        private string _contributor_name;

        public string Contributor_name
        {
            get { return _contributor_name; }
            set { _contributor_name = value; }
        }
        private string _contributor_type;

        public string Contributor_type
        {
            get { return _contributor_type; }
            set { _contributor_type = value; }
        }
        private string _contributor_employer;

        public string Contributor_employer
        {
            get { return _contributor_employer; }
            set { _contributor_employer = value; }
        }
        private string _seat_result;

        public string Seat_result
        {
            get { return _seat_result; }
            set { _seat_result = value; }
        }
        private string _transaction_type;

        public string Transaction_type
        {
            get { return _transaction_type; }
            set { _transaction_type = value; }
        }
        private string _contributor_ext_id;

        public string Contributor_ext_id
        {
            get { return _contributor_ext_id; }
            set { _contributor_ext_id = value; }
        }
        private string _committee_party;

        public string Committee_party
        {
            get { return _committee_party; }
            set { _committee_party = value; }
        }

        /// <summary>
        /// The amount of the contribution in US dollars in one of the following formats:
        ///Example	Description
        ///500	exactly 500 dollars
        ///>|500	greater than or equal to 500
        ///<|500	less than or equal to 500
        /// </summary>
        private string _amount;

        public string Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }


        /// <summary>
        /// A YYYY formatted year, 1990 - 2010.
        /// </summary>
        private string _cycle;

        public string Cycle
        {
            get { return _cycle; }
            set { _cycle = value; }
        }
        /// <summary>
        /// date of the contribution in ISO date format
        /// </summary>
        private string _date;

        public string Date
        {
            get { return _date; }
            set { _date = value; }
        }

        /// <summary>
        /// When organizations run ads against a candidate, they are counted as independent expenditures with the candidate as the recipient. This parameter can be used to filter contributions meant for the candidate and those meant to be against the candidate.
        ///Options	Description
        ///for	contributions made in support of the candidate
        ///against	contributions made against the candidate
        /// </summary>
        private string _forAgainst;

        public string ForAgainst
        {
            get { return _forAgainst; }
            set { _forAgainst = value; }
        }

        /// <summary>
        /// two-letter abbreviation of state in which the candidate receiving the contribution is running
        /// </summary>
        private string _recipientState;

        public string RecipientState
        {
            get { return _recipientState; }
            set { _recipientState = value; }
        }

        /// <summary>
        /// type of office being sought
        ///Options	Description
        ///federal:senate	US Senate
        ///federal:house	US House of Representatives
        ///federal:president	US President
        ///state:upper	upper chamber of state legislature
        ///state:lower	lower chamber of state legislature
        ///state:governor	state governor
        ///Multiple values must be separated by a pipe character.
        ///Example	Description
        ///federal:senate	only the US Senate
        ///federal:senate|federal:house	US Senate OR US House
        /// </summary>
        private string _seat;

        public string Seat
        {
            get { return _seat; }
            set { _seat = value; }
        }
        /// <summary>
        /// filters on federal or state contributions
        ///Options	Description
        ///urn:fec:transaction	federal contributions
        ///urn:nimsp:transaction	state contributions
        /// </summary>


        /// <summary>
        /// http://transparencydata.com/api/contributions/
        /// </summary>
        public List<ContributionsModel> GetContributionsFromTransparencyData(string apiKey, int page, int pageSize)
        {
            string url = TRANSPARENCY_DATA_JSON_API_URL + "?apikey=" + apiKey;

            if (page != 0)
            {
                url += "&page=" + page;
            }
            if (pageSize < 1000)
            {
                throw new Exception("Page Size must be larger than 1000");
            }
            if (pageSize != 0)
            {
                url += "&per_page=" + pageSize;
            }

            return GetContributionsData(url);
        }
        private List<ContributionsModel> GetContributionsData(string url)
        {
            WebClient wc = new WebClient();
            try
            {
                string data = wc.DownloadString(url);
                return JsonConvert.DeserializeObject<List<ContributionsModel>>(data);
            }
            catch (Exception e)
            {
                Boomers.Utilities.Documents.TextLogger.LogItem("BlackTract", "URL didn't work for Contributions" + url);
            }
            return new List<ContributionsModel>();
        }
    }
}