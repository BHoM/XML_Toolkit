/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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
using BH.oM.Environment.Elements;
using BH.oM.Base;

using BH.Engine.Adapter;
using BH.Engine.Adapters.XML;
using BH.oM.XML.CSProject;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace BH.Adapter.XML
{
    public partial class XMLAdapter : BHoMAdapter
    {
        private bool CreateCSProject<T>(IEnumerable<T> objects, XMLConfig config)
        {
            bool success = true;

            Project doc = objects.ToList()[0] as Project;

            if (doc == null)
            {
                BH.Engine.Base.Compute.RecordError("The CSProject schema requires a full system to be provided as a single push operation.");
                return false;
            }

            try
            {
                System.Reflection.PropertyInfo[] bhomProperties = typeof(BHoMObject).GetProperties();
                XmlAttributeOverrides overrides = new XmlAttributeOverrides();

                foreach (System.Reflection.PropertyInfo pi in bhomProperties)
                    overrides.Add(typeof(BHoMObject), pi.Name, new XmlAttributes { XmlIgnore = true });

                XmlSerializerNamespaces xns = new XmlSerializerNamespaces();
                xns.Add("", "");

                List<string> xmlParts = new List<string>();

                xmlParts.Add("<Project ToolsVersion=\"" + doc.ToolsVersion + "\" DefaultTarget=\"" + doc.DefaultTargets + "\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">");

                StringWriter textWriter = new StringWriter();

                XmlSerializer szer = new XmlSerializer(typeof(BH.oM.XML.CSProject.Import), overrides);
                for (int x = 0; x < doc.Imports.Count - 1; x++)
                {
                    szer.Serialize(textWriter, doc.Imports[x], xns);
                    xmlParts.Add(textWriter.ToString());
                    textWriter = new StringWriter(); //To be safe
                }

                szer = new XmlSerializer(typeof(BH.oM.XML.CSProject.PropertyGroup), overrides);
                for (int x = 0; x < doc.PropertyGroups.Count - 1; x++)
                {
                    szer.Serialize(textWriter, doc.PropertyGroups[x], xns);
                    xmlParts.Add(textWriter.ToString());
                    textWriter = new StringWriter(); //To be safe
                }

                szer = new XmlSerializer(typeof(BH.oM.XML.CSProject.ItemGroup), overrides);
                foreach (var i in doc.ItemGroups)
                {
                    szer.Serialize(textWriter, i, xns);
                    xmlParts.Add(textWriter.ToString());
                    textWriter = new StringWriter();
                }

                szer = new XmlSerializer(typeof(BH.oM.XML.CSProject.Import), overrides);
                szer.Serialize(textWriter, doc.Imports.Last(), xns);
                xmlParts.Add(textWriter.ToString());
                textWriter = new StringWriter();

                szer = new XmlSerializer(typeof(BH.oM.XML.CSProject.PropertyGroup), overrides);
                szer.Serialize(textWriter, doc.PropertyGroups.Last(), xns);
                xmlParts.Add(textWriter.ToString());
                textWriter = new StringWriter();

                xmlParts.Add("</Project>");

                StreamWriter sw = new StreamWriter(_fileSettings.GetFullFileName());

                xmlParts = xmlParts.Select(x => Regex.Replace(x, @"<\?xml version=""1.0"" encoding=""utf-[0-9]*""\?>\r\n", "")).ToList();
                xmlParts = xmlParts.Select(x => x.Replace("q1:", "")).ToList();

                sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");

                foreach (string s in xmlParts)
                    sw.WriteLine(s);

                sw.Close();
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


