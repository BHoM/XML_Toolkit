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
using System.Xml;
using System.IO;
using System.Reflection;

using BH.oM.Adapters.XML;
using BH.oM.Adapters.XML.Settings;
using BH.oM.Environment.Elements;
using BH.oM.Base;
using BH.Adapter.XML.GBXMLSchema;

using BH.Engine.Adapter;
using BH.Engine.Adapters.XML;

using System.Runtime.Serialization;
using System.Xml.Linq;

namespace BH.Adapter.XML
{
    public partial class XMLAdapter : BHoMAdapter
    {
        private bool CreateDefault<T>(IEnumerable<T> objects, XMLConfig config)
        {
            bool success = true;

            try
            {
                System.Reflection.PropertyInfo[] bhomProperties = typeof(BHoMObject).GetProperties();
                XmlAttributeOverrides overrides = new XmlAttributeOverrides();

                foreach (System.Reflection.PropertyInfo pi in bhomProperties)
                    overrides.Add(typeof(BHoMObject), pi.Name, new XmlAttributes { XmlIgnore = true });

                var exportType = typeof(T);
                if (exportType == typeof(IBHoMObject))
                    exportType = objects.First().GetType();

                XmlSerializerNamespaces xns = new XmlSerializerNamespaces();
                XmlSerializer szer = new XmlSerializer(exportType, overrides);
                TextWriter ms = new StreamWriter(config.File.GetFullFileName());
                foreach (var obj in objects)
                {
                    try
                    {
                        szer.Serialize(ms, obj, xns);
                    }
                    catch(Exception e)
                    {
                        BH.Engine.Base.Compute.RecordError($"Error occurred when serialising object {obj.GetType()}. Error received was: {e.ToString()}.");
                    }
                }
                ms.Close();
            }
            catch (Exception e)
            {
                BH.Engine.Base.Compute.RecordError($"Error serialising objects to XML. Error received: {e.ToString()}.");
                success = false;
            }

            return success;
        }
    }
}





