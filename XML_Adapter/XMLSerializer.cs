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

using BH.Engine.XML;

namespace BH.Adapter.GBXML
{
    public class GBXMLSerializer
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static void Serialize<T>(IEnumerable<T> bhomObjects, BH.oM.XML.GBXML gbx, bool isIES) where T : IObject
        {
            SerializeCollection(bhomObjects as dynamic, gbx, isIES);

            // Document History                          
            DocumentHistory DocumentHistory = new DocumentHistory();
            DocumentHistory.CreatedBy.Date = DateTime.Now.ToString();
            gbx.DocumentHistory = DocumentHistory;
        }

        /***************************************************/

        public static void SerializeCollection(IEnumerable<BHE.Building> bHoMBuilding, BH.oM.XML.GBXML gbx, bool isIES)
        {
            int buildingIndex = 0;
            foreach (BHE.Building building in bHoMBuilding)
            {
                SerializeCollection(building.Spaces, gbx, isIES, building); //Spaces
                SerializeCollection(building.BuildingElements, gbx, isIES); //ShadeElements

                //Construction and materials are only for the IES specific GBXML
                if (isIES)
                    SerializeCollection(building.BuildingElementProperties, gbx, isIES); //Construction and materials

                gbx.Campus.Location = BH.Engine.XML.Convert.ToGBXML(building);
                gbx.Campus.Building[buildingIndex].Area = (float)BH.Engine.Environment.Query.BuildingArea(building);

                //From Custom Data
                if (building.CustomData.ContainsKey("Place Name"))
                    gbx.Campus.Building[buildingIndex].StreetAddress = (building.CustomData["Place Name"]).ToString();
                if (building.CustomData.ContainsKey("Building Name"))
                    gbx.Campus.Building[buildingIndex].BuildingType = (building.CustomData["Building Name"]).ToString();

                buildingIndex++;
            }
        }

        /***************************************************/

        public static void SerializeCollection(IEnumerable<BHE.BuildingElement> bHoMBuildingElements, BH.oM.XML.GBXML gbx, bool isIES)
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
                xmlPanel.SurfaceType = type;

                if (bHoMBuildingElement.BuildingElementProperties != null)
                    xmlPanel.CADObjectID = BH.Engine.XML.Query.CadObjectId(bHoMBuildingElement, isIES);

                xmlPanel.ID = "Shade-" + panelIndex.ToString();
                xmlPanel.ExposedToSun = BH.Engine.Environment.Query.ExposedToSun(xmlPanel.SurfaceType).ToString();

                if (isIES)
                    xmlPanel.ConstructionIDRef = BH.Engine.XML.Query.IdRef(bHoMBuildingElement); //Only for IES!

                RectangularGeometry xmlRectangularGeom = BH.Engine.XML.Convert.ToGBXML(bHoMBuildingElement.BuildingElementGeometry as BHE.BuildingElementPanel);
                PlanarGeometry plGeo = new PlanarGeometry();
                plGeo.ID = "PlanarGeometry" + "shade";
                BHG.Polyline pline = new BHG.Polyline() { ControlPoints = (bHoMBuildingElement.BuildingElementGeometry as BHE.BuildingElementPanel).PolyCurve.ControlPoints() }; //TODO: Change to ToPolyline method
                xmlRectangularGeom.CartesianPoint = BH.Engine.XML.Convert.ToGBXML(pline.ControlPoints.Last());
                plGeo.PolyLoop = BH.Engine.XML.Convert.ToGBXML(pline);
                xmlPanel.PlanarGeometry = plGeo;
                xmlPanel.RectangularGeometry = xmlRectangularGeom;

                gbx.Campus.Surface.Add(xmlPanel);
                panelIndex++;
            }
        }

        /***************************************************/

        public static void SerializeCollection(IEnumerable<BHE.Space> bhomSpaces, BH.oM.XML.GBXML gbx, bool isIES = false, BHE.Building building = null)
        {
            //Levels unique by name in all spaces. We can access this info from the building, but we need it if the input is space (without building):
            List<BH.oM.Architecture.Elements.Level> levels = bhomSpaces.Select(x => x.Level).Distinct(new BH.Engine.Base.Objects.BHoMObjectNameComparer()).Select(x => x as BH.oM.Architecture.Elements.Level).ToList();

            //Make sure we only have spaces with geometries.
            List<BHE.Space> validSpaces = bhomSpaces.Where(x => x.BuildingElements.Count != 0).ToList();

            Serialize(levels, validSpaces.ToList(), gbx, isIES);
            List<BHE.BuildingElement> buildingElementsList = new List<oM.Environment.Elements.BuildingElement>();
            List<BHP.BuildingElementProperties> propList = new List<oM.Environment.Properties.BuildingElementProperties>();

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

            //Construction and materials are only for the IES specific GBXML
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


                // Generate GBXMLSurfaces
                /***************************************************/
                if (bHoMPanels != null)
                {
                    List<Surface> srfs = new List<Surface>();

                    for (int i = 0; i < bHoMPanels.Count; i++)
                    {
                        Surface xmlPanel = new Surface();
                        xmlPanel.Name = "Panel-" + panelIndex.ToString();
                        xmlPanel.SurfaceType = BH.Engine.XML.Convert.ToGBXMLType(bHoMBuildingElement[i], isIES);

                        if (bHoMBuildingElement[i].BuildingElementProperties != null)
                            xmlPanel.CADObjectID = BH.Engine.XML.Query.CadObjectId(bHoMBuildingElement[i], isIES);

                        xmlPanel.ID = "Panel-" + panelIndex.ToString();
                        xmlPanel.ExposedToSun = BH.Engine.Environment.Query.ExposedToSun(xmlPanel.SurfaceType).ToString();

                        if (isIES)
                            xmlPanel.ConstructionIDRef = BH.Engine.XML.Query.IdRef(bHoMBuildingElement[i]); //Only for IES!

                        RectangularGeometry xmlRectangularGeom = BH.Engine.XML.Convert.ToGBXML(bHoMPanels[i]);
                        PlanarGeometry plGeo = new PlanarGeometry();
                        plGeo.ID = "PlanarGeometry" + i.ToString();

                        /* Ensure that all of the surface coordinates are listed in a counterclockwise order
                         * This is a requirement of GBXML Polyloop definitions */

                        BHG.Polyline pline = new BHG.Polyline() { ControlPoints = bHoMPanels[i].PolyCurve.ControlPoints() }; //TODO: Change to ToPolyline method
                        BHG.Polyline srfBound = new BHG.Polyline();

                        if (!BH.Engine.Environment.Query.NormalAwayFromSpace(pline, bHoMSpace))
                        {
                            plGeo.PolyLoop = BH.Engine.XML.Convert.ToGBXML(pline.Flip());
                            srfBound = pline.Flip();

                            xmlRectangularGeom.Tilt = Math.Round(BH.Engine.Environment.Query.Tilt(srfBound), 3);
                            xmlRectangularGeom.Azimuth = Math.Round(BH.Engine.Environment.Query.Azimuth(srfBound, BHG.Vector.YAxis), 3);

                        }
                        else
                        {
                            plGeo.PolyLoop = BH.Engine.XML.Convert.ToGBXML(pline);
                            srfBound = pline;
                        }

                        xmlPanel.PlanarGeometry = plGeo;
                        xmlPanel.RectangularGeometry = xmlRectangularGeom;

                        //AdjacentSpace
                        xmlPanel.AdjacentSpaceID = BH.Engine.XML.Query.GetAdjacentSpace(bHoMBuildingElement[i], spaces).ToArray();

                        //Remove duplicate surfaces
                        BHE.BuildingElement elementKeep = BH.Engine.XML.Query.ElementToKeep(bHoMBuildingElement[i], srfBound, spaces);
                        if (elementKeep != null)
                        {
                            BHE.BuildingElement test = uniqueBEs.Where(x => x.BHoM_Guid == elementKeep.BHoM_Guid).FirstOrDefault();
                            if (test == null)
                            {

                                uniqueBEs.Add(elementKeep);

                                //Create openings
                                if (bHoMPanels[i].Openings.Count > 0)
                                    xmlPanel.Opening = Serialize(bHoMPanels[i].Openings, ref openingIndex, buildingElementsList, bHoMSpace, gbx, isIES).ToArray();

                                //If we have a curtain wall with GLZ we should create an extra opening (with tha size of the whole panel)
                                string cadObjID = BH.Engine.XML.Query.CadObjectId(bHoMBuildingElement[i], isIES);
                                if (isIES && cadObjID.Contains("Curtain Wall") && cadObjID.Contains("GLZ"))
                                {
                                    BHE.BuildingElement newBe = new BHE.BuildingElement();

                                    //Define boundaries for opening.
                                    List<BHG.ICurve> openingBounds = new List<oM.Geometry.ICurve>();

                                    if (bHoMPanels[i].Openings.Count > 0) //If a surface already has openings we need to cut them out.
                                    {
                                        List<BHG.Polyline> refRegion = (bHoMPanels[i].Openings.Where(x => x.PolyCurve != null).ToList().Select(x => x.PolyCurve.CollapseToPolyline(1e-06))).ToList();
                                        openingBounds.AddRange(BH.Engine.Geometry.Compute.BooleanDifference(new List<BHG.Polyline> { bHoMBuildingElement[i].BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06) }, refRegion, 0.01));
                                    }
                                    else
                                        openingBounds.Add(bHoMBuildingElement[i].BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06));

                                    newBe = bHoMBuildingElement[i].BuildingElementOpening(openingBounds);

                                    if (newBe != null)
                                        xmlPanel.Opening = Serialize((newBe.BuildingElementGeometry as BHE.BuildingElementPanel).Openings, ref openingIndex, buildingElementsList, bHoMSpace, gbx, isIES).ToArray();
                                }
                                gbx.Campus.Surface.Add(xmlPanel);
                                panelIndex++;
                            }
                        }
                    }
                }

                // Generate GBXMLSpaces
                if (spaces != null)
                    Serialize(bHoMSpace, uniqueBEs, gbx, isIES);
            }
        }

        /***************************************************/

        public static void Serialize(List<BH.oM.Architecture.Elements.Level> levels, List<BHE.Space> bHoMSpaces, BH.oM.XML.GBXML gbx, bool isIES)
        {
            //Levels unique by name in all spaces:
            List<BH.oM.XML.BuildingStorey> xmlLevels = new List<BuildingStorey>();
            foreach (BH.oM.Architecture.Elements.Level level in levels)
            {
                BuildingStorey storey = BH.Engine.XML.Convert.ToGBXML(level);
                BHG.Polyline storeyGeometry = BH.Engine.Environment.Query.StoreyGeometry(level, bHoMSpaces);
                if (storeyGeometry == null)
                    continue;
                storey.PlanarGeometry.PolyLoop = BH.Engine.XML.Convert.ToGBXML(storeyGeometry);
                xmlLevels.Add(storey);
            }

            gbx.Campus.Building[0].BuildingStorey = xmlLevels.ToArray();

        }

        /***************************************************/

        public static List<Opening> Serialize(List<BHE.BuildingElementOpening> bHoMOpenings, ref int openingIndex, List<BHE.BuildingElement> buildingElementsList, BHE.Space space, BH.oM.XML.GBXML gbx, bool isIES)
        {
            List<Opening> xmlOpenings = new List<Opening>();

            foreach (BHE.BuildingElementOpening opening in bHoMOpenings)
            {
                if (opening.PolyCurve == null)
                    continue;

                Opening GBXMLOpening = BH.Engine.XML.Convert.ToGBXML(opening);

                //normals away from space
                BHG.Polyline pline = new BHG.Polyline() { ControlPoints = opening.PolyCurve.ControlPoints() };
                if (!BH.Engine.Environment.Query.NormalAwayFromSpace(pline, space))
                    GBXMLOpening.PlanarGeometry.PolyLoop = BH.Engine.XML.Convert.ToGBXML(pline.Flip());

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

                        GBXMLOpening.CADObjectID = BH.Engine.XML.Query.CadObjectId(opening, buildingElementsList, isIES);
                        GBXMLOpening.OpeningType = BH.Engine.XML.Convert.ToGBXMLType(buildingElement, isIES);

                        if (familyName == "System Panel") //No SAM_BuildingElementType for this one atm
                            GBXMLOpening.OpeningType = "FixedWindow";


                        if (isIES && GBXMLOpening.OpeningType.Contains("Window") && buildingElement.BuildingElementProperties.Name.Contains("SLD")) //Change windows with SLD construction into doors for IES
                            GBXMLOpening.OpeningType = "NonSlidingDoor";
                    }
                }

                GBXMLOpening.ID = "opening-" + openingIndex.ToString();
                GBXMLOpening.Name = "opening-" + openingIndex.ToString();
                if (isIES)
                    GBXMLOpening.ConstructionIDRef = BH.Engine.XML.Query.IdRef(buildingElement); //Only for IES!
                xmlOpenings.Add(GBXMLOpening);
                openingIndex++;
            }

            return xmlOpenings;
        }

        /***************************************************/

        public static void Serialize(BHE.Space bHoMSpace, List<BHE.BuildingElement> uniqueBEs, BH.oM.XML.GBXML gbx, bool isIES)
        {
            List<BH.oM.XML.Space> xspaces = new List<Space>();
            BH.oM.XML.Space xspace = BH.Engine.XML.Convert.ToGBXML(bHoMSpace);

            //Closed Shell
            xspace.ShellGeometry.ClosedShell.PolyLoop = BH.Engine.XML.Query.ClosedShellGeometry(bHoMSpace).ToArray();

            //Space Boundaries
            xspace.SpaceBoundary = BH.Engine.XML.Query.SpaceBoundaries(bHoMSpace, uniqueBEs);

            //Planar Geometry
            if (BH.Engine.Environment.Query.FloorGeometry(bHoMSpace) != null)
                xspace.PlanarGeoemtry.PolyLoop = BH.Engine.XML.Convert.ToGBXML(BH.Engine.Environment.Query.FloorGeometry(bHoMSpace));

            gbx.Campus.Building[0].Space.Add(xspace);
        }

        /***************************************************/

        public static void Serialize(BHE.BuildingElementPanel bHoMPanel, BH.oM.XML.GBXML gbx, bool isIES)
        {
            throw new NotImplementedException();
        }

        /***************************************************/
        public static void SerializeCollection(List<BHP.BuildingElementProperties> bHoMProperties, BH.oM.XML.GBXML gbx, bool isIES) //Only for IES export
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
                BH.oM.XML.Construction xmlConstruction = BH.Engine.XML.Convert.ToGBXML(prop);
                xmlConstruction.ID = BH.Engine.XML.Query.IdRef(prop);
                xmlConstruction.LayerID.LayerIDRef = BH.Engine.XML.Query.IdRef(prop);
                xmlConstructions.Add(xmlConstruction);

                //Layers: Add all unique layers to the xml file
                BH.oM.XML.Layer xmlLayer = new BH.oM.XML.Layer();
                xmlLayer.ID = BH.Engine.XML.Query.IdRef(prop);
                xmlLayer.MaterialID.MaterialIDRef = BH.Engine.XML.Query.IdRef(prop);
                xmlLayers.Add(xmlLayer);

                //Materials: Add all unique materials to the xml file
                BH.oM.XML.Material xmlMaterial = new Material();
                xmlMaterial.ID = BH.Engine.XML.Query.IdRef(prop);
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