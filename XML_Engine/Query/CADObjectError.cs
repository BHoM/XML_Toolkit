using System;
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

        public static BHE.BuildingElement CADObjectError(this BHE.BuildingElement bHoMBuildingElement)
        {
            if (bHoMBuildingElement == null)
                return null;

            if (bHoMBuildingElement.CadObjectId() == "")
                return bHoMBuildingElement;

            //Check invalid combinations of adjacent spaces and CADobjectID
            else if (bHoMBuildingElement.CadObjectId().Contains("EXT") && bHoMBuildingElement.AdjacentSpaces.Count != 1)
                return bHoMBuildingElement;

            else if (bHoMBuildingElement.CadObjectId().Contains("INT") && bHoMBuildingElement.AdjacentSpaces.Count != 2)
                return bHoMBuildingElement;

            else if (bHoMBuildingElement.AdjacentSpaces.Count > 2) //This should never happen. Maximum value is 2
                return bHoMBuildingElement;

            return null;


            /***************************************************/
        }
    }
}




