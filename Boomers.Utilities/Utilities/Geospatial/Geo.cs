using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Boomers.Utilities.Geospatial
{
    public class Geo
    {
        public const double sm_a = 6378137.0;
        public const double sm_b = 6356752.314;
        public const double sm_EccSquared = 6.69437999013e-03;
        public const double UTMScaleFactor = 0.9996;

        public static GeoCourdinate GetMidpoint(GeoCourdinate start, GeoCourdinate end)
        {
            var dLat = DegToRad(end.Latitude - start.Latitude);
            var dLon = DegToRad(end.Longitude - start.Longitude);
            var lat2 = DegToRad(end.Latitude);
            var lat1 = DegToRad(start.Latitude);
            var lon1 = DegToRad(start.Longitude);

            var Bx = Math.Cos(lat2) * Math.Cos(dLon);
            var By = Math.Cos(lat2) * Math.Sin(dLon);
            var lat3 = Math.Atan2(Math.Sin(lat1) + Math.Sin(lat2), Math.Sqrt((Math.Cos(lat1) + Bx) * (Math.Cos(lat1) + Bx) + By * By));
            var lon3 = lon1 + Math.Atan2(By, Math.Cos(lat1) + Bx);
            GeoCourdinate coord = new GeoCourdinate();
            coord.Longitude = RadToDeg(lon3);
            coord.Latitude = RadToDeg(lat3);
            return coord;
        }

        /// <summary>
        /// Gets the Destination Endpoint In GeoCourdinate Style
        /// </summary>
        /// <param name="start"></param>
        /// <param name="distance"></param>
        /// <param name="azimuth"></param>
        /// <returns></returns>
        public static GeoCourdinate GetDestinationEndPoint(GeoCourdinate start, double distance, double azimuth)
        {
            var lat = RadToDeg(Math.Asin(Math.Sin(DegToRad(start.Latitude)) * Math.Cos(DegToRad(distance / sm_a)) + Math.Cos(DegToRad(start.Latitude)) * Math.Sin(DegToRad(distance / sm_a)) * Math.Cos(DegToRad(azimuth))));
            var lon = start.Longitude + DegToRad(Math.Atan2(Math.Sin(DegToRad(azimuth)) * Math.Sin(DegToRad(distance / sm_a)) * Math.Cos(DegToRad(start.Latitude)), Math.Cos(DegToRad(distance / sm_a)) - Math.Sin(DegToRad(start.Latitude)) * Math.Sin(DegToRad(lat))));
            GeoCourdinate coord = new GeoCourdinate();
            coord.Longitude = lon;
            coord.Latitude = lat;
            return coord;
        }

        public static double GetDistance(GeoCourdinate c1, GeoCourdinate c2)
        {
            var dLat = DegToRad(c2.Latitude - c1.Latitude);
            var dLon = DegToRad(c2.Longitude - c2.Longitude);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(DegToRad(c1.Latitude)) * Math.Cos(DegToRad(c2.Latitude)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return sm_a * c;
        }

        public static double GetAzimuth(GeoCourdinate c1, GeoCourdinate c2)
        {
            var lat1 = DegToRad(c1.Latitude);
            var lon1 = DegToRad(c1.Longitude);
            var lat2 = DegToRad(c2.Latitude);
            var lon2 = DegToRad(c2.Longitude);

            return RadToDeg(Math.Asin(Math.Sin(lon1 - lon2) * Math.Cos(lat2) / Math.Sin(Math.Acos(Math.Sin(lat2) * Math.Sin(lat1) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(lon2 - lon1)))));
        }

        public static GeoCourdinate UTMToLatLong(GeoPoint xy, bool southernHemisphere)
        {
            xy.X -= 500000.0;
            xy.X /= UTMScaleFactor;

            /* If in southern hemisphere, adjust y accordingly. */
            if (southernHemisphere)
                xy.Y -= 10000000.0;

            xy.Y /= UTMScaleFactor;
            int zone = UTMZone(xy.X);
            var cMeridian = UTMToCentralMeridian(zone);
            GeoCourdinate point = MapXYToLatLon(xy.X, xy.Y, cMeridian);
            point.Latitude = RadToDeg(point.Latitude);
            point.Longitude = RadToDeg(point.Longitude);
            return point;
        }

        public static GeoCourdinate LatLongToUTM(GeoPoint xy)
        {
            int zone = UTMZone(xy.X);
            double phi = UTMToCentralMeridian(zone);
            double fotoprint = FootpointLatitude(xy.Y);
            double ArcLength = ArcLengthOfMeridian(phi);
            GeoPoint point = MapLatLonToXY(phi, fotoprint, ArcLength);
            GeoCourdinate GeoCoord = MapXYToLatLon(point.X, point.Y, ArcLength);
            return AdjustEastingNorthingforUTM(GeoCoord);
        }

        public static GeoCourdinate AdjustEastingNorthingforUTM(GeoCourdinate xy)
        {
            xy.Longitude = xy.Longitude * UTMScaleFactor + 500000.0;
            xy.Latitude = xy.Latitude * UTMScaleFactor;
            if (xy.Latitude < 0.0)
                xy.Latitude += 10000000.0;
            return xy;
        }

        public static double FootpointLatitude(double y)
        {
            /* Precalculate n (Eq. 10.18) */
            double n = (sm_a - sm_b) / (sm_a + sm_b);

            /* Precalculate alpha_ (Eq. 10.22) */
            /* (Same as alpha in Eq. 10.17) */
            var alpha_ = ((sm_a + sm_b) / 2.0) * (1 + (Math.Pow(n, 2.0) / 4) + (Math.Pow(n, 4.0) / 64));

            /* Precalculate y_ (Eq. 10.23) */
            var y_ = y / alpha_;

            /* Precalculate beta_ (Eq. 10.22) */
            var beta_ = (3.0 * n / 2.0) + (-27.0 * Math.Pow(n, 3.0) / 32.0) + (269.0 * Math.Pow(n, 5.0) / 512.0);

            /* Precalculate gamma_ (Eq. 10.22) */
            var gamma_ = (21.0 * Math.Pow(n, 2.0) / 16.0) + (-55.0 * Math.Pow(n, 4.0) / 32.0);

            /* Precalculate delta_ (Eq. 10.22) */
            var delta_ = (151.0 * Math.Pow(n, 3.0) / 96.0) + (-417.0 * Math.Pow(n, 5.0) / 128.0);

            /* Precalculate epsilon_ (Eq. 10.22) */
            var epsilon_ = (1097.0 * Math.Pow(n, 4.0) / 512.0);

            /* Now calculate the sum of the series (Eq. 10.21) */
            return y_ + (beta_ * Math.Sin(2.0 * y_)) + (gamma_ * Math.Sin(4.0 * y_)) + (delta_ * Math.Sin(6.0 * y_)) + (epsilon_ * Math.Sin(8.0 * y_));
        }

        public static double ArcLengthOfMeridian(double phi)
        {
            /* Precalculate n */
            double n = (sm_a - sm_b) / (sm_a + sm_b);

            /* Precalculate alpha */
            var alpha = ((sm_a + sm_b) / 2.0) * (1.0 + (Math.Pow(n, 2.0) / 4.0) + (Math.Pow(n, 4.0) / 64.0));

            /* Precalculate beta */
            var beta = (-3.0 * n / 2.0) + (9.0 * Math.Pow(n, 3.0) / 16.0) + (-3.0 * Math.Pow(n, 5.0) / 32.0);

            /* Precalculate gamma */
            var gamma = (15.0 * Math.Pow(n, 2.0) / 16.0) + (-15.0 * Math.Pow(n, 4.0) / 32.0);

            /* Precalculate delta */
            var delta = (-35.0 * Math.Pow(n, 3.0) / 48.0) + (105.0 * Math.Pow(n, 5.0) / 256.0);

            /* Precalculate epsilon */
            var epsilon = (315.0 * Math.Pow(n, 4.0) / 512.0);

            /* Now calculate the sum of the series and return */
            return alpha * (phi + (beta * Math.Sin(2.0 * phi)) + (gamma * Math.Sin(4.0 * phi)) + (delta * Math.Sin(6.0 * phi)) + (epsilon * Math.Sin(8.0 * phi)));
        }
        public static GeoPoint MapLatLonToXY(double phi, double lambda, double lambda0)
        {
            /* Precalculate ep2 */
            var ep2 = (Math.Pow(sm_a, 2.0) - Math.Pow(sm_b, 2.0)) / Math.Pow(sm_b, 2.0);

            /* Precalculate nu2 */
            var nu2 = ep2 * Math.Pow(Math.Cos(phi), 2.0);

            /* Precalculate N */
            var N = Math.Pow(sm_a, 2.0) / (sm_b * Math.Sqrt(1 + nu2));

            /* Precalculate t */
            var t = Math.Tan(phi);
            var t2 = t * t;
            var tmp = (t2 * t2 * t2) - Math.Pow(t, 6.0);

            /* Precalculate l */
            var l = lambda - lambda0;

            /* Precalculate coefficients for l**n in the equations below 
               so a normal human being can read the expressions for easting 
               and northing 
               — l**1 and l**2 have coefficients of 1.0 */
            var l3coef = 1.0 - t2 + nu2;

            var l4coef = 5.0 - t2 + 9 * nu2 + 4.0 * (nu2 * nu2);

            var l5coef = 5.0 - 18.0 * t2 + (t2 * t2) + 14.0 * nu2 - 58.0 * t2 * nu2;

            var l6coef = 61.0 - 58.0 * t2 + (t2 * t2) + 270.0 * nu2 - 330.0 * t2 * nu2;

            var l7coef = 61.0 - 479.0 * t2 + 179.0 * (t2 * t2) - (t2 * t2 * t2);

            var l8coef = 1385.0 - 3111.0 * t2 + 543.0 * (t2 * t2) - (t2 * t2 * t2);

            var xy = new GeoPoint();
            /* Calculate easting (x) */
            xy.X = N * Math.Cos(phi) * l + (N / 6.0 * Math.Pow(Math.Cos(phi), 3.0) * l3coef * Math.Pow(l, 3.0)) + (N / 120.0 * Math.Pow(Math.Cos(phi), 5.0) * l5coef * Math.Pow(l, 5.0)) + (N / 5040.0 * Math.Pow(Math.Cos(phi), 7.0) * l7coef * Math.Pow(l, 7.0));

            /* Calculate northing (y) */
            xy.Y = ArcLengthOfMeridian(phi) + (t / 2.0 * N * Math.Pow(Math.Cos(phi), 2.0) * Math.Pow(l, 2.0)) + (t / 24.0 * N * Math.Pow(Math.Cos(phi), 4.0) * l4coef * Math.Pow(l, 4.0)) + (t / 720.0 * N * Math.Pow(Math.Cos(phi), 6.0) * l6coef * Math.Pow(l, 6.0)) + (t / 40320.0 * N * Math.Pow(Math.Cos(phi), 8.0) * l8coef * Math.Pow(l, 8.0));

            return xy;
        }

        public static GeoCourdinate MapXYToLatLon(double x, double y, double lambda0)
        {
            /* Get the value of phif, the footpoint latitude. */
            double phif = FootpointLatitude(y);

            /* Precalculate ep2 */
            double ep2 = (Math.Pow(sm_a, 2.0) - Math.Pow(sm_b, 2.0)) / Math.Pow(sm_b, 2.0);

            /* Precalculate cos (phif) */
            var cf = Math.Cos(phif);

            /* Precalculate nuf2 */
            var nuf2 = ep2 * Math.Pow(cf, 2.0);

            /* Precalculate Nf and initialize Nfpow */
            var Nf = Math.Pow(sm_a, 2.0) / (sm_b * Math.Sqrt(1 + nuf2));
            var Nfpow = Nf;

            /* Precalculate tf */
            var tf = Math.Tan(phif);
            var tf2 = tf * tf;
            var tf4 = tf2 * tf2;

            /* Precalculate fractional coefficients for x**n in the equations 
               below to simplify the expressions for latitude and longitude. */
            var x1frac = 1.0 / (Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**2) */
            var x2frac = tf / (2.0 * Nfpow);

            Nfpow *= Nf;   /* now equals Nf**3) */
            var x3frac = 1.0 / (6.0 * Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**4) */
            var x4frac = tf / (24.0 * Nfpow);

            Nfpow *= Nf;   /* now equals Nf**5) */
            var x5frac = 1.0 / (120.0 * Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**6) */
            var x6frac = tf / (720.0 * Nfpow);

            Nfpow *= Nf;   /* now equals Nf**7) */
            var x7frac = 1.0 / (5040.0 * Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**8) */
            var x8frac = tf / (40320.0 * Nfpow);

            /* Precalculate polynomial coefficients for x**n. 
               — x**1 does not have a polynomial coefficient. */
            var x2poly = -1.0 - nuf2;

            var x3poly = -1.0 - 2 * tf2 - nuf2;

            var x4poly = 5.0 + 3.0 * tf2 + 6.0 * nuf2 - 6.0 * tf2 * nuf2 - 3.0 * (nuf2 * nuf2) - 9.0 * tf2 * (nuf2 * nuf2);

            var x5poly = 5.0 + 28.0 * tf2 + 24.0 * tf4 + 6.0 * nuf2 + 8.0 * tf2 * nuf2;

            var x6poly = -61.0 - 90.0 * tf2 - 45.0 * tf4 - 107.0 * nuf2 + 162.0 * tf2 * nuf2;

            var x7poly = -61.0 - 662.0 * tf2 - 1320.0 * tf4 - 720.0 * (tf4 * tf2);

            var x8poly = 1385.0 + 3633.0 * tf2 + 4095.0 * tf4 + 1575 * (tf4 * tf2);

            var philambda = new GeoCourdinate();
            /* Calculate latitude */
            philambda.Latitude = phif + x2frac * x2poly * (x * x) + x4frac * x4poly * Math.Pow(x, 4.0) + x6frac * x6poly * Math.Pow(x, 6.0) + x8frac * x8poly * Math.Pow(x, 8.0);

            /* Calculate longitude */
            philambda.Longitude = lambda0 + x1frac * x + x3frac * x3poly * Math.Pow(x, 3.0) + x5frac * x5poly * Math.Pow(x, 5.0) + x7frac * x7poly * Math.Pow(x, 7.0);

            return philambda;
        }

        public static double UTMToCentralMeridian(int zone)
        {
            return DegToRad(-183.0 + (zone * 6.0));
        }
        /// <summary>
        /// Converts to UTM Time Zone.
        /// </summary>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public static int UTMZone(double longitude)
        {
            return (int)(Math.Floor((longitude + 180.0) / 6) + 1);
        }
        /// <summary>
        /// Converts Degrees to Radians
        /// </summary>
        /// <param name="deg"></param>
        /// <returns></returns>
        public static double DegToRad(double deg)
        {
            return (deg / 180.0 * Math.PI);
        }
        /// <summary>
        /// Converts Radians to Degrees.
        /// </summary>
        /// <param name="rad"></param>
        /// <returns></returns>
        public static double RadToDeg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
        public static Double ToDecimalDegrees(String degreesMinsSeconds)
        {
            Double degrees = 0;
            Double minutes = 0;
            Double seconds = 0;
            Double decimalDegrees = 0;

            if (!degreesMinsSeconds.Equals(String.Empty))
            {
                if (degreesMinsSeconds.Length > 11)
                {
                    degrees = Convert.ToDouble(degreesMinsSeconds.Substring(1, 2));
                    minutes = Convert.ToDouble(degreesMinsSeconds.Substring(3, 2));
                    seconds = Convert.ToDouble(degreesMinsSeconds.Substring(5, 7));
                    decimalDegrees = ((seconds / 3600) + (minutes / 60) + degrees) * -1;
                }
                else
                {
                    degrees = Convert.ToDouble(degreesMinsSeconds.Substring(0, 2));
                    minutes = Convert.ToDouble(degreesMinsSeconds.Substring(2, 2));
                    seconds = Convert.ToDouble(degreesMinsSeconds.Substring(4, 7));
                    decimalDegrees = (seconds / 3600) + (minutes / 60) + degrees;
                }
            }

            return decimalDegrees;
        }
    }
    
}
