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
using BHE = BH.oM.Environment.Elements;
using BHX = BH.oM.XML;
using BHG = BH.oM.Geometry;

using BH.Engine.Geometry;
using BH.Engine.Environment;
using BH.oM.XML.Settings;

using BHP = BH.oM.Environment.Fragments;

namespace BH.Engine.XML
{
    public static partial class Convert
    {
        public static BHX.Surface ToGBXML(this BHE.Panel element)
        {
            BHP.OriginContextFragment contextProperties = element.FindFragment<BHP.OriginContextFragment>(typeof(BHP.OriginContextFragment));

            BHX.Surface surface = new BHX.Surface();
            surface.CADObjectID = element.CADObjectID();
            surface.ConstructionIDRef = (contextProperties == null ? element.ConstructionID() : contextProperties.TypeName.CleanName());

            BHX.RectangularGeometry geom = element.ToGBXMLGeometry();
            BHX.PlanarGeometry planarGeom = new BHX.PlanarGeometry();
            planarGeom.ID = "PlanarGeometry-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);

            BHG.Polyline pLine = element.Polyline();
            planarGeom.PolyLoop = pLine.ToGBXML();

            surface.PlanarGeometry = planarGeom;
            surface.RectangularGeometry = geom;

            surface.Opening = new BHX.Opening[element.Openings.Count];
            for (int x = 0; x < element.Openings.Count; x++)
            {
                if(element.Openings[x].Polyline().IControlPoints().Count != 0)
                    surface.Opening[x] = element.Openings[x].ToGBXML();
            }

            return surface;
        }

        public static BHX.Surface ToGBXML(this BHE.Panel element, List<List<BHE.Panel>> adjacentSpaces, List<BHE.Panel> space, XMLSettings settings)
        {
            BHX.Surface surface = element.ToGBXML();

            surface.SurfaceType = element.ToGBXMLType(adjacentSpaces);
            surface.ExposedToSun = Query.ExposedToSun(surface.SurfaceType).ToString().ToLower();

            BHG.Polyline pLine = element.Polyline();
            if (!pLine.NormalAwayFromSpace(space, settings.PlanarTolerance))
            {
                pLine = pLine.Flip();
                surface.PlanarGeometry.PolyLoop = pLine.ToGBXML();
                surface.RectangularGeometry.Tilt = Math.Round(pLine.Tilt(), 3);
                surface.RectangularGeometry.Azimuth = Math.Round(pLine.Azimuth(BHG.Vector.YAxis), 3);
            }

            List<BHX.AdjacentSpaceId> adjIDs = new List<BHX.AdjacentSpaceId>();
            foreach (List<BHE.Panel> sp in adjacentSpaces)
                adjIDs.Add(sp.AdjacentSpaceID());
            surface.AdjacentSpaceID = adjIDs.ToArray();

            return surface;
        }

        public static BHX.RectangularGeometry ToGBXMLGeometry(this BHE.Panel element)
        {
            BHX.RectangularGeometry geom = new BHX.RectangularGeometry();

            BHG.Polyline pLine = element.Polyline();

            geom.Tilt = Math.Round(element.Tilt(), 3);
            geom.Azimuth = Math.Round(element.Azimuth(BHG.Vector.YAxis), 3);
            geom.Height = Math.Round(element.Height(), 3);
            geom.Width = Math.Round(element.Width(), 3);
            geom.CartesianPoint = pLine.ControlPoints.First().ToGBXML();
            geom.ID = "geom-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);

            if(geom.Height == 0)
                geom.Height = Math.Round(element.Area() / geom.Width, 3);
            if (geom.Width == 0)
                geom.Width = Math.Round(element.Area() / geom.Height, 3);
            if (geom.Tilt == -1)
                BH.Engine.Reflection.Compute.RecordWarning("Hej");

            return geom;
        }

        public static BHE.Panel ToBHoM(this BHX.Surface surface)
        {
            BHE.Panel panel = new BHE.Panel();

            surface.Opening = surface.Opening ?? new List<BHX.Opening>().ToArray();

            panel.ExternalEdges = surface.PlanarGeometry.PolyLoop.ToBHoM().ToEdges();
            foreach (BHX.Opening opening in surface.Opening)
                panel.Openings.Add(opening.ToBHoM());

            string[] cadSplit = surface.CADObjectID.Split('[');
            if(cadSplit.Length > 0)
                panel.Name = cadSplit[0].Trim();
            if (cadSplit.Length > 1)
            {
                BHP.OriginContextFragment envContext = new BHP.OriginContextFragment();
                envContext.ElementID = cadSplit[1].Split(']')[0].Trim();
                envContext.TypeName = panel.Name;

                if (panel.Fragments == null) panel.Fragments = new FragmentSet();
                panel.Fragments.Add(envContext);

            }

            panel.Type = surface.SurfaceType.ToBHoMPanelType();
            panel.ConnectedSpaces = new List<string>();
            if (surface.AdjacentSpaceID != null)
            {
                foreach (BHX.AdjacentSpaceId adjacent in surface.AdjacentSpaceID)
                    panel.ConnectedSpaces.Add(adjacent.SpaceIDRef);
            }

            return panel;
        }
    }
}
