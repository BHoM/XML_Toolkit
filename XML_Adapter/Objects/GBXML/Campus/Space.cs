/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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

namespace BH.Adapter.XML.GBXMLSchema
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class Space
    {
        [XmlAttribute(AttributeName = "zoneIdRef")]
        public string ZoneIDRef { get; set; } = "ZoneID";
        [XmlAttribute(AttributeName = "conditionType")]
        public string ConditionType { get; set; } = "Unconditioned";
        [XmlAttribute(AttributeName = "buildingStoreyIdRef")]
        public string BuildingStoreyIDRef { get; set; } = "StoreyID";
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "SpaceID";
        [XmlElement("Area")]
        public double Area { get; set; } = 0;
        [XmlElement("Volume")]
        public double Volume { get; set; } = 0;
        [XmlElement("PlanarGeometry")]
        public PlanarGeometry PlanarGeoemtry { get; set; } = new PlanarGeometry();
        [XmlElement("ShellGeometry")]
        public ShellGeometry ShellGeometry { get; set; } = new ShellGeometry();
        [XmlElement("SpaceBoundary")]
        public SpaceBoundary[] SpaceBoundary { get; set; } = new List<SpaceBoundary> { new SpaceBoundary() }.ToArray();
        [XmlElement("Name")]
        public string Name { get; set; } = "Space";
        [XmlElement("Description")]
        public string Description { get; set; } = "None";
        [XmlElement("CADObjectId")]
        public string CADObjectID { get; set; } = "xxxxxx";
    }
}

