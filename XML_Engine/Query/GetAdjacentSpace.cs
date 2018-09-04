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

        public static List<AdjacentSpaceId> GetAdjacentSpace(this BHE.BuildingElement bHoMBuildingElement, List<BHE.Space> spaces)
        {
            List<AdjacentSpaceId> adSpace = new List<AdjacentSpaceId>();

            foreach (Guid adjSpace in bHoMBuildingElement.AdjacentSpaces)
            {
                AdjacentSpaceId adjId = new AdjacentSpaceId();
                if (spaces.Select(x => x.BHoM_Guid).Contains(adjSpace))
                {
                    BHE.Space foundSpace = spaces.Find(x => x.BHoM_Guid == adjSpace);
                    if (foundSpace == null)
                        continue;
                    adjId.SpaceIDRef = foundSpace.Number + "-" + foundSpace.Name;
                    adSpace.Add(adjId);
                }
            }

            return adSpace;

        }

        /***************************************************/

        public static List<Guid> GetAdjacentSpace(this List<BHE.BuildingElement> bHoMBuildingElement)
        {
            List<Guid> adjSpace = new List<Guid>();

            foreach (BHE.BuildingElement element in bHoMBuildingElement)
            {
                adjSpace.Add(element.AdjacentSpaces[0]);
            }

            return adjSpace;

            /***************************************************/
        }

    }
}




