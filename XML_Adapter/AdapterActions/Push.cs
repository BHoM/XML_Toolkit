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

using BH.oM.Data.Requests;
using BH.oM.Adapter;
using BH.oM.Base;
using System.Reflection;
using BH.oM.Adapters.XML;
using BH.oM.Adapters.XML.Enums;
using System.IO;
using BH.Engine.Adapter;

namespace BH.Adapter.XML
{
    public partial class XMLAdapter : BHoMAdapter
    {
        public override List<object> Push(IEnumerable<object> objects, String tag = "", PushType pushType = PushType.AdapterDefault, ActionConfig actionConfig = null)
        {
            // If unset, set the pushType to AdapterSettings' value (base AdapterSettings default is FullCRUD).
            if (pushType == PushType.AdapterDefault)
                pushType = m_AdapterSettings.DefaultPushType;

            if (actionConfig == null)
            {
                BH.Engine.Base.Compute.RecordError("Please provide configuration settings to push to an XML file");
                return new List<object>();
            }

            XMLConfig config = actionConfig as XMLConfig;
            if (config == null)
            {
                BH.Engine.Base.Compute.RecordError("Please provide valid a XMLConfig object for pushing to an XML file");
                return new List<object>();
            }

            IEnumerable<IBHoMObject> objectsToPush = ProcessObjectsForPush(objects, actionConfig); // Note: default Push only supports IBHoMObjects.

            bool success = false;
            switch (config.Schema)
            {
                case Schema.CSProject:
                    success = CreateCSProject(objectsToPush, config);
                    break;
                case Schema.GBXML:
                    success = CreateGBXML(objectsToPush, config);
                    break;
                case Schema.KML:
                    success = CreateKML(objectsToPush, config);
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
                    success = CreateDefault(objectsToPush, config);
                    break;
            }

            if (success && config.RemoveNils)
                RemoveNil(config.File);

            return success ? objects.ToList() : new List<object>();
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



