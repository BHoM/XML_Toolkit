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

using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;

using BH.oM.Architecture.Elements;

using BH.oM.XML.Enums;

using BHP = BH.oM.Environment.Properties;

using BH.oM.Environment.Interface;

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
            List<string> usedSpaceNames = new List<string>();
            List<BH.oM.XML.WindowType> usedWindows = new List<WindowType>();

            foreach (List<BuildingElement> space in elementsAsSpaces)
            {
                //For each collection of BuildingElements that define a space, convert the panels to XML surfaces and add to the GBXML
                List<Surface> spaceSurfaces = new List<Surface>();

                for (int x = 0; x < space.Count; x++)
                {
                    if (usedBEs.Where(i => i.BHoM_Guid == space[x].BHoM_Guid).FirstOrDefault() != null) continue;

                    BHP.EnvironmentContextProperties envContextProperties = space[x].EnvironmentContextProperties() as BHP.EnvironmentContextProperties;
                    BHP.ElementProperties elementProperties = space[x].ElementProperties() as BHP.ElementProperties;

                    List<BH.oM.Environment.Elements.Space> adjacentSpaces = BH.Engine.Environment.Query.AdjacentSpaces(space[x], elementsAsSpaces, spaces);

                    Surface srf = space[x].ToGBXML(adjacentSpaces, space);
                    srf.ID = "Panel-" + gbx.Campus.Surface.Count.ToString();
                    srf.Name = "Panel-" + gbx.Campus.Surface.Count.ToString();

                    if (space[x] != null)
                        srf.CADObjectID = BH.Engine.XML.Query.CADObjectID(space[x], exportType);

                    if (exportType == ExportType.gbXMLIES)
                    {
                        srf.ConstructionIDRef = (envContextProperties != null ? envContextProperties.TypeName.CleanName() : space[x].ConstructionID());

                        //If the surface is a basic Wall: SIM_EXT_GLZ so Curtain Wall after CADObjectID translation add the wall as an opening
                        if (srf.CADObjectID.Contains("Curtain") && srf.CADObjectID.Contains("Wall") && srf.CADObjectID.Contains("GLZ"))
                        {
                            List<BHG.Polyline> newOpeningBounds = new List<oM.Geometry.Polyline>();
                            if (space[x].Openings.Count > 0)
                            {
                                //This surface already has openings - cut them out of the new opening
                                List<BHG.Polyline> refRegion = space[x].Openings.Where(y => y.OpeningCurve != null).ToList().Select(z => z.OpeningCurve.ICollapseToPolyline(BHG.Tolerance.Angle)).ToList();
                                newOpeningBounds.AddRange((new List<BHG.Polyline> { space[x].PanelCurve.ICollapseToPolyline(BHG.Tolerance.Angle) }).BooleanDifference(refRegion, 0.01));
                            }
                            else
                                newOpeningBounds.Add(space[x].PanelCurve.ICollapseToPolyline(BHG.Tolerance.Angle));

                            BH.oM.Environment.Elements.Opening curtainWallOpening = BH.Engine.Environment.Create.Opening(newOpeningBounds);
                            curtainWallOpening.Name = space[x].Name;
                            BHP.EnvironmentContextProperties curtainWallProperties = new BHP.EnvironmentContextProperties();
                            if (envContextProperties != null)
                            {
                                curtainWallProperties.ElementID = envContextProperties.ElementID;
                                curtainWallProperties.TypeName = envContextProperties.TypeName;
                            }

                            BHP.ElementProperties curtainElementProperties = new BHP.ElementProperties();
                            if (elementProperties != null)
                            {
                                curtainElementProperties.BuildingElementType = BuildingElementType.CurtainWall;
                                curtainElementProperties.Construction = elementProperties.Construction;
                            }

                            //Update the host elements element type
                            srf.SurfaceType = (adjacentSpaces.Count == 1 ? BuildingElementType.WallExternal : BuildingElementType.WallInternal).ToGBXML();

                            curtainWallOpening.ExtendedProperties.Add(curtainWallProperties);
                            curtainWallOpening.ExtendedProperties.Add(curtainElementProperties);

                            space[x].Openings.Add(curtainWallOpening);
                        }
                    }
                    else if (exportType == ExportType.gbXMLTAS)
                    {
                        srf.ConstructionIDRef = null;
                        //Fix surface type for curtain walls
                        if (srf.CADObjectID.Contains("Curtain Basic") && srf.CADObjectID.Contains("GLZ") && elementProperties != null && elementProperties.BuildingElementType == BuildingElementType.CurtainWall)
                            srf.SurfaceType = (adjacentSpaces.Count == 1 ? BuildingElementType.WallExternal : BuildingElementType.WallInternal).ToGBXML();
                    }                   

                    //Openings
                    if (space[x].Openings.Count > 0)
                    {
                        srf.Opening = Serialize(space[x].Openings, space, allElements, elementsAsSpaces, spaces, gbx, exportType).ToArray();
                        foreach(BH.oM.Environment.Elements.Opening o in space[x].Openings)
                        {
                            string nameCheck = "";

                            BHP.EnvironmentContextProperties openingEnvContextProperties = o.EnvironmentContextProperties() as BHP.EnvironmentContextProperties;
                            BHP.ElementProperties openingElementProperties = o.ElementProperties() as BHP.ElementProperties;

                            if (openingEnvContextProperties != null)
                                nameCheck = openingEnvContextProperties.TypeName;
                            else if (openingElementProperties != null && openingElementProperties.Construction != null)
                                nameCheck = openingElementProperties.Construction.Name;
                            
                            var t = usedWindows.Where(a => a.Name == nameCheck).FirstOrDefault();
                            if (t == null)
                                usedWindows.Add(openingElementProperties.Construction.ToGBXMLWindow(o));
                        }
                    }

                    gbx.Campus.Surface.Add(srf);

                    usedBEs.Add(space[x]);

                    if (exportType == ExportType.gbXMLIES)
                    {
                        BH.oM.XML.Construction conc = space[x].ToGBXMLConstruction();
                        BH.oM.XML.Construction test = usedConstructions.Where(y => y.ID == conc.ID).FirstOrDefault();
                        if (test == null)
                        {
                            if (space[x].ElementProperties() != null)
                            {
                                List<BH.oM.XML.Material> materials = new List<Material>();
                                BH.oM.Environment.Elements.Construction construction = (space[x].ElementProperties() as BHP.ElementProperties).Construction;

                                foreach (BH.oM.Environment.Materials.Material m in construction.Materials)
                                    materials.Add(m.ToGBXML());

                                BH.oM.XML.Layer layer = materials.ToGBXML();
                                conc.LayerID.LayerIDRef = layer.ID;

                                usedConstructions.Add(conc);
                                usedLayers.Add(layer);

                                foreach(BH.oM.XML.Material mat in materials)
                                {
                                    if (usedMaterials.Where(y => y.ID == mat.ID).FirstOrDefault() == null)
                                        usedMaterials.Add(mat);
                                }
                            }
                        }
                    }
                }

                //BuildingElement elementForSpace = space.Where(x => x.CustomData.ContainsKey("Space_Custom_Data") && !usedSpaceNames.Contains(x.CustomData["SAM_SPACE_NAME_TEST"].ToString())).FirstOrDefault();
                BuildingElement elementForSpace = space.Where(x => x.CustomData.ContainsKey("Space_Custom_Data") && !usedSpaceNames.Contains((x.CustomData["Space_Custom_Data"] as Dictionary<string, object>)["SAM_SpaceName"].ToString())).FirstOrDefault();
                Dictionary<string, object> spaceData = null;
                if (elementForSpace != null)
                    spaceData = elementForSpace.CustomData["Space_Custom_Data"] as Dictionary<string, object>;

                spaceData = spaceData ?? new Dictionary<string, object>();

                BH.oM.Environment.Elements.Space s = space.Space(gbx.Campus.Building[0].Space.Count, gbx.Campus.Building[0].Space.Count.ToString());
                BH.oM.XML.Space xmlSpace = new oM.XML.Space();
                xmlSpace.Name = (spaceData.ContainsKey("SAM_SpaceName") && spaceData["SAM_SpaceName"] != null ? spaceData["SAM_SpaceName"].ToString() : s.Name); //CUSTOMDATA SAM_SpaceName
                xmlSpace.ID = "Space-" + s.Number + "-" + s.Name;
                xmlSpace.CADObjectID = BH.Engine.XML.Query.CADObjectID(space);
                xmlSpace.ShellGeometry.ClosedShell.PolyLoop = BH.Engine.XML.Query.ClosedShellGeometry(space).ToArray();
                xmlSpace.ShellGeometry.ID = "SpaceShellGeometry-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
                xmlSpace.SpaceBoundary = BH.Engine.XML.Query.SpaceBoundaries(space, uniqueBuildingElements);
                xmlSpace.PlanarGeoemtry.ID = "SpacePlanarGeometry-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
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

                usedSpaceNames.Add(xmlSpace.Name);
            }

            gbx.Construction = usedConstructions.ToArray();
            gbx.Layer = usedLayers.ToArray();
            gbx.Material = usedMaterials.ToArray();

            if (exportType == ExportType.gbXMLIES)
                gbx.WindowType = usedWindows.ToArray();
            else if (exportType == ExportType.gbXMLTAS)//We have to force null otherwise WindowType will be created
                gbx.WindowType = null;
        }

        public static void SerializeCollection(IEnumerable<BuildingElement> inputElements, BH.oM.XML.GBXML gbx, ExportType exportType)
        {
            //For serializing shade elements
            List<BuildingElement> buildingElements = inputElements.ToList();

            foreach (BuildingElement be in buildingElements)
            {
                Surface gbSrf = be.ToGBXML();
                gbSrf.ID = "Panel-" + gbx.Campus.Surface.Count.ToString();
                gbSrf.Name = "Panel-" + gbx.Campus.Surface.Count.ToString();
                gbSrf.SurfaceType = "Shade";
                gbSrf.ExposedToSun = BH.Engine.Environment.Query.ExposedToSun(gbSrf.SurfaceType).ToString().ToLower();
                gbSrf.CADObjectID = be.CADObjectID();

                if (exportType == ExportType.gbXMLIES)
                    gbSrf.ConstructionIDRef = be.ConstructionID();
                else if (exportType == ExportType.gbXMLTAS) //We have to force null otherwise Construction will be created
                    gbSrf.ConstructionIDRef = null;

                gbx.Campus.Surface.Add(gbSrf);
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