using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.Collections
{
    // cause Distinct implementation is shit
    public class CollectionComparer<T> : IEqualityComparer<IEnumerable<T>> where T : IComparable<T>
    {
        Dictionary<IEnumerable<T>, int> dict = new Dictionary<IEnumerable<T>, int>();

        public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
        {
            if (x.Count() != y.Count())
                return false;
            return x.Zip(y, (xi, yi) => xi.Equals(yi)).All(compareResult => compareResult);
        }

        public int GetHashCode(IEnumerable<T> obj)
        {
            var inDict = dict.Keys.FirstOrDefault(k => Equals(k, obj));
            if (inDict != null)
                return dict[inDict];
            else
            {
                int n = dict.Count;
                dict[obj] = n;
                return n;
            }
        }
    }// class
}
