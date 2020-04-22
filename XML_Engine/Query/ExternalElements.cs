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
using System.Linq;
using BH.oM.Environment.Elements;
using BH.Engine.Environment;

using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.XML
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Filters a collection of Environment Panels grouped to represent closed spaces to only those which have only one adjacenet space")]
        [Input("elementsAsSpaces", "A nested collection of Environment Panels that represent closed spaced")]
        [Output("externalElements", "The collection of Environment Panels which are externally facing")]
        public static List<Panel> ExternalElements(this List<List<Panel>> elementsAsSpaces)
        {
            List<Panel> externalElements = new List<Panel>();

            foreach (List<Panel> space in elementsAsSpaces)
                externalElements.AddRange(space.Where(x => x.AdjacentSpaces(elementsAsSpaces).Count == 1).ToList());

            return externalElements;
        }
    }
}