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
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    [XmlType("Glaze")]
    public class Glaze : GBXMLObject
    {
        [XmlAttribute("id")]
        public string ID { get; set; } = "GlazingIdentification";

        [XmlElement("Name")]
        public string Name { get; set; } = "Glazing";

        [XmlElement("Description")]
        public string Description { get; set; } = "Standard Dbl Glazed";

        [XmlElement("Thickness")]
        public Thickness Thickness { get; set; } = new Thickness();

        [XmlElement("Conductivity")]
        public Conductivity Conductivity { get; set; } = new Conductivity();

        [XmlElement("Transmittance")]
        public Transmittance[] Transmittance { get; set; } = new List<Transmittance> { new Transmittance() }.ToArray();

        [XmlElement("Reflectance")]
        public Reflectance[] Reflectance { get; set; } = new List<Reflectance> { new Reflectance() }.ToArray();

        [XmlElement("Emittance")]
        public Emittance[] Emittance { get; set; } = new List<Emittance> { new Emittance() }.ToArray();
    }
}