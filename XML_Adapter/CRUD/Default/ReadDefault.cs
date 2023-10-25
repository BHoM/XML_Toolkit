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
            object obj = null;
            try
            {
                System.Reflection.PropertyInfo[] bhomProperties = typeof(BHoMObject).GetProperties();
                XmlAttributeOverrides overrides = new XmlAttributeOverrides();

                foreach (System.Reflection.PropertyInfo pi in bhomProperties)
                    overrides.Add(typeof(BHoMObject), pi.Name, new XmlAttributes { XmlIgnore = true });

                TextReader reader = new StreamReader(config.File.GetFullFileName());
                XmlSerializer szer = new XmlSerializer(type, overrides);
                obj = System.Convert.ChangeType(szer.Deserialize(reader), type);
                reader.Close();
            }
            catch(Exception e)
            {
                BH.Engine.Base.Compute.RecordError($"Error occurred while deserialising the XML to object of type {type}. Error received was: {e.ToString()}.");
                return null;
            }

            try
            {
                var bhomObj = (IBHoMObject)obj;
                return new List<IBHoMObject> { bhomObj };
            }
            catch(Exception e)
            {
                BH.Engine.Base.Compute.RecordWarning($"Could not cast deserialised object back to an IBHoMObject. Data returned as CustomObject instead.");
                CustomObject cObj = new CustomObject();
                cObj.CustomData["Data"] = obj;
                return new List<IBHoMObject> { cObj };
            }
        }
    }
}


