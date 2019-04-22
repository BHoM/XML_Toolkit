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

using BH.oM.Environment.Elements;
using System;
using System.Collections.Generic;

using System.Linq;
using BH.Engine.Environment;

using BH.oM.XML;
using BH.Engine.XML;

using BH.oM.Geometry;
using BH.Engine.Geometry;

using BH.oM.XML.Enums;

namespace BH.Adapter.XML
{
    public partial class GBXMLSerializer
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static void SerializeLevels(List<BH.oM.Architecture.Elements.Level> levels, List<List<Panel>> spaces, BH.oM.XML.GBXML gbx, ExportType exportType)
        {
            List<BH.oM.XML.BuildingStorey> xmlLevels = new List<BuildingStorey>();

            foreach(BH.oM.Architecture.Elements.Level level in levels)
            {
                string levelName = "";
                if (level.Name == "")
                    levelName = level.Elevation.ToString();
                else
                    levelName = level.Name;

                BuildingStorey storey = BH.Engine.XML.Convert.ToGBXML(level);
                Polyline storeyGeometry = BH.Engine.Environment.Query.StoreyGeometry(level, spaces);
                if (storeyGeometry == null)
                    continue;
                storey.PlanarGeometry.PolyLoop = BH.Engine.XML.Convert.ToGBXML(storeyGeometry);
                storey.PlanarGeometry.ID = "LevelPlanarGeometry-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
                storey.Name = levelName;
                storey.Level = (float)level.Elevation;
                storey.ID = "Level-" + levelName.Replace(" ", "").ToLower();

                xmlLevels.Add(storey);
            }

            gbx.Campus.Building[0].BuildingStorey = xmlLevels.ToArray();
        }
    }
}