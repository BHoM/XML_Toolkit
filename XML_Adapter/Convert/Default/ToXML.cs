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
        public static string ToXML(this object o, bool includeHead = true)
        {
            if (o == null)
                return "";

            Type objectType = o.GetType();
            if (objectType.IsPrimitiveOrStringOrGuid())
                return o.ToString(); //For primitive types, just return the value

            string line = "";

            if (!objectType.IsEnum)
            {
                //Whatever you change here, make sure you change at the bottom too!
                if(includeHead)
                    line = "<" + objectType.Name + ">";

                line += "<_t>" + objectType.FullName + "</_t>";
            }

            PropertyInfo[] props = objectType.GetProperties();
            if (props.Length == 0)
                line += o.ToString(); //Handles enums and other objects that aren't primitive but have no properties and need their type name setting
            else
            {
                foreach (PropertyInfo pi in props)
                {
                    object value = pi.GetValue(o);

                    if (value != null && typeof(System.Collections.IEnumerable).IsAssignableFrom(value.GetType()))
                    {
                        //Handles lists of objects, ensuring each object is individually serialised
                        List<object> list = new List<object>();
                        var enumerator = ((System.Collections.IEnumerable)value).GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            list.Add(enumerator.Current);
                        }

                        bool includeSubHeadings = (list.FirstOrDefault() != null && list.FirstOrDefault().GetType().IsPrimitiveOrStringOrGuid()); //If the list is just a list of strings or primitives then each item should have its own header to separate it. If it is a list of complex objects then the original pi.name header is sufficent

                        for (int x = 0; x < list.Count; x++)
                        {
                            line += "<" + pi.Name + ">";
                            line += list[x].ToXML(false);
                            line += "</" + pi.Name + ">";
                        }
                    }
                    else if(value != null)
                    {
                        line += "<" + pi.Name + ">";
                        line += value.ToXML(!pi.PropertyType.IsInterface); //If this property is an interface, don't include its type as the header cause it'll confuse the issue when reading it back
                        line += "</" + pi.Name + ">";
                    }
                }
            }

            if (!objectType.IsEnum && includeHead)
                line += "</" + objectType.Name + ">";

            return line;
        }

        private static bool IsPrimitiveOrStringOrGuid(this Type type)
        {
            return type.IsPrimitive || typeof(string).IsAssignableFrom(type) || typeof(Guid).IsAssignableFrom(type);
        }
    }
}

