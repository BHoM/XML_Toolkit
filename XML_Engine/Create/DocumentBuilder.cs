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
using BH.oM.Reflection.Attributes;

using BH.oM.Environment.Elements;
using BH.oM.XML.Environment;
using BH.oM.Base;
using BH.oM.Geometry.SettingOut;
using BH.Engine.Environment;

namespace BH.Engine.XML
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static DocumentBuilder DocumentBuilder(List<Building> building, List<List<Panel>> elementsAsSpaces, List<Panel> shadingElements, List<Level> levels, List<Panel> unassignedPanels)
        {
            return new DocumentBuilder
            {
                Buildings = building,
                ElementsAsSpaces = elementsAsSpaces,
                ShadingElements = shadingElements,
                Levels = levels,
                UnassignedPanels = unassignedPanels,
            };         
        }

        public static DocumentBuilder DocumentBuilder(List<IBHoMObject> objs)
        {
            List<Panel> panels = objs.Panels();
            List<Space> spaces = objs.Spaces();
            List<Level> levels = objs.Levels();
            List<Building> buildings = objs.Buildings();

            List<Panel> unassignedPanels = new List<Panel>();

            List<Panel> shadingElements = panels.PanelsByType(PanelType.Shade);
            panels = panels.PanelsNotByType(PanelType.Shade); //Remove shading if it exists

            List<List<Panel>> elementsAsSpaces = panels.ToSpaces();
            unassignedPanels.AddRange(panels.Where(x => !elementsAsSpaces.IsContaining(x)).ToList());

            return DocumentBuilder(buildings, elementsAsSpaces, shadingElements, levels, unassignedPanels);
        }




        /***************************************************/
        /**** Deprecated Methods                        ****/
        /***************************************************/

        [Deprecated("2.4", "BH.oM.Architecture.Elements.Level superseded by BH.oM.Geometry.SettingOut.Level")]
        public static DocumentBuilder DocumentBuilder(List<Building> building, List<List<Panel>> elementsAsSpaces, List<Panel> shadingElements, List<BH.oM.Architecture.Elements.Level> levels, List<Panel> unassignedPanels)
        {
            return new DocumentBuilder
            {
                Buildings = building,
                ElementsAsSpaces = elementsAsSpaces,
                ShadingElements = shadingElements,
                Levels = levels.UpgradeVersion(),
                UnassignedPanels = unassignedPanels,
            };
        }


        /******************************************/
        /****      Level Version Converts      ****/
        /******************************************/

        [DeprecatedAttribute("2.4", "BH.oM.Architecture.Elements.Level replaced by BH.oM.Geometry.SettingOut.Level")]
        private static List<Level> UpgradeVersion(this List<BH.oM.Architecture.Elements.Level> levels)
        {
            List<Level> upgradedLevels = new List<Level>();

            foreach (BH.oM.Architecture.Elements.Level level in levels)
                upgradedLevels.Add(level.UpgradeVersion());

            return upgradedLevels;
        }

        /******************************************/

        [DeprecatedAttribute("2.4", "BH.oM.Architecture.Elements.Level replaced by BH.oM.Geometry.SettingOut.Level")]
        private static Level UpgradeVersion(this BH.oM.Architecture.Elements.Level level)
        {
            return new Level { Name = level.Name, Elevation = level.Elevation, CustomData = level.CustomData, Fragments = level.Fragments };
        }

        /******************************************/
    }
}