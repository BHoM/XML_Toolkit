/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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

using BH.Engine;
using BH.oM.Base;
using System.Reflection;

using BH.oM.XML.Enums;
using BH.oM.Reflection.Attributes;
using System.ComponentModel;
using BH.oM.Data.Requests;
using System.IO;

using BH.oM.XML.Settings;

namespace BH.Adapter.XML
{
    public partial class XMLAdapter : BHoMAdapter
    {
        [Description("Specify XML file and properties for data transfer")]
        [Input("fileSettings", "Input the file settings to get the file name and directory the XML Adapter should use")]
        [Input("xmlSettings", "Input the additional XML Settings the adapter should use. Only used when pushing to an XML file. Default null")]
        [Output("adapter", "Adapter to XML")]
        public XMLAdapter(BH.oM.Adapter.FileSettings fileSettings = null, IXMLSettings xmlSettings = null)
        {
            if(fileSettings == null)
            {
                BH.Engine.Reflection.Compute.RecordError("Please set the File Settings correctly to enable the XML Adapter to work correctly");
                return;
            }

            if(!Path.HasExtension(fileSettings.FileName) || Path.GetExtension(fileSettings.FileName) != ".xml")
            {
                BH.Engine.Reflection.Compute.RecordError("File name must contain a file extension");
                return;
            }

            _fileSettings = fileSettings;
            _xmlSettings = xmlSettings;

            AdapterIdName = "XML_Adapter";
        }

        private BH.oM.Adapter.FileSettings _fileSettings { get; set; } = null;
        private GBXMLSettings _xmlSettings { get; set; } = null;
    }
}
