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

        public static List<BHE.BuildingElement> ReplaceBuildingElement(this List<BHE.BuildingElement> oldBuildingElements, List<BHE.BuildingElement> newBuildingElements)
        {
            List<BHE.BuildingElement> bElement = new List<oM.Environmental.Elements.BuildingElement>();

            foreach (Guid guid in newBuildingElements.Select(x => x.BHoM_Guid)) 
            {
                if (oldBuildingElements.Select(x => x.BHoM_Guid).Contains(guid))
                {
                    bElement.Add(newBuildingElements.Find(x => x.BHoM_Guid == guid));
                    bElement.AddRange(oldBuildingElements.FindAll(x => x.BHoM_Guid != guid));
                }
            }

            return bElement;
        }
        /***************************************************/
    }
}




