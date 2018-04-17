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

        public static bool NormalTowardsSpace(this BHE.BuildingElement buildingElement, BHE.Space space)
        {
            BHG.Polyline pline = new BHG.Polyline() { ControlPoints = buildingElement.BuildingElementGeometry.ICurve().IControlPoints() };
            BHG.Point centrePt = pline.Centre();

            List<BHG.Point> pts = BH.Engine.Geometry.Query.DiscontinuityPoints(pline);
            BHG.Plane plane = BH.Engine.Geometry.Create.Plane(pts[0], pts[1], pts[2]);

            //The polyline can be locally concave. Check if the polyline is clockwise.
            if (!BH.Engine.Geometry.Query.IsClockwise(pline, plane.Normal))
                plane.Normal = -plane.Normal;

            //Move centrepoint along the normal. If inside - flip the panel
            if (IsContaining(space, centrePt.Translate(plane.Normal)))
                return true;
            else
                return false;

            /***************************************************/
        }

        public static bool NormalTowardsSpace(this BHG.Polyline pline, BHE.Space space)
        {
            BHG.Point centrePt = pline.Centre();

            List<BHG.Point> pts = BH.Engine.Geometry.Query.DiscontinuityPoints(pline);
            BHG.Plane plane = BH.Engine.Geometry.Create.Plane(pts[0], pts[1], pts[2]);

            //The polyline can be locally concave. Check if the polyline is clockwise.
            if (!BH.Engine.Geometry.Query.IsClockwise(pline, plane.Normal))
                plane.Normal = -plane.Normal;

            //Move centrepoint along the normal. If inside - flip the panel
            if (IsContaining(space, centrePt.Translate(plane.Normal)))
                return true;
            else
                return false;

            /***************************************************/
        }
    }
}




