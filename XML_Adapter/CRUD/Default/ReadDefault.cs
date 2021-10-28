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
using System.Xml.Serialization;
using System.IO;

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
using BH.Engine.Geometry;
using BH.oM.Environment.Elements;
using BH.Engine.Environment;

using System.Xml;

namespace BH.Adapter.XML
{
    public partial class XMLAdapter : BHoMAdapter
    {
        private IEnumerable<IBHoMObject> ReadDefault(Type type = null, XMLConfig config = null)
        {
            //Most XML files include header information, which causes the XmlTextReader to fail. Removing the header information fixes this failure. 
            //Assuming the header information isn't important to us, we can remove it. 
            List<string> docLines = File.ReadAllLines(_fileSettings.GetFullFileName()).ToList();
            if (docLines[0].StartsWith(@"<?xml"))
                docLines.RemoveAt(0);

            //Due to needing to read an XML file without a schema, we read each node instead and convert it to a JSON string which we then deserialise via Serialiser_Engine to get the custom objects
            XmlTextReader reader = new XmlTextReader(new StringReader(string.Join(Environment.NewLine, docLines)));
            XmlDocument doc = new XmlDocument();
            XmlNode node = doc.ReadNode(reader);

            if (node.NodeType == XmlNodeType.XmlDeclaration)
                node = doc.ReadNode(reader); //Try the next node...

            bool isBHoM = node.Name.ToLower() == "bhom";

            string obj = ExtractData(node, null, isBHoM);

            if (obj.EndsWith(","))
                obj = obj.Substring(0, obj.Length - 1);

            if(obj.StartsWith("\"BHoM\":{"))
            {
                //An XML deserialised that was previously serialised as BHoM - remove the BHoM heading
                obj = obj.Replace("\"BHoM\":{", "{");
            }
            else
                obj = "{" + obj + "}"; //Wrap the json string

            return new List<CustomObject>() { BH.Engine.Serialiser.Convert.FromJson(obj) as CustomObject };
        }

        private string ExtractData(XmlNode node, string previousName = null, bool isBHoM = false)
        {
            //This is a recursive method
            if (node == null || node.Name.ToLower().Contains("whitespace"))
                return "";

            string line = "";

            string nodeName = node.Name.Replace("#", "");

            if (previousName != nodeName)
            {
                if (!isBHoM || (isBHoM && nodeName.ToLower() != "text"))
                {
                    previousName = nodeName;
                    line += "\"" + previousName + "\":";
                }
            }

            if (node.HasChildNodes)
            {
                List<XmlNode> nodes1 = new List<XmlNode>();
                foreach (XmlNode n in node.ChildNodes)
                    nodes1.Add(n);

                var nodes = nodes1.GroupBy(x => x.Name.Replace("#", ""));

                if(nodes.Any(x => x.Key.ToLower() != "text") || !isBHoM)
                    line += "{";

                foreach (var group in nodes)
                {
                    if (group.Key.ToLower().Contains("whitespace"))
                        continue;

                    if (group.Key.ToLower() == previousName.ToLower() && group.Count() == 1)
                        line += "\"" + previousName + "\":{";

                    if (group.Count() > 1)
                    {
                        line += "\"" + group.Key + "\":[";
                        previousName = group.Key;
                    }

                    foreach (var v in group)
                    {
                        string s = ExtractData(v, previousName, isBHoM);
                        if(!string.IsNullOrEmpty(s) && s != "\"\",")
                            line += s;
                    }

                    if (group.Key.ToLower() == previousName.ToLower() && group.Count() == 1)
                    {
                        if (line.EndsWith(","))
                            line = line.Substring(0, line.Length - 1); //Remove last ','
                        line += "},";
                    }

                    if (group.Count() > 1)
                    {
                        if (line.EndsWith(","))
                            line = line.Substring(0, line.Length - 1); //Remove last ','
                        line += "],";
                    }
                }

                if (line.EndsWith(","))
                    line = line.Substring(0, line.Length - 1); //Remove last ','

                if (nodes.Any(x => x.Key.ToLower() != "text") || !isBHoM)
                    line += "}";
            }
            else if (node.Value != null)
                line += "\"" + node.Value.Replace("\r", "").Replace("\n", "").Replace("\"", "'").Trim() + "\"";
            else
                line += "\"\""; //Empty element

            return line + ",";
        }
    }
}
