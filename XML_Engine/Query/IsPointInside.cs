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

        public static bool IsPointInside(this BHG.Polyline polyline, BHG.Point point)
        {
            if (!polyline.IsClosed())
                throw new Exception("The polyline is not closed");

            BHG.Plane plane = polyline.FitPlane();

            if (!point.IsInPlane(plane))
                return false;

            List<BHG.Point> ctrlPoints = polyline.ControlPoints;

            bool result = false;
            int j = ctrlPoints.Count() - 1;
            for (int i = 0; i < ctrlPoints.Count(); i++)
            {
                if (ctrlPoints[i].Y < point.Y && ctrlPoints[j].Y >= point.Y || ctrlPoints[j].Y < point.Y && ctrlPoints[i].Y >= point.Y)
                {
                    if (ctrlPoints[i].X + (point.Y - ctrlPoints[i].Y) / (ctrlPoints[j].Y - ctrlPoints[i].Y) * (ctrlPoints[j].X - ctrlPoints[i].X) < point.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }

        /***************************************************/

        public static bool IsPointInside(this BHE.Space space, BHG.Point point)
        {
            List<BHE.BuildingElement> buildingElements = space.BuildingElements;
            //List<BHG.Plane> planes = buildingElements.Select(x => x.BuildingElementGeometry.ICurve().IControlPoints().FitPlane()).ToList();
            List<BHG.Point> intersectiongPoints = new List<BHG.Point>();
            BHG.BoundingBox bound = BH.Engine.Geometry.Query.Bounds(buildingElements.Select(x => x.BuildingElementGeometry.ICurve().IControlPoints()));


            bool result = false;

            foreach (BHE.BuildingElement element in buildingElements)
            {
                List<BHG.Point> vertices = element.BuildingElementGeometry.ICurve().IControlPoints();
                BHG.Point projectedPt = point.Project(vertices.FitPlane());
                BHG.Vector vector = (projectedPt - point).Normalise();

                //Project along the vector and determine wheter the ray will intersect with another building element
                for (int i = 0; i < buildingElements.Count; i++)
                {
                    List<BHG.Point> pt = Geometry.Query.ClosestPoint(;
                    BHG.Polyline pline = new BHG.Polyline() { ControlPoints = buildingElements[i].BuildingElementGeometry.ICurve().IControlPoints() };
                    if (BH.Engine.Geometry.Query.IsContaining(pline, pt))
                        intersectiongPoints.AddRange(pt);
                }

            }

            //If the number of intersections is odd the point is inside the space. 
            if (intersectiongPoints.Count % 2 == 0)
                result = false;
            else
                result = true;
            
            return result;
        }
        /***************************************************/
    }
}





