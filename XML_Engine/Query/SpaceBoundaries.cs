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
using BH.oM.XML;
using BH.oM.Base;
using BHE = BH.oM.Environment.Elements;
using BHP = BH.oM.Environment.Fragments;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.Engine.Environment;

using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.XML
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Gets the GBXML SpaceBoundary representation of a collection of Environment panels which comprise a closed volumetric space")]
        [Input("spaceBoundaries", "A collection of Environment Panels which comprise a closed volumetric space")]
        [Input("uniqueBEs", "A collection of unique Environment Panels used in the model as a whole")]
        [Input("planarTolerance", "The tolerance to define planarity - default to BH.oM.Geometry.Tolerance.Distance")]
        [Output("spaceBoundaries", "GBXML representation of space boundaries")]
        public static SpaceBoundary[] SpaceBoundaries(this List<BHE.Panel> spaceBoundaries, List<BHE.Panel> uniqueBEs, double planarTolerance = BH.oM.Geometry.Tolerance.Distance)
        {
            List<Polyloop> pLoops = new List<Polyloop>();
            List<BHG.Polyline> panels = spaceBoundaries.Select(x => x.Polyline()).ToList();

            foreach(BHG.Polyline pLine in panels)
            {
                if (BH.Engine.Environment.Query.NormalAwayFromSpace(pLine, spaceBoundaries, planarTolerance))
                    pLoops.Add(BH.Engine.XML.Convert.ToGBXML(pLine));
                else
                    pLoops.Add(BH.Engine.XML.Convert.ToGBXML(pLine.Flip()));
            }

            SpaceBoundary[] boundaries = new SpaceBoundary[pLoops.Count];

            for(int x = 0; x < pLoops.Count; x++)
            {
                PlanarGeometry planarGeom = new PlanarGeometry();
                planarGeom.PolyLoop = pLoops[x];
                planarGeom.ID = "pGeom" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
                boundaries[x] = new SpaceBoundary { PlanarGeometry = planarGeom };

                //Get the ID from the referenced element
                boundaries[x].SurfaceIDRef = "Panel-" + uniqueBEs.FindIndex(i => i.BHoM_Guid == spaceBoundaries[x].BHoM_Guid).ToString();
            }

            return boundaries;
        }
    }
}




