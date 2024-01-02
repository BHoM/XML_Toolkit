/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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

using BH.oM.Adapters.XML;
using BH.oM.Base.Attributes;
using System.ComponentModel;

namespace BH.Engine.Adapters.XML
{
    public static partial class Create
    {
        [Description("Creates an instance of a GBXMLUnitSetUp object based on the LengthUnit type. If the LengthUnit type is Meters, the Area and Volume Units will become SquareMeters and CubicMeters respectively. Allows for the creation of GBXMLUnitSetUp from just the length and temperature attributes.")]
        [Input("length", "The length unit to build the area and volume units from.")]
        [Input("temperature", "The temperature unit to use, defaults to Celcius")]
        [Output("gbxmlUnitSetUp", "A GBXMLUnitSetUp object containing set up options for the gbXML export.")]
        public static GBXMLUnitSetUp GBXMLUnitSetUp(LengthUnit length, TemperatureUnit temperature = TemperatureUnit.Celcius)
        {
            string type  = length.ToString();

            return new GBXMLUnitSetUp
            {
                LengthUnit = length,
                AreaUnit = (AreaUnit)Enum.Parse(typeof(AreaUnit), "Square" + type),
                TemperatureUnit = temperature,
                VolumeUnit = (VolumeUnit)Enum.Parse(typeof(VolumeUnit), "Cubic" + type),
            };
        }
    }
}



