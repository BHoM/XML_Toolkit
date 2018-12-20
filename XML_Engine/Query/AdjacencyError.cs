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
using System.Linq;
using BH.oM.XML;
using BH.oM.Base;
using BHE = BH.oM.Environment.Elements;
using BHP = BH.oM.Environment.Properties;
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
            /*if (bHoMBuildingElement == null)
                return null;

            BHE.BuildingElement buildingElement = null;

            string type = bHoMBuildingElement.ToGBXMLType();

            if (!string.IsNullOrEmpty(type))
            {
                if (type.Contains("Shade") && bHoMBuildingElement.AdjacentSpaces.Count != 0)
                    buildingElement = bHoMBuildingElement;

                if ((type.Contains("Exterior") || type.Contains("Roof") || type.Contains("Raised") || type.Contains("Slab") || type.Contains("Underground") || type.Contains("Exposed")) && bHoMBuildingElement.AdjacentSpaces.Count != 1)
                    buildingElement = bHoMBuildingElement;

                if ((type.Contains("Interior") || type.Contains("Ceiling") || type.Contains("Air")) && bHoMBuildingElement.AdjacentSpaces.Count != 2)
                    buildingElement = bHoMBuildingElement;

                if (bHoMBuildingElement.AdjacentSpaces.Count > 2) //This should never happen. Maximum value is 2
                    buildingElement = bHoMBuildingElement;

            }

            return buildingElement;*/
            return null;


            /***************************************************/
        }
    }
}




