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

using BH.Engine;
using BH.oM.Base;
using System.Reflection;

using BH.oM.XML.Enums;

using BH.oM.DataManipulation.Queries;

namespace BH.Adapter.XML
{
    public partial class XMLAdapter : BHoMAdapter
    {
        public XMLAdapter(String projectName = "BHoM_gbXML_Export", String SaveDirectoryPath = null, ExportType exportType = ExportType.gbXMLTAS, ExportDetail exportDetail = ExportDetail.Full)
        {
            FilePath = SaveDirectoryPath ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            ProjectName = projectName;
            ExportType = exportType;
            ExportDetail = exportDetail;

            if(System.IO.Path.HasExtension(ProjectName) && System.IO.Path.GetExtension(ProjectName) == ".xml")
            {
                BH.Engine.Reflection.Compute.RecordError("Project Name cannot contain a file extension");
                return;
            }

            AdapterId = "XML_Adapter";
            Config.MergeWithComparer = false;   //Set to true after comparers have been implemented
            Config.ProcessInMemory = false;
            Config.SeparateProperties = false;  //Set to true after Dependency types have been implemented
            Config.UseAdapterId = false;        //Set to true when NextId method and id tagging has been implemented
        }

        public override List<IObject> Push(IEnumerable<IObject> objects, String tag = "", Dictionary<String, object> config = null)
        {
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

        public override IEnumerable<object> Pull(IQuery query, Dictionary<string, object> config = null)
        {
            if (!System.IO.File.Exists(System.IO.Path.Combine(FilePath, ProjectName + ".xml")))
                return new List<IBHoMObject>();

            if (query != null)
            {
                FilterQuery filterQuery = query as FilterQuery;

                return Read(filterQuery.Type);
            }
            else
                return Read(null);
        }

        private String FilePath { get; set; }
        private String ProjectName { get; set; }
        private ExportType ExportType { get; set; }
        private ExportDetail ExportDetail { get; set; }
    }
}
