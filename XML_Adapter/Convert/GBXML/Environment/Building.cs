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
using BHP = BH.oM.Environment.Fragments;
using BHX = BH.Adapter.XML.GBXMLSchema;
using BHG = BH.oM.Geometry;

using BH.Engine.Geometry;
using BH.Engine.Environment;

using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Adapter.XML
{
    public static partial class Convert
    {
        [Description("Get the GBXML representation of a BHoM Environments Building")]
        [Input("building", "The BHoM Environments Building to convert into a GBXML Building")]
        [Output("building", "The GBXML representation of a BHoM Environment Building")]
        public static BHX.Building ToGBXML(this BHE.Building building)
        {
            BHX.Building gbBuilding = new BHX.Building();

            gbBuilding.Name = building.Name;

            BHP.BuildingContextFragment context = building.FindFragment<BHP.BuildingContextFragment>(typeof(BHP.BuildingContextFragment));

            if (context != null)
                gbBuilding.StreetAddress = context.PlaceName;

            if (building.CustomData.ContainsKey("Building Name"))
                gbBuilding.BuildingType = (building.CustomData["Building Name"]).ToString();

            if (gbBuilding.BuildingType == "")
                gbBuilding.BuildingType = "Unknown";

            return gbBuilding;
        }

        [Description("Get the GBXML representation of a BHoM Environments Building")]
        [Input("building", "The BHoM Environments Building to convert into a GBXML Location")]
        [Output("location", "The GBXML representation of a BHoM Environment Building")]
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

        [Description("Get the BHoM representation of a GBXML Building")]
        [Input("gbBuilding", "The GBXML Building to convert into a BHoM Building")]
        [Output("building", "The BHoM representation of a GBXML Building")]
        public static BHE.Building FromGBXML(this BHX.Building gbBuilding)
        {
            BHE.Building building = new BHE.Building();

            building.Name = gbBuilding.Name;
            BHP.BuildingContextFragment props = new BHP.BuildingContextFragment();
            props.PlaceName = gbBuilding.StreetAddress;
            building.Fragments.Add(props);
            building.CustomData.Add("Building Name", gbBuilding.BuildingType);

            return building;
        }

        [Description("Get the BHoM representation of a GBXML Location")]
        [Input("location", "The GBXML Location to convert into a BHoM Building")]
        [Output("building", "The BHoM representation of a GBXML Location")]
        public static BHE.Building FromGBXML(this BHX.Location location)
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


