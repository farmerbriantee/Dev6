using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public enum DrawType { LineStrip, LineLoop, Points, Tram, Triangles, TriangleStrip };

    public class Polyline
    {
        public List<vec2> points = new List<vec2>();
        public bool loop;
    }

    public class Polyline2 : Polyline
    {
        public bool ResetPoints, ResetIndexer;
        public int BufferPoints = int.MinValue, BufferIndex = int.MinValue, BufferPointsCnt = 0, BufferIndexCnt = 0;

        public void DrawPolyLine(DrawType type)
        {
            if (points.Count > 0)
            {
                if (type == DrawType.Tram)
                {
                    if (BufferPoints != int.MinValue)
                    {
                        GL.BindBuffer(BufferTarget.ArrayBuffer, BufferPoints);
                        GL.VertexPointer(2, VertexPointerType.Double, 16, IntPtr.Zero);
                        GL.EnableClientState(ArrayCap.VertexArray);
                        if (loop)
                            GL.DrawArrays(PrimitiveType.LineLoop, 0, BufferPointsCnt);
                        else
                            GL.DrawArrays(PrimitiveType.LineStrip, 0, BufferPointsCnt);
                    }

                    if (BufferIndex != int.MinValue)
                    {
                        GL.BindBuffer(BufferTarget.ArrayBuffer, BufferIndex);
                        GL.VertexPointer(2, VertexPointerType.Double, 16, IntPtr.Zero);
                        GL.EnableClientState(ArrayCap.VertexArray);
                        if (loop)
                            GL.DrawArrays(PrimitiveType.LineLoop, 0, BufferIndexCnt);
                        else
                            GL.DrawArrays(PrimitiveType.LineStrip, 0, BufferIndexCnt);
                    }
                }
                else
                {
                    if (type == DrawType.Triangles && (BufferIndex == int.MinValue || ResetIndexer))
                    {
                        List<int> Indexer = points.TriangulatePolygon();

                        if (BufferIndex == int.MinValue) GL.GenBuffers(1, out BufferIndex);
                        GL.BindBuffer(BufferTarget.ElementArrayBuffer, BufferIndex);
                        GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(Indexer.Count * 4), Indexer.ToArray(), BufferUsageHint.StaticDraw);

                        BufferIndexCnt = Indexer.Count;
                        ResetIndexer = false;
                    }
                    else if (BufferPoints == int.MinValue || ResetPoints)
                    {
                        if (BufferPoints == int.MinValue) GL.GenBuffers(1, out BufferPoints);
                        GL.BindBuffer(BufferTarget.ArrayBuffer, BufferPoints);
                        GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(points.Count * 16), points.ToArray(), BufferUsageHint.StaticDraw);
                        BufferPointsCnt = points.Count;
                        ResetPoints = false;
                    }

                    GL.BindBuffer(BufferTarget.ArrayBuffer, BufferPoints);
                    GL.VertexPointer(2, VertexPointerType.Double, 0, IntPtr.Zero);
                    GL.EnableClientState(ArrayCap.VertexArray);

                    if (type == DrawType.Triangles)
                    {
                        GL.BindBuffer(BufferTarget.ElementArrayBuffer, BufferIndex);
                        GL.DrawElements(PrimitiveType.Triangles, BufferIndexCnt, DrawElementsType.UnsignedInt, IntPtr.Zero);
                    }
                    else if (type == DrawType.TriangleStrip)
                    {
                        GL.DrawArrays(PrimitiveType.TriangleStrip, 2, BufferPointsCnt - 2);
                    }
                    else if (type == DrawType.Points)
                        GL.DrawArrays(PrimitiveType.Points, 0, BufferPointsCnt);
                    else if (loop || type == DrawType.LineLoop)
                        GL.DrawArrays(PrimitiveType.LineLoop, 0, BufferPointsCnt);
                    else
                        GL.DrawArrays(PrimitiveType.LineStrip, 0, BufferPointsCnt);
                }
            }
        }

        public void RemoveHandle()
        {
            if (BufferPoints != int.MinValue)
            {
                try
                {
                    if (GL.IsBuffer(BufferPoints))
                        GL.DeleteBuffer(BufferPoints);
                    BufferPoints = int.MinValue;
                }
                catch
                {
                }
            }
            if (BufferIndex != int.MinValue)
            {
                try
                {
                    if (GL.IsBuffer(BufferIndex))
                        GL.DeleteBuffer(BufferIndex);
                    BufferIndex = int.MinValue;
                }
                catch
                {
                }
            }
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

            if (denominator < -0.00000001 || denominator > 0.00000001)
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

        public static T OffsetAndDissolvePolyline<T>(this T Poly, double width, double header, int start = -1, int end = -1, bool add = true, double radius = 0) where T : Polyline, new()
        {
            List<vec2> OffsetPoints = Poly.OffsetPolyline(width, header, start, end, add);//, radius != 0);

            List<T> Output = OffsetPoints.DissolvePolyLine<T>(Poly.loop && add);

            if (Output.Count > 0)
            {
                //if (radius != 0)
                //    Output[0].points = Output[0].OffsetPolyline(width < 0 ? radius : -radius, 0, start, end, add, radius > 0);

                return Output[0];
            }
            else
                return new T();
        }

        public static List<T> OffsetAndDissolvePolyline<T>(this T Poly, bool _, double width, double header, int start = -1, int end = -1, bool add = true) where T : Polyline, new()
        {
            List<vec2> OffsetPoints = Poly.OffsetPolyline(width, header, start, end, add);

            List<T> Output = OffsetPoints.DissolvePolyLine<T>(Poly.loop);

            return Output;
        }

        public static List<vec2> OffsetPolyline<T>(this T Poly, double Distance, double AddHeader = 0, int Start = -1, int End = -1, bool Add = true, bool round = true) where T : Polyline, new()
        {
            List<vec2> OffsetPoints = new List<vec2>();
            if (Poly.points.Count > 1)
            {
                int A, C;
                vec2 norm1, norm2;
                bool dd = Start > End;

                int stop = !Add || Start > End ? End : Poly.points.Count - 1;

                for (int B = Add || Start < 0 ? 0 : Start; B <= stop || dd; B++)
                {
                    if (B == Poly.points.Count && dd)
                    {
                        dd = false;
                        B = -1;
                        continue;
                    }

                    if (Start == -1 || (Start > End ? (B >= Start || B <= End) : (B >= Start && B <= End)))
                    {
                        A = (B - 1).Clamp(Poly.points.Count);
                        C = (B + 1).Clamp(Poly.points.Count);

                        norm1 = (Poly.points[A] - Poly.points[B]).Normalize();
                        norm2 = (Poly.points[C] - Poly.points[B]).Normalize();

                        if ((!Poly.loop && B == 0) || (!Add && B == Start))
                        {
                            double Easting = Poly.points[B].easting + Distance * norm2.northing;
                            double Northing = Poly.points[B].northing - Distance * norm2.easting;

                            if (AddHeader != 0)
                                OffsetPoints.Add(new vec2(Easting - AddHeader * norm2.easting, Northing - AddHeader * norm2.northing));
                            OffsetPoints.Add(new vec2(Easting, Northing));
                        }
                        else if ((!Poly.loop && B == Poly.points.Count - 1) || (!Add && B == End))
                        {
                            double Easting = Poly.points[B].easting - Distance * norm1.northing;
                            double Northing = Poly.points[B].northing + Distance * norm1.easting;

                            OffsetPoints.Add(new vec2(Easting, Northing));
                            if (AddHeader != 0)
                                OffsetPoints.Add(new vec2(Easting - AddHeader * norm1.easting, Northing - AddHeader * norm1.northing));
                        }
                        else if (Distance == 0)
                            OffsetPoints.Add(Poly.points[B]);
                        else
                        {
                            vec2 start1 = new vec2(Poly.points[A].easting - Distance * norm1.northing, Poly.points[A].northing + Distance * norm1.easting);
                            vec2 end1 = new vec2(Poly.points[B].easting - Distance * norm1.northing, Poly.points[B].northing + Distance * norm1.easting);
                            vec2 start2 = new vec2(Poly.points[C].easting + Distance * norm2.northing, Poly.points[C].northing - Distance * norm2.easting);
                            vec2 end2 = new vec2(Poly.points[B].easting + Distance * norm2.northing, Poly.points[B].northing - Distance * norm2.easting);

                            double sinA = norm1.Cross(norm2);
                            if (GetLineIntersection(start1, end1, start2, end2, out vec2 Crossing, out double Time, out double Time2, true))
                            {
                                if (round && sinA > 0.0 == Distance > 0)
                                {
                                    double startAngle = Math.Atan2(end1.easting - Poly.points[B].easting, end1.northing - Poly.points[B].northing);
                                    double endAngle = Math.Atan2(end2.easting - Poly.points[B].easting, end2.northing - Poly.points[B].northing);

                                    if (startAngle < 0) startAngle += glm.twoPI;
                                    if (endAngle < 0) endAngle += glm.twoPI;

                                    double sweepAngle;

                                    if (((glm.twoPI - endAngle + startAngle) % glm.twoPI) < ((glm.twoPI - startAngle + endAngle) % glm.twoPI))
                                        sweepAngle = (glm.twoPI - endAngle + startAngle) % glm.twoPI;
                                    else
                                        sweepAngle = (glm.twoPI - startAngle + endAngle) % glm.twoPI;
                                    double Distance2 = Distance;

                                    int sign = Math.Sign(-sweepAngle);
                                    if (Distance < 0)
                                    {
                                        sign = -sign;
                                        Distance2 = -Distance2;
                                    }
                                    int pointsCount = (int)Math.Round(Math.Abs(sweepAngle / 0.0436332)) + 1;

                                    //ss based on radius!

                                    double degreeFactor = sweepAngle / pointsCount;

                                    for (int j = 0; j < pointsCount; ++j)
                                    {
                                        var pointX = Poly.points[B].northing + Math.Cos(startAngle + sign * (j + 1) * degreeFactor) * Distance2;
                                        var pointY = Poly.points[B].easting + Math.Sin(startAngle + sign * (j + 1) * degreeFactor) * Distance2;
                                        OffsetPoints.Add(new vec2(pointY, pointX));
                                    }
                                }
                                else if (Time > 0.0 && Time < 1.0 && Time2 > 0.0 && Time2 < 1.0)
                                    OffsetPoints.Add(Crossing);
                                else if ((Distance > 0) == (sinA > 0.0))
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
                        OffsetPoints.Add(Poly.points[B]);
                    else if (B == End)
                        break;
                }
            }
            return OffsetPoints;
        }

        public static vec2 GetProportionPoint(this vec2 point, double segment, double length, double dx, double dy)
        {
            double factor = segment / length;
            if (double.IsNaN(factor))
                factor = 0;
            return new vec2(point.easting - dy * factor, point.northing - dx * factor);
        }

        public static void CalculateRoundedCorner(this List<vec2> Points, double Radius, double Radius2, bool Loop, double MaxAngle, int Count1 = 0, int Count2 = int.MaxValue)
        {
            double tt = Math.Min(Math.Asin(0.5 / Radius), Math.Asin(0.5 / Radius2));
            if (!double.IsNaN(tt)) MaxAngle = Math.Min(tt, MaxAngle);

            vec2 tt3;
            int A, B, C, oldA, OldC;
            bool reverse = false;

            for (B = Count1; B < Points.Count && B < Count2; B++)
            {
                if (!Loop && (B == 0 || B + 1 == Points.Count)) continue;
                A = (B == 0) ? Points.Count - 1 : B - 1;
                C = (B + 1 == Points.Count) ? 0 : B + 1;
                bool stop = false;
                double dx1, dy1, dx2, dy2, angle, segment = 0, length1 = 0, length2 = 0;
                while (true)
                {
                    tt3 = Points[B];
                    if (GetLineIntersection(Points[A], Points[(A + 1).Clamp(Points.Count)], Points[C], Points[(C - 1).Clamp(Points.Count)], out vec2 Crossing, out double Time, out _, true))
                    {
                        if (Time > -100 && Time < 100)
                            tt3 = Crossing;
                    }

                    dx1 = tt3.northing - Points[A].northing;
                    dy1 = tt3.easting - Points[A].easting;
                    dx2 = tt3.northing - Points[C].northing;
                    dy2 = tt3.easting - Points[C].easting;

                    angle = Math.Atan2(dy1, dx1) - Math.Atan2(dy2, dx2);

                    if (angle < 0) angle += glm.twoPI;
                    if (angle > glm.twoPI) angle -= glm.twoPI;
                    angle /= 2;

                    double Tan = Math.Abs(Math.Tan(angle));

                    angle = Math.Abs(angle);

                    //if ((angle > glm.PIBy2 - MaxAngle && angle < glm.PIBy2 + MaxAngle) || (angle > Math.PI - MaxAngle && angle < Math.PI + MaxAngle) || (angle < MaxAngle))
                    {
                        //if (C - A > 2)//Check why this is somethimes wrong!
                        //{
                        //    while (C - 1 > A)
                        //    {
                        //        C = C == 0 ? Points.Count - 1 : C - 1;
                        //        Points.RemoveAt(C);
                        //    }
                        //}
                        //stop = true;

                        //if (Loop || A > 0) A = (A == 0) ? Points.Count - 1 : A - 1;
                        //if (Loop || C < Points.Count - 1) C = (C + 1 == Points.Count) ? 0 : C + 1;

                        //break;
                        //continue;
                    }

                    reverse = angle > glm.PIBy2;

                    segment = (reverse ? Radius2 : Radius) / Tan;

                    length1 = Math.Sqrt(dx1 * dx1 + dy1 * dy1);
                    length2 = Math.Sqrt(dx2 * dx2 + dy2 * dy2);
                    oldA = A;
                    OldC = C;
                    if (segment > length1)
                    {
                        if (Loop || A > 0) A = (A == 0) ? Points.Count - 1 : A - 1;

                        if (A == C)
                        {
                            stop = true;
                            break;
                        }
                    }
                    if (segment > length2)
                    {
                        if (Loop || C < Points.Count - 1) C = (C + 1 == Points.Count) ? 0 : C + 1;
                        if (C == A)
                        {
                            stop = true;
                            break;
                        }
                    }
                    else if (segment < length1)
                    {
                        Points[B] = tt3;
                        break;
                    }

                    if (oldA == A && OldC == C)
                    {
                        stop = true;
                        break;
                    }
                }
                if (stop) continue;

                bool Looping = (A > C);
                while (C - 1 > A || Looping)
                {
                    if (C == 0)
                    {
                        if (A == Points.Count - 1) break;
                        Looping = false;
                    }

                    C = C == 0 ? Points.Count - 1 : C - 1;

                    if (A > C) A--;

                    Points.RemoveAt(C);
                    if (Count2 != int.MaxValue)
                        Count2--;
                }

                B = A > B ? -1 : A;

                vec2 p1Cross = tt3.GetProportionPoint(segment, length1, dx1, dy1);
                vec2 p2Cross = tt3.GetProportionPoint(segment, length2, dx2, dy2);

                if (reverse)
                {
                    vec2 test = p1Cross;
                    p1Cross = p2Cross;
                    p2Cross = test;
                }

                double dx = tt3.northing * 2 - p1Cross.northing - p2Cross.northing;
                double dy = tt3.easting * 2 - p1Cross.easting - p2Cross.easting;
                if (dx1 == 0 && dy1 == 0 || dx2 == 0 && dy2 == 0 || dx == 0 && dy == 0) continue;

                double L = Math.Sqrt(dx * dx + dy * dy);
                double d = Math.Sqrt(segment * segment + (reverse ? Radius2 : Radius) * (reverse ? Radius2 : Radius));
                vec2 circlePoint = tt3.GetProportionPoint(d, L, dx, dy);

                double startAngle = Math.Atan2(p1Cross.easting - circlePoint.easting, p1Cross.northing - circlePoint.northing);
                double endAngle = Math.Atan2(p2Cross.easting - circlePoint.easting, p2Cross.northing - circlePoint.northing);

                if (startAngle < 0) startAngle += glm.twoPI;
                if (endAngle < 0) endAngle += glm.twoPI;


                double sweepAngle;

                if (((glm.twoPI - endAngle + startAngle) % glm.twoPI) < ((glm.twoPI - startAngle + endAngle) % glm.twoPI))
                    sweepAngle = (glm.twoPI - endAngle + startAngle) % glm.twoPI;
                else
                    sweepAngle = (glm.twoPI - startAngle + endAngle) % glm.twoPI;

                int sign = Math.Sign(sweepAngle);

                if (reverse)
                {
                    sign = -sign;
                    startAngle = endAngle;
                }

                int pointsCount = (int)Math.Round(Math.Abs(sweepAngle / MaxAngle));

                double degreeFactor = sweepAngle / pointsCount;

                vec2[] points = new vec2[pointsCount];

                for (int j = 0; j < pointsCount; ++j)
                {
                    var pointX = circlePoint.northing + Math.Cos(startAngle + sign * (j + 1) * degreeFactor) * (reverse ? Radius2 : Radius);
                    var pointY = circlePoint.easting + Math.Sin(startAngle + sign * (j + 1) * degreeFactor) * (reverse ? Radius2 : Radius);
                    points[j] = new vec2(pointY, pointX);
                }

                Points.InsertRange(B + 1, points);
                if (Count2 != int.MaxValue)
                    Count2 += pointsCount;
                B += points.Length;
            }
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

        public static List<T> DissolvePolyLine<T>(this List<vec2> Points, bool loop = true) where T : Polyline, new()
        {
            if (Points.Count < 2) return new List<T>();

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
                        if (!loop && SecondVertex.Next == StopVertex)
                            break;

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
                            if (loop)
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

            if (loop)
            {
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
            }

            List<T> FinalPolyLine = new List<T>();

            for (int I = 0; I < Polygons.Count; I++)
            {
                FinalPolyLine.Add(new T());
                FinalPolyLine[FinalPolyLine.Count - 1].loop = loop;
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

        public static List<T> ClipPolyLine<T>(this T polyline, List<T> clipPolylines, bool isClipOuter) where T : Polyline, new()
        {
            List<T> FinalPolyLine = new List<T>();

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
                    FinalPolyLine.Add(new T());
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
                                FinalPolyLine.Add(new T());
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
                        double d = PointList[i].FindDistanceToSegment(PointList[key], PointList[endP], out _);
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

        public static double FindDistanceToSegment(this vec2 pt, vec2 p1, vec2 p2, out double Time, bool signed = false, bool aa = true, bool bb = true)
        {
            double dx = p2.northing - p1.northing;
            double dy = p2.easting - p1.easting;

            if ((dx == 0) && (dy == 0))
            {
                Time = 0;
                dx = pt.northing - p1.northing;
                dy = pt.easting - p1.easting;
                return Math.Sqrt(dx * dx + dy * dy);
            }

            Time = ((pt.northing - p1.northing) * dx + (pt.easting - p1.easting) * dy) / (dx * dx + dy * dy);

            if (aa && Time < 0)
            {
                dx = pt.northing - p1.northing;
                dy = pt.easting - p1.easting;
            }
            else if (bb && Time > 1)
            {
                dx = pt.northing - p2.northing;
                dy = pt.easting - p2.easting;
            }
            else
            {
                dx = pt.northing - (p1.northing + Time * dx);
                dy = pt.easting - (p1.easting + Time * dy);
            }

            if (signed)
            {
                double sign = Math.Sign((p2.northing - p1.northing) * (pt.easting - p1.easting) - (p2.easting - p1.easting) * (pt.northing - p1.northing));

                return sign * Math.Sqrt(dx * dx + dy * dy);
            }
            else
                return Math.Sqrt(dx * dx + dy * dy);
        }

        public static void GetCurrentSegment(this vec2 Point, Polyline curList, out int AA, out int BB, int Start = 0, int End = int.MaxValue)
        {
            AA = -1;
            BB = -1;

            double minDistA = double.MaxValue;
            int A = Start > 0 ? Start - 1 : curList.points.Count - 1;
            for (int B = (Start > 0 ? Start : 0); B < curList.points.Count && B < End; A = B++)
            {
                if (B == 0 && !curList.loop)
                    continue;

                double dist2 = Point.FindDistanceToSegment(curList.points[A], curList.points[B], out _);

                if (dist2 < minDistA)
                {
                    minDistA = dist2;
                    AA = A;
                    BB = B;
                }
                else if (dist2 == minDistA && B > Start + 1)
                {
                    vec2 dd = (curList.points[A] - curList.points[A - 1]).Normalize();
                    vec2 ee = (curList.points[A] - curList.points[B]).Normalize();
                    vec2 tt = curList.points[A] + (dd + ee);

                    double sign = Math.Sign((tt.northing - curList.points[A].northing) * (Point.easting - curList.points[A].easting) - (tt.easting - curList.points[A].easting) * (Point.northing - curList.points[A].northing));

                    if (sign < 0)
                    {
                        minDistA = dist2;
                        AA = A;
                        BB = B;
                    }
                }
            }
            
            return;
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

        public static vec2[] SmoothAB(this List<vec2> points, int smPts)
        {
            //count the reference list of original curve
            int cnt = points.Count;

            //the temp array
            vec2[] arr = new vec2[cnt];

            //read the points before and after the setpoint
            for (int s = 0; s < smPts / 2; s++)
            {
                arr[s].easting = points[s].easting;
                arr[s].northing = points[s].northing;
            }

            for (int s = cnt - (smPts / 2); s < cnt; s++)
            {
                arr[s].easting = points[s].easting;
                arr[s].northing = points[s].northing;
            }

            //average them - center weighted average
            for (int i = smPts / 2; i < cnt - (smPts / 2); i++)
            {
                for (int j = -smPts / 2; j < smPts / 2; j++)
                {
                    arr[i].easting += points[j + i].easting;
                    arr[i].northing += points[j + i].northing;
                }
                arr[i].easting /= smPts;
                arr[i].northing /= smPts;
            }
            return arr;
        }

        public static void AddFirstLastPoints(this List<vec2> points, double length, double lineHeading, bool StraightLine = true)
        {
            if (points.Count > 1)
            {
                if (StraightLine)
                {
                    points.Add(new vec2(points[points.Count - 1].easting + (Math.Sin(lineHeading) * length), points[points.Count - 1].northing + (Math.Cos(lineHeading) * length)));
                    points.Insert(0, new vec2(points[0].easting - (Math.Sin(lineHeading) * length), points[0].northing - (Math.Cos(lineHeading) * length)));
                }
                else
                {
                    double x = 0, y = 0, TotalDistance = 0;
                    int i = 0;

                    for (int j = 1; j < points.Count; i = j++)
                    {
                        double N = points[j].northing - points[i].northing;
                        double E = points[j].easting - points[i].easting;
                        double Distance = Math.Sqrt(Math.Pow(N, 2) + Math.Pow(E, 2));

                        if (TotalDistance + Distance > lineHeading)
                        {
                            double U = (lineHeading - TotalDistance) / Distance; // the remainder to yet travel
                            TotalDistance += U * Distance;
                            x += U * N;
                            y += U * E;
                            break;
                        }
                        else
                        {
                            x += N;
                            y += E;
                            TotalDistance += Distance;
                        }
                    }
                    x /= TotalDistance;
                    y /= TotalDistance;

                    double head1 = Math.Atan2(y, x);
                    points.Insert(0, new vec2(points[0].easting - (Math.Sin(head1) * length), points[0].northing - (Math.Cos(head1) * length)));



                    x = y = TotalDistance = 0;
                    i = points.Count - 1;

                    for (int j = points.Count - 2; j > 0; i = j--)
                    {
                        double N = points[j].northing - points[i].northing;
                        double E = points[j].easting - points[i].easting;
                        double Distance = Math.Sqrt(Math.Pow(N, 2) + Math.Pow(E, 2));

                        if (TotalDistance + Distance > lineHeading)
                        {
                            double U = (lineHeading - TotalDistance) / Distance; // the remainder to yet travel
                            TotalDistance += U * Distance;
                            x += U * N;
                            y += U * E;
                            break;
                        }
                        else
                        {
                            x += N;
                            y += E;
                            TotalDistance += Distance;
                        }
                    }
                    x /= TotalDistance;
                    y /= TotalDistance;

                    double head2 = Math.Atan2(y, x);


                    points.Add(new vec2(points[points.Count - 1].easting - (Math.Sin(head2) * length), points[points.Count - 1].northing - (Math.Cos(head2) * length)));
                }
            }
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
