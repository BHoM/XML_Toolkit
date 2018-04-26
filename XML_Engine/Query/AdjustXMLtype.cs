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

        //TODO: We are updating the reference atm. Make sure we clone the object and modify the copy!

        public static BHE.BuildingElement AdjustXMLType(this BHE.BuildingElement buildingElement)
        {

            BHE.BuildingElement newBuildingElement = buildingElement.GetShallowClone() as BHE.BuildingElement;
            newBuildingElement.BuildingElementProperties.CustomData = new Dictionary<string, object>(buildingElement.BuildingElementProperties.CustomData);

            if (buildingElement.AdjacentSpaces.Count == 0) //Shade
                newBuildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"] = "Shade";


            else if (buildingElement.AdjacentSpaces.Count == 1) // External
            {
                if (buildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"].ToString() == "Internal Wall")
                    newBuildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"] = "External Wall";
                else if (buildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"].ToString() == "Internal Floor")
                    newBuildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"] = "Exposed Floor";
            }


            else if (buildingElement.AdjacentSpaces.Count == 2) // Internal
            {
                if (buildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"].ToString() == "Roof")
                    newBuildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"] = "Ceiling";
                else if (buildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"].ToString() == "External Wall")
                    newBuildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"] = "Internal Wall";
                else if (buildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"].ToString() == "External Floor")
                    newBuildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"] = "Internal Floor";
                else if (buildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"].ToString() == "Raised Floor")
                    newBuildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"] = "Internal Floor";
            }

            return newBuildingElement;


        }

        /***************************************************/

        public static List<BHE.BuildingElement> AdjustXMLType(this List<BHE.BuildingElement> bHoMBuildingElement)
        {
            List<BHE.BuildingElement> bHoMBuildingElements = new List<oM.Environmental.Elements.BuildingElement>();

            foreach (BHE.BuildingElement element in bHoMBuildingElement)
            {
                BHE.BuildingElement newBuildingElement = element.GetShallowClone() as BHE.BuildingElement;
                bHoMBuildingElements.Add(AdjustXMLType(newBuildingElement));
            }

            return bHoMBuildingElements;
        }

        /***************************************************/
    }
}



