using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public enum DrawType { LineStrip, Points, Tram, Triangles };

    public class Polyline
    {
        public List<vec2> points = new List<vec2>(128);
        public List<int> indexer = new List<int>();
        public bool loop;

        public void DrawPolyLine(DrawType type)
        {
            if (points.Count > 0)
            {
                if (type == DrawType.Triangles)
                {
                    GL.Begin(PrimitiveType.Triangles);
                    if (indexer.Count == 0)
                        indexer = points.TriangulatePolygon();

                    for (int i = 0; i < indexer.Count; i++)
                    {
                        GL.Vertex3(points[indexer[i]].easting, points[indexer[i]].northing, 0);
                    }

                    GL.End();
                }
                else
                {
                    if (type == DrawType.Points)
                        GL.Begin(PrimitiveType.Points);
                    else if (loop)
                        GL.Begin(PrimitiveType.LineLoop);
                    else
                        GL.Begin(PrimitiveType.LineStrip);

                    for (int i = 0; i < points.Count; i++)
                    {
                        GL.Vertex3(points[i].easting, points[i].northing, 0);
                    }
                    GL.End();
                }
            }
        }

        public void Clear()
        {
            points.Clear();
        }
    }

    public static class StaticClass
    {
        public static bool GetLineIntersection(vec2 PointAA, vec2 PointAB, vec2 PointBA, vec2 PointBB, out vec2 Crossing, out double TimeA, out double TimeB, bool Limit = false)
        {
            TimeA = -1;
            TimeB = -1;
            Crossing = new vec2();
            double denominator = (PointAB.northing - PointAA.northing) * (PointBB.easting - PointBA.easting) - (PointBB.northing - PointBA.northing) * (PointAB.easting - PointAA.easting);

            if (denominator < -0.0001 || denominator > 0.0001)
            {
                TimeA = ((PointBB.northing - PointBA.northing) * (PointAA.easting - PointBA.easting) - (PointAA.northing - PointBA.northing) * (PointBB.easting - PointBA.easting)) / denominator;

                if (Limit || (TimeA > 0.0 && TimeA < 1.0))
                {
                    TimeB = ((PointAB.northing - PointAA.northing) * (PointAA.easting - PointBA.easting) - (PointAA.northing - PointBA.northing) * (PointAB.easting - PointAA.easting)) / denominator;
                    if (Limit || (TimeB > 0.0 && TimeB < 1.0))
                    {
                        Crossing = PointAA + (PointAB - PointAA) * TimeA;
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            return false;
        }

        public static Polyline OffsetAndDissolvePolyline(this Polyline Poly, double width, int start = -1, int end = -1, bool add = true)
        {
            List<vec2> OffsetPoints = Poly.points.OffsetPolyline(width, true, 0, start, end, add);

            List<Polyline> Output = OffsetPoints.DissolvePolyLine();

            if (Output.Count > 0)
                return Output[0];
            else
                return new Polyline();
        }

        public static List<Polyline> OffsetAndDissolvePolyline(this Polyline Poly, bool _, double width, int start = -1, int end = -1, bool add = true)
        {
            List<vec2> OffsetPoints = Poly.points.OffsetPolyline(width, true, 0, start, end, add);

            List<Polyline> Output = OffsetPoints.DissolvePolyLine();

            return Output;
        }

        public static List<vec2> OffsetPolyline(this List<vec2> Points, double Distance, bool Loop, double AddHeader = 0, int Start = -1, int End = -1, bool Add = true)
        {
            List<vec2> OffsetPoints = new List<vec2>();
            if (Points.Count > 1)
            {
                int A, C;
                vec2 norm1, norm2;
                bool dd = Start > End;

                int stop = Add ? Points.Count - 1 : End;

                for (int B = Add || Start < 0 ? 0 : Start; B <= stop || dd; B++)
                {
                    if (B == Points.Count && dd)
                    {
                        dd = false;
                        B = -1;
                        continue;
                    }

                    if (Start == -1 || (Start > End && (B >= Start || B <= End)) || (Start < End && B >= Start && B <= End))
                    {
                        A = (B - 1).Clamp(Points.Count);
                        C = (B + 1).Clamp(Points.Count);

                        norm1 = (Points[A] - Points[B]).Normalize();
                        norm2 = (Points[C] - Points[B]).Normalize();

                        if ((!Loop && B == 0) || (!Add && B == Start))
                        {
                            double Easting = Points[B].easting + Distance * norm2.northing - AddHeader * norm2.easting;
                            double Northing = Points[B].northing - Distance * norm2.easting - AddHeader * norm2.northing;
                            OffsetPoints.Add(new vec2(Easting, Northing));
                        }
                        else if ((!Loop && B == Points.Count - 1) || (!Add && B == End))
                        {
                            double Easting = Points[B].easting - Distance * norm1.northing - AddHeader * norm1.easting;
                            double Northing = Points[B].northing + Distance * norm1.easting - AddHeader * norm1.northing;
                            OffsetPoints.Add(new vec2(Easting, Northing));
                        }
                        else if (Distance == 0)
                            OffsetPoints.Add(Points[B]);
                        else
                        {
                            vec2 start1 = new vec2(Points[A].easting - Distance * norm1.northing, Points[A].northing + Distance * norm1.easting);
                            vec2 end1 = new vec2(Points[B].easting - Distance * norm1.northing, Points[B].northing + Distance * norm1.easting);
                            vec2 start2 = new vec2(Points[C].easting + Distance * norm2.northing, Points[C].northing - Distance * norm2.easting);
                            vec2 end2 = new vec2(Points[B].easting + Distance * norm2.northing, Points[B].northing - Distance * norm2.easting);

                            double sinA = norm1.Cross(norm2);
                            if (GetLineIntersection(start1, end1, start2, end2, out vec2 Crossing, out double Time, out double Time2, true))
                            {
                                if (Time > 0.0 && Time < 1.0 && Time2 > 0.0 && Time2 < 1.0)
                                    OffsetPoints.Add(Crossing);
                                else if (!Loop || (Distance > 0) == (sinA > 0.0))
                                    OffsetPoints.Add(Crossing);
                                else
                                {
                                    OffsetPoints.Add(end1);
                                    OffsetPoints.Add(end2);
                                }
                            }
                        }
                    }
                    else if (Add)
                        OffsetPoints.Add(Points[B]);
                    else if (B == End)
                        break;
                }
            }
            return OffsetPoints;
        }

        public class VertexPoint
        {
            public vec2 Coords;
            public VertexPoint Next;
            public VertexPoint Crossing;
            public double Time = -1;

            public VertexPoint(vec2 coords)
            {
                Coords = coords;
            }
        }

        public class VertexData
        {
            public double Area;
            public bool global = false;
            public int winding = 1;
            public VertexPoint Point;

            public VertexData(VertexPoint _startPoint)
            {
                Point = _startPoint;
            }
        }

        public static List<Polyline> DissolvePolyLine(this List<vec2> Points)
        {
            if (Points.Count < 2) return new List<Polyline>();

            VertexPoint First = new VertexPoint(Points[0]);
            VertexPoint CurrentVertex = First;

            for (int i = 1; i < Points.Count; i++)
            {
                CurrentVertex.Next = new VertexPoint(Points[i]);
                CurrentVertex = CurrentVertex.Next;
            }
            CurrentVertex.Next = First;

            CurrentVertex = First;
            VertexPoint StopVertex = CurrentVertex;

            int IntersectionCount = 0;

            int TotalCount = Points.Count;
            int safety = 0;

            bool start = true;
            while (true)
            {
                if (!start && CurrentVertex == StopVertex) break;
                start = false;

                VertexPoint SecondVertex = CurrentVertex.Next;

                List<VertexPoint> Crossings2 = new List<VertexPoint>();
                int safety2 = 0;
                while (true)
                {
                    if (SecondVertex == StopVertex) break;

                    if (GetLineIntersection(CurrentVertex.Coords, CurrentVertex.Next.Coords, SecondVertex.Coords, SecondVertex.Next.Coords, out vec2 intersectionPoint2D, out double Time, out _))
                    {
                        VertexPoint aa = new VertexPoint(intersectionPoint2D)
                        {
                            Crossing = CurrentVertex,
                            Next = SecondVertex,
                            Time = Time
                        };
                        Crossings2.Add(aa);

                        IntersectionCount++;
                    }
                    SecondVertex = SecondVertex.Next;

                    if (++safety2 - safety > TotalCount) break;
                }
                CurrentVertex = CurrentVertex.Next;

                Crossings2.Sort((x, y) => y.Time.CompareTo(x.Time));
                for (int j = 0; j < Crossings2.Count; j++)
                {
                    VertexPoint AA = new VertexPoint(Crossings2[j].Coords);
                    VertexPoint BB = new VertexPoint(Crossings2[j].Coords);

                    AA.Next = Crossings2[j].Crossing.Next;
                    Crossings2[j].Crossing.Next = AA;

                    BB.Next = Crossings2[j].Next.Next;
                    Crossings2[j].Next.Next = BB;

                    AA.Crossing = BB;
                    BB.Crossing = AA;
                }

                if (++safety > TotalCount + IntersectionCount) break;
            }

            TotalCount += IntersectionCount * 2;

            List<VertexData> Polygons = new List<VertexData>();

            if (IntersectionCount > 0)
            {
                List<VertexPoint> Crossings = new List<VertexPoint> { First };

                while (Crossings.Count > 0)
                {
                    CurrentVertex = StopVertex = Crossings[0];
                    Polygons.Add(new VertexData(CurrentVertex));

                    start = true;
                    Crossings.RemoveAt(0);
                    safety = 0;

                    while (true)
                    {
                        if (!start && CurrentVertex == StopVertex)
                            break;
                        start = false;

                        if (CurrentVertex.Crossing != null)
                        {
                            Crossings.Add(CurrentVertex.Crossing);

                            VertexPoint CC = CurrentVertex.Crossing.Next;
                            CurrentVertex.Crossing.Next = CurrentVertex.Next;
                            CurrentVertex.Next = CC;

                            CurrentVertex.Crossing.Crossing = null;
                            CurrentVertex.Crossing = null;
                        }
                        CurrentVertex = CurrentVertex.Next;
                        if (++safety > TotalCount) break;
                    }
                }
            }
            else Polygons.Add(new VertexData(First));


            for (int i = 0; i < Polygons.Count; i++)
            {
                Polygons[i].Area = VertexArea(Polygons[i].Point, TotalCount);

                if (Polygons[i].Area < 0)
                    Polygons[i].Area = -Polygons[i].Area;

                if (Polygons[i].Area >= 25)
                {
                    Polygons[i].winding = IsVertexOrientedClockwise(Polygons[i].Point, TotalCount, Points);

                    if (Polygons[i].winding != 1)
                    {
                        if (Polygons[i].Point.Crossing != null)
                            Polygons[i].Point.Crossing.Crossing = null;

                        Polygons.RemoveAt(i);
                        i--;
                        continue;
                    }
                }
                else
                {
                    if (Polygons[i].Point.Crossing != null)
                    {
                        Polygons[i].Point.Crossing.Crossing = null;
                    }
                    Polygons.RemoveAt(i);
                    i--;
                    continue;
                }
            }

            Polygons.Sort((x, y) => y.CompareTo(x));

            for (int i = Polygons.Count - 1; i >= 0; i--)
            {
                if (Polygons[i] == null)
                    Polygons.RemoveAt(i);
            }

            List<Polyline> FinalPolyLine = new List<Polyline>();

            for (int I = 0; I < Polygons.Count; I++)
            {
                if (Polygons[I].winding == 1 && Polygons[I].Area > 0)
                {
                    FinalPolyLine.Add(new Polyline());
                    FinalPolyLine[FinalPolyLine.Count - 1].loop = true;
                    start = true;
                    StopVertex = CurrentVertex = Polygons[I].Point;

                    safety = 0;
                    while (true)
                    {
                        if (!start && CurrentVertex == StopVertex)
                            break;
                        start = false;

                        FinalPolyLine[FinalPolyLine.Count - 1].points.Add(CurrentVertex.Coords);
                        CurrentVertex = CurrentVertex.Next;
                        if (safety++ > TotalCount) break;
                    }
                }
            }

            return FinalPolyLine;
        }

        public static int CompareTo(this VertexData vertex, VertexData vertex2)
        {
            int idx = 0;
            if (vertex == null && vertex2 == null)
                return 0;
            else if (vertex == null)
                return -3;
            else if (vertex2 == null)
                return 3;

            if (vertex.Area > vertex2.Area)
                idx++;
            else
                idx--;

            if (vertex.winding == 1 && vertex2.winding == 1)
            {
            }
            else if (vertex.winding == 1)
                idx++;
            else
                idx--;

            return idx;
        }

        public static int IsVertexOrientedClockwise(VertexPoint CurrentVertex, int TotalCount, List<vec2> Points)
        {
            int winding = -255;

            bool inside;

            VertexPoint StopVertex = CurrentVertex;

            bool start = true;
            int safety = 0;

            while (true)
            {
                if (!start && CurrentVertex == StopVertex)
                    break;
                start = false;

                if (!IsTriangleOrientedClockwise(CurrentVertex.Coords, CurrentVertex.Next.Coords, CurrentVertex.Next.Next.Coords))
                {
                    inside = false;

                    for (int l = 0; l < Points.Count; l++)
                    {
                        if (IsPointInTriangle(CurrentVertex.Coords, CurrentVertex.Next.Coords, CurrentVertex.Next.Next.Coords, Points[l]))
                        {
                            inside = true;
                            break;
                        }
                    }

                    if (!inside)
                    {
                        int winding_number = 0;

                        double a = (CurrentVertex.Coords.northing + CurrentVertex.Next.Coords.northing + CurrentVertex.Next.Next.Coords.northing) / 3.0;
                        double b = (CurrentVertex.Coords.easting + CurrentVertex.Next.Coords.easting + CurrentVertex.Next.Next.Coords.easting) / 3.0;

                        vec2 test3 = new vec2(b, a);
                        int l = Points.Count - 1;
                        for (int m = 0; m < Points.Count; l = m++)
                        {
                            if (Points[l].easting <= test3.easting && Points[m].easting > test3.easting)
                            {
                                if ((Points[m].northing - Points[l].northing) * (test3.easting - Points[l].easting) -
                                (test3.northing - Points[l].northing) * (Points[m].easting - Points[l].easting) > 0)
                                {
                                    ++winding_number;
                                }
                            }
                            else if (Points[l].easting > test3.easting && Points[m].easting <= test3.easting)
                            {
                                if ((Points[m].northing - Points[l].northing) * (test3.easting - Points[l].easting) -
                                (test3.northing - Points[l].northing) * (Points[m].easting - Points[l].easting) < 0)
                                {
                                    --winding_number;
                                }
                            }
                        }
                        winding = winding_number;
                        break;
                    }
                }

                CurrentVertex = CurrentVertex.Next;
                if (safety++ > TotalCount) break;
            }
            return winding;
        }

        public static double VertexArea(VertexPoint CurrentVertex, int TotalCount)
        {
            VertexPoint StopVertex = CurrentVertex;

            double area = 0;
            bool start = true;
            int safety = 0;

            while (true)
            {
                if (!start && CurrentVertex == StopVertex)
                    break;
                start = false;

                area += (CurrentVertex.Coords.easting + CurrentVertex.Next.Coords.easting) * (CurrentVertex.Coords.northing - CurrentVertex.Next.Coords.northing);

                CurrentVertex = CurrentVertex.Next;
                if (safety++ > TotalCount) break;
            }

            return area;
        }

        public static List<Polyline> ClipPolyLine(this Polyline polyline, List<Polyline> clipPolylines, bool isClipOuter)
        {
            List<Polyline> FinalPolyLine = new List<Polyline>();

            int m = polyline.points.Count - 1;
            int isInside = -1;
            bool loop = false;
            int second = polyline.points.Count;

            if (polyline.loop)
            {
                isInside = 0;
                loop = true;
            }
            else
            {
                bool test = false;
                for (int x = 0; x < clipPolylines.Count; x++)
                    test |= clipPolylines[x].points.PointInPolygon(polyline.points[0]);

                if (test == isClipOuter)
                {
                    FinalPolyLine.Add(new Polyline());
                    isInside = 1;
                }
                else
                    isInside = -1;
            }

            for (int l = 0; l <= second || loop; m = l++)
            {
                if (l == polyline.points.Count)
                {
                    if (second == polyline.points.Count) break;
                    loop = false;
                    l = -1;
                    continue;
                }
                if (m < 0) m = polyline.points.Count - 1;


                if (polyline.loop || l > 0)
                {
                    List<vec3> Crossings2 = new List<vec3>();
                    for (int i = 0; i < clipPolylines.Count; i++)
                    {
                        int j = clipPolylines[i].points.Count - 1;
                        for (int k = 0; k < clipPolylines[i].points.Count; j = k++)
                        {
                            if (GetLineIntersection(polyline.points[m], polyline.points[l], clipPolylines[i].points[j], clipPolylines[i].points[k], out vec2 Crossing, out double Time, out _))
                            {
                                Crossings2.Add(new vec3(Crossing.easting, Crossing.northing, Time));
                            }
                        }
                    }

                    if (Crossings2.Count > 0)
                    {
                        Crossings2.Sort((x, y) => x.heading.CompareTo(y.heading));

                        for (int k = 0; k < Crossings2.Count; k++)
                        {
                            if (isInside == 1)
                            {
                                FinalPolyLine[FinalPolyLine.Count - 1].points.Add(new vec2(Crossings2[k].easting, Crossings2[k].northing));
                            }

                            if (isInside == 0)
                            {
                                second = l;

                                bool test = false;
                                for (int x = 0; x < clipPolylines.Count; x++)
                                    test |= clipPolylines[x].points.PointInPolygon(polyline.points[l]);

                                if (test == isClipOuter)
                                    isInside = -1;
                                else
                                    isInside = 1;
                            }

                            if (isInside == -1)
                            {
                                isInside = 1;
                                FinalPolyLine.Add(new Polyline());
                                FinalPolyLine[FinalPolyLine.Count - 1].points.Add(new vec2(Crossings2[k].easting, Crossings2[k].northing));

                                FinalPolyLine[FinalPolyLine.Count - 1].loop = false;
                            }
                            else
                                isInside = -1;
                        }
                    }
                }
                if (isInside == 1)
                    FinalPolyLine[FinalPolyLine.Count - 1].points.Add(polyline.points[l]);
            }


            if (isInside == 0 && polyline.points.Count > 0)
            {
                bool test = false;
                for (int k = 0; k < clipPolylines.Count; k++)
                    test |= clipPolylines[k].points.PointInPolygon(polyline.points[0]);

                if (test == isClipOuter)
                    FinalPolyLine.Add(polyline);
            }

            return FinalPolyLine;
        }

        public static void LangSimplify(this List<vec2> PointList, double Tolerance)
        {
            int key = 0;
            int endP = PointList.Count - 1;
            while (key < PointList.Count)
            {
                if (key + 1 == endP)
                {
                    key++;
                    endP = PointList.Count - 1;
                    continue;
                }
                else
                {
                    double maxD = 0;
                    for (int i = key + 1; i < endP; i++)
                    {
                        double d = PointList[i].FindDistanceToSegment(PointList[key], PointList[endP]);
                        if (d > maxD)
                        {
                            maxD = d;
                            if (d > Tolerance)
                            {
                                break;
                            }
                        }
                    }

                    if (maxD > Tolerance)
                        endP--;
                    else
                    {
                        for (int i = endP - 1; i > key; i--)
                            PointList.RemoveAt(i);
                        key++;
                        endP = PointList.Count - 1;
                    }
                }
            }
        }

        public static double FindDistanceToSegment(this vec2 pt, vec2 p1, vec2 p2)
        {
            double dx = p2.northing - p1.northing;
            double dy = p2.easting - p1.easting;
            if ((dx == 0) && (dy == 0))
            {
                dx = pt.northing - p1.northing;
                dy = pt.easting - p1.easting;
                return Math.Sqrt(dx * dx + dy * dy);
            }
            double Time = ((pt.northing - p1.northing) * dx + (pt.easting - p1.easting) * dy) / (dx * dx + dy * dy);

            if (Time < 0)
            {
                dx = pt.northing - p1.northing;
                dy = pt.easting - p1.easting;
            }
            else if (Time > 1)
            {
                dx = pt.northing - p2.northing;
                dy = pt.easting - p2.easting;
            }
            else
            {
                dx = pt.northing - (p1.northing + Time * dx);
                dy = pt.easting - (p1.easting + Time * dy);
            }
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static double FindDistanceToSegment(this vec3 pt, vec2 p1, vec2 p2)
        {
            double dx = p2.northing - p1.northing;
            double dy = p2.easting - p1.easting;
            if (dx == 0 && dy == 0)
            {
                dx = pt.northing - p1.northing;
                dy = pt.easting - p1.easting;
                return Math.Sqrt(dx * dx + dy * dy);
            }

            double Time = ((pt.northing - p1.northing) * dx + (pt.easting - p1.easting) * dy) / (dx * dx + dy * dy);

            if (Time < 0)
            {
                dx = pt.northing - p1.northing;
                dy = pt.easting - p1.easting;
            }
            else if (Time > 1)
            {
                dx = pt.northing - p2.northing;
                dy = pt.easting - p2.easting;
            }
            else
            {
                dx = pt.northing - (p1.northing + Time * dx);
                dy = pt.easting - (p1.easting + Time * dy);
            }
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static double CalculateAverageHeadings(this List<vec2> curvePts)
        {
            double x = 0, y = 0, N, E;
            double Distance = 0;
            int i = 0;
            for (int j = 1; j < curvePts.Count; i = j++)
            {
                x += (N = curvePts[j].northing - curvePts[i].northing);
                y += (E = curvePts[j].easting - curvePts[i].easting);
                Distance += Math.Sqrt(Math.Pow(N, 2) + Math.Pow(E, 2));
            }
            x /= Distance;
            y /= Distance;

            return Math.Atan2(y, x);
        }

        public static int Clamp(this int Idx, int Size)
        {
            return (Size + Idx) % Size;
        }

        public static bool PointInPolygon(this List<vec2> Polygon, vec2 pointAA)
        {
            vec2 PointAB = new vec2(0.0, 200000.0);

            int NumCrossings = 0;

            for (int i = 0; i < Polygon.Count; i++)
            {
                vec2 PointBB = Polygon[(i + 1).Clamp(Polygon.Count)];

                if (GetLineIntersection(pointAA, PointAB, Polygon[i], PointBB, out _, out _, out _))
                    NumCrossings += 1;
            }
            return NumCrossings % 2 == 1;
        }

        public static List<int> TriangulatePolygon(this List<vec2> Points)
        {
            List<int> Indexer = new List<int>();
            if (Points.Count < 3) return Indexer;
            List<int> OldIdx = new List<int>();

            for (int i = 0; i < Points.Count; i++)
            {
                OldIdx.Add(i);
            }
            bool test = true;
            int j = 0;
            while (OldIdx.Count > 3)
            {
                if (j >= OldIdx.Count)
                {
                    if (test)
                    {
                        test = false;
                        j = 0;
                    }
                    else break; //only happens on self crossing polygons!
                }

                int i = j < 1 ? OldIdx.Count - 1 : j - 1;
                int k = j >= OldIdx.Count - 1 ? 0 : j + 1;

                if (IsEar(Points[OldIdx[i]], Points[OldIdx[j]], Points[OldIdx[k]], Points))
                {
                    test = true;
                    Indexer.Add(OldIdx[j]);
                    Indexer.Add(OldIdx[i]);
                    Indexer.Add(OldIdx[k]);

                    OldIdx.RemoveAt(j);
                }
                else j++;
            }

            if (IsEar(Points[OldIdx[0]], Points[OldIdx[1]], Points[OldIdx[2]], Points))
            {
                Indexer.Add(OldIdx[1]);
                Indexer.Add(OldIdx[0]);
                Indexer.Add(OldIdx[2]);
            }

            return Indexer;
        }

        public static bool IsTriangleOrientedClockwise(vec2 p1, vec2 p2, vec2 p3)
        {
            return p1.northing * p2.easting + p3.northing * p1.easting + p2.northing * p3.easting - p1.northing * p3.easting - p3.northing * p2.easting - p2.northing * p1.easting <= 0;
        }

        private static bool IsEar(vec2 PointA, vec2 PointB, vec2 PointC, List<vec2> vertices)
        {
            bool hasPointInside = IsTriangleOrientedClockwise(PointA, PointB, PointC);
            if (!hasPointInside)
            {
                for (int i = 0; i < vertices.Count; i++)
                {
                    if (IsPointInTriangle(PointA, PointB, PointC, vertices[i]))
                    {
                        hasPointInside = true;
                        break;
                    }
                }
            }
            return !hasPointInside;
        }

        public static bool IsPointInTriangle(vec2 p1, vec2 p2, vec2 p3, vec2 p)
        {
            double Denominator = ((p2.easting - p3.easting) * (p1.northing - p3.northing) + (p3.northing - p2.northing) * (p1.easting - p3.easting));
            double a = ((p2.easting - p3.easting) * (p.northing - p3.northing) + (p3.northing - p2.northing) * (p.easting - p3.easting)) / Denominator;

            if (a > 0.0 && a < 1.0)
            {
                double b = ((p3.easting - p1.easting) * (p.northing - p3.northing) + (p1.northing - p3.northing) * (p.easting - p3.easting)) / Denominator;
                if (b > 0.0 && b < 1.0)
                {
                    double c = 1 - a - b;
                    if (c > 0.0 && c < 1.0)
                        return true;
                }
            }
            return false;
        }
    }
}
