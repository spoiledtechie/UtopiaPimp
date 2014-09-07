using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Boomers.Utilities.Geospatial
{
    public class Rhumb
    {
        /// <summary>
        /// Rhumb lines or loxodrome is a path of constant bearing, which crosses all meridians at the same angle.
        /// This calculation is very useful, if you want to follow constant compass bearing, instead of continually 
        /// adjustment of it.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double GetRhumbDistance(GeoCourdinate start, GeoCourdinate end)
        {
            var dLat = Geo.DegToRad(end.Latitude - start.Latitude);
            var dLon = Geo.DegToRad(end.Longitude - start.Longitude);
            var lat1 = Geo.DegToRad(start.Latitude);
            var lon1 = Geo.DegToRad(start.Longitude);
            var lat2 = Geo.DegToRad(end.Latitude);
            var lon2 = Geo.DegToRad(end.Longitude);

            var dPhi = Math.Log(Math.Tan(lat2 / 2 + Math.PI / 4) / Math.Tan(lat1 / 2 + Math.PI / 4));
            var q = (Math.Abs(dLat) > 1e-10) ? dLat / dPhi : Math.Cos(lat1);
            if (Math.Abs(dLon) > Math.PI)
            {
                dLon = dLon > 0 ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);
            }
            return Math.Sqrt(dLat * dLat + q * q * dLon * dLon) * Geo.sm_a;
        }

        public static double GetRhumbBearing(GeoCourdinate start, GeoCourdinate end)
        {
            var dLat = Geo.DegToRad(end.Latitude - start.Latitude);
            var dLon = Geo.DegToRad(end.Longitude - start.Longitude);

            var lat1 = Geo.DegToRad(start.Latitude);
            var lon1 = Geo.DegToRad(start.Longitude);
            var lat2 = Geo.DegToRad(end.Latitude);
            var lon2 = Geo.DegToRad(end.Longitude);

            var dPhi = Math.Log(Math.Tan(lat2 / 2 + Math.PI / 4) / Math.Tan(lat1 / 2 + Math.PI / 4));
            if (Math.Abs(dLon) > Math.PI)
            {
                dLon = dLon > 0 ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);
            }
            return Math.Atan2(dLon, dPhi);
        }

        public static GeoCourdinate GetRhumbDestination(GeoCourdinate start, GeoCourdinate end)
        {
            var lat1 = Geo.DegToRad(start.Latitude);
            var lon1 = Geo.DegToRad(start.Longitude);
            var lat2 = Geo.DegToRad(end.Latitude);
            var lon2 = Geo.DegToRad(end.Longitude);

            var d = GetRhumbDistance(start, end);
            var b = GetRhumbBearing(start, end);
            lat2 = lat1 + d * Math.Cos(b);
            var dPhi = Math.Log(Math.Tan(lat2 / 2 + Math.PI / 4) / Math.Tan(lat1 / 2 + Math.PI / 4));
            var q = (Math.Abs(lat2 - lat1) > 1e-10) ? (lat2 - lat1) / dPhi : Math.Cos(lat1);
            var dLon = d * Math.Sin(b) / q;
            if (Math.Abs(lat2) > Math.PI / 2) lat2 = lat2 > 0 ? Math.PI - lat2 : -Math.PI - lat2;
            lon2 = (lon1 + dLon + Math.PI) % (2 * Math.PI) - Math.PI;

            GeoCourdinate coord = new GeoCourdinate();
            coord.Longitude = Geo.RadToDeg(lon2);
            coord.Latitude = Geo.RadToDeg(lat2);
            return coord;
        }
    }
}
