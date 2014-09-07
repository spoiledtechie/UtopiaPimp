using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.Numbers
{
    class Geometry
    {
        public static double dot(Vector v1, Vector v2)
        { return (v1.X * v2.X + v1.Y * v2.Y); }
        /// <summary>
        /// Get the distance between two points
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double distance(Point p1, Point p2)
        {
            var a = Math.Pow(p1.X, 2) - Math.Pow(p2.X, 2);
            var b = Math.Pow(p1.Y, 2) - Math.Pow(p2.Y, 2);
            return Math.Sqrt(Math.Abs(a + b));
        }
        /// <summary>
        /// Get the shortest distance between point and line segment
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static double distance(Point p1, Point p2, Point p)
        {
            Vector v = new Vector();
            v.X = p1.X - p2.X;
            v.Y = p1.Y - p2.Y;
            Vector w = new Vector();
            w.X = p.X - p2.X;
            w.Y = p.Y - p2.Y;

            var c1 = dot(w, v);
            if (c1 <= 0) return distance(p, p1);

            var c2 = dot(v, v);
            if (c2 <= c1) return distance(p, p2);

            var b = c1 / c2;
            Point Pb = new Point();
            Pb.X = p1.X + (b * v.X);
            Pb.Y = p1.Y + (b * v.Y);
            return distance(p, Pb);
        }
    }
}
