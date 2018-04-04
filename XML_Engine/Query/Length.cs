//using System;
//using System.Collections.Generic;
//using BHG = BH.oM.Geometry;
//using BH.Engine.Geometry;
//using System.Linq;
//using BH.oM.Environmental.Elements;


//namespace BH.Engine.XML
//{

//    public static partial class Query
//    {
//        /***************************************************/
//        /**** Public Methods                            ****/
//        /***************************************************/

//        public static double Length(BHG.Polyline pline)
//        {
//            double length = 0;
//            List<BHG.Point> pts = BoundingCurve(pline).DiscontinuityPoints();
//            if (pts.Count == 4)
//                    length = Geometry.Query.Distance(pts[0], pts[1]);
            
//            return length;
//        }

//        /***************************************************/
//    }
//}




