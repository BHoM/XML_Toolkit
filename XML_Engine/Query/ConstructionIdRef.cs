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

        public static string IdRef(this BHE.BuildingElement bHoMBuildingElement)
        {
            if (bHoMBuildingElement == null)
                return "-1"; //Return an error (-1)

            return bHoMBuildingElement.BuildingElementProperties.IdRef();
        }

        /***************************************************/

        public static string IdRef(this BHP.BuildingElementProperties bHoMprop)
        {
            //Method for constructing an ID based on the name of the property - this allows the same string ID to be generated for the same property for consistency in finding string IDs

            //Originally we used the GUID and got the combinations below - but each name is unique so each returned string ID will be unique anyway - but the following comment line is being left in as a nice little factoid for the next person... (//TD)
            //Using the first 8 digits of the GUID gives 218,340,105,584,896 possible combinations of IDs, so the liklihood of 2 different GUIDs producing the same result from this function is fairly small...

            if (bHoMprop == null) return "-1"; //Return an error (-1) if the property isn't really here

            String rtnID = "";

            for (int x = 0; x < bHoMprop.Name.Length; x++)
                rtnID += ((int)bHoMprop.Name[x]).ToString();

            return rtnID;
        }

        /***************************************************/
    }
}




