using System;
using System.Collections.Generic;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;
using System.Linq;
using BHE = BH.oM.Environment.Elements;

using BH.oM.XML.Enums;

namespace BH.Engine.XML
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static string CadObjectId(this BHE.BuildingElement bHoMBuildingElement, ExportType exportType)
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

                if (exportType == ExportType.gbXMLIES && familyName.Contains("Wall") && bHoMBuildingElement.BuildingElementProperties.Name.Contains("GLZ"))
                    familyName = "Curtain Wall";

                CADObjectID = familyName + ": " + bHoMBuildingElement.BuildingElementProperties.Name + " [" + revitElementID + "]";
            }
            return CADObjectID;
        }

        public static string SurfaceName(this BHE.BuildingElement element, ExportType exportType)
        {
            string CADObjectID = "";
            string familyName = "";

            if (element.BuildingElementProperties != null)
            {
                if (element.BuildingElementProperties.CustomData.ContainsKey("Family Name"))
                    familyName = element.BuildingElementProperties.CustomData["Family Name"].ToString();

                if (exportType == ExportType.gbXMLIES && familyName.Contains("Wall") && element.BuildingElementProperties.Name.Contains("GLZ"))
                    familyName = "Curtain Wall";

                CADObjectID = familyName + ": " + element.BuildingElementProperties.Name;
            }
            return CADObjectID;
        }

        /***************************************************/

        public static string CadObjectId(BHE.Opening bHoMOpening, List<BHE.BuildingElement> buildingElementsList, ExportType exportType)
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

        public static string CadObjectId(List<BHE.BuildingElement> space)
        {
            string CADObjectID = "";

            BHE.BuildingElement spaceCustomData = space.Where(x => x.CustomData.ContainsKey("Space_Custom_Data")).FirstOrDefault();

            if (spaceCustomData == null) return CADObjectID;

            Dictionary<string, object> data = spaceCustomData.CustomData["Space_Custom_Data"] as Dictionary<string, object>;

            if(spaceCustomData != null)
            {
                if (data.ContainsKey("Revit_elementId"))
                    CADObjectID = data["Revit_elementId"].ToString();
            }

            return CADObjectID;
        }
    }
}




