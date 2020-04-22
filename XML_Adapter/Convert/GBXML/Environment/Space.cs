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

using BH.oM.Environment.Elements;
using BH.oM.External.XML.Settings;
using GBXML = BH.oM.External.XML.GBXML;
using BH.oM.Geometry.SettingOut;

using BH.Engine.Geometry;
using BH.Engine.Environment;

namespace BH.Adapter.XML
{
    public static partial class Convert
    {
        public static GBXML.Space ToGBXML(this List<Panel> panelsAsSpace, Level spaceLevel, GBXMLSettings settings)
        {
            GBXML.Space xmlSpace = new GBXML.Space();

            xmlSpace.Name = panelsAsSpace.ConnectedSpaceName();
            xmlSpace.ID = "Space" + xmlSpace.Name.Replace(" ", "").Replace("-", "");
            xmlSpace.CADObjectID = BH.Engine.XML.Query.CADObjectID(panelsAsSpace);
            xmlSpace.ShellGeometry.ClosedShell.PolyLoop = BH.Engine.XML.Query.ClosedShellGeometry(panelsAsSpace, settings.PlanarTolerance).ToArray();
            xmlSpace.ShellGeometry.ID = "SpaceShellGeometry-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
            xmlSpace.SpaceBoundary = BH.Engine.XML.Query.SpaceBoundaries(panelsAsSpace, uniqueBuildingElements, settings.PlanarTolerance);
            xmlSpace.PlanarGeoemtry.ID = "SpacePlanarGeometry-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
            if (BH.Engine.Environment.Query.FloorGeometry(panelsAsSpace) != null)
            {
                xmlSpace.PlanarGeoemtry.PolyLoop = ToGBXML(BH.Engine.Environment.Query.FloorGeometry(panelsAsSpace));
                xmlSpace.Area = BH.Engine.Environment.Query.FloorGeometry(panelsAsSpace).Area();
                xmlSpace.Volume = panelsAsSpace.Volume();
            }

            if (spaceLevel != null)
            {
                string levelName = "";
                if (spaceLevel.Name == "")
                    levelName = spaceLevel.Elevation.ToString();
                else
                    levelName = spaceLevel.Name;

                xmlSpace.BuildingStoreyIDRef = "Level-" + levelName.Replace(" ", "").ToLower();
            }

            return xmlSpace;
        }
    }
}


