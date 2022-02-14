/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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
using BH.oM.Base.Attributes;

using BH.oM.Environment.Elements;
using GBXML = BH.oM.Adapters.XML;
using BH.oM.Base;
using BH.oM.Spatial.SettingOut;
using BH.Engine.Environment;

using System.ComponentModel;

namespace BH.Engine.Adapters.XML
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Create a Document Builder object to collate data required for GBXML schema documents")]
        [Input("building", "The building object for the elements in the schema")]
        [Input("elementsAsSpaces", "The nested collection of Environment Panels which form closed spaces within the building")]
        [Input("shadingElements", "A collection of Environment Panels which provide shading to the building")]
        [Input("levels", "A collection of levels which group spaces by floor")]
        [Input("unassignedPanels", "Any additional panels needed to be included within the GBXML file")]
        [Output("documentBuilder", "A Document Builder object suitable for GBXML schema document creation")]
        public static GBXML.GBXMLDocumentBuilder DocumentBuilder(List<Building> building, List<List<Panel>> elementsAsSpaces, List<Panel> shadingElements, List<Level> levels, List<Panel> unassignedPanels)
        {
            return new GBXML.GBXMLDocumentBuilder
            {
                Buildings = building,
                ElementsAsSpaces = elementsAsSpaces,
                ShadingElements = shadingElements,
                Levels = levels,
                UnassignedPanels = unassignedPanels,
            };         
        }

        [Description("Create a Document Builder object to collate data required for GBXML schema documents automatically from a given list of BHoM Objects")]
        [Input("objs", "A collection of BHoM Objects to sort into a suitable format for inclusion within a GBXML file")]
        [Output("documentBuilder", "A Document Builder object suitable for GBXML schema document creation")]
        public static GBXML.GBXMLDocumentBuilder DocumentBuilder(List<IBHoMObject> objs)
        {
            List<Panel> panels = objs.Panels();
            List<Space> spaces = objs.Spaces();
            List<Level> levels = objs.Levels();
            List<Building> buildings = objs.Buildings();

            List<Panel> unassignedPanels = new List<Panel>();

            List<Panel> shadingElements = panels.FilterPanelsByType(new List<PanelType>() { PanelType.Shade }).Item1;
            panels = panels.FilterPanelsByType(new List<PanelType>() { PanelType.Shade }).Item2; //Remove shading if it exists

            List<List<Panel>> elementsAsSpaces = panels.ToSpaces();
            unassignedPanels.AddRange(panels.Where(x => !elementsAsSpaces.IsContaining(x)).ToList());

            return DocumentBuilder(buildings, elementsAsSpaces, shadingElements, levels, unassignedPanels);
        }
    }
}


