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

        public double turnOffset;

        public int rowSkipsWidth = 1, uTurnSmoothing = 10;

        public bool alternateSkips = false, previousBigSkip = true;
        public int rowSkipsWidth2 = 3, turnSkips = 2;

        /// <summary>  /// distance from headland as offset where to start turn shape /// </summary>
        public int youTurnStartOffset;

        //guidance values
        public double uturnDistanceFromBoundary;

        public bool isTurnCreationTooClose = false, isTurnCreationNotCrossingError = false, TurnCreationWidthError = false;

        //list of points for scaled and rotated YouTurn line, used for pattern, dubins, abcurve, abline
        public Polyline ytList = new Polyline();

        //is UTurn pattern in or out of bounds
        public bool isOutOfBounds = false, TurnRight;

        //sequence of operations of finding the next turn 0 to 3
        public int youTurnPhase;

        //pure pursuit values
        public vecCrossing ExitPoint = new vecCrossing(0, 0, 0, 0, 0, 0), EntryPoint = new vecCrossing(0, 0, 0, 0, 0, 0);
        private Polyline OffsetList = new Polyline();

        private int currIdx, offsetIdx, upDownCount;
        private double currHeading, offsetHeading, Offset = 5;
        private vec2 Start, End;

        //Patterns or Dubins
        public byte YouTurnType = 2;
        public bool SwapYouTurn = true;
        public double totalUTurnLength, onA;

        public void FindTurnPoints(vec2 pivot)
        {
            if (curList.points.Count < 2)
            {
                youTurnPhase = 255;
                isTurnCreationNotCrossingError = true;
                return;
            }
            else if (youTurnPhase == 0)
            {
                isOutOfBounds = true;
                youTurnPhase = 10;
                return;
            }
            else if (youTurnPhase == 10)
            {
                #region FindExitPoint

                ytList.points.Clear();

                bool Loop = true;
                int Count = isHeadingSameWay ? 1 : -1;
                vecCrossing Crossing = new vecCrossing(0, 0, double.MaxValue, -1, -1, -1);

                vec2 Start = new vec2(rEast, rNorth), End;

                if (mf.bnd.IsPointInsideTurnArea(Start) != 0)
                {
                    youTurnPhase = 255;
                    isTurnCreationNotCrossingError = true;
                    return;
                }

                for (int i = isHeadingSameWay ? currentLocationIndexB : currentLocationIndexA; (isHeadingSameWay ? i < currentLocationIndexB : i > currentLocationIndexA) || Loop; i += Count)
                {
                    if ((isHeadingSameWay && i >= curList.points.Count) || (!isHeadingSameWay && i < 0))
                    {
                        if (curList.loop && Loop)
                        {
                            if (i < 0)
                                i = curList.points.Count;
                            else
                                i = -1;
                            Loop = false;
                            continue;
                        }
                        else break;
                    }

                    End = new vec2(curList.points[i].easting, curList.points[i].northing);
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
                        ExitPoint = Crossing;

                        break;
                    }
                    Start = End;
                }

                if (Crossing.boundaryIdx == -1)
                {
                    youTurnPhase = 255;
                    if (!curList.loop) isTurnCreationNotCrossingError = true;
                    return;
                }

                #endregion FindExitPoint

                youTurnPhase = 11;
                return;
            }
            else if (youTurnPhase == 11)
            {
                #region CreateOffsetList

                double distAway = (mf.tool.toolWidth - mf.tool.toolOverlap) * (howManyPathsAway + rowSkipsWidth * (isHeadingSameWay ^ isYouTurnRight ? -1 : 1)) + (isHeadingSameWay ? mf.tool.toolOffset : -mf.tool.toolOffset);

                OffsetList = BuildOffsetList(currentGuidanceLine, distAway);

                #endregion CreateOffsetList

                youTurnPhase = 12;
                return;
            }
            else if (youTurnPhase == 12)
            {
                #region FindEntryPoint

                turnOffset = ((mf.tool.toolWidth - mf.tool.toolOverlap) * rowSkipsWidth) + (isYouTurnRight ? mf.tool.toolOffset * 2.0 : -mf.tool.toolOffset * 2.0);
                if (turnOffset == 0)
                {
                    EntryPoint = ExitPoint;
                    if (!isHeadingSameWay)
                        EntryPoint.crosssingIdx += 1;
                    youTurnPhase = 13;
                    return;
                }
                TurnRight = (turnOffset > 0 ^ ExitPoint.boundaryIdx != 0) ? isYouTurnRight : !isYouTurnRight;


                int Idx = (ExitPoint.turnLineIdx - (TurnRight ? 0 : 1)).Clamp(mf.bnd.bndList[ExitPoint.boundaryIdx].turnLine.points.Count);
                int StartInt = Idx;

                int Count = TurnRight ? 1 : -1;

                ytList.points.Add(new vec2(ExitPoint.easting, ExitPoint.northing));

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
                    for (int K = 1; K < OffsetList.points.Count; L = K++)
                    {
                        if (StaticClass.GetLineIntersection(OffsetList.points[L], OffsetList.points[K], Start, End, out vec2 _Crossing, out double Time, out _))
                        {
                            if (Time < Crossing.time)
                                Crossing = new vecCrossing(_Crossing.easting, _Crossing.northing, Time, 1, K, j);
                        }
                    }

                    bool selfintersect = false;

                    if (YouTurnType == 2 && !curList.loop)
                    {
                        vec2 End2, Start2 = new vec2(ExitPoint.easting, ExitPoint.northing);
                        for (int i = (isHeadingSameWay ? ExitPoint.crosssingIdx : ExitPoint.crosssingIdx + 1); i < curList.points.Count; i++)
                        {
                            End2 = curList.points[i];
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

                        Start2 = new vec2(ExitPoint.easting, ExitPoint.northing);
                        for (int i = (isHeadingSameWay ? ExitPoint.crosssingIdx - 1 : ExitPoint.crosssingIdx); i > -1; i--)
                        {
                            End2 = curList.points[i];
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
                    else ytList.points.Add(new vec2(End.easting, End.northing));

                    Start = End;
                }

                if (Crossing.crosssingIdx == -2)
                {
                    ytList.points.Clear();

                    List<vec2> ExitLine;

                    if (isHeadingSameWay)
                        ExitLine = curList.points.GetRange(0, ExitPoint.crosssingIdx);
                    else
                    {
                        ExitLine = curList.points.GetRange((ExitPoint.crosssingIdx + 1).Clamp(curList.points.Count), curList.points.Count - (ExitPoint.crosssingIdx + 1).Clamp(curList.points.Count));
                        ExitLine.Reverse();
                    }

                    ytList.points.Add(new vec2(ExitPoint.easting, ExitPoint.northing));

                    //now stepback to add uturn length again? . . .
                    double TotalDist = 0;
                    for (int i = ExitLine.Count - 1; i >= 0; i--)
                    {
                        double Distance = glm.Distance(ytList.points[0], ExitLine[i]);
                        if (TotalDist + Distance > youTurnStartOffset)
                        {
                            double Dx = (ytList.points[0].northing - ExitLine[i].northing) / Distance * (youTurnStartOffset - TotalDist);
                            double Dy = (ytList.points[0].easting - ExitLine[i].easting) / Distance * (youTurnStartOffset - TotalDist);
                            ytList.points.Insert(0, new vec2(ytList.points[0].easting - Dy, ytList.points[0].northing - Dx));
                            break;
                        }
                        else
                        {
                            TotalDist += Distance;
                            ytList.points.Insert(0, ExitLine[i]);
                        }
                    }

                    TurnCreationWidthError = true;
                    EntryPoint = ExitPoint;
                    OffsetList.points.Clear();
                    youTurnPhase = 255;
                    return;
                }

                ytList.points.Add(new vec2(Crossing.easting, Crossing.northing));
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
                    Polyline FinalLine = new Polyline();
                    List<vec2> ExitLine, EntryLine;
                    if (isHeadingSameWay)
                    {
                        if (ExitPoint.crosssingIdx - currentLocationIndexB > 0)
                            ExitLine = curList.points.GetRange(currentLocationIndexB, ExitPoint.crosssingIdx - currentLocationIndexB);
                        else
                            ExitLine = new List<vec2>();

                        ExitLine.Insert(0, new vec2(rEast, rNorth));

                        if (EntryPoint.boundaryIdx > 0)
                            EntryLine = OffsetList.points.GetRange(0, EntryPoint.crosssingIdx);
                        else
                            EntryLine = curList.points.GetRange(EntryPoint.crosssingIdx, curList.points.Count - EntryPoint.crosssingIdx);
                        EntryLine.Reverse();
                    }
                    else
                    {
                        if (ExitPoint.crosssingIdx + 1 - currentLocationIndexB > 0)
                            ExitLine = curList.points.GetRange(currentLocationIndexB, curList.points.Count - currentLocationIndexB);
                        else
                            ExitLine = new List<vec2>();

                        ExitLine.Insert(0, new vec2(rEast, rNorth));

                        ExitLine.Reverse();

                        if (EntryPoint.boundaryIdx > 0)
                            EntryLine = OffsetList.points.GetRange(EntryPoint.crosssingIdx, OffsetList.points.Count - EntryPoint.crosssingIdx);
                        else
                            EntryLine = curList.points.GetRange(0, EntryPoint.crosssingIdx + 1);
                    }

                    if (EntryPoint.boundaryIdx > 0 && YouTurnType == 1)
                    {
                        vec2 Start, End;
                        Dx = EntryPoint.northing - ExitPoint.northing;
                        Dy = EntryPoint.easting - ExitPoint.easting;

                        double Distance, MaxDistance = 0;

                        for (int j = 0; j < ytList.points.Count; j++)
                        {
                            Distance = ((Dx * ytList.points[j].easting) - (Dy * ytList.points[j].northing) + (EntryPoint.easting
                                        * ExitPoint.northing) - (EntryPoint.northing * ExitPoint.easting))
                                            / Math.Sqrt((Dx * Dx) + (Dy * Dy));
                            if (!TurnRight && Distance < MaxDistance || TurnRight && Distance > MaxDistance)
                            {
                                MaxDistance = Distance;
                            }
                        }

                        ytList.points.Clear();

                        if (Math.Abs(MaxDistance) > 0.001)
                        {
                            double Length1 = Math.Sqrt(Dx * Dx + Dy * Dy);

                            Dx /= Length1;
                            Dy /= Length1;

                            Start.northing = ExitPoint.northing - MaxDistance * Dy - Dx * mf.maxCrossFieldLength;
                            Start.easting = ExitPoint.easting + MaxDistance * Dx - Dy * mf.maxCrossFieldLength;
                            End.northing = EntryPoint.northing - MaxDistance * Dy + Dx * mf.maxCrossFieldLength;
                            End.easting = EntryPoint.easting + MaxDistance * Dx + Dy * mf.maxCrossFieldLength;

                            vec2 End2, Start2 = new vec2(ExitPoint.easting, ExitPoint.northing);

                            for (int j = ExitLine.Count - 1; j >= 0; j--)
                            {
                                End2 = ExitLine[j];

                                if (StaticClass.GetLineIntersection(Start2, End2, Start, End, out vec2 _Crossing, out _, out _))
                                {
                                    ytList.points.Add(new vec2(_Crossing.easting, _Crossing.northing));
                                    break;
                                }
                                ExitLine.RemoveAt(j);
                                Start2 = End2;
                            }

                            Start2 = new vec2(EntryPoint.easting, EntryPoint.northing);
                            for (int j = EntryLine.Count - 1; j >= 0; j--)
                            {
                                End2 = EntryLine[j];
                                if (StaticClass.GetLineIntersection(Start2, End2, Start, End, out vec2 _Crossing, out _, out _))
                                {
                                    ytList.points.Add(new vec2(_Crossing.easting, _Crossing.northing));
                                    break;
                                }
                                EntryLine.RemoveAt(j);
                                Start2 = End2;
                            }
                        }
                        else
                        {
                            ytList.points.Add(new vec2(ExitPoint.easting, ExitPoint.northing));
                            ytList.points.Add(new vec2(EntryPoint.easting, EntryPoint.northing));
                        }
                    }

                    FinalLine.points.AddRange(ExitLine);
                    FinalLine.points.AddRange(ytList.points);
                    FinalLine.points.AddRange(EntryLine);

                    double a = ExitPoint.boundaryIdx == 0 == TurnRight ? mf.vehicle.minTurningRadius : uturnDistanceFromBoundary;
                    double b = ExitPoint.boundaryIdx == 0 == TurnRight ? Math.Max(mf.vehicle.minTurningRadius, uturnDistanceFromBoundary) : mf.vehicle.minTurningRadius;

                    FinalLine.points.CalculateRoundedCorner(a, b, false, 0.0436332, ExitLine.Count, FinalLine.points.Count - EntryLine.Count);

                    if (FinalLine.points.Count > 0)
                    {
                        int i = 0;
                        for (; FinalLine.points.Count > 0 && i < ExitLine.Count; i++)
                        {
                            if (FinalLine.points[0].northing == ExitLine[i].northing && FinalLine.points[0].easting == ExitLine[i].easting)
                            {
                                FinalLine.points.RemoveAt(0);
                            }
                            else break;
                        }

                        //now stepback to add uturn length again? . . .
                        double TotalDist = 0;
                        i--;
                        for (; i >= 0; i--)
                        {
                            double Distance = glm.Distance(FinalLine.points[0], ExitLine[i]);
                            if (TotalDist + Distance > youTurnStartOffset)
                            {
                                Dx = (FinalLine.points[0].northing - ExitLine[i].northing) / Distance * (youTurnStartOffset - TotalDist);
                                Dy = (FinalLine.points[0].easting - ExitLine[i].easting) / Distance * (youTurnStartOffset - TotalDist);
                                FinalLine.points.Insert(0, new vec2(FinalLine.points[0].easting - Dy, FinalLine.points[0].northing - Dx));
                                break;
                            }
                            else
                            {
                                TotalDist += Distance;
                                FinalLine.points.Insert(0, ExitLine[i]);
                            }
                        }

                        i = EntryLine.Count - 1;

                        for (int j = FinalLine.points.Count - 1; i >= 0; i--)
                        {
                            if (FinalLine.points[j].northing == EntryLine[i].northing && FinalLine.points[j].easting == EntryLine[i].easting)
                            {
                                FinalLine.points.RemoveAt(j);
                                j = FinalLine.points.Count - 1;
                            }
                            else
                                break;
                        }

                        //now stepback to add uturn length again? . . .
                        TotalDist = 0;
                        i++;

                        for (; i < EntryLine.Count; i++)
                        {
                            double Distance = glm.Distance(FinalLine.points[FinalLine.points.Count - 1], EntryLine[i]);
                            if (TotalDist + Distance > youTurnStartOffset)
                            {
                                Dx = (FinalLine.points[FinalLine.points.Count - 1].northing - EntryLine[i].northing) / Distance;
                                Dy = (FinalLine.points[FinalLine.points.Count - 1].easting - EntryLine[i].easting) / Distance;
                                FinalLine.points.Add(new vec2(FinalLine.points[FinalLine.points.Count - 1].easting - Dy * (youTurnStartOffset - TotalDist), FinalLine.points[FinalLine.points.Count - 1].northing - Dx * (youTurnStartOffset - TotalDist)));

                                FinalLine.points.Add(new vec2(FinalLine.points[FinalLine.points.Count - 1].easting - Dy, FinalLine.points[FinalLine.points.Count - 1].northing - Dx));

                                break;
                            }
                            else
                            {
                                TotalDist += Distance;
                                FinalLine.points.Add(EntryLine[i]);
                            }
                        }

                        totalUTurnLength = 0;
                        isOutOfBounds = false;
                        for (int j = 0; j + 2 < FinalLine.points.Count; j++)
                        {
                            totalUTurnLength += glm.Distance(FinalLine.points[j], FinalLine.points[j + 1]);

                            //if (mf.bnd.IsPointInsideTurnArea(FinalLine[j]) != 0)
                            //{
                                //isOutOfBounds = true;
                                //break;
                            //}
                        }
                    }
                    ytList = FinalLine;
                    youTurnPhase = 255;
                    return;
                }
                else
                {
                    if (youTurnPhase == 13)
                    {
                        youTurnPhase = 14;

                        currIdx = (isHeadingSameWay ? ExitPoint.crosssingIdx - 1 : ExitPoint.crosssingIdx + 1);
                        offsetIdx = (isHeadingSameWay ? EntryPoint.crosssingIdx - 1 : EntryPoint.crosssingIdx);

                        Dx = ExitPoint.northing - curList.points[currIdx].northing;
                        Dy = ExitPoint.easting - curList.points[currIdx].easting;
                        double Dx2 = EntryPoint.northing - OffsetList.points[offsetIdx].northing;
                        double Dy2 = EntryPoint.easting - OffsetList.points[offsetIdx].easting;

                        currHeading = Math.Atan2(Dy, Dx);
                        offsetHeading = Math.Atan2(Dy2, Dx2) + Math.PI;

                        Start = new vec2(ExitPoint.easting, ExitPoint.northing);
                        End = new vec2(EntryPoint.easting, EntryPoint.northing);

                        Offset = 5;

                        upDownCount = isHeadingSameWay ? -1 : 1;
                    }

                    //while (true)
                    {
                        ytList.points.Clear();

                        CDubins dubYouTurnPath = new CDubins(mf.vehicle.minTurningRadius);
                        ytList.points = dubYouTurnPath.GenerateDubins(Start, currHeading, End, offsetHeading);

                        //now stepback to add uturn length again? . . .
                        double TotalDist = 0;
                        for (int i = upDownCount > 0 == isHeadingSameWay ? (currIdx + (isHeadingSameWay ? -1 : 1)) : currIdx; i >= 0 && i < curList.points.Count; i += isHeadingSameWay ? -1 : 1)
                        {
                            double Distance = glm.Distance(ytList.points[0], curList.points[i]);
                            if (TotalDist + Distance > youTurnStartOffset)
                            {
                                Dx = (ytList.points[0].northing - curList.points[i].northing) / Distance * (youTurnStartOffset - TotalDist);
                                Dy = (ytList.points[0].easting - curList.points[i].easting) / Distance * (youTurnStartOffset - TotalDist);
                                ytList.points.Insert(0, new vec2(ytList.points[0].easting - Dy, ytList.points[0].northing - Dx));
                                break;
                            }
                            else
                            {
                                TotalDist += Distance;
                                ytList.points.Insert(0, curList.points[i]);
                            }
                        }

                        //now stepback to add uturn length again? . . .
                        TotalDist = 0;
                        for (int i = upDownCount > 0 == isHeadingSameWay ? (offsetIdx + (isHeadingSameWay ? -1 : 1)) : offsetIdx; i >= 0 && i < OffsetList.points.Count; i += isHeadingSameWay ? -1 : 1)
                        {
                            double Distance = glm.Distance(ytList.points[ytList.points.Count - 1], OffsetList.points[i]);
                            if (TotalDist + Distance > youTurnStartOffset)
                            {
                                Dx = (ytList.points[ytList.points.Count - 1].northing - OffsetList.points[i].northing) / Distance;
                                Dy = (ytList.points[ytList.points.Count - 1].easting - OffsetList.points[i].easting) / Distance;

                                ytList.points.Add(new vec2(ytList.points[ytList.points.Count - 1].easting - Dy * (youTurnStartOffset - TotalDist), ytList.points[ytList.points.Count - 1].northing - Dx * (youTurnStartOffset - TotalDist)));

                                ytList.points.Add(new vec2(ytList.points[ytList.points.Count - 1].easting - Dy, ytList.points[ytList.points.Count - 1].northing - Dx));

                                break;
                            }
                            else
                            {
                                TotalDist += Distance;
                                ytList.points.Add(OffsetList.points[i]);
                            }
                        }

                        bool isOutOfBounds2 = false;
                        for (int k = 0; k < ytList.points.Count; k += 2)
                        {
                            if (mf.bnd.IsPointInsideTurnArea(new vec2(ytList.points[k].easting, ytList.points[k].northing)) != 0)
                            {
                                isOutOfBounds2 = true;
                                break;
                            }
                        }
                        if (!isOutOfBounds2 && Offset == 5)
                        {
                            Offset = 0.5;
                            upDownCount = isHeadingSameWay ? 1 : -1;
                            offsetIdx += upDownCount;
                            currIdx += upDownCount;
                        }
                        else if (isOutOfBounds2 && Offset == 0.5)
                        {
                            Offset = 0.05;
                            upDownCount = isHeadingSameWay ? -1 : 1;
                            offsetIdx += upDownCount;
                            currIdx += upDownCount;
                        }
                        else if (!isOutOfBounds2 && Offset == 0.05)
                        {
                            totalUTurnLength = 0;
                            for (int k = 0; k + 2 < ytList.points.Count; k++)
                            {
                                totalUTurnLength += glm.Distance(ytList.points[k], ytList.points[k + 1]);
                            }

                            isOutOfBounds = false;
                            youTurnPhase = 254;
                            return;
                        }

                        double distSoFar = 0;

                        //cycle thru segments and keep adding lengths. check if start and break if so.
                        while (true)
                        {
                            if (currIdx == -1 || currIdx == curList.points.Count)
                            {
                                isTurnCreationNotCrossingError = true;
                                return;
                            }

                            double dx1 = Start.northing - curList.points[currIdx].northing;
                            double dy1 = Start.easting - curList.points[currIdx].easting;

                            currHeading = Math.Atan2(dy1, dx1) + (upDownCount > 0 == isHeadingSameWay ? Math.PI : 0);
                            double length1 = Math.Sqrt(dx1 * dx1 + dy1 * dy1);

                            //will we go too far?
                            if ((length1 + distSoFar) > Offset)
                            {
                                double factor = (Offset - distSoFar) / length1;

                                Start = new vec2(Start.easting - dy1 * factor, Start.northing - dx1 * factor);
                                break; //tempDist contains the full length of next segment
                            }
                            distSoFar += length1;
                            Start = new vec2(curList.points[currIdx].easting, curList.points[currIdx].northing);
                            currIdx += upDownCount;
                        }

                        distSoFar = 0;
                        while (true)
                        {
                            if (offsetIdx == -1 || offsetIdx == OffsetList.points.Count)
                            {
                                isTurnCreationNotCrossingError = true;
                                return;
                            }
                            double dx1 = End.northing - OffsetList.points[offsetIdx].northing;
                            double dy1 = End.easting - OffsetList.points[offsetIdx].easting;

                            offsetHeading = Math.Atan2(dy1, dx1) + (upDownCount > 0 == isHeadingSameWay ? 0 : Math.PI);
                            double length1 = Math.Sqrt(dx1 * dx1 + dy1 * dy1);

                            //will we go too far?
                            if ((length1 + distSoFar) > Offset)
                            {
                                double factor = (Offset - distSoFar) / length1;

                                End = new vec2(End.easting - dy1 * factor, End.northing - dx1 * factor);
                                break; //tempDist contains the full length of next segment
                            }
                            distSoFar += length1;
                            End = new vec2(OffsetList.points[offsetIdx].easting, OffsetList.points[offsetIdx].northing);
                            offsetIdx += upDownCount;
                        }

                        if (upDownCount > 0 == isHeadingSameWay)
                            isTurnCreationTooClose = false;

                        if (!isTurnCreationTooClose)
                        {
                            double distancePivotToTurnLine;
                            for (int k = 1; k < ytList.points.Count; k += 2)
                            {
                                distancePivotToTurnLine = glm.Distance(ytList.points[k], pivot);
                                if (distancePivotToTurnLine < youTurnStartOffset + 3)
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
            CompleteYouTurn();
            isTurnCreationTooClose = false;
            isTurnCreationNotCrossingError = false;
            TurnCreationWidthError = false;
        }

        public void ResetCreatedYouTurn()
        {
            youTurnPhase = -2;
            ytList.points.Clear();
            OffsetList.points.Clear();
        }

        public void BuildManualYouLateral(bool isTurnRight)
        {
            if (isHeadingSameWay == isTurnRight)
                howManyPathsAway += 1;
            else
                howManyPathsAway -= 1;
        }

        //build the points and path of youturn to be scaled and transformed
        public void BuildManualYouTurn(bool isTurnRight)
        {
            isYouTurnTriggered = true;
            SwapYouTurn = true;

            if (isHeadingSameWay == isTurnRight)
                howManyPathsAway += rowSkipsWidth;
            else
                howManyPathsAway -= rowSkipsWidth;

            if (curList.points.Count < 2) return;

            double rEastYT = rEast;
            double rNorthYT = rNorth;

            //get the distance from currently active AB line
            double Dx = curList.points[currentLocationIndexB].northing - curList.points[currentLocationIndexA].northing;
            double Dy = curList.points[currentLocationIndexB].easting - curList.points[currentLocationIndexA].easting;

            if (Math.Abs(Dy) < double.Epsilon && Math.Abs(Dx) < double.Epsilon) return;

            double Heading = Math.Atan2(Dy, Dx);

            isHeadingSameWay = !isHeadingSameWay;

            //grab the vehicle widths and offsets
            double turnOffset = (mf.tool.toolWidth - mf.tool.toolOverlap) * rowSkipsWidth + (isTurnRight ? mf.tool.toolOffset * 2.0 : -mf.tool.toolOffset * 2.0);

            CDubins dubYouTurnPath = new CDubins(mf.vehicle.minTurningRadius);

            //if its straight across it makes 2 loops instead so goal is a little lower then start
            if (isHeadingSameWay) Heading += 3.14;
            else Heading -= 0.01;

            //move the start forward 2 meters, this point is critical to formation of uturn
            rEastYT += (Math.Sin(Heading) * 4.0);
            rNorthYT += (Math.Cos(Heading) * 4.0);

            //now we have our start point
            var start = new vec2(rEastYT, rNorthYT);
            var goal = new vec2(0, 0);

            //now we go the other way to turn round
            double Heading2 = Heading - Math.PI;
            if (Heading2 < 0) Heading2 += glm.twoPI;

            //set up the goal point for Dubins
            if (isTurnRight)
            {
                goal.easting = rEastYT - (Math.Cos(-Heading2) * turnOffset);
                goal.northing = rNorthYT - (Math.Sin(-Heading2) * turnOffset);
            }
            else
            {
                goal.easting = rEastYT + (Math.Cos(-Heading2) * turnOffset);
                goal.northing = rNorthYT + (Math.Sin(-Heading2) * turnOffset);
            }

            //generate the turn points
            ytList.points = dubYouTurnPath.GenerateDubins(start, Heading, goal, Heading2);
        }
    }
}