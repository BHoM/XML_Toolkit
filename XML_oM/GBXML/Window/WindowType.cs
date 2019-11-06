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
    public class WindowType : GBXMLObject
    {
        [XmlAttribute("id")]
        public string ID { get; set; } = null;

        [XmlElement("Name", Order = 1)]
        public new string Name { get; set; } = null;

        [XmlElement("Description", Order = 2)]
        public string Description { get; set; } = null;

        [XmlElement("U-value", Order = 3)]
        public UValue UValue { get; set; } = new UValue();

        [XmlElement("SolarHeatGainCoeff", Order = 4)]
        public SolarHeatGainCoefficient SolarHeatGainCoefficient { get; set; } = new SolarHeatGainCoefficient();

        [XmlElement("Transmittance", Order = 5)]
        public Transmittance Transmittance { get; set; } = new Transmittance();

        [XmlElement(ElementName = "Glaze", Order = 6)]
        public Glaze InternalGlaze { get; set; } = null;

        [XmlElement("Gap", Order = 7)]
        public Gap Gap { get; set; } = null;

        [XmlElement(ElementName = "Glaze", Order = 8)]
        public Glaze ExternalGlaze { get; set; } = null;
    }
}