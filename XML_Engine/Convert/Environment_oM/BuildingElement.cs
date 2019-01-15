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

using BHE = BH.oM.Environment.Elements;
using BHX = BH.oM.XML;
using BHG = BH.oM.Geometry;

using BH.Engine.Geometry;
using BH.Engine.Environment;

namespace BH.Engine.XML
{
    public static partial class Convert
    {
        public static BHX.Surface ToGBXML(this BHE.BuildingElement element)
        {
            BHX.Surface surface = new BHX.Surface();
            surface.CADObjectID = element.CADObjectID();
            surface.ConstructionIDRef = element.ConstructionID();

            BHX.RectangularGeometry geom = element.ToGBXMLGeometry();
            BHX.PlanarGeometry planarGeom = new BHX.PlanarGeometry();
            planarGeom.ID = "PlanarGeometry-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);

            BHG.Polyline pLine = new BHG.Polyline() { ControlPoints = element.PanelCurve.IControlPoints() };
            planarGeom.PolyLoop = pLine.ToGBXML();

            surface.PlanarGeometry = planarGeom;
            surface.RectangularGeometry = geom;

            surface.Opening = new BHX.Opening[element.Openings.Count];
            for (int x = 0; x < element.Openings.Count; x++)
                surface.Opening[x] = element.Openings[x].ToGBXML();

            return surface;
        }

        public static BHX.Surface ToGBXML(this BHE.BuildingElement element, List<BHE.Space> adjacentSpaces, List<BHE.BuildingElement> space)
        {
            BHX.Surface surface = element.ToGBXML();

            surface.SurfaceType = element.ToGBXMLType(adjacentSpaces);
            surface.ExposedToSun = BH.Engine.Environment.Query.ExposedToSun(surface.SurfaceType).ToString().ToLower();

            BHG.Polyline pLine = new BHG.Polyline() { ControlPoints = element.PanelCurve.IControlPoints() };
            if (!pLine.NormalAwayFromSpace(space))
            {
                pLine = pLine.Flip();
                surface.PlanarGeometry.PolyLoop = pLine.ToGBXML();
                surface.RectangularGeometry.Tilt = Math.Round(pLine.Tilt(), 3);
                surface.RectangularGeometry.Azimuth = Math.Round(pLine.Azimuth(BHG.Vector.YAxis), 3);
            }

            List<BHX.AdjacentSpaceId> adjIDs = new List<BHX.AdjacentSpaceId>();
            foreach (BHE.Space sp in adjacentSpaces)
                adjIDs.Add(sp.AdjacentSpaceID());
            surface.AdjacentSpaceID = adjIDs.ToArray();

            return surface;
        }

        public static BHX.RectangularGeometry ToGBXMLGeometry(this BHE.BuildingElement element)
        {
            BHX.RectangularGeometry geom = new BHX.RectangularGeometry();

            BHG.Polyline pLine = new oM.Geometry.Polyline() { ControlPoints = element.PanelCurve.IControlPoints() };

            geom.Tilt = Math.Round(element.Tilt(), 3);
            geom.Azimuth = Math.Round(element.Azimuth(BHG.Vector.YAxis), 3);
            geom.Height = Math.Round(element.Height(), 3);
            geom.Width = Math.Round(element.Width(), 3);
            geom.CartesianPoint = pLine.ControlPoints.First().ToGBXML();
            geom.ID = "geom-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);

            return geom;
        }

        public static BHE.BuildingElement ToBHoM(this BHX.Surface surface)
        {
            BHE.BuildingElement buildingElement = new BHE.BuildingElement();

            surface.Opening = surface.Opening ?? new List<BHX.Opening>().ToArray();

            buildingElement.PanelCurve = surface.PlanarGeometry.PolyLoop.ToBHoM();
            foreach (BHX.Opening opening in surface.Opening)
                buildingElement.Openings.Add(opening.ToBHoM());

            return buildingElement;
        }
    }
}
