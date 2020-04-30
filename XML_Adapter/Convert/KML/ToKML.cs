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

using System.Collections.Generic;
using KML = BH.oM.External.XML.KMLSchema;
using BH.oM.Base;
using BH.oM.External.XML.Settings;
using BH.oM.External.XML;
using BH.oM.External.XML.KMLSchema;
using BHG = BH.oM.Geometry;
using System.Linq;

namespace BH.Adapter.XML
{
    public static partial class Convert
    {
        public static KML.KML ToKML(this KMLDocumentBuilder docBuilder, KMLSettings settings)
        {

            KML.KML kml = new KML.KML();
            kml.Document.Name = docBuilder.Name;
            List<Style> docStyles = new List<Style>();
            //set the default style
            docStyles.Add(new Style());
            List<Folder> folders = new List<Folder>();
            //each KMLGeometry is placed in a Folder
            foreach(KMLGeometry kMLGeometry in docBuilder.KMLGeometries)
            {
                //Add kmlGeo style
                
                List<BHG.Point> points = new List<BHG.Point>();
                List<BHG.Mesh> meshes = new List<BHG.Mesh>();

                foreach(BHG.IGeometry igeo in kMLGeometry.Geometries)
                {
                    if (igeo is BHG.Point)
                        points.Add(igeo as BHG.Point);

                    if (igeo is BHG.Mesh)
                        meshes.Add(igeo as BHG.Mesh);
                }

                Folder folder = new Folder();
                List<Placemark> placemarks = new List<Placemark>();
                folder.Name = kMLGeometry.Name;
                
                List<Polygon> polygons = new List<Polygon>();
                Placemark placemark = new Placemark();
                MultiGeometry multiGeometry = new MultiGeometry();
                foreach (BHG.Mesh m in meshes)
                {
                    placemark = new Placemark();
                    
                    multiGeometry = new MultiGeometry();
                    polygons = new List<Polygon>();
                    polygons = m.ToKML(kMLGeometry.GeoReference);
                    multiGeometry.Polygons = polygons.ToArray();
                    placemarks.Add(placemark);
                }


                folders.Add(folder);
            }
            kml.Document.Folders = folders.ToArray();
            kml.Document.Styles = docStyles.ToArray();
            return kml;
        }
    }
}
