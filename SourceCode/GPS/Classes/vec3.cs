using System;

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

        public static vec3 operator -(vec3 lhs, vec3 rhs)
        {
            return new vec3(lhs.easting - rhs.easting, lhs.northing - rhs.northing, 0);
        }

        public static vec3 operator +(vec3 lhs, vec3 rhs)
        {
            return new vec3(lhs.easting + rhs.easting, lhs.northing + rhs.northing, 0);
        }

        public static vec3 operator *(vec3 self, double s)
        {
            return new vec3(self.easting * s, self.northing * s, 0);
        }
    }

    public struct vecCrossing
    {
        public double easting;
        public double northing;
        public int crosssingIdx;
        public int turnLineIdx;
        public double time;
        public int boundaryIdx;

        public vecCrossing(double _easting, double _northing, double _time, int _boundaryIdx, int _crosssingIdx, int _turnLineIdx)
        {
            northing = _northing;
            easting = _easting;
            time = _time;
            boundaryIdx = _boundaryIdx;
            crosssingIdx = _crosssingIdx;
            turnLineIdx = _turnLineIdx;
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

        public static vec2 operator /(vec2 self, double s)
        {
            return new vec2(self.easting / s, self.northing / s);
        }

        //add 2 vectors
        public static vec2 operator +(vec2 lhs, vec2 rhs)
        {
            return new vec2(lhs.easting + rhs.easting, lhs.northing + rhs.northing);
        }
    }

    public class CFlag
    {
        //WGS84 Lat Long
        public double latitude = 0;

        public double longitude = 0;

        //UTM coordinates
        public double northing = 0;

        public double easting = 0, heading = 0;

        //color of the flag - 0 is red, 1 is green, 2 is purple
        public int color = 0;

        public string notes = "";

        //constructor
        public CFlag(double _lati, double _longi, double _easting, double _northing, double _heading, int _color, string _notes = "Notes")
        {
            latitude = Math.Round(_lati, 7);
            longitude = Math.Round(_longi, 7);
            easting = Math.Round(_easting, 7);
            northing = Math.Round(_northing, 7);
            heading = Math.Round(_heading, 7);
            color = _color;
            notes = _notes;
        }
    }
}