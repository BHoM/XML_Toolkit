using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.XML;
using BH.oM.Base;
using BHE = BH.oM.Environment.Elements;
using BHP = BH.oM.Environment.Properties;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.Engine.Environment;


namespace BH.Engine.XML
{

    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        //TODO: move convex hull to Geomtry Engine 
        //TODO: Does only work for points in the XY plane. Add plane as input?

        public static BHG.Point nextHullPoint(List<BHG.Point> points, BHG.Point currPt)
        {

            int right = -1;
            int none = 0;

            BHG.Point nextPt = currPt;
            int t;
            foreach (BHG.Point pt in points)
            {
                t = ((nextPt.X - currPt.X) * (pt.Y - currPt.Y) - (pt.X - currPt.X) * (nextPt.Y - currPt.Y)).CompareTo(0);
                if (t == right || t == none && Geometry.Query.Distance(currPt, pt) > Geometry.Query.Distance(currPt, nextPt))
                    nextPt = pt;
            }
            return nextPt;
        }

        /***************************************************/

        public static BHG.Polyline convexHull(List<BHG.Point> points)
        {
            List<BHG.Point> hull = new List<BHG.Point>();
            foreach (BHG.Point p in points)
            {
                if (hull.Count == 0)
                    hull.Add(p);
                else
                {
                    if (hull[0].X > p.X)
                        hull[0] = p;
                    else if (hull[0].X == p.X)
                        if (hull[0].Y > p.Y)
                            hull[0] = p;
                }
            }


            BHG.Point nextPt = new BHG.Point();
            int counter = 0;
            while (counter < hull.Count)
            {
                nextPt = nextHullPoint(points, hull[counter]);
                if (nextPt != hull[0])
                    hull.Add(nextPt);
                counter++;
            }

            hull.Add(hull[0]);

            BH.oM.Geometry.Polyline hullBoundary = new BHG.Polyline() { ControlPoints = hull };
            return hullBoundary;
        }

        /***************************************************/

    }
}




