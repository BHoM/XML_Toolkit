using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.XML;
using BH.oM.Base;
using BHE = BH.oM.Environmental.Elements;
using BHP = BH.oM.Environmental.Properties;
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

        public static bool IsContaining(this BHE.Space space, BHG.Point point)
        {
            bool result = false;
            List<BHE.BuildingElement> buildingElements = space.BuildingElements;
            List<BHG.Plane> planes = buildingElements.Select(x => x.BuildingElementGeometry.ICurve().IControlPoints().FitPlane()).ToList();
            List<BHG.Point> ctrPoints = buildingElements.SelectMany(x => x.BuildingElementGeometry.ICurve().IControlPoints()).ToList();
            BHG.BoundingBox bound = BH.Engine.Geometry.Query.Bounds(ctrPoints);

            if (!BH.Engine.Geometry.Query.IsContaining(bound, point))
                return false;

            //Get a lenght longer than the longest side in the bounding Box:
            double length = ((bound.Max - bound.Min).Length()) * 2;

            //We need to check one line that starts in the point and ends outside the bbox.
            BHG.Vector vector = new BHG.Vector() { X = 1, Y = 0, Z = 0 };
            BHG.Point endpoint = point.Translate(vector * length);
            BHG.Line line = new BHG.Line() { Start = point, End = endpoint };


            //Check intersections
            int counter = 0;

            for (int i = 0; i < planes.Count; i++)
            {
                if ((BH.Engine.Geometry.Query.PlaneIntersection(line, planes[i])) == null)
                    continue;

                List<BHG.Point> intersectingPoints = new List<BHG.Point>();
                intersectingPoints.Add(BH.Engine.Geometry.Query.PlaneIntersection(line, planes[i]));
                BHG.Polyline pline = new BHG.Polyline() { ControlPoints = buildingElements[i].BuildingElementGeometry.ICurve().IControlPoints() };

                try
                {
                    if (intersectingPoints != null && BH.Engine.Geometry.Query.IsContaining(pline, intersectingPoints))
                        counter++;
                }
                catch
                {

                }
            }

            //If the number of intersections is odd the point is inside the space. 
            if (counter % 2 == 0)
                result = false;
            else
                result = true;
            return result;
        }
        /***************************************************/
    }
}





