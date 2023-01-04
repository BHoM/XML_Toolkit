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

                StringWriter stringWriter = new StringWriter();

                XmlSerializerNamespaces xns = new XmlSerializerNamespaces();
                xns.Add("", "");
                XmlSerializer szer = new XmlSerializer(typeof(T), overrides);

                StreamWriter sw = new StreamWriter(_fileSettings.GetFullFileName());

                List<T> obj = objects.ToList();

                for(int x = 0; x < obj.Count; x++)
                {
                    szer.Serialize(stringWriter, obj[x], xns);
                    string line = stringWriter.ToString();
                    if (x > 0)
                        line = line.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n", ""); //Don't need this header on every item
                    sw.WriteLine(line);

                    StringBuilder sb = stringWriter.GetStringBuilder();
                    sb.Remove(0, sb.Length);
                }

                sw.Close();
            }
            catch (Exception e)
            {
                //Try exporting the objects manually if automatic serialisation didn't work
                try
                {
                    List<string> xmlStrings = objects.Select(x => (x as object).ToXML()).ToList();

                    string xml = "<BHoM>";
                    xmlStrings.ForEach(x => xml += x);
                    xml += "</BHoM>";

                    XmlDocument xdoc = new XmlDocument();
                    xdoc.LoadXml(xml);
                    xdoc.Save(_fileSettings.GetFullFileName());
                }
                catch(Exception ex)
                {
                    //Well something went terribly wrong so lets give the user both error messages to help with debugging
                    BH.Engine.Base.Compute.RecordError("An error occurred in serialising the objects of type " + objects.GetType() + ". Error messages will follow.");
                    BH.Engine.Base.Compute.RecordError(e.ToString());
                    BH.Engine.Base.Compute.RecordError(ex.ToString());
                    success = false;
                }
            }

            return success;
        }
    }
}



