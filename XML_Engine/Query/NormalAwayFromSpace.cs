using System;
using System.Collections.Generic;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;
using System.Linq;
using BH.oM.Geometry;
using BHE = BH.oM.Environmental.Elements;
using BH.Engine.Environment;


namespace BH.Engine.XML
{

    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/
        //TODO: move to BHoM Engine

        public static bool NormalAwayFromSpace(this BHE.BuildingElement buildingElement, BHE.Space space)
        {
            BHG.Polyline bound = new BHG.Polyline() { ControlPoints = buildingElement.BuildingElementGeometry.ICurve().IControlPoints() };

            return NormalAwayFromSpace(bound, space);
        }

        /***************************************************/

        public static bool NormalAwayFromSpace(this BHG.Polyline pline, BHE.Space space)
        {
            List<BHG.Point> centrePtList = new List<Point>();
            BHG.Point centrePt = pline.Centre();
            centrePtList.Add(centrePt);

            //Make sure the centrepoint is in the polyline region. If not - use a new point
            //if (!BH.Engine.Geometry.Query.IsContaining(pline, centrePtList, false, BH.oM.Geometry.Tolerance.MicroDistance))
            //{
            //    BHG.Point pointOnPline = BH.Engine.Geometry.Query.ClosestPoint(pline, centrePt);
            //    BHG.Vector vector = new BHG.Vector();
            //    if (BH.Engine.Geometry.Query.Distance(pointOnPline, centrePt) > BH.oM.Geometry.Tolerance.MicroDistance)
            //        vector = pointOnPline - centrePt;
            //    else
            //        vector = pointOnPline.Translate(BHG.Vector.XAxis*0.0001) - centrePt; //?? Change this


            //    centrePt = BH.Engine.Geometry.Modify.Translate(pointOnPline, BH.Engine.Geometry.Modify.Normalise(vector) * 0.001);
            //}

            List<BHG.Point> pts = BH.Engine.Geometry.Query.DiscontinuityPoints(pline);
            BHG.Plane plane = BH.Engine.Geometry.Create.Plane(pts[0], pts[1], pts[2]);

            //The polyline can be locally concave. Check if the polyline is clockwise.
            if (!BH.Engine.Geometry.Query.IsClockwise(pline, plane.Normal))
                plane.Normal = -plane.Normal;

            if (!BH.Engine.Geometry.Query.IsContaining(pline, centrePtList, false, BH.oM.Geometry.Tolerance.MicroDistance))
            {
                BHG.Point pointOnPline = BH.Engine.Geometry.Query.ClosestPoint(pline, centrePt);
                BHG.Vector vector = new BHG.Vector();
                if (BH.Engine.Geometry.Query.Distance(pointOnPline, centrePt) > BH.oM.Geometry.Tolerance.MicroDistance)
                    vector = pointOnPline - centrePt;
                else
                {
                    BHG.Line line = XML.Query.GetLine(pline, pointOnPline);
                    vector = ((line.Start - line.End).Normalise()).CrossProduct(plane.Normal);
                }

                centrePt = BH.Engine.Geometry.Modify.Translate(pointOnPline, BH.Engine.Geometry.Modify.Normalise(vector) * 0.001);
            }


            //Move centrepoint along the normal. If inside - flip the panel
            if (IsContaining(space, centrePt.Translate(plane.Normal * 0.1)))
                return false;
            else
                return true;

            /***************************************************/
        }
    }
}




