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

        public static BHE.BuildingElement ElementToKeep(this BHE.BuildingElement bHoMBuildingElement, BHG.Polyline srfBound, List<BHE.Space> spaces)
        {
            //Check if the surface normal is pointing away from the first AdjSpace. Keep if it does.

            BHE.BuildingElement buildingElement = new BHE.BuildingElement();
            if (bHoMBuildingElement.AdjacentSpaces.Count > 0)
            {
                Guid firstGuid = bHoMBuildingElement.AdjacentSpaces.First();
                BHE.Space firstSpace = spaces.Find(x => x.BHoM_Guid == firstGuid);

                if (firstSpace == null)
                    return bHoMBuildingElement;
                else
                {
                    //if (BH.Engine.Geometry.Query.IsClockwise(srfBound, BH.Engine.Environment.Query.Centre(firstSpace)))
                    if (BH.Engine.XML.Query.NormalAwayFromSpace(srfBound,firstSpace))
                        return bHoMBuildingElement;
                    else //If the surface normal is pointing towards the first adjacent space we will ignore this building element. 
                        return null;
                }
            }

            else  //Shade elements (no adjacent space)
                return bHoMBuildingElement;
            
            

            /***************************************************/
        }
    }
}




