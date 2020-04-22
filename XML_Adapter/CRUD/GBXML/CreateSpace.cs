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
using System.Text;
using System.Threading.Tasks;

using BH.oM.External.XML.Settings;
using GBXML = BH.oM.External.XML.GBXML;
using BH.oM.Environment.Elements;
using BH.oM.Environment.Fragments;
using BH.oM.Geometry.SettingOut;

using BH.Engine.Environment;

namespace BH.Adapter.XML
{
    public partial class XMLAdapter : BHoMAdapter
    {
        private List<GBXML.Space> CreateSpace(List<List<Panel>> panelsAsSpaces, List<Level> levels, GBXMLSettings settings)
        {
            List<GBXML.Space> xmlSpaces = new List<GBXML.Space>();

            List<List<Panel>> elementsAsSpaces = new List<List<Panel>>();

            foreach (IEnumerable<Panel> input in panelsAsSpaces)
                elementsAsSpaces.Add(input.ToList());

            List<Panel> uniqueBuildingElements = elementsAsSpaces.UniquePanels();

            List<Panel> usedPanels = new List<Panel>();

            //List<BH.oM.XML.Construction> usedConstructions = new List<BH.oM.XML.Construction>();
            //List<BH.oM.XML.Material> usedMaterials = new List<Material>();
            //List<BH.oM.XML.Layer> usedLayers = new List<Layer>();
            List<string> usedSpaceNames = new List<string>();
            //List<BH.oM.XML.WindowType> usedWindows = new List<WindowType>();

            foreach (List<Panel> space in elementsAsSpaces)
            {
                //For each collection of BuildingElements that define a space, convert the panels to XML surfaces and add to the GBXML
                List<GBXML.Surface> spaceSurfaces = new List<GBXML.Surface>();

                for (int x = 0; x < space.Count; x++)
                {
                    if (usedPanels.Where(i => i.BHoM_Guid == space[x].BHoM_Guid).FirstOrDefault() != null) continue;

                    OriginContextFragment envContextProperties = space[x].FindFragment<OriginContextFragment>(typeof(OriginContextFragment));

                    List<List<Panel>> adjacentSpaces = BH.Engine.Environment.Query.AdjacentSpaces(space[x], elementsAsSpaces);

                    GBXML.Surface srf = space[x].ToGBXML(adjacentSpaces, space, settings);
                    srf.ID = "Panel-" + gbx.Campus.Surface.Count.ToString().Replace(" ", "").Replace("-", "");
                    srf.Name = "Panel" + gbx.Campus.Surface.Count.ToString().Replace(" ", "").Replace("-", "");

                    if (space[x] != null)
                        srf.CADObjectID = BH.Engine.XML.Query.CADObjectID(space[x], settings.ReplaceCurtainWalls);

                    if (settings.IncludeConstructions)
                        srf.ConstructionIDRef = (envContextProperties != null ? envContextProperties.TypeName.CleanName() : space[x].ConstructionID());
                    else
                        srf.ConstructionIDRef = null;

                    if (settings.ReplaceCurtainWalls)
                    {
                        //If the surface is a basic Wall: SIM_EXT_GLZ so Curtain Wall after CADObjectID translation add the wall as an opening
                        if (srf.CADObjectID.Contains("Curtain") && srf.CADObjectID.Contains("GLZ"))
                        {
                            List<BHG.Polyline> newOpeningBounds = new List<oM.Geometry.Polyline>();
                            if (space[x].Openings.Count > 0)
                            {
                                //This surface already has openings - cut them out of the new opening
                                List<BHG.Polyline> refRegion = space[x].Openings.Where(y => y.Polyline() != null).ToList().Select(z => z.Polyline()).ToList();

                                newOpeningBounds.AddRange(BH.Engine.Geometry.Triangulation.Compute.DelaunayTriangulation(space[x].Polyline(), refRegion, conformingDelaunay: false));
                                List<BHG.Polyline> outer = new List<BHG.Polyline> { space[x].Polyline() };
                                double outerArea = space[x].Area();
                                for (int z = 0; z > space[x].Openings.Count; z++)
                                {
                                    BHG.Polyline pLine = outer.BooleanDifference(new List<BHG.Polyline> { space[x].Openings[z].Polyline() })[0];
                                    if (pLine.Area() != outerArea)
                                        space[x].Openings[z].Edges = space[x].Openings[z].Polyline().Offset(settings.OffsetDistance).ToEdges();
                                }
                            }
                            else
                                newOpeningBounds.Add(space[x].Polyline());

                            foreach (BHG.Polyline b in newOpeningBounds)
                            {
                                BH.oM.Environment.Elements.Opening curtainWallOpening = BH.Engine.Environment.Create.Opening(externalEdges: b.ToEdges());
                                curtainWallOpening.Name = space[x].Name;
                                BHP.OriginContextFragment curtainWallProperties = new BHP.OriginContextFragment();
                                if (envContextProperties != null)
                                {
                                    curtainWallProperties.ElementID = envContextProperties.ElementID;
                                    curtainWallProperties.TypeName = envContextProperties.TypeName;
                                }

                                curtainWallOpening.Type = OpeningType.CurtainWall;
                                curtainWallOpening.OpeningConstruction = space[x].Construction;

                                curtainWallOpening.Fragments.Add(curtainWallProperties);

                                space[x].Openings.Add(curtainWallOpening);
                            }
                            //Update the host elements element type
                            srf.SurfaceType = (adjacentSpaces.Count == 1 ? PanelType.WallExternal : PanelType.WallInternal).ToGBXML();
                            srf.ExposedToSun = BH.Engine.XML.Query.ExposedToSun(srf.SurfaceType).ToString().ToLower();
                        }
                    }
                    else
                    {
                        //Fix surface type for curtain walls
                        if (space[x].Type == PanelType.CurtainWall)
                        {
                            srf.SurfaceType = (adjacentSpaces.Count == 1 ? PanelType.WallExternal : PanelType.WallInternal).ToGBXML();
                            srf.ExposedToSun = BH.Engine.XML.Query.ExposedToSun(srf.SurfaceType).ToString().ToLower();
                        }
                    }

                    if (settings.FixIncorrectAirTypes && space[x].Type == PanelType.Undefined && space[x].ConnectedSpaces.Count == 1)
                    {
                        //Fix external air types
                        if (space[x].Tilt(settings.AngleTolerance) == 0)
                            srf.SurfaceType = PanelType.Roof.ToGBXML();
                        else if (space[x].Tilt(settings.AngleTolerance) == 90)
                            srf.SurfaceType = PanelType.WallExternal.ToGBXML();
                        else if (space[x].Tilt(settings.AngleTolerance) == 180)
                            srf.SurfaceType = PanelType.SlabOnGrade.ToGBXML();
                    }

                    //Openings
                    if (space[x].Openings.Count > 0)
                    {
                        srf.Opening = SerializeOpenings(space[x].Openings, space, allElements, elementsAsSpaces, gbx, settings, space[x]).ToArray();
                        foreach (BH.oM.Environment.Elements.Opening o in space[x].Openings)
                        {
                            string nameCheck = "";

                            BHP.OriginContextFragment openingEnvContextProperties = o.FindFragment<BHP.OriginContextFragment>(typeof(BHP.OriginContextFragment));

                            if (openingEnvContextProperties != null)
                                nameCheck = openingEnvContextProperties.TypeName;
                            else if (o.OpeningConstruction != null)
                                nameCheck = o.OpeningConstruction.Name;

                            var t = usedWindows.Where(a => a.Name == nameCheck).FirstOrDefault();
                            if (t == null && o.OpeningConstruction != null)
                                usedWindows.Add(o.OpeningConstruction.ToGBXMLWindow(o));
                        }
                    }

                    gbx.Campus.Surface.Add(srf);

                    usedPanels.Add(space[x]);

                    if (settings.IncludeConstructions && space[x].Construction != null)
                    {
                        BH.oM.XML.Construction conc = space[x].ToGBXMLConstruction();
                        BH.oM.XML.Construction test = usedConstructions.Where(y => y.ID == conc.ID).FirstOrDefault();
                        if (test == null)
                        {
                            List<BH.oM.XML.Material> materials = new List<Material>();
                            BHC.Construction construction = space[x].Construction as BHC.Construction;

                            foreach (BHC.Layer l in construction.Layers)
                                materials.Add(l.ToGBXML());

                            BH.oM.XML.Layer layer = materials.ToGBXML();
                            conc.LayerID.LayerIDRef = layer.ID;

                            usedConstructions.Add(conc);

                            if (usedLayers.Where(y => y.ID == layer.ID).FirstOrDefault() == null)
                                usedLayers.Add(layer);

                            foreach (BH.oM.XML.Material mat in materials)
                            {
                                if (usedMaterials.Where(y => y.ID == mat.ID).FirstOrDefault() == null)
                                    usedMaterials.Add(mat);
                            }
                        }
                    }
                }

                BH.oM.XML.Space xmlSpace = new oM.XML.Space();
                xmlSpace.Name = space.ConnectedSpaceName();
                xmlSpace.ID = "Space" + xmlSpace.Name.Replace(" ", "").Replace("-", "");
                xmlSpace.CADObjectID = BH.Engine.XML.Query.CADObjectID(space);
                xmlSpace.ShellGeometry.ClosedShell.PolyLoop = BH.Engine.XML.Query.ClosedShellGeometry(space, settings.PlanarTolerance).ToArray();
                xmlSpace.ShellGeometry.ID = "SpaceShellGeometry-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
                xmlSpace.SpaceBoundary = BH.Engine.XML.Query.SpaceBoundaries(space, uniqueBuildingElements, settings.PlanarTolerance);
                xmlSpace.PlanarGeoemtry.ID = "SpacePlanarGeometry-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
                if (BH.Engine.Environment.Query.FloorGeometry(space) != null)
                {
                    xmlSpace.PlanarGeoemtry.PolyLoop = BH.Engine.XML.Convert.ToGBXML(BH.Engine.Environment.Query.FloorGeometry(space));
                    xmlSpace.Area = BH.Engine.Environment.Query.FloorGeometry(space).Area();
                    xmlSpace.Volume = space.Volume();
                }
                Level spaceLevel = space.Level(levels);
                if (spaceLevel != null)
                {
                    string levelName = "";
                    if (spaceLevel.Name == "")
                        levelName = spaceLevel.Elevation.ToString();
                    else
                        levelName = spaceLevel.Name;

                    xmlSpace.BuildingStoreyIDRef = "Level-" + levelName.Replace(" ", "").ToLower();
                }

                gbx.Campus.Building[0].Space.Add(xmlSpace);

                usedSpaceNames.Add(xmlSpace.Name);
            }

            //Reorder the spaces
            gbx.Campus.Building[0].Space = gbx.Campus.Building[0].Space.OrderBy(x => x.Name).ToList();

            gbx.Construction = usedConstructions.ToArray();
            gbx.Layer = usedLayers.ToArray();
            gbx.Material = usedMaterials.ToArray();

            if (settings.IncludeConstructions)
                gbx.WindowType = usedWindows.ToArray();
            else //We have to force null otherwise WindowType will be created
                gbx.WindowType = null;

            //Set the building area
            List<Panel> floorElements = allElements.Where(x => x.Type == PanelType.Floor || x.Type == PanelType.FloorExposed || x.Type == PanelType.FloorInternal || x.Type == PanelType.FloorRaised || x.Type == PanelType.SlabOnGrade || x.Type == PanelType.UndergroundSlab).ToList();

            double buildingFloorArea = 0;
            foreach (Panel be in floorElements)
                buildingFloorArea += be.Area();

            gbx.Campus.Building[0].Area = buildingFloorArea;

            return xmlSpaces;
        }
    }
}
