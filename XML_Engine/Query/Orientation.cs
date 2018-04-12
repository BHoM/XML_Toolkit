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

        public static double Orientation(this BHG.Polyline pline)
        {
            double orientation;

            List<BHG.Point> pts = BH.Engine.Geometry.Query.DiscontinuityPoints(pline);
            BHG.Plane plane = BH.Engine.Geometry.Create.Plane(pts[0], pts[1], pts[2]);

            if (Geometry.Modify.Normalise(plane.Normal).Z == 1)
                orientation = 0;
            else if (Geometry.Modify.Normalise(plane.Normal).Z == -1)
                orientation = 180;
            else
                orientation = (BH.Engine.Geometry.Query.Angle(Geometry.Modify.Project(plane.Normal, BHG.Plane.XY), BHG.Plane.XZ.Normal) * (180 / Math.PI));

            return orientation;
        }
    }
}




