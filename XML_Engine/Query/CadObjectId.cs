/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

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

        public static string CADObjectID(this BHE.BuildingElement bHoMBuildingElement, ExportType exportType = ExportType.gbXMLTAS)
        {
            string CADObjectID = "";
            string revitElementID = "";
            string familyName = "";

            if (bHoMBuildingElement.BuildingElementProperties != null)
            {
                //TODO: Make this non-dependent on Revit somehow so we can do the same thing but from another adapter, e.g. info pulled from TAS or from another gbXML
                if (bHoMBuildingElement.CustomData.ContainsKey("Revit_elementId"))
                    revitElementID = bHoMBuildingElement.CustomData["Revit_elementId"].ToString();
                if (bHoMBuildingElement.BuildingElementProperties.CustomData.ContainsKey("Family Name"))
                    familyName = bHoMBuildingElement.BuildingElementProperties.CustomData["Family Name"].ToString();

                // change only Basic Wall and keep Curtain as it is
                if (exportType == ExportType.gbXMLIES && familyName.Contains("Basic Wall") && bHoMBuildingElement.BuildingElementProperties.Name.Contains("GLZ"))
                    familyName = "Curtain Basic Wall";
                else if (exportType == ExportType.gbXMLIES && familyName.Contains("Floor") && bHoMBuildingElement.BuildingElementProperties.Name.Contains("GLZ"))
                    familyName = "Curtain Basic Floor";
                else if (exportType == ExportType.gbXMLIES && familyName.Contains("Roof") && bHoMBuildingElement.BuildingElementProperties.Name.Contains("GLZ"))
                    familyName = "Curtain Basic Roof";

                CADObjectID = familyName + ": " + bHoMBuildingElement.BuildingElementProperties.Name + " [" + revitElementID + "]";
            }
            return CADObjectID;
        }

        public static string CADObjectID(this List<BHE.BuildingElement> space)
        {
            string CADObjectID = "";

            BHE.BuildingElement spaceCustomData = space.Where(x => x.CustomData.ContainsKey("Space_Custom_Data")).FirstOrDefault();

            if (spaceCustomData == null) return CADObjectID;

            Dictionary<string, object> data = spaceCustomData.CustomData["Space_Custom_Data"] as Dictionary<string, object>;

            if (spaceCustomData != null)
            {
                if (data.ContainsKey("Revit_elementId"))
                    CADObjectID = data["Revit_elementId"].ToString();
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

                if (exportType == ExportType.gbXMLIES && familyName.Contains("Basic Wall") && element.BuildingElementProperties.Name.Contains("GLZ"))
                    familyName = "Curtain Basic Wall";

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




