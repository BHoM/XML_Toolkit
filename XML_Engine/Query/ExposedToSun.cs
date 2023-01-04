/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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

using BH.oM.Base.Attributes;
using System.ComponentModel;

namespace BH.Engine.Adapters.XML
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("BH.Engine.XML.Query.ExposedToSun => Returns whether a given gbXML surface type string can be classed as being exposed to the sun")]
        [Input("surfaceType", "A gbXML string representing a surface type")]
        [Output("exposedToSun", "True if the surface type can be considered to be exposed to the sun, false otherwise")]
        public static bool ExposedToSun(string surfaceType)
        {
            if (String.IsNullOrEmpty(surfaceType)) return false;

            surfaceType = surfaceType.Replace(" ", String.Empty).ToLower();

            return surfaceType == "raisedfloor" || surfaceType == "exteriorwall" || surfaceType == "roof" || surfaceType == "exposedfloor" || surfaceType == "shade";
        }
    }
}




