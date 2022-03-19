using System;

namespace AgOpenGPS
{
    public class CFieldData
    {
        private readonly FormGPS mf;

        //all the section area added up;
        public double workedAreaTotal;

        //just a cumulative tally based on distance and eq width.
        public double workedAreaTotalUser;

        //accumulated user distance
        public double distanceUser;

        public double overlapPercent = 0;

        //Outside area minus inner boundaries areas (m)
        public double areaBoundaryOuterLessInner;

        //used for overlap calcs - total done minus overlap
        public double actualAreaCovered;

        //Inner area of outer boundary(m)
        public double areaOuterBoundary;

        public string WorkedAreaRemainPercentage
        {
            get
            {
                if (areaBoundaryOuterLessInner > 10)
                {
                    return ((areaBoundaryOuterLessInner - workedAreaTotal) * 100 / areaBoundaryOuterLessInner).ToString("0.0") + "%";
                }
                else
                {
                    return "0.00%";
                }
            }
        }

        public string BoundaryArea
        {
            get
            {
                double userBoundaryArea = areaBoundaryOuterLessInner * mf.m2ToUser;
                return userBoundaryArea.ToString(userBoundaryArea < 9.995 ? "0.00" : "0.0") + mf.unitsHaAc;
            }
        }

        public string WorkedAreaRemain
        {
            get
            {
                return ((areaBoundaryOuterLessInner - workedAreaTotal) * mf.m2ToUser).ToString("0.00") + mf.unitsHaAc;
            }
        }

        public string WorkedArea
        {
            get
            {
                double userWorkedArea = workedAreaTotal * mf.m2ToUser;
                return userWorkedArea.ToString(userWorkedArea < 9.995 ? "0.00" : "0.0") + mf.unitsHaAc;
            }
        }


        public string TimeTillFinished
        {
            get
            {
                if (mf.pn.speed > 2 && mf.tool.toolWidth > 0)
                {
                    TimeSpan timeSpan = TimeSpan.FromHours(((areaBoundaryOuterLessInner - workedAreaTotal) * 0.0001
                        / (mf.tool.toolWidth * mf.pn.speed * 0.1)));
                    return timeSpan.Hours.ToString("00") + ":" + timeSpan.Minutes.ToString("00");
                }
                else return "\u221E Hrs";
            }
        }

        //constructor
        public CFieldData(FormGPS _f)
        {
            mf = _f;
            workedAreaTotal = 0;
            workedAreaTotalUser = 0;
        }

        public void UpdateFieldBoundaryGUIAreas()
        {
            if (mf.bnd.bndList.Count > 0)
            {
                areaOuterBoundary = mf.bnd.bndList[0].area;
                areaBoundaryOuterLessInner = areaOuterBoundary;

                for (int i = 1; i < mf.bnd.bndList.Count; i++)
                {
                    areaBoundaryOuterLessInner -= mf.bnd.bndList[i].area;
                }
            }
            else
            {
                areaOuterBoundary = 0;
                areaBoundaryOuterLessInner = 0;
            }
        }
    }
}