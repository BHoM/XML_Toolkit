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

                gbx.Campus.Location = BH.Engine.XML.Convert.ToGbXML(building); //Location data
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
                    if (element.BuildingElementProperties.BuildingElementType != BHE.BuildingElementType.Window) //Shade
                        if (element.BuildingElementProperties.BuildingElementType != BHE.BuildingElementType.Door) //Shade
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
                xmlRectangularGeom.CartesianPoint = BH.Engine.XML.Convert.ToGbXML(BH.Engine.Geometry.Query.Centre(pline));
                plGeo.PolyLoop = BH.Engine.XML.Convert.ToGbXML(pline);
                xmlPanel.PlanarGeometry = plGeo;
                xmlPanel.RectangularGeometry = xmlRectangularGeom;

                gbx.Campus.Surface.Add(xmlPanel);
                panelIndex++;
            }
        }

        /***************************************************/

        public static void SerializeCollection(IEnumerable<BHE.Space> bhomObjects, BH.oM.XML.gbXML gbx, BHE.Building building = null)
        {
            //Levels unique by name in all spaces. We can access this info from the building, but we need it if the input is space (without building):
            List<BH.oM.Architecture.Elements.Level> levels = bhomObjects.Select(x => x.Level).Distinct(new BH.Engine.Base.Objects.BHoMObjectNameComparer()).Select(x => x as BH.oM.Architecture.Elements.Level).ToList();
            Serialize(levels, bhomObjects.ToList(), gbx);

            List<BHE.BuildingElement> buildingElementsList;

            if (building == null)
            {
                buildingElementsList = bhomObjects.SelectMany(x => x.BuildingElements).Cast<BHE.BuildingElement>().ToList();
            }
            else
            {
                buildingElementsList = building.BuildingElements;
            }


            //Spaces
            double panelindex = 0;
            double openingIndex = 0;
            foreach (BHE.Space bHoMSpace in bhomObjects)
            {
                List<BHE.BuildingElementPanel> bHoMPanels = new List<BHE.BuildingElementPanel>();
                List<BHE.BuildingElement> bHoMBuildingElement = new List<BHE.BuildingElement>();
                List<BHP.BuildingElementProperties> bHoMBuildingElementProperties = new List<BHP.BuildingElementProperties>();

                bHoMPanels.AddRange(bHoMSpace.BuildingElements.Select(x => x.BuildingElementGeometry as BHE.BuildingElementPanel));
                bHoMBuildingElement.AddRange(bHoMSpace.BuildingElements);
                bHoMBuildingElementProperties.AddRange(bHoMSpace.BuildingElements.Select(x => x.BuildingElementProperties as BHP.BuildingElementProperties));

                BHG.Point spaceCentrePoint = BH.Engine.Environment.Query.Centre(bHoMSpace as BHE.Space);

                List<BHE.Space> spaces = bhomObjects.Where(x => x is BHE.Space).Select(x => x as BHE.Space).ToList();


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
                        //TODO: Update to a ToPolyline() method once an appropriate version has been implemented

                        BHG.Polyline pline = new BHG.Polyline() { ControlPoints = bHoMPanels[i].PolyCurve.ControlPoints() }; //TODO: Change to ToPolyline method
                        BHG.Polyline srfBound = new BHG.Polyline();

                        if (BH.Engine.Geometry.Query.IsClockwise(pline, spaceCentrePoint))
                        {
                            plGeo.PolyLoop = BH.Engine.XML.Convert.ToGbXML(pline.Flip());
                            //xmlRectangularGeom.Polyloop = BH.Engine.XML.Convert.ToGbXML(pline.Flip()); //TODO: for bounding curve
                            srfBound = pline.Flip();
                        }
                        else
                        {
                            plGeo.PolyLoop = BH.Engine.XML.Convert.ToGbXML(pline);
                            //xmlRectangularGeom.Polyloop = BH.Engine.XML.Convert.ToGbXML(pline); //TODO: for bounding curve
                            srfBound = pline;
                        }

                        xmlRectangularGeom.CartesianPoint = BH.Engine.XML.Convert.ToGbXML(BH.Engine.Geometry.Query.Centre(pline));


                        xmlPanel.PlanarGeometry = plGeo;
                        xmlPanel.RectangularGeometry = xmlRectangularGeom;


                        //// Create openings
                        //if (bHoMPanels[i].Openings.Count > 0)
                        //    xmlPanel.Opening = Serialize(bHoMPanels[i].Openings, ref openingIndex, buildingElementsList, gbx).ToArray();



                        // Adjacent Spaces
                        /***************************************************/

                        List<AdjacentSpaceId> adspace = new List<AdjacentSpaceId>();
                        // We don't know anything about adjacency if the input is a list of spaces. Atm this does only work when the input is Building. 
                        foreach (Guid adjSpace in bHoMBuildingElement[i].AdjacentSpaces)
                        {
                            AdjacentSpaceId adjId = new AdjacentSpaceId();
                            if (spaces.Select(x => x.BHoM_Guid).Contains(adjSpace))
                            {
                                adjId.spaceIdRef = "Space-" + spaces.Find(x => x.BHoM_Guid == adjSpace).Name;
                                adspace.Add(adjId);
                            }
                        }

                        xmlPanel.AdjacentSpaceId = adspace.ToArray();

                        //Check if the surface normal is pointing away from the first AdjSpace. Add if it does.
                        if (bHoMBuildingElement[i].AdjacentSpaces.Count > 0)
                        {
                            Guid firstGuid = bHoMBuildingElement[i].AdjacentSpaces.First();
                            BHE.Space firstSpace = spaces.Find(x => x.BHoM_Guid == firstGuid);

                            if (firstSpace == null)
                            {
                                // Create openings
                                if (bHoMPanels[i].Openings.Count > 0)
                                    xmlPanel.Opening = Serialize(bHoMPanels[i].Openings, ref openingIndex, buildingElementsList, gbx).ToArray();
                                gbx.Campus.Surface.Add(xmlPanel);
                                panelindex++;
                            }
                            else
                            {
                                if (!BH.Engine.Geometry.Query.IsClockwise(srfBound, BH.Engine.Environment.Query.Centre(firstSpace)))
                                {
                                    // Create openings
                                    if (bHoMPanels[i].Openings.Count > 0)
                                        xmlPanel.Opening = Serialize(bHoMPanels[i].Openings, ref openingIndex, buildingElementsList, gbx).ToArray();
                                    gbx.Campus.Surface.Add(xmlPanel);
                                    panelindex++;
                                }
                            }
                        }

                        else  //Shade elements
                        {
                            // Create openings
                            if (bHoMPanels[i].Openings.Count > 0)
                                xmlPanel.Opening = Serialize(bHoMPanels[i].Openings, ref openingIndex, buildingElementsList, gbx).ToArray();
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

        public static List<Opening> Serialize(List<BHE.BuildingElementOpening> bHoMOpenings, ref double openingIndex, List<BHE.BuildingElement> buildingElementsList, BH.oM.XML.gbXML gbx)
        {
            List<Opening> xmlOpenings = new List<Opening>();

            foreach (BHE.BuildingElementOpening opening in bHoMOpenings)
            {
                Opening gbXMLOpening = BH.Engine.XML.Convert.ToGbXML(opening);
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
                    //gbXMLOpening.CADObjectId = familyName + ": " + typeName + " [" + elementID + "]";
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
            List<BH.oM.XML.Polyloop> ploops = new List<Polyloop>();
            BHG.Point spaceCentrePoint = BH.Engine.Environment.Query.Centre(bHoMSpace);
            List<BHE.BuildingElement> closedShellElements = new List<BHE.BuildingElement>();

            //Just works for polycurves at the moment. ToDo: fix this for all type of curves
            //IEnumerable<BHG.PolyCurve> bePanel = bHoMSpace.BuildingElements.Select(x => x.BuildingElementGeometry.ICurve() as BHG.PolyCurve);

            //Test
            foreach (BHE.BuildingElement element in bHoMSpace.BuildingElements)
            {
                if (element.BuildingElementProperties != null)
                    if (element.BuildingElementProperties.BuildingElementType != BHE.BuildingElementType.Window) //Shade
                        if (element.BuildingElementProperties.BuildingElementType != BHE.BuildingElementType.Door) //Shade
                                    closedShellElements.Add(element);
            }

            IEnumerable<BHG.PolyCurve> bePanel = closedShellElements.Select(x => x.BuildingElementGeometry.ICurve() as BHG.PolyCurve);
            //EndTest

            //Closed shell. The closed shell is defined by walls, floors and roofs (windows and soors are not included)
            foreach (BHG.PolyCurve pCrv in bePanel)
            {
                /* Ensure that all of the surface coordinates are listed in a counterclockwise order
                * This is a requirement of gbXML Polyloop definitions */
                BHG.Polyline pline = new BHG.Polyline() { ControlPoints = pCrv.ControlPoints() }; //TODO: Change to ToPolyline method

                if (BH.Engine.Geometry.Query.IsClockwise(pline, spaceCentrePoint))
                    ploops.Add(BH.Engine.XML.Convert.ToGbXML(pline.Flip()));
                else
                    ploops.Add(BH.Engine.XML.Convert.ToGbXML(pline));
            }
            xspace.ShellGeometry.ClosedShell.PolyLoop = ploops.ToArray();


            //Space Boundaries
            SpaceBoundary[] bounadry = new SpaceBoundary[ploops.Count()];

            for (int i = 0; i < ploops.Count(); i++)
            {
                PlanarGeometry planarGeom = new PlanarGeometry();
                planarGeom.PolyLoop = ploops[i];
                SpaceBoundary bound = new SpaceBoundary { PlanarGeometry = planarGeom };
                bounadry[i] = bound;

                //TODO: create surface and get its ID

            }
            xspace.SpaceBoundary = bounadry;

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
