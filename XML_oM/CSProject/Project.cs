/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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

using System.Xml.Serialization;
using BH.oM.Base;

namespace BH.oM.XML.CSProject
{
    [Serializable]
    [XmlRoot(ElementName = "Project", IsNullable = false, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class Project : CSProjectObject
    {
        [XmlAttribute("ToolsVersion")]
        public virtual string ToolsVersion { get; set; } = "";

        [XmlAttribute("DefaultTargets")]
        public virtual string DefaultTargets { get; set; } = "";

        [XmlAttribute("xmlns")]
        public virtual string XMLNamespace { get; set; } = "";

        [XmlElement("Import")]
        public virtual List<Import> Imports { get; set; } = new List<Import>();

        [XmlElement("PropertyGroup")]
        public virtual List<PropertyGroup> PropertyGroups { get; set; } = new List<PropertyGroup>();

        [XmlElement("ItemGroup")]
        public virtual List<ItemGroup> ItemGroups { get; set; } = new List<ItemGroup>();
    }
}



