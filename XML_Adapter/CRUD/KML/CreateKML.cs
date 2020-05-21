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
using System.Xml.Serialization;
using System.IO;

using BH.oM.Adapters.XML;
using KML = BH.oM.Adapters.XML;
using KMLSchema = BH.oM.Adapters.XML.KMLSchema;
using BH.oM.Adapters.XML.Settings;
using BH.oM.Environment.Elements;
using BH.oM.Base;

using BH.Engine.Adapter;
using BH.Engine.Adapters.XML;

namespace BH.Adapter.XML
{
    public partial class XMLAdapter : BHoMAdapter
    {
        private bool CreateKML<T>(IEnumerable<T> objects, XMLConfig config)
        {
            bool success = true;

            if (config.Settings == null)
            {
                BH.Engine.Reflection.Compute.RecordError("Please provide a suitable set of KMLSettings with the XMLConfig to determine how the KML file should be created");
                return false;
            }

            KMLSettings settings = config.Settings as KMLSettings;
            if (settings == null)
            {
                BH.Engine.Reflection.Compute.RecordError("Please provide a suitable set of KMLSettings with the XMLConfig to determine how the KML file should be created");
                return false;
            }

            KMLDocumentBuilder docBuilder = objects.ToList()[0] as KML.KMLDocumentBuilder;

            if (docBuilder == null)
            {
                BH.Engine.Reflection.Compute.RecordError("The KML schema requires a full model to be provided as a single push operation. For pushing to the KML version, you need to plug your objects into a KMLDocumentBuilder which collates the objects for pushing and push that to KML via this adapter.");
                return false;
            }

            KMLSchema.KML kml = docBuilder.ToKML(settings);

            try
            {
                System.Reflection.PropertyInfo[] bhomProperties = typeof(BHoMObject).GetProperties();
                XmlAttributeOverrides overrides = new XmlAttributeOverrides();

                foreach (System.Reflection.PropertyInfo pi in bhomProperties)
                    overrides.Add(typeof(BHoMObject), pi.Name, new XmlAttributes { XmlIgnore = true });

                XmlSerializerNamespaces xns = new XmlSerializerNamespaces();
                XmlSerializer szer = new XmlSerializer(typeof(BH.oM.Adapters.XML.KMLSchema.KML), overrides);
                TextWriter ms = new StreamWriter(_fileSettings.GetFullFileName());
                szer.Serialize(ms, kml, xns);
                ms.Close();
            }
            catch (Exception e)
            {
                BH.Engine.Reflection.Compute.RecordError(e.ToString());
                success = false;
            }

            return success;
        }


    }
}
