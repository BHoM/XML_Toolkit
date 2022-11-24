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

        [Description("Determines whether a collection of Environment Panels representing a single space contains any panels which are externally facing")]
        [Input("elementsAsSpace", "A collection of Environment Panels that represent closed spaced")]
        [Input("elementsAsSpaces", "A nested collection of Environment Panels which represent all spaces in the model")]
        [Output("isExternal", "True if the space has at least one externally facing Panel")]
        public static double Depth(this Markup markup)
        {
            try
            {
                return System.Convert.ToDouble(markup.XMLDepth);
            }
            catch (Exception e)
            {
                BH.Engine.Base.Compute.RecordError("Could not convert XMLDepth to numerical depth.");
                return -1;
            }
        }
    }
}

