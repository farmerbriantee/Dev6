using System;

namespace AgOpenGPS
{
    public class CSim
    {
        private readonly FormGPS mf;

        #region properties sim
        private double northing, easting, heading;
        public double stepDistance = 0.0, steerAngle;
        public double steerAngleScrollBar = 0;
        #endregion properties sim

        public CSim(FormGPS _f)
        {
            mf = _f;
        }

        public void DoSimTick(double _st)
        {
            steerAngle = _st;

            easting += Math.Sin(heading) * stepDistance * mf.fixUpdateTime;
            northing += Math.Cos(heading) * stepDistance * mf.fixUpdateTime;

            vec2 frontWheel;
            frontWheel.easting = (Math.Sin(heading) * mf.vehicle.wheelbase) + Math.Sin(heading + glm.toRadians(steerAngle)) * stepDistance * mf.fixUpdateTime;
            frontWheel.northing = (Math.Cos(heading) * mf.vehicle.wheelbase) + Math.Cos(heading + glm.toRadians(steerAngle)) * stepDistance * mf.fixUpdateTime;

            heading = Math.Atan2(frontWheel.easting, frontWheel.northing);

            mf.pn.fix.easting = easting + (Math.Sin(heading) * mf.vehicle.antennaPivot);
            mf.pn.fix.northing = northing + (Math.Cos(heading) * mf.vehicle.antennaPivot);

            mf.pn.ConvertLocalToWGS84(mf.pn.fix.northing, mf.pn.fix.easting, out mf.pn.latitude, out mf.pn.longitude);

            mf.pn.speed = Math.Abs(Math.Round(stepDistance * 3.6, 1));
            mf.pn.AverageTheSpeed();

            mf.pn.headingTrue = mf.pn.headingTrueDual = glm.toDegrees(heading);

            mf.sentenceCounter = 0;
            mf.UpdateFixPosition();
        }

        public void resetSim()
        {
            easting = 0;
            northing = 0;
            heading = 0;
            
            mf.pn.latitude = mf.pn.latStart;
            mf.pn.longitude = mf.pn.lonStart;
        }
    }
}