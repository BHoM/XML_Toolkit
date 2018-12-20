/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
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
using BH.oM.Environment.Elements;
using BH.oM.Environment.Properties;
using BH.oM.Environment.Interface;
using BHG = BH.oM.Geometry;
using BH.Engine;
using BHE = BH.oM.Environment;

namespace BH.Adapter.XML
{
    public partial class XMLAdapter : BHoMAdapter
    {
        protected override IEnumerable<IBHoMObject> Read(Type type, IList indices = null)
        {
            if (type == typeof(Space))
                return ReadSpaces();
            if (type == typeof(Panel))
                return ReadPanels();
            return null;
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<Space> ReadSpaces(List<String> ids = null)
        {
            BH.oM.XML.GBXML gbx = XMLReader.Load(FilePath, FileName);
            IEnumerable<IBHoMObject> bHoMObject = GBXMLDeserializer.Deserialize(gbx);
            return bHoMObject.Where(x => x is BHE.Elements.Space).Cast<Space>().ToList();
        }

        /***************************************************/

        private List<Panel> ReadPanels(List<string> ids = null)
        {
            BH.oM.XML.GBXML gbx = XMLReader.Load(FilePath, FileName);
            IEnumerable<IBHoMObject> bHoMObject = GBXMLDeserializer.Deserialize(gbx);
            return bHoMObject.Where(x => x is BHE.Elements.Panel).Cast<Panel>().ToList();
        }
    }
}