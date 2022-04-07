using System;
using System.Text;

namespace AgOpenGPS
{
    public class CNMEA
    {
        //WGS84 Lat Long
        public double latitude, longitude, latitudeTool, longitudeTool;

        //local plane geometry
        public double latStart, lonStart;
        public double mPerDegreeLat, mPerDegreeLon;

        //our current fix
        public vec2 fix = new vec2(0, 0);
        public vec2 fixTool = new vec2(0, 0);
        
        //used to offset the antenna position to compensate for drift
        public vec2 fixOffset = new vec2(0, 0);

        //other GIS Info
        public double altitude, speed;

        public double headingTrueDual, headingTrueDualTool, headingTrue, hdop, age, hdopTool, ageTool, headingTrueDualOffset;

        public int fixQuality, fixQualityTool, ageAlarm;
        public int satellitesTracked, satellitesTrackedTool;
        public bool isToolSteering;

        public StringBuilder logNMEASentence = new StringBuilder();

        private readonly FormGPS mf;
        public CNMEA(FormGPS f)
        {
            //constructor, grab the main form reference
            mf = f;
            latStart = 0;
            lonStart = 0;
            ageAlarm = Properties.Settings.Default.setGPS_ageAlarm;
        }

        public void AverageTheSpeed()
        {
            //average the speed
            //if (mf.isReverse) speed *= -1;

            mf.avgSpeed = (mf.avgSpeed * 0.5) + (speed * 0.5);
        }

        public void SetLocalMetersPerDegree()
        {
            double LatRad = latStart * 0.01745329251994329576923690766743;

            mPerDegreeLat = 111132.92 - 559.82 * Math.Cos(2.0 * LatRad) + 1.175 * Math.Cos(4.0 * LatRad) - 0.0023 * Math.Cos(6.0 * LatRad);
            mPerDegreeLon = 111412.84 * Math.Cos(LatRad) - 93.5 * Math.Cos(3.0 * LatRad) + 0.118 * Math.Cos(5.0 * LatRad);

            ConvertWGS84ToLocal(latitude, longitude, out double northing, out double easting);
            mf.worldManager.checkZoomWorldGrid(northing, easting);
        }

        public void ConvertWGS84ToLocal(double Lat, double Lon, out double Northing, out double Easting)
        {
            double LatRad = Lat * 0.01745329251994329576923690766743;
            mPerDegreeLon = 111412.84 * Math.Cos(LatRad) - 93.5 * Math.Cos(3.0 * LatRad) + 0.118 * Math.Cos(5.0 * LatRad);

            Northing = (Lat - latStart) * mPerDegreeLat;
            Easting = (Lon - lonStart) * mPerDegreeLon;
        }

        public void ConvertLocalToWGS84(double Northing, double Easting, out double Lat, out double Lon)
        {
            Lat = ((Northing + fixOffset.northing) / mPerDegreeLat) + latStart;
            double LatRad = Lat * 0.01745329251994329576923690766743;

            mPerDegreeLon = 111412.84 * Math.Cos(LatRad) - 93.5 * Math.Cos(3.0 * LatRad) + 0.118 * Math.Cos(5.0 * LatRad);
            Lon = ((Easting + fixOffset.easting) / mPerDegreeLon) + lonStart;
        }

        public string GetLocalToWSG84_KML(double Easting, double Northing)
        {
            double Lat = (Northing / mPerDegreeLat) + latStart;
            double LatRad = Lat * 0.01745329251994329576923690766743;

            mPerDegreeLon = 111412.84 * Math.Cos(LatRad) - 93.5 * Math.Cos(3.0 * LatRad) + 0.118 * Math.Cos(5.0 * LatRad);
            double Lon = (Easting / mPerDegreeLon) + lonStart;

            return Lon.ToString("0.0000000") + ',' + Lat.ToString("0.0000000") + ",0 ";
        }
    }
}