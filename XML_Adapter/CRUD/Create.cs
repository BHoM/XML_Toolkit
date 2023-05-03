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

using BH.oM.Adapters.XML;
using BH.oM.Adapters.XML.Enums;

using BH.oM.Adapter;
using System.IO;

namespace BH.Adapter.XML
{
    public partial class XMLAdapter : BHoMAdapter
    {
        protected override bool ICreate<T>(IEnumerable<T> objects, ActionConfig actionConfig = null)
        {
            if(actionConfig == null)
            {
                BH.Engine.Base.Compute.RecordError("Please provide configuration settings to push to an XML file");
                return false;
            }

            XMLConfig config = actionConfig as XMLConfig;
            if(config == null)
            {
                BH.Engine.Base.Compute.RecordError("Please provide valid a XMLConfig object for pushing to an XML file");
                return false;
            }

            bool success = false;

            switch(config.Schema)
            {
                case Schema.CSProject:
                    success = CreateCSProject(objects, config);
                    break;
                case Schema.GBXML:
                    success = CreateGBXML(objects, config);
                    break;
                case Schema.KML:
                    success = CreateKML(objects, config);
                    break;
                case Schema.EnergyPlusLoads:
                    BH.Engine.Base.Compute.RecordError("The EnergyPlusLoads Schema is not supported for push operations at this time");
                    success = false;
                    break;
                case Schema.Bluebeam:
                    BH.Engine.Base.Compute.RecordError("The Bluebeam markup schema is not supported for push operations at this time.");
                    success = false;
                    break;
                default:
                    success = CreateDefault(objects, config);
                    break;
            }

            if (success && config.RemoveNils)
                RemoveNil(_fileSettings);

            return success;
        }

        private static bool RemoveNil(FileSettings file)
        {
            var path = Path.Combine(file.Directory, file.FileName);
            var xmlFile = File.ReadAllLines(path);

            xmlFile = xmlFile.Where(x => !x.Trim().Contains("xsi:nil")).ToArray();
            xmlFile = xmlFile.Where(x => x != null).ToArray();

            File.Delete(path);
            File.WriteAllLines(path, xmlFile);

            return true;
        }
    }
}


