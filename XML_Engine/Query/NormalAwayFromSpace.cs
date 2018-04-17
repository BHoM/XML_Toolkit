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

        //public static bool NormalAwayFromFirstSpace(BHE.BuildingElement bHoMBuildingElement, List<BHE.Space> bHoMSpaces)
        //{
        //    BHG.Polyline boundary = new BHG.Polyline() { ControlPoints = (bHoMBuildingElement.BuildingElementGeometry as BHE.BuildingElementPanel).PolyCurve.ControlPoints() }; //TODO: Change to ToPolyline method

        //    //Check if the surface normal is pointing away from the first AdjSpace
        //    if (bHoMBuildingElement.AdjacentSpaces.Count > 0)
        //    {
        //        Guid firstGuid = bHoMBuildingElement.AdjacentSpaces.First();
        //        BHE.Space firstSpace = bHoMSpaces.Find(x => x.BHoM_Guid == firstGuid);

        //        if (firstSpace == null)
        //           return false;
        //        else
        //        {
        //            if (!BH.Engine.Geometry.Query.IsClockwise(boundary, BH.Engine.Environment.Query.Centre(firstSpace)))
        //                return true;
        //            return false;
        //        }
        //    }
        //    else  //Shade elements
        //        return false;
        //}

        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static bool NormalAwayFromSpace(this BHE.BuildingElement buildingElement, BHE.Space space)
        {
            BHG.Polyline bound = new BHG.Polyline() { ControlPoints = buildingElement.BuildingElementGeometry.ICurve().IControlPoints() };

            return NormalAwayFromSpace(bound, space);
        }

        /***************************************************/

        public static bool NormalAwayFromSpace(this BHG.Polyline pline, BHE.Space space)
        {
            BHG.Point centrePt = pline.Centre();

            List<BHG.Point> pts = BH.Engine.Geometry.Query.DiscontinuityPoints(pline);
            BHG.Plane plane = BH.Engine.Geometry.Create.Plane(pts[0], pts[1], pts[2]);

            //The polyline can be locally concave. Check if the polyline is clockwise.
            if (!BH.Engine.Geometry.Query.IsClockwise(pline, plane.Normal))
                plane.Normal = -plane.Normal;

            //Move centrepoint along the normal. If inside - flip the panel
            if (IsContaining(space, centrePt.Translate(plane.Normal * 0.1)))
                return false;
            else
                return true;

            /***************************************************/
        }
    }
}




