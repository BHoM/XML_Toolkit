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

        public static List<BHE.Space> FindAdjacentSpace(this List<BHE.BuildingElement> bHoMBuildingElement, List<BHE.Space> spaces)
        {
            List<BHE.Space> adjSpaces = new List<BHE.Space>();
            foreach (BHE.BuildingElement element in bHoMBuildingElement)
            {
                foreach (BHE.Space space in spaces)
                {
                    foreach (BHE.BuildingElement panel in space.BuildingElements)
                    {
                        if (panel.BHoM_Guid.ToString() == element.BHoM_Guid.ToString())
                        {
                            adjSpaces.Add(space);
                        }
                    }
                }
            }
            return adjSpaces;

        }

        /***************************************************/

       

    }
}




