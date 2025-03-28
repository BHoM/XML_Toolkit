/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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
    public class ProgramInfo
    {
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "BHoMGBXML";
        [XmlElement("CompanyName")]
        public string CompanyName { get; set; } = "BuroHappold Engineering";
        [XmlElement("ProductName")]
        public string ProductName { get; set; } = "Autodesk Revit 2018 BEES";
        [XmlElement("Version")]
        public string Version { get; set; } = "2018 20170223_1515(x64)";
        [XmlElement("Platform")]
        public string Platform { get; set; } = "Microsoft Windows";
        [XmlElement("ProjectEntity")]
        public ProjectEntity ProjectEntity { get; set; } = new ProjectEntity();
    }
}





