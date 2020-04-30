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

using BHG = BH.oM.Geometry;
using KML = BH.oM.External.XML.KMLSchema;
using BH.oM.External.XML;
using System.Collections.Generic;

namespace BH.Adapter.XML
{
    public static partial class Convert
    {
        public static List<KML.Polygon> ToKML(this BHG.Mesh mesh, GeoReference geoReference)
        {
            List<KML.Polygon> polygons = new List<KML.Polygon>();
            foreach (BHG.Face face in mesh.Faces)
            {
                List<double> coords = new List<double>();
                List<BHG.Point> points = new List<BHG.Point> {
                    mesh.Vertices[face.A],
                    mesh.Vertices[face.B],
                    mesh.Vertices[face.C],
                    mesh.Vertices[face.D] 
                };
                foreach (BHG.Point p in points)
                {
                    KML.Point kmlpoint = p.ToKML(geoReference);
                    coords.AddRange(kmlpoint.Coordinates);
                }
                KML.Polygon polygon = new KML.Polygon();
                polygon.AltitudeMode = geoReference.AltitudeMode.ToKML();
                polygon.OuterBoundaryIs.LinearRing.Coordinates = coords.ToArray();
                polygons.Add(polygon);
            }
            return polygons;
        }
    }
}
