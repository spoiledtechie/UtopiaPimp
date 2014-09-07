using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Boomers.Utilities.Compare
{
    public class GenericComparer : IComparer
    {


        private SortDirection sortDirection;

        public SortDirection SortDirection
        {
            get { return this.sortDirection; }
            set { this.sortDirection = value; }
        }

        private string sortExpression;

        public GenericComparer(string sortExpression, SortDirection sortDirection)
        {
            this.sortExpression = sortExpression;
            this.sortDirection = sortDirection;
        }



        public int Compare(object x, object y)
        {
            PropertyInfo propertyInfo = typeof(string).GetProperty(sortExpression);
            IComparable obj1 = (IComparable)propertyInfo.GetValue(x, null);
            IComparable obj2 = (IComparable)propertyInfo.GetValue(y, null);


            if (SortDirection == SortDirection.Ascending)
            {
                return obj1.CompareTo(obj2);
            }
            else return obj2.CompareTo(obj1);

        }


    }

    class MyType : IComparable
    {
        public MyType(string s)
        {
            Value = s;
        }

        int IComparable.CompareTo(object obj)
        {
            return -(this.Value.CompareTo(((MyType)obj).Value));
        }

        private string _Value;
        public string Value { set { _Value = value; } get { return _Value; } }

        public override string ToString()
        {
            return _Value;
        }
    }
}