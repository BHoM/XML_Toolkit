/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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
using BH.oM.Environment.Elements;
using BH.Engine.Environment;

using System.ComponentModel;
using BH.oM.Base.Attributes;
using BH.oM.XML.Bluebeam;
using System.Text;
using System.Threading.Tasks;

using BH.oM.Adapters.XML;
using BH.oM.Adapters.XML.Enums;

namespace BH.Engine.Adapters.XML
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Returns the 'depth' property of a markup object as a double. If the value cannot be converted to a double (if the value is null or blank for example) then an error will be provided and -1 will be returned.")]
        [Input("markup", "A markup object which contains the 'depth' property.")]
        [Output("depth", "The depth value as a double, or -1 if the value could not be converted to a double.")]
        public static double Depth(this Markup markup)
        {
            try
            {
                return System.Convert.ToDouble(markup.XMLDepth);
            }
            catch (Exception e)
            {
                BH.Engine.Base.Compute.RecordError($"Could not convert XMLDepth value {markup.XMLDepth} to numerical depth.");
                return -1;
            }
        }
    }
}

