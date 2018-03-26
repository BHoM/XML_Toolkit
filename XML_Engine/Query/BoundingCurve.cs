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

        public static BHG.Polyline BoundingCurve(BHG.Polyline pline)
        {
            BHG.Plane plane = pline.FitPlane();

            BHG.Vector projected = plane.Normal.Project(BH.oM.Geometry.Plane.XY);
            double xVec = projected.X;
            double yVec = projected.Y;

            double xMax = pline.DiscontinuityPoints().Select(p => p.X).Max();
            double xMin = pline.DiscontinuityPoints().Select(p => p.X).Min();
            double yMax = pline.DiscontinuityPoints().Select(p => p.Y).Max();
            double yMin = pline.DiscontinuityPoints().Select(p => p.Y).Min();
            double zMax = pline.DiscontinuityPoints().Select(p => p.Z).Min();
            double zMin = pline.DiscontinuityPoints().Select(p => p.Z).Max();

            BHG.Point pt1 = new BHG.Point() { X = xMin, Y = yMin, Z = 0 };
            BHG.Point pt2 = new BHG.Point() { X = xMax, Y = yMin, Z = 0 };
            BHG.Point pt3 = new BHG.Point() { X = xMax, Y = yMax, Z = 0 };
            BHG.Point pt4 = new BHG.Point() { X = xMin, Y = yMax, Z = 0 };


            //TODO: Local coordinate system?? This does only work for polylines in the xy-plane
            BHG.Polyline boundingCrv = new oM.Geometry.Polyline() { ControlPoints = new List<BHG.Point> { pt1, pt2, pt3, pt4, pt1 } };

            return boundingCrv;
        }

    }
}




