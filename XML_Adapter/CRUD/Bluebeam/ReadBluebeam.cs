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

namespace BH.Adapter.XML
{
    public partial class XMLAdapter : BHoMAdapter
    {
        private IEnumerable<IBHoMObject> ReadBluebeam(Type type = null, XMLConfig config = null)
        {
            BH.oM.XML.Bluebeam.BluebeamObject report = null;

            TextReader reader = new StreamReader(_fileSettings.GetFullFileName());
            XmlSerializer szer = new XmlSerializer(typeof(BH.oM.XML.Bluebeam.MarkupSummary));
            report = (BH.oM.XML.Bluebeam.MarkupSummary)szer.Deserialize(reader);
            reader.Close();

            return new List<IBHoMObject> { report };
        }
    }
}


