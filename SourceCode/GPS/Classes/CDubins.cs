using System;
using System.Collections.Generic;

/* Special thanks to erik.nordeus@gmail.com for his core dubins code originally written
 * in Unity. I converted this to work as a class in C# from that Unity C Script
 *  http://www.habrador.com/about/
 */

namespace AgOpenGPS
{
    //To keep track of the different paths when debugging
    public enum PathType { LSL = 37, RSL = 38, LRL = 49, LSR = 69, RSR = 70, RLR = 74 }// L = 1 R = 2     | S = 4, L = 8 R = 16    | L = 32, R = 64

    //Display the final Dubins Paths
    public class CDubins
    {
        //How far we are driving each update, the accuracy will improve if we lower the driveDistance
        public static readonly double driveDistance = 0.05;

        //The radius the car can turn 360 degrees with
        public static double turningRadius;

        //Position, Heading is in radians
        private vec2 startPos, goalPos;

        private double startHeading, goalHeading;

        private List<OneDubinsPath> pathDataList = new List<OneDubinsPath>();


        public CDubins(double _turningRadius)
        {
            turningRadius = _turningRadius;
        }

        //takes 2 points and headings to create a path - returns list of vec3 points and headings
        public List<vec2> GenerateDubins(vec2 _startPos, double _startHeading, vec2 _goalPos, double _goalHeading)
        {
            //positions and heading
            startPos = _startPos;
            startHeading = _startHeading;

            goalPos = _goalPos;
            goalHeading = _goalHeading;

            //Get all valid Dubins paths
            pathDataList = GetAllDubinsPaths();

            //clear out existing path of vec3 points
            List<vec2> dubinsShortestPathList = new List<vec2>();

            if (pathDataList.Count > 0)
            {
                for (int i = 0; i < pathDataList[0].pathCoordinates.Count; i += 5)
                {
                    dubinsShortestPathList.Add(pathDataList[0].pathCoordinates[i]);
                }
            }
            return dubinsShortestPathList;
        }

        //The 4 different circles we have that sits to the left/right of the start/goal
        private vec2 startLeftCircle, startRightCircle, goalLeftCircle, goalRightCircle;

        //Get all valid Dubins paths sorted from shortest to longest
        private List<OneDubinsPath> GetAllDubinsPaths()
        {
            //Reset the list with all Dubins paths
            pathDataList.Clear();

            //Position the circles that are to the left/right of the cars
            PositionLeftRightCircles();

            //Find the length of each path with tangent coordinates
            CalculateDubinsPathsLengths();

            //If we have paths
            if (pathDataList.Count > 0)
            {
                //Sort the list with paths so the shortest path is first
                pathDataList.Sort((x, y) => x.totalLength.CompareTo(y.totalLength));

                //Generate the final coordinates of the path from tangent points and segment lengths
                GetTotalPath(pathDataList[0]);
            }

            //No paths could be found
            else
            {
                pathDataList.Clear();
            }

            //return either empty or the actual list.
            return pathDataList;
        }

        //Position the left and right circles that are to the left/right of the target and the car
        private void PositionLeftRightCircles()
        {
            //Goal pos
            goalRightCircle = DubinsMath.GetRightCircleCenterPos(goalPos, goalHeading);

            goalLeftCircle = DubinsMath.GetLeftCircleCenterPos(goalPos, goalHeading);

            //Start pos
            startRightCircle = DubinsMath.GetRightCircleCenterPos(startPos, startHeading);

            startLeftCircle = DubinsMath.GetLeftCircleCenterPos(startPos, startHeading);
        }

        //Calculate the path lengths of all Dubins paths by using tangent points
        private void CalculateDubinsPathsLengths()
        {
            //

            //RSR ****RSR and LSL is only working if the circles don't have the same position
            if (startRightCircle.easting != goalRightCircle.easting || startRightCircle.northing != goalRightCircle.northing)
            {
                Get_RSR_Length();
            }

            //LSL
            if (startLeftCircle.easting != goalLeftCircle.easting || startLeftCircle.northing != goalLeftCircle.northing)
            {
                Get_LSL_Length();
            }

            //RSL and LSR is only working of the circles don't intersect
            double comparisonSqr = CDubins.turningRadius * 2f * CDubins.turningRadius * 2f;

            //RSL
            if ((startRightCircle - goalLeftCircle).GetLengthSquared() > comparisonSqr)
            {
                Get_RSL_Length();
            }

            //LSR
            if ((startLeftCircle - goalRightCircle).GetLengthSquared() > comparisonSqr)
            {
                Get_LSR_Length();
            }

            //With the LRL and RLR paths, the distance between the circles have to be less than 4 * r
            comparisonSqr = 4f * CDubins.turningRadius * 4f * CDubins.turningRadius;

            //RLR
            if ((startRightCircle - goalRightCircle).GetLengthSquared() < comparisonSqr)
            {
                Get_RLR_Length();
            }

            //LRL
            if ((startLeftCircle - goalLeftCircle).GetLengthSquared() < comparisonSqr)
            {
                Get_LRL_Length();
            }
        }

        //RSR
        private void Get_RSR_Length()
        {
            //Find both tangent positons
            DubinsMath.LSLorRSR(startRightCircle, goalRightCircle, false, out vec2 startTangent, out vec2 goalTangent);

            //Calculate lengths
            double length1 = DubinsMath.GetArcLength(startRightCircle, startPos, startTangent, false);
            double length2 = (startTangent - goalTangent).GetLength();
            double length3 = DubinsMath.GetArcLength(goalRightCircle, goalTangent, goalPos, false);

            //Save the data
            pathDataList.Add(new OneDubinsPath(length1, length2, length3, startTangent, goalTangent, PathType.RSR));
        }

        //LSL
        private void Get_LSL_Length()
        {
            //Find both tangent positions
            DubinsMath.LSLorRSR(startLeftCircle, goalLeftCircle, true, out vec2 startTangent, out vec2 goalTangent);

            //Calculate lengths
            double length1 = DubinsMath.GetArcLength(startLeftCircle, startPos, startTangent, true);
            double length2 = (startTangent - goalTangent).GetLength();
            double length3 = DubinsMath.GetArcLength(goalLeftCircle, goalTangent, goalPos, true);

            //Save the data
            pathDataList.Add(new OneDubinsPath(length1, length2, length3, startTangent, goalTangent, PathType.LSL));
        }

        //RSL
        private void Get_RSL_Length()
        {
            //Find both tangent positions
            DubinsMath.RSLorLSR(startRightCircle, goalLeftCircle, false, out vec2 startTangent, out vec2 goalTangent);

            //Calculate lengths
            double length1 = DubinsMath.GetArcLength(startRightCircle, startPos, startTangent, false);
            double length2 = (startTangent - goalTangent).GetLength();
            double length3 = DubinsMath.GetArcLength(goalLeftCircle, goalTangent, goalPos, true);

            //Save the data
            pathDataList.Add(new OneDubinsPath(length1, length2, length3, startTangent, goalTangent, PathType.RSL));
        }

        //LSR
        private void Get_LSR_Length()
        {
            //Find both tangent positions
            DubinsMath.RSLorLSR(startLeftCircle, goalRightCircle, true, out vec2 startTangent, out vec2 goalTangent);

            //Calculate lengths
            double length1 = DubinsMath.GetArcLength(startLeftCircle, startPos, startTangent, true);
            double length2 = (startTangent - goalTangent).GetLength();
            double length3 = DubinsMath.GetArcLength(goalRightCircle, goalTangent, goalPos, false);

            //Save the data
            pathDataList.Add(new OneDubinsPath(length1, length2, length3, startTangent, goalTangent, PathType.LSR));
        }

        //RLR - Find both tangent positions and the position of the 3rd circle
        private void Get_RLR_Length()
        {
            DubinsMath.GetRLRorLRLTangents(
                startRightCircle,
                goalRightCircle,
                false,
                out vec2 startTangent,
                out vec2 goalTangent,
                out vec2 middleCircle);

            //Calculate lengths
            double length1 = DubinsMath.GetArcLength(startRightCircle, startPos, startTangent, false);
            double length2 = DubinsMath.GetArcLength(middleCircle, startTangent, goalTangent, true);
            double length3 = DubinsMath.GetArcLength(goalRightCircle, goalTangent, goalPos, false);

            //Save the data
            pathDataList.Add(new OneDubinsPath(length1, length2, length3, startTangent, goalTangent, PathType.RLR));
        }

        //LRL - Find both tangent positions and the position of the 3rd circle
        private void Get_LRL_Length()
        {
            DubinsMath.GetRLRorLRLTangents(
                startLeftCircle,
                goalLeftCircle,
                true,
                out vec2 startTangent,
                out vec2 goalTangent,
                out vec2 middleCircle);

            //Calculate the total length of this path
            double length1 = DubinsMath.GetArcLength(startLeftCircle, startPos, startTangent, true);
            double length2 = DubinsMath.GetArcLength(middleCircle, startTangent, goalTangent, false);
            double length3 = DubinsMath.GetArcLength(goalLeftCircle, goalTangent, goalPos, true);

            //Save the data
            pathDataList.Add(new OneDubinsPath(length1, length2, length3, startTangent, goalTangent, PathType.LRL));
        }

        //Find the coordinates of the entire path from the 2 tangents and length of each segment
        private void GetTotalPath(OneDubinsPath pathData)
        {
            //Store the waypoints of the final path here
            List<vec2> finalPath = new List<vec2>();

            //Start position of the car
            vec2 currentPos = startPos;
            //Start heading of the car
            double theta = startHeading;

            //We always have to add the first position manually
            finalPath.Add(currentPos);

            //How many line segments can we fit into this part of the path

            //First
            int segments = (int)Math.Floor(pathData.length1 / CDubins.driveDistance);

            DubinsMath.AddCoordinatesToPath(
                ref currentPos,
                ref theta,
                finalPath,
                segments,
                true,
                ((int)pathData.pathType & 2) == 2);

            //Second
            segments = (int)Math.Floor(pathData.length2 / CDubins.driveDistance);

            DubinsMath.AddCoordinatesToPath(
                ref currentPos,
                ref theta,
                finalPath,
                segments,
                ((int)pathData.pathType & 24) > 0,
                ((int)pathData.pathType & 16) == 16);

            //Third
            segments = (int)Math.Floor(pathData.length3 / CDubins.driveDistance);

            DubinsMath.AddCoordinatesToPath(
                ref currentPos,
                ref theta,
                finalPath,
                segments,
                true,
                ((int)pathData.pathType & 64) == 64);

            //Add the final goal coordinate
            finalPath.Add(new vec2(goalPos.easting, goalPos.northing));

            //Save the final path in the path data
            pathData.pathCoordinates = finalPath;
        }
    }

    //Takes care of all standardized methods related the generating of Dubins paths
    public static class DubinsMath
    {
        //Calculate center positions of the Right circle
        public static vec2 GetRightCircleCenterPos(vec2 circlePos, double heading)
        {
            //The circle is 90 degrees (pi/2 radians) to the right of the car's heading
            vec2 rightCirclePos = new vec2(circlePos.easting + (CDubins.turningRadius * Math.Sin(heading + glm.PIBy2)),
                circlePos.northing + (CDubins.turningRadius * Math.Cos(heading + glm.PIBy2))
            );
            return rightCirclePos;
        }

        //Calculate center positions of the Left circle
        public static vec2 GetLeftCircleCenterPos(vec2 circlePos, double heading)
        {
            vec2 rightCirclePos = new vec2(0, 0)
            {
                //The circle is 90 degrees (pi/2 radians) to the left of the car's heading
                easting = circlePos.easting + (CDubins.turningRadius * Math.Sin(heading - glm.PIBy2)),
                northing = circlePos.northing + (CDubins.turningRadius * Math.Cos(heading - glm.PIBy2))
            };
            return rightCirclePos;
        }

        //
        // Calculate the start and end positions of the tangent lines
        //

        //Outer tangent (LSL and RSR)
        public static void LSLorRSR(vec2 startCircle, vec2 goalCircle, bool isBottom,
                                        out vec2 startTangent, out vec2 goalTangent)
        {
            //The angle to the first tangent coordinate is always 90 degrees if the both circles have the same radius
            double theta = glm.PIBy2;

            //Need to modify theta if the circles are not on the same height (z)
            theta += Math.Atan2(goalCircle.northing - startCircle.northing, goalCircle.easting - startCircle.easting);

            //Add pi to get the "bottom" coordinate which is on the opposite side (180 degrees = pi)
            if (isBottom) theta += Math.PI;

            //The coordinates of the first tangent points
            double xT1 = startCircle.easting + (CDubins.turningRadius * Math.Cos(theta));
            double zT1 = startCircle.northing + (CDubins.turningRadius * Math.Sin(theta));

            //To get the second coordinate we need a direction
            //This direction is the same as the direction between the center pos of the circles
            vec2 dirVec = goalCircle - startCircle;

            double xT2 = xT1 + dirVec.easting;
            double zT2 = zT1 + dirVec.northing;

            //The final coordinates of the tangent lines
            startTangent = new vec2(xT1, zT1);
            goalTangent = new vec2(xT2, zT2);
        }

        //Inner tangent (RSL and LSR)
        public static void RSLorLSR(
            vec2 startCircle,
            vec2 goalCircle,
            bool isBottom,
            out vec2 startTangent,
            out vec2 goalTangent)
        {
            //Find the distance between the circles
            double D = (startCircle - goalCircle).GetLength();

            //If the circles have the same radius we can use cosine and not the law of cosines
            //to calculate the angle to the first tangent coordinate
            double theta = Math.Acos((2 * CDubins.turningRadius) / D);

            //If the circles is LSR, then the first tangent pos is on the other side of the center line
            if (isBottom) theta *= -1.0;

            //Need to modify theta if the circles are not on the same height
            theta += Math.Atan2(goalCircle.northing - startCircle.northing, goalCircle.easting - startCircle.easting);

            //The coordinates of the first tangent point
            double xT1 = startCircle.easting + (CDubins.turningRadius * Math.Cos(theta));
            double zT1 = startCircle.northing + (CDubins.turningRadius * Math.Sin(theta));

            //To get the second tangent coordinate we need the direction of the tangent
            //To get the direction we move up 2 circle radius and end up at this coordinate
            double xT1_tmp = startCircle.easting + (2.0 * CDubins.turningRadius * Math.Cos(theta));
            double zT1_tmp = startCircle.northing + (2.0 * CDubins.turningRadius * Math.Sin(theta));

            //The direction is between the new coordinate and the center of the target circle
            vec2 dirVec = goalCircle - new vec2(xT1_tmp, zT1_tmp);

            //The coordinates of the second tangent point is the
            double xT2 = xT1 + dirVec.easting;
            double zT2 = zT1 + dirVec.northing;

            //The final coordinates of the tangent lines
            startTangent = new vec2(xT1, zT1);
            goalTangent = new vec2(xT2, zT2);
        }

        //Get the RLR or LRL tangent points
        public static void GetRLRorLRLTangents(
            vec2 startCircle,
            vec2 goalCircle,
            bool isLRL,
            out vec2 startTangent,
            out vec2 goalTangent,
            out vec2 middleCircle)
        {
            //The distance between the circles
            double D = (startCircle - goalCircle).GetLength();

            //The angle between the goal and the new 3rd circle we create with the law of cosines
            double theta = Math.Acos(D / (4f * CDubins.turningRadius));

            //But we need to modify the angle theta if the circles are not on the same line
            vec2 V1 = goalCircle - startCircle;

            //Different depending on if we calculate LRL or RLR
            if (isLRL)
                theta = Math.Atan2(V1.northing, V1.easting) + theta;
            else
                theta = Math.Atan2(V1.northing, V1.easting) - theta;

            //Calculate the position of the third circle
            double x = startCircle.easting + (2 * CDubins.turningRadius * Math.Cos(theta));
            double z = startCircle.northing + (2 * CDubins.turningRadius * Math.Sin(theta));
            middleCircle = new vec2(x, z);

            //Calculate the tangent points
            vec2 V2 = (startCircle - middleCircle).Normalize();
            vec2 V3 = (goalCircle - middleCircle).Normalize();
            V2 *= CDubins.turningRadius;
            V3 *= CDubins.turningRadius;

            startTangent = middleCircle + V2;
            goalTangent = middleCircle + V3;
        }

        //Calculate the length of an circle arc depending on which direction we are driving
        public static double GetArcLength(
            vec2 circleCenterPos,
            vec2 startPos,
            vec2 goalPos,
            bool isLeftCircle)
        {
            vec2 V1 = startPos - circleCenterPos;
            vec2 V2 = goalPos - circleCenterPos;

            double theta = Math.Atan2(V2.northing, V2.easting) - Math.Atan2(V1.northing, V1.easting);
            if (theta < 0f && isLeftCircle) theta += 2.0 * Math.PI;
            else if (theta > 0 && !isLeftCircle) theta -= 2.0 * Math.PI;
            return Math.Abs(theta * CDubins.turningRadius);
        }

        //Loops through segments of a path and add new coordinates to the final path
        public static void AddCoordinatesToPath(
            ref vec2 currentPos,
            ref double theta,
            List<vec2> finalPath,
            int segments,
            bool isTurning,
            bool isTurningRight)
        {

            //Which way are we turning?
            double turnParameter = (CDubins.driveDistance / CDubins.turningRadius) * (isTurningRight ? 1.0 : -1.0);

            for (int i = 0; i <= segments; i++)
            {
                //Update the position of the car
                currentPos.easting += CDubins.driveDistance * Math.Sin(theta);
                currentPos.northing += CDubins.driveDistance * Math.Cos(theta);

                //Don't update the heading if we are driving straight
                if (isTurning)
                {
                    //Update the heading
                    theta += turnParameter;
                }

                //Add the new coordinate to the path
                finalPath.Add(currentPos);
            }
        }
    }

    //Will hold data related to one Dubins path so we can sort them
    public class OneDubinsPath
    {
        //Tthe total length of this path
        public double totalLength;

        //Need the individual path lengths for debugging and to find the final path
        public double length1, length2, length3;

        //The 2 tangent points we need to connect the lines and curves
        public vec2 tangent1, tangent2;

        //The type, such as RSL
        public PathType pathType;

        //The coordinates of the final path
        public List<vec2> pathCoordinates;

        public OneDubinsPath(double length1, double length2, double length3, vec2 tangent1, vec2 tangent2, PathType pathType)
        {
            //Calculate the total length of this path
            this.totalLength = length1 + length2 + length3;

            this.length1 = length1;
            this.length2 = length2;
            this.length3 = length3;

            this.tangent1 = tangent1;
            this.tangent2 = tangent2;

            this.pathType = pathType;
        }
    }
}