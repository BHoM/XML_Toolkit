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
using System.Xml.Serialization;

namespace BH.oM.Adapters.XML.KMLSchema
{
    [Serializable]
    [XmlRoot(ElementName = "kml", IsNullable = false, Namespace = "http://www.opengis.net/kml/2.2")]
    public class ColourStyle 
    {
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "";

        [XmlElement("color")]
        public string Colour { get; set; } = "46000000";

        [XmlElement("colorMode")]
        public ColourMode ColourMode { get; set; } = ColourMode.Normal;
    }
}


