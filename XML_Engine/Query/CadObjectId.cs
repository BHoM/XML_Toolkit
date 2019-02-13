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
using BHP = BH.oM.Environment.Properties;

using BH.oM.XML.Enums;

using BH.Engine.Environment;

namespace BH.Engine.XML
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static string CADObjectID(this BHE.BuildingElement element, ExportType exportType = ExportType.gbXMLTAS)
        {
            string CADObjectID = "";
            if (element == null) return CADObjectID;

            BHP.EnvironmentContextProperties contextProperties = element.EnvironmentContextProperties() as BHP.EnvironmentContextProperties;

            if (contextProperties != null)
            {
                // change only Basic Wall and keep Curtain as it is
                if (exportType == ExportType.gbXMLIES && element.Name.Contains("GLZ") && (element.Name.Contains("Basic Wall") || element.Name.Contains("Floor") || element.Name.Contains("Roof")))
                    element.Name = "Curtain " + element.Name;

                CADObjectID = element.Name + " [" + contextProperties.ElementID + "]";
            }

            return CADObjectID;
        }

        public static string CADObjectID(this List<BHE.BuildingElement> space)
        {
            /*string CADObjectID = "";

            BHE.BuildingElement spaceCustomData = space.Where(x => x.CustomData.ContainsKey("Space_Custom_Data")).FirstOrDefault();

            if (spaceCustomData == null) return CADObjectID;

            Dictionary<string, object> data = spaceCustomData.CustomData["Space_Custom_Data"] as Dictionary<string, object>;

            if (spaceCustomData != null)
            {
                if (data.ContainsKey("Revit_elementId"))
                    CADObjectID = data["Revit_elementId"].ToString();
            }

            return CADObjectID;*/

            return "FIX CAD OBJECT ID FOR SPACE";
        }

        /***************************************************/

        public static string CADObjectID(this BHE.Opening opening, ExportType exportType)
        {
            BHP.EnvironmentContextProperties contextProp = opening.EnvironmentContextProperties() as BHP.EnvironmentContextProperties;
            if (contextProp == null) return "";

            return contextProp.TypeName + " [" + contextProp.ElementID + "]";
        }

        /***************************************************/

        public static string CADObjectID(this BHE.Space space)
        {
            BHP.EnvironmentContextProperties contextProp = space.EnvironmentContextProperties() as BHP.EnvironmentContextProperties;
            if (contextProp == null) return "";

            return contextProp.TypeName + " [" + contextProp.ElementID + "]";
        }
    }
}
