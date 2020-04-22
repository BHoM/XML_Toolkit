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

using BH.oM.XML;
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

            List<IBHoMObject> bhomObjects = objects.Where(x => (x as IBHoMObject) != null).Select(x => x as IBHoMObject).ToList();

            List<Panel> panels = bhomObjects.Panels();
            List<Space> spaces = bhomObjects.Spaces();
            List<Level> levels = bhomObjects.Levels();
            List<Building> buildings = bhomObjects.Buildings();

            return success;
        }

        private 
    }
}
