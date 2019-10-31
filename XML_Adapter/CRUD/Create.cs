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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BH.oM.Base;
using BHE = BH.oM.Environment;
using BH.oM.XML;
using BH.oM.XML.Enums;

using BH.Engine.XML;

namespace BH.Adapter.XML
{
    public partial class XMLAdapter : BHoMAdapter
    {
        protected override bool Create<T>(IEnumerable<T> objects)
        {
            string fileName = _fileSettings.FullFileName();

            GBXML gbx = new GBXML();

            if(!_xmlSettings.NewFile)
                gbx = XMLReader.Load(_fileSettings.FullFileName()); //Load up the previous file to append to

            if(_xmlSettings.UnitType == UnitType.Imperial)
            {
                gbx.TemperatureUnit = "F";
                gbx.LengthUnit = "Feet";
                gbx.AreaUnit = "SquareFeet";
                gbx.VolumeUnit = "CubicFeet";
                gbx.UseSIUnitsForResults = "false";
            }

            if (typeof(IBHoMObject).IsAssignableFrom(typeof(T)))
            {
                XML.GBXMLSerializer.Serialize(objects, gbx, fileName, _xmlSettings);
                XMLWriter.Save(fileName, gbx);
            }

            return true;
        }
    }
}