using OpenTK.Graphics.OpenGL;

namespace AgOpenGPS
{
    public class CCamera
    {
        public double camPitch;
        public double camSetDistance = -75;
        public double gridZoom;
        public double zoomValue = 15;
        public bool camFollowing;
        public double camSmoothFactor;

        //private double camDelta = 0;

        public CCamera()
        {
            //get the pitch of camera from settings
            camPitch = Properties.Settings.Default.setDisplay_camPitch;
            zoomValue = Properties.Settings.Default.setDisplay_camZoom;
            camFollowing = true;
            camSmoothFactor = (Properties.Settings.Default.setDisplay_camSmooth * 0.004) + 0.2;
        }

        public void SetWorldCam(double camPosX, double camPosY, double camYaw)
        {
            //back the camera up
            GL.Translate(0.0, 0.0, camSetDistance * 0.5);

            //rotate the camera down to look at fix
            GL.Rotate(camPitch, 1.0, 0.0, 0.0);

            //following game style or N fixed cam
            if (camFollowing)
                GL.Rotate(camYaw, 0.0, 0.0, 1.0);

            GL.Translate(-camPosX, -camPosY, 0.0);
        }
    }
}