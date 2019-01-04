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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BH.oM.Base;
using BHE = BH.oM.Environment;
using BH.oM.XML;
using BH.oM.XML.Enums;

namespace BH.Adapter.XML
{
    public partial class XMLAdapter : BHoMAdapter
    {
        protected override bool Create<T>(IEnumerable<T> objects, bool replaceAll = false)
        {
            string fileName = ProjectName;
            switch(ExportType)
            {
                case ExportType.gbXMLIES:
                    fileName += "_IES";
                    break;
                case ExportType.gbXMLTAS:
                    fileName += "_TAS";
                    break;
                default :
                    fileName += "";
                    break;
            }

            GBXML gbx = new GBXML();
            if (typeof(IBHoMObject).IsAssignableFrom(typeof(T)))
            {
                XML.GBXMLSerializer.Serialize(objects, gbx, ExportType, ExportDetail, FilePath, ProjectName);
                XMLWriter.Save(FilePath, fileName, gbx);
            }

            return true;
        }
    }
}