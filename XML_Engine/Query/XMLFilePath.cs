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

using BHE = BH.oM.Environment.Elements;
using BHX = BH.oM.XML;
using BH.Engine.Environment;

namespace BH.Engine.XML
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static string XMLFilePath(string xmlFileName, string xmlDirectoryPath, BHX.Enums.ExportType exportType = BHX.Enums.ExportType.gbXMLTAS)
        {
            switch(exportType)
            {
                case oM.XML.Enums.ExportType.gbXMLIES:
                    return System.IO.Path.Combine(xmlDirectoryPath, xmlFileName + "_IES.xml");
                case BHX.Enums.ExportType.gbXMLTAS:
                    return System.IO.Path.Combine(xmlDirectoryPath, xmlFileName + "_TAS.xml");
                default:
                    return System.IO.Path.Combine(xmlDirectoryPath, xmlFileName + ".xml");
            }
        }
    }
}
