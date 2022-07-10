using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public partial class CGuidance
    {
        private readonly FormGPS mf;

        public List<CGuidanceLine> curveArr = new List<CGuidanceLine>();

        public CGuidanceLine currentGuidanceLine, currentABLine, currentCurveLine, currentRecPath, EditGuidanceLine, creatingContour;

        //the list of points to drive on
        public Polyline curList = new Polyline();
        public List<Polyline> sideGuideLines = new List<Polyline>();
        
        public Mode CurrentGMode = Mode.None;

        private int currentLocationIndexA, currentLocationIndexB, backSpacing = 30;

        public int currentPositonIndex, resumeState;

        public bool isValid, isOkToAddDesPoints, isLocked = false;
        public double lastSecond = 0, lastSecondSearch = 0, moveDistance;

        public double abLength;
        public int lineWidth;
        public int tramPassEvery;

        public double distanceFromRefLine;

        public int howManyPathsAway, oldHowManyPathsAway = int.MaxValue;

        //steer, pivot, and ref indexes
        private int sA, sB, pA, pB, rA, rB;

        public bool isSmoothWindowOpen, isHeadingSameWay = true, oldIsHeadingSameWay = true;

        public double distanceFromCurrentLinePivot, distanceFromCurrentLineTool;
        public double steerAngle, rEast, rNorth;

        public vec2 goalPoint = new vec2(0, 0);

        public double inty, steerHeadingError, xTrackSteerCorrection = 0;

        private double pivotDistError, lastPivotDistError, pivotDerivativeDistError;
        public double steerDistError, lastSteerDistError, steerDerivativeDistError;


        //derivative counter
        private int counter2;

        public CGuidance(FormGPS _f)
        {
            //constructor
            mf = _f;

            lineWidth = Properties.Settings.Default.setDisplay_lineWidth;
            abLength = Properties.Settings.Default.setAB_lineLength;

            uturnDistanceFromBoundary = Properties.Vehicle.Default.set_youTurnDistanceFromBoundary;

            //how far before or after boundary line should turn happen
            youTurnStartOffset = Properties.Vehicle.Default.set_youTurnExtensionLength;

            rowSkipsWidth = Properties.Vehicle.Default.set_youSkipWidth;
            Set_Alternate_skips();
        }

        public void CalculateSteerAngle(vec2 pivot, vec2 steer, double heading, Polyline curList)
        {
            bool completeYouTurn = !isYouTurnTriggered;

            if (curList?.points.Count > 1)
            {
                pivot.GetCurrentSegment(curList, out pA, out pB, !isYouTurnTriggered && CurrentGMode == Mode.RecPath ? currentPositonIndex - 1 : 0, !isYouTurnTriggered && CurrentGMode == Mode.RecPath ? currentPositonIndex + 5 : int.MaxValue);

                if (pA < 0 || pB < 0) return;

                //return and reset if too far away or end of the line
                if (pB > curList.points.Count - 2)
                    completeYouTurn = true;

                if (CurrentGMode == Mode.RecPath && mf.isAutoSteerBtnOn && !isYouTurnTriggered)
                {
                    currentPositonIndex = pA;
                    if (pB > curList.points.Count - 2)
                    {
                        //EndOfRecPath
                        mf.sim.stepDistance = 0;
                        return;
                    }
                    if (curList is CGuidanceRecPath RecPath)
                    {
                        mf.sim.stepDistance = RecPath.Status[currentPositonIndex].speed / 3.6;

                        mf.setSectionBtnState(RecPath.Status[currentPositonIndex].autoBtnState);
                    }
                }

                //get the pivot distance from currently active AB segment   ///////////  Pivot  ////////////
                double dx2 = curList.points[pB].easting - curList.points[pA].easting;
                double dy2 = curList.points[pB].northing - curList.points[pA].northing;

                if (Math.Abs(dx2) < double.Epsilon && Math.Abs(dy2) < double.Epsilon) return;

                distanceFromCurrentLinePivot = pivot.FindDistanceToSegment(curList.points[pA], curList.points[pB], out _, true, false, false);

                //should get its own closest segment
                distanceFromCurrentLineTool = ((dy2 * mf.tool.Pos.easting) - (dx2 * mf.tool.Pos.northing) + (curList.points[pB].easting
                            * curList.points[pA].northing) - (curList.points[pB].northing * curList.points[pA].easting))
                                / Math.Sqrt((dy2 * dy2) + (dx2 * dx2));

                double U = (((pivot.easting - curList.points[pA].easting) * dx2)
                                + ((pivot.northing - curList.points[pA].northing) * dy2))
                                / ((dx2 * dx2) + (dy2 * dy2));

                rEast = curList.points[pA].easting + (U * dx2);
                rNorth = curList.points[pA].northing + (U * dy2);

                if (isYouTurnTriggered)
                {
                    onA = 0;
                    for (int k = 0; k < pA; k++)
                    {
                        onA += glm.Distance(curList.points[k], curList.points[k + 1]);
                    }

                    onA += glm.Distance(curList.points[pA], rEast, rNorth);
                }

                currentLocationIndexA = pA;
                currentLocationIndexB = pB;
                double pivotHeading = Math.Atan2(dx2, dy2);

                if (mf.isStanleyUsed && CurrentGMode != Mode.RecPath)
                {
                    #region Stanley

                    steer.GetCurrentSegment(curList, out sA, out sB);

                    if (isYouTurnTriggered)
                    {
                        //feed backward to turn slower to keep pivot on
                        sA -= 7;
                        if (sA < 0)
                        {
                            sA = 0;
                        }
                        sB = sA + 1;

                        //return and reset if too far away or end of the line
                        if (sB >= curList.points.Count - 8)
                        {
                            completeYouTurn = true;
                        }
                    }

                    //get the distance from currently active AB line
                    double dx = curList.points[sB].easting - curList.points[sA].easting;
                    double dy = curList.points[sB].northing - curList.points[sA].northing;

                    if (Math.Abs(dx) < double.Epsilon && Math.Abs(dy) < double.Epsilon) return;

                    //how far from current AB Line is fix
                    double distanceFromCurrentLineSteer = ((dy * steer.easting) - (dx * steer.northing) + (curList.points[sB].easting
                                * curList.points[sA].northing) - (curList.points[sB].northing * curList.points[sA].easting))
                                    / Math.Sqrt((dy * dy) + (dx * dx));


                    if (!isYouTurnTriggered && CurrentGMode == Mode.Curve)
                    {
                        distanceFromCurrentLineSteer -= inty;

                        if (isHeadingSameWay && sB > 0 && sB + 1 < curList.points.Count)
                        {
                            dx = curList.points[sB + 1].easting - curList.points[sB - 1].easting;
                            dy = curList.points[sB + 1].northing - curList.points[sB - 1].northing;
                        }
                        else
                        {
                            dx = curList.points[sA + 1].easting - curList.points[sA - 1].easting;
                            dy = curList.points[sA + 1].northing - curList.points[sA - 1].northing;
                        }
                    }

                    double lineHeading = Math.Atan2(dx, dy);
                    if (lineHeading < 0) lineHeading += glm.twoPI;

                    if (isYouTurnTriggered || isHeadingSameWay)
                    {
                        steerHeadingError = heading - lineHeading;
                    }
                    else
                    {
                        distanceFromCurrentLineSteer *= -1.0;
                        steerHeadingError = heading - lineHeading + Math.PI;
                    }


                    while (steerHeadingError < -Math.PI)
                        steerHeadingError += glm.twoPI;
                    while (steerHeadingError > Math.PI)
                        steerHeadingError -= glm.twoPI;

                    if (mf.isReverse) steerHeadingError *= -1;
                    //Overshoot setting on Stanley tab
                    steerHeadingError *= mf.vehicle.stanleyHeadingErrorGain;

                    if (steerHeadingError > 0.74) steerHeadingError = 0.74;
                    if (steerHeadingError < -0.74) steerHeadingError = -0.74;

                    //the non linear distance error part of stanley
                    double XTEc = Math.Atan((distanceFromCurrentLineSteer * mf.vehicle.stanleyDistanceErrorGain)
                        / ((Math.Abs(mf.mc.avgSpeed) * 0.277777) + 1));

                    //clamp it to max 42 degrees
                    if (XTEc > 0.74) XTEc = 0.74;
                    if (XTEc < -0.74) XTEc = -0.74;

                    xTrackSteerCorrection = (xTrackSteerCorrection * 0.5) + XTEc * 0.5;

                    steerAngle = glm.toDegrees((((!isYouTurnTriggered || CurrentGMode == Mode.Contour) ? XTEc : xTrackSteerCorrection) + steerHeadingError) * -1.0);

                    if (!isYouTurnTriggered && (CurrentGMode == Mode.Curve || CurrentGMode == Mode.AB))
                    {
                        if (Math.Abs(distanceFromCurrentLineSteer) > 0.5) steerAngle *= 0.5;
                        else steerAngle *= (1 - Math.Abs(distanceFromCurrentLineSteer));
                    }

                    //derivative of steer distance error
                    steerDistError = (steerDistError * 0.95) + (xTrackSteerCorrection * 3.0);
                    pivotDistError = (pivotDistError * 0.6) + distanceFromCurrentLinePivot * 0.4;

                    if (counter2++ > 5)
                    {
                        steerDerivativeDistError = steerDistError - lastSteerDistError;
                        lastSteerDistError = steerDistError;
                        pivotDerivativeDistError = pivotDistError - lastPivotDistError;
                        lastPivotDistError = pivotDistError;
                        counter2 = 0;
                    }


                    if (mf.vehicle.stanleyIntegralGainAB != 0 && !mf.isReverse)
                    {
                        if (!isYouTurnTriggered && mf.isAutoSteerBtnOn && mf.mc.avgSpeed > mf.startSpeed && Math.Abs(steerDerivativeDistError) < 1 && Math.Abs(pivotDerivativeDistError) < 0.15)
                        {
                            //if over the line heading wrong way, rapidly decrease integral
                            if ((inty < 0 && distanceFromCurrentLinePivot < 0) || (inty > 0 && distanceFromCurrentLinePivot > 0))
                            {
                                inty += pivotDistError * mf.vehicle.stanleyIntegralGainAB * -0.1;
                            }
                            else
                            {
                                inty += pivotDistError * mf.vehicle.stanleyIntegralGainAB * -0.01;
                            }
                        }
                        else inty *= 0.7;
                    }
                    else inty = 0;

                    if (!isYouTurnTriggered && mf.mc.imuRoll != 88888)
                        steerAngle += mf.mc.imuRoll * -mf.mc.sideHillCompFactor;

                    //add them up and clamp to max in vehicle settings
                    if (steerAngle < -mf.vehicle.maxSteerAngle) steerAngle = -mf.vehicle.maxSteerAngle;
                    if (steerAngle > mf.vehicle.maxSteerAngle) steerAngle = mf.vehicle.maxSteerAngle;
                    #endregion
                }
                else
                {
                    #region Pure Pursuit

                    if (mf.vehicle.purePursuitIntegralGain != 0 && !mf.isReverse)
                    {
                        //integral slider is set to 0
                        pivotDistError = pivotDistError * 0.8 + (distanceFromCurrentLinePivot * 0.2);
                        if (counter2++ > 4)
                        {
                            pivotDerivativeDistError = pivotDistError - lastPivotDistError;
                            lastPivotDistError = pivotDistError;
                            counter2 = 0;
                            pivotDerivativeDistError *= 2;
                        }

                        if (!isYouTurnTriggered && mf.isAutoSteerBtnOn && Math.Abs(pivotDerivativeDistError) < 0.1 && mf.mc.avgSpeed > 2.5)
                        {
                            //if over the line heading wrong way, rapidly decrease integral
                            if ((inty < 0 && distanceFromCurrentLinePivot < 0) || (inty > 0 && distanceFromCurrentLinePivot > 0))
                            {
                                inty += pivotDistError * mf.vehicle.purePursuitIntegralGain * (CurrentGMode == Mode.Contour ? -0.06 : -0.04);
                            }
                            else
                            {
                                if (Math.Abs(distanceFromCurrentLinePivot) > 0.02)
                                {
                                    inty += pivotDistError * mf.vehicle.purePursuitIntegralGain * -0.02;
                                    if (inty > 0.2) inty = 0.2;
                                    else if (inty < -0.2) inty = -0.2;
                                }
                            }
                        }
                        else inty *= 0.95;
                    }
                    else inty = 0;

                    //update base on autosteer settings and distance from line
                    double goalPointDistance = (isYouTurnTriggered ? 0.8 : 1.0) * mf.vehicle.UpdateGoalPointDistance();

                    bool CountUp = mf.isReverse ^ (isYouTurnTriggered || isHeadingSameWay);

                    int count = CountUp ? 1 : -1;
                    vec2 start = new vec2(rEast, rNorth);
                    double distSoFar = 0;

                    bool loop = true;// curList.loop;

                    double dist = goalPointDistance - distSoFar;

                    if (pA == 0 && !curList.loop && !CountUp && U < 0)//extend end of line
                    {
                        goalPoint.northing = rNorth + (Math.Cos(pivotHeading) * -goalPointDistance);
                        goalPoint.easting = rEast + (Math.Sin(pivotHeading) * -goalPointDistance);
                    }
                    else if (!curList.loop && pB == curList.points.Count - 1 && CountUp && U > 1)//extend end of line
                    {
                        goalPoint.northing = start.northing + (Math.Cos(pivotHeading) * goalPointDistance);
                        goalPoint.easting = start.easting + (Math.Sin(pivotHeading) * goalPointDistance);
                    }
                    else
                    {
                        for (int i = CountUp ? pB : pA; (CountUp ? i < pB : i > pA) || loop; i += count)
                        {
                            if (i >= curList.points.Count || i <= -1)
                            {
                                if (loop && curList.loop)
                                {
                                    if (i <= 0) i = curList.points.Count;
                                    else i = -1;
                                    loop = false;
                                    continue;
                                }
                                else
                                    break;
                            }

                            // used for calculating the length squared of next segment.
                            double tempDist = glm.Distance(start, curList.points[i]);

                            //will we go too far?
                            if ((tempDist + distSoFar) > goalPointDistance)
                            {
                                double j = (goalPointDistance - distSoFar) / tempDist; // the remainder to yet travel

                                goalPoint.easting = (((1 - j) * start.easting) + (j * curList.points[i].easting));
                                goalPoint.northing = (((1 - j) * start.northing) + (j * curList.points[i].northing));
                                break;
                            }
                            else distSoFar += tempDist;

                            start = curList.points[i];

                            if (i == 0 && !curList.loop && !CountUp)//extend end of line
                            {
                                goalPoint.northing = start.northing + (Math.Cos(pivotHeading) * -(goalPointDistance - distSoFar));
                                goalPoint.easting = start.easting + (Math.Sin(pivotHeading) * -(goalPointDistance - distSoFar));
                            }
                            else if (!curList.loop && i == curList.points.Count - 1 && CountUp)//extend end of line
                            {
                                //goalPointDistance is longer than remaining u-turn
                                completeYouTurn = true;
                                goalPoint.northing = start.northing + (Math.Cos(pivotHeading) * (goalPointDistance - distSoFar));
                                goalPoint.easting = start.easting + (Math.Sin(pivotHeading) * (goalPointDistance - distSoFar));
                            }
                        }
                    }
                    //calc "D" the distance from pivot axle to lookahead point
                    double goalPointDistanceSquared = glm.DistanceSquared(goalPoint, pivot);

                    //calculate the the delta x in local coordinates and steering angle degrees based on wheelbase
                    double localHeading = glm.twoPI - heading + ((isYouTurnTriggered || isHeadingSameWay) ? inty : -inty);

                    steerAngle = glm.toDegrees(Math.Atan(2 * (((goalPoint.easting - pivot.easting) * Math.Cos(localHeading))
                        + ((goalPoint.northing - pivot.northing) * Math.Sin(localHeading))) * mf.vehicle.wheelbase / goalPointDistanceSquared));

                    if (!isYouTurnTriggered && mf.mc.imuRoll != 88888)
                        steerAngle += mf.mc.imuRoll * -mf.mc.sideHillCompFactor;

                    if (steerAngle < -mf.vehicle.maxSteerAngle) steerAngle = -mf.vehicle.maxSteerAngle;
                    if (steerAngle > mf.vehicle.maxSteerAngle) steerAngle = mf.vehicle.maxSteerAngle;

                    #endregion Pure Pursuit
                }

                if (!isYouTurnTriggered && !isHeadingSameWay)
                {
                    distanceFromCurrentLinePivot *= -1.0;
                    distanceFromCurrentLineTool *= -1.0;
                }

                mf.guidanceLineDistanceOff = distanceFromCurrentLinePivot;
                mf.guidanceLineDistanceOffTool = distanceFromCurrentLineTool;
                mf.guidanceLineSteerAngle = (short)(steerAngle * 100);
            }
            else
            {
                completeYouTurn = true;
                //invalid distance so tell AS module
                mf.guidanceLineDistanceOff = double.NaN;
            }
            if (completeYouTurn && isYouTurnTriggered)
                CompleteYouTurn();
        }
    }

    public enum Mode { None = 0, AB = 2, Curve = 4, Contour = 8, RecPath = 16 };//, Heading, Circle, Spiral

    public class CGuidanceLine : Polyline
    {
        public Mode mode;
        public string Name = "aa";

        public CGuidanceLine(Mode _mode, Polyline poly)
        {
            mode = _mode;
            loop = poly.loop;
            points = poly.points;
        }

        public CGuidanceLine(Mode _mode)
        {
            mode = _mode;
        }

        public CGuidanceLine(CGuidanceLine old)
        {
            mode = old.mode;
            loop = old.loop;
            Name = old.Name;
            points.AddRange(old.points.ToArray());
        }
    }

    public class CGuidanceRecPath : CGuidanceLine
    {
        public List<CRecPathPt> Status = new List<CRecPathPt>();

        public CGuidanceRecPath(Mode _mode) : base(_mode)
        {
        }

        public CGuidanceRecPath(CGuidanceLine old) : base(old)
        {
            if (old is CGuidanceRecPath _old)
            {
                Status = _old.Status;
            }
        }
    }
}