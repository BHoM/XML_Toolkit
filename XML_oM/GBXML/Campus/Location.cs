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
    public class Location : GBXMLObject
    {
        [XmlElement("StationId")]
        public StationID StationID { get; set; } = new StationID();
        [XmlElement("ZipcodeOrPostalCode")]
        public string ZipcodeOrPostalCode { get; set; } = "00000";
        [XmlElement("Longitude")]
        public double Longitude { get; set; } = 0;
        [XmlElement("Latitude")]
        public double Latitude { get; set; } = 0;
        [XmlElement("Elevation")]
        public double Elevation { get; set; } = 0;
        [XmlElement("CADModelAzimuth")]
        public double CADModelAzimuth { get; set; } = 0;
        [XmlElement("Name")]
        public new string Name { get; set; } = "Location";
    }
}