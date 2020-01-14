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
using BHP = BH.oM.Environment.Fragments;
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
            BHX.Building gbBuilding = new BHX.Building();

            gbBuilding.Name = building.Name;

            BHP.BuildingContextFragment context = building.FindFragment<BHP.BuildingContextFragment>(typeof(BHP.BuildingContextFragment));

            if (context != null)
                gbBuilding.StreetAddress = context.PlaceName;

            if (building.CustomData.ContainsKey("Building Name"))
                gbBuilding.BuildingType = (building.CustomData["Building Name"]).ToString();

            return gbBuilding;
        }

        public static BHX.Location ToGBXMLLocation(this BHE.Building building)
        {
            BHX.Location location = new BHX.Location();
            location.Longitude = Math.Round(building.Longitude, 5);
            location.Latitude = Math.Round(building.Latitude, 5);
            location.Elevation = Math.Round(building.Elevation, 5);

            BHP.BuildingContextFragment context = building.FindFragment<BHP.BuildingContextFragment>(typeof(BHP.BuildingContextFragment));

            if (context != null)
            { 
                location.Name = context.PlaceName;
                location.StationID.ID = context.WeatherStation;
            }

            return location;
        }

        public static BHE.Building ToBHoM(this BHX.Building gbBuilding)
        {
            BHE.Building building = new BHE.Building();

            building.Name = gbBuilding.Name;
            BHP.BuildingContextFragment props = new BHP.BuildingContextFragment();
            props.PlaceName = gbBuilding.StreetAddress;
            building.Fragments.Add(props);
            building.CustomData.Add("Building Name", gbBuilding.BuildingType);

            return building;
        }

        public static BHE.Building ToBHoM(this BHX.Location location)
        {
            BHE.Building building = new oM.Environment.Elements.Building();

            building.Elevation = location.Elevation;
            building.Longitude = location.Longitude;
            building.Latitude = location.Latitude;

            BHP.BuildingContextFragment props = new BHP.BuildingContextFragment();
            props.PlaceName = location.Name;
            props.WeatherStation = location.StationID.ID;
            building.Fragments.Add(props);

            return building;
        }
    }
}

