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

using System.Reflection;

namespace BH.Adapter.XML
{
    public static partial class Convert
    {
        public static string ToXML(this object o)
        {
            if (o == null)
                return "";

            Type objectType = o.GetType();
            if (objectType.IsPrimitiveOrString())
                return o.ToString(); //For primitive types, just return the value

            string line = "<" + objectType.Name + ">";

            PropertyInfo[] props = objectType.GetProperties();
            if (props.Length == 0)
                line += o.ToString(); //Handles enums and other objects that aren't primitive but have no properties and need their type name setting
            else
            {
                foreach (PropertyInfo pi in props)
                {
                    object value = pi.GetValue(o);
                    line += "<" + pi.Name + ">";

                    if (value != null && typeof(System.Collections.IEnumerable).IsAssignableFrom(value.GetType()))
                    {
                        //Handles lists of objects, ensuring each object is individually serialised
                        List<object> list = new List<object>();
                        var enumerator = ((System.Collections.IEnumerable)value).GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            list.Add(enumerator.Current);
                        }

                        bool includeSubHeadings = (list.FirstOrDefault() != null && list.FirstOrDefault().GetType().IsPrimitiveOrString()); //If the list is just a list of strings or primitives then each item should have its own header to separate it. If it is a list of complex objects then the original pi.name header is sufficent

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
                        line += value.ToXML();

                    line += "</" + pi.Name + ">";
                }
            }

            line += "</" + objectType.Name + ">";

            return line;
        }

        private static bool IsPrimitiveOrString(this Type type)
        {
            return type.IsPrimitive || typeof(string).IsAssignableFrom(type);
        }
    }
}

