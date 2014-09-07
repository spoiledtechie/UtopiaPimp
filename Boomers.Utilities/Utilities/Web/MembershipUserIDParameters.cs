using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.Security;

namespace Boomers.Utilities.Web
{
    public class MembershipUserIDParameters : Parameter
    {
        #region Constructors
        /// <summary>
        /// The default constructor; creates a new instance of the MembershipUserIdParameter object.
        /// </summary>
        public MembershipUserIDParameters()
        {

        }

        /// <remarks>Used by the <see cref="Clone" /> method to replicate the parameter object.</remarks>
        protected MembershipUserIDParameters(MembershipUserIDParameters original)
        {
            //If you add any properties to MembershipUserIdParameter, add code here to copy them from original to Me
        }
        #endregion
        #region Methods
        /// <summary>
        /// Returns the value of the parameter. For MembershipUserIdParameter, this is the currently logged
        /// on user's UserId. If the current visitor is anonymous, a value of <b>Nothing</b> (<b>null</b> in C#)
        /// is returned.
        /// </summary>
        protected override object Evaluate(System.Web.HttpContext context, System.Web.UI.Control control)
        {
            MembershipUser currentUser = Membership.GetUser();
            if (currentUser == null)
            {
                //Either Membership is not setup or the visitor to this page is anonymous
                return null;
            }
            else
            {
                //Return the currently logged on user's UserId
                return currentUser.ProviderUserKey;
            }
        }

        /// <summary>
        /// Creates a clone of the parameter object
        /// </summary>
        /// <remarks>Needs to be provided in order to support design-time parameter editing support.
        /// See http://www.leftslipper.com/ShowFaq.aspx?FaqId=11 for more information.</remarks>
        protected override System.Web.UI.WebControls.Parameter Clone()
        {
            return new MembershipUserIDParameters(this);
        }
        #endregion
    }
}
