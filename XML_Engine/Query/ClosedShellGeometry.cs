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
using BH.oM.XML;
using BH.oM.Base;
using BHE = BH.oM.Environment.Elements;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.XML
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("BH.Engine.XML.Query.ClosedShellGeometry => Gets the XML Polyloop closed shell geometry for a collection of panels representing a space")]
        [Input("panelsAsSpace", "A collection of Environment Panels representing a single space")]
        [Input("planarTolerance", "Set tolerance for planar surfaces")]
        [Output("A collection of XML Geometry Polyloops which represent the shell of the space")]
        public static List<Polyloop> ClosedShellGeometry(this List<BHE.Panel> panelsAsSpace, double planarTolerance = BH.oM.Geometry.Tolerance.Distance)
        {
            List<Polyloop> shell = new List<Polyloop>();

            List<BHG.Polyline> polylines = BH.Engine.Environment.Query.ClosedShellGeometry(panelsAsSpace);

            //Ensure that all of the surface coordinates are listed in a clockwise order
            foreach(BHG.Polyline pLine in polylines)
            {
                if (BH.Engine.Environment.Query.NormalAwayFromSpace(pLine, panelsAsSpace, planarTolerance))
                    shell.Add(BH.Engine.XML.Convert.ToGBXML(pLine));
                else
                    shell.Add(BH.Engine.XML.Convert.ToGBXML(pLine.Flip()));
            }

            return shell;
        }
    }
}





