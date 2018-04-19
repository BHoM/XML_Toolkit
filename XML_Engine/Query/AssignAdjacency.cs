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

        public static BHE.BuildingElement AssignAdjacency(this BHE.BuildingElement bHoMBuildingElement, List<Guid> spaceGUID)
        {
            foreach (Guid guid in spaceGUID)
            {
                if (!bHoMBuildingElement.AdjacentSpaces.Contains(guid))
                    bHoMBuildingElement.AdjacentSpaces.Add(guid);
            }

            return bHoMBuildingElement;
        }
        /***************************************************/
    }
}




