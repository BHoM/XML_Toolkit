/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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

using BH.oM.External.XML;
using GBXML = BH.oM.External.XML.GBXML;
using BH.oM.External.XML.Settings;
using BH.oM.Base;
using BH.oM.Environment.Elements;
using BH.oM.Geometry.SettingOut;

using BH.Engine.Environment;

namespace BH.Adapter.XML
{
    public partial class XMLAdapter : BHoMAdapter
    {
        private bool CreateGBXML<T>(IEnumerable<T> objects, XMLConfig config)
        {
            bool success = true;

            if(config.Settings == null)
            {
                BH.Engine.Reflection.Compute.RecordError("Please provide a suitable set of GBXMLSettings with the XMLConfig to determine how the GBXML file should be created");
                return false;
            }

            GBXMLSettings settings = config.Settings as GBXMLSettings;
            if(settings == null)
            {
                BH.Engine.Reflection.Compute.RecordError("Please provide a suitable set of GBXMLSettings with the XMLConfig to determine how the GBXML file should be created");
                return false;
            }

            List<IBHoMObject> bhomObjects = objects.Where(x => (x as IBHoMObject) != null).Select(x => x as IBHoMObject).ToList();

            List<Panel> panels = bhomObjects.Panels();
            List<Level> levels = bhomObjects.Levels();
            List<Building> buildings = bhomObjects.Buildings();
            List<List<Panel>> panelsAsSpaces = panels.ToSpaces();

            //Run some checks to make sure the user is aware if we are missing any suitable data elements
            if(panelsAsSpaces.Count == 0)
                BH.Engine.Reflection.Compute.RecordWarning("There are no spaces represented by panels included within the objects being pushed");
            if (panels.Count == 0)
                BH.Engine.Reflection.Compute.RecordWarning("There are no panels included within the objects being pushed");
            if (levels.Count == 0)
                BH.Engine.Reflection.Compute.RecordWarning("There are no levels included within the objects being pushed");
            if (buildings.Count == 0)
                BH.Engine.Reflection.Compute.RecordWarning("There are no buildings included within the objects being pushed");

            List<GBXML.Space> xmlSpaces = panelsAsSpaces.Select(x => x.ToGBXML(x.Level(levels), settings)).ToList();

            return success;
        }


    }
}
