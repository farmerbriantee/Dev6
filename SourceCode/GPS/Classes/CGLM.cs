using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public static class glm
    {
        public static bool isMetric = true, isKeyboardOn = true, isSimEnabled = false;
        public static double mToUser = 100.0, userToM = 0.01, mToUserBig = 1.0, userBigToM = 1.0, m2ToUser = 0.0001, KMHToUser = 1.0, userToKMH = 1.0;

        public static string unitsFtM, unitsInCm, unitsHaAc;

        public static void SetUserScales(bool _metric)
        {
            isMetric = _metric;
            if (isMetric)
            {
                userToM = 0.01;//cm to m
                mToUser = 100.0;//m to cm

                mToUserBig = 1.0;//m to m
                userBigToM = 1.0;//m to m
                KMHToUser = 1.0;//Km/H to Km/H
                userToKMH = 1.0;//Km/H to Km/H

                m2ToUser = 0.0001;//m2 to Ha

                unitsFtM = " m";
                unitsInCm = " cm";
                unitsHaAc = " Ha";
            }
            else
            {
                userToM = 0.0254;//inches to meters
                mToUser = 39.3701;//meters to inches

                mToUserBig = 3.28084;//meters to feet
                userBigToM = 0.3048;//feet to meters
                KMHToUser = 0.62137;//Km/H to mph
                userToKMH = 1.60934;//mph to Km/H

                m2ToUser = 0.000247105;//m2 to Acres

                unitsFtM = " ft";
                unitsInCm = " in";
                unitsHaAc = " Ac";
            }
        }

        public static bool KeypadToNUD(this NumericUpDown sender)
        {
            sender.Value = Math.Round(sender.Value, sender.DecimalPlaces);

            using (FormNumeric form = new FormNumeric((double)sender.Minimum, (double)sender.Maximum, (double)sender.Value, 2))
            {
                DialogResult result = form.ShowDialog(sender);
                if (result == DialogResult.OK)
                {
                    sender.Value = (decimal)form.ReturnValue;
                    return true;
                }
                return false;
            }
        }

        public static bool KeyboardToText(this TextBox sender)
        {
            if (isKeyboardOn)
            {
                using (FormKeyboard form = new FormKeyboard(sender.Text))
                {
                    if (form.ShowDialog(sender) == DialogResult.OK)
                    {
                        sender.Text = form.ReturnString;
                        return true;
                    }
                }
            }
            return false;
        }
        
        public static bool KeypadToButton(this Button sender, ref double value, double minimum, double maximum, int decimals, double Mtr2Unit = 1.0, double unit2meter = 1.0, decimal divisible = -1)
        {
            using (FormNumeric form = new FormNumeric(minimum, maximum, value, decimals, true, unit2meter, Mtr2Unit, divisible))
            {
                if (form.ShowDialog(sender) == DialogResult.OK)
                {
                    value = form.ReturnValue;

                    string guifix = "0";
                    if (decimals > 0)
                    {
                        guifix = "0.0";
                        while (--decimals > 0)
                            guifix += "#";
                    }

                    sender.Text = (value * Mtr2Unit).ToString(guifix);
                    return true;
                }
            }
            return false;
        }

        public static bool KeypadToButton(this Button sender, ref int value, int minimum, int maximum, double Mtr2Unit = 1.0, double unit2meter = 1.0, decimal divisible = -1)
        {
            using (FormNumeric form = new FormNumeric(minimum, maximum, value, 0, true, unit2meter, Mtr2Unit, divisible))
            {
                if (form.ShowDialog(sender) == DialogResult.OK)
                {
                    value = (int)form.ReturnValue;
                    sender.Text = (value * Mtr2Unit).ToString("0");
                    return true;
                }
            }
            return false;
        }

        //message box pops up with info then goes away
        public static void TimedMessageBox(this Control control, int timeout, string s1, string s2)
        {
            FormTimedMessage form = new FormTimedMessage(timeout, s1, s2);
            form.Show(control);
        }

        public static bool IsPointInPolygon(this List<vec2> polygon, vec2 testPoint)
        {
            bool result = false;
            int j = polygon.Count - 1;
            for (int i = 0; i < polygon.Count; i++)
            {
                if ((polygon[i].easting < testPoint.easting && polygon[j].easting >= testPoint.easting)
                    || (polygon[j].easting < testPoint.easting && polygon[i].easting >= testPoint.easting))
                {
                    if (polygon[i].northing + (testPoint.easting - polygon[i].easting)
                        / (polygon[j].easting - polygon[i].easting) * (polygon[j].northing - polygon[i].northing)
                        < testPoint.northing)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }

        // Catmull Rom interpoint spline calculation
        public static vec2 Catmull(double t, vec2 p0, vec2 p1, vec2 p2, vec2 p3)
        {
            double tt = t * t;
            double ttt = tt * t;

            double q1 = -ttt + 2.0f * tt - t;
            double q2 = 3.0f * ttt - 5.0f * tt + 2.0f;
            double q3 = -3.0f * ttt + 4.0f * tt + t;
            double q4 = ttt - tt;

            //Catmull Rom gradient calculation
            //double q1 = -3.0f * tt + 4.0f * t - 1;
            //double q2 = 9.0f * tt - 10.0f * t;
            //double q3 = -9.0f * tt + 8.0f * t + 1.0f;
            //double q4 = 3.0f * tt - 2.0f * t;

            double tx = 0.5f * (p0.easting * q1 + p1.easting * q2 + p2.easting * q3 + p3.easting * q4);
            double ty = 0.5f * (p0.northing * q1 + p1.northing * q2 + p2.northing * q3 + p3.northing * q4);

            vec2 ret = new vec2(tx, ty);
            return ret;
        }

        //Regex file expression
        public const string fileRegex = "(^(PRN|AUX|NUL|CON|COM[1-9]|LPT[1-9]|(\\.+)$)(\\..*)?$)|(([\\x00-\\x1f\\\\?*:\";‌​|/<>])+)|([\\.]+)";
        
        //the pi's
        public const double twoPI = 6.28318530717958647692;

        public const double PIBy2 = 1.57079632679489661923;

        //Degrees Radians Conversions
        public static double toDegrees(double radians)
        {
            return radians * 57.295779513082325225835265587528;
        }

        public static double toRadians(double degrees)
        {
            return degrees * 0.01745329251994329576923690768489;
        }

        public static double Distance(vec2 first, vec2 second)
        {
            return Math.Sqrt(
                Math.Pow(first.easting - second.easting, 2)
                + Math.Pow(first.northing - second.northing, 2));
        }

        public static double Distance(vec3 first, vec2 second)
        {
            return Math.Sqrt(
                Math.Pow(first.easting - second.easting, 2)
                + Math.Pow(first.northing - second.northing, 2));
        }

        public static double Distance(vec2 first, double east, double north)
        {
            return Math.Sqrt(
                Math.Pow(first.easting - east, 2)
                + Math.Pow(first.northing - north, 2));
        }

        public static double Distance(vecFix2Fix first, vec2 second)
        {
            return Math.Sqrt(
                Math.Pow(first.easting - second.easting, 2)
                + Math.Pow(first.northing - second.northing, 2));
        }

        public static double Distance(vecFix2Fix first, vecFix2Fix second)
        {
            return Math.Sqrt(
                Math.Pow(first.easting - second.easting, 2)
                + Math.Pow(first.northing - second.northing, 2));
        }

        public static double DistanceSquared(vec2 first, vec2 second)
        {
            return (
            Math.Pow(first.easting - second.easting, 2)
            + Math.Pow(first.northing - second.northing, 2));
        }

        public static Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);
            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);
            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
              {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
              });
            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();
            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);
            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }
    }
}