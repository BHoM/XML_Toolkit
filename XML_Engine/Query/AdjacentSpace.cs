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

        public static List<AdjacentSpaceId> AdjacentSpace(this BHE.BuildingElement bHoMBuildingElement, List<BHE.Space> spaces)
        {
            List<AdjacentSpaceId> adspace = new List<AdjacentSpaceId>();

            foreach (Guid adjSpace in bHoMBuildingElement.AdjacentSpaces)
            {
                AdjacentSpaceId adjId = new AdjacentSpaceId();
                if (spaces.Select(x => x.BHoM_Guid).Contains(adjSpace))
                {
                    adjId.spaceIdRef = "Space-" + spaces.Find(x => x.BHoM_Guid == adjSpace).Name;
                    adspace.Add(adjId);
                }
            }

            return adspace;

        }

        /***************************************************/

        public static List<Guid> AdjacentSpace(this List<BHE.BuildingElement> bHoMBuildingElement)
        {
            List<Guid> adjspace = new List<Guid>();

            foreach (BHE.BuildingElement element in bHoMBuildingElement)
            {
                adjspace.Add(element.AdjacentSpaces[0]);
            }

            return adjspace;

            /***************************************************/
        }

    }
}




