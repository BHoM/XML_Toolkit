using System;
using System.Collections.Generic;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;
using System.Linq;
using BHE = BH.oM.Environmental.Elements;


namespace BH.Engine.XML
{

    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/
        //TODO: move to geometry Engine

        public static double Inclination(this BHG.Polyline pline)
        {
            double inclination;

            List<BHG.Point> pts = BH.Engine.Geometry.Query.DiscontinuityPoints(pline);
            BHG.Plane plane = BH.Engine.Geometry.Create.Plane(pts[0], pts[1], pts[2]);

            inclination = BH.Engine.Geometry.Query.Angle(plane.Normal, BHG.Plane.XY.Normal) * (180 / Math.PI);

            return inclination;
        }

    }
}




