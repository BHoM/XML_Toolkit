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

using BHE = BH.oM.Environment.Elements;
using BHA = BH.oM.Architecture.Elements;
using BHX = BH.Adapter.XML.GBXMLSchema;
using BHG = BH.oM.Geometry;
using BHS = BH.oM.Spatial;

using BH.Engine.Geometry;
using BH.Engine.Environment;
using System.ComponentModel;
using BH.oM.Base.Attributes;
using BH.oM.Adapters.XML.Settings;

namespace BH.Adapter.XML
{
    public static partial class Convert
    {
        [Description("Returns a gbXML BuildingStorey represention of a BHoM level and spaces")]
        [Input("level", "A BHoM level to find the storey for")]
        [Input("spaces", "A list of BHoM spaces that sits on the given level")]
        [Output("buildingStorey", "The gbXML building storey")]
        public static BHX.BuildingStorey ToGBXML(this BHS.SettingOut.Level level, BHG.Polyline storeyGeometry, GBXMLSettings settings)
        {
            BHX.BuildingStorey storey = new BHX.BuildingStorey();

            if (storeyGeometry != null)
                storey.PlanarGeometry.PolyLoop = storeyGeometry.ToGBXML(settings);

            storey.PlanarGeometry.ID = "LevelPlanarGeometry-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
            storey.Name = level.Name;
            storey.ID = "Level-" + level.Name.Replace(" ", "").ToLower();
            storey.Level = (float)level.Elevation;

            return storey;
        }

        [Description("Returns a BHoM Level represention of a gbXML storey")]
        [Input("storey", "Set a gbXML storey to get the Level from")]
        [Output("level", "The BHoM level")]
        public static BHS.SettingOut.Level FromGBXML(this BHX.BuildingStorey storey)
        {
            BHS.SettingOut.Level level = new BHS.SettingOut.Level();

            level.Name = storey.Name;
            level.Elevation = storey.Level;

            return level;
        }
    }
}



