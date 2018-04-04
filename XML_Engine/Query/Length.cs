using System;
using System.Collections.Generic;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;
using System.Linq;
using BH.oM.Environmental.Elements;


namespace BH.Engine.XML
{

    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static double Length(BHG.Polyline pline)
        {

            List<BHG.Point> pts = pline.DiscontinuityPoints();

            double length = pts.Last().Distance(pts.First());

            for (int i = 0; i < pts.Count - 1; i++)
            {
                double dist = pts[i].Distance(pts[i + 1]);
                length = dist > length ? dist : length;
            }

            double area = pline.Area();
            double width = area / length;

            return length;
        }

        /***************************************************/
    }
}




