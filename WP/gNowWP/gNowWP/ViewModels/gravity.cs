using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gNowWP.ViewModels
{
    class gravity
    {
        /*Basic Constants
         * Max height
         * Min height
         * Value of feets
         */
        private const int everest = 6168;
        private const int deadSea = -427;
        private const double feet = 3.2808399;

        private double longitude;
        private double latitude;
        private double altitude;

        public gravity()
        {
            longitude = 0;
            latitude = 0;
            altitude = 0;
        }

        public gravity(double lat, double lng, double alt)
        {
            this.longitude = lng;
            this.latitude = lat;
            this.altitude = alt;
        }

        /*
         * 0 means invalid latitude
         * 1 means higher than Everest
         * 2 means deeper than Dead Sea
         * Else other values means a gravity
         */
        public double getGravity()
        {
            #region validations
            if (this.latitude < -90 || this.latitude > 90)
                return 0;

            if (this.altitude > everest)
                return 1;

            if (this.altitude < deadSea)
                return 2;
            #endregion

            var IGF = 9.780327 * (1 + 0.0053024 * Math.Pow(Math.Sin(this.latitude), 2) - 0.0000058 * Math.Pow(Math.Sin(2 * this.latitude), 2));

            var FAC = -3.086 * Math.Pow(10, -6) * this.altitude;

            var g = IGF + FAC;

            return g;
        }

        public double changeToMetres(double value)
        {
            return value / feet;
        }

        public double changeToFeet(double value)
        {
            return value * feet;
        }

    }
}
