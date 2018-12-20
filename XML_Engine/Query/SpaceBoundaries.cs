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
using BHP = BH.oM.Environment.Properties;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.Engine.Environment;

namespace BH.Engine.XML
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static SpaceBoundary[] SpaceBoundaries(this BHE.Space bHoMSpace, List<BHE.BuildingElement> be)
        {
            /*List<BH.oM.XML.Polyloop> ploops = new List<Polyloop>();
            IEnumerable<BHG.PolyCurve> bePanel = bHoMSpace.BuildingElements.Select(x => x.BuildingElementGeometry.ICurve() as BHG.PolyCurve);

            foreach (BHG.PolyCurve pCrv in bePanel)
            {
                // Ensure that all of the surface coordinates are listed in a counterclockwise order
                //* This is a requirement of GBXML Polyloop definitions
                BHG.Polyline pline = new BHG.Polyline() { ControlPoints = pCrv.ControlPoints() };

                if (!BH.Engine.Environment.Query.NormalAwayFromSpace(pline, bHoMSpace))
                    ploops.Add(BH.Engine.XML.Convert.ToGBXML(pline.Flip()));
                else
                    ploops.Add(BH.Engine.XML.Convert.ToGBXML(pline));
            }

            SpaceBoundary[] spaceBound = new SpaceBoundary[ploops.Count()];

            for (int i = 0; i < ploops.Count(); i++)
            {
                PlanarGeometry planarGeom = new PlanarGeometry();
                planarGeom.PolyLoop = ploops[i];
                SpaceBoundary bound = new SpaceBoundary { PlanarGeometry = planarGeom };
                spaceBound[i] = bound;

                //Get the id from the referenced panel
                string refPanel = "Panel-" + be.FindIndex(x => x.BHoM_Guid.ToString() == bHoMSpace.BuildingElements[i].BHoM_Guid.ToString()).ToString();
                spaceBound[i].SurfaceIDRef = refPanel;
            }

            return spaceBound;*/
            return null;

            /***************************************************/
        }

        public static SpaceBoundary[] SpaceBoundaries(this List<BHE.BuildingElement> spaceBoundaries, List<BHE.BuildingElement> uniqueBEs)
        {
            List<Polyloop> pLoops = new List<Polyloop>();
            List<BHG.Polyline> panels = spaceBoundaries.Select(x => x.PanelCurve.ICollapseToPolyline(BHG.Tolerance.Angle)).ToList();

            foreach(BHG.Polyline pLine in panels)
            {
                if (BH.Engine.Environment.Query.NormalAwayFromSpace(pLine, spaceBoundaries))
                    pLoops.Add(BH.Engine.XML.Convert.ToGBXML(pLine));
                else
                    pLoops.Add(BH.Engine.XML.Convert.ToGBXML(pLine.Flip()));
            }

            SpaceBoundary[] boundaries = new SpaceBoundary[pLoops.Count];

            for(int x = 0; x < pLoops.Count; x++)
            {
                PlanarGeometry planarGeom = new PlanarGeometry();
                planarGeom.PolyLoop = pLoops[x];
                planarGeom.ID = "pGeom" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);
                boundaries[x] = new SpaceBoundary { PlanarGeometry = planarGeom };

                //Get the ID from the referenced element
                boundaries[x].SurfaceIDRef = "Panel-" + uniqueBEs.FindIndex(i => i.BHoM_Guid == spaceBoundaries[x].BHoM_Guid).ToString();
            }

            return boundaries;
        }
    }
}




