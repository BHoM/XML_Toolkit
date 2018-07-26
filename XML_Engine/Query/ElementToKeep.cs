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

        public static BHE.BuildingElement ElementToKeep(this BHE.BuildingElement bHoMBuildingElement, BHG.Polyline srfBound, List<BHE.Space> spaces)
        {
            //Check if the surface normal is pointing away from the first AdjSpace. Keep if it does.

            BHE.BuildingElement buildingElement = null;

            if (bHoMBuildingElement.AdjacentSpaces.Count > 0)
            {
                Guid firstGuid = bHoMBuildingElement.AdjacentSpaces.First();
                BHE.Space firstSpace = spaces.Find(x => x.BHoM_Guid == firstGuid);

                if (firstSpace == null)
                    buildingElement = bHoMBuildingElement;
                else
                {
                    if (BH.Engine.XML.Query.NormalAwayFromSpace(srfBound,firstSpace))
                        buildingElement = bHoMBuildingElement; //If the surface normal is pointing away from the first adjacent space we will keep this building element. 
                }
            }

            else  //Shade elements (no adjacent space)
                buildingElement = bHoMBuildingElement;

            return buildingElement;          
            

            /***************************************************/
        }
    }
}




