﻿using System;
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

        public static List<BHE.BuildingElement> AdjacentSurface(this BHE.BuildingElement bHoMBuildingElement, BHE.Building building)
        {
            if (bHoMBuildingElement == null)
                return null;

            BHG.Plane plane = bHoMBuildingElement.BuildingElementGeometry.ICurve().IFitPlane();
            List<BHG.Polyline> plines = new List<BHG.Polyline>();

            List<BHE.BuildingElement> beInPlane = new List<BHE.BuildingElement>();
            BHG.Polyline pline = new BHG.Polyline() { ControlPoints = bHoMBuildingElement.BuildingElementGeometry.ICurve().IControlPoints() };

            if (pline.IsClosed())
                plines.Add(pline);

            //Find Space
            BHE.Space space = building.Spaces.Find(x => x.BHoM_Guid.ToString() == bHoMBuildingElement.AdjacentSpaces.First().ToString());

            //Find the other panels in the same plane that intersects with the polyline
            foreach (BHE.BuildingElement be in space.BuildingElements)
            {
                if (be.BuildingElementGeometry.ICurve().IIsInPlane(plane) && be.BHoM_Guid.ToString() != bHoMBuildingElement.BHoM_Guid.ToString())
                {
                    if (be.BuildingElementGeometry.ICurve().IIsClosed())
                    {
                        plines.Add(be.BuildingElementGeometry.ICurve().ICollapseToPolyline(BHG.Tolerance.MacroDistance));

                        if (plines.Count == 2 && BH.Engine.Geometry.Compute.BooleanUnion(plines).Count == 1) //If the two polylines intersect we they can be merged into one srf by booleanUnion
                            beInPlane.Add(be);
                    }
                }
            }

            return beInPlane;


            /***************************************************/
        }
    }
}



