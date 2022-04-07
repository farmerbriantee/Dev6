//Please, if you use this, share the improvements

using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace AgOpenGPS
{
    public class CWorldManager
    {
        private readonly FormGPS mf;

        public double camPitch;
        public double camSetDistance = -75;
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
        }

        public void SetWorldPerspective(double camPosX, double camPosY)
        {
            //back the camera up
            GL.Translate(0.0, 0.0, camSetDistance * 0.5);

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

            GL.Enable(EnableCap.Texture2D);

            if (mf.isTextureOn)
            {
                //adjust bitmap zoom based on cam zoom
                if (zoomValue > 100) Count = 4;
                else if (zoomValue > 80) Count = 8;
                else if (zoomValue > 50) Count = 16;
                else if (zoomValue > 20) Count = 32;
                else if (zoomValue > 10) Count = 64;
                else Count = 128;

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
            }

            GL.Disable(EnableCap.Texture2D);
            ////if grid is on draw it
            if (isGridOn) DrawWorldGrid(gridZoom);
        }

        public void DrawWorldGrid(double _gridZoom)
        {
            _gridZoom *= 0.5;

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
            for (double num = Math.Round(eastingMin / _gridZoom, MidpointRounding.AwayFromZero) * _gridZoom; num < eastingMax; num += _gridZoom)
            {
                if (num < eastingMin) continue;

                GL.Vertex3(num, northingMax, 0.1);
                GL.Vertex3(num, northingMin, 0.1);
            }
            for (double num2 = Math.Round(northingMin / _gridZoom, MidpointRounding.AwayFromZero) * _gridZoom; num2 < northingMax; num2 += _gridZoom)
            {
                if (num2 < northingMin) continue;

                GL.Vertex3(eastingMax, num2, 0.1);
                GL.Vertex3(eastingMin, num2, 0.1);
            }
            GL.End();
        }

        public void checkZoomWorldGrid(double northing, double easting)
        {
            double n = Math.Round(northing / (GridSize / Count * 2), MidpointRounding.AwayFromZero) * (GridSize / Count * 2);
            double e = Math.Round(easting / (GridSize / Count * 2), MidpointRounding.AwayFromZero) * (GridSize / Count * 2);

            northingMax = n + GridSize;
            northingMin = n - GridSize;
            eastingMax = e + GridSize;
            eastingMin = e - GridSize;
        }
    }
}