/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2019, the respective contributors. All rights reserved.
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

using BH.oM.XML.Settings;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.XML
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Create a FileSettings object for use with the XML Adapter")]
        [Input("fileName", "Name of XML file, not including the file extension. Default 'BHoM_gbXML_Export'")]
        [Input("directory", "Path to XML file. Defaults to your desktop")]
        [Output("fileSettings", "The file settings to use with the XML adapter for pull and push")]
        public static FileSettings FileSettings(string fileName = "BHoM_gbXML_Export", string directory = null)
        {
            if (directory == null)
                directory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);

            if(System.IO.Path.HasExtension(fileName) && System.IO.Path.GetExtension(fileName) == ".xml")
            {
                BH.Engine.Reflection.Compute.RecordError("File name cannot contain a file extension");
                return null;
            }

            return new FileSettings
            {
                Directory = directory,
                FileName = fileName,
            };
        }
    }
}
