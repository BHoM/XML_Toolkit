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
using BH.oM.External.XML.Enums;

using BH.oM.Adapter;

namespace BH.Adapter.XML
{
    public partial class XMLAdapter : BHoMAdapter
    {
        protected override bool ICreate<T>(IEnumerable<T> objects, ActionConfig actionConfig = null)
        {
            if(actionConfig == null)
            {
                BH.Engine.Reflection.Compute.RecordError("Please provide configuration settings to push to an XML file");
                return false;
            }

            XMLConfig config = actionConfig as XMLConfig;
            if(config == null)
            {
                BH.Engine.Reflection.Compute.RecordError("Please provide valid a XMLConfig object for pushing to an XML file");
                return false;
            }

            switch(config.Schema)
            {
                case Schema.GBXML:
                    return CreateGBXML(objects, config);
                default:
                    BH.Engine.Reflection.Compute.RecordError("The XML Schema you have supplied is not currently supported by the XML Toolkit");
                    return false;
            }

            /*string fileName = _fileSettings.GetFullFileName();

            GBXML gbx = new GBXML();

            if(!_xmlSettings.NewFile)
                gbx = XMLReader.Load(_fileSettings.GetFullFileName()); //Load up the previous file to append to

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
                XML.GBXMLSerializer.Serialize(objects, gbx, _fileSettings, _xmlSettings);
                if(_xmlSettings.ExportDetail != ExportDetail.IndividualSpaces)
                    XMLWriter.Save(fileName, gbx);
            }

            return true;*/
        }
    }
}