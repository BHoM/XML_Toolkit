/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2019, the respective contributors. All rights reserved.
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

using BH.Engine;
using BH.oM.Base;
using System.Reflection;

using BH.oM.XML.Enums;
using BH.oM.Reflection.Attributes;
using System.ComponentModel;
using BH.oM.Data.Requests;

using BH.oM.XML.Settings;

namespace BH.Adapter.XML
{
    public partial class XMLAdapter : BHoMAdapter
    {
        [Description("Specify XML file and properties for data transfer")]
        [Input("fileSettings", "Input the file settings the XML Adapter should use, default null")]
        [Input("xmlSettings", "Input the additional XML Settings the adapter should use. Only used when pushing to an XML file. Default null")]
        [Output("adapter", "Adapter to XML")]
        public XMLAdapter(FileSettings fileSettings = null, XMLSettings xmlSettings = null)
        {
            if(fileSettings == null)
            {
                BH.Engine.Reflection.Compute.RecordError("Please set the File Settings correctly to enable the XML Adapter to work correctly");
                return;
            }

            _fileSettings = fileSettings;
            _xmlSettings = xmlSettings;

            AdapterId = "XML_Adapter";
            Config.MergeWithComparer = false;   //Set to true after comparers have been implemented
            Config.ProcessInMemory = false;
            Config.SeparateProperties = false;  //Set to true after Dependency types have been implemented
            Config.UseAdapterId = false;        //Set to true when NextId method and id tagging has been implemented
        }

        public override List<IObject> Push(IEnumerable<IObject> objects, String tag = "", Dictionary<String, object> config = null)
        {
            if(_xmlSettings == null)
            {
                BH.Engine.Reflection.Compute.RecordError("Please set some XML Settings on the XML Adapter before pushing to an XML File");
                return new List<IObject>();
            }

            bool success = true;

            MethodInfo methodInfos = typeof(Enumerable).GetMethod("Cast");
            foreach(var typeGroup in objects.GroupBy(x => x.GetType()))
            {
                MethodInfo mInfo = methodInfos.MakeGenericMethod(new[] { typeGroup.Key });
                var list = mInfo.Invoke(typeGroup, new object[] { typeGroup });
                success &= Create(list as dynamic, false);
            }

            return success ? objects.ToList() : new List<IObject>();
        }

        public override IEnumerable<object> Pull(IRequest request, Dictionary<string, object> config = null)
        {
            if (!System.IO.File.Exists(System.IO.Path.Combine(_fileSettings.Directory, _fileSettings.FileName + ".xml")))
            {
                BH.Engine.Reflection.Compute.RecordError("File does not exist to pull from");
                return new List<IBHoMObject>();
            }

            if (request != null)
            {
                FilterRequest filterRequest = request as FilterRequest;

                return Read(filterRequest.Type);
            }
            else
                return Read(null);
        }

        private FileSettings _fileSettings { get; set; } = null;
        private XMLSettings _xmlSettings { get; set; } = null;
    }
}
