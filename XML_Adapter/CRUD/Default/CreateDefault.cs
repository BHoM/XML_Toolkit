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
                        line = line.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n", "");
                    sw.WriteLine(line);

                    StringBuilder sb = stringWriter.GetStringBuilder();
                    sb.Remove(0, sb.Length);
                }

                sw.Close();

                /*var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings() { OmitXmlDeclaration = true });
                foreach(T t in objects)
                {
                    XElement e = Blah.ToXML(t);
                    lines.Add(e.ToString());
                }

                */

            }
            catch (Exception e)
            {
                //Try exporting the objects manually if automatic serialisation didn't work
                try
                {
                    List<string> strings = objects.Select(x => (x as object).ToXML()).ToList();

                    StreamWriter sw = new StreamWriter(_fileSettings.GetFullFileName());
                    sw.WriteLine("<BHoM>");
                    strings.ForEach(x => sw.WriteLine(x));
                    sw.WriteLine("</BHoM>");

                    sw.Close();
                }
                catch(Exception ex)
                {
                    BH.Engine.Reflection.Compute.RecordError(e.ToString());
                    success = false;

                }



                BH.Engine.Reflection.Compute.RecordError(e.ToString());
                success = false;
            }

            return success;
        }

        
    }

    public static class Blah
    {
        public static string ToXML(this object o)
        {
            if (o == null)
                return "";

            Type t = o.GetType();


            if (t.IsPrimitive || typeof(string).IsAssignableFrom(t))
                return o.ToString(); //For primitive types, just return the value

            string line = "";
            line += "<" + t.Name + ">";

            PropertyInfo[] props = t.GetProperties();
            if (props.Length == 0)
                line += o.ToString();
            else
            {
                foreach (PropertyInfo pi in props)
                {
                    object value = pi.GetValue(o);
                    line += "<" + pi.Name + ">";

                    if (value != null && typeof(System.Collections.IEnumerable).IsAssignableFrom(value.GetType()))
                    {
                        List<object> list = new List<object>();
                        var enumerator = ((System.Collections.IEnumerable)value).GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            list.Add(enumerator.Current);
                        }

                        bool includeSubHeadings = (list.FirstOrDefault() != null && list.FirstOrDefault().GetType().IsAssignableFrom(typeof(string)));

                        for (int x = 0; x < list.Count; x++)
                        {
                            if (includeSubHeadings)
                                line += "<Item" + x.ToString() + ">";
                            line += list[x].ToXML();

                            if (includeSubHeadings)
                                line += "</Item" + x.ToString() + ">";
                        }
                    }
                    else
                    {
                        line += value.ToXML();
                    }

                    line += "</" + pi.Name + ">";
                }
            }

            line += "</" + t.Name + ">";

            return line;
        }

        /*public static XElement ToXML(this object o)
        {
            Type t = o.GetType();

            List<Type> extraTypes = t.GetProperties()
                .Where(p => p.PropertyType.IsInterface)
                .Select(p =>
                {
                    object val = p.GetValue(o, null);
                    if (val != null)
                        return val.GetType();
                    else
                        return null;
                })
                .ToList();

            extraTypes = BH.Engine.Reflection.Query.BHoMTypeList();

            extraTypes = extraTypes.Where(x => x != null && !x.IsGenericType && !x.IsInterface).ToList();           

            DataContractSerializer serializer = new DataContractSerializer(t, extraTypes);
            StringWriter sw = new StringWriter();
            XmlTextWriter xw = new XmlTextWriter(sw);
            serializer.WriteObject(xw, o);
            return XElement.Parse(sw.ToString());
        }*/
    }
}

