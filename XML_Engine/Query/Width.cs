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

//        public static double Width(BHG.Polyline pline)
//        {
//            double width = 0;
//            List<BHG.Point> pts = BoundingCurve(pline).DiscontinuityPoints();
//            if (pts.Count == 4)
//                    width = Geometry.Query.Distance(pts[1], pts[2]);
            
//            return width;
//        }

//        /***************************************************/
//    }
//}




