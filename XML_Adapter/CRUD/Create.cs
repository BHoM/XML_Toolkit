using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.XML;
using BH.oM.Base;
using BHE = BH.oM.Environmental.Elements;
using BHP = BH.oM.Environmental.Properties;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.Engine.Environment;

namespace BH.Adapter.gbXML
{
    public class gbXMLSerializer
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static void Serialize<T>(IEnumerable<T> bhomObjects, BH.oM.XML.gbXML gbx, bool isIES) where T : IObject
        {
            SerializeCollection(bhomObjects as dynamic, gbx, isIES);

            // Document History                          
            DocumentHistory DocumentHistory = new DocumentHistory();
            DocumentHistory.CreatedBy.date = DateTime.Now.ToString();
            gbx.DocumentHistory = DocumentHistory;
        }

        /***************************************************/

        public static void SerializeCollection(IEnumerable<BHE.Building> bHoMBuilding, BH.oM.XML.gbXML gbx, bool isIES)
        {
            int buildingIndex = 0;
            foreach (BHE.Building building in bHoMBuilding)
            {
                SerializeCollection(building.Spaces, gbx, isIES, building); //Spaces
                SerializeCollection(building.BuildingElements, gbx, isIES); //ShadeElements

                //Construction and materials are only for the IES specific gbXML
                if (isIES)
                    SerializeCollection(building.BuildingElementProperties, gbx, isIES); //Construction and materials

                gbx.Campus.Location = BH.Engine.XML.Convert.ToGbXML(building);
                gbx.Campus.Building[buildingIndex].Area = (float)BH.Engine.XML.Query.BuildingArea(building);

                //From Custom Data
                if (building.CustomData.ContainsKey("Place Name"))
                    gbx.Campus.Building[buildingIndex].StreetAddress = (building.CustomData["Place Name"]).ToString();
                if (building.CustomData.ContainsKey("Building Name"))
                    gbx.Campus.Building[buildingIndex].buildingType = (building.CustomData["Building Name"]).ToString();

                buildingIndex++;
            }
        }

        /***************************************************/

        public static void SerializeCollection(IEnumerable<BHE.BuildingElement> bHoMBuildingElements, BH.oM.XML.gbXML gbx, bool isIES)
        {
            //This is for shading elements only (at the moment the other building elements are accessible from the spaces)
            List<BHE.BuildingElement> shadeElements = new List<BHE.BuildingElement>();
            List<BHE.BuildingElementPanel> bHoMPanels = new List<BHE.BuildingElementPanel>();
            foreach (BHE.BuildingElement element in bHoMBuildingElements)
            {
                if (element.AdjacentSpaces.Count == 0 && element.BuildingElementProperties != null && element.BuildingElementProperties.BuildingElementType != BHE.BuildingElementType.Window && element.BuildingElementProperties.BuildingElementType != BHE.BuildingElementType.Door)
                    shadeElements.Add(element);
            }

            bHoMPanels.AddRange(shadeElements.Select(x => x.BuildingElementGeometry as BHE.BuildingElementPanel));

            int panelIndex = 0;
            foreach (BHE.BuildingElement bHoMBuildingElement in shadeElements)
            {
                Surface xmlPanel = new Surface();
                string type = "Shade";
                xmlPanel.Name = "Shade-" + panelIndex.ToString();
                xmlPanel.surfaceType = type;

                if (bHoMBuildingElement.BuildingElementProperties != null)
                    xmlPanel.CADobjectId = BH.Engine.XML.Query.CadObjectId(bHoMBuildingElement, isIES);

                xmlPanel.id = "Shade-" + panelIndex.ToString();
                xmlPanel.exposedToSun = XML_Engine.Query.ExposedToSun(xmlPanel.surfaceType).ToString();

                if (isIES)
                    xmlPanel.constructionIdRef = BH.Engine.XML.Query.IdRef(bHoMBuildingElement); //Only for IES!

                RectangularGeometry xmlRectangularGeom = BH.Engine.XML.Convert.ToGbXML(bHoMBuildingElement.BuildingElementGeometry as BHE.BuildingElementPanel);
                PlanarGeometry plGeo = new PlanarGeometry();
                plGeo.id = "PlanarGeometry" + "shade";
                BHG.Polyline pline = new BHG.Polyline() { ControlPoints = (bHoMBuildingElement.BuildingElementGeometry as BHE.BuildingElementPanel).PolyCurve.ControlPoints() }; //TODO: Change to ToPolyline method
                xmlRectangularGeom.CartesianPoint = BH.Engine.XML.Convert.ToGbXML(pline.ControlPoints.Last());
                plGeo.PolyLoop = BH.Engine.XML.Convert.ToGbXML(pline);
                xmlPanel.PlanarGeometry = plGeo;
                xmlPanel.RectangularGeometry = xmlRectangularGeom;

                gbx.Campus.Surface.Add(xmlPanel);
                panelIndex++;
            }
        }

        /***************************************************/

        public static void SerializeCollection(IEnumerable<BHE.Space> bhomSpaces, BH.oM.XML.gbXML gbx, bool isIES = false, BHE.Building building = null)
        {
            //Levels unique by name in all spaces. We can access this info from the building, but we need it if the input is space (without building):
            List<BH.oM.Architecture.Elements.Level> levels = bhomSpaces.Select(x => x.Level).Distinct(new BH.Engine.Base.Objects.BHoMObjectNameComparer()).Select(x => x as BH.oM.Architecture.Elements.Level).ToList();

            //Make sure we only have spaces with geometries.
            List<BHE.Space> validSpaces = bhomSpaces.Where(x => x.BuildingElements.Count != 0).ToList();

            Serialize(levels, validSpaces.ToList(), gbx, isIES);
            List<BHE.BuildingElement> buildingElementsList = new List<oM.Environmental.Elements.BuildingElement>();
            List<BHP.BuildingElementProperties> propList = new List<oM.Environmental.Properties.BuildingElementProperties>();

            if (building == null)
            {
                buildingElementsList = validSpaces.SelectMany(x => x.BuildingElements).Cast<BHE.BuildingElement>().ToList();
                propList = buildingElementsList.Select(x => x.BuildingElementProperties).ToList();
            }
            else
            {
                buildingElementsList = building.BuildingElements;
                propList = building.BuildingElementProperties;
            }

            //Construction and materials are only for the IES specific gbXML
            //if (isIES)
            //    SerializeCollection(propList, gbx, isIES); //Construction and materials

            List<BHE.BuildingElement> uniqueBEs = new List<BHE.BuildingElement>(); //List with building elements with correct point order. 

            //Create surfaces for each space
            int panelIndex = 0;
            int openingIndex = 0;
            foreach (BHE.Space bHoMSpace in validSpaces)
            {
                List<BHE.BuildingElementPanel> bHoMPanels = new List<BHE.BuildingElementPanel>();
                List<BHE.BuildingElement> bHoMBuildingElement = new List<BHE.BuildingElement>();

                bHoMPanels.AddRange(bHoMSpace.BuildingElements.Select(x => x.BuildingElementGeometry as BHE.BuildingElementPanel));
                bHoMBuildingElement.AddRange(bHoMSpace.BuildingElements);

                List<BHE.Space> spaces = validSpaces.ToList();


                // Generate gbXMLSurfaces
                /***************************************************/
                if (bHoMPanels != null)
                {
                    List<Surface> srfs = new List<Surface>();

                    for (int i = 0; i < bHoMPanels.Count; i++)
                    {
                        Surface xmlPanel = new Surface();
                        xmlPanel.Name = "Panel-" + panelIndex.ToString();
                        xmlPanel.surfaceType = BH.Engine.XML.Convert.ToGbXMLType(bHoMBuildingElement[i], isIES);

                        if (bHoMBuildingElement[i].BuildingElementProperties != null)
                            xmlPanel.CADobjectId = BH.Engine.XML.Query.CadObjectId(bHoMBuildingElement[i], isIES);

                        xmlPanel.id = "Panel-" + panelIndex.ToString();
                        xmlPanel.exposedToSun = XML_Engine.Query.ExposedToSun(xmlPanel.surfaceType).ToString();
                        if (isIES)
                            xmlPanel.constructionIdRef = BH.Engine.XML.Query.IdRef(bHoMBuildingElement[i]); //Only for IES!

                        RectangularGeometry xmlRectangularGeom = BH.Engine.XML.Convert.ToGbXML(bHoMPanels[i]);
                        PlanarGeometry plGeo = new PlanarGeometry();
                        plGeo.id = "PlanarGeometry" + i.ToString();

                        /* Ensure that all of the surface coordinates are listed in a counterclockwise order
                         * This is a requirement of gbXML Polyloop definitions */

                        BHG.Polyline pline = new BHG.Polyline() { ControlPoints = bHoMPanels[i].PolyCurve.ControlPoints() }; //TODO: Change to ToPolyline method
                        BHG.Polyline srfBound = new BHG.Polyline();

                        if (!BH.Engine.XML.Query.NormalAwayFromSpace(pline, bHoMSpace))
                        {
                            plGeo.PolyLoop = BH.Engine.XML.Convert.ToGbXML(pline.Flip());
                            srfBound = pline.Flip();

                            xmlRectangularGeom.Tilt = Math.Round(BH.Engine.Environment.Query.Tilt(srfBound), 3);
                            xmlRectangularGeom.Azimuth = Math.Round(BH.Engine.Environment.Query.Azimuth(srfBound, BHG.Vector.YAxis), 3);

                        }
                        else
                        {
                            plGeo.PolyLoop = BH.Engine.XML.Convert.ToGbXML(pline);
                            srfBound = pline;
                        }

                        xmlPanel.PlanarGeometry = plGeo;
                        xmlPanel.RectangularGeometry = xmlRectangularGeom;

                        //AdjacentSpace
                        xmlPanel.AdjacentSpaceId = BH.Engine.XML.Query.GetAdjacentSpace(bHoMBuildingElement[i], spaces).ToArray();

                        //Remove duplicate surfaces
                        BHE.BuildingElement elementKeep = BH.Engine.XML.Query.ElementToKeep(bHoMBuildingElement[i], srfBound, spaces);
                        if (elementKeep != null)
                        {
                            uniqueBEs.Add(elementKeep);

                            //Create openings
                            if (bHoMPanels[i].Openings.Count > 0)
                                xmlPanel.Opening = Serialize(bHoMPanels[i].Openings, ref openingIndex, buildingElementsList, bHoMSpace, gbx, isIES).ToArray();

                            //If we have a curtain wall with GLZ we should create an extra opening (with tha size of the whole panel)
                            if (isIES && BH.Engine.XML.Query.CadObjectId(bHoMBuildingElement[i], isIES).Contains("Curtain Wall") && BH.Engine.XML.Query.CadObjectId(bHoMBuildingElement[i], isIES).Contains("GLZ"))
                            {
                                BHE.BuildingElement newBE = new BHE.BuildingElement();
                                newBE = BH.Engine.XML.Create.BuildingElementOpening(bHoMBuildingElement[i], bHoMBuildingElement[i].BuildingElementGeometry.ICurve());
                                xmlPanel.Opening = Serialize((newBE.BuildingElementGeometry as BHE.BuildingElementPanel).Openings, ref openingIndex, buildingElementsList, bHoMSpace, gbx, isIES).ToArray();
                            }

                            gbx.Campus.Surface.Add(xmlPanel);
                            panelIndex++;
                        }
                    }
                }

                // Generate gbXMLSpaces
                if (spaces != null)
                    Serialize(bHoMSpace, uniqueBEs, gbx, isIES);
            }
        }

        /***************************************************/

        public static void Serialize(List<BH.oM.Architecture.Elements.Level> levels, List<BHE.Space> bHoMSpaces, BH.oM.XML.gbXML gbx, bool isIES)
        {
            //Levels unique by name in all spaces:
            List<BH.oM.XML.BuildingStorey> xmlLevels = new List<BuildingStorey>();
            foreach (BH.oM.Architecture.Elements.Level level in levels)
            {
                BuildingStorey storey = BH.Engine.XML.Convert.ToGbXML(level);
                BHG.Polyline storeyGeometry = BH.Engine.XML.Query.StoreyGeometry(level, bHoMSpaces);
                if (storeyGeometry == null)
                    continue;
                storey.PlanarGeometry.PolyLoop = BH.Engine.XML.Convert.ToGbXML(storeyGeometry);
                xmlLevels.Add(storey);
            }

            gbx.Campus.Building[0].BuildingStorey = xmlLevels.ToArray();

        }

        /***************************************************/

        public static List<Opening> Serialize(List<BHE.BuildingElementOpening> bHoMOpenings, ref int openingIndex, List<BHE.BuildingElement> buildingElementsList, BHE.Space space, BH.oM.XML.gbXML gbx, bool isIES)
        {
            List<Opening> xmlOpenings = new List<Opening>();

            foreach (BHE.BuildingElementOpening opening in bHoMOpenings)
            {
                Opening gbXMLOpening = BH.Engine.XML.Convert.ToGbXML(opening);

                //normals away from space
                BHG.Polyline pline = new BHG.Polyline() { ControlPoints = opening.PolyCurve.ControlPoints() };
                if (!BH.Engine.XML.Query.NormalAwayFromSpace(pline, space))
                    gbXMLOpening.PlanarGeometry.PolyLoop = BH.Engine.XML.Convert.ToGbXML(pline.Flip());

                string familyName = "";
                string typeName = "";

                BHE.BuildingElement buildingElement = new BHE.BuildingElement();

                if (opening.CustomData.ContainsKey("Revit_elementId"))
                {
                    string elementID = (opening.CustomData["Revit_elementId"]).ToString();
                    buildingElement = buildingElementsList.Find(x => x != null && x.CustomData.ContainsKey("Revit_elementId") && x.CustomData["Revit_elementId"].ToString() == elementID);

                    if (buildingElement != null)
                    {

                        if (buildingElement.BuildingElementProperties.CustomData.ContainsKey("Family Name"))
                        {
                            familyName = buildingElement.BuildingElementProperties.CustomData["Family Name"].ToString();
                            typeName = buildingElement.BuildingElementProperties.Name;
                        }

                        gbXMLOpening.CADObjectId = BH.Engine.XML.Query.CadObjectId(opening, buildingElementsList, isIES);
                        gbXMLOpening.openingType = BH.Engine.XML.Convert.ToGbXMLType(buildingElement, isIES);

                        if (familyName == "System Panel") //No SAM_BuildingElementType for this one atm
                            gbXMLOpening.openingType = "FixedWindow";


                        if (isIES && gbXMLOpening.openingType.Contains("Window") && buildingElement.BuildingElementProperties.Name.Contains("SLD")) //Change windows with SLD construction into doors for IES
                            gbXMLOpening.openingType = "NonSlidingDoor";
                    }
                }

                gbXMLOpening.id = "opening-" + openingIndex.ToString();
                gbXMLOpening.Name = "opening-" + openingIndex.ToString();
                if (isIES)
                    gbXMLOpening.constructionIdRef = BH.Engine.XML.Query.IdRef(buildingElement); //Only for IES!
                xmlOpenings.Add(gbXMLOpening);
                openingIndex++;
            }

            return xmlOpenings;
        }

        /***************************************************/

        public static void Serialize(BHE.Space bHoMSpace, List<BHE.BuildingElement> uniqueBEs, BH.oM.XML.gbXML gbx, bool isIES)
        {
            List<BH.oM.XML.Space> xspaces = new List<Space>();
            BH.oM.XML.Space xspace = BH.Engine.XML.Convert.ToGbXML(bHoMSpace);

            //Closed Shell
            xspace.ShellGeometry.ClosedShell.PolyLoop = BH.Engine.XML.Query.ClosedShellGeometry(bHoMSpace).ToArray();

            //Space Boundaries
            xspace.SpaceBoundary = BH.Engine.XML.Query.SpaceBoundaries(bHoMSpace, uniqueBEs);

            //Planar Geometry
            if (BH.Engine.XML.Query.FloorGeometry(bHoMSpace) != null)
                xspace.PlanarGeoemtry.PolyLoop = BH.Engine.XML.Convert.ToGbXML(BH.Engine.XML.Query.FloorGeometry(bHoMSpace));

            gbx.Campus.Building[0].Space.Add(xspace);
        }

        /***************************************************/

        public static void Serialize(BHE.BuildingElementPanel bHoMPanel, BH.oM.XML.gbXML gbx, bool isIES)
        {
            throw new NotImplementedException();
        }

        /***************************************************/
        public static void SerializeCollection(List<BHP.BuildingElementProperties> bHoMProperties, BH.oM.XML.gbXML gbx, bool isIES) //Only for IES export
        {
            //Construction, Layers and Materials
            List<BH.oM.XML.Construction> xmlConstructions = new List<Construction>();
            List<BH.oM.XML.Layer> xmlLayers = new List<Layer>();
            List<BH.oM.XML.Material> xmlMaterials = new List<Material>();

            //Make sure we only have the unique construction categories in the building. 
            List<BHP.BuildingElementProperties> props = bHoMProperties.Distinct(new BH.Engine.Base.Objects.BHoMObjectNameComparer()).Select(x => x as BHP.BuildingElementProperties).ToList();

            foreach (BHP.BuildingElementProperties prop in props)
            {
                //Construction: Add all unique constructions to the xml file
                BH.oM.XML.Construction xmlConstruction = BH.Engine.XML.Convert.ToGbXML(prop);
                xmlConstruction.id = BH.Engine.XML.Query.IdRef(prop);
                xmlConstruction.LayerId.layerIdRef = BH.Engine.XML.Query.IdRef(prop);
                xmlConstructions.Add(xmlConstruction);

                //Layers: Add all unique layers to the xml file
                BH.oM.XML.Layer xmlLayer = new BH.oM.XML.Layer();
                xmlLayer.id = BH.Engine.XML.Query.IdRef(prop);
                xmlLayer.MaterialId.materialIdRef = BH.Engine.XML.Query.IdRef(prop);
                xmlLayers.Add(xmlLayer);

                //Materials: Add all unique materials to the xml file
                BH.oM.XML.Material xmlMaterial = new Material();
                xmlMaterial.id = BH.Engine.XML.Query.IdRef(prop);
                xmlMaterial.Name = prop.Name.ToString();
                xmlMaterial.Thickness = 0.01; //TODO: get the real thickness. At the moment we use this value because we need a thickess. Otherwise we end up with errors. 
                xmlMaterial.Conductivity = prop.ThermalConductivity;
                xmlMaterials.Add(xmlMaterial);
            }

            gbx.Construction = xmlConstructions.ToArray();
            gbx.Layer = xmlLayers.ToArray();
            gbx.Material = xmlMaterials.ToArray();

        }
        /***************************************************/
    }

}
