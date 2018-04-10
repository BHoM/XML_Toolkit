using System;
using System.Collections.Generic;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;
using System.Linq;
using BHE = BH.oM.Environmental.Elements;


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
    }
}




