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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

using BH.oM.Adapters.XML;
using BH.oM.Adapters.XML.Settings;
using BH.oM.Environment.Elements;
using BH.oM.Base;
using BH.Adapter.XML.GBXMLSchema;

using BH.Engine.Adapter;
using BH.Engine.Adapters.XML;

using BH.oM.Spatial.SettingOut;
using BH.Engine.Environment;

namespace BH.Adapter.XML
{
    public partial class XMLAdapter : BHoMAdapter
    {
        private bool CreateGBXML<T>(IEnumerable<T> objects, XMLConfig config)
        {
            bool success = true;

            if(config.Settings == null)
            {
                BH.Engine.Base.Compute.RecordError("Please provide a suitable set of GBXMLSettings with the XMLConfig to determine how the GBXML file should be created");
                return false;
            }

            GBXMLSettings settings = config.Settings as GBXMLSettings;
            if(settings == null)
            {
                BH.Engine.Base.Compute.RecordError("Please provide a suitable set of GBXMLSettings with the XMLConfig to determine how the GBXML file should be created");
                return false;
            }

            List<Panel> panels = objects.Where(x => x.GetType() == typeof(Panel)).Cast<Panel>().ToList();

            List<IBHoMObject> bhomObjects = new List<IBHoMObject>();
            bhomObjects.AddRange(objects.Where(x => x.GetType() == typeof(oM.Environment.Elements.Building)).Cast<oM.Environment.Elements.Building>());
            bhomObjects.AddRange(objects.Where(x => x.GetType() == typeof(Level)).Cast<Level>());

            if (settings.ExportDetail == oM.Adapters.XML.Enums.ExportDetail.Full)
                bhomObjects.AddRange(panels);
            else if(settings.ExportDetail == oM.Adapters.XML.Enums.ExportDetail.BuildingShell)
                bhomObjects.AddRange(panels.ToSpaces().ExternalElements());
            else
            {
                BH.Engine.Base.Compute.RecordError("The ExportDetail has not been appropriately set. Please set the ExportDetail to continue");
                return false;
            }

            GBXML gbx = bhomObjects.ToGBXML(settings);

            try
            {
                System.Reflection.PropertyInfo[] bhomProperties = typeof(BHoMObject).GetProperties();
                XmlAttributeOverrides overrides = new XmlAttributeOverrides();

                foreach (System.Reflection.PropertyInfo pi in bhomProperties)
                    overrides.Add(typeof(BHoMObject), pi.Name, new XmlAttributes { XmlIgnore = true });

                XmlSerializerNamespaces xns = new XmlSerializerNamespaces();
                XmlSerializer szer = new XmlSerializer(typeof(BH.Adapter.XML.GBXMLSchema.GBXML), overrides);
                TextWriter ms = new StreamWriter(config.File.GetFullFileName());
                szer.Serialize(ms, gbx, xns);
                ms.Close();
            }
            catch (Exception e)
            {
                BH.Engine.Base.Compute.RecordError(e.ToString());
                success = false;
            }

            return success;
        }
    }
}





