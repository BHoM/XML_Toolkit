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
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using BH.oM.Base;
using BHE = BH.oM.Environment.Elements;
using BH.oM.Environment.Fragments;
using BHG = BH.oM.Geometry;

using BH.oM.Adapters.XML;
using BH.oM.Adapters.XML.Enums;
using BHX = BH.Adapter.XML.GBXMLSchema;
using BHC = BH.oM.Physical.Constructions;

using BH.oM.Adapter;
using BH.Engine.Adapter;

using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace BH.Adapter.XML
{
    public partial class XMLAdapter : BHoMAdapter
    {
        
        protected override IEnumerable<IBHoMObject> IRead(Type type, IList indices = null, ActionConfig actionConfig = null)
        {
            if (actionConfig == null)
            {
                BH.Engine.Reflection.Compute.RecordError("Please provide configuration settings to push to an XML file");
                return new List<IBHoMObject>();
            }

            XMLConfig config = actionConfig as XMLConfig;
            if (config == null)
            {
                BH.Engine.Reflection.Compute.RecordError("Please provide valid a XMLConfig object for pushing to an XML file");
                return new List<IBHoMObject>();
            }

            switch (config.Schema)
            {
                case Schema.CSProject:
                    return ReadCSProject(type, config);
                case Schema.EnergyPlusLoads:
                    return ReadEnergyPlus(type, config);
                case Schema.GBXML:
                    return ReadGBXML(type, config);
                case Schema.KML:
                    BH.Engine.Reflection.Compute.RecordError("The KML Schema is not supported for pull operations at this time");
                    return new List<IBHoMObject>();
                default:
                    BH.Engine.Reflection.Compute.RecordError("The XML Schema you have supplied is not currently supported by the XML Toolkit");
                    return ReadBasicFile();// new List<IBHoMObject>();
            }
        }

        private IEnumerable<CustomObject> ReadBasicFile()
        {
            /*string line = "";
            StreamReader sr = new StreamReader(_fileSettings.GetFullFileName());
            line = sr.ReadLine();

            if (line == null)
            {
                sr.Close();
                return null;
            }

            if(line.StartsWith("<?xml"))
            {
                int index = line.IndexOf("?>");
                if (line.Length == index + 1)
                    line = sr.ReadLine(); //Go for the next line
                else
                    line = line.Substring(index + 1);
            }

            sr.Close();

            int start = line.IndexOf("<");
            int end = line.IndexOf(" ");
            line = line.Substring(start + 1, (end - start));

            TextReader reader = new StreamReader(_fileSettings.GetFullFileName());
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = line;
            xRoot.IsNullable = true;

            XmlSerializer szer = new XmlSerializer(typeof(Blah), xRoot);
            Blah obj = (Blah)szer.Deserialize(reader);
            reader.Close();

            string s = BH.Engine.Serialiser.Convert.ToJson(obj);

            return new List<CustomObject>() { BH.Engine.Serialiser.Convert.FromJson(s) as CustomObject };*/

            XmlTextReader reader = new XmlTextReader(_fileSettings.GetFullFileName());
            XmlDocument doc = new XmlDocument();
            XmlNode node = doc.ReadNode(reader);

            if (node.NodeType == XmlNodeType.XmlDeclaration)
                node = doc.ReadNode(reader); //Try the next node...

            string obj = Recursive(node);

            if (obj.EndsWith(","))
                obj = obj.Substring(0, obj.Length - 1);

            obj = "{" + obj + "}";

            //string s = BH.Engine.Serialiser.Convert.ToJson(node);

            return new List<CustomObject>() { BH.Engine.Serialiser.Convert.FromJson(obj) as CustomObject };
        }

        private string Recursive(XmlNode node, string previousName = null)
        {
            if (node == null || node.Name.ToLower().Contains("whitespace"))
                return "";

            string line = "";

            if (previousName != node.Name.Replace("#", ""))
            {
                previousName = node.Name.Replace("#", "");
                line += "\"" + previousName + "\":";
            }

            //string line = "";

            if (node.HasChildNodes)
            {
                line += "{";

                List<XmlNode> nodes1 = new List<XmlNode>();
                foreach (XmlNode n in node.ChildNodes)
                    nodes1.Add(n);

                var nodes = nodes1.GroupBy(x => x.Name.Replace("#", ""));
                foreach(var group in nodes)
                {
                    if (group.Count() > 1)
                    {
                        line += "\"" + group.Key + "\":[";
                        previousName = group.Key;
                    }

                    foreach(var v in group)
                    {
                        line += Recursive(v, previousName);
                    }

                    if (group.Count() > 1)
                    {
                        if(line.EndsWith(","))
                            line = line.Substring(0, line.Length - 1); //Remove last ','
                        line += "],";
                    }
                }

                /*foreach (XmlNode n in node.ChildNodes)
                {
                    _nodes[parentName].Add(n);
                    //line += Recursive(n, parentName);
                    Dictionary<string, string> l = Recursive(n, parentName);
                }*/

                if(line.EndsWith(","))
                    line = line.Substring(0, line.Length - 1); //Remove last ','

                line += "}";
            }
            else if (node.Value != null)
                line += "\"" + node.Value.Replace("\r", "").Replace("\n", "").Replace("\"", "'").Trim() + "\"";
            else
                line += "\"\"";

            return line + ",";
            //return new Dictionary<string, string>().Add(parentName, line);
        }
    }

    public class Blah { public object Property = null; }
}
