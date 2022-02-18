using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public partial class CGuidance
    {
        /// <summary>/// triggered right after youTurnTriggerPoint is set /// </summary>
        public bool isYouTurnTriggered;

        /// <summary>  /// turning right or left?/// </summary>
        public bool isYouTurnRight;

        /// <summary> /// Is the youturn button enabled? /// </summary>
        public bool isYouTurnBtnOn;

        public double boundaryAngleOffPerpendicular, turnOffset;

        public int rowSkipsWidth = 1, uTurnSmoothing = 10;

        public bool alternateSkips = false, previousBigSkip = true;
        public int rowSkipsWidth2 = 3, turnSkips = 2;

        /// <summary>  /// distance from headland as offset where to start turn shape /// </summary>
        public int youTurnStartOffset;

        //guidance values
        public double uturnDistanceFromBoundary;

        public bool isTurnCreationTooClose = false, isTurnCreationNotCrossingError = false, TurnCreationWidthError = false;

        //list of points for scaled and rotated YouTurn line, used for pattern, dubins, abcurve, abline
        public List<vec3> ytList = new List<vec3>();

        //is UTurn pattern in or out of bounds
        public bool isOutOfBounds = false, TurnRight;

        //sequence of operations of finding the next turn 0 to 3
        public int youTurnPhase, onA;

        //pure pursuit values
        public vecCrossing ExitPoint = new vecCrossing(0, 0, 0, 0, 0, 0), EntryPoint = new vecCrossing(0, 0, 0, 0, 0, 0);
        private List<vec3> OffsetList = new List<vec3>();

        //Patterns or Dubins
        public byte YouTurnType = 2;
        public bool SwapYouTurn = true;
        double ytLength;


        public void FindTurnPoints()
        {
            if (curList.Count < 2)
            {
                youTurnPhase = 255;
                isTurnCreationNotCrossingError = true;
                return;
            }
            else if (youTurnPhase == 0)
            {
                youTurnPhase = 10;
                return;
            }
            else if (youTurnPhase == 10)
            {
                #region FindExitPoint

                ytList.Clear();

                bool Loop = true;
                int Count = isHeadingSameWay ? 1 : -1;
                vecCrossing Crossing = new vecCrossing(0, 0, double.MaxValue, -1, -1, -1);

                vec3 Start = new vec3(rEast, rNorth, 0), End;

                if (mf.bnd.IsPointInsideTurnArea(Start) != 0)
                {
                    youTurnPhase = 255;
                    isTurnCreationNotCrossingError = true;
                    return;
                }

                for (int i = isHeadingSameWay ? currentLocationIndexB : currentLocationIndexA; (isHeadingSameWay ? i < currentLocationIndexB : i > currentLocationIndexA) || Loop; i += Count)
                {
                    if ((isHeadingSameWay && i >= curList.Count) || (!isHeadingSameWay && i < 0))
                    {
                        if (currentGuidanceLine.mode.HasFlag(Mode.Boundary) && Loop)
                        {
                            if (i < 0)
                                i = curList.Count - 1;
                            else
                                i = 0;
                            Loop = false;
                        }
                        else break;
                    }

                    End = curList[i];
                    for (int j = 0; j < mf.bnd.bndList.Count; j++)
                    {
                        if (mf.bnd.bndList[j].isDriveThru) continue;

                        if (mf.bnd.bndList[j].turnLine.points.Count > 1)
                        {
                            int k = mf.bnd.bndList[j].turnLine.points.Count - 1;
                            for (int l = 0; l < mf.bnd.bndList[j].turnLine.points.Count; l++)
                            {
                                if (StaticClass.GetLineIntersection(Start, End, mf.bnd.bndList[j].turnLine.points[l], mf.bnd.bndList[j].turnLine.points[k], out vec2 _Crossing, out double Time, out _))
                                {
                                    if (Time < Crossing.time)
                                        Crossing = new vecCrossing(_Crossing.easting, _Crossing.northing, Time, j, i, l);
                                }
                                k = l;
                            }
                        }
                    }

                    if (Crossing.boundaryIdx >= 0)
                    {
                        mf.distancePivotToTurnLine = glm.Distance(mf.pivotAxlePos, Crossing.easting, Crossing.northing);

                        ExitPoint = Crossing;

                        break;
                    }
                    Start = End;
                }

                if (Crossing.boundaryIdx == -1)
                {
                    youTurnPhase = 255;
                    if (!currentGuidanceLine.mode.HasFlag(Mode.Boundary)) isTurnCreationNotCrossingError = true;
                    return;
                }

                #endregion FindExitPoint

                youTurnPhase = 11;
                return;
            }
            else if (youTurnPhase == 11)
            {
                #region CreateOffsetList

                double distAway = (mf.tool.toolWidth - mf.tool.toolOverlap) * (howManyPathsAway + rowSkipsWidth * (isHeadingSameWay ? (isYouTurnRight ? 1 : -1) : (isYouTurnRight ? -1 : 1))) + (isHeadingSameWay ? mf.tool.toolOffset : -mf.tool.toolOffset);

                OffsetList = BuildOffsetList(currentGuidanceLine, distAway);

                #endregion CreateOffsetList

                youTurnPhase = 12;
                return;
            }
            else if (youTurnPhase == 12)
            {
                #region FindEntryPoint

                turnOffset = ((mf.tool.toolWidth - mf.tool.toolOverlap) * rowSkipsWidth) + (isYouTurnRight ? mf.tool.toolOffset * 2.0 : -mf.tool.toolOffset * 2.0);

                TurnRight = (turnOffset > 0) ? isYouTurnRight : !isYouTurnRight;
                if (ExitPoint.boundaryIdx != 0) TurnRight = !TurnRight;

                int Idx = (ExitPoint.turnLineIdx - (TurnRight ? 0 : 1)).Clamp(mf.bnd.bndList[ExitPoint.boundaryIdx].turnLine.points.Count);
                int StartInt = Idx;

                int Count = TurnRight ? 1 : -1;

                ytList.Add(new vec3(ExitPoint.easting, ExitPoint.northing, 0));

                vec2 Start = new vec2(ExitPoint.easting, ExitPoint.northing);
                vecCrossing Crossing = new vecCrossing(0, 0, double.MaxValue, -2, -2, -2);
                bool Loop = true;
                //check if outside a border
                for (int j = Idx; (TurnRight ? j < StartInt : j > StartInt) || Loop; j += Count)
                {
                    if (j >= mf.bnd.bndList[ExitPoint.boundaryIdx].turnLine.points.Count || j < 0)
                    {
                        if (j < 0)
                            j = mf.bnd.bndList[ExitPoint.boundaryIdx].turnLine.points.Count - 1;
                        else j = 0;
                        Loop = false;
                    }
                    vec2 End = mf.bnd.bndList[ExitPoint.boundaryIdx].turnLine.points[j];

                    int L = 0;
                    for (int K = 1; K < OffsetList.Count; L = K++)
                    {
                        if (StaticClass.GetLineIntersection(OffsetList[L], OffsetList[K], Start, End, out vec2 _Crossing, out double Time, out _))
                        {
                            if (Time < Crossing.time)
                                Crossing = new vecCrossing(_Crossing.easting, _Crossing.northing, Time, 1, K, j);
                        }
                    }

                    bool selfintersect = false;

                    if (YouTurnType == 2 && !currentGuidanceLine.mode.HasFlag(Mode.Boundary))
                    {
                        vec3 End2;

                        vec3 Start2 = new vec3(ExitPoint.easting, ExitPoint.northing, 0);
                        for (int i = (isHeadingSameWay ? ExitPoint.crosssingIdx : ExitPoint.crosssingIdx - 1).Clamp(curList.Count); i < curList.Count; i++)
                        {
                            End2 = curList[i];
                            if (StaticClass.GetLineIntersection(Start2, End2, Start, End, out vec2 _Crossing, out double Time, out _))
                            {
                                if (isHeadingSameWay)
                                {
                                    if (Time < Crossing.time)
                                        Crossing = new vecCrossing(_Crossing.easting, _Crossing.northing, Time, -1, i, j);
                                }
                                else
                                    selfintersect = true;
                            }
                            Start2 = End2;
                        }

                        Start2 = new vec3(ExitPoint.easting, ExitPoint.northing, 0);
                        for (int i = (isHeadingSameWay ? ExitPoint.crosssingIdx - 1 : ExitPoint.crosssingIdx).Clamp(curList.Count); i > -1; i--)
                        {
                            End2 = curList[i];
                            if (StaticClass.GetLineIntersection(Start2, End2, Start, End, out vec2 _Crossing, out double Time, out _))
                            {
                                if (!isHeadingSameWay)
                                {
                                    if (Time < Crossing.time)
                                        Crossing = new vecCrossing(_Crossing.easting, _Crossing.northing, Time, -1, i, j);
                                }
                                else
                                    selfintersect = true;
                            }
                            Start2 = End2;
                        }
                    }

                    if (selfintersect || Crossing.crosssingIdx >= 0)
                        break;//we only care for the closest one;
                    else ytList.Add(new vec3(End.easting, End.northing, 0));

                    Start = End;
                }

                if (Crossing.crosssingIdx == -2)
                {
                    ytList.Clear();

                    List<vec3> ExitLine;

                    if (isHeadingSameWay)
                        ExitLine = curList.GetRange(0, ExitPoint.crosssingIdx);
                    else
                    {
                        ExitLine = curList.GetRange((ExitPoint.crosssingIdx + 1).Clamp(curList.Count), curList.Count - (ExitPoint.crosssingIdx + 1).Clamp(curList.Count));
                        ExitLine.Reverse();
                    }

                    ytList.Add(new vec3(ExitPoint.easting, ExitPoint.northing, 0));

                    //now stepback to add uturn length again? . . .
                    double TotalDist = 0;
                    for (int i = ExitLine.Count - 1; i >= 0; i--)
                    {
                        double Distance = glm.Distance(ytList[0], ExitLine[i]);
                        if (TotalDist + Distance > youTurnStartOffset)
                        {
                            double Dx = (ytList[0].northing - ExitLine[i].northing) / Distance * (youTurnStartOffset - TotalDist);
                            double Dy = (ytList[0].easting - ExitLine[i].easting) / Distance * (youTurnStartOffset - TotalDist);
                            ytList.Insert(0, new vec3(ytList[0].easting - Dy, ytList[0].northing - Dx, 0));
                            break;
                        }
                        else
                        {
                            TotalDist += Distance;
                            ytList.Insert(0, ExitLine[i]);
                        }
                    }

                    TurnCreationWidthError = true;
                    EntryPoint = ExitPoint;
                    OffsetList.Clear();
                    youTurnPhase = 255;
                    return;
                }

                ytList.Add(new vec3(Crossing.easting, Crossing.northing, 0));
                EntryPoint = Crossing;

                #endregion FindEntryPoint

                youTurnPhase = 13;
                return;
            }
            else if (youTurnPhase >= 13)
            {
                #region CreateUTurn

                SwapYouTurn = EntryPoint.boundaryIdx >= 0;

                double Dx;
                double Dy;

                if ((youTurnPhase == 13 && YouTurnType > 0 && Math.Abs(turnOffset) > mf.vehicle.minTurningRadius * 2) || EntryPoint.boundaryIdx == -1)
                {
                    List<vec3> FinalLine = new List<vec3>();
                    List<vec3> ExitLine, EntryLine;
                    if (isHeadingSameWay)
                    {
                        if (ExitPoint.crosssingIdx - currentLocationIndexB > 0)
                            ExitLine = curList.GetRange(currentLocationIndexB, ExitPoint.crosssingIdx - currentLocationIndexB);
                        else
                            ExitLine = new List<vec3>();

                        ExitLine.Insert(0, new vec3(rEast, rNorth, 0));

                        if (EntryPoint.boundaryIdx > 0)
                            EntryLine = OffsetList.GetRange(0, EntryPoint.crosssingIdx);
                        else
                            EntryLine = curList.GetRange(EntryPoint.crosssingIdx, curList.Count - EntryPoint.crosssingIdx);
                        EntryLine.Reverse();
                    }
                    else
                    {
                        if (ExitPoint.crosssingIdx + 1 - currentLocationIndexB > 0)
                            ExitLine = curList.GetRange(currentLocationIndexB, curList.Count - currentLocationIndexB);
                        else
                            ExitLine = new List<vec3>();

                        ExitLine.Insert(0, new vec3(rEast, rNorth, 0));

                        ExitLine.Reverse();

                        if (EntryPoint.boundaryIdx > 0)
                            EntryLine = OffsetList.GetRange(EntryPoint.crosssingIdx, OffsetList.Count - EntryPoint.crosssingIdx);
                        else
                            EntryLine = curList.GetRange(0, EntryPoint.crosssingIdx + 1);
                    }

                    if (EntryPoint.boundaryIdx > 0 && YouTurnType == 1)
                    {
                        vec2 Start, End;
                        Dx = EntryPoint.northing - ExitPoint.northing;
                        Dy = EntryPoint.easting - ExitPoint.easting;

                        double Distance, MaxDistance = 0;

                        for (int j = 0; j < ytList.Count; j++)
                        {
                            Distance = ((Dx * ytList[j].easting) - (Dy * ytList[j].northing) + (EntryPoint.easting
                                        * ExitPoint.northing) - (EntryPoint.northing * ExitPoint.easting))
                                            / Math.Sqrt((Dx * Dx) + (Dy * Dy));
                            if (!TurnRight && Distance < MaxDistance || TurnRight && Distance > MaxDistance)
                            {
                                MaxDistance = Distance;
                            }
                        }

                        ytList.Clear();

                        if (Math.Abs(MaxDistance) > 0.001)
                        {
                            double Length1 = Math.Sqrt(Dx * Dx + Dy * Dy);

                            Dx /= Length1;
                            Dy /= Length1;

                            Start.northing = ExitPoint.northing - MaxDistance * Dy - Dx * mf.maxCrossFieldLength;
                            Start.easting = ExitPoint.easting + MaxDistance * Dx - Dy * mf.maxCrossFieldLength;
                            End.northing = EntryPoint.northing - MaxDistance * Dy + Dx * mf.maxCrossFieldLength;
                            End.easting = EntryPoint.easting + MaxDistance * Dx + Dy * mf.maxCrossFieldLength;

                            vec3 End2, Start2 = new vec3(ExitPoint.easting, ExitPoint.northing, 0);

                            for (int j = ExitLine.Count - 1; j >= 0; j--)
                            {
                                End2 = ExitLine[j];

                                if (StaticClass.GetLineIntersection(Start2, End2, Start, End, out vec2 _Crossing, out _, out _))
                                {
                                    ytList.Add(new vec3(_Crossing.easting, _Crossing.northing, 0));
                                    break;
                                }
                                ExitLine.RemoveAt(j);
                                Start2 = End2;
                            }

                            Start2 = new vec3(EntryPoint.easting, EntryPoint.northing, 0);
                            for (int j = EntryLine.Count - 1; j >= 0; j--)
                            {
                                End2 = EntryLine[j];
                                if (StaticClass.GetLineIntersection(Start2, End2, Start, End, out vec2 _Crossing, out _, out _))
                                {
                                    ytList.Add(new vec3(_Crossing.easting, _Crossing.northing, 0));
                                    break;
                                }
                                EntryLine.RemoveAt(j);
                                Start2 = End2;
                            }
                        }
                        else
                        {
                            ytList.Add(new vec3(ExitPoint.easting, ExitPoint.northing, 0));
                            ytList.Add(new vec3(EntryPoint.easting, EntryPoint.northing, 0));
                        }
                    }

                    FinalLine.AddRange(ExitLine);
                    FinalLine.AddRange(ytList);
                    FinalLine.AddRange(EntryLine);

                    ytList = FinalLine;
                    youTurnPhase = 254;


                    /*

                    double a = ExitPoint.boundaryIndex == 0 == TurnRight ? mf.vehicle.minTurningRadius : -mf.vehicle.minTurningRadius;

                    double b = ExitPoint.boundaryIndex == 0 == TurnRight ? -Math.Max(mf.vehicle.minTurningRadius, uturnDistanceFromBoundary) : Math.Max(mf.vehicle.minTurningRadius, uturnDistanceFromBoundary);

                    double rad = ExitPoint.boundaryIndex == 0 == TurnRight ? mf.vehicle.minTurningRadius : -mf.vehicle.minTurningRadius;



                    List<vec2> InwardLine = FinalLine.OffsetPolyline(rad, false);

                    List<Polyline> Output = InwardLine.DissolvePolyLine(false, 1);
                    */

                    if (FinalLine.Count > 0)
                    {

                        //FinalLine = Output[0].points.CalculateRoundedCorner(a, b, false, 0.0436332);

                        int i = 0;
                        for (; FinalLine.Count > 0 && i < ExitLine.Count; i++)
                        {
                            if (FinalLine[0].northing == ExitLine[i].northing && FinalLine[0].easting == ExitLine[i].easting)
                            {
                                FinalLine.RemoveAt(0);
                            }
                            else break;
                        }

                        //now stepback to add uturn length again? . . .
                        double TotalDist = 0;
                        i--;
                        for (; i >= 0; i--)
                        {
                            double Distance = glm.Distance(FinalLine[0], ExitLine[i]);
                            if (TotalDist + Distance > youTurnStartOffset)
                            {
                                Dx = (FinalLine[0].northing - ExitLine[i].northing) / Distance * (youTurnStartOffset - TotalDist);
                                Dy = (FinalLine[0].easting - ExitLine[i].easting) / Distance * (youTurnStartOffset - TotalDist);
                                FinalLine.Insert(0, new vec3(FinalLine[0].easting - Dy, FinalLine[0].northing - Dx, 0));
                                break;
                            }
                            else
                            {
                                TotalDist += Distance;
                                FinalLine.Insert(0, ExitLine[i]);
                            }
                        }

                        i = EntryLine.Count - 1;

                        for (int j = FinalLine.Count - 1; i >= 0; i--)
                        {
                            if (FinalLine[j].northing == EntryLine[i].northing && FinalLine[j].easting == EntryLine[i].easting)
                            {
                                FinalLine.RemoveAt(j);
                                j = FinalLine.Count - 1;
                            }
                            else
                                break;
                        }

                        //now stepback to add uturn length again? . . .
                        TotalDist = 0;
                        i++;

                        for (; i < EntryLine.Count; i++)
                        {
                            double Distance = glm.Distance(FinalLine[FinalLine.Count - 1], EntryLine[i]);
                            if (TotalDist + Distance > youTurnStartOffset)
                            {
                                Dx = (FinalLine[FinalLine.Count - 1].northing - EntryLine[i].northing) / Distance;
                                Dy = (FinalLine[FinalLine.Count - 1].easting - EntryLine[i].easting) / Distance;
                                FinalLine.Add(new vec3(FinalLine[FinalLine.Count - 1].easting - Dy * (youTurnStartOffset - TotalDist), FinalLine[FinalLine.Count - 1].northing - Dx * (youTurnStartOffset - TotalDist), 0));

                                FinalLine.Add(new vec3(FinalLine[FinalLine.Count - 1].easting - Dy, FinalLine[FinalLine.Count - 1].northing - Dx, 0));

                                break;
                            }
                            else
                            {
                                TotalDist += Distance;
                                FinalLine.Add(EntryLine[i]);
                            }
                        }

                        ytLength = 0;
                        isOutOfBounds = false;
                        for (int j = 0; j + 2 < FinalLine.Count; j++)
                        {
                            ytLength += glm.Distance(FinalLine[j], FinalLine[j + 1]);

                            if (mf.bnd.IsPointInsideTurnArea(FinalLine[j]) != 0)
                            {
                                //isOutOfBounds = true;
                                break;
                            }
                        }
                    }
                    ytList = FinalLine;
                    youTurnPhase = 254;
                    return;
                }
                else
                {
                    int i = (isHeadingSameWay ? ExitPoint.crosssingIdx - 1 : ExitPoint.crosssingIdx + 1).Clamp(curList.Count);
                    int j = (isHeadingSameWay ? EntryPoint.crosssingIdx - 1 : EntryPoint.crosssingIdx).Clamp(OffsetList.Count);

                    Dx = ExitPoint.northing - curList[i].northing;
                    Dy = ExitPoint.easting - curList[i].easting;
                    double Dx2 = EntryPoint.northing - OffsetList[j].northing;
                    double Dy2 = EntryPoint.easting - OffsetList[j].easting;

                    double Heading = Math.Atan2(Dy, Dx);
                    double Heading2 = Math.Atan2(Dy2, Dx2) + Math.PI;

                    vec3 Point = new vec3(ExitPoint.easting, ExitPoint.northing, Heading);
                    vec3 Point2 = new vec3(EntryPoint.easting, EntryPoint.northing, Heading2);

                    double Offset = 5;

                    int count = isHeadingSameWay ? -1 : 1;

                    while (true)
                    {
                        ytList.Clear();

                        CDubins dubYouTurnPath = new CDubins();
                        ytList = dubYouTurnPath.GenerateDubins(Point, Point2);

                        isOutOfBounds = false;
                        for (int k = 0; k < ytList.Count; k += 2)
                        {
                            if (mf.bnd.IsPointInsideTurnArea(ytList[k]) != 0)
                            {
                                isOutOfBounds = true;
                                break;
                            }
                        }
                        if (!isOutOfBounds && Offset == 5)
                        {
                            Offset = 0.5;
                            count = isHeadingSameWay ? 1 : -1;
                            j += count;
                            i += count;
                        }
                        else if (isOutOfBounds && Offset == 0.5)
                        {
                            Offset = 0.05;
                            count = isHeadingSameWay ? -1 : 1;
                            j += count;
                            i += count;
                        }
                        else if (!isOutOfBounds && Offset == 0.05)
                        {
                            ytLength = 0;
                            for (int k = 0; k + 2 < ytList.Count; k++)
                            {
                                ytLength += glm.Distance(ytList[k], ytList[k + 1]);
                            }

                            youTurnPhase = 254;
                            return;
                        }

                        double distSoFar = 0;

                        //cycle thru segments and keep adding lengths. check if start and break if so.
                        while (true)
                        {
                            if (i == -1 || i == curList.Count)
                            {
                                isTurnCreationNotCrossingError = true;
                                return;
                            }

                            double dx1 = curList[i].northing - Point.northing;
                            double dy1 = curList[i].easting - Point.easting;

                            Heading = Math.Atan2(dy1, dx1);
                            double length1 = Math.Sqrt(dx1 * dx1 + dy1 * dy1);

                            //will we go too far?
                            if ((length1 + distSoFar) > Offset)
                            {
                                double factor = (Offset - distSoFar) / length1;

                                Point.easting += dy1 * factor;
                                Point.northing += dx1 * factor;
                                break; //tempDist contains the full length of next segment
                            }
                            distSoFar += length1;
                            Point.easting = curList[i].easting;
                            Point.northing = curList[i].northing;
                            Point.heading = isHeadingSameWay ? Heading : Heading + Math.PI;
                            i += count;
                        }

                        distSoFar = 0;
                        while (true)
                        {
                            if (j == -1 || j == OffsetList.Count)
                            {
                                isTurnCreationNotCrossingError = true;
                                return;
                            }
                            double dx1 = OffsetList[j].northing - Point2.northing;
                            double dy1 = OffsetList[j].easting - Point2.easting;

                            Heading = Math.Atan2(dy1, dx1);
                            double length1 = Math.Sqrt(dx1 * dx1 + dy1 * dy1);

                            //will we go too far?
                            if ((length1 + distSoFar) > Offset)
                            {
                                double factor = (Offset - distSoFar) / length1;

                                Point2.easting += dy1 * factor;
                                Point2.northing += dx1 * factor;
                                break; //tempDist contains the full length of next segment
                            }
                            distSoFar += length1;
                            Point2.easting = OffsetList[j].easting;
                            Point2.northing = OffsetList[j].northing;

                            Point2.heading = isHeadingSameWay ? Heading + Math.PI : Heading;
                            j += count;
                        }

                        if (count > 0)
                            isTurnCreationTooClose = false;

                        if (!isTurnCreationTooClose)
                        {
                            double distancePivotToTurnLine;
                            for (int k = 0; k < ytList.Count; k += 2)
                            {
                                distancePivotToTurnLine = glm.Distance(ytList[k], mf.pivotAxlePos);
                                if (distancePivotToTurnLine > 3)
                                {
                                    isTurnCreationTooClose = false;
                                }
                                else
                                {
                                    isTurnCreationTooClose = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                #endregion CreateUTurn
            }
        }

        public void AddSequenceLines(double head)
        {
            vec3 pt;
            for (int a = 0; a < youTurnStartOffset * 2; a++)
            {
                pt.easting = ytList[0].easting + (Math.Sin(head) * 0.2);
                pt.northing = ytList[0].northing + (Math.Cos(head) * 0.2);
                pt.heading = ytList[0].heading;
                ytList.Insert(0, pt);
            }

            int count = ytList.Count;

            for (int i = 1; i <= youTurnStartOffset * 2; i++)
            {
                pt.easting = ytList[count - 1].easting + (Math.Sin(head) * i * 0.2);
                pt.northing = ytList[count - 1].northing + (Math.Cos(head) * i * 0.2);
                pt.heading = head;
                ytList.Add(pt);
            }

            double distancePivotToTurnLine;
            count = ytList.Count;
            for (int i = 0; i < count; i += 2)
            {
                distancePivotToTurnLine = glm.Distance(ytList[i], mf.pivotAxlePos);
                if (distancePivotToTurnLine > 3)
                {
                    isTurnCreationTooClose = false;
                }
                else
                {
                    isTurnCreationTooClose = true;
                    //set the flag to Critical stop machine
                    if (isTurnCreationTooClose) mf.mc.isOutOfBounds = true;
                    break;
                }
            }
        }

        public void SmoothYouTurn(int smPts)
        {
            //count the reference list of original curve
            int cnt = ytList.Count;

            //the temp array
            vec3[] arr = new vec3[cnt];

            //read the points before and after the setpoint
            for (int s = 0; s < smPts / 2; s++)
            {
                arr[s].easting = ytList[s].easting;
                arr[s].northing = ytList[s].northing;
                arr[s].heading = ytList[s].heading;
            }

            for (int s = cnt - (smPts / 2); s < cnt; s++)
            {
                arr[s].easting = ytList[s].easting;
                arr[s].northing = ytList[s].northing;
                arr[s].heading = ytList[s].heading;
            }

            //average them - center weighted average
            for (int i = smPts / 2; i < cnt - (smPts / 2); i++)
            {
                for (int j = -smPts / 2; j < smPts / 2; j++)
                {
                    arr[i].easting += ytList[j + i].easting;
                    arr[i].northing += ytList[j + i].northing;
                }
                arr[i].easting /= smPts;
                arr[i].northing /= smPts;
                arr[i].heading = ytList[i].heading;
            }

            ytList?.Clear();

            //calculate new headings on smoothed line
            for (int i = 1; i < cnt - 1; i++)
            {
                arr[i].heading = Math.Atan2(arr[i + 1].easting - arr[i].easting, arr[i + 1].northing - arr[i].northing);
                if (arr[i].heading < 0) arr[i].heading += glm.twoPI;
                ytList.Add(arr[i]);
            }
            youTurnPhase = 255;
        }

        //called to initiate turn
        public void YouTurnTrigger()
        {
            if (SwapYouTurn)
            {
                if (isHeadingSameWay == isYouTurnRight)
                    howManyPathsAway += rowSkipsWidth;
                else
                    howManyPathsAway -= rowSkipsWidth;

                isHeadingSameWay = !isHeadingSameWay;

                if (alternateSkips && rowSkipsWidth2 > 1)
                {
                    if (--turnSkips == 0)
                    {
                        isYouTurnRight = !isYouTurnRight;
                        turnSkips = rowSkipsWidth2 * 2 - 1;
                    }
                    else if (previousBigSkip = !previousBigSkip)
                        rowSkipsWidth = rowSkipsWidth2 - 1;
                    else
                        rowSkipsWidth = rowSkipsWidth2;
                }
                else isYouTurnRight = !isYouTurnRight;

                curList.Clear();
                curList = OffsetList;
            }

            SwapYouTurn = true;
            isYouTurnTriggered = true;
        }

        //Normal copmpletion of youturn
        public void CompleteYouTurn()
        {
            isYouTurnTriggered = false;
            ResetCreatedYouTurn();
            mf.sounds.isBoundAlarming = false;
        }

        public void Set_Alternate_skips()
        {
            rowSkipsWidth2 = rowSkipsWidth;
            turnSkips = rowSkipsWidth2 * 2 - 1;
            previousBigSkip = false;
        }

        //something went seriously wrong so reset everything
        public void ResetYouTurn()
        {
            //fix you turn
            isYouTurnTriggered = false;
            ytList?.Clear();
            ResetCreatedYouTurn();
            mf.sounds.isBoundAlarming = false;
            isTurnCreationTooClose = false;
            isTurnCreationNotCrossingError = false;
            TurnCreationWidthError = false;
        }

        public void ResetCreatedYouTurn()
        {
            youTurnPhase = -2;
            ytList?.Clear();
        }

        public void BuildManualYouLateral(bool isTurnRight)
        {
            if (isHeadingSameWay == isTurnRight)
                howManyPathsAway += 1;
            else
                howManyPathsAway -= 1;
        }

        //build the points and path of youturn to be scaled and transformed
        public void BuildManualYouTurn(bool isTurnRight, bool isTurnButtonTriggered)
        {
            isYouTurnTriggered = true;

            if (isHeadingSameWay == isTurnRight)
                howManyPathsAway += rowSkipsWidth;
            else
                howManyPathsAway -= rowSkipsWidth;

            if (curList.Count < 2) return;

            double rEastYT = rEast;
            double rNorthYT = rNorth;

            //get the distance from currently active AB line
            double Dx = curList[currentLocationIndexB].northing - curList[currentLocationIndexA].northing;
            double Dy = curList[currentLocationIndexB].easting - curList[currentLocationIndexA].easting;

            if (Math.Abs(Dy) < double.Epsilon && Math.Abs(Dx) < double.Epsilon) return;

            double Heading = Math.Atan2(Dy, Dx);

            isHeadingSameWay = !isHeadingSameWay;

            //grab the vehicle widths and offsets
            double turnOffset = (mf.tool.toolWidth - mf.tool.toolOverlap) * rowSkipsWidth + (isTurnRight ? mf.tool.toolOffset * 2.0 : -mf.tool.toolOffset * 2.0);

            CDubins dubYouTurnPath = new CDubins();
            CDubins.turningRadius = mf.vehicle.minTurningRadius;

            //if its straight across it makes 2 loops instead so goal is a little lower then start
            if (isHeadingSameWay) Heading += 3.14;
            else Heading -= 0.01;

            //move the start forward 2 meters, this point is critical to formation of uturn
            rEastYT += (Math.Sin(Heading) * 4.0);
            rNorthYT += (Math.Cos(Heading) * 4.0);

            //now we have our start point
            var start = new vec3(rEastYT, rNorthYT, Heading);
            var goal = new vec3(0, 0, 0);

            //now we go the other way to turn round
            Heading -= Math.PI;
            if (Heading < 0) Heading += glm.twoPI;

            //set up the goal point for Dubins
            goal.heading = Heading;
            if (isTurnRight)
            {
                goal.easting = rEastYT - (Math.Cos(-Heading) * turnOffset);
                goal.northing = rNorthYT - (Math.Sin(-Heading) * turnOffset);
            }
            else
            {
                goal.easting = rEastYT + (Math.Cos(-Heading) * turnOffset);
                goal.northing = rNorthYT + (Math.Sin(-Heading) * turnOffset);
            }

            //generate the turn points
            ytList = dubYouTurnPath.GenerateDubins(start, goal);
        }
    }
}