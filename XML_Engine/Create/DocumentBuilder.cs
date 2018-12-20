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
using System.Text;
using System.Threading.Tasks;

using BH.oM.Environment.Elements;
using BH.oM.XML.Environment;

namespace BH.Engine.XML
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static DocumentBuilder DocumentBuilder(List<Building> building = default(List<Building>), List<List<BuildingElement>> elementsAsSpaces = default(List<List<BuildingElement>>), List<BuildingElement> shadingElements = default(List<BuildingElement>), List<Space> spaces = default(List<Space>), List<BH.oM.Architecture.Elements.Level> levels = default(List<BH.oM.Architecture.Elements.Level>), List<BuildingElement> openings = default(List<BuildingElement>))
        {
            return new DocumentBuilder
            {
                Buildings = building,
                ElementsAsSpaces = elementsAsSpaces,
                ShadingElements = shadingElements,
                Spaces = spaces,
                Levels = levels,
                Openings = openings,
            };         
        }
    }
}