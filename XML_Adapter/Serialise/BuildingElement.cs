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

using BH.oM.Environment.Elements;
using System;
using System.Collections.Generic;

using System.Linq;
using BH.Engine.Environment;

using BH.oM.XML;
using BH.Engine.XML;

using BH.oM.Geometry;
using BH.Engine.Geometry;

using BH.oM.Architecture.Elements;

using BH.oM.XML.Enums;

namespace BH.Adapter.XML
{
    public partial class GBXMLSerializer
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static void SerializeCollection(IEnumerable<IEnumerable<BuildingElement>> inputElements, List<Level> levels, List<BuildingElement> openings, BH.oM.XML.GBXML gbx, ExportType exportType)
        {
            List<List<BuildingElement>> elementsAsSpaces = new List<List<BuildingElement>>();

            foreach (IEnumerable<BuildingElement> input in inputElements)
                elementsAsSpaces.Add(input.ToList());

            List<BuildingElement> uniqueBuildingElements = elementsAsSpaces.UniqueBuildingElements();

            List<BuildingElement> allElements = new List<BuildingElement>(uniqueBuildingElements);
            allElements.AddRange(openings);

            List<BH.oM.Environment.Elements.Space> spaces = elementsAsSpaces.Spaces();

            List<BuildingElement> usedBEs = new List<BuildingElement>();

            List<BH.oM.XML.Construction> usedConstructions = new List<BH.oM.XML.Construction>();
            List<BH.oM.XML.Material> usedMaterials = new List<Material>();
            List<BH.oM.XML.Layer> usedLayers = new List<Layer>();

            foreach (List<BuildingElement> space in elementsAsSpaces)
            {
                //For each collection of BuildingElements that define a space, convert the panels to XML surfaces and add to the GBXML
                List<Surface> spaceSurfaces = new List<Surface>();

                for (int x = 0; x < space.Count; x++)
                {
                    if (usedBEs.Where(i => i.BHoM_Guid == space[x].BHoM_Guid).FirstOrDefault() != null) continue;

                    List<BH.oM.Environment.Elements.Space> adjacentSpaces = BH.Engine.Environment.Query.AdjacentSpaces(space[x], elementsAsSpaces, spaces);

                    Surface srf = new Surface();
                    srf.Name = "Panel-" + gbx.Campus.Surface.Count.ToString();
                    srf.SurfaceType = BH.Engine.XML.Convert.ToGBXMLType(space[x], adjacentSpaces, exportType);

                    if (space[x].BuildingElementProperties != null)
                        srf.CADObjectID = BH.Engine.XML.Query.CadObjectId(space[x], exportType);

                    srf.ID = "Panel-" + gbx.Campus.Surface.Count.ToString();
                    srf.ExposedToSun = BH.Engine.Environment.Query.ExposedToSun(srf.SurfaceType).ToString().ToLower();

                    if (exportType == ExportType.gbXMLIES)
                        srf.ConstructionIDRef = BH.Engine.XML.Query.IdRef(space[x]);

                    RectangularGeometry srfGeom = BH.Engine.XML.Convert.ToGBXML(space[x]);
                    PlanarGeometry plGeom = new PlanarGeometry();
                    plGeom.ID = "PlanarGeometry-" + x.ToString() + "-" + space[x].BHoM_Guid.ToString().Replace("-", "").Substring(0, 5);

                    // Ensure that all of the surface coordinates are listed in a counterclockwise order
                    // This is a requirement of GBXML Polyloop definitions 

                    Polyline pline = new Polyline() { ControlPoints = space[x].PanelCurve.IControlPoints() }; //TODO: Change to ToPolyline method
                    Polyline srfBound = new Polyline();

                    if (!BH.Engine.Environment.Query.NormalAwayFromSpace(pline, space))
                    {
                        plGeom.PolyLoop = BH.Engine.XML.Convert.ToGBXML(pline.Flip());
                        srfBound = pline.Flip();

                        srfGeom.Tilt = Math.Round(BH.Engine.Environment.Query.Tilt(srfBound), 3);
                        srfGeom.Azimuth = Math.Round(BH.Engine.Environment.Query.Azimuth(srfBound, Vector.YAxis), 3);

                    }
                    else
                    {
                        plGeom.PolyLoop = BH.Engine.XML.Convert.ToGBXML(pline);
                        srfBound = pline;
                    }

                    srf.PlanarGeometry = plGeom;
                    srf.RectangularGeometry = srfGeom;

                    //Adjacent Spaces
                    List<AdjacentSpaceId> adjIDs = new List<AdjacentSpaceId>();
                    foreach (BH.oM.Environment.Elements.Space sp in adjacentSpaces)
                        adjIDs.Add(sp.GetAdjacentSpaceID());
                    srf.AdjacentSpaceID = adjIDs.ToArray();

                    //Openings
                    if (space[x].Openings.Count > 0)
                        srf.Opening = Serialize(space[x].Openings, space, allElements, elementsAsSpaces, spaces, gbx, exportType).ToArray();

                    gbx.Campus.Surface.Add(srf);

                    usedBEs.Add(space[x]);

                    if (exportType == ExportType.gbXMLIES)
                    {
                        BH.oM.XML.Construction conc = space[x].ToGBXMLConstruction();
                        BH.oM.XML.Construction test = usedConstructions.Where(y => y.ID == conc.ID).FirstOrDefault();
                        if (test == null)
                        {
                            List<BH.oM.XML.Material> materials = new List<Material>();
                            foreach (BH.oM.Environment.Materials.Material m in space[x].BuildingElementProperties.Construction.Materials)
                                materials.Add(m.ToGBXML());

                            BH.oM.XML.Layer layer = materials.ToGBXML();
                            conc.LayerID.LayerIDRef = layer.ID;

                            usedConstructions.Add(conc);
                            usedLayers.Add(layer);
                            usedMaterials.AddRange(materials);
                        }
                    }
                }

                Dictionary<String, object> spaceData = (space.Where(x => x.CustomData.ContainsKey("Space_Custom_Data")).FirstOrDefault() != null ? space.Where(x => x.CustomData.ContainsKey("Space_Custom_Data")).FirstOrDefault().CustomData["Space_Custom_Data"] as Dictionary<String, object> : new Dictionary<string, object>());
                //Create the space in
                BH.oM.Environment.Elements.Space s = space.Space(gbx.Campus.Building[0].Space.Count, gbx.Campus.Building[0].Space.Count.ToString());
                BH.oM.XML.Space xmlSpace = new oM.XML.Space();
                xmlSpace.Name = (spaceData.ContainsKey("SAM_SpaceName") && spaceData["SAM_SpaceName"] != null ? spaceData["SAM_SpaceName"].ToString() : s.Name); //CUSTOMDATA SAM_SpaceName
                xmlSpace.ID = "Space-" + s.Number + "-" + s.Name;
                xmlSpace.CADObjectID = BH.Engine.XML.Query.CadObjectId(space);
                xmlSpace.ShellGeometry.ClosedShell.PolyLoop = BH.Engine.XML.Query.ClosedShellGeometry(space).ToArray();
                xmlSpace.ShellGeometry.ID = "SpaceShellGeometry-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);
                xmlSpace.SpaceBoundary = BH.Engine.XML.Query.SpaceBoundaries(space, uniqueBuildingElements);
                xmlSpace.PlanarGeoemtry.ID = "SpacePlanarGeometry-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);
                if (BH.Engine.Environment.Query.FloorGeometry(space) != null)
                {
                    xmlSpace.PlanarGeoemtry.PolyLoop = BH.Engine.XML.Convert.ToGBXML(BH.Engine.Environment.Query.FloorGeometry(space));
                    xmlSpace.Area = BH.Engine.Environment.Query.FloorGeometry(space).Area();
                    xmlSpace.Volume = space.Volume();
                }
                Level spaceLevel = space.Level(levels);
                if (spaceLevel != null)
                    xmlSpace.BuildingStoreyIDRef = "Level-" + spaceLevel.Name.Replace(" ", "").ToLower();

                gbx.Campus.Building[0].Space.Add(xmlSpace);
            }

            gbx.Construction = usedConstructions.ToArray();
            gbx.Layer = usedLayers.ToArray();
            gbx.Material = usedMaterials.ToArray();
        }

        public static void SerializeCollection(IEnumerable<BuildingElement> inputElements, BH.oM.XML.GBXML gbx, ExportType exportType)
        {
            //For serializing shade elements
            List<BuildingElement> buildingElements = inputElements.ToList();

            foreach (BuildingElement be in buildingElements)
            {
                Surface xmlSrf = new Surface();
                xmlSrf.Name = "Shade-" + gbx.Campus.Surface.Count.ToString();
                xmlSrf.SurfaceType = "Shade";
                xmlSrf.ID = xmlSrf.Name;
                xmlSrf.ExposedToSun = BH.Engine.Environment.Query.ExposedToSun(xmlSrf.SurfaceType).ToString().ToLower();

                if (be.BuildingElementProperties != null)
                    xmlSrf.CADObjectID = BH.Engine.XML.Query.CadObjectId(be, exportType);

                if (exportType == ExportType.gbXMLIES)
                    xmlSrf.ConstructionIDRef = BH.Engine.XML.Query.IdRef(be); ; //Only for IES!

                RectangularGeometry xmlRectangularGeom = BH.Engine.XML.Convert.ToGBXML(be);
                PlanarGeometry plGeo = new PlanarGeometry();
                plGeo.ID = "PlanarGeometry-" + "shade-" + gbx.Campus.Surface.Count.ToString();
                Polyline pline = new Polyline() { ControlPoints = be.PanelCurve.IControlPoints() }; //TODO: Change to ToPolyline method
                xmlRectangularGeom.CartesianPoint = BH.Engine.XML.Convert.ToGBXML(pline.ControlPoints.Last());
                plGeo.PolyLoop = BH.Engine.XML.Convert.ToGBXML(pline);
                xmlSrf.PlanarGeometry = plGeo;
                xmlSrf.RectangularGeometry = xmlRectangularGeom;

                gbx.Campus.Surface.Add(xmlSrf);
            }
        }

        public static void SerializeCollection(IEnumerable<BuildingElement> inputElements, List<Level> levels, List<BuildingElement> openings, BH.oM.XML.GBXML gbx, ExportType exportType)
        {
            List<List<BuildingElement>> elementsAsSpaces = new List<List<BuildingElement>>();
            elementsAsSpaces.Add(inputElements.ToList());

            SerializeCollection(elementsAsSpaces, levels, openings, gbx, exportType);
        }
    }
}