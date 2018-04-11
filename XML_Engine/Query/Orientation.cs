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

            orientation = (BH.Engine.Geometry.Query.Angle(plane.Normal, BHG.Plane.XZ.Normal) * (180 / Math.PI));


            return orientation;
        }
    }
}




