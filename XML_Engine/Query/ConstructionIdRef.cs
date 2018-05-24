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

        public static string IdRef(this BHE.BuildingElement bHoMBuildingElement, List<BHE.BuildingElement> buildingElementList)
        {
            List<BHP.BuildingElementProperties> props = buildingElementList.Select(x => x.BuildingElementProperties).Distinct(new BH.Engine.Base.Objects.BHoMObjectNameComparer()).Select(x => x as BHP.BuildingElementProperties).ToList();

            return bHoMBuildingElement.BuildingElementProperties.IdRef(props);
        }

        /***************************************************/

        public static string IdRef(this BHP.BuildingElementProperties bHoMprop, List<BHP.BuildingElementProperties> bHoMprops)
        {
            //Method for constructing an ID based on the GUID of the property - this allows the same string ID to be generated for the same GUID for consistency in finding string IDs

            //Using the first 8 digits of the GUID gives 218,340,105,584,896 possible combinations of IDs, so the liklihood of 2 different GUIDs producing the same result from this function is fairly small...

            if (bHoMprop == null) return "-1"; //Return an error (-1) if the property isn't really here

            String guidPart = bHoMprop.BHoM_Guid.ToString().Split('-')[0];
            String rtnID = "";

            for (int x = 0; x < guidPart.Length; x++)
                rtnID += ((int)guidPart[x]).ToString();

            return rtnID;
        }

        /***************************************************/
    }
}




