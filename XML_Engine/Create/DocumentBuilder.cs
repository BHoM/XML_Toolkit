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
using BH.oM.Base;
using BH.oM.Architecture.Elements;
using BH.Engine.Environment;

namespace BH.Engine.XML
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static DocumentBuilder DocumentBuilder(List<Building> building, List<List<BuildingElement>> elementsAsSpaces, List<BuildingElement> shadingElements, List<BH.oM.Architecture.Elements.Level> levels, List<BuildingElement> openings)
        {
            return new DocumentBuilder
            {
                Buildings = building,
                ElementsAsSpaces = elementsAsSpaces,
                ShadingElements = shadingElements,
                Levels = levels,
                Openings = openings,
            };         
        }

        public static DocumentBuilder DocumentBuilder(List<IBHoMObject> objs)
        {
            List<BuildingElement> buildingElements = objs.BuildingElements();
            List<Space> spaces = objs.Spaces();
            List<Level> levels = objs.ConvertAll(x => (BHoMObject)x).Levels();
            List<Building> buildings = objs.Buildings();

            List<BuildingElement> openings = buildingElements.ElementsByType(BuildingElementType.Door);
            openings.AddRange(buildingElements.ElementsByType(BuildingElementType.Rooflight));
            openings.AddRange(buildingElements.ElementsByType(BuildingElementType.RooflightWithFrame));
            openings.AddRange(buildingElements.ElementsByType(BuildingElementType.Window));
            openings.AddRange(buildingElements.ElementsByType(BuildingElementType.WindowWithFrame));

            List<BuildingElement> shadingElements = buildingElements.ShadingElements();
            buildingElements = buildingElements.ElementsWithoutType(BuildingElementType.Shade); //Remove shading if it exists

            List<string> uniqueSpaceNames = buildingElements.UniqueSpaceNames();

            List<List<BuildingElement>> elementsAsSpaces = buildingElements.BuildSpaces(uniqueSpaceNames);

            return DocumentBuilder(buildings, elementsAsSpaces, shadingElements, levels, openings);
        }
    }
}