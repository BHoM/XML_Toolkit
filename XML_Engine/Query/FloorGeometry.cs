﻿using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.XML;
using BH.oM.Base;
using BHE = BH.oM.Environment.Elements;
using BHP = BH.oM.Environment.Properties;
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

        public static BHG.Polyline FloorGeometry(this BHE.Space bHoMSpace)
        {
            List<BHE.BuildingElement> bHoMBuildingElement = bHoMSpace.BuildingElements;
            BHG.PolyCurve polyCrv = new BHG.PolyCurve();

            foreach (BHE.BuildingElement element in bHoMBuildingElement)
            {
                if (BH.Engine.Environment.Query.Tilt(element.BuildingElementGeometry) == 180) // if floor
                    polyCrv = element.BuildingElementGeometry.ICurve() as BHG.PolyCurve;
                //TODO: What if we have more than one floor?
            }


            BHG.Polyline floorBoundary = new BHG.Polyline() { ControlPoints = polyCrv.ControlPoints() };

            if (floorBoundary.ControlPoints.Count < 3)
                return null;
            return floorBoundary;
        }

        /***************************************************/

    }
}




