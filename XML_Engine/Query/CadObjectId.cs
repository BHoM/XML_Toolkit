using System;
using System.Collections.Generic;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;
using System.Linq;
using BHE = BH.oM.Environment.Elements;

namespace BH.Engine.XML
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static string CadObjectId(this BHE.BuildingElement bHoMBuildingElement, bool isIES = false)
        {
            string CADObjectID = "";
            string revitElementID = "";
            string familyName = "";

            if (bHoMBuildingElement.BuildingElementProperties != null)
            {
                if (bHoMBuildingElement.CustomData.ContainsKey("Revit_elementId"))
                    revitElementID = bHoMBuildingElement.CustomData["Revit_elementId"].ToString();
                if (bHoMBuildingElement.BuildingElementProperties.CustomData.ContainsKey("Family Name"))
                    familyName = bHoMBuildingElement.BuildingElementProperties.CustomData["Family Name"].ToString();

                if (isIES && familyName.Contains("Wall") && bHoMBuildingElement.BuildingElementProperties.Name.Contains("GLZ"))
                    familyName = "Curtain Wall";

                CADObjectID = familyName + ": " + bHoMBuildingElement.BuildingElementProperties.Name + " [" + revitElementID + "]";
            }
            return CADObjectID;
        }

        /***************************************************/

        public static string CadObjectId(BHE.Opening bHoMOpening, List<BHE.BuildingElement> buildingElementsList, bool isIES = false)
        {
            string CADObjectID = "";
            string familyName = "";
            string typeName = "";

            if (bHoMOpening.CustomData.ContainsKey("Revit_elementId"))
            {
                string elementID = (bHoMOpening.CustomData["Revit_elementId"]).ToString();
                BHE.BuildingElement buildingElement = buildingElementsList.Find(x => x != null && x.CustomData.ContainsKey("Revit_elementId") && x.CustomData["Revit_elementId"].ToString() == elementID);
                if (buildingElement != null && buildingElement.BuildingElementProperties.CustomData.ContainsKey("Family Name"))
                {
                    familyName = buildingElement.BuildingElementProperties.CustomData["Family Name"].ToString();
                    typeName = buildingElement.BuildingElementProperties.Name;
                }
                CADObjectID = familyName + ": " + typeName + " [" + elementID + "]";
            }
            return CADObjectID;
        }

        /***************************************************/

        public static string CadObjectId(BHE.Space bHoMSpace)
        {
            string CADObjectID = "";

            if (bHoMSpace.CustomData.ContainsKey("Revit_elementId"))
                CADObjectID = (bHoMSpace.CustomData["Revit_elementId"]).ToString();

            return CADObjectID;
        }

        /***************************************************/
    }
}




