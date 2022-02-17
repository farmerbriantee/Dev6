﻿//Please, if you use this, share the improvements

using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public struct vec3
    {
        public double easting;
        public double northing;
        public double heading;

        public vec3(double easting, double northing, double heading)
        {
            this.easting = easting;
            this.northing = northing;
            this.heading = heading;
        }

        public vec3(vec3 v)
        {
            easting = v.easting;
            northing = v.northing;
            heading = v.heading;
        }

        public double HeadingXZ()
        {
            return Math.Atan2(easting, northing);
        }

        public void Normalize()
        {
            double length = GetLength();
            if (Math.Abs(length) < 0.0000000000001)
            {
                throw new DivideByZeroException("Trying to normalize a vector with length of zero.");
            }

            easting /= length;
            northing /= length;
            heading /= length;
        }

        //Returns the length of the vector
        public double GetLength()
        {
            return Math.Sqrt((easting * easting) + (heading * heading) + (northing * northing));
        }

        // Calculates the squared length of the vector.
        public double GetLengthSquared()
        {
            return (easting * easting) + (heading * heading) + (northing * northing);
        }

        public static vec3 operator -(vec3 lhs, vec3 rhs)
        {
            return new vec3(lhs.easting - rhs.easting, lhs.northing - rhs.northing, lhs.heading - rhs.heading);
        }
    }

    public class CRecPathPt
    {
        public double easting { get; set; }
        public double northing { get; set; }
        public double heading { get; set; }
        public double speed { get; set; }
        public bool autoBtnState { get; set; }

        //constructor
        public CRecPathPt(double _easting, double _northing, double _heading, double _speed,
                            bool _autoBtnState)
        {
            easting = _easting;
            northing = _northing;
            heading = _heading;
            speed = _speed;
            autoBtnState = _autoBtnState;
        }
    }

    public struct vecFix2Fix
    {
        public double easting; //easting
        public double distance; //distance since last point
        public double northing; //norting
        public int isSet;    //altitude

        public vecFix2Fix(double _easting, double _northing, double _distance, int _isSet)
        {
            this.easting = _easting;
            this.distance = _distance;
            this.northing = _northing;
            this.isSet = _isSet;
        }
    }


    //

    /// <summary>
    /// easting, northing, heading, boundary#
    /// </summary>
    public struct vec4
    {
        public double easting; //easting
        public double heading; //heading etc
        public double northing; //northing
        public int index;    //altitude

        public vec4(double _easting, double _northing, double _heading, int _index)
        {
            this.easting = _easting;
            this.heading = _heading;
            this.northing = _northing;
            this.index = _index;
        }
    }

    public struct vec2
    {
        public double easting; //easting
        public double northing; //northing

        public vec2(double easting, double northing)
        {
            this.easting = easting;
            this.northing = northing;
        }

        public static vec2 operator -(vec2 lhs, vec2 rhs)
        {
            return new vec2(lhs.easting - rhs.easting, lhs.northing - rhs.northing);
        }

        //public static bool operator ==(vec2 lhs, vec2 rhs)
        //{
        //    return (lhs.x == rhs.x && lhs.z == rhs.z);
        //}

        //public static bool operator !=(vec2 lhs, vec2 rhs)
        //{
        //    return (lhs.x != rhs.x && lhs.z != rhs.z);
        //}

        //calculate the heading of dirction pointx to pointz
        public double HeadingXZ()
        {
            return Math.Atan2(easting, northing);
        }

        //normalize to 1
        public vec2 Normalize()
        {
            double length = GetLength();
            if (Math.Abs(length) < 0.000000000001)
                return new vec2();
            else
                return new vec2(easting /= length, northing /= length);
        }

        public double Cross(vec2 v2)
        {
            return northing * v2.easting - easting * v2.northing;
        }

        //Returns the length of the vector
        public double GetLength()
        {
            return Math.Sqrt((easting * easting) + (northing * northing));
        }

        // Calculates the squared length of the vector.
        public double GetLengthSquared()
        {
            return (easting * easting) + (northing * northing);
        }

        //scalar double
        public static vec2 operator *(vec2 self, double s)
        {
            return new vec2(self.easting * s, self.northing * s);
        }

        //add 2 vectors
        public static vec2 operator +(vec2 lhs, vec2 rhs)
        {
            return new vec2(lhs.easting + rhs.easting, lhs.northing + rhs.northing);
        }
    }
}

//public double this[int index]
//{
//    get
//    {
//        if (index == 0) return x;
//        else if (index == 1) return z;
//        else throw new Exception("Out of range.");
//    }
//    set
//    {
//        if (index == 0) x = value;
//        else if (index == 1) z = value;
//        else throw new Exception("Out of range.");
//    }
//}

//public vec2(double s)
//{
//    x = z = s;
//}

//public vec2(vec2 v)
//{
//    this.x = v.x;
//    this.z = v.z;
//}

//public vec2(vec3 v)
//{
//    this.x = v.x;
//    this.z = v.z;
//}

//public static vec2 operator +(vec2 lhs, vec2 rhs)
//{
//    return new vec2(lhs.x + rhs.x, lhs.z + rhs.z);
//}

//public static vec2 operator +(vec2 lhs, double rhs)
//{
//    return new vec2(lhs.x + rhs, lhs.z + rhs);
//}

//public static vec2 operator -(vec2 lhs, double rhs)
//{
//    return new vec2(lhs.x - rhs, lhs.z - rhs);
//}

//public static vec2 operator *(vec2 self, double s)
//{
//    return new vec2(self.x * s, self.z * s);
//}

//public static vec2 operator *(double lhs, vec2 rhs)
//{
//    return new vec2(rhs.x * lhs, rhs.z * lhs);
//}

//public static vec2 operator *(vec2 lhs, vec2 rhs)
//{
//    return new vec2(rhs.x * lhs.x, rhs.z * lhs.z);
//}

//public static vec2 operator /(vec2 lhs, double rhs)
//{
//    return new vec2(lhs.x / rhs, lhs.z / rhs);
//}

//public double[] to_array()
//{
//    return new[] { x, z };
//}