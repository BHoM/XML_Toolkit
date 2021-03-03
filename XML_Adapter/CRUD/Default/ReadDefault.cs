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
            XmlTextReader reader = new XmlTextReader(_fileSettings.GetFullFileName());
            XmlDocument doc = new XmlDocument();
            XmlNode node = doc.ReadNode(reader);

            if (node.NodeType == XmlNodeType.XmlDeclaration)
                node = doc.ReadNode(reader); //Try the next node...

            string obj = ExtractData(node);

            if (obj.EndsWith(","))
                obj = obj.Substring(0, obj.Length - 1);

            obj = "{" + obj + "}"; //Wrap the json string

            return new List<CustomObject>() { BH.Engine.Serialiser.Convert.FromJson(obj) as CustomObject };
        }

        private string ExtractData(XmlNode node, string previousName = null)
        {
            //This is a recursive method
            if (node == null || node.Name.ToLower().Contains("whitespace"))
                return "";

            string line = "";

            if (previousName != node.Name.Replace("#", ""))
            {
                previousName = node.Name.Replace("#", "");
                line += "\"" + previousName + "\":";
            }

            if (node.HasChildNodes)
            {
                line += "{";

                List<XmlNode> nodes1 = new List<XmlNode>();
                foreach (XmlNode n in node.ChildNodes)
                    nodes1.Add(n);

                var nodes = nodes1.GroupBy(x => x.Name.Replace("#", ""));
                foreach (var group in nodes)
                {
                    if (group.Key.ToLower().Contains("whitespace"))
                        continue;

                    if (group.Count() > 1)
                    {
                        line += "\"" + group.Key + "\":[";
                        previousName = group.Key;
                    }

                    foreach (var v in group)
                    {
                        line += ExtractData(v, previousName);
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
