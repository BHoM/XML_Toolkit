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

using BHE = BH.oM.Environment.Elements;
using BHA = BH.oM.Architecture.Elements;
using BHX = BH.oM.XML;
using BHG = BH.oM.Geometry;

using BH.Engine.Geometry;
using BH.Engine.Environment;
using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.XML
{
    public static partial class Convert
    {
        [Description("Returns a BuildingStorey for a given level and spaces")]
        [Input("level","A level to find the storey for")]
        [Input("space", "A list of BHoM spaces that sits on that level")]
        [Output("buildingStorey", "The building storey of the set level and spaces")]
        public static BHX.BuildingStorey ToGBXML(this BHG.SettingOut.Level level, List<List<BHE.Panel>> spaces = null)
        {
            BHX.BuildingStorey storey = new BHX.BuildingStorey();

            if (spaces != null)
            {
                BHG.Polyline storeyGeometry = level.StoreyGeometry(spaces);
                if (storeyGeometry != null)
                    storey.PlanarGeometry.PolyLoop = storeyGeometry.ToGBXML();
            }

            storey.PlanarGeometry.ID = "level-planar-geometry-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
            storey.Name = level.Name;
            storey.ID = "Level-" + level.Name.Replace(" ", "").ToLower();
            storey.Level = (float)level.Elevation;

            return storey;
        }

        [Description("Returns a Level of a given storey")]
        [Input("storey", "Set a storey to get the Level off")]
        [Output("level", "The level of the set storey")]
        public static BHG.SettingOut.Level ToBHoM(this BHX.BuildingStorey storey)
        {
            BHG.SettingOut.Level level = new BHG.SettingOut.Level();

            level.Name = storey.Name;
            level.Elevation = storey.Level;

            return level;
        }
    }
}
