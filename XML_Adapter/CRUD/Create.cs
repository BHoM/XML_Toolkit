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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BH.oM.Adapters.XML;
using BH.oM.Adapters.XML.Enums;

using BH.oM.Adapter;

namespace BH.Adapter.XML
{
    public partial class XMLAdapter : BHoMAdapter
    {
        protected override bool ICreate<T>(IEnumerable<T> objects, ActionConfig actionConfig = null)
        {
            if(actionConfig == null)
            {
                BH.Engine.Reflection.Compute.RecordError("Please provide configuration settings to push to an XML file");
                return false;
            }

            XMLConfig config = actionConfig as XMLConfig;
            if(config == null)
            {
                BH.Engine.Reflection.Compute.RecordError("Please provide valid a XMLConfig object for pushing to an XML file");
                return false;
            }

            switch(config.Schema)
            {
                case Schema.CSProject:
                    return CreateCSProject(objects, config);
                case Schema.GBXML:
                    return CreateGBXML(objects, config);
                case Schema.KML:
                    return CreateKML(objects, config);
                case Schema.EnergyPlusLoads:
                    BH.Engine.Reflection.Compute.RecordError("The EnergyPlusLoads Schema is not supported for push operations at this time");
                    return false;
                default:
                    BH.Engine.Reflection.Compute.RecordError("The XML Schema you have supplied is not currently supported by the XML Toolkit");
                    return false;
            }
        }
    }
}
