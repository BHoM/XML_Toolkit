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

using BH.oM.Data.Requests;
using BH.oM.Adapter;
using BH.oM.Base;
using System.Reflection;

namespace BH.Adapter.XML
{
    public partial class XMLAdapter : BHoMAdapter
    {
        public override List<object> Push(IEnumerable<object> objects, String tag = "", PushType pushType = PushType.AdapterDefault, ActionConfig actionConfig = null)
        {
            // If unset, set the pushType to AdapterSettings' value (base AdapterSettings default is FullCRUD).
            if (pushType == PushType.AdapterDefault)
                pushType = m_AdapterSettings.DefaultPushType;

            if (_xmlSettings == null)
            {
                BH.Engine.Reflection.Compute.RecordError("Please set some XML Settings on the XML Adapter before pushing to an XML File");
                return new List<object>();
            }

            IEnumerable<IBHoMObject> objectsToPush = ProcessObjectsForPush(objects, actionConfig); // Note: default Push only supports IBHoMObjects.

            bool success = true;

            MethodInfo methodInfos = typeof(Enumerable).GetMethod("Cast");
            foreach (var typeGroup in objectsToPush.GroupBy(x => x.GetType()))
            {
                MethodInfo mInfo = methodInfos.MakeGenericMethod(new[] { typeGroup.Key });
                var list = mInfo.Invoke(typeGroup, new object[] { typeGroup });
                success &= ICreate(list as dynamic);
            }

            return success ? objects.ToList() : new List<object>();
        }
    }
}
