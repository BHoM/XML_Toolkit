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
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class GBXML : GBXMLObject
    {
        [XmlAttribute(AttributeName = "temperatureUnit")]
        public string TemperatureUnit { get; set; } = "C";

        [XmlAttribute(AttributeName = "lengthUnit")]
        public string LengthUnit { get; set; } = "Meters";

        [XmlAttribute(AttributeName = "areaUnit")]
        public string AreaUnit { get; set; } = "SquareMeters";

        [XmlAttribute(AttributeName = "volumeUnit")]
        public string VolumeUnit { get; set; } = "CubicMeters";

        [XmlAttribute(AttributeName = "useSIUnitsForResults")]
        public string UseSIUnitsForResults { get; set; } = "true";

        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; } = "0.37";

        [XmlElement("Campus")]
        public Campus Campus { get; set; } = new Campus();

        [XmlElement("Construction")]
        public Construction[] Construction { get; set; }

        [XmlElement("Layer")]
        public Layer[] Layer { get; set; }

        [XmlElement("Material")]
        public Material[] Material { get; set; }

        [XmlElement("Zone")]
        public Zone[] Zone { get; set; } = new List<Zone> { new Zone() }.ToArray();

        [XmlElement("WindowType")]
        public WindowType[] WindowType { get; set; } = new List<WindowType> { new WindowType() }.ToArray();

        [XmlElement("DocumentHistory")]
        public DocumentHistory DocumentHistory { get; set; } = new DocumentHistory();
    }
}