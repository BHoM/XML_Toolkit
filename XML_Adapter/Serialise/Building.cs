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
using BHP = BH.oM.Environment.Properties;
using BH.Engine.XML;

namespace BH.Adapter.XML
{
    public static partial class XMLSerializer
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static void SerializeCollection(IEnumerable<Building> inputBuildings, BH.oM.XML.GBXML gbx, bool isIES)
        {
            List<Building> buildings = inputBuildings.ToList();
            gbx.Campus.Building = new oM.XML.Building[buildings.Count];
            for(int x = 0; x < buildings.Count; x++)
            {
                gbx.Campus.Building[x] = buildings[x].ToGBXML();
                gbx.Campus.Location = buildings[x].ToGBXMLLocation();

                BHP.BuildingContextFragment props = buildings[x].FindFragment<BHP.BuildingContextFragment>(typeof(BHP.BuildingContextFragment));
                if(props != null)
                    gbx.Campus.Building[x].StreetAddress = props.PlaceName;

                if (buildings[x].CustomData.ContainsKey("Building Name"))
                    gbx.Campus.Building[x].BuildingType = (buildings[x].CustomData["Building Name"]).ToString();
            }
        }
    }
}