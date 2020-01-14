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
using BH.oM.XML;
using BH.oM.Base;
using BH.oM.Environment.Elements;
using BHP = BH.oM.Environment.Fragments;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;
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

        [Description("Determines whether a collection of Environment Panels representing a single space contains any panels which are externally facing")]
        [Input("elementsAsSpace", "A collection of Environment Panels that represent closed spaced")]
        [Input("elementsAsSpaces", "A nested collection of Environment Panels which represent all spaces in the model")]
        [Output("isExternal", "True if the space has at least one externally facing Panel")]
        public static bool IsExternal(this List<Panel> elementsAsSpace, List<List<Panel>> elementsAsSpaces)
        {
            //Check whether the space contains at least one external element
            return (elementsAsSpace.Where(x => x.AdjacentSpaces(elementsAsSpaces).Count == 1).ToList().Count > 0);
        }
    }
}