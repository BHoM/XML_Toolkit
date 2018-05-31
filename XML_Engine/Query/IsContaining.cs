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

        //TODO: Move to Environment Engine

        public static bool IsContaining(this BHE.Space space, BHG.Point point)
        {
            bool result = false;
            List<BHE.BuildingElement> buildingElements = space.BuildingElements;
            List<BHG.Plane> planes = buildingElements.Select(x => x.BuildingElementGeometry.ICurve().IControlPoints().FitPlane()).ToList();
            List<BHG.Point> ctrPoints = buildingElements.SelectMany(x => x.BuildingElementGeometry.ICurve().IControlPoints()).ToList();
            BHG.BoundingBox bound = BH.Engine.Geometry.Query.Bounds(ctrPoints);

            if (!BH.Engine.Geometry.Query.IsContaining(bound, point))
                return false;

            //Get a lenght longer than the longest side in the bounding Box. Change to infinite line?
            double length = ((bound.Max - bound.Min).Length()) * 10;

            //We need to check one line that starts in the point and ends outside the bbox.
            BHG.Vector vector = new BHG.Vector() { X = 1, Y = 0, Z = 0 };
            BHG.Point endpoint = point.Translate(vector * length);
            BHG.Line line = new BHG.Line() { Start = point, End = endpoint };


            //Check intersections
            int counter = 0;
            List<BHG.Point> intersectPts = new List<BHG.Point>();

            for (int i = 0; i < planes.Count; i++)
            {
                if ((BH.Engine.Geometry.Query.PlaneIntersection(line, planes[i], false)) == null) //false for using infinite lines. 
                    continue;

                List<BHG.Point> intersectingPoints = new List<BHG.Point>();
                intersectingPoints.Add(BH.Engine.Geometry.Query.PlaneIntersection(line, planes[i]));
                BHG.Polyline pline = new BHG.Polyline() { ControlPoints = buildingElements[i].BuildingElementGeometry.ICurve().IControlPoints() };


                if (intersectingPoints != null && BH.Engine.Geometry.Query.IsContaining(pline, intersectingPoints, true, 1e-05))
                {
                    intersectPts.AddRange(intersectingPoints);
                    if (intersectPts.CullDuplicates().Count == intersectPts.Count()) //Check if the point already has been added to the list
                        counter++;
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

        //TODO: Move to Geometry Engine

        public static bool IsContaining(this BHG.Line line, BHG.Point pt)
        {
            double AB = line.Length();
            double AP = BH.Engine.Geometry.Query.Distance(pt, line.Start);
            double PB = BH.Engine.Geometry.Query.Distance(line.End, pt);

            if (AB == (AP + PB))
                return true;
            return false;

        }

        /***************************************************/

    }
}





