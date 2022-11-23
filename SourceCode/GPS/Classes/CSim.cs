using System;

namespace AgOpenGPS
{
    public class CSim
    {
        private readonly FormGPS mf;

        #region properties sim
        private double northing, easting, heading, sinH, cosH = 1.0;
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

            easting += sinH * stepDistance / mf.HzTime;
            northing += cosH * stepDistance / mf.HzTime;

            vec2 frontWheel;
            frontWheel.easting = (sinH * mf.vehicle.wheelbase) + Math.Sin(heading + glm.toRadians(steerAngle)) * stepDistance / mf.HzTime;
            frontWheel.northing = (cosH * mf.vehicle.wheelbase) + Math.Cos(heading + glm.toRadians(steerAngle)) * stepDistance / mf.HzTime;

            heading = Math.Atan2(frontWheel.easting, frontWheel.northing);

            sinH = Math.Sin(heading);
            cosH = Math.Cos(heading);

            bool VehicleGPS = true;
            bool ToolGPS = mf.tool.isSteering;

            if (VehicleGPS)
            {
                mf.vehicleGPSWatchdog = 0;
                if (mf.toolGPSWatchdog < 20) mf.toolGPSWatchdog++;

                mf.mc.fix.easting = easting + (sinH * mf.vehicle.antennaPivot);
                mf.mc.fix.northing = northing + (cosH * mf.vehicle.antennaPivot);

                mf.worldManager.ConvertLocalToWGS84(mf.mc.fix.northing, mf.mc.fix.easting, out mf.mc.latitude, out mf.mc.longitude);

                mf.mc.headingTrue = mf.mc.headingTrueDual = glm.toDegrees(heading);
                if (mf.mc.isDualAsIMU)
                {
                    mf.mc.imuHeading = mf.mc.headingTrueDual;
                    mf.mc.headingTrueDual = double.MaxValue;
                }
            }

            if (ToolGPS)
            {
                mf.toolGPSWatchdog = 0;
                if (mf.vehicleGPSWatchdog < 20) mf.vehicleGPSWatchdog++;

                double shiftpos = steerAngleScrollBar * 0.1;

                if (mf.vehicleGPSWatchdog < 11)
                {
                    if (mf.tool.isToolTrailing)
                    {
                        mf.mc.fixTool.easting = easting + (sinH * mf.tool.hitchLength) + (Math.Sin(heading - glm.toRadians(steerAngleScrollBar)) * mf.tool.TrailingAxleLength);
                        mf.mc.fixTool.northing = northing + (cosH * mf.tool.hitchLength) + (Math.Cos(heading - glm.toRadians(steerAngleScrollBar)) * mf.tool.TrailingAxleLength);
                    }
                    else
                    {
                        mf.mc.fixTool.easting = easting + (sinH * mf.tool.hitchLength) + (cosH * shiftpos);
                        mf.mc.fixTool.northing = northing + (cosH * mf.tool.hitchLength) - (sinH * shiftpos);
                    }
                }
                else
                {
                    mf.mc.fixTool.easting = easting;
                    mf.mc.fixTool.northing = northing;
                }
            
                mf.mc.headingTrueDualTool = glm.toDegrees(heading);
                mf.worldManager.ConvertLocalToWGS84(mf.mc.fixTool.northing, mf.mc.fixTool.easting, out mf.mc.latitudeTool, out mf.mc.longitudeTool);
            }

            mf.sentenceCounter = 0;
            mf.UpdateFixPosition();
        }

        public void resetSim()
        {
            easting = 0;
            northing = 0;
            heading = 0;
            sinH = 0.0;
            cosH = 1.0;

            mf.mc.latitude = mf.worldManager.latStart;
            mf.mc.longitude = mf.worldManager.lonStart;
        }
    }
}