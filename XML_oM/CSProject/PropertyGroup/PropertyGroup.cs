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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using BH.oM.Base;

namespace BH.oM.XML.CSProject
{
    [Serializable]
    [XmlRoot(ElementName = "PropertyGroup", IsNullable = false)]
    public class PropertyGroup : CSProjectObject
    {
        [XmlAttribute("Condition")]
        public virtual string Condition { get; set; } = null;

        [XmlElement("Configuration")]
        public virtual Configuration Configuration { get; set; } = null;

        [XmlElement("Platform")]
        public virtual Configuration Platform { get; set; } = null;

        [XmlElement("ProjectGuid")]
        public virtual string ProjectGUID { get; set; } = null;

        [XmlElement("OutputType")]
        public virtual string OutputType { get; set; } = null;

        [XmlElement("AppDesignerFolder")]
        public virtual string AppDesignerFolder { get; set; } = null;

        [XmlElement("RootNamespace")]
        public virtual string RootNamespace { get; set; } = null;

        [XmlElement("AssemblyName")]
        public virtual string AssemblyName { get; set; } = null;

        [XmlElement("TargetFrameworkVersion")]
        public virtual string TargetFrameworkVersion { get; set; } = null;

        [XmlElement("FileAlignment")]
        public virtual string FileAlignment { get; set; } = null;

        [XmlElement("DebugSymbols")]
        public virtual string DebugSymbols { get; set; } = null;

        [XmlElement("DebugType")]
        public virtual string DebugType { get; set; } = null;

        [XmlElement("Optimize")]
        public virtual bool? Optimise { get; set; } = null;

        [XmlElement("OutputPath")]
        public virtual string OutputPath { get; set; } = null;

        [XmlElement("DefineConstants")]
        public virtual string DefineConstants { get; set; } = null;

        [XmlElement("ErrorReport")]
        public virtual string ErrorReport { get; set; } = null;

        [XmlElement("WarningLevel")]
        public virtual int? WarningLevel { get; set; } = null;

        [XmlElement("PostBuildEvent")]
        public virtual string PostBuildEvent { get; set; } = null;

        public bool ShouldSerializeOptimise()
        {
            return Optimise.HasValue;
        }

        public bool ShouldSerializeWarningLevel()
        {
            return WarningLevel.HasValue;
        }
    }
}


