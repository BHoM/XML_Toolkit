﻿/*
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
using BHX = BH.oM.XML;
using BHG = BH.oM.Geometry;

using BH.Engine.Geometry;
using BH.Engine.Environment;

namespace BH.Engine.XML
{
    public static partial class Convert
    {
        public static BHX.Building ToGBXML(this BHE.Building building)
        {
            BHX.Building jsonBuilding = new BHX.Building();

            jsonBuilding.Name = building.Name;
            if (building.CustomData.ContainsKey("Place Name"))
                jsonBuilding.StreetAddress = (building.CustomData["Place Name"]).ToString();

            if (building.CustomData.ContainsKey("Building Name"))
                jsonBuilding.BuildingType = (building.CustomData["Building Name"]).ToString();

            return jsonBuilding;
        }

        public static BHX.Location ToGBXMLLocation(this BHE.Building building)
        {
            BHX.Location location = new BHX.Location();
            location.Longitude = Math.Round(building.Longitude, 5);
            location.Latitude = Math.Round(building.Latitude, 5);
            location.Elevation = Math.Round(building.Elevation, 5);

            if (building.CustomData.ContainsKey("Place Name"))
                location.Name = (building.CustomData["Place Name"]).ToString();
            if (building.CustomData.ContainsKey("Weather Station Name"))
                location.StationID.ID = (building.CustomData["Weather Station Name"]).ToString();

            return location;
        }

        public static BHE.Building ToBHoM(this BHX.Building jsonBuilding)
        {
            BHE.Building building = new BHE.Building();

            building.Name = jsonBuilding.Name;
            building.CustomData.Add("Place Name", jsonBuilding.StreetAddress);
            building.CustomData.Add("Building Name", jsonBuilding.BuildingType);

            return building;
        }

        public static BHE.Building ToBHoM(this BHX.Location location)
        {
            BHE.Building building = new oM.Environment.Elements.Building();

            building.Elevation = location.Elevation;
            building.Longitude = location.Longitude;
            building.Latitude = location.Latitude;

            building.CustomData.Add("Place Name", location.Name);
            building.CustomData.Add("Weather Station Name", location.StationID.ID);

            return building;
        }
    }
}

