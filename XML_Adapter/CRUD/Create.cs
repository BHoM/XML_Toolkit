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

namespace XML_Adapter.gbXML
{
    public class gbXMLSerializer
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static void Serialize<T>(IEnumerable<T> bhomObjects, BH.oM.XML.gbXML gbx) where T: IObject
        {
            SerializeCollection(bhomObjects as dynamic, gbx);
            
            // Document History                          
            DocumentHistory DocumentHistory = new DocumentHistory();
            DocumentHistory.CreatedBy.date = DateTime.Now.ToString();
            gbx.DocumentHistory = DocumentHistory;
        }

        /***************************************************/

        public static void SerializeCollection(IEnumerable<BHE.Space> bhomObjects, BH.oM.XML.gbXML gbx)
        {
            //Levels unique by name in all spaces:
            List<BH.oM.Architecture.Elements.Level> levels = bhomObjects.Select(x => x.Level).Distinct(new BH.Engine.Base.Objects.BHoMObjectNameComparer()).Select(x => x as BH.oM.Architecture.Elements.Level).ToList();
            SerializeStoreys(levels, gbx);

            //Spaces
            double panelindex = 0;
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
                        xmlPanel.Name = type;
                        xmlPanel.surfaceType = type;
                        string revitElementID = "";
                        xmlPanel.surfaceType = BH.Engine.XML.Convert.ToGbXMLSurfaceType(bHoMBuildingElement[i]);

                        if (bHoMBuildingElement[i].BuildingElementProperties != null)
                        {
                            xmlPanel.Name = bHoMBuildingElement[i].BuildingElementProperties.Name;
                            xmlPanel.CADobjectId = bHoMBuildingElement[i].BuildingElementProperties.Name;
                            if (bHoMBuildingElement[i].BuildingElementProperties.CustomData.ContainsKey("Revit_elementId"))
                                revitElementID = bHoMBuildingElement[i].BuildingElementProperties.CustomData["Revit_elementId"].ToString();
                        }

                        xmlPanel.id = "Panel-" + panelindex.ToString();
                        xmlPanel.exposedToSun = XML_Engine.Query.ExposedToSun(xmlPanel.surfaceType).ToString();

                        RectangularGeometry xmlRectangularGeom = BH.Engine.XML.Convert.ToGbXML(bHoMPanels[i]);
                        PlanarGeometry plGeo = new PlanarGeometry();
                        plGeo.id = "PlanarGeometry" + i.ToString();

                        /* Ensure that all of the surface coordinates are listed in a counterclockwise order
                         * This is a requirement of gbXML Polyloop definitions */
                        //TODO: Update to a ToPolyline() method once an appropriate version has been implemented

                        BHG.Polyline pline = new BHG.Polyline() { ControlPoints = bHoMPanels[i].PolyCurve.ControlPoints() }; //TODO: Change to ToPolyline method

                        if (BH.Engine.Geometry.Query.IsClockwise(pline, spaceCentrePoint))
                        {
                            plGeo.PolyLoop = BH.Engine.XML.Convert.ToGbXML(pline.Flip());
                            xmlRectangularGeom.Polyloop = BH.Engine.XML.Convert.ToGbXML(pline.Flip()); //TODO: for bounding curve
                        }
                        else
                        {
                            plGeo.PolyLoop = BH.Engine.XML.Convert.ToGbXML(pline);
                            xmlRectangularGeom.Polyloop = BH.Engine.XML.Convert.ToGbXML(pline); //TODO: for bounding curve
                        }

                        xmlRectangularGeom.CartesianPoint = BH.Engine.XML.Convert.ToGbXML(BH.Engine.Geometry.Query.Centre(pline));


                        xmlPanel.PlanarGeometry = plGeo;
                        xmlPanel.RectangularGeometry = xmlRectangularGeom;


                        // Create openings
                        if (bHoMPanels[i].Openings.Count > 0)
                        {
                            List<Opening> xmlOpenings = new List<Opening>();

                            foreach (BHE.BuildingElementOpening opening in bHoMPanels[i].Openings)
                            {
                                Opening gbXMLOpening = BH.Engine.XML.Convert.ToGbXML(opening);
                                xmlOpenings.Add(gbXMLOpening);
                            }
                            xmlPanel.Opening = xmlOpenings.ToArray();
                        }


                        // Adjacent Spaces
                        /***************************************************/
                        try // We don't know anything about adjacency if the input is a list of spaces. Atm this does only work when the input is Building. 
                        {
                            List<AdjacentSpaceId> adspace = new List<AdjacentSpaceId>();
                            foreach (Guid adjSpace in bHoMBuildingElement[i].AdjacentSpaces)
                            {
                                AdjacentSpaceId adjId = new AdjacentSpaceId();
                                adjId.spaceIdRef = "Space-" + spaces.Find(x => x.BHoM_Guid == adjSpace).Name;
                                adspace.Add(adjId);
                            }

                            xmlPanel.AdjacentSpaceId = adspace.ToArray();
                            gbx.Campus.Surface.Add(xmlPanel);

                        }
                        catch (Exception e)
                        {
                            string message = "Couldn't find adjacent spaces. " + e.Message;
                            throw new Exception(message);
                        }

                        panelindex++;
                    }

                    panelindex = panelindex - 1;
                    panelindex++;
                }


                // Generate gbXMLSpaces
                /***************************************************/
                if (spaces != null)
                {
                    List<BH.oM.XML.Space> xspaces = new List<Space>();
                    BH.oM.XML.Space xspace = BH.Engine.XML.Convert.ToGbXML(bHoMSpace);
                    List<BH.oM.XML.Polyloop> ploops = new List<Polyloop>();

                    //Just works for polycurves at the moment. ToDo: fix this for all type of curves
                    IEnumerable<BHG.PolyCurve> bePanel = bHoMSpace.BuildingElements.Select(x => x.BuildingElementGeometry.ICurve() as BHG.PolyCurve);

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

                    gbx.Campus.Building[0].Space.Add(xspace);
                }
            }
        }

        /***************************************************/

        public static void SerializeStoreys(List<BH.oM.Architecture.Elements.Level> levels, BH.oM.XML.gbXML gbx)
        {
            //Levels unique by name in all spaces:
            List<BH.oM.XML.BuildingStorey> xmlLevels = new List<BuildingStorey>();
            foreach (BH.oM.Architecture.Elements.Level level in levels)
            {
                BuildingStorey storey = BH.Engine.XML.Convert.ToGbXML(level);
                xmlLevels.Add(storey);
            }

            gbx.Campus.Building[0].BuildingStorey = xmlLevels.ToArray();

        }

        /***************************************************/

    }

}
