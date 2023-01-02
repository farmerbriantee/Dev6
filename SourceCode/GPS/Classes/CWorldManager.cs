//Please, if you use this, share the improvements

using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.IO;

namespace AgOpenGPS
{
    public class CWorldManager
    {
        private readonly FormGPS mf;

        //local plane geometry
        public double latStart, lonStart;
        public double mPerDegreeLat, mPerDegreeLon;
        //used to offset the antenna position to compensate for drift
        public vec2 fixOffset = new vec2(0, 0);

        public double camPitch;
        public double camSetDistance = 75;
        public double gridZoom;
        public double zoomValue = 15;
        public bool camFollowing;
        public double camSmoothFactor;

        public double northingMax;
        public double eastingMaxGeo;

        public double northingMin;
        public double eastingMinGeo;

        public double eastingMax;
        public double northingMaxGeo;

        public double eastingMin;
        public double northingMinGeo;

        public double GridSize = 20000;
        public double Count = 40;
        public bool isGridOn;
        public bool isGeoMap;
        public double camHeading = 0.0, smoothCamHeading = 0;

        public CWorldManager(FormGPS _f)
        {
            mf = _f;

            //get the pitch of camera from settings
            camPitch = Properties.Settings.Default.setDisplay_camPitch;
            zoomValue = Properties.Settings.Default.setDisplay_camZoom;
            camFollowing = true;
            camSmoothFactor = (Properties.Settings.Default.setDisplay_camSmooth * 0.004) + 0.2;
            latStart = 0;
            lonStart = 0;
        }

        public void SetLocalMetersPerDegree()
        {
            double LatRad = glm.toRadians(latStart);

            mPerDegreeLat = 111132.92 - 559.82 * Math.Cos(2.0 * LatRad) + 1.175 * Math.Cos(4.0 * LatRad) - 0.0023 * Math.Cos(6.0 * LatRad);
            mPerDegreeLon = 111412.84 * Math.Cos(LatRad) - 93.5 * Math.Cos(3.0 * LatRad) + 0.118 * Math.Cos(5.0 * LatRad);

            ConvertWGS84ToLocal(mf.mc.latitude, mf.mc.longitude, out double northing, out double easting);
            mf.worldManager.checkZoomWorldGrid(northing, easting);

            if (mf.isDriveIn && mf.isJobStarted)
            {
                mf.LoadDriveInFields();
            }
        }
        
        public void ConvertWGS84ToLocal(double Lat, double Lon, out double Northing, out double Easting)
        {
            double LatRad = glm.toRadians(Lat);
            mPerDegreeLon = 111412.84 * Math.Cos(LatRad) - 93.5 * Math.Cos(3.0 * LatRad) + 0.118 * Math.Cos(5.0 * LatRad);

            Northing = (Lat - latStart) * mPerDegreeLat;
            Easting = (Lon - lonStart) * mPerDegreeLon;
        }

        public void ConvertLocalToLocal(double Northing, double Easting, double latStart2, double lonStart2, double mPerDegreeLat1, out double Northing2, out double Easting2)
        {
            double Lat = (Northing / mPerDegreeLat1) + latStart2;
            double LatRad = Lat * 0.01745329251994329576923690766743;

            mPerDegreeLon = 111412.84 * Math.Cos(LatRad) - 93.5 * Math.Cos(3.0 * LatRad) + 0.118 * Math.Cos(5.0 * LatRad);
            double Lon = (Easting / mPerDegreeLon) + lonStart2;

            Northing2 = (Lat - latStart) * mPerDegreeLat;
            Easting2 = (Lon - lonStart) * mPerDegreeLon;
        }

        public void ConvertLocalToWGS84(double Northing, double Easting, out double Lat, out double Lon)
        {
            Lat = ((Northing + fixOffset.northing) / mPerDegreeLat) + latStart;
            double LatRad = glm.toRadians(Lat);

            mPerDegreeLon = 111412.84 * Math.Cos(LatRad) - 93.5 * Math.Cos(3.0 * LatRad) + 0.118 * Math.Cos(5.0 * LatRad);
            Lon = ((Easting + fixOffset.easting) / mPerDegreeLon) + lonStart;
        }

        public string GetLocalToWSG84_KML(double Easting, double Northing)
        {
            double Lat = (Northing / mPerDegreeLat) + latStart;
            double LatRad = glm.toRadians(Lat);

            mPerDegreeLon = 111412.84 * Math.Cos(LatRad) - 93.5 * Math.Cos(3.0 * LatRad) + 0.118 * Math.Cos(5.0 * LatRad);
            double Lon = (Easting / mPerDegreeLon) + lonStart;

            return Lon.ToString("0.0000000") + ',' + Lat.ToString("0.0000000") + ",0 ";
        }

        public void SetWorldPerspective(double camPosX, double camPosY)
        {
            //back the camera up
            GL.Translate(0.0, 0.0, camSetDistance * -0.5);

            //rotate the camera down to look at fix
            GL.Rotate(camPitch, 1.0, 0.0, 0.0);

            //following game style or N fixed cam
            if (camFollowing)
                GL.Rotate(camHeading, 0.0, 0.0, 1.0);

            GL.Translate(-camPosX, -camPosY, 0.0);

            DrawFieldSurface();
        }

        public void SmoothCam(double newHeading)
        {
            double distance = (newHeading - smoothCamHeading) % glm.twoPI;
            if (distance < -glm.PIBy2) distance += glm.twoPI;
            if (distance > glm.PIBy2) distance -= glm.twoPI;

            smoothCamHeading += distance * camSmoothFactor;

            if (smoothCamHeading > glm.twoPI) smoothCamHeading -= glm.twoPI;
            else if (smoothCamHeading < 0) smoothCamHeading += glm.twoPI;

            camHeading = glm.toDegrees(smoothCamHeading);
        }

        public void DrawFieldSurface()
        {
            Color field = mf.isDay ? mf.fieldColorDay : mf.fieldColorNight;

            if (mf.isTextureOn)
            {
                GL.Enable(EnableCap.Texture2D);
                //adjust bitmap zoom based on cam zoom

                Count = 5120 / gridZoom;

                GL.Color3(field.R, field.G, field.B);
                GL.BindTexture(TextureTarget.Texture2D, mf.texture[1]);
                GL.Begin(PrimitiveType.TriangleStrip);
                
                GL.TexCoord2(0, 0);
                GL.Vertex3(eastingMin, northingMax, 0.0);
                GL.TexCoord2(Count, 0.0);
                GL.Vertex3(eastingMax, northingMax, 0.0);
                GL.TexCoord2(0.0, Count);
                GL.Vertex3(eastingMin, northingMin, 0.0);
                GL.TexCoord2(Count, Count);
                GL.Vertex3(eastingMax, northingMin, 0.0);

                GL.End();
                GL.Disable(EnableCap.Texture2D);
            }
            else
            {
                GL.Color3(field.R, field.G, field.B);
                GL.Begin(PrimitiveType.TriangleStrip);
                GL.Vertex3(eastingMin, northingMax, 0.0);
                GL.Vertex3(eastingMax, northingMax, 0.0);
                GL.Vertex3(eastingMin, northingMin, 0.0);
                GL.Vertex3(eastingMax, northingMin, 0.0);
                GL.End();
            }

            if (isGeoMap && zoomValue > 15)
            {
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, mf.texture[20]);
                GL.Begin(PrimitiveType.TriangleStrip);
                GL.Color3(0.6f, 0.6f, 0.6f);
                GL.TexCoord2(0, 0);
                GL.Vertex3(eastingMinGeo, northingMaxGeo, 0.0);
                GL.TexCoord2(1, 0.0);
                GL.Vertex3(eastingMaxGeo, northingMaxGeo, 0.0);
                GL.TexCoord2(0.0, 1);
                GL.Vertex3(eastingMinGeo, northingMinGeo, 0.0);
                GL.TexCoord2(1, 1);
                GL.Vertex3(eastingMaxGeo, northingMinGeo, 0.0);
                GL.End();
                GL.Disable(EnableCap.Texture2D);
            }

            ////if grid is on draw it
            if (isGridOn) DrawWorldGrid();
        }

        public void DrawWorldGrid()
        {
            if (mf.isDay)
            {
                GL.Color3(0.5, 0.5, 0.5);
            }
            else
            {
                GL.Color3(0.17, 0.17, 0.17);
            }
            GL.LineWidth(1);
            GL.Begin(PrimitiveType.Lines);
            for (double num = Math.Round(eastingMin / gridZoom, MidpointRounding.AwayFromZero) * gridZoom; num < eastingMax; num += gridZoom)
            {
                if (num < eastingMin) continue;
                else if (mf.eastMin > num)
                    continue;
                else if (mf.eastMax < num)
                    break;

                GL.Vertex3(num, northingMax, 0.0);
                GL.Vertex3(num, northingMin, 0.0);
            }
            for (double num2 = Math.Round(northingMin / gridZoom, MidpointRounding.AwayFromZero) * gridZoom; num2 < northingMax; num2 += gridZoom)
            {
                if (num2 < northingMin) continue;
                else if (mf.northMin > num2)
                    continue;
                else if (mf.northMax < num2)
                    break;

                GL.Vertex3(eastingMax, num2, 0.0);
                GL.Vertex3(eastingMin, num2, 0.0);
            }
            GL.End();
        }

        public void checkZoomWorldGrid(double northing, double easting)
        {
            double multiply = GridSize / Count * 2;

            double n = Math.Round(northing / multiply, MidpointRounding.AwayFromZero) * multiply;
            double e = Math.Round(easting / multiply, MidpointRounding.AwayFromZero) * multiply;

            northingMax = n + GridSize;
            northingMin = n - GridSize;
            eastingMax = e + GridSize;
            eastingMin = e - GridSize;
        }
    }
}