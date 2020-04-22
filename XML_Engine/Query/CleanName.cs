/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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

using BH.oM.External.XML.Enums;

using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.External.XML
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Get a clean string name representation of a name suitable for GBXML. Removes colons (replaced with underscores _ ), removes spaces, commas, forward slashes, open and closed round brackets")]
        [Input("name", "The string name to clean up")]
        [Output("cleanName", "The clean name suitable for GBXML")]
        public static string CleanName(this string name)
        {
            if (name == null)
                return null;

            if (name == string.Empty)
                return string.Empty;

            name = name.Replace(":", "_");
            name = name.Replace(" ", "");
            name = name.Replace(",", "");
            name = name.Replace("/", "");
            name = name.Replace("(", "");
            name = name.Replace(")", "");

            return name;
        }
    }
}





