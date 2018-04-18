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

        public static void Serialize<T>(IEnumerable<T> bhomObjects, BH.oM.XML.gbXML gbx) where T : IObject
        {
            SerializeCollection(bhomObjects as dynamic, gbx);

            // Document History                          
            DocumentHistory DocumentHistory = new DocumentHistory();
            DocumentHistory.CreatedBy.date = DateTime.Now.ToString();
            gbx.DocumentHistory = DocumentHistory;
        }

        /***************************************************/

        public static void SerializeCollection(IEnumerable<BHE.Building> bHoMBuilding, BH.oM.XML.gbXML gbx)
        {
            int buildingIndex = 0;
            foreach (BHE.Building building in bHoMBuilding)
            {
                SerializeCollection(building.Spaces, gbx, building); //Spaces
                SerializeCollection(building.BuildingElements, gbx); //ShadeElements

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

        public static void SerializeCollection(IEnumerable<BHE.BuildingElement> bHoMBuildingElements, BH.oM.XML.gbXML gbx)
        {
            //This is for shading elements only (at the moment the other building elements are accessible from the spaces)
            List<BHE.BuildingElement> shadeElements = new List<BHE.BuildingElement>();
            List<BHE.BuildingElementPanel> bHoMPanels = new List<BHE.BuildingElementPanel>();
            foreach (BHE.BuildingElement element in bHoMBuildingElements)
            {
                if (element.AdjacentSpaces.Count == 0 && element.BuildingElementProperties != null)
                    if (element.BuildingElementProperties.BuildingElementType != BHE.BuildingElementType.Window)
                        if (element.BuildingElementProperties.BuildingElementType != BHE.BuildingElementType.Door)
                            shadeElements.Add(element);
            }

            bHoMPanels.AddRange(shadeElements.Select(x => x.BuildingElementGeometry as BHE.BuildingElementPanel));

            double panelIndex = 0;
            foreach (BHE.BuildingElement bHoMBuildingElement in shadeElements)
            {
                Surface xmlPanel = new Surface();
                string type = "Shade";
                xmlPanel.Name = "Shade-" + panelIndex.ToString();
                xmlPanel.surfaceType = type;

                if (bHoMBuildingElement.BuildingElementProperties != null)
                    xmlPanel.CADobjectId = BH.Engine.XML.Query.CadObjectId(bHoMBuildingElement);

                xmlPanel.id = "Shade-" + panelIndex.ToString();
                xmlPanel.exposedToSun = XML_Engine.Query.ExposedToSun(xmlPanel.surfaceType).ToString();

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

        public static void SerializeCollection(IEnumerable<BHE.Space> bhomSpaces, BH.oM.XML.gbXML gbx, BHE.Building building = null)
        {
            //Levels unique by name in all spaces. We can access this info from the building, but we need it if the input is space (without building):
            List<BH.oM.Architecture.Elements.Level> levels = bhomSpaces.Select(x => x.Level).Distinct(new BH.Engine.Base.Objects.BHoMObjectNameComparer()).Select(x => x as BH.oM.Architecture.Elements.Level).ToList();
            Serialize(levels, bhomSpaces.ToList(), gbx);

            List<BHE.BuildingElement> buildingElementsList;

            if (building == null)
                buildingElementsList = bhomSpaces.SelectMany(x => x.BuildingElements).Cast<BHE.BuildingElement>().ToList();
            else
                buildingElementsList = building.BuildingElements;


            //Create surfaces for each space
            int panelindex = 0;
            int openingIndex = 0;
            foreach (BHE.Space bHoMSpace in bhomSpaces)
            {
                List<BHE.BuildingElementPanel> bHoMPanels = new List<BHE.BuildingElementPanel>();
                List<BHE.BuildingElement> bHoMBuildingElement = new List<BHE.BuildingElement>();

                bHoMPanels.AddRange(bHoMSpace.BuildingElements.Select(x => x.BuildingElementGeometry as BHE.BuildingElementPanel));
                bHoMBuildingElement.AddRange(bHoMSpace.BuildingElements);

                List<BHE.Space> spaces = bhomSpaces.ToList();


                // Generate gbXMLSurfaces
                /***************************************************/
                if (bHoMPanels != null)
                {
                    List<Surface> srfs = new List<Surface>();

                    for (int i = 0; i < bHoMPanels.Count; i++)
                    {
                        Surface xmlPanel = new Surface();
                        string type = "Air";
                        xmlPanel.Name = "Panel-" + panelindex.ToString();
                        xmlPanel.surfaceType = type;
                        xmlPanel.surfaceType = BH.Engine.XML.Convert.ToGbXMLType(bHoMBuildingElement[i]);

                        if (bHoMBuildingElement[i].BuildingElementProperties != null)
                            xmlPanel.CADobjectId = BH.Engine.XML.Query.CadObjectId(bHoMBuildingElement[i]);

                        xmlPanel.id = "Panel-" + panelindex.ToString();
                        xmlPanel.exposedToSun = XML_Engine.Query.ExposedToSun(xmlPanel.surfaceType).ToString();

                        RectangularGeometry xmlRectangularGeom = BH.Engine.XML.Convert.ToGbXML(bHoMPanels[i]);
                        PlanarGeometry plGeo = new PlanarGeometry();
                        plGeo.id = "PlanarGeometry" + i.ToString();

                        /* Ensure that all of the surface coordinates are listed in a counterclockwise order
                         * This is a requirement of gbXML Polyloop definitions */

                        BHG.Polyline pline = new BHG.Polyline() { ControlPoints = bHoMPanels[i].PolyCurve.ControlPoints() }; //TODO: Change to ToPolyline method
                        BHG.Polyline srfBound = new BHG.Polyline();

                        // if (!BH.Engine.Geometry.Query.IsClockwise(pline, bHoMSpace.Centre()))
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
                        xmlPanel.AdjacentSpaceId = BH.Engine.XML.Query.AdjacentSpace(bHoMBuildingElement[i], spaces).ToArray();

                        //Remove duplicate surfaces
                        BHE.BuildingElement elementKeep = BH.Engine.XML.Query.ElementToKeep(bHoMBuildingElement[i], srfBound, spaces);
                        if (elementKeep != null)
                        {
                            //Create openings
                            if (bHoMPanels[i].Openings.Count > 0)
                                xmlPanel.Opening = Serialize(bHoMPanels[i].Openings, ref openingIndex, buildingElementsList, bHoMSpace, gbx).ToArray();
                            gbx.Campus.Surface.Add(xmlPanel);
                            panelindex++;
                        }
                    }
                    panelindex = panelindex - 1;
                    panelindex++;
                }

                // Generate gbXMLSpaces
                if (spaces != null)
                    Serialize(bHoMSpace, gbx);

            }
        }

        /***************************************************/

        public static void Serialize(List<BH.oM.Architecture.Elements.Level> levels, List<BHE.Space> bHoMSpaces, BH.oM.XML.gbXML gbx)
        {
            //Levels unique by name in all spaces:
            List<BH.oM.XML.BuildingStorey> xmlLevels = new List<BuildingStorey>();
            foreach (BH.oM.Architecture.Elements.Level level in levels)
            {
                BuildingStorey storey = BH.Engine.XML.Convert.ToGbXML(level);
                storey.PlanarGeoemtry.PolyLoop = BH.Engine.XML.Convert.ToGbXML(BH.Engine.XML.Query.StoreyGeometry(level, bHoMSpaces));
                xmlLevels.Add(storey);
            }

            gbx.Campus.Building[0].BuildingStorey = xmlLevels.ToArray();

        }

        /***************************************************/

        public static List<Opening> Serialize(List<BHE.BuildingElementOpening> bHoMOpenings, ref int openingIndex, List<BHE.BuildingElement> buildingElementsList, BHE.Space space, BH.oM.XML.gbXML gbx)
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

                if (opening.CustomData.ContainsKey("Revit_elementId"))
                {
                    string elementID = (opening.CustomData["Revit_elementId"]).ToString();
                    BHE.BuildingElement buildingElement = buildingElementsList.Find(x => x != null && x.CustomData.ContainsKey("Revit_elementId") && x.CustomData["Revit_elementId"].ToString() == elementID);
                    if (buildingElement != null && buildingElement.BuildingElementProperties.CustomData.ContainsKey("Family Name"))
                    {
                        familyName = buildingElement.BuildingElementProperties.CustomData["Family Name"].ToString();
                        typeName = buildingElement.BuildingElementProperties.Name;
                    }
                    gbXMLOpening.CADObjectId = BH.Engine.XML.Query.CadObjectId(opening, buildingElementsList);
                    gbXMLOpening.openingType = BH.Engine.XML.Convert.ToGbXMLType(buildingElement);

                    if (familyName == "System Panel") //No SAM_BuildingElementType for this one atm
                        gbXMLOpening.openingType = "FixedWindow";
                }


                gbXMLOpening.id = "opening-" + openingIndex;
                gbXMLOpening.Name = "opening-" + openingIndex;
                xmlOpenings.Add(gbXMLOpening);
                openingIndex++;
            }

            return xmlOpenings;
        }

        /***************************************************/

        public static void Serialize(BHE.Space bHoMSpace, BH.oM.XML.gbXML gbx)
        {
            List<BH.oM.XML.Space> xspaces = new List<Space>();
            BH.oM.XML.Space xspace = BH.Engine.XML.Convert.ToGbXML(bHoMSpace);

            //Closed Shell
            xspace.ShellGeometry.ClosedShell.PolyLoop = BH.Engine.XML.Query.ClosedShellGeometry(bHoMSpace).ToArray();

            //Space Boundaries
            xspace.SpaceBoundary = BH.Engine.XML.Query.SpaceBoundaries(bHoMSpace);

            //Planar Geometry
            if (BH.Engine.XML.Query.FloorGeometry(bHoMSpace) != null)
                xspace.PlanarGeoemtry.PolyLoop = BH.Engine.XML.Convert.ToGbXML(BH.Engine.XML.Query.FloorGeometry(bHoMSpace));

            gbx.Campus.Building[0].Space.Add(xspace);
        }

        /***************************************************/

        public static void Serialize(BHE.BuildingElementPanel bHoMPanel, BH.oM.XML.gbXML gbx)
        {
            throw new NotImplementedException();
        }

        /***************************************************/
    }

}
