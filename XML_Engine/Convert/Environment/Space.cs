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
using System.Text;
using System.Threading.Tasks;

using BHE = BH.oM.Environment.Elements;
using BHM = BH.oM.Environment.MaterialFragments;
using BHP = BH.oM.Environment.Fragments;
using BHA = BH.oM.Architecture.Elements;
using BHX = BH.oM.XML;
using BHG = BH.oM.Geometry;

using BH.Engine.Geometry;
using BH.Engine.Environment;

using BH.oM.XML.Enums;

namespace BH.Engine.XML
{
    public static partial class Convert
    {
        /*public static BHX.Space ToGBXML(this BHE.Space space, List<BHE.BuildingElement> elementsAsSpace, List<BHE.BuildingElement> uniqueBuildingElements, List<BHA.Level> levels)
        {
            Dictionary<string, object> spaceData = (elementsAsSpace.Where(x => x.CustomData.ContainsKey("Space_Custom_Data")).FirstOrDefault() != null ? elementsAsSpace.Where(x => x.CustomData.ContainsKey("Space_Custom_Data")).FirstOrDefault().CustomData["Space_Custom_Data"] as Dictionary<string, object> : new Dictionary<string, object>());

            BHX.Space gbSpace = new BHX.Space();
            gbSpace.Name = (spaceData.ContainsKey("SAM_SpaceName") && spaceData["SAM_SpaceName"] != null ? spaceData["SAM_SpaceName"].ToString() : space.Name); //CUSTOMDATA SAM_SpaceName
            gbSpace.ID = "Space-" + space.Number + "-" + space.Name;
            gbSpace.CADObjectID = elementsAsSpace.CADObjectID();
            gbSpace.ShellGeometry.ClosedShell.PolyLoop = elementsAsSpace.ClosedShellGeometry().ToArray();
            gbSpace.ShellGeometry.ID = "SpaceShellGeometry-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
            gbSpace.SpaceBoundary = elementsAsSpace.SpaceBoundaries(uniqueBuildingElements);
            gbSpace.PlanarGeoemtry.ID = "SpacePlanarGeometry-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
            if (elementsAsSpace.FloorGeometry() != null)
            {
                gbSpace.PlanarGeoemtry.PolyLoop = elementsAsSpace.FloorGeometry().ToGBXML();
                gbSpace.Area = elementsAsSpace.FloorGeometry().Area();
                gbSpace.Volume = elementsAsSpace.Volume();
            }

            BHA.Level spaceLevel = space.Level(levels);
            if (spaceLevel != null)
                gbSpace.BuildingStoreyIDRef = "Level-" + spaceLevel.Name.Replace(" ", "").ToLower();

            return gbSpace;
        }*/
    }
}


