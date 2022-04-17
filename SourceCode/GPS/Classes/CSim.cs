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

            bool VehicleGPS = true;
            bool ToolGPS = mf.tool.isSteering;

            if (VehicleGPS)
            {
                mf.vehicleGPSWatchdog = 0;
                if (mf.toolGPSWatchdog < 20) mf.toolGPSWatchdog++;

                mf.mc.fix.easting = easting + (Math.Sin(heading) * mf.vehicle.antennaPivot);
                mf.mc.fix.northing = northing + (Math.Cos(heading) * mf.vehicle.antennaPivot);

                mf.worldManager.ConvertLocalToWGS84(mf.mc.fix.northing, mf.mc.fix.easting, out mf.mc.latitude, out mf.mc.longitude);

                mf.mc.headingTrue = mf.mc.headingTrueDual = glm.toDegrees(heading);
                if (mf.mc.isDualAsIMU) mf.mc.headingTrueDual = double.MaxValue;
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
                        mf.mc.fixTool.easting = easting + (Math.Sin(heading) * (mf.tool.hitchLength + mf.tool.toolTrailingHitchLength)) + (Math.Cos(heading) * shiftpos);
                        mf.mc.fixTool.northing = northing + (Math.Cos(heading) * (mf.tool.hitchLength + mf.tool.toolTrailingHitchLength)) - (Math.Sin(heading) * shiftpos);
                    }
                    else
                    {
                        mf.mc.fixTool.easting = easting + (Math.Sin(heading) * mf.tool.hitchLength) + (Math.Cos(heading) * shiftpos);
                        mf.mc.fixTool.northing = northing + (Math.Cos(heading) * mf.tool.hitchLength) - (Math.Sin(heading) * shiftpos);
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
            
            mf.mc.latitude = mf.worldManager.latStart;
            mf.mc.longitude = mf.worldManager.lonStart;
        }
    }
}