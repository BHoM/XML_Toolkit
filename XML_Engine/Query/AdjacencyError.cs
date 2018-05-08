﻿using System;
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

        public static BHE.BuildingElement AdjacentError(this BHE.BuildingElement bHoMBuildingElement)
        {
            if (bHoMBuildingElement == null)
                return null;

            string type = bHoMBuildingElement.ToGbXMLType();

            if (!string.IsNullOrEmpty(type))
            {
                if (type.Contains("Shade") && bHoMBuildingElement.AdjacentSpaces.Count != 0)
                    return bHoMBuildingElement;

                else if ((type.Contains("Exterior") || type.Contains("Roof") || type.Contains("Raised") || type.Contains("Slab") || type.Contains("Underground") || type.Contains("Exposed")) && bHoMBuildingElement.AdjacentSpaces.Count != 1)
                    return bHoMBuildingElement;

                else if ((type.Contains("Interior") || type.Contains("Ceiling") || type.Contains("Air")) && bHoMBuildingElement.AdjacentSpaces.Count != 2)
                    return bHoMBuildingElement;

                else if (bHoMBuildingElement.AdjacentSpaces.Count > 2) //This should never happen. Maximum value is 2
                    return bHoMBuildingElement;

            }

            return null;


            /***************************************************/
        }
    }
}



